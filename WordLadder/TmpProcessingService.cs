using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordLadder.Models;
using System.Text.RegularExpressions;
using System.IO;

namespace WordLadder
{
    public class TmpProcessingService
    {
        //private List<string> _wordList;
        //private string _startWord;
        //private string _finalWord;
        private List<string> _workingWordList;
        private List<string> _workingMatchList;
       // private JobPayload _payload;

       //private MatchResult rootSearchTree;

        private List<string> WorkingWordList { get { return _workingWordList == null ? _workingWordList = new List<string>() : _workingWordList; } }

        public TmpProcessingService()
        {
            //_wordList = wordList;
            //_startWord = payload.StartWord;
            //_finalWord = payload.EndWord;
            //_payload = payload;
            //rootSearchTree = new MatchResult(_startWord, null);
        }

        //public List<string> Process(List<string> wordList, JobPayload payload)
        //{
        //    var _startWord = payload.StartWord;

        //    List<string> resultList = new List<string>() { _startWord };
        //    var _wordSize = _startWord.Length;

        //    _workingWordList = wordList.Where(e => e.Length == _wordSize).Select(e => e).ToList();

        //    _workingWordList.Remove(_startWord);

        //    List<string> _currentMatches = new List<string>() { _startWord };
        //    List<string> _nextMatches = new List<string>();
        //    bool keepProcessing = true;

        //    while (keepProcessing)
        //    {
        //        foreach (var s in _currentMatches)
        //        {
        //            keepProcessing &= !MatchingCycle(_wordSize, s, _nextMatches, payload);
        //            _nextMatches = RemoveDuplicates(_nextMatches);
        //        }
        //        _currentMatches = RemoveDuplicates(_nextMatches);
        //    }

        //    return resultList;
        //}

 

        public List<string> ProcessTwo(List<string> wordList, JobPayloadCommand payload)
        {
            var _startWord = payload.StartWord;
            var _wordSize = _startWord.Length;

            List<string> resultList = new List<string>() { _startWord };

            _workingMatchList = new List<string>();
            _workingWordList = wordList.Where(e => e.Length == _wordSize).Select(e => e).ToList();
            _workingWordList.Remove(_startWord);

            bool keepProcessing = true;
            MatchResult mResult = new MatchResult(_startWord, null);
            MatchResult pointer = null;
            keepProcessing &= !MatchingCycleSearchable(_wordSize, mResult.SourceWord, mResult, payload);
            pointer = mResult;
            int counter = 0;
           

            while (keepProcessing)
            {
                Console.WriteLine($"Nível: {counter}");
                foreach (var mr in pointer.MatchesList)
                {
                    keepProcessing &= !MatchingCycleSearchable(_wordSize, mr.SourceWord, mr,payload);
                    // _nextMatches = RemoveDuplicates(_nextMatches);
                    //PrintNode(mr);
                    if (!keepProcessing) break;
                }

                //_currentMatches = RemoveDuplicates(_nextMatches);
                pointer = NextNodeBreadFirst(pointer);
                keepProcessing &= pointer != null;
                counter++;
            }


            return resultList;
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

        //private bool MatchingCycle(int _wordSize, string _startWord, List<string> _matchedList,  JobPayload payload)
        //{

        //    var mutations = GenerateMutations(_wordSize, _startWord);
        //    var regexSet = GenerateRegex(_wordSize, mutations);
        //    _matchedList.AddRange(GenerateMatchesForBreathFirst(regexSet));

        //    return IsFinal(_matchedList, payload.EndWord);
        //}

        private bool MatchingCycleSearchable(int _wordSize, string _startWord, MatchResult mResult, JobPayloadCommand payload)
        {

            var mutations = GenerateMutations(_wordSize, _startWord);
            var regexSet = GenerateRegex(_wordSize, mutations);
            GenerateMatchesForDeepFirst(regexSet, mResult);

            var isfinal = IsFinal(mResult.MatchesList.Select(e => e.SourceWord).ToList(), payload.EndWord);
            if (isfinal)
            {
                File.WriteAllText(payload.ResultPublicationPath, PrintPath(mResult, payload.EndWord));
                //PrintPath(mResult);
            }

            return isfinal;
        }

        private string PrintPath(MatchResult mResult, string finalWord)
        {

            //Console.WriteLine(_startWord+"=>");
            var sb = new StringBuilder();
            var l = mResult.AncestorAndSelfList();
            l.Reverse();
            l.ForEach(e => sb.AppendLine(" => "+ e));
            sb.AppendLine("<="+finalWord);

            var ret = sb.ToString();
            Console.WriteLine(ret);

            return ret;
        }

        private void PrintNode(MatchResult mResult) 
        {
            StringBuilder sb = new StringBuilder();
            var _names = mResult.MatchesList.Select(e => e.SourceWord).ToList() ;
            foreach (var s in _names) sb.Append(s + " ");

            Console.WriteLine($"{mResult.SourceWord}=>{sb.ToString()}");
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

        private List<string> GenerateMatchesForBreathFirst(Regex[] regexset)
        {
            List<string> matches = new List<string>();

            regexset.ToList().ForEach((r) => matches.AddRange(_workingWordList.Where(e => r.IsMatch(e)).Select(e => e).ToList()));

            return matches;
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

        private bool IsFinal(List<string> resultsList, string endWord)
        {
            return resultsList == null || resultsList.Contains(endWord);
        }

        private List<string> RemoveDuplicates(List<string> _list)
        {
            List<string> result = new List<string>();
            foreach (var s in _list)
            {
                if (!result.Contains(s)) result.Add(s);
            }

            return result;
        }
    }
}
