using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Common;

namespace PWMIS.Core
{
    /// <summary>
    /// 文本搜索实用工具类
    /// </summary>
    public  class TextSearchUtil
    {

        /// <summary>
        /// 在 source句子字符串中搜索一个短语，并忽略source和短语中多余1个的空白字符，忽略大小写
        /// </summary>
        /// <param name="source">要搜索的源字符串</param>
        /// <param name="words">要匹配的字符串，以空格隔开的多个单词</param>
        /// <returns>返回短语在句子中第一次开始的位置和结束位置的结构</returns>
        public static  Point SearchWordsIndex(string source, string words)
        {
            string[] matchWrodArray = words.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int firstMatchIndex = source.IndexOf(matchWrodArray[0], 0, StringComparison.OrdinalIgnoreCase);
            if (matchWrodArray.Length == 1 && firstMatchIndex != -1)
                return new Point(firstMatchIndex, firstMatchIndex + matchWrodArray[0].Length);
            return SearchWordsIndex(source, matchWrodArray, firstMatchIndex);
        }

        /// <summary>
        /// 在 source句子字符串中搜索一个单词（全文匹配），并忽略source和单词中多余1个的空白字符，忽略大小写
        /// </summary>
        /// <param name="source">要搜索的源字符串</param>
        /// <param name="words">要匹配的一个或者多个单词，以空格隔开的多个单词</param>
        /// <returns>返回短语在句子中最后一个开始的位置和结束位置的结构</returns>
        public static Point SearchWordsLastIndex(string source, string words)
        {
            string[] matchWrodArray = words.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //如果有多个单词，之前的搜索会有问题，因此需要增加一个空格，感谢网友 “青岛-无刃剑”发现此问题！
            string matchWord = matchWrodArray.Length > 1 ? matchWrodArray[0] + " " : matchWrodArray[0];
            int firstMatchIndex = source.LastIndexOf(matchWord, source.Length - 1, StringComparison.OrdinalIgnoreCase);
            if (matchWrodArray.Length == 1 && firstMatchIndex != -1)
                return new Point(firstMatchIndex, firstMatchIndex + matchWord.Length);
            return SearchWordsIndex(source, matchWrodArray, firstMatchIndex);
        }

