using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Extensions;
using ZachJohnson.Promptu.UserModel;
using System.IO;
using ZachJohnson.Promptu.UserModel.Collections;
using System.Globalization;

namespace ZachJohnson.Promptu.Skins
{
    internal class SuggestionProvider
    {
        private static string PathSeparatorString = new string(Path.DirectorySeparatorChar, 1);
        private static char[] PathSeparatorCharArray = new char[] { Path.DirectorySeparatorChar };
        private PopulationInfo populationInfo;
        private PromptHandler.SeparateSuggestionHandler suggestionHandler;

        public SuggestionProvider(PromptHandler.SeparateSuggestionHandler suggestionHandler)
        {
            if (suggestionHandler == null)
            {
                throw new ArgumentNullException("suggestionHandler");
            }

            this.suggestionHandler = suggestionHandler;
        }

        public PopulationInfo PopulationInfo
        {
            get { return this.populationInfo; }
            set { this.populationInfo = value; }
        }

        public int SuggestIndex(string suggestionArea, int cursorIndexInPartTyped, ParameterHelpContext mainContext)
        {
            int index = -1;
            if (this.populationInfo != null)
            {
                int lengthOfSuggestion;
                int indexOfSuggestionArea = SuggestionUtilities.GetIndexOfSuggestionArea(suggestionArea, cursorIndexInPartTyped, mainContext, out lengthOfSuggestion);

                string partTypedOfSuggestion = suggestionArea.Substring(indexOfSuggestionArea, lengthOfSuggestion);

                if (this.suggestionHandler.SuggestionMode == SuggestionMode.Normal)
                {
                    int parameterIndex = SuggestionUtilities.GetCurrentParameterIndex(suggestionArea, cursorIndexInPartTyped);

                    if (mainContext == null)
                    {
                        string fullPartTyped = suggestionArea.Substring(0, indexOfSuggestionArea + lengthOfSuggestion);
                        string historyMatch = InternalGlobals.CurrentProfile.History.TryFindKey(fullPartTyped, CaseSensitivity.Insensitive);

                        if (historyMatch != null && !this.PopulationInfo.ContainsItemName(partTypedOfSuggestion))
                        {
                            string historySuggestion = SuggestionUtilities.GetSuggestionArea(historyMatch, cursorIndexInPartTyped, mainContext);
                            index = this.PopulationInfo.TranslateToIndex(historySuggestion);
                        }
                    }
                    else
                    {
                        object currentItem = mainContext.GetCurrentItem();
                        Function function;
                        Command command;
                        if ((function = currentItem as Function) != null)
                        {
                            List<string> parameterizedPartTyped = new List<string>(SuggestionUtilities.ExtractNameAndParametersFrom(suggestionArea));
                            if (suggestionArea.EndsWith(" "))
                            {
                                parameterizedPartTyped.Add(String.Empty);
                            }

                            if (parameterIndex >= 0 && parameterIndex < parameterizedPartTyped.Count - 1)
                            {
                                string historyMatch = null;
                                string parameterPartTyped = parameterizedPartTyped[parameterIndex + 1];

                                if (parameterPartTyped.StartsWith("\""))
                                {
                                    int length = parameterPartTyped.Length - 1;
                                    if (length > 0)
                                    {
                                        if (parameterPartTyped.EndsWith("\"") && (parameterPartTyped.Length <= 1 || parameterPartTyped.CountReverseCharRun('\\', parameterPartTyped.Length - 2) % 2 == 0))
                                        {
                                            length--;
                                        }
                                    }

                                    parameterPartTyped = parameterPartTyped.Substring(1, length).Unescape();
                                    //partTypedOfSuggestion = parameterPartTyped;
                                }

                                FunctionHistoryCollection history = InternalGlobals.CurrentProfile.History.FunctionHistory;
                                bool found;
                                FunctionHistory functionHistory;
                                if ((functionHistory = history.TryGetItem(function.StringId, CaseSensitivity.Insensitive, out found)) != null && found)
                                {
                                    if (function.Parameters.Count > parameterIndex)
                                    {
                                        string lookupString;
                                        string prefixToRemove;

                                        bool inQuote = suggestionArea.CountOf('"', true) % 2 != 0;

                                        if (Function.IsInFunctionSyntax(suggestionArea) && !inQuote)
                                        {
                                            string actualParameterPartTyped = parameterizedPartTyped[parameterIndex + 1];
                                            lookupString = String.Format(CultureInfo.CurrentCulture, "&f:{0}", actualParameterPartTyped);

                                            string actualFilter;
                                            if (parameterIndex + 1 < parameterizedPartTyped.Count)
                                            {
                                                string fullFilter = actualParameterPartTyped;
                                                actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, mainContext);
                                            }
                                            else
                                            {
                                                actualFilter = String.Empty;
                                            }

                                            prefixToRemove = actualFilter.Escape();

                                            if (parameterIndex >= 0 && parameterIndex < functionHistory.ParameterHistory.Count)
                                            {
                                                historyMatch = functionHistory.ParameterHistory[parameterIndex].TryFindKey(lookupString, CaseSensitivity.Insensitive);
                                            }

                                            if (historyMatch != null)
                                            {
                                                historyMatch = historyMatch.Substring(prefixToRemove.Length + 3, historyMatch.Length - prefixToRemove.Length - 4);
                                                //if (mainContext.CurrentParameterRequiresInQuoteProcessing)
                                                //{
                                                historyMatch = SuggestionUtilities.GetSuggestionArea(historyMatch, 0, mainContext);
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            if (!mainContext.CurrentParameterRequiresInQuoteProcessing)
                                            {
                                                lookupString = String.Format(CultureInfo.CurrentCulture, "\"{0}", parameterPartTyped);
                                                prefixToRemove = String.Empty;
                                                //historyMatch = functionHistory.ParameterHistory[parameterIndex].TryFindKey(parameterPartTyped, CaseSensitivity.Insensitive);
                                            }
                                            else
                                            {
                                                string actualFilter;
                                                if (parameterIndex + 1 < parameterizedPartTyped.Count)
                                                {
                                                    string fullFilter = parameterPartTyped;

                                                    //bool inQuote = fullFilter.CountOf('"', true) % 2 != 0;

                                                    //if (inQuote)
                                                    //{
                                                    //    fullFilter = fullFilter.Substring(1);
                                                    //}

                                                    actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, mainContext);
                                                }
                                                else
                                                {
                                                    actualFilter = String.Empty;
                                                }

                                                prefixToRemove = actualFilter.Escape();

                                                lookupString = String.Format(CultureInfo.CurrentCulture, "\"{0}{1}", prefixToRemove, partTypedOfSuggestion.Escape());
                                            }

                                            if (parameterIndex >= 0 && parameterIndex < functionHistory.ParameterHistory.Count)
                                            {
                                                historyMatch = functionHistory.ParameterHistory[parameterIndex].TryFindKey(lookupString, CaseSensitivity.Insensitive);
                                            }

                                            if (historyMatch != null)
                                            {
                                                historyMatch = historyMatch.Substring(prefixToRemove.Length + 1, historyMatch.Length - prefixToRemove.Length - 2);

                                                if (!inQuote)
                                                {
                                                    historyMatch = historyMatch.Unescape();
                                                }

                                                if (mainContext.CurrentParameterRequiresInQuoteProcessing)
                                                {
                                                    historyMatch = SuggestionUtilities.GetSuggestionArea(historyMatch, 0, mainContext);
                                                }
                                            }
                                        }
                                    }

                                    if (historyMatch != null)
                                    {
                                        index = this.PopulationInfo.TranslateToIndex(historyMatch);
                                    }
                                }
                            }
                        }
                        else if ((command = currentItem as Command) != null)
                        {
                            List<string> parameterizedPartTyped = new List<string>(SuggestionUtilities.ExtractNameAndParametersFrom(suggestionArea));
                            if (suggestionArea.EndsWith(" "))
                            {
                                parameterizedPartTyped.Add(String.Empty);
                            }

                            if (parameterIndex >= 0 && parameterIndex < parameterizedPartTyped.Count - 1)
                            {
                                string historyMatch = null;
                                string parameterPartTyped = parameterizedPartTyped[parameterIndex + 1];

                                if (parameterPartTyped.StartsWith("\""))
                                {
                                    int length = parameterPartTyped.Length - 1;
                                    if (length > 0)
                                    {
                                        if (parameterPartTyped.EndsWith("\"") && (parameterPartTyped.Length <= 1 || parameterPartTyped.CountReverseCharRun('\\', parameterPartTyped.Length - 2) % 2 == 0))
                                        {
                                            length--;
                                        }
                                    }

                                    parameterPartTyped = parameterPartTyped.Substring(1, length).Unescape();
                                    //partTypedOfSuggestion = parameterPartTyped;
                                }

                                CommandParameterHistoryCollection history = InternalGlobals.CurrentProfile.History.CommandParameterHistory;
                                bool found;
                                CommandParameterHistory commandHistory;
                                if ((commandHistory = history.TryGetItem(Command.GenerateItemId(command, mainContext.GetCurrentItemList()), CaseSensitivity.Insensitive, out found)) != null && found)
                                {
                                    if (commandHistory.ParameterHistory.Count > parameterIndex)
                                    {
                                        string lookupString;
                                        string prefixToRemove;

                                        bool inQuote = suggestionArea.CountOf('"', true) % 2 != 0;

                                        if (!mainContext.CurrentParameterRequiresInQuoteProcessing)
                                        {
                                            lookupString = String.Format(CultureInfo.CurrentCulture, "\"{0}", parameterPartTyped);
                                            prefixToRemove = String.Empty;
                                        }
                                        else
                                        {
                                            string actualFilter;
                                            if (parameterIndex + 1 < parameterizedPartTyped.Count)
                                            {
                                                string fullFilter = parameterPartTyped;

                                                actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, mainContext);
                                            }
                                            else
                                            {
                                                actualFilter = String.Empty;
                                            }

                                            prefixToRemove = actualFilter.Escape();

                                            lookupString = String.Format(CultureInfo.CurrentCulture, "\"{0}{1}", prefixToRemove, partTypedOfSuggestion.Escape());
                                        }

                                        if (parameterIndex >= 0 && parameterIndex < commandHistory.ParameterHistory.Count)
                                        {
                                            historyMatch = commandHistory.ParameterHistory[parameterIndex].TryFindKey(lookupString, CaseSensitivity.Insensitive);
                                        }

                                        if (historyMatch != null)
                                        {
                                            historyMatch = historyMatch.Substring(prefixToRemove.Length + 1, historyMatch.Length - prefixToRemove.Length - 2);

                                            if (!inQuote)
                                            {
                                                historyMatch = historyMatch.Unescape();
                                            }

                                            if (mainContext.CurrentParameterRequiresInQuoteProcessing)
                                            {
                                                historyMatch = SuggestionUtilities.GetSuggestionArea(historyMatch, 0, mainContext);
                                            }
                                        }
                                    }

                                    if (historyMatch != null)
                                    {
                                        index = this.PopulationInfo.TranslateToIndex(historyMatch);
                                    }
                                }
                            }
                        }
                    }
                }

