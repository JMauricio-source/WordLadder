using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Abstract.Services;
using WordLadder.Models;
using System.Linq;
using System.IO;

namespace WordLadder.Services
{
    public class CommandLinePayloadLoader : IPayloadLoader
    {
        private string[] _args;
        private StringBuilder validationErrors;
        private int _allowedWordSize;

        private const string _s = "-s";
        private const string _f = "-f";
        private const string _d = "-d";
        private const string _o = "-o";

        public CommandLinePayloadLoader(string[] args, int allowedWordSize)
        {
            _args = args;
            validationErrors = new StringBuilder();
            _allowedWordSize = allowedWordSize;
        }

        public string HelpText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("** WordLadder application **");
            sb.AppendLine("[USAGE]: ");
            //-s bard -f chaw -d \"[PATH_TO_WORD_DICTIONARY]\words-english.txt\" -o \"[PATH_TO_RESULT_FILE]\result.txt\"
            sb.AppendLine("-s [Initial word] -f [Final word] -d \"[PATH_TO_WORD_DICTIONARY]/words-english.txt\" -o \"[PATH_TO_RESULT_FILE]\result.txt\"");
            return sb.ToString();
        }

        public bool IsValid(out string errors)
        {
            bool allOk = true;
            validationErrors.Clear();

            var pairs = keyValuePairs();

            
            if (!AllArgsPresentRule(pairs)) 
            {
                errors = validationErrors.ToString();
                return allOk;
            }
            allOk &= AllArgsValidRules(pairs);
            
            errors = validationErrors.ToString();
            return allOk;
        }

        public JobPayload LoadJob()
        {
            JobPayload j = new JobPayload();
            var dict = keyValuePairs();

            j.StartWord = dict[_s];
            j.EndWord = dict[_f];
            j.SourceFilePath = dict[_d];
            j.ResultPublicationPath = dict[_o];
            //[todo: get from cmd line]
            j.TypeOfResult = JobPayload.ResultType.FIRST;
            j.TypeOfSearch = JobPayload.SearchType.BREATH_FIRST;

            return j;
        }

        private Dictionary<string, string> keyValuePairs() 
        {
            Dictionary<string, string> dict = new Dictionary<string, string>() 
            { { _s,"" },{ _f,"" },{ _d,"" },{ _o,"" } };
            var nArgs = _args.Length;
            for (int i = 0; i < _args.Length; i = i + 2)
            {
                switch (_args[i].ToLower())
                {
                    case _s: { dict[_s] = i < nArgs ? _args[i+1]:""; }; break;
                    case _f: { dict[_f] = i < nArgs ? _args[i + 1] : ""; }; break;
                    case _d: { dict[_d] = i < nArgs ? _args[i + 1] : ""; }; break;
                    case _o: { dict[_o] = i < nArgs ? _args[i + 1] : ""; ; }; break;
                }
            }

            return dict;
        }


        private bool AllArgsPresentRule(Dictionary<string, string> keyValuePairs)
        {
            var isValid = keyValuePairs.Count() == 4 && !keyValuePairs.Values.Any(e => string.IsNullOrEmpty(e));
            if (!isValid) 
            {
                validationErrors.AppendLine("The following mandatory arguments are missing:");
                keyValuePairs.Where(e=> string.IsNullOrEmpty(e.Value)).Select(e=>e.Key).ToList().ForEach(e=> validationErrors.AppendLine(e));
            }
            return isValid;
        }

        private bool AllArgsValidRules(Dictionary<string, string> keyValuePairs)
        {
            bool rulesOK = true;
            //start word has correct size
            if (keyValuePairs[_s].Length != _allowedWordSize) 
            {
                validationErrors.AppendLine($"Parameter Start word({_s}) must have {_allowedWordSize} chars.");
                rulesOK = false;
            }

            //finish word has correct size
            if (keyValuePairs[_f].Length != _allowedWordSize)
            {
                validationErrors.AppendLine($"Parameter final word({_f}) must have {_allowedWordSize} chars.");
                rulesOK = false;
            }

            //Can read dictionary - Is valid Path
            if (!System.IO.File.Exists(keyValuePairs[_d]) 
                || 
                !System.IO.File.Exists(Path.GetFullPath(keyValuePairs[_d]))) 
            {                
                validationErrors.AppendLine($"Parameter final word({_f}) must have {_allowedWordSize} chars.");
                rulesOK = false;
            }

            /*
            // File must not exist
            if (File.Exists(keyValuePairs[_o])) 
            {
                validationErrors.AppendLine($"Output file already exists. Please choose a different name.");
                rulesOK = false;
            }
            //Can write to final path

            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
               // System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            */

            return rulesOK;
        }
    }
}
