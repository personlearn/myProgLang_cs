using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone.ast
{
    public class ASTList : ASTree
    {
        protected List<ASTree> Vchildren;
        public ASTList(List<ASTree> list) { Vchildren = list; }
        public override ASTree child(int i)
        {
            return Vchildren[i];
        }

        public override IEnumerator<ASTree> children()
        {
            return Vchildren.GetEnumerator();
        }

        public override string location()
        {
            foreach(ASTree t in Vchildren)
            {
                string s = t.location();
                if (s != null)
                {
                    return s;
                }
            }
            return null;
        }

        public override int numChildren()
        {
            return Vchildren.Count;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            string sep = "";
            foreach(ASTree t in Vchildren)
            {
                builder.Append(sep);
                sep = " ";
                builder.Append(t.ToString());
            }
            return builder.Append(')').ToString();
        }
    }
}
