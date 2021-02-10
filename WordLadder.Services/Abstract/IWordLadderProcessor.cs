using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WordLadder.Models;

namespace WordLadder.Services.Abstract
{
    public interface IWordLadderProcessor
    {

        Task<ProcessingResult> ProcessAsync(JobPayloadCommand payload);
    }
}
