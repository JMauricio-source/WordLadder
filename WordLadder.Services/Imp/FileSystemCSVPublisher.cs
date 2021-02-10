using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordLadder.Models;
using WordLadder.Services.Abstract;
using System.Linq;

namespace WordLadder.Services.Imp
{
    /// <summary>
    /// Publisher for writting results as csv files
    /// </summary>
    public class FileSystemCSVPublisher : IPublisher
    {
        private ILogger<FileSystemTextPublisher> _logger;
        private WordLadderOptions wordLadderOptions;

        public FileSystemCSVPublisher(ILogger<FileSystemTextPublisher> logger, IOptions<WordLadderOptions> options)
        {
            _logger = logger;
            wordLadderOptions = options.Value;
        }


        public void Publish(ProcessingResult result)
        {
            try
            {

                var filePath = ResolveFilePath(result.Payload);
                StringBuilder sb = new StringBuilder();
                result.Results.ForEach(list => sb.AppendLine(string.Join(";", list)));

                SaveFile(filePath, sb.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("Error writing result {0}:{1}", e.Message, e.StackTrace);
            }
        }

        public void Publish(string message, JobPayloadCommand payload)
        {
            try
            {
                var filePath = ResolveFilePath(payload);

                SaveFile(filePath, message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error writing result {0}:{1}", e.Message, e.StackTrace);
            }
        }

        private string ResolveFilePath(JobPayloadCommand payload)
        {
            var _path = string.IsNullOrEmpty(payload.ResultPublicationPath) ? wordLadderOptions.ResultsDefaultPath : payload.ResultPublicationPath;
            return _path.Replace(".txt", ".csv");
        }

        private void SaveFile(string filePath, string content)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            if (File.Exists(filePath))
            {
                File.AppendAllText(filePath, content);
            }
            else { File.WriteAllText(filePath, content); }
        }
    }
}
