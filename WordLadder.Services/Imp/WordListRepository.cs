using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WordLadder.Models;
using WordLadder.Models.Exceptions;
using WordLadder.Services.Abstract;

namespace WordLadder.Services.Imp
{
    /// <summary>
    /// Word list repository sourced by the file System
    /// </summary>
    public class WordListRepository : IWordListRepository
    {
        private List<string> _baseWordList;
        private string[] _args;
        private WordLadderOptions _wordLadderOptions;
        ILogger<WordListRepository> _logger;

        public List<string> WordList =>  _baseWordList;

        public WordListRepository(ILogger<WordListRepository> logger, IOptions<WordLadderOptions> options)
        {
            _baseWordList = new List<string>();
            _args = GetCommandLineArgs();
            _wordLadderOptions = options.Value;
            _logger = logger;
        }

        public virtual string[] GetCommandLineArgs() => Environment.GetCommandLineArgs().Skip(1).ToArray();

        public List<string> All()
        {
            
            if (_baseWordList.Count == 0) Load();

            return this._baseWordList;
        }

        public void Clear()
        {
            _baseWordList.Clear();
        }

        public List<string> GetFiltered(Func<string, bool> filterPredicate)
        {
            if (WordList.Count == 0) Load();

            return this._baseWordList.Where(filterPredicate).Select(e => e).ToList();
        }

        private void Load()
        {
            try
            {
                var payload = CommonHelpers.LoadJob(_args, _wordLadderOptions.TypeOfSearch.Value);
                string[] _lines;
                if (!string.IsNullOrEmpty(payload.SourceFilePath))
                {
                    _lines = File.ReadAllLines(payload.SourceFilePath);
                }
                else if (!string.IsNullOrEmpty(_wordLadderOptions.LocalWordDictionaryFilePath))
                {
                    _lines = File.ReadAllLines(_wordLadderOptions.LocalWordDictionaryFilePath);
                }
                else
                {
                    _logger.LogError("word Source not found. Unable to continue to process.");
                    throw new SourceNotFoundException();
                }

                if (_lines == null || _lines.Length == 0)
                {
                    _logger.LogError("word Source not found. Unable to continue to process.");
                    throw new SourceNotFoundException();
                }
                this._baseWordList.AddRange(_lines);
            }
            catch (SourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to load word list.{0}:{1} ", ex.Message, ex.StackTrace);
            }
        }
    }
}
