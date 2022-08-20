using System;
using System.Collections.Generic;
using System.Text;
using System.Extensions;
using ZachJohnson.Promptu.UserModel;
using System.IO;

namespace ZachJohnson.Promptu.Skins
{
    internal static class SuggestionUtilities
    {
        public static string QuotizeCommandExecution(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (SuggestionUtilities.IsFilepath(text))
            {
                return text;
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                string[] split = Command.ExtractSimpleNameAndParametersFrom(text);
                for (int i = 0; i < split.Length; i++)
                {
                    if (i > 0)
                    {
                        builder.AppendFormat(" \"{0}\"", split[i].Escape());
                    }
                    else
                    {
                        builder.Append(split[i]);
                    }
                }

                return builder.ToString();
            }
        }

        public static string ReplaceCommandParameter(
            string text, 
            int parameterIndex, 
            string parameterTextLookingFor, 
            string replacementText)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (parameterIndex < 0)
            {
                throw new ArgumentOutOfRangeException("parameterIndex");
            }
            else if (parameterTextLookingFor == null)
            {
                throw new ArgumentNullException("parameterTextLookingFor");
            }
            else if (replacementText == null)
            {
                throw new ArgumentNullException("replacementText");
            }

            StringBuilder builder = new StringBuilder();

            StringBuilder currentParameter = new StringBuilder();
            int currentParameterIndex = -1;

            bool justAppended = false;
            bool justAddedToQuote = false;

            for (int i = 0; i < text.Length; i++)
            {
                bool ignoreSpaceThisTime = justAddedToQuote;
                justAddedToQuote = false;
                justAppended = false;
                char character = text[i];

                if (character == ' ')
                {
                    string currentParameterToString;

                    if (!ignoreSpaceThisTime)
                    {
                        currentParameterToString = currentParameter.ToString();
                        if (currentParameterIndex == parameterIndex && currentParameterToString == parameterTextLookingFor)
                        {
                            builder.Append(replacementText);
                        }
                        else
                        {
                            builder.Append(currentParameter);
                        }

                        currentParameter = new StringBuilder();
                        builder.Append(character);
                        currentParameterIndex++;
                        justAppended = true;
                    }
                    else
                    {
                        builder.Append(character);
                    }

                    if (i + 1 < text.Length)
                    {
                        char nextChar = text[i + 1];
                        if (nextChar == '"')
                        {
                            i++;
                            currentParameter.Append(nextChar);
                            while (i + 1 < text.Length)
                            {
                                justAppended = false;
                                i++;
                                char innerChar = text[i];

                                currentParameter.Append(innerChar);

                                if (innerChar == '"' && (text.CountReverseCharRun('\\', i - 1) % 2 == 0 || (i + 1 < text.Length && text[i + 1] == ' ')))
                                {
                                    currentParameterToString = currentParameter.ToString();
                                    if (currentParameterIndex == parameterIndex && currentParameterToString == parameterTextLookingFor)
                                    {
                                        builder.Append(replacementText);
                                    }
                                    else
                                    {
                                        builder.Append(currentParameter);
                                    }

                                    currentParameter = new StringBuilder();
                                    justAddedToQuote = true;
                                    currentParameterIndex++;
                                    justAppended = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    currentParameter.Append(character);
                }
            }

            if (!justAppended)
            {
                string currentParameterToString = currentParameter.ToString();
                if (currentParameterIndex == parameterIndex && currentParameterToString == parameterTextLookingFor)
                {
                    builder.Append(replacementText);
                }
                else
                {
                    builder.Append(currentParameter);
                }
            }

            return builder.ToString();
        }

        public static string ReplaceFunctionParameterIfMatch(
            string text,
            int totalParameterCount,
            int parameterIndex,
            string parameterTextLookingFor,
            string replacementText)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (totalParameterCount < 0)
            {
                throw new ArgumentOutOfRangeException("totalParameterCount");
            }
            else if (parameterIndex < 0)
            {
                throw new ArgumentOutOfRangeException("parameterIndex");
            }
            else if (parameterTextLookingFor == null)
            {
                throw new ArgumentNullException("parameterTextLookingFor");
            }
            else if (replacementText == null)
            {
                throw new ArgumentNullException("replacementText");
            }

            int numberOfParameters = 0;
            int level = -1;

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char? breakChar;
                int indexOfNextBreakChar = text.IndexOfNextBreakingChar(i, text.Length - i, true, out breakChar, '(', ',', ')');

                bool breakCharIsOpenParentheses = breakChar == '(';

                if (breakCharIsOpenParentheses)
                {
                    level++;
                }

                if (indexOfNextBreakChar == -1)
                {
                    indexOfNextBreakChar = text.Length;
                }

                i--;

                StringBuilder currentParameter = new StringBuilder();

                while (i + 1 < indexOfNextBreakChar)
                {
                    i++;
                    currentParameter.Append(text[i]);
                    
                }

                i++;

                //if (currentParameter.Length == 0)
                //{
                    //i++;
                //}

                if (level == 0)
                {
                    string currentParameterValue = currentParameter.ToString().Trim();

                    if (numberOfParameters == parameterIndex + 1 && currentParameterValue == parameterTextLookingFor)
                    {
                        builder.Append(replacementText);
                    }
                    else
                    {
                        builder.Append(currentParameterValue);
                    }

                    //if (breakChar == '(' || breakChar == ',')
                    //{
                        numberOfParameters++;
                    //}
                }
                else
                {
                    builder.Append(currentParameter);
                }

                builder.Append(breakChar);

                if (breakChar == ')')
                {
                    level--;
                }
            }

            if (numberOfParameters == totalParameterCount + 1)
            {
                return builder.ToString();
            }
            else
            {
                return text;
            }
        }

        public static bool IsConsideredComplex(string text)
        {
            bool isComplex = !InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(text);
            if (isComplex)
            {
                if (Function.IsInFunctionSyntax(text))
                {
                    //isComplex = Function.ExtractNameAndParametersFrom(text).Length > 1;
                    string[] parsedText = Function.ExtractNameAndParametersFrom(text);
                    //isComplex = parsedText.Length != 2 || parsedText[1].Length > 0;
                    isComplex = parsedText.Length != 1;
                }
            }

            return isComplex;
        }

        public static bool IsFilepath(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (Char.IsWhiteSpace(c) || c == '(')
                {
                    break;
                }
                else if (c == Path.DirectorySeparatorChar)
                {
                    return true;
                }
            }

            return false;
        }

