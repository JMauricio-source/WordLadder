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

        public WordListRepository(ILogger<WordListRepository> logger, IOptions<WordLadderOptions> options)
        {
            _baseWordList = new List<string>();
            _args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            _wordLadderOptions = options.Value;
            _logger = logger;
        }

        public List<string> All()
        {
            if (_baseWordList.Count == 0) Load();

            return this._baseWordList;
        }

        public void Clean()
        {
            _baseWordList.Clear();
        }

        public List<string> GetFiltered(Func<string, bool> filterPredicate)
        {
            if (_baseWordList.Count == 0) Load();

            return this._baseWordList.Where(filterPredicate).Select(e => e).ToList();
        }

        private void Load()
        {
            try
            {
                var payload = CommonHelpers.LoadJob(_args,  _wordLadderOptions.TypeOfSearch.Value);
                string[] _lines;
                if (!string.IsNullOrEmpty(payload.SourceFilePath))
                {
                    _lines = File.ReadAllLines(payload.SourceFilePath);
                }
                else if (!string.IsNullOrEmpty(_wordLadderOptions.LocalWordDictionaryFilePath))
                {
                    _lines = File.ReadAllLines(payload.SourceFilePath);
                }
                else {
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
            catch (Exception ex)
            {
                _logger.LogError("Unable to load word list.{0}:{1} ", ex.Message, ex.StackTrace);
            }
        }
    }
}
