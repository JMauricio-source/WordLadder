using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models.Exceptions
{
    public class SourceNotFoundException: Exception
    {
        public override string Message => "Fatal. Source not found.";
    }
}