        public static string[] ExtractNameAndParametersFrom(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (Function.IsInFunctionSyntax(text))
            {
                return Function.ExtractNameAndParametersFrom(text);
            }
            else
            {
                return Command.ExtractNameAndParametersFrom(text);
            }
        }

        public static string[] ExtractSimpleNameAndParametersFrom(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (Function.IsInFunctionSyntax(text))
            {
                return Function.ExtractNameAndParametersFrom(text);
            }
            else
            {
                return Command.ExtractSimpleNameAndParametersFrom(text);
            }
        }

        public static string GetWhatFilterShouldBe(string text, int indexInText, ParameterHelpContext contextInfo)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (indexInText > text.Length)
            {
                throw new ArgumentOutOfRangeException("'indexInText' cannot be greater than 'text.Length'.");
            }

            int length;
            int start;

            if (Function.IsInFunctionSyntax(text))
            {
                start = GetIndexOfSuggestionAreaFunctionSyntax(text, indexInText, contextInfo, out length);
            }
            else
            {
                start = GetIndexOfSuggestionAreaNonFunctionSyntax(text, indexInText, contextInfo, out length);
            }

            return text.Substring(0, start);
        }

        public static bool IsBreakingChar(char c)
        {
            switch (c)
            {
                case ',':
                case '"':
                case '.':
                case '(':
                case ')':
                case ' ':
                    return true;
                default:
                    return c == Path.DirectorySeparatorChar;
            }
        }

        public static int GetIndexOfBlockToRemove(string text, int indexInText, ParameterHelpContext contextInfo, out int length)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (indexInText > text.Length)
            {
                throw new ArgumentOutOfRangeException("'indexInText' cannot be greater than 'text.Length'.");
            }

