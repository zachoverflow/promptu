// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
