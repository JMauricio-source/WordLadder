using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Models
{
    public class MatchResultLinkedList: LinkedList<MatchResult>
    {


        public MatchResult GetNextSibling(MatchResult currentNode) 
        {
            if (currentNode.ParentMatch == null) return null;
            var _next =  currentNode.ParentMatch.MatchesList.Find(currentNode)?.Next;

            return _next != null ? _next.Value : null;
        }

        
    }
}
