using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;

namespace WordLadder.Services.Abstract
{
    public interface IPublisherHub
    {

        void PublishResultToAll(ProcessingResult result);

        void PublishMessageToAll(string message, JobPayload payload);
    }
}
