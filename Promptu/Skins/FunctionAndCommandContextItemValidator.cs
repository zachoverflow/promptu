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
    using System.Collections.Generic;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.UserModel;

    internal class FunctionAndCommandContextItemValidator : ItemValidator
    {
        private bool forFunctionSyntax;
        private bool inQuote;
        private bool allowFileSystem;

        public FunctionAndCommandContextItemValidator(ParameterSuggestion parameterSuggestion, int parameterIndex, bool forFunctionSyntax, bool inQuote)
        {
            this.forFunctionSyntax = forFunctionSyntax;
            this.inQuote = inQuote;
            this.allowFileSystem = parameterSuggestion is FileSystemParameterSuggestion && (!forFunctionSyntax || inQuote);
        }

        public bool FileSystemAllowed
        {
            get { return this.allowFileSystem; }
        }

        public override bool Include(string name, SuggestionItemType itemType)
        {
            switch (itemType)
            {
                case SuggestionItemType.Command:
                    if (this.allowFileSystem)
                    {
                        bool found;
                        GroupedCompositeItem item = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
                        if (found && item != null)
                        {
                            return item.ContainsFileSystemCommand;
                        }
                    }

                    break;
                case SuggestionItemType.Namespace:
                    return this.IncludeNamespace(name);
                case SuggestionItemType.Function:
                    return this.forFunctionSyntax && !this.inQuote;
                default:
                    break;
            }

            return false;
        }

        private bool IncludeNamespace(string name)
        {
            if (!name.EndsWith("."))
            {
                name += ".";
            }

            foreach (KeyValuePair<string, GroupedCompositeItem> entry in InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.FindAllThatStartWith(name))
            {
                if (entry.Value != null)
                {
                    if (this.forFunctionSyntax)
                    {
                        if (!this.inQuote)
                        {
                            if (entry.Value.StringFunctions.Count > 0)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (this.allowFileSystem && entry.Value.ContainsFileSystemCommand)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (this.allowFileSystem && entry.Value.ContainsFileSystemCommand)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
