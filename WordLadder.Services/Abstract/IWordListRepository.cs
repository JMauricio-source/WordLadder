using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Services.Abstract
{
    public interface IWordListRepository
    {

        public List<string> WordList { get; }

        void Clear();

        List<string> All();

        List<string> GetFiltered(Func<string,bool> filterPredicate); 
    }
}
