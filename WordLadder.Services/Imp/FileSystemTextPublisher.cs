using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordLadder.Models;
using WordLadder.Services.Abstract;

namespace WordLadder.Services.Imp
{
    /// <summary>
    /// Publisher for writting results as txt files
    /// </summary>
    public class FileSystemTextPublisher : IPublisher
    {
        private ILogger<FileSystemTextPublisher> _logger;
        private WordLadderOptions wordLadderOptions;



        public FileSystemTextPublisher(ILogger<FileSystemTextPublisher> logger, IOptions<WordLadderOptions> options)
        {
            _logger = logger;
            wordLadderOptions = options.Value;
        }

        public void Publish(ProcessingResult result)
        {
            try
            {
                var filePath = ResolveFilePath(result.Payload);

                SaveFile(filePath, result.Print());
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
            return string.IsNullOrEmpty(payload.ResultPublicationPath) ? wordLadderOptions.ResultsDefaultPath : payload.ResultPublicationPath;
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
