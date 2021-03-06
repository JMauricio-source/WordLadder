﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    /// <summary>
    /// Holds the context and the result of one processor iteration 
    /// </summary>
    public class ProcessingResult
    {

        public ProcessingResult()
        {
            Result = new List<string>();
        }

        public JobPayloadCommand Payload { get; set; }
        public List<string> Result;
        public bool WasSuccefull { get; set; }
        public string ResultMessage { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}