            if (Function.IsInFunctionSyntax(text))
            {
                return GetIndexOfBlockToRemoveFunctionSyntax(text, indexInText, contextInfo, out length);
            }
            else
            {
                return GetIndexOfBlockToRemoveNonFunctionSyntax(text, indexInText, contextInfo, out length);
            }
        }

        public static int GetIndexOfSuggestionArea(string text, int indexInText, ParameterHelpContext contextInfo, out int length)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (indexInText > text.Length)
            {
                throw new ArgumentOutOfRangeException("'indexInText' cannot be greater than 'text.Length'.");
            }

            if (Function.IsInFunctionSyntax(text))
            {
                return GetIndexOfSuggestionAreaFunctionSyntax(text, indexInText, contextInfo, out length);
            }
            else
            {
                return GetIndexOfSuggestionAreaNonFunctionSyntax(text, indexInText, contextInfo, out length);
            }
        }

        public static string GetSuggestionArea(string text, int indexInText, ParameterHelpContext contextInfo)
        {
            int length;
            int index = GetIndexOfSuggestionArea(text, indexInText, contextInfo, out length);

            return text.Substring(index, length);
        }

        public static int GetStartOfCurrentLevel(string text, int indexInText)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (indexInText > text.Length)
            {
                throw new ArgumentOutOfRangeException("'indexInText' cannot be greater than 'text.Length'.");
            }
            else if (indexInText < 0)
            {
                throw new ArgumentOutOfRangeException("'indexInText' cannot be greater than 'text.Length'.");
            }
            else if (!Function.IsInFunctionSyntax(text))
            {
                return 0;
            }

            List<int> levelBeginnings = new List<int>();
            levelBeginnings.Add(0);
            int indexOfCurentAreaStart = 0;
            char character;

            int level = -1;

            for (int i = 0; i < text.Length; i++)
            {
                character = text[i];

                //int levelOffset = 0;

                switch (character)
                {
                    case ' ':
                        if (i < indexInText)
                        {
                            indexOfCurentAreaStart = i + 1;
                        }

                        break;
                    case '(':
                        //levelOffset = -1;
                        level++;

                        for (int j = level; levelBeginnings.Count <= level; j++)
                        {
                            levelBeginnings.Add(0);
                        }

                        if (level >= 0)
                        {
                            levelBeginnings[level] = indexOfCurentAreaStart;
                        }

                        if (i < indexInText)
                        {
                            indexOfCurentAreaStart = i + 1;
                        }

                        break;
                    case ')':
                        //levelOffset = 1;
                        level--;
                        if (i < indexInText)
                        {
                            indexOfCurentAreaStart = i + 1;
                        }

                        if (level == -1)
                        {
                            levelBeginnings[0] = indexOfCurentAreaStart;
                        }

                        break;
                    case '"':
                        if (i < indexInText)
                        {
                            indexOfCurentAreaStart = i + 1;
                        }

                        i++;

                        while (i < text.Length)
                        {
                            if (text[i] == '"' && (text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                            {
                                if (i <= indexInText)
                                {
                                    indexOfCurentAreaStart = i + 1;
                                }

                                break;
                            }

                            i++;
                        }

                        break;
                    default:
                        break;
                }

                if (i >= indexInText - 1)
                {
                    //level = level + levelOffset;

                    break;
                }
            }

            if (level < 0)
            {
                level = 0;
            }

            return levelBeginnings[level];
        }

        private static int GetIndexOfSuggestionAreaFunctionSyntax(string text, int indexInText, ParameterHelpContext contextInfo, out int length)
        {
            int indexOfSuggestionAreaStart = 0;
            char character;

            int level = -1;

            for (int i = 0; i < text.Length; i++)
            {
                character = text[i];

                switch (character)
                {
                    case ' ':
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        break;
                    case '(':
                        if (i < indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        level++;

                        if (level > 0)
                        {
                            i++;

                            while (i < text.Length)
                            {
                                character = text[i];
                                if (character == '(')
                                {
                                    level++;
                                }
                                if (character == ')')
                                {
                                    level--;
                                    if (level == 0)
                                    {
                                        break;
                                    }
                                }

                                i++;
                            }
                        }

                        break;
                    case ')':
                    case ',':
                    case '.':
                        if (i < indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        break;
                    case '"':
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        i++;

                        bool breakout = false;

                        bool seenFirstDirectorySeparatorChar = false;
                        while (!breakout && i < text.Length)
                        {
                            if (contextInfo != null && contextInfo.CurrentParameterRequiresInQuoteProcessing)
                            {
                                char innerChar = text[i];
                                switch (innerChar)
                                {
                                    case '\\':
                                        if (i + 1 < text.Length)
                                        {
                                            i++;
                                            char escapedChar = text[i];
                                            switch (escapedChar)
                                            {
                                                case '\\':
                                                    if (contextInfo.CurrentParameterAllowsFileSystem && Path.DirectorySeparatorChar == '\\')
                                                    {
                                                        seenFirstDirectorySeparatorChar = true;
                                                        if (i <= indexInText)
                                                        {
                                                            indexOfSuggestionAreaStart = i + 1;
                                                        }
                                                        else
                                                        {
                                                            length = i - indexOfSuggestionAreaStart;
                                                            return indexOfSuggestionAreaStart;
                                                        }
                                                    }

                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        break;
                                    case '"':
                                        //if ((text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                                        //{
                                            if (i <= indexInText)
                                            {
                                                indexOfSuggestionAreaStart = i + 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }

                                            breakout = true;
                                        //}

                                        break;
                                    case '.':
                                        if (!contextInfo.CurrentParameterAllowsFileSystem || !seenFirstDirectorySeparatorChar)
                                        {
                                            if (i <= indexInText)
                                            {
                                                indexOfSuggestionAreaStart = i + 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }
                                        }

                                        break;
                                    default:
                                        if (contextInfo.CurrentParameterAllowsFileSystem && innerChar == Path.DirectorySeparatorChar)
                                        {
                                            seenFirstDirectorySeparatorChar = true;
                                            if (i <= indexInText)
                                            {
                                                indexOfSuggestionAreaStart = i + 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }
                                        }

                                        break;
                                }
                            }
                            else
                            {
                                if (text[i] == '"' && (text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                                {
                                    if (i <= indexInText)
                                    {
                                        indexOfSuggestionAreaStart = i + 1;
                                    }
                                    else
                                    {
                                        length = i - indexOfSuggestionAreaStart;
                                        return indexOfSuggestionAreaStart;
                                    }

                                    break;
                                }
                            }

                            if (!breakout)
                            {
                                i++;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            length = text.Length - indexOfSuggestionAreaStart;
            return indexOfSuggestionAreaStart;
        }

        private static int GetIndexOfSuggestionAreaNonFunctionSyntax(string text, int indexInText, ParameterHelpContext contextInfo, out int length)
        {
            int indexOfSuggestionAreaStart = 0;
            char character;

            bool isFilepath = IsFilepath(text);
            bool allowPaths = (contextInfo != null && contextInfo.CurrentParameterAllowsFileSystem)
                    || (contextInfo == null && isFilepath);

            bool seenFirstDirectorySeparatorChar = false;
            for (int i = 0; i < text.Length; i++)
            {
                character = text[i];

                if (character == '.' && (contextInfo == null || contextInfo.CurrentParameterRequiresInQuoteProcessing))
                {
                    if (!allowPaths || !seenFirstDirectorySeparatorChar)
                    {
                        if (i < indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }
                    }
                }
                else if (character == Path.DirectorySeparatorChar)
                {
                    if (allowPaths)
                    {
                        seenFirstDirectorySeparatorChar = true;
                        if (i < indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }
                    }
                }
                else if (character == ' ')
                {
                    if (!isFilepath)
                    {
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i + 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }
                    }

                    if (i + 1 < text.Length)
                    {
                        if (text[i + 1] == '"')
                        {
                            i++;

                            if (i <= indexInText)
                            {
                                indexOfSuggestionAreaStart = i + 1;
                            }
                            else
                            {
                                length = i - indexOfSuggestionAreaStart;
                                return indexOfSuggestionAreaStart;
                            }

                            i++;

                            bool breakout = false;

                            while (!breakout && i < text.Length)
                            {
                                if (contextInfo != null && contextInfo.CurrentParameterRequiresInQuoteProcessing)
                                {
                                    char innerChar = text[i];
                                    switch (innerChar)
                                    {
                                        case '\\':
                                            if (i + 1 < text.Length)
                                            {
                                                i++;
                                                char escapedChar = text[i];
                                                switch (escapedChar)
                                                {
                                                    case '\\':
                                                        if (contextInfo.CurrentParameterAllowsFileSystem && Path.DirectorySeparatorChar == '\\')
                                                        {
                                                            seenFirstDirectorySeparatorChar = true;
                                                            if (i <= indexInText)
                                                            {
                                                                indexOfSuggestionAreaStart = i + 1;
                                                            }
                                                            else
                                                            {
                                                                length = i - indexOfSuggestionAreaStart;
                                                                return indexOfSuggestionAreaStart;
                                                            }
                                                        }

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            break;
                                        case '"':
                                            //if ((text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                                            //{
                                            if (i <= indexInText)
                                            {
                                                indexOfSuggestionAreaStart = i + 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }

                                            breakout = true;
                                            //}

                                            break;
                                        case '.':
                                            if (!contextInfo.CurrentParameterAllowsFileSystem || !seenFirstDirectorySeparatorChar)
                                            {
                                                if (i <= indexInText)
                                                {
                                                    indexOfSuggestionAreaStart = i + 1;
                                                }
                                                else
                                                {
                                                    length = i - indexOfSuggestionAreaStart;
                                                    return indexOfSuggestionAreaStart;
                                                }
                                            }

                                            break;
                                        default:
                                            if (contextInfo.CurrentParameterAllowsFileSystem && innerChar == Path.DirectorySeparatorChar)
                                            {
                                                seenFirstDirectorySeparatorChar = true;
                                                if (i <= indexInText)
                                                {
                                                    indexOfSuggestionAreaStart = i + 1;
                                                }
                                                else
                                                {
                                                    length = i - indexOfSuggestionAreaStart;
                                                    return indexOfSuggestionAreaStart;
                                                }
                                            }

                                            break;
                                    }
                                }
                                else
                                {
                                    if (text[i] == '"' && (text.CountReverseCharRun('\\', i - 1) % 2 == 0 || (i + 1 < text.Length && text[i + 1] == ' ')))
                                    {
                                        if (i <= indexInText)
                                        {
                                            indexOfSuggestionAreaStart = i + 1;
                                        }
                                        else
                                        {
                                            length = i - indexOfSuggestionAreaStart;
                                            return indexOfSuggestionAreaStart;
                                        }

                                        break;
                                    }
                                }

                                if (!breakout)
                                {
                                    i++;
                                }
                            }

                            //if (i <= indexInText)
                            //{
                            //    indexOfSuggestionAreaStart = i + 1;
                            //}
                            //else
                            //{
                            //    length = i - indexOfSuggestionAreaStart;
                            //    return indexOfSuggestionAreaStart;
                            //}

                            //while (i < text.Length)
                            //{
                            //    if (text[i] == '"' && (text[i - 1] != '\\' || (i + 1 < text.Length && text[i + 1] == ' ')))
                            //    {
                            //        break;
                            //    }

                            //    i++;
                            //}
                        }
                    }
                }
            }

            length = text.Length - indexOfSuggestionAreaStart;
            return indexOfSuggestionAreaStart;
        }

        private static int GetIndexOfBlockToRemoveFunctionSyntax(string text, int indexInText, ParameterHelpContext contextInfo, out int length)
        {
            int indexOfSuggestionAreaStart = 0;
            char character;

            int level = -1;

            int nextIndexOfSuggestionAreaStart = 0;
            int charsTillNextIndexApplied = -1;

            for (int i = 0; i < text.Length; i++)
            {
                character = text[i];

                charsTillNextIndexApplied--;

                switch (character)
                {
                    case ' ':
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i;

                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        break;
                    case '(':
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i;

                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        level++;

                        if (level > 0)
                        {
                            i++;

                            while (i < text.Length)
                            {
                                charsTillNextIndexApplied--;
                                character = text[i];
                                if (character == '(')
                                {
                                    level++;
                                }
                                if (character == ')')
                                {
                                    level--;
                                    if (level == 0)
                                    {
                                        break;
                                    }
                                }

                                i++;

                                if (charsTillNextIndexApplied == 0)
                                {
                                    charsTillNextIndexApplied = -1;
                                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                }
                            }

                            if (charsTillNextIndexApplied == 0)
                            {
                                charsTillNextIndexApplied = -1;
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }
                        }

                        break;
                    case ')':
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i;
                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        break;
                    case ',':
                    case '.':
                        if (i <= indexInText)
                        {
                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        break;
                    case '"':
                        if (i <= indexInText)
                        {
                            indexOfSuggestionAreaStart = i;

                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }

                        //charsTillNextIndexApplied--;
                        i++;

                        bool breakout = false;

                        bool seenFirstDirectorySeparatorChar = false;
                        while (!breakout && i < text.Length)
                        {
                            charsTillNextIndexApplied--;
                            if (contextInfo != null && contextInfo.CurrentParameterRequiresInQuoteProcessing)
                            {
                                char innerChar = text[i];
                                switch (innerChar)
                                {
                                    case '\\':
                                        if (i + 1 < text.Length)
                                        {
                                            i++;
                                            char escapedChar = text[i];
                                            switch (escapedChar)
                                            {
                                                case '\\':
                                                    if (contextInfo.CurrentParameterAllowsFileSystem && Path.DirectorySeparatorChar == '\\')
                                                    {
                                                        seenFirstDirectorySeparatorChar = true;
                                                        if (i <= indexInText)
                                                        {
                                                            if (charsTillNextIndexApplied >= 0)
                                                            {
                                                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                            }

                                                            nextIndexOfSuggestionAreaStart = i + 1;
                                                            charsTillNextIndexApplied = 1;
                                                        }
                                                        else
                                                        {
                                                            length = i - indexOfSuggestionAreaStart;
                                                            return indexOfSuggestionAreaStart;
                                                        }
                                                    }

                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        break;
                                    case '"':
                                        //if ((text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                                        //{
                                        if (i <= indexInText)
                                        {
                                            if (charsTillNextIndexApplied >= 0)
                                            {
                                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                            }

                                            nextIndexOfSuggestionAreaStart = i + 1;
                                            charsTillNextIndexApplied = 1;
                                        }
                                        else
                                        {
                                            length = i - indexOfSuggestionAreaStart;
                                            return indexOfSuggestionAreaStart;
                                        }

                                        breakout = true;
                                        //}

                                        break;
                                    case '.':
                                        if (!contextInfo.CurrentParameterAllowsFileSystem || !seenFirstDirectorySeparatorChar)
                                        {
                                            if (i <= indexInText)
                                            {
                                                if (charsTillNextIndexApplied >= 0)
                                                {
                                                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                }

                                                nextIndexOfSuggestionAreaStart = i + 1;
                                                charsTillNextIndexApplied = 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }
                                        }

                                        break;
                                    default:
                                        if (contextInfo.CurrentParameterAllowsFileSystem && innerChar == Path.DirectorySeparatorChar)
                                        {
                                            seenFirstDirectorySeparatorChar = true;
                                            if (i <= indexInText)
                                            {
                                                if (charsTillNextIndexApplied >= 0)
                                                {
                                                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                }

                                                nextIndexOfSuggestionAreaStart = i + 1;
                                                charsTillNextIndexApplied = 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }
                                        }

                                        break;
                                }
                            }
                            else
                            {
                                if (text[i] == '"' && (text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                                {
                                    if (i <= indexInText)
                                    {
                                        if (charsTillNextIndexApplied >= 0)
                                        {
                                            indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                        }

                                        nextIndexOfSuggestionAreaStart = i + 1;
                                        charsTillNextIndexApplied = 1;
                                    }
                                    else
                                    {
                                        length = i - indexOfSuggestionAreaStart;
                                        return indexOfSuggestionAreaStart;
                                    }

                                    break;
                                }
                            }

                            if (!breakout)
                            {
                                i++;
                            }

                            if (charsTillNextIndexApplied == 0)
                            {
                                charsTillNextIndexApplied = -1;
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }
                        }

                        break;
                    default:
                        break;
                }

                if (charsTillNextIndexApplied == 0)
                {
                    charsTillNextIndexApplied = -1;
                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                }
            }

            length = text.Length - indexOfSuggestionAreaStart;
            return indexOfSuggestionAreaStart;
        }

        private static int GetIndexOfBlockToRemoveNonFunctionSyntax(string text, int indexInText, ParameterHelpContext contextInfo, out int length)
        {
            int indexOfSuggestionAreaStart = 0;
            char character;

            bool allowPaths = (contextInfo != null && contextInfo.CurrentParameterAllowsFileSystem)
                    || (contextInfo == null && IsFilepath(text));

            bool seenFirstDirectorySeparatorChar = false;
            int nextIndexOfSuggestionAreaStart = 0;
            int charsTillNextIndexApplied = -1;

            for (int i = 0; i < text.Length; i++)
            {
                character = text[i];

                charsTillNextIndexApplied--;

                if (character == '.')
                {
                    if (!allowPaths || !seenFirstDirectorySeparatorChar)
                    {
                        if (i <= indexInText)
                        {
                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }
                    }
                }
                else if (character == Path.DirectorySeparatorChar)
                {
                    if (allowPaths)
                    {
                        seenFirstDirectorySeparatorChar = true;
                        if (i <= indexInText)
                        {
                            if (charsTillNextIndexApplied >= 0)
                            {
                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                            }

                            nextIndexOfSuggestionAreaStart = i + 1;
                            charsTillNextIndexApplied = 1;
                        }
                        else
                        {
                            length = i - indexOfSuggestionAreaStart;
                            return indexOfSuggestionAreaStart;
                        }
                    }
                }
                else if (character == ' ')
                {
                    if (i <= indexInText)
                    {
                        indexOfSuggestionAreaStart = i;

                        if (charsTillNextIndexApplied >= 0)
                        {
                            indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                        }

                        nextIndexOfSuggestionAreaStart = i + 1;
                        charsTillNextIndexApplied = 1;
                    }
                    else
                    {
                        length = i - indexOfSuggestionAreaStart;
                        return indexOfSuggestionAreaStart;
                    }

                    if (i + 1 < text.Length)
                    {
                        if (text[i + 1] == '"')
                        {
                            charsTillNextIndexApplied--;
                            i++;

                            if (i <= indexInText)
                            {
                                //if (charsTillNextIndexApplied >= 0)
                                //{
                                //    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                //}

                                nextIndexOfSuggestionAreaStart = i + 1;
                                charsTillNextIndexApplied = 1;
                            }
                            else
                            {
                                length = i - indexOfSuggestionAreaStart;
                                return indexOfSuggestionAreaStart;
                            }

                            //charsTillNextIndexApplied--;
                            i++;

                            bool breakout = false;

                            while (!breakout && i < text.Length)
                            {
                                charsTillNextIndexApplied--;
                                if (contextInfo != null && contextInfo.CurrentParameterRequiresInQuoteProcessing)
                                {
                                    char innerChar = text[i];
                                    switch (innerChar)
                                    {
                                        case '\\':
                                            if (i + 1 < text.Length)
                                            {
                                                charsTillNextIndexApplied--;
                                                i++;
                                                char escapedChar = text[i];
                                                switch (escapedChar)
                                                {
                                                    case '\\':
                                                        if (contextInfo.CurrentParameterAllowsFileSystem && Path.DirectorySeparatorChar == '\\')
                                                        {
                                                            seenFirstDirectorySeparatorChar = true;
                                                            if (i <= indexInText)
                                                            {
                                                                if (charsTillNextIndexApplied >= 0)
                                                                {
                                                                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                                }

                                                                nextIndexOfSuggestionAreaStart = i + 1;
                                                                charsTillNextIndexApplied = 1;
                                                            }
                                                            else
                                                            {
                                                                length = i - indexOfSuggestionAreaStart;
                                                                return indexOfSuggestionAreaStart;
                                                            }
                                                        }

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            break;
                                        case '"':
                                            //if ((text.CountReverseCharRun('\\', i - 1) % 2) == 0)
                                            //{
                                            if (i <= indexInText)
                                            {
                                                if (charsTillNextIndexApplied >= 0)
                                                {
                                                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                }

                                                nextIndexOfSuggestionAreaStart = i + 1;
                                                charsTillNextIndexApplied = 1;
                                            }
                                            else
                                            {
                                                length = i - indexOfSuggestionAreaStart;
                                                return indexOfSuggestionAreaStart;
                                            }

                                            breakout = true;
                                            //}

                                            break;
                                        case '.':
                                            if (!contextInfo.CurrentParameterAllowsFileSystem || !seenFirstDirectorySeparatorChar)
                                            {
                                                if (i <= indexInText)
                                                {
                                                    if (charsTillNextIndexApplied >= 0)
                                                    {
                                                        indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                    }

                                                    nextIndexOfSuggestionAreaStart = i + 1;
                                                    charsTillNextIndexApplied = 1;
                                                }
                                                else
                                                {
                                                    length = i - indexOfSuggestionAreaStart;
                                                    return indexOfSuggestionAreaStart;
                                                }
                                            }

                                            break;
                                        default:
                                            if (contextInfo.CurrentParameterAllowsFileSystem && innerChar == Path.DirectorySeparatorChar)
                                            {
                                                seenFirstDirectorySeparatorChar = true;
                                                if (i <= indexInText)
                                                {
                                                    if (charsTillNextIndexApplied >= 0)
                                                    {
                                                        indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                                    }

                                                    nextIndexOfSuggestionAreaStart = i + 1;
                                                    charsTillNextIndexApplied = 1;
                                                }
                                                else
                                                {
                                                    length = i - indexOfSuggestionAreaStart;
                                                    return indexOfSuggestionAreaStart;
                                                }
                                            }

                                            break;
                                    }
                                }
                                else
                                {
                                    if (text[i] == '"' && (text[i - 1] != '\\' || (i + 1 < text.Length && text[i + 1] == ' ')))
                                    {
                                        if (i <= indexInText)
                                        {
                                            if (charsTillNextIndexApplied >= 0)
                                            {
                                                indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                            }

                                            nextIndexOfSuggestionAreaStart = i + 1;
                                            charsTillNextIndexApplied = 1;
                                        }
                                        else
                                        {
                                            length = i - indexOfSuggestionAreaStart;
                                            return indexOfSuggestionAreaStart;
                                        }

                                        break;
                                    }
                                }

                                if (!breakout)
                                {
                                    i++;
                                }

                                if (charsTillNextIndexApplied == 0)
                                {
                                    charsTillNextIndexApplied = -1;
                                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                                }
                            }

                            //if (i <= indexInText)
                            //{
                            //    indexOfSuggestionAreaStart = i + 1;
                            //}
                            //else
                            //{
                            //    length = i - indexOfSuggestionAreaStart;
                            //    return indexOfSuggestionAreaStart;
                            //}

                            //while (i < text.Length)
                            //{
                            //    if (text[i] == '"' && (text[i - 1] != '\\' || (i + 1 < text.Length && text[i + 1] == ' ')))
                            //    {
                            //        break;
                            //    }

                            //    i++;
                            //}
                        }
                    }
                }

                if (charsTillNextIndexApplied == 0)
                {
                    charsTillNextIndexApplied = -1;
                    indexOfSuggestionAreaStart = nextIndexOfSuggestionAreaStart;
                }
            }

            length = text.Length - indexOfSuggestionAreaStart;
            return indexOfSuggestionAreaStart;
        }

        public static string GetItemNameFrom(string filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            char? breakingChar;
            int indexOfBreak = filter.IndexOfNextBreakingChar(true, out breakingChar, '(', ' ');
            if (indexOfBreak > -1 && breakingChar != null)
            {
                return filter.Substring(0, indexOfBreak);
            }

            return null;
        }

        public static int GetCurrentParameterIndex(string text, int locationInText)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (locationInText < 0)
            {
                throw new ArgumentOutOfRangeException("'locationInText' cannot be less than zero.");
            }
            else if (locationInText > text.Length)
            {
                throw new ArgumentOutOfRangeException("'locationInText' cannot be greater than 'text.Length'.");
            }

            if (Function.IsInFunctionSyntax(text))
            {
                return GetCurrentParameterIndexFromFunctionSyntax(text, locationInText);
            }
            else
            {
                return GetCurrentParameterIndexFromNonFunctionSyntax(text, locationInText);
            }
        }

        private static int GetCurrentParameterIndexFromNonFunctionSyntax(this string text, int locationInText)
        {
            int parameterIndex = -1;

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];

                if (character == ' ')
                {
                    parameterIndex++;

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
                                    if (i + 1 >= text.Length || (text[i-1] != '\\' && i + 1 < text.Length && text[i + 1] == ' '))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (i >= locationInText - 1)
                {
                    break;
                }
            }

            return parameterIndex;

            //bool nextCharIsNewArgument = false;
            //List<char> breakingCharsList = new List<char>(breakingChars);
            //int breakCount = 0;
            //for (int i = 0; i < text.Length; i++)
            //{
            //    char character = text[i];
            //    if (character == ' ')
            //    {
            //        if (i + 1 < text.Length)
            //        {
            //            char nextCharacter = text[i + 1];
            //            if (nextCharacter == '"')
            //            {
            //                breakCount++;
            //                i++;

            //                while (i + 1 < text.Length)
            //                {
            //                    i++;

            //                    character = text[i];
            //                    if (character == '"')
            //                    {
            //                        if (i + 1 >= text.Length || (i + 1 < text.Length && text[i + 1] == ' '))
            //                        {
            //                            break;
            //                        }
            //                    }
            //                }
            //            }
            //            else if (nextCharacter != ' ')
            //            {
            //                breakCount++;
            //            }
            //        }
            //        else
            //        {
            //            breakCount++;
            //        }
            //    }
            //    else if (otherBreakingCharsList.Contains(character))
            //    {
            //        breakCount++;
            //    }
            //}

            //return breakCount;
        }

        private static int GetCurrentParameterIndexFromFunctionSyntax(this string text, int locationInText)
        {
            int parameterIndex = -1;

            int currentLevel = -1;

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];

                switch (character)
                {
                    case '(':
                        currentLevel++;
                        if (currentLevel == 0)
                        {
                            parameterIndex++;
                        }

                        break;
                    case ')':
                        currentLevel--;
                        if (currentLevel == -1)
                        {
                            parameterIndex = -1;
                        }

                        break;
                    case ',':
                        if (currentLevel == 0)
                        {
                            parameterIndex++;
                        }

                        break;
                    case '"':
                        while (i + 1 < text.Length)
                        {
                            i++;
                            character = text[i];
                            if (character == '"')
                            {
                                break;
                            }
                        }

                        break;
                    default:
                        break;
                }


                if (i >= locationInText - 1)
                {
                    break;
                }
            }

            return parameterIndex;
        }
    }
}
