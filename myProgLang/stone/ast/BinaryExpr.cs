using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone.ast
{
    public class BinaryExpr : ASTList
    {
        public BinaryExpr(List<ASTree> list) : base(list)
        {
        }

        public ASTree left()
        {
            return child(0);
        }

        public string Operator()
        {
            return ((ASTLeaf)child(1)).Mtoken().getText();
        }

        public ASTree right()
        {
            return child(2);
        }
    }
}
