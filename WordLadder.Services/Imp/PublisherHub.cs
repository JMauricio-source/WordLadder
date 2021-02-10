using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;
using WordLadder.Services.Abstract;

namespace WordLadder.Services.Imp
{
    public class PublisherHub : IPublisherHub
    {
        private ILogger<PublisherHub> _logger;
        private IEnumerable<IPublisher> _publishers;

        public PublisherHub(ILogger<PublisherHub> logger, IEnumerable<IPublisher> publishers)
        {
            _logger = logger;
            _publishers = publishers;
        }

        public void PublishMessageToAll(string message, JobPayloadCommand payload)
        {
            foreach (var publisher in _publishers)
            {
                publisher.Publish(message, payload);
            }
        }

        public void PublishResultToAll(ProcessingResult result)
        {
            foreach (var publisher in _publishers)
            {
                publisher.Publish(result);
            }
        }
    }
}
