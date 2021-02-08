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
    public class FileSystemPublisher : IPublisher
    {
        private ILogger<FileSystemPublisher> _logger;
        private WordLadderOptions wordLadderOptions;



        public FileSystemPublisher(ILogger<FileSystemPublisher> logger, IOptions<WordLadderOptions> options)
        {
            _logger = logger;
            wordLadderOptions = options.Value;
        }

        public void Publish(ProcessingResult result)
        {
            try
            {
                var filePath = ResolveFilePath(result.Payload);

                //if (!Directory.Exists(Path.GetFullPath(filePath))) 
                //{ 
                //    Directory.CreateDirectory(filePath); 
                //} 
                while (File.Exists(filePath))
                {
                    Guid sufix = Guid.NewGuid();
                    filePath = filePath.Replace(".txt", sufix + ".txt");
                }

                File.WriteAllText(filePath, result.Print());
            }
            catch (Exception e)
            {
                _logger.LogError("Error writing result {0}:{1}", e.Message, e.StackTrace);
            }
        }

        public void Publish(string message, JobPayload payload)
        {

            try
            {
                var filePath = ResolveFilePath(payload);

                //if (!Directory.Exists(Path.GetFullPath(filePath)))
                //{
                //    Directory.CreateDirectory(filePath);
                //}

                while (File.Exists(filePath))
                {
                    Guid sufix = Guid.NewGuid();
                    filePath = filePath.Replace(".txt", sufix + ".txt");
                }

                File.WriteAllText(filePath, message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error writing result {0}:{1}", e.Message, e.StackTrace);
            }
        }

        private string ResolveFilePath(JobPayload payload)
        {
            return string.IsNullOrEmpty(payload.ResultPublicationPath) ? wordLadderOptions.ResultsDefaultPath : payload.ResultPublicationPath;
        }
    }
}
