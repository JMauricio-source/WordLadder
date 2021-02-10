using System;
using System.Collections.Generic;
using System.Text;
using static WordLadder.Models.JobPayloadCommand;

namespace WordLadder.Models
{
    /// <summary>
    /// Configurations options
    /// </summary>
    public class WordLadderOptions
    {
        public const string Key = "WordLadderOptions";

        /// <summary>
        /// Size of the words allowed as arguments
        /// </summary>
        public int? AllowedWordSize { get; set; }

        /// <summary>
        /// if after processing one ladder asks for another set of arguments or finish
        /// </summary>
        public bool? ContinuosMode { get; set; }

        /// <summary>
        /// if the search sould be done by searching in deep or breath
        /// </summary>
        public SearchType? TypeOfSearch { get; set; }

        /// <summary>
        /// Number o results to be retrieved if ResultType=TOP_N
        /// </summary>
        public int? TOP_N_Amount { get; set; }
        
        /// <summary>
        /// Default word list file path
        /// </summary>
        public string LocalWordDictionaryFilePath { get; set; }

        /// <summary>
        /// Default Result location
        /// </summary>
        public string ResultsDefaultPath { get; set; }
    }
}
