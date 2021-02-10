using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordLadder.Models;
using WordLadder.Services.Abstract;

namespace WordLadder.Services.Imp
{
    public class WordLadderProcessor : IWordLadderProcessor
    {
        private ILogger<WordLadderProcessor> _logger;
        private IWordListRepository _repository;
        private List<string> _workingWordList;
        private List<string> _workingMatchList;
        private WordLadderOptions _wordLadderOptions;
        private ProcessingResult _processingResult;

        public WordLadderProcessor(
            ILogger<WordLadderProcessor> logger,
            IWordListRepository repository, IOptions<WordLadderOptions> options
            )
        {
            _logger = logger;
            _repository = repository;
            _wordLadderOptions = options.Value;
            _workingMatchList = new List<string>();
        }

        private enum ResultEval
        {
            IsFinalOK,
            IsFinalNotOK,
            Continue
        }

        Task<ProcessingResult> IWordLadderProcessor.ProcessAsync(JobPayloadCommand payload)
        {
            var _startWord = payload.StartWord;
            var _wordSize = _startWord.Length;
            SetInitialResult(payload);

            List<string> resultList = new List<string>() { _startWord };

       
            _workingMatchList.Clear();
            _workingWordList = _repository.GetFiltered((_word) => _word.Length == _wordSize);
            _workingWordList.Remove(_startWord);

            bool keepProcessing = true;
            ResultEval cycleResult;
            MatchResult mResult = new MatchResult(_startWord, null);
            MatchResult pointer = null;
            cycleResult = MatchingCycleSearchable(_wordSize, mResult.SourceWord, mResult, payload);
            pointer = mResult;
            int counter = 0;
            keepProcessing = cycleResult == ResultEval.Continue;

            while (keepProcessing)
            {
                //
                foreach (var mr in pointer.MatchesList)
                {
                    cycleResult = MatchingCycleSearchable(_wordSize, mr.SourceWord, mr, payload);

                    keepProcessing = cycleResult == ResultEval.Continue;
                    if (!keepProcessing) break;
                }

                pointer = payload.TypeOfSearch == JobPayloadCommand.SearchType.BREATH_FIRST ? NextNodeBreadFirst(pointer) : NextNodeDeepFirst(pointer);
                keepProcessing &= pointer != null;
                counter++;
                
            }
            //
            

            PrepareResult(payload);

            return Task.FromResult(_processingResult);
        }

        private void PrepareResult(JobPayloadCommand payload)
        {
            if (_processingResult.Results.Count > 0)
            {
                _processingResult.WasSuccefull = true;
                _processingResult.ResultMessage = "Found a sequence between the start and end words.";
            }
            if (_processingResult.End == null)
            {
                _processingResult.End = DateTimeOffset.UtcNow;
            }
        }

        private void SetInitialResult(JobPayloadCommand payload)
        {
            _processingResult = new ProcessingResult();
            _processingResult.Payload = payload;
            _processingResult.Start = DateTimeOffset.UtcNow;
            _processingResult.WasSuccefull = false;
            _processingResult.ResultMessage = "Unable to find a path bettween the start and end word";
        }

        private ResultEval MatchingCycleSearchable(int _wordSize, string _startWord, MatchResult mResult, JobPayloadCommand payload)
        {

            var mutations = GenerateMutations(_wordSize, _startWord);
            var regexSet = GenerateRegex(_wordSize, mutations);
            GenerateMatches(regexSet, mResult);

            var isfinal = IsFinal(mResult.MatchesList.Select(e => e.SourceWord).ToList(), payload.EndWord, payload);
            if (isfinal == ResultEval.IsFinalNotOK || isfinal == ResultEval.IsFinalOK)
            {
                _processingResult.Results.Add(GetTreeLine(mResult, payload.EndWord));
                
                if (isfinal == ResultEval.IsFinalOK)
                {
                    _processingResult.End = DateTimeOffset.UtcNow;
                    _processingResult.WasSuccefull = true;
                }
            }

            return isfinal;
        }

        private ResultEval IsFinal(List<string> resultsList, string endWord, JobPayloadCommand payload)
        {
            ResultEval resultEval = ResultEval.Continue;


            if (resultsList.Contains(endWord))
            {
                resultEval = ResultEval.IsFinalOK;
            }

             return resultEval;
        }

        private List<string> GetTreeLine(MatchResult mResult, string finalWord)
        {
            var l = mResult.AncestorAndSelfList();
            l.Reverse();

            return l;
        }

        private WordTokens[] GenerateMutations(int wordLength, string word)
        {
            WordTokens[] mutations = new WordTokens[wordLength];

            for (int i = 0; i < wordLength; i++)
            {
                var left = new String(word.Take(i).ToArray());
                var right = new String(word.Skip(i + 1).ToArray());

                mutations[i] = new WordTokens() { LeftToken = left, RightToken = right };
            }

            return mutations;
        }

        private Regex[] GenerateRegex(int wordLength, WordTokens[] tokens)
        {
            string oneCharToken = @"\D{1}";
            Regex[] regexp = new Regex[wordLength];

            for (int i = 0; i < wordLength; i++)
            {
                regexp[i] = new Regex(string.Concat(tokens[i].LeftToken, oneCharToken, tokens[i].RightToken), RegexOptions.IgnoreCase);
            }

            return regexp;
        }

        private List<string> GenerateMatches(Regex[] regexset, MatchResult mResult)
        {
            List<string> matches = new List<string>();

            regexset.ToList().ForEach((r) => matches.AddRange(_workingWordList.Where(e => r.IsMatch(e)
                                && e != mResult.SourceWord
                                ).Select(e => e).ToList()));

            matches = matches.Except(this._workingMatchList).ToList();
            //var matches2 = matches.Except(this._workingMatchList).ToList();
            this._workingMatchList.AddRange(matches);

            matches.ForEach(e => mResult.MatchesList.AddLast(new MatchResult(e, mResult)));

            return matches;
        }

        private MatchResult NextNodeBreadFirst(MatchResult current)
        {
            // if it is not the start word
            if (current.ParentMatch != null)
            {
                var n = current.ParentMatch.MatchesList.GetNextSibling(current);
                if (n != null) return n;
            }
            if (current.MatchesList.Count > 0) return current.MatchesList.First<MatchResult>();

            return null;
        }


        private MatchResult NextNodeDeepFirst(MatchResult current)
        {
            // if it is not the start word
            if (current.ParentMatch != null)
            {
                if (current.MatchesList.Count > 0)
                {
                    //fetch first descendant
                    return current.MatchesList.First<MatchResult>();
                }
                else
                {
                    var _current = current;
                    current.ParentMatch.MatchesList.Remove(current);
                    var tmp = current.ParentMatch.MatchesList.GetNextSibling(_current);

                    while (tmp == null && _current.ParentMatch.ParentMatch != null)
                    {
                        tmp = _current.ParentMatch.ParentMatch.MatchesList.GetNextSibling(_current.ParentMatch);
                        if (tmp == null) tmp = _current.ParentMatch.ParentMatch;
                    }

                    return tmp;
                }


            }
            if (current.MatchesList.Count > 0) return current.MatchesList.First<MatchResult>();

            return null;
        }


    }
}
