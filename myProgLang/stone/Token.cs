using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProgLang.stone
{
    public abstract class Token
    {
        //public const Token EOF = new Token(-1) { };

        public class eof : Token
        {
            public eof() : base(-1)
            {

            }
        }
        public static Token EOF = new eof() { };

        public const string EOL = "\\n";

        private int lineNumber;

        protected Token(int line)
        {
            lineNumber = line;
        }
        public virtual int getLineNumber() { return lineNumber; }
        public virtual bool isIdentifier() { return false; }
        public virtual bool isNumber() { return false; }
        public virtual bool isString() { return false; }
        public virtual int getNumber() { throw new StoneException("not number token"); }
        public virtual string getText() { return ""; }
    }
}
