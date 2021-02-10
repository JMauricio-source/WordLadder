using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;

namespace WordLadder.Abstract.Services
{
    public interface IPayloadLoader
    {

        JobPayloadCommand LoadJob();

        bool IsValid( out string errors);

        string HelpText();


    }
}
