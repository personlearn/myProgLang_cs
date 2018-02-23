using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone.ast
{
    public abstract class ASTree
    {
        public abstract ASTree child(int i);
        public abstract int numChildren();
        public abstract IEnumerator<ASTree> children();
        public abstract string location();
        public IEnumerator<ASTree> iterator() { return children(); }
    }
}