                if (index < 0)
                {
                    index = this.PopulationInfo.TranslateToNearestMatchIndex(partTypedOfSuggestion);
                }
            }

            return index;
        }
    }
}

//string historyMatch = PromptuSettings.CurrentProfile.History.TryFindKey(filter + partTyped, CaseSensitivity.Insensitive);
//if (historyMatch != null)
//{
//    string part;
//    if (Utilities.IsValidPath(historyMatch))
//    {
//        int splitIndex = filter.OccurenceCountOf(Path.DirectorySeparatorChar);
//        //if (upToCaret.EndsWith(PathSeparatorString))
//        //{
//        //    splitIndex--;
//        //}

//        string[] split = historyMatch.Split(PathSeparatorCharArray);
//        part = split[splitIndex];
//        if (splitIndex == 0)
//        {
//            //if (!(filter.Length == 0 && !this.promptHandler.suggestionIsCommandBasedPath && !suggestion.EndsWith(PathSeparatorString)))
//            //{
//            //    suggestion += Path.DirectorySeparatorChar;
//            //}
//            //else
//            //{
//            int namespaceSplitIndex = filter.CountBreaks('.');
//            //if (splitIndex >= 0)
//            //{
//            //int splitIndex = this.promptHandler.prompt.TextualInput.InputText.OccurenceCountOf(' ');
//            string[] namespaceSplit = part.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');//suggestion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//            if (namespaceSplit.Length == 0)
//            {
//                part = String.Empty;
//            }
//            else
//            {
//                part = namespaceSplit[namespaceSplitIndex];
//            }
//            //}
//        }
//    }
//    else
//    {
//        int splitIndex = filter.CountBreaks('.');
//        //if (splitIndex >= 0)
//        //{
//        //int splitIndex = this.promptHandler.prompt.TextualInput.InputText.OccurenceCountOf(' ');
//        string[] split = historyMatch.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');//suggestion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//        part = split[splitIndex];
//        //}
//    }
//    //string section;
//    index = this.PopulationInfo.TranslateToIndex(part);
//}
