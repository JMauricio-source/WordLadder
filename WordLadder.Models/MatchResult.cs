using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    public class MatchResult
    {
        private MatchResultLinkedList _matches;
        private MatchResult parent;

        public MatchResult(string sourceWord, MatchResult parentMatch)
        {
            SourceWord = sourceWord;
            _matches = new MatchResultLinkedList();
            parent = parentMatch;
        }

        public string SourceWord { get; set; }

        public MatchResultLinkedList MatchesList { get { return _matches; } }

        public MatchResult ParentMatch { get { return this.parent; } }

        public List<string> AncestorList()
        {
            List<string> ancestors = new List<string>();
            var node = this.parent;
            while ( node!= null) 
            {
                ancestors.Add(node.SourceWord);
                node = node.parent;
            }

            return ancestors;
        }
        public List<string> AncestorAndSelfList()
        {
            List<string> ancestors = new List<string>() { this.SourceWord};
            var node = this.parent;
            while (node != null)
            {
                ancestors.Add(node.SourceWord);
                node = node.parent;
            }

            return ancestors;
        }
    }
}
