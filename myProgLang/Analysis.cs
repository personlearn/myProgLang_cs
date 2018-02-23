using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WordsAnalysis
{
    class Analysis
    {
        private string ansStr;
        private string[] ArrayByLine;
        private List<string> errList;
        private List<AnalysisResult> resultList;

        /// <summary>
        /// 
        /// </summary>
        struct AnalysisResult
        {
            public AnalysisResult(string HelpStr, int code)
            {
                this.helpStr = HelpStr;
                this.AnsCode = code;
            }

            public override string ToString()
            {
                return "(" + helpStr + "," + AnsCode.ToString() + ")";
            }

            public string helpStr;
            public int AnsCode;
        }
        /// <summary>
        /// 词法分析类构造方法
        /// </summary>
        public Analysis()
        {
            errList = new List<string>();
            resultList = new List<AnalysisResult>();
        }

        /// <summary>
        /// 词法分析类构造方法
        /// </summary>
        /// <param name="str">要分析的内容</param>
        public Analysis(string str)
        {
            errList = new List<string>();
            resultList = new List<AnalysisResult>();
            this.ansStr = str;
        }

        /// <summary>
        /// 要分析的内容
        /// </summary>
        public string AnalysisString
        {
            get { return this.ansStr; }
            set { this.ansStr = value; }
        }

        /// <summary>
        /// 将输入内容按行分成数组
        /// </summary>
        /// <param name="splitStr">输入内容</param>
        private void SplitByLine(string splitStr)
        {
            //this.ArrayByLine = splitStr.Split('/n');
            this.ArrayByLine = Regex.Split(splitStr, "/n");
        }

        /// <summary>
        /// 将内容以空格划分成数组
        /// </summary>
        /// <param name="splitStr">要分组的内容</param>
        /// <returns>已经分好的数组</returns>
        private string[] SplitBySpace(string splitStr)
        {
            return splitStr.Split(' ');
        }

        /// <summary>
        /// 开始分析
        /// </summary>
        public void Anaslysis()
        {
            SplitByLine(ansStr);
            for (int i = 0; i < ArrayByLine.Length; i++)
            {
                string[] checkStrings = SplitBySpace(ArrayByLine[i]);
                foreach (string checkStr in checkStrings)
                {
                    if (checkStr != "")
                    {
                        check(checkStr, i + 1);
                        resultList.Add(new AnalysisResult("$SPACE", 0));
                    }
                }
                resultList.Add(new AnalysisResult("$ENTER", 15));
            }
        }

        /// <summary>
        /// 分析指定的代码段
        /// </summary>
        /// <param name="str">代码段</param>
        /// <param name="LineCode">行号</param>
        private void check(string str, int LineCode)
        {
            if (str.Length == 1)
            {
                if (Char.IsNumber(str.ToCharArray()[0]) == true)
                {
                    resultList.Add(new AnalysisResult("$INT", 7));
                    return;
                }
                else if (CheckOperend(str) != 0)
                {
                    int n = CheckOperend(str);
                    switch (n)
                    {
                        case 8: resultList.Add(new AnalysisResult("$ASSIGH", 8)); break;
                        case 9: resultList.Add(new AnalysisResult("$PLUS", 9)); break;
                        case 10: resultList.Add(new AnalysisResult("$STAR", 10)); break;
                        case 12: resultList.Add(new AnalysisResult("$COMMA", 12)); break;
                        case 13: resultList.Add(new AnalysisResult("$LPAR", 13)); break;
                        case 14: resultList.Add(new AnalysisResult("$RPAR", 14)); break;
                    }
                    return;
                }
                else if (Char.IsLetter(str, 0) == true)
                {
                    resultList.Add(new AnalysisResult("$ID", 6));
                    return;
                }
                else
                {
                    errList.Add(errMessage(LineCode, 1, str));
                    resultList.Add(new AnalysisResult("$ERROR", 16));
                    return;
                }

            }
            if (HasOperend(str) == false)
            {
                if (IsNumeric(str.ToCharArray()) == true)
                {
                    resultList.Add(new AnalysisResult("$INT", 7));
                    return;
                }
                else if (CheckKeepWord(str.ToUpper()) != 0)
                {
                    int n = CheckKeepWord(str.ToUpper());
                    switch (n)
                    {
                        case 1: resultList.Add(new AnalysisResult("$DIM", 1)); break;
                        case 2: resultList.Add(new AnalysisResult("$IF", 2)); break;
                        case 3: resultList.Add(new AnalysisResult("$DO", 3)); break;
                        case 4: resultList.Add(new AnalysisResult("$STOP", 4)); break;
                        case 5: resultList.Add(new AnalysisResult("$END", 5)); break;
                    }
                    return;
                }
                else if (IsID(str.ToCharArray()) == true)
                {
                    if (str.Length <= 8)
                    {
                        resultList.Add(new AnalysisResult("$ID", 6));
                        return;
                    }
                    else
                    {
                        errList.Add(errMessage(LineCode, 3, str));
                        return;
                    }
                }
                else
                {
                    errList.Add(errMessage(LineCode, 1, str));
                    return;
                }
            }
            else if (HasOperend(str) == true)
            {
                char[] chars = str.ToCharArray();
                int k = 0;
                for (int i = 0; i < chars.Length; i++)
                {
                    if (IsOperend(chars[i]) == true)
                    {
                        if ((i - k) == 0)
                        {
                            int n = CheckOperend(chars[i]);
                            switch (n)
                            {
                                case 8: resultList.Add(new AnalysisResult("$ASSIGH", 8)); break;
                                case 9: resultList.Add(new AnalysisResult("$PLUS", 9)); break;
                                case 10:
                                    {
                                        try
                                        {
                                            char power = chars[i + 1];
                                            if (power == '*')
                                            {
                                                resultList.Add(new AnalysisResult("$POWER", 11));
                                                i++;
                                            }
                                            else
                                            {
                                                resultList.Add(new AnalysisResult("$STAR", 10));
                                            }
                                        }
                                        catch
                                        {
                                            resultList.Add(new AnalysisResult("$STAR", 10));
                                        }
                                        break;
                                    }
                                case 12: resultList.Add(new AnalysisResult("$COMMA", 12)); break;
                                case 13: resultList.Add(new AnalysisResult("$LPAR", 13)); break;
                                case 14: resultList.Add(new AnalysisResult("$RPAR", 14)); break;
                                default: break;
                            }
                        }
                        else
                        {
                            char[] tempChar = TempChar(chars, k, i);
                            if (tempChar.Length == 1)
                            {
                                if (Char.IsNumber(tempChar[0]) == true)
                                {
                                    resultList.Add(new AnalysisResult("$INT", 7));
                                }
                                else if (Char.IsLetter(tempChar[0]) == true)
                                {
                                    resultList.Add(new AnalysisResult("$ID", 6));
                                }
                                else
                                {
                                    errList.Add(errMessage(LineCode, 2, str));
                                }
                                int n = CheckOperend(Convert.ToString(chars[i]));
                                switch (n)
                                {
                                    case 8: resultList.Add(new AnalysisResult("$ASSIGH", 8)); break;
                                    case 9: resultList.Add(new AnalysisResult("$PLUS", 9)); break;
                                    case 10:
                                        {
                                            try
                                            {
                                                char power = chars[i + 1];
                                                if (power == '*')
                                                {
                                                    resultList.Add(new AnalysisResult("$POWER", 11));
                                                    i++;
                                                }
                                                else
                                                {
                                                    resultList.Add(new AnalysisResult("$STAR", 10));
                                                }
                                            }
                                            catch
                                            {
                                                resultList.Add(new AnalysisResult("$STAR", 10));
                                            }
                                            break;
                                        }
                                    case 12: resultList.Add(new AnalysisResult("$COMMA", 12)); break;
                                    case 13: resultList.Add(new AnalysisResult("$LPAR", 13)); break;
                                    case 14: resultList.Add(new AnalysisResult("$RPAR", 14)); break;
                                }
                            }
                            else
                            {
                                if (CheckKeepWord(Convert.ToString(tempChar).ToUpper()) != 0)
                                {
                                    int n = CheckKeepWord(Convert.ToString(tempChar).ToUpper());
                                    switch (n)
                                    {
                                        case 1: resultList.Add(new AnalysisResult("$DIM", 1)); break;
                                        case 2: resultList.Add(new AnalysisResult("$IF", 2)); break;
                                        case 3: resultList.Add(new AnalysisResult("$DO", 3)); break;
                                        case 4: resultList.Add(new AnalysisResult("$STOP", 4)); break;
                                        case 5: resultList.Add(new AnalysisResult("$END", 5)); break;
                                    }
                                }
                                else if (IsID(tempChar) == true)
                                {
                                    resultList.Add(new AnalysisResult("$ID", 6));
                                }
                                else if (IsNumeric(tempChar) == true)
                                {
                                    resultList.Add(new AnalysisResult("$INT", 7));
                                }
                                else
                                {
                                    errList.Add(errMessage(LineCode, 2, str));
                                }
                                int x = CheckOperend(Convert.ToString(chars[i]));
                                switch (x)
                                {
                                    case 8: resultList.Add(new AnalysisResult("$ASSIGH", 8)); break;
                                    case 9: resultList.Add(new AnalysisResult("$PLUS", 9)); break;
                                    case 10:
                                        {
                                            if (tempChar[i + 1] == '*')
                                            {
                                                resultList.Add(new AnalysisResult("$POWER", 11));
                                                i++;
                                            }
                                            resultList.Add(new AnalysisResult("$STAR", 10));
                                            break;
                                        }
                                    case 12: resultList.Add(new AnalysisResult("$COMMA", 12)); break;
                                    case 13: resultList.Add(new AnalysisResult("$LPAR", 13)); break;
                                    case 14: resultList.Add(new AnalysisResult("$RPAR", 14)); break;
                                }
                            }
                        }
                        k = i + 1;
                    }
                }
                if (k == chars.Length - 1)
                {
                    char[] tempChar = TempChar(chars, k, chars.Length);
                    if (CheckKeepWord(Convert.ToString(tempChar).ToUpper()) != 0)
                    {
                        int n = CheckKeepWord(Convert.ToString(tempChar).ToUpper());
                        switch (n)
                        {
                            case 1: resultList.Add(new AnalysisResult("$DIM", 1)); break;
                            case 2: resultList.Add(new AnalysisResult("$IF", 2)); break;
                            case 3: resultList.Add(new AnalysisResult("$DO", 3)); break;
                            case 4: resultList.Add(new AnalysisResult("$STOP", 4)); break;
                            case 5: resultList.Add(new AnalysisResult("$END", 5)); break;
                        }
                    }
                    else if (IsID(tempChar) == true)
                    {
                        resultList.Add(new AnalysisResult("$ID", 6));
                    }
                    else if (IsNumeric(tempChar) == true)
                    {
                        resultList.Add(new AnalysisResult("$INT", 7));
                    }
                    else
                    {
                        errList.Add(errMessage(LineCode, 2, str));
                    }
                }
                return;
            }
        }

        /// <summary>
        /// 检查是否为数字
        /// </summary>
        /// <param name="c">要检查的内容</param>
        /// <returns>是:true,否:false</returns>
        private bool IsNumeric(char[] chars)
        {
            foreach (char c in chars)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查是否为数字
        /// </summary>
        /// <param name="c">要检查的内容</param>
        /// <returns>是:true,否:false</returns>
        private bool IsOneNumeric(char c)
        {
            if (c >= '0' && c <= '9') return true;
            return false;
        }

        /// <summary>
        /// 检查是否为字母
        /// </summary>
        /// <param name="s">要检查的内容</param>
        /// <returns>是:true,否:false</returns>
        private bool IsLetter(char c)
        {
            if (Char.IsLetter(c)) return true;
            return false;
        }

        /// <summary>
        /// 检查是否含有操作符
        /// </summary>
        /// <param name="str">要检查的内容</param>
        /// <returns>含有:true,没有:false</returns>
        private bool HasOperend(string str)
        {
            int n = 0;
            char[] operends = { '=', '+', '*', ',', '(', ')' };
            n = str.IndexOfAny(operends);
            if (n >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// 检查是否为操作符
        /// </summary>
        /// <param name="c">要检查的内容</param>
        /// <returns>是:true,否:false</returns>
        private bool IsOperend(char c)
        {
            int n = 0;
            string str = Convert.ToString(c);
            char[] operends = { '=', '+', '*', ',', '(', ')' };
            n = str.IndexOfAny(operends);
            if (n >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// 得到指定范围的新char类型数组
        /// </summary>
        /// <param name="chars">原始数组</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>新的长度为end-start的char类型数组</returns>
        private char[] TempChar(char[] chars, int start, int end)
        {
            char[] tempChar = new char[end - start];
            int n = 0;
            for (int i = start; i < end; i++)
            {
                tempChar[n] = chars[i];
                n++;
            }
            return tempChar;
        }

        /// <summary>
        /// 检查操作符类型
        /// </summary>
        /// <param name="c">要检查的字符</param>
        /// <returns>操作符类型编码,为0则不是操作符</returns>
        private int CheckOperend(char c)
        {
            if (c == '=') return 8;
            else if (c == '+') return 9;
            else if (c == '*') return 10;
            else if (c == ',') return 12;
            else if (c == '(') return 13;
            else if (c == ')') return 14;
            else return 0;
        }

        /// <summary>
        /// 检查操作符类型
        /// </summary>
        /// <param name="s">要检查的内容</param>
        /// <returns>操作符编码,为0则不是操作符</returns>
        private int CheckOperend(string s)
        {
            if (s == "=") return 8;
            else if (s == "+") return 9;
            else if (s == "*") return 10;
            else if (s == ",") return 12;
            else if (s == "(") return 13;
            else if (s == ")") return 14;
            else return 0;
        }

        private bool IsID(char[] c)
        {
            if (Char.IsLetter(c[0]) == false && c[0] != '_')
            {
                return false;
            }
            for (int i = 1; i <= c.Length; i++)
            {
                if (Char.IsLetter(c[0]) == false && Char.IsNumber(c[0]) == false && c[0] != '_')
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查是否为保留字
        /// </summary>
        /// <param name="c">要检查的内容</param>
        /// <returns>返回保留字编码,为0则不是保留字</returns>
        private int CheckKeepWord(char[] c)
        {
            string str = Convert.ToString(c).ToUpper();
            int n = 0;
            switch (str)
            {
                case "DIM": n = 1; break;
                case "IF": n = 2; break;
                case "DO": n = 3; break;
                case "STOP": n = 4; break;
                case "END": n = 5; break;
                default: n = 0; break;
            }
            return n;
        }

        /// <summary>
        /// 检查是否为保留字
        /// </summary>
        /// <param name="str">要检查的内容</param>
        /// <returns>返回保留字编码,为0则不是保留字</returns>
        private int CheckKeepWord(string str)
        {
            int n = 0;
            switch (str)
            {
                case "DIM": n = 1; break;
                case "IF": n = 2; break;
                case "DO": n = 3; break;
                case "STOP": n = 4; break;
                case "END": n = 5; break;
                default: n = 0; break;
            }
            return n;
        }


        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <returns>所有错误信息</returns>
        public string ShowError()
        {
            string error = "";
            string[] errs = errList.ToArray();
            foreach (string err in errs)
            {
                error += err + "/r/n";
            }
            return error;
        }

        /// <summary>
        /// 显示分析结果
        /// </summary>
        /// <returns>所有分析结果</returns>
        public string ShowResult()
        {
            string result = "";
            AnalysisResult[] ans = resultList.ToArray();
            foreach (AnalysisResult an in ans)
            {
                if (an.AnsCode == 15)
                {
                    result += "/r/n";
                }
                else if (an.AnsCode == 0)
                {
                    result += " ";
                }
                else
                {
                    result += an.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 生成错误信息
        /// </summary>
        /// <param name="errLine">错误行号</param>
        /// <param name="errCode">错误代号</param>
        /// <param name="errStr">错误内容</param>
        /// <returns>错误信息</returns>
        private string errMessage(int errLine, int errCode, string errStr)
        {
            string errClass = "";
            if (errCode == 1)
            {
                errClass = "非法的标识符";
            }
            else if (errCode == 2)
            {
                errClass = "错误的表达式";
            }
            else if (errCode == 3)
            {
                errClass = "标识符长度最大为8";
            }
            resultList.Add(new AnalysisResult("$ERROR", 16));
            return "第" + errLine + "行 " + errClass + ":" + errStr;
        }

    }
}
