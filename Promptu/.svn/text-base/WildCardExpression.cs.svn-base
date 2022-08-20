using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal class WildCardExpression
    {
        private static char[] wildcards = new char[] { '*', '?' };
        private List<string> matchPatterns = new List<string>();
        private bool caseSensitive;
        private List<string> notMatchPatterns = new List<string>();

        public WildCardExpression(string pattern, bool caseSensitive)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }

            if (!caseSensitive)
            {
                pattern = pattern.ToUpperInvariant();
            }

            string[] splitPatterns = pattern.Split('|');

            foreach (string splitPattern in splitPatterns)
            {
                if (splitPattern.Length > 0 && splitPattern[0] == '!')
                {
                    this.notMatchPatterns.Add(splitPattern.Substring(1));
                }
                else if (splitPattern.StartsWith("\\!"))
                {
                    this.matchPatterns.Add(splitPattern.Substring(1));
                }
                else
                {
                    this.matchPatterns.Add(splitPattern);
                }
            }

            this.caseSensitive = caseSensitive;
        }

        public bool IsMatch(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (!this.caseSensitive)
            {
                text = text.ToUpperInvariant();
            }

            foreach (string notMatchPattern in this.notMatchPatterns)
            {
                if (notMatchPattern.Length == 0 || IsMatch(text, notMatchPattern))
                {
                    return false;
                }
            }

            foreach (string pattern in this.matchPatterns)
            {
                if (IsMatch(text, pattern))
                {
                    return true;
                }
            }

            return this.matchPatterns.Count <= 0;
        }

        private bool IsMatch(string text, string pattern)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (pattern.IndexOfAny(wildcards) < 0)
            {
                return text == pattern;
            }

            int patternI = 0;
            int textI = 0;

            while (patternI < pattern.Length && textI < text.Length)
            {
                char patternChar = pattern[patternI];
                if (patternChar == '*')
                {
                    break;
                }

                //if (patternChar == '\\' && patternI + 1 < pattern.Length)
                //{
                //    char nextChar = this.pattern[patternI + 1];
                //    if (nextChar == '*')
                //    {
                //        patternChar = nextChar;
                //        patternI++;
                //    }
                //}

                if (patternChar != text[textI] && patternChar != '?')
                {
                    return false;
                }

                textI++;
                patternI++;
            }

            if (patternI >= pattern.Length)
            {
                return textI >= text.Length;
            }

            int cp = 0;
            int mp = 0;

            while (textI < text.Length)
            {
                if (patternI < pattern.Length && pattern[patternI] == '*')
                {
                    if (++patternI >= pattern.Length)
                    {
                        return true;
                    }

                    mp = patternI - 1;
                    cp = textI + 1;
                }
                else if (patternI < pattern.Length && (pattern[patternI] == text[textI] || pattern[patternI] == '?'))
                {
                    patternI++;
                    textI++;
                }
                else
                {
                    patternI = mp;
                    textI = cp++;
                }
            }

            while (patternI < pattern.Length && pattern[patternI] == '*')
            {
                patternI++;
            }

            return patternI >= pattern.Length;
        }

        //public static void Test()
        //{
        //    test("", "", true);
        //    test("*", "", true);
        //    test("*", "A", true);
        //    test("", "A", false);
        //    test("A*", "", false);
        //    test("A*", "AAB", true);
        //    test("A*", "BAA", false);
        //    test("A*", "A", true);
        //    test("A*B", "", false);
        //    test("A*B", "AAB", true);
        //    test("A*B", "AB", true);
        //    test("A*B", "AABA", false);
        //    test("A*B", "ABAB", true);
        //    test("A*B", "ABBBB", true);
        //    test("A*B*C", "", false);
        //    test("A*B*C", "ABC", true);
        //    test("A*B*C", "ABCC", true);
        //    test("A*B*C", "ABBBC", true);
        //    test("A*B*C", "ABBBBCCCC", true);
        //    test("A*B*C", "ABCBBBCBCCCBCBCCCC", true);
        //    test("A*B*", "AB", true);
        //    test("A*B*", "AABA", true);
        //    test("A*B*", "ABAB", true);
        //    test("A*B*", "ABBBB", true);
        //    test("A*B*C*", "", false);
        //    test("A*B*C*", "ABC", true);
        //    test("A*B*C*", "ABCC", true);
        //    test("A*B*C*", "ABBBC", true);
        //    test("A*B*C*", "ABBBBCCCC", true);
        //    test("A*B*C*", "ABCBBBCBCCCBCBCCCC", true);
        //    test("A?", "AAB", false);
        //    test("A?B", "AAB", true);
        //    test("A?*", "A", false);
        //    test("A?*", "ABBCC", true);
        //    test("A?*", "BAA", false);
        //    test("!", "", false);
        //    test("!*.txt|*.txt", "bob.TXT", false);
        //    test("!*.txt|!*.svml", "bob.svml", false);
        //    test("!*.txt|\\!*.svml", "!bob.svml", true);
        //}

        //private static void test(string pattern, string text, bool expected)
        //{
        //    if (new WildCardExpression(pattern, false).IsMatch(text) != expected)
        //    {
        //        throw new ArgumentException();
        //    }
        //}
    }
}
