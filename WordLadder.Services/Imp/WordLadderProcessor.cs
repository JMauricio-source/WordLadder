using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WordLadder.Models;
using WordLadder.Services.Abstract;

namespace WordLadder.Services.Imp
{
    public class WordLadderProcessor : IWordLadderProcessor
    {
        private ILogger<WordLadderProcessor> _logger;
        private IWordListRepository _repository;

        public WordLadderProcessor(
            ILogger<WordLadderProcessor> logger, 
            IWordListRepository repository )
        {
            _logger = logger;
            _repository = repository;
        }

        Task<ProcessingResult> IWordLadderProcessor.ProcessAsync(JobPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
