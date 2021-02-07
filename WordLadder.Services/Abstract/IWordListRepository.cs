using System;
using System.Collections.Generic;
using System.Text;

namespace WordLadder.Services.Abstract
{
    public interface IWordListRepository
    {

        void Clean();

        List<string> All();

        List<string> GetFiltered(Func<string,bool> filterPredicate); 
    }
}
