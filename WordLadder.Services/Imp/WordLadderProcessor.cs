using Microsoft.Extensions.Logging;
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
        
        private ProcessingResult _processingResult;

        public WordLadderProcessor(
            ILogger<WordLadderProcessor> logger,
            IWordListRepository repository
            )
        {
            _logger = logger;
            _repository = repository;
            
            _workingMatchList = new List<string>();
        }

        private enum ResultEval
        {
            IsFinalOK,
            IsFinalNotOK,
            IsValidPartial,
            Continue
        }

        Task<ProcessingResult> IWordLadderProcessor.ProcessAsync(JobPayload payload)
        {
            var _startWord = payload.StartWord;
            var _wordSize = _startWord.Length;
            SetInitialResult(payload);

            List<string> resultList = new List<string>() { _startWord };

            //get word list
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
            keepProcessing = cycleResult == ResultEval.Continue || cycleResult == ResultEval.IsValidPartial;

            while (keepProcessing)
            {
                //Console.WriteLine($"Nível: {counter}");
                foreach (var mr in pointer.MatchesList)
                {
                    cycleResult = MatchingCycleSearchable(_wordSize, mr.SourceWord, mr, payload);
                    // _nextMatches = RemoveDuplicates(_nextMatches);
                    //PrintNode(mr);
                    keepProcessing = cycleResult == ResultEval.Continue || cycleResult == ResultEval.IsValidPartial;
                    if (!keepProcessing) break;
                }

                //_currentMatches = RemoveDuplicates(_nextMatches);
                pointer = NextNodeBreadFirst(pointer);
                keepProcessing &= pointer != null;
                counter++;
            }
            //
            if (_processingResult.End == null) 
            {
                _processingResult.End = DateTimeOffset.UtcNow;
                
            }
            
            //return Task.FromResult(resultList);
            return Task.FromResult(_processingResult);
        }

        private void SetInitialResult(JobPayload payload)
        {
            _processingResult = new ProcessingResult();
            _processingResult.Payload = payload;
            _processingResult.Start = DateTimeOffset.UtcNow;
            _processingResult.WasSuccefull = false;
            _processingResult.ResultMessage = "Unable to find a path bettween the start and end word";
        }

        private ResultEval MatchingCycleSearchable(int _wordSize, string _startWord, MatchResult mResult, JobPayload payload)
        {

            var mutations = GenerateMutations(_wordSize, _startWord);
            var regexSet = GenerateRegex(_wordSize, mutations);
            GenerateMatchesForDeepFirst(regexSet, mResult);

            var isfinal = IsFinal(mResult.MatchesList.Select(e => e.SourceWord).ToList(), payload.EndWord, payload);
            if (isfinal == ResultEval.IsFinalNotOK || isfinal == ResultEval.IsFinalOK)
            {
                //File.WriteAllText(payload.ResultPublicationPath, PrintPath(mResult, payload.EndWord));
                //PrintPath(mResult);
                _processingResult.End = DateTimeOffset.UtcNow;
                if (isfinal == ResultEval.IsFinalOK)
                {
                    _processingResult.Results.Add(GetTreeLine(mResult, payload.EndWord));
                    _processingResult.WasSuccefull = true;
                }
            }

            return isfinal;
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

        private List<string> GenerateMatchesForDeepFirst(Regex[] regexset, MatchResult mResult)
        {
            List<string> matches = new List<string>();

            regexset.ToList().ForEach((r) => matches.AddRange(_workingWordList.Where(e => r.IsMatch(e)
                                && e != mResult.SourceWord
                                ).Select(e => e).ToList()));

            matches = matches.Except(this._workingMatchList).ToList();
            this._workingMatchList.AddRange(matches);

            matches.ForEach(e => mResult.MatchesList.AddLast(new MatchResult(e, mResult)));

            return matches;
        }

        private MatchResult NextNodeBreadFirst(MatchResult current)
        {
            if (current.ParentMatch != null)
            {
                var n = current.ParentMatch.MatchesList.GetNextSibling(current);
                if (n != null) return n;
            }
            if (current.MatchesList.Count > 0) return current.MatchesList.First<MatchResult>();

            return null;
        }

        private ResultEval IsFinal(List<string> resultsList, string endWord, JobPayload payload)
        {
            ResultEval resultEval = ResultEval.Continue;

            switch (payload.TypeOfResult)
            {
                case JobPayload.ResultType.FIRST: { 
                            if (resultsList == null || resultsList.Contains(endWord)) resultEval = ResultEval.IsFinalOK;
                               
                            }; break;
                case JobPayload.ResultType.ALL: { }; break;
                case JobPayload.ResultType.TOP_N: { }; break;
            }
            return resultEval;
        }
    }
}
