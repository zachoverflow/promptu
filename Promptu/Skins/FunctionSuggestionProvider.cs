//-----------------------------------------------------------------------
// <copyright file="FunctionSuggestionProvider.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Skins
{
    using System;
    using ZachJohnson.Promptu.Collections;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UserModel.Collections;

    internal class FunctionSuggestionProvider
    {
        private TrieList nonHistoryItems;

        public FunctionSuggestionProvider()
        {
        }

        public TrieList NonHistoryItems
        {
            get { return this.nonHistoryItems; }
            set { this.nonHistoryItems = value; }
        }

        public string GetSuggestion(string partTyped, int parameterIndex, Function functionFor)
        {
            if (partTyped == null)
            {
                throw new ArgumentNullException("partTyped");
            }

            string match = null;

            string[] parameterizedPartTyped = Function.ExtractNameAndParametersFrom(partTyped);

            if (parameterIndex >= 0 && parameterizedPartTyped.Length - 1 > parameterIndex)
            {
                string parameterPartTyped = parameterizedPartTyped[parameterIndex + 1];
                FunctionHistoryCollection history = InternalGlobals.CurrentProfile.History.FunctionHistory;
                bool found;
                FunctionHistory functionHistory;
                if ((functionHistory = history.TryGetItem(functionFor.StringId, CaseSensitivity.Insensitive, out found)) != null && found)
                {
                    if (functionFor.Parameters.Count > parameterIndex)
                    {
                        match = functionHistory.ParameterHistory[parameterIndex].TryFindKey(parameterPartTyped, CaseSensitivity.Insensitive);
                    }
                }

                if (match == null && this.NonHistoryItems != null)
                {
                    match = this.NonHistoryItems.TryFind(parameterPartTyped, CaseSensitivity.Insensitive);
                }
            }

            return match;
        }
    }
}
