using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    /// <summary>
    /// Holds all the information to parametize the Word Ladder Algorithm
    /// </summary>
    public class JobPayloadCommand
    {
        public JobPayloadCommand(){}

        public string StartWord { get; set; }
        public string EndWord { get; set; }

        public string SourceFilePath { get; set; }
        public string ResultPublicationPath { get; set; }

        public SearchType TypeOfSearch { get; set; }

        public enum SearchType
        {
            BREATH_FIRST,
            DEEP_FIRST
        }

       
    }
}
