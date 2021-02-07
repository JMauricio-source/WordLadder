using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    public class JobPayload
    {
        public JobPayload(){}

        public string StartWord { get; set; }
        public string EndWord { get; set; }

        public string SourceFilePath { get; set; }
        public string ResultPublicationPath { get; set; }

        public SearchType TypeOfSearch { get; set; }
        public ResultType TypeOfResult { get; set; }

        public enum SearchType
        {
            BREATH_FIRST,
            DEEP_FIRST
        }

        public enum ResultType
        {
            FIRST,
            TOP_N,
            ALL
        }
    }
}
