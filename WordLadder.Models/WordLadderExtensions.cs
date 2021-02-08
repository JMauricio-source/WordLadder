using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    public static class WordLadderExtensions
    {
        /// <summary>
        /// Printed version of a JobPayload instance
        /// </summary>
        /// <param name="jobPayload"></param>
        /// <returns></returns>
        public static string Print(this JobPayload jobPayload)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Start word     : {jobPayload.StartWord}");
            sb.AppendLine($"End word       : {jobPayload.EndWord}");
            sb.AppendLine($"Type of Result : {jobPayload.TypeOfResult}");
            sb.AppendLine($"Type of search : {jobPayload.TypeOfSearch}");
            sb.AppendLine($"Source file    : {jobPayload.SourceFilePath}");
            sb.AppendLine($"Results file   : {jobPayload.ResultPublicationPath}");

            return sb.ToString();
        }

        public static string Print(this ProcessingResult processingResult)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Word Ladder game");
            if (processingResult.Start != null && processingResult.End != null)
            {
                var duration = (processingResult.End - processingResult.Start).TotalMilliseconds;
                sb.AppendLine($"Processing Started at {processingResult.Start} and ended at {processingResult.End} [{duration} ms]");
            }
            sb.AppendLine("Payload:");
            sb.AppendLine(processingResult.Payload.Print());
            sb.AppendLine("Execution Result:");
            sb.AppendLine($"Was Succefull: {processingResult.WasSuccefull}");
            if (!string.IsNullOrEmpty(processingResult.ResultMessage))
                sb.AppendLine(processingResult.ResultMessage);

            if (processingResult.WasSuccefull) 
            {
                sb.AppendLine($"Found {processingResult.Results.Count} results");
                var counter = 1;
                foreach (var _stringList in processingResult.Results) 
                {
                    sb.AppendLine($"Result {counter} - {_stringList.Count} words");
                    _stringList.ForEach(e => sb.AppendLine(e));
                    counter++;
                }
            }
                
            return sb.ToString();
        }

    }
}
