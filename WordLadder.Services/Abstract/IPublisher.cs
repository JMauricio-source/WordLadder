using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;

namespace WordLadder.Services.Abstract
{
    public interface IPublisher
    {
        void Publish(ProcessingResult result);

        void Publish(string message, JobPayload payload);
    }
}
