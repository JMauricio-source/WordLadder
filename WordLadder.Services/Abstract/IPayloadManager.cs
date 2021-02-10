using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;

namespace WordLadder.Services.Abstract
{
    public interface IPayloadManager
    {
        JobPayloadCommand LoadJob();

        bool IsValid(out string errors);
        bool HaveContinuosMode { get; set; }

        string HelpText();
        //List<string> GetWordList();
    }
}
