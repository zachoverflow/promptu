//-----------------------------------------------------------------------
// <copyright file="FunctionAndCommandContextItemValidator.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
