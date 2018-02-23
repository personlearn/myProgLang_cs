using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace myProgLang.stone
{
    public class Lexer
    {
        public static string regexPat = "\\s*((//.*)|([0-9]+)|(\"(\\\\\"|\\\\\\\\|\\\\n|[^\"])*\")|[A-Z_a-z][A-Z_a-z0-9]*|==|<=|>=|&&|\\|\\||" + "[!|\"|#|\\$|%|&|'|\\(|\\)|\\*|\\+|,|-|\\.|/|:|;|<|=|>|\\?|@|\\[|\\\\|\\]|\\^|_|`|\\{|\\||\\}|~])?";

        private Regex pattern = new Regex(regexPat, RegexOptions.Compiled);

        private List<Token> queue = new List<Token>();

        private bool hasMore;

        private StreamReader reader;

        public Lexer(Stream r)
        {
            hasMore = true;
            reader = new StreamReader(r);
        }

        public Token read()
        {
            if (fillQueue(0))
            {
                var ret = queue[0];
                queue.RemoveAt(0);
                return ret;
            }
            else
            {
                return Token.EOF;
            }
        }

        public Token peek(int i)
        {
            if (fillQueue(i))
            {
                return queue[i];
            }
            else
            {
                return Token.EOF;
            }
        }

        private bool fillQueue(int i)
        {
            while (i >= queue.Count)
            {
                if (hasMore)
                {
                    readLine();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        protected void readLine()
        {
            string line;
            int linenumber = 0;
            try
            {
                line = reader.ReadLine();
                linenumber++;
            }
            catch (IOException e)
            {
                throw e;
            }
            if (line == null)
            {
                hasMore = false;
                return;
            }

            int lineNo = linenumber;
            Match matcher = pattern.Match(line);
            //matcher.useTransparentBounds(true).useAnchoringBounds(false);
            int pos = 0;
            int endpos = line.Length;
            while (pos < endpos)
            {
                //matcher.region(pos, endpos);
                if (matcher.Success)
                {
                    addToken(lineNo, matcher);
                    pos = matcher.Index + matcher.Length;
                }
                else
                {
                    throw new Exception("bad token at line " + lineNo);
                }
                matcher = matcher.NextMatch();
            }
            queue.Add(new IdToken(lineNo, Token.EOL));
        }

        protected void addToken(int lineNo, Match matcher)
        {
            string m = matcher.Groups[1].ToString();
            if (m != "")
            {
                if (matcher.Groups[2].ToString() == "")
                {
                    Token token;
                    if (matcher.Groups[3].ToString() != "")
                    {
                        token = new NumToken(lineNo, int.Parse(m));
                    }
                    else if (matcher.Groups[4].ToString() != "")
                    {
                        token = new StrToken(lineNo, toStringLiteral(m));
                    }
                    else
                    {
                        token = new IdToken(lineNo, m);
                    }
                    queue.Add(token);
                }
            }
        }

        protected string toStringLiteral(string s)
        {
            StringBuilder sb = new StringBuilder();
            int len = s.Length - 1;
            for (int i = 0; i < len; i++)
            {
                char c = s[i];
                if (c == '\\' && i + 1 < len)
                {
                    int c2 = s[i + 1];
                    if (c2 == '"' || c2 == '\\')
                    {
                        c = s[++i];
                    }
                    else if (c2 == 'n')
                    {
                        ++i;
                        c = '\n';
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected class NumToken : Token
        {
            public int value;
            public NumToken(int line,int v) : base(line)
            {
                value = v;
            }
            public override bool isNumber()
            {
                return true;
            }
            public override string getText()
            {
                return value.ToString();
            }
            public override int getNumber()
            {
                return value;
            }
        }

        protected class IdToken : Token
        {
            private string text;
            public IdToken(int line,string id) : base(line)
            {
                text = id;
            }

            public override bool isIdentifier()
            {
                return false;
            }

            public override string getText()
            {
                return text;
            }
        }

        protected class StrToken : Token
        {
            private string literal;
            public StrToken(int line,string str) : base(line)
            {
                literal = str;
            }
            public override bool isString()
            {
                return true;
            }
            public override string getText()
            {
                return literal;
            }
        }
    }
}
