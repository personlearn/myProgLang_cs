using myProgLang.stone.ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone
{
    public class StoneException : SystemException
    {
        public StoneException(string m) : base(m) { }
        public StoneException(string m, ASTree t) : base(m + " " + t.location()) { }
    }
}
