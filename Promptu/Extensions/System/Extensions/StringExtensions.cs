//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ZachJohnson.Promptu;

    internal static class StringExtensions
    {
        public static string ReplaceIntrinsics(this string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            string[] split = s.Split('$');

            if (split.Length == 1)
            {
                return s;
            }

            StringBuilder result = new StringBuilder();
            bool nextIsNotVariable = false;

            for (int i = 0; i < split.Length; i++)
            {
                if (!nextIsNotVariable && i > 0 && i < split.Length - 1)
                {
                    nextIsNotVariable = false;
                    bool notVariable = false;

                    switch (split[i].ToUpperInvariant())
                    {
                        case "DRIVE":
                            string drive = System.IO.Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
                            if (drive.Length > 0 && drive[drive.Length - 1] == System.IO.Path.DirectorySeparatorChar)
                            {
                                drive = drive.Substring(0, drive.Length - 1);
                            }

                            result.Append(drive);
                            break;
                        default:
                            notVariable = true;
                            break;
                    }

                    if (notVariable)
                    {
                        result.Append("$");
                        result.Append(split[i]);
                    }
                    else
                    {
                        nextIsNotVariable = true;
                    }
                }
                else
                {
                    if (!nextIsNotVariable && i > 0)
                    {
                        result.Append('$');
                    }

                    nextIsNotVariable = false;

                    result.Append(split[i]);
                }
            }

            return result.ToString();
        }

        public static string ExpandEnvironmentVariables(this string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            string[] split = s.Split('%');

            if (split.Length == 1)
            {
                return s;
            }

            StringBuilder result = new StringBuilder();
            bool nextIsNotVariable = false;

            for (int i = 0; i < split.Length; i++)
            {
                if (!nextIsNotVariable && i > 0 && i < split.Length - 1)
                {
                    nextIsNotVariable = false;
                    bool notVariable = false;

                    try
                    {
                        string variableValue = Environment.GetEnvironmentVariable(split[i], EnvironmentVariableTarget.Process);

                        if (variableValue != null)
                        {
                            result.Append(variableValue);
                        }
                        else
                        {
                            notVariable = true;
                        }
                    }
                    catch (NotSupportedException)
                    {
                        notVariable = true;
                    }
                    catch (Security.SecurityException)
                    {
                        notVariable = true;
                    }

                    if (notVariable)
                    {
                        result.Append("%");
                        result.Append(split[i]);
                    }
                    else
                    {
                        nextIsNotVariable = true;
                    }
                }
                else
                {
                    if (!nextIsNotVariable && i > 0)
                    {
                        result.Append('%');
                    }

                    nextIsNotVariable = false;

                    result.Append(split[i]);
                }
            }

            return result.ToString();
        }

        public static string Replace(this string s, string oldValue, string newValue, bool caseInsensitive)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (caseInsensitive)
            {
                string tmp = s;

                int index = 0;
                int positionToStartAt = 0;
                while ((index = tmp.IndexOf(oldValue, positionToStartAt, StringComparison.CurrentCultureIgnoreCase)) >= 0)
                {
                    tmp = tmp.Substring(0, index) + newValue + tmp.Substring(index + oldValue.Length);
                }

                return tmp;
            }
            else
            {
                return s.Replace(oldValue, newValue);
            }
        }

        public static string Escape(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                switch (character)
                {
                    case '"':
                    case '\\':
                        builder.Append("\\");
                        break;
                    default:
                        break;
                }

                builder.Append(character);
            }

            return builder.ToString();
        }

        public static string Unescape(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                switch (character)
                {
                    case '\\':
                        if (++i < text.Length)
                        {
                            character = text[i];
                        }
                        else
                        {
                            continue;
                        }

                        break;
                    default:
                        break;
                }

                builder.Append(character);
            }

            return builder.ToString();
        }

        public static string ToUpperInvariantNullSafe(this string s)
        {
            if (s == null)
            {
                return s;
            }
            else
            {
                return s.ToUpperInvariant();
            }
        }

        public static int IndexOfNextBreakingChar(this string text, char breakingChar)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            char? breakChar;

            return IndexOfNextBreakingCharInternal(text, 0, text.Length, false, out breakChar, breakingChar);
        }

        public static int IndexOfNextBreakingChar(this string text, int startIndex, int count, char breakingChar)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            char? breakChar;

            return IndexOfNextBreakingCharInternal(text, startIndex, count, false, out breakChar, breakingChar);
        }

        public static int IndexOfNextBreakingChar(this string text, bool strict, out char? breakChar, params char[] otherBreakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return IndexOfNextBreakingCharInternal(text, 0, text.Length, strict, out breakChar, otherBreakingChars);
        }

        public static int IndexOfNextBreakingChar(this string text, bool strict, params char[] otherBreakingChars)
        {
            char? breakChar;
            return IndexOfNextBreakingChar(text, strict, out breakChar, otherBreakingChars);
        }

        public static int IndexOfNextBreakingChar(this string text, int startIndex, int count, bool strict, params char[] otherBreakingChars)
        {
            char? breakChar;
            return IndexOfNextBreakingChar(text, startIndex, count, strict, out breakChar, otherBreakingChars);
        }

        public static int IndexOfNextBreakingChar(this string text, int startIndex, int count, bool strict, out char? breakChar, params char[] otherBreakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return IndexOfNextBreakingCharInternal(text, startIndex, count, strict, out breakChar, otherBreakingChars);
        }

        public static string ReplaceSubstring(this string text, string replacementValue, int index, int length)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (replacementValue == null)
            {
                throw new ArgumentNullException("replacementValue");
            }
            else if (index < 0)
            {
                throw new ArgumentOutOfRangeException("'index' cannot be less than zero.");
            }
            else if (index + length > text.Length)
            {
                throw new ArgumentOutOfRangeException("'index' + 'length' cannot be greater than the length of the provided string.");
            }

            StringBuilder result = new StringBuilder();

            if (index > 0)
            {
                string substring = text.Substring(0, index);
                result.Append(substring);
            }

            result.Append(replacementValue);

            result.Append(text.Substring(index + length));

            return result.ToString();
        }

        public static int LastIndexOfBreakingChar(this string text, bool strict, char breakingChar)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return LastIndexOfBreakingChar(text, text.Length - 1, strict, breakingChar);
        }

        public static int LastIndexOfBreakingChar(this string text, bool strict, params char[] otherBreakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return LastIndexOfBreakingChar(text, text.Length - 1, strict, otherBreakingChars);
        }

        public static int LastIndexOfBreakingChar(this string text, int startIndex, bool strict, char breakingChar)
        {
            return LastIndexOfBreakingCharInternal(text, startIndex, strict, breakingChar);
        }

        public static int LastIndexOfBreakingChar(this string text, int startIndex, bool strict, params char[] otherBreakingChars)
        {
            return LastIndexOfBreakingCharInternal(text, startIndex, strict, otherBreakingChars);
        }

        public static int CountReverseCharRun(this string text, char c)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return CountReverseCharRun(text, c, text.Length - 1);
        }

        public static int CountReverseCharRun(this string text, char c, int index)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            int count = 0;

            for (int i = index; i >= 0; i--)
            {
                if (text[i] == c)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        public static bool PreviousCharIs(this string text, int index, bool ignoreWhitespace, params char[] chars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            for (int i = index; i >= 0; i--)
            {
                char c = text[i];
                if (ignoreWhitespace && char.IsWhiteSpace(c))
                {
                    continue;
                }

                foreach (char charToFind in chars)
                {
                    if (c == charToFind)
                    {
                        return true;
                    }
                }

                break;
            }

            return false;
        }

        public static int CountOf(this string text, char c, bool ignoreIfEscaped)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return text.CountOf(c, ignoreIfEscaped, 0);
        }

        public static int CountOf(this string text, char c, bool ignoreIfEscaped, int startIndex)
        {
            return text.CountOf(c, ignoreIfEscaped, startIndex, text.Length - startIndex);
        }

        public static int CountOf(this string text, char c, bool ignoreIfEscaped, int startIndex, int length)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            int totalLength = startIndex + length;

            int count = 0;

            for (int i = startIndex; i < totalLength; i++)
            {
                if (text[i] == c && (i > 0 && text.CountReverseCharRun('\\', i - 1) % 2 == 0))
                {
                    count++;
                }
            }

            return count;
        }

        public static int CountBreaks(this string text, params char[] otherBreakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            List<char> otherBreakingCharsList = new List<char>(otherBreakingChars);
            int breakCount = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                if (character == ' ')
                {
                    if (i + 1 < text.Length)
                    {
                        char nextCharacter = text[i + 1];
                        if (nextCharacter == '"')
                        {
                            i++;

                            while (i + 1 < text.Length)
                            {
                                i++;

                                character = text[i];
                                if (character == '"')
                                {
                                    if (i + 1 >= text.Length || (i + 1 < text.Length && text[i + 1] == ' '))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (otherBreakingCharsList.Contains(character))
                {
                    breakCount++;
                }
            }

            return breakCount;
        }

        public static bool NextCharIs(this string s, char c, int indexToStart, bool ignoreWhitespace)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            else if (indexToStart < 0)
            {
                throw new ArgumentOutOfRangeException("'indexToStart' cannot be less than zero.");
            }
            else if (indexToStart > s.Length)
            {
                throw new ArgumentOutOfRangeException("'indexToStart' cannot be greater than the length of the provided string.");
            }
            else if (char.IsWhiteSpace(c) && ignoreWhitespace)
            {
                throw new ArgumentException("'c' is whitespace.  'c' cannot be whitespace since 'ignoreWhitespace' is 'true'.");
            }
            else if (indexToStart == s.Length)
            {
                return false;
            }

            for (int i = indexToStart; i < s.Length; i++)
            {
                char character = s[i];
                if (ignoreWhitespace && char.IsWhiteSpace(character))
                {
                    continue;
                }

                return character == c;
            }

            return false;
        }

        public static string[] BreakApart(this string text, Quotes quotes, Spaces spaces)
        {
            return text.BreakApart(quotes, spaces, BreakingCharAction.Eat);
        }

        public static string[] BreakApart(this string text)
        {
            return text.BreakApart(Quotes.Eat, Spaces.Break);
        }

        public static string[] BreakApart(this string text, Quotes quotes, Spaces spaces, BreakingCharAction otherBreakingCharAction, params char[] otherBreakingChars)
        {
            int[] breakBeginIndexes;
            return text.BreakApart(quotes, spaces, otherBreakingCharAction, out breakBeginIndexes, otherBreakingChars);
        }

        public static string[] BreakApart(this string text, Quotes quotes, Spaces spaces, BreakingCharAction otherBreakingCharAction, out int[] breakBeginIndexes, params char[] otherBreakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("s");
            }

            List<string> args = new List<string>();
            List<int> breakIndexes = new List<int>();
            StringBuilder currentArgument = new StringBuilder();
            bool eatQuotes = quotes == Quotes.Eat;
            bool justAdded = false;
            for (int i = 0; i < text.Length; i++)
            {
                justAdded = false;
                char character = text[i];

                bool isSpace = character == ' ';

                if (otherBreakingChars.Contains(character)
                    || (isSpace && spaces != Spaces.None))
                {
                    if (!isSpace || spaces != Spaces.DoNotBreak)
                    {
                        if (!isSpace && otherBreakingCharAction == BreakingCharAction.AppendToBefore)
                        {
                            currentArgument.Append(character);
                        }

                        string value = currentArgument.ToString();
                        breakIndexes.Add(i - value.Length);
                        args.Add(value);
                        currentArgument = new StringBuilder();
                        justAdded = true;

                        if (!isSpace && otherBreakingCharAction == BreakingCharAction.AppendToNext)
                        {
                            currentArgument.Append(character);
                        }
                    }
                    else
                    {
                        currentArgument.Append(character);
                    }

                    if (quotes != Quotes.None)
                    {
                        if (i + 1 < text.Length)
                        {
                            char nextCharacter = text[i + 1];
                            if (nextCharacter == '"')
                            {
                                justAdded = false;
                                if (eatQuotes)
                                {
                                    i++;
                                }

                                while (i + 1 < text.Length)
                                {
                                    i++;

                                    character = text[i];
                                    if (character == '"' && text.CountReverseCharRun('\\', i - 1) % 2 == 0)
                                    {
                                        bool endQuote = i + 1 >= text.Length;
                                        if (!endQuote && i + 1 < text.Length)
                                        {
                                            char postQuoteChar = text[i + 1];
                                            endQuote = otherBreakingChars.Contains(postQuoteChar)
                                                || (postQuoteChar == ' ' && spaces != Spaces.None);
                                        }

                                        if (endQuote)
                                        {
                                            if (!eatQuotes)
                                            {
                                                currentArgument.Append(character);
                                            }

                                            break;
                                        }
                                    }

                                    currentArgument.Append(character);
                                }

                                if (quotes == Quotes.Eat)
                                {
                                    currentArgument = new StringBuilder(currentArgument.ToString().Unescape());
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    currentArgument.Append(character);
                }
            }

            if (!justAdded)
            {
                string value = currentArgument.ToString();
                breakIndexes.Add(text.Length - value.Length);
                args.Add(value);
            }

            breakBeginIndexes = breakIndexes.ToArray();
            return args.ToArray();
        }

        public static string[] ConvertToArguments(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            List<string> args = new List<string>();
            StringBuilder currentArgument = new StringBuilder();
            bool justAdded = false;
            for (int i = 0; i < text.Length; i++)
            {
                justAdded = false;
                char character = text[i];

                if (character == ' ' || character == '"')
                {
                    if (i > 0)
                    {
                        string value = currentArgument.ToString();
                        args.Add(value);
                        currentArgument = new StringBuilder();
                        justAdded = true;
                    }

                    int nextIndex = character == '"' ? i : i + 1;

                    if (nextIndex < text.Length)
                    {
                        char nextCharacter = text[nextIndex];
                        if (nextCharacter == '"')
                        {
                            justAdded = false;
                            i = nextIndex;

                            while (i + 1 < text.Length)
                            {
                                i++;

                                character = text[i];
                                if (character == '"')
                                {
                                    bool endQuote = i + 1 >= text.Length;
                                    if (!endQuote && i + 1 < text.Length)
                                    {
                                        char postQuoteChar = text[i + 1];
                                        endQuote = postQuoteChar == ' ';
                                    }

                                    if (endQuote)
                                    {
                                        break;
                                    }
                                }

                                currentArgument.Append(character);
                            }

                            currentArgument = new StringBuilder(currentArgument.ToString().Unescape());
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    currentArgument.Append(character);
                }
            }

            if (!justAdded)
            {
                string value = currentArgument.ToString();
                args.Add(value);
            }

            return args.ToArray();
        }

        public static int OccurenceCountOf(this string s, params char[] chars)
        {
            return s.OccurenceCountOf(false, chars);
        }

        public static int OccurenceCountOf(this string s, int length, params char[] chars)
        {
            return s.OccurenceCountOf(false, length, chars);
        }

        public static int OccurenceCountOf(this string s, bool ignoreQuotedSections, params char[] chars)
        {
            return s.OccurenceCountOf(ignoreQuotedSections, s.Length, chars);
        }

        public static int OccurenceCountOf(this string s, bool ignoreQuotedSections, int length, params char[] chars)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            else if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "'length' cannot be less than zero.");
            }
            else if (length > s.Length)
            {
                throw new ArgumentOutOfRangeException("length", "'length' cannot be greater than the length of the string.");
            }

            int count = 0;
            for (int i = 0; i < length; i++)
            {
                char character = s[i];

                foreach (char c in chars)
                {
                    if (character == c)
                    {
                        count++;
                        break;
                    }
                }

                if (ignoreQuotedSections && character == ' ')
                {
                    if (i + 1 < s.Length)
                    {
                        char nextCharacter = s[i + 1];
                        if (nextCharacter == '"')
                        {
                            i++;

                            while (i + 1 < s.Length)
                            {
                                i++;

                                character = s[i];
                                if (character == '"')
                                {
                                    if (i + 1 >= s.Length || (i + 1 < s.Length && s[i + 1] == ' '))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return count;
        }

        public static string ConcatenateAll(this string[] array, string separator)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            return array.ConcatenateAll(0, separator);
        }

        public static string ConcatenateAll(this string[] array, int start, string separator)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            return array.Concatenate(start, array.Length - start, separator);
        }

        public static string ConcatenateAllBetween(this string[] array, int start, int end, string separator)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            return array.Concatenate(start, end - start + 1, separator);
        }

        public static string Concatenate(this string[] array, int start, int count, string separator)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (start < 0)
            {
                throw new ArgumentOutOfRangeException("'start' cannot be less than zero.");
            }
            else if (start >= array.Length)
            {
                throw new ArgumentOutOfRangeException("'start' cannot be greater than or equal to the length of the array.");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException("'count' cannot be less than zero.");
            }
            else if (start + count > array.Length)
            {
                throw new ArgumentOutOfRangeException("'start' + 'count' cannot be greater than the length of the array.");
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                if (i > 0 && separator != null)
                {
                    builder.Append(separator);
                }

                builder.Append(array[i + start]);
            }

            return builder.ToString();
        }

        private static int LastIndexOfBreakingCharInternal(this string text, int startIndex, bool strict, params char[] breakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("'startIndex' cannot be less than zero.");
            }
            else if (startIndex >= text.Length)
            {
                throw new ArgumentOutOfRangeException("'startIndex' cannot be greater than or equal to the length of the provided string.");
            }

            List<char> breakingCharsList = new List<char>(breakingChars);
            int lastIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];

                if (i <= startIndex && breakingCharsList.Contains(character))
                {
                    lastIndex = i;
                }

                if (strict)
                {
                    if (character == ' ')
                    {
                        if (i + 1 < text.Length)
                        {
                            char nextCharacter = text[i + 1];
                            if (nextCharacter == '"')
                            {
                                i++;

                                while (i + 1 < text.Length)
                                {
                                    i++;

                                    character = text[i];
                                    if (character == '"')
                                    {
                                        if (i + 1 >= text.Length || (i + 1 < text.Length && text[i + 1] == ' '))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (character == '"')
                    {
                        while (i + 1 < text.Length)
                        {
                            i++;

                            character = text[i];
                            if (character == '"')
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return lastIndex;
        }

        private static int IndexOfNextBreakingCharInternal(this string text, int startIndex, int count, bool strict, out char? breakChar, params char[] otherBreakingChars)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("'startIndex' cannot be less than zero.");
            }
            else if (count < 0)
            {
                throw new ArgumentOutOfRangeException("'count' cannot be less than zero.");
            }
            else if (startIndex + count > text.Length)
            {
                throw new ArgumentOutOfRangeException("'startIndex' + 'count' cannot be greater than the length of the provided string.");
            }

            breakChar = null;

            List<char> otherBreakingCharsList = new List<char>(otherBreakingChars);

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];

                if (i >= startIndex && otherBreakingCharsList.Contains(character))
                {
                    breakChar = character;
                    return i;
                }
                else if (i >= startIndex + count)
                {
                    return -1;
                }

                if (!strict)
                {
                    if (character == ' ')
                    {
                        if (i + 1 < text.Length)
                        {
                            char nextCharacter = text[i + 1];
                            if (nextCharacter == '"')
                            {
                                i++;

                                while (i + 1 < text.Length)
                                {
                                    i++;

                                    character = text[i];
                                    if (character == '"' && text.CountReverseCharRun('\\', i - 1) % 2 == 0)
                                    {
                                        if (i + 1 >= text.Length || (i + 1 < text.Length && text[i + 1] == ' '))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (character == '"')
                    {
                        while (i + 1 < text.Length)
                        {
                            i++;

                            character = text[i];
                            if (character == '"' && text.CountReverseCharRun('\\', i - 1) % 2 == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        ////public static void TestBreakApart()
        ////{
        ////    Test("system.datetime.now.tostring()".BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.'), new string[] {"system", "datetime", "now", "tostring()" });
        ////    Test("system.datetime.now.".BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.'), new string[] { "system", "datetime", "now"});
        ////    Test("this param & that".BreakApart(Quotes.Include, Spaces.DoNotBreak, BreakingCharAction.Eat, '&'), new string[] { "this param ", " that"});
        ////    Test("command \"arg\\\" hi\" this".BreakApart(), new string[] { "command", "arg\" hi", "this"});
        ////    Test("command \"arg\\\" hi\" this".BreakApart(Quotes.Include, Spaces.Break), new string[] { "command", "\"arg\\\" hi\"", "this" });
        ////    Test("command \"arg\\\" hi\"".BreakApart(), new string[] { "command", "arg\" hi" });
        ////    Test("command hi".BreakApart(), new string[] { "command", "hi" });
        ////}

        ////private static void Test(string[] brokenApart, string[] expected)
        ////{
        ////    if (brokenApart.Length < expected.Length)
        ////    {
        ////        throw new ArgumentException("brokenApart too small");
        ////    }
        ////    else if (brokenApart.Length > expected.Length)
        ////    {
        ////        throw new ArgumentException("brokenApart too large");
        ////    }

        ////    for (int i = 0; i < expected.Length; i++)
        ////    {
        ////        if (brokenApart[i] != expected[i])
        ////        {
        ////            throw new ArgumentException("wrong value");
        ////        }
        ////    }
        ////}
    }
}
