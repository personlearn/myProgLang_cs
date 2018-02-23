using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone.ast
{
    public class NumberLiteral : ASTLeaf
    {
        public NumberLiteral(Token t) : base(t)
        {
        }

        public int value()
        {
            return Mtoken().getNumber();
        }
    }
}
