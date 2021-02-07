using System;
using System.Collections.Generic;
using System.Text;

using WordLadder.Models;
using System.Linq;
using System.IO;
using WordLadder.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WordLadder.Services.Imp
{
    public class CommandLinePayloadLoader : IPayloadManager
    {
        private string[] _args;
        private StringBuilder validationErrors;
        private WordLadderOptions wordLadderOptions;
        private int allowedWordSize;
        private bool haveContinuosMode;

        private const string _s = "-s";
        private const string _f = "-f";
        private const string _d = "-d";
        private const string _o = "-o";

        public bool HaveContinuosMode
        {
            get { return haveContinuosMode; }
            set { haveContinuosMode = value; }
        }

        public CommandLinePayloadLoader(IOptions<WordLadderOptions> options)
        {
            _args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            wordLadderOptions = options.Value;

            validationErrors = new StringBuilder();

            allowedWordSize = wordLadderOptions.AllowedWordSize ?? 2;

            haveContinuosMode = wordLadderOptions.ContinuosMode ?? false;

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

            var pairs = CommonHelpers.keyValuePairs(_args);


            if (!AllMandatoryArgsPresentRule(pairs))
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
            return CommonHelpers.LoadJob(_args);
        }

        private bool AllMandatoryArgsPresentRule(Dictionary<string, string> keyValuePairs)
        {
            var isValid = keyValuePairs.Count() == 4 && !keyValuePairs.Values.Any(e => string.IsNullOrEmpty(e));
            if (!isValid)
            {
                validationErrors.AppendLine("The following mandatory arguments are missing:");
                keyValuePairs.Where(e => string.IsNullOrEmpty(e.Value)).Select(e => e.Key).ToList().ForEach(e => validationErrors.AppendLine(e));
            }
            return isValid;
        }

        private bool AllArgsValidRules(Dictionary<string, string> keyValuePairs)
        {
            bool rulesOK = true;
            //start word has correct size
            if (keyValuePairs[_s].Length != allowedWordSize)
            {
                validationErrors.AppendLine($"Parameter Start word({_s}) must have {allowedWordSize} chars.");
                rulesOK = false;
            }

            //finish word has correct size
            if (keyValuePairs[_f].Length != allowedWordSize)
            {
                validationErrors.AppendLine($"Parameter final word({_f}) must have {allowedWordSize} chars.");
                rulesOK = false;
            }

            //Can read dictionary - Is valid Path
            if (!System.IO.File.Exists(keyValuePairs[_d])
                ||
                !System.IO.File.Exists(Path.GetFullPath(keyValuePairs[_d])))
            {
                validationErrors.AppendLine($"Parameter final word({_f}) must have {allowedWordSize} chars.");
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
