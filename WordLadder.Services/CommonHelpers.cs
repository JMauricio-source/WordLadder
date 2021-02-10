using System;
using System.Collections.Generic;
using System.Text;
using WordLadder.Models;

namespace WordLadder.Services
{
    public class CommonHelpers
    {
        private const string _s = "-s";
        private const string _f = "-f";
        private const string _d = "-d";
        private const string _o = "-o";

        public static Dictionary<string, string> keyValuePairs(string [] _args)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            { { _s,"" },{ _f,"" },{ _d,"" },{ _o,"" } };
            var nArgs = _args.Length;
            for (int i = 0; i < _args.Length; i = i + 2)
            {
                switch (_args[i].ToLower())
                {
                    case _s: { dict[_s] = i < nArgs ? _args[i + 1] : ""; }; break;
                    case _f: { dict[_f] = i < nArgs ? _args[i + 1] : ""; }; break;
                    case _d: { dict[_d] = i < nArgs ? _args[i + 1] : ""; }; break;
                    case _o: { dict[_o] = i < nArgs ? _args[i + 1] : ""; }; break;
                }
            }

            return dict;
        }

        public static JobPayloadCommand LoadJob(string[] _args, JobPayloadCommand.SearchType searchType )
        {
            JobPayloadCommand j = new JobPayloadCommand();
            var dict = keyValuePairs(_args);

            j.StartWord = dict[_s];
            j.EndWord = dict[_f];
            j.SourceFilePath = dict[_d];
            j.ResultPublicationPath = dict[_o];
            j.TypeOfSearch = searchType;

            return j;
        }

    }
}