        /// <summary>
        /// 在 source句子字符串中搜索一组单词，并忽略source和短语中多余1个的空白字符，忽略大小写
        /// </summary>
        /// <param name="source">要搜索的源字符串</param>
        /// <param name="matchWrodArray">要匹配的单词数组</param>
        /// <param name="firstMatchIndex">目标单词在源字符串里面首次匹配的位置</param>
        /// <returns>返回单词在句子中开始的位置和结束位置的结构</returns>
        public static Point SearchWordsIndex(string source, string[] matchWrodArray, int firstMatchIndex)
        {
            //string[] matchWrodArray = words.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //int firstMatchIndex = source.LastIndexOf(matchWrodArray[0], source.Length - 1, StringComparison.OrdinalIgnoreCase);
            if (firstMatchIndex == -1) return new Point(-1, 0);
            int matchWordIndex = 1;
            int startIndex = firstMatchIndex + matchWrodArray[0].Length;
            bool needSpace = true;
            List<char> charList = new List<char>();
            for (int i = startIndex; i < source.Length; i++)
            {
                //匹配一个单词后，需要至少一个空白字符
                if (needSpace)
                {
                    if (char.IsWhiteSpace(source[i]))
                    {
                        needSpace = false;
                        continue;
                    }
                }
                if (!char.IsWhiteSpace(source[i]))
                {
                    //遇到非空白字符，处理字符串叠加
                    charList.Clear();
                    while (i < source.Length)
                    {
                        char c = source[i];
                        if (!char.IsWhiteSpace(c))
                        {
                            charList.Add(c);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //遇到空白字符，循环结束
                    string findWord = new string(charList.ToArray());
                    string matchWord = matchWrodArray[matchWordIndex];
                    if (string.Equals(findWord, matchWord, StringComparison.OrdinalIgnoreCase))
                    {
                        //如果当前句子中找到的单词跟当前要匹配的单词一致，则匹配成功，则进行下一轮匹配，直到要匹配的词语结束
                        matchWordIndex++;
                        if (matchWordIndex >= matchWrodArray.Length)
                            return new Point(firstMatchIndex, i);
                        else
                            needSpace = true;
                    }
                    else
                    {
                        //否则，当前不匹配，中断处理，结束句子查找
                        break;
                    }
                }
            }//end for
            return new Point(-1, 0);
        }

        /// <summary>
        /// 从输入字符串的指定位置开始寻找一个临近的单词，忽略前面的空白字符，直到遇到单词之后第一个空白字符或者分割符或者标点符号为止。
        /// </summary>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="startIndex">在输入字符串中要寻找单词的起始位置</param>
        /// <param name="maxLength">单词的最大长度，忽略超出部分</param>
        /// <param name="targetIndex">所找到目标单词在源字符串中的位置索引，如果没有找到，返回-1</param>
        /// <returns>找到的新单词</returns>
        public static string FindNearWords(string inputString, int startIndex, int maxLength, out int targetIndex)
        {
            return FindNearWords(inputString, startIndex, maxLength, '[', ']', out targetIndex);
        }

        /// <summary>
        /// 从输入字符串的指定位置开始寻找一个临近的单词，忽略前面的空白字符，直到遇到单词之后第一个空白字符或者分割符或者标点符号为止。
        /// </summary>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="startIndex">在输入字符串中要寻找单词的起始位置</param>
        /// <param name="maxLength">要索搜的最大长度，忽略超出部分，注意目标单词可能有空白字符，所以该长度应该大于单词的长度</param>
        /// <param name="leftSqlSeparator">SQL名字左分隔符</param>
        /// <param name="rightSqlSeparator">SQL名字右分隔符</param>
        /// <param name="targetIndex">所找到目标单词在源字符串中的位置索引，如果没有找到，返回-1</param>
        /// <returns>找到的新单词</returns>
        public static string FindNearWords(string inputString, int startIndex, int maxLength, char leftSqlSeparator, char rightSqlSeparator, out int targetIndex)
        {
            if (startIndex + maxLength > inputString.Length)
                maxLength = inputString.Length - startIndex;
            bool start = false;
            char[] words = new char[maxLength];//存储过程名字，最大长度255；
            int index = 0;
            bool sqlSeparator = false;//SQL 分割符，比如[],如果遇到，则忽略中间的空格，标点符号等
            int countWhiteSpace = 0;
            targetIndex = -1;

            foreach (char c in inputString.ToCharArray(startIndex, maxLength))
            {
                if (Char.IsWhiteSpace(c))
                {
                    countWhiteSpace++;
                    if (!start)
                        continue;//过滤前面的空白字符
                    else
                    {
                        if (!sqlSeparator)
                            break;//已经获取过字母字符，又遇到了空白字符，说明单词已经结束，跳出。
                        else
                            words[index++] = c;//空白字符在SQL分隔符内部，放入字母，找单词
                    }
                }
                else
                {
                    if (Char.IsSeparator(c) || Char.IsPunctuation(c))
                    {
                        if (sqlSeparator && c == rightSqlSeparator)
                            sqlSeparator = false;
                        else if (c == leftSqlSeparator)
                            sqlSeparator = true;

                        //需要处理[] 之间的的字符串
                        //感谢网友 “福州初学者” 发现多表查询使用 * 号查询的问题，2016.3.31
                        if (c == '*' || c == '.' || c == '_' || c == '-' || c == leftSqlSeparator || c == rightSqlSeparator)
                        {
                            words[index++] = c;//放入字母，找单词
                        }
                        else
                        {
                            if (!sqlSeparator)
                                break;//分割符（比如空格）或者标点符号，跳出。
                        }

                    }
                    else
                    {
                        words[index++] = c;//放入字母，找单词

                    }
                    if (!start)
                    {
                        start = true;
                        targetIndex = startIndex + countWhiteSpace;
                    }
                }
            }
            return new string(words, 0, index);
        }

        /// <summary>
        /// 从一个句子中指定的位置开始搜索目标字符，直到遇到非空白字符为止，返回该字符的位置索引。该字符不包括在单引号内
        /// </summary>
        /// <param name="inputString">要搜索的源字符串</param>
        /// <param name="startIndex">在源中搜索的起始位置</param>
        /// <param name="targetChar">目标字符</param>
        /// <returns>目标字符的在源字符串的位置索引，如果没有找到，返回-1</returns>
        public static int FindPunctuationBeforWord(string inputString, int startIndex, char targetChar)
        {
            if (startIndex >= inputString.Length - 1)
                return -1;
            bool isSqlStringFlag = false;
            for (int i = startIndex; i < inputString.Length; i++)
            {
                if (isSqlStringFlag)
                {
                    if (inputString[i] != '\'')
                        continue;
                    else
                        isSqlStringFlag = false;
                }
                else
                {
                    if (inputString[i] == targetChar)
                    {
                        return i;
                    }
                    else if (inputString[i] == '\'')
                    {
                        isSqlStringFlag = true;
                    }
                    else if (!char.IsWhiteSpace(inputString[i]))
                    {
                        break;
                    }
                }//end if
            }//end for
            return -1;
        }
    }
}
