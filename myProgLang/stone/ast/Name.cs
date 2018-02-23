using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone.ast
{
    public class Name : ASTLeaf
    {
        public Name(Token t) : base(t)
        {
        }

        public string name()
        {
            return Mtoken().getText();
        }
    }
}
