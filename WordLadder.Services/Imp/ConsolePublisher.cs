using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;
using WordLadder.Services.Abstract;

namespace WordLadder.Services.Imp
{
    public class ConsolePublisher : IPublisher
    {
        private ILogger<ConsolePublisher> _logger;

        public ConsolePublisher(ILogger<ConsolePublisher> logger)
        {
            _logger = logger;
        }

        public void Publish(ProcessingResult result)
        {
            Console.WriteLine(result.Print());
        }

        public void Publish(string message, JobPayload payload)
        {
            Console.WriteLine(payload.Print());
            Console.WriteLine(message);
        }
    }
}
