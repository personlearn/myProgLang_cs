using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone.ast
{
    public class ASTLeaf : ASTree
    {
        private static List<ASTree> empty = new List<ASTree>();

        protected Token token;

        public ASTLeaf(Token t) { token = t; }
        public override ASTree child(int i)
        {
            throw new IndexOutOfRangeException();
        }

        public override IEnumerator<ASTree> children()
        {
            return empty.GetEnumerator();
        }

        public override string location()
        {
            return "at line " + token.getLineNumber();
        }

        public override int numChildren()
        {
            return 0;
        }

        public override string ToString() { return token.getText(); }

        public Token Mtoken() { return token; }

    }
}
