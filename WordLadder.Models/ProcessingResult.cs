using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    public class ProcessingResult
    {

        public ProcessingResult()
        {
            Results = new List<List<string>>();
        }

        public JobPayload Payload { get; set; }
        public List<List<string>> Results;
    }
}
