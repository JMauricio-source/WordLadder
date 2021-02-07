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
    public class WordListRepository : IWordListRepository
    {
        private List<string> baseWordList;
        private string[] _args;
        private StringBuilder validationErrors;
        private WordLadderOptions wordLadderOptions;
        ILogger<WordListRepository> _logger;

        public WordListRepository(ILogger<WordListRepository> logger, IOptions<WordLadderOptions> options)
        {
            baseWordList = new List<string>();
            _args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            wordLadderOptions = options.Value;
            _logger = logger;
        }

        public List<string> All()
        {
            if (baseWordList.Count == 0) Load();

            return this.baseWordList;
        }

        public void Clean()
        {
            baseWordList.Clear();
        }

        public List<string> GetFiltered(Func<string, bool> filterPredicate)
        {
            if (baseWordList.Count == 0) Load();

            return this.baseWordList.Where(filterPredicate).Select(e => e).ToList();
        }

        private void Load()
        {
            try
            {
                var payload = CommonHelpers.LoadJob(_args);
                string[] _lines;
                if (!string.IsNullOrEmpty(payload.SourceFilePath))
                {
                    _lines = File.ReadAllLines(payload.SourceFilePath);
                }
                else if (!string.IsNullOrEmpty(wordLadderOptions.LocalWordDictionaryFilePath))
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
                this.baseWordList.AddRange(_lines);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to load word list.{0}:{1} ", ex.Message, ex.StackTrace);
            }
        }
    }
}
