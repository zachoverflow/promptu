//-----------------------------------------------------------------------
// <copyright file="ParameterHelpContext.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Skins
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using ZachJohnson.Promptu.Collections;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.UIModel.RichText;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UserModel.Collections;

    internal class ParameterHelpContext
    {
        private GroupedCompositeItem entryForItem;
        private int currentIndex;
        private int parameterIndex;
        private string itemName;
        private bool mustBeFunction;
        private bool? currentParameterAllowsFileSystem;
        private bool? currentParameterRequiresInQuoteProcessing;
        private TrieDictionary<Int32Encapsulator> lastIndexCache;

        public ParameterHelpContext(string itemName, bool mustBeFunction, GroupedCompositeItem entryForItem, int currentIndex, TrieDictionary<Int32Encapsulator> lastIndexCache)
        {
            if (entryForItem == null)
            {
                throw new ArgumentNullException("entryForItem");
            }
            else if (itemName == null)
            {
                throw new ArgumentNullException("itemName");
            }
            else if (lastIndexCache == null)
            {
                throw new ArgumentNullException("lastIndexCache");
            }
            else if (currentIndex < 0)
            {
                throw new ArgumentOutOfRangeException("'currentIndex' cannot be less than zero");
            }
            else if (currentIndex >= entryForItem.Commands.Count + entryForItem.StringFunctions.Count)
            {
                throw new ArgumentOutOfRangeException("'currentIndex' cannot greater than or equal to the number of commands plus the number of functions");
            }

            this.mustBeFunction = mustBeFunction;
            this.itemName = itemName;
            this.entryForItem = entryForItem;
            this.currentIndex = currentIndex;

            this.lastIndexCache = lastIndexCache;
        }

        public event EventHandler CurrentIndexChanged;

        public bool CurrentParameterRequiresInQuoteProcessing
        {
            get
            {
                if (this.currentParameterRequiresInQuoteProcessing == null)
                {
                    object currentItem = this.GetCurrentItem();
                    Function function = currentItem as Function;
                    Command command;
                    if (function != null && this.ParameterIndex >= 0 && this.ParameterIndex < function.Parameters.Count)
                    {
                        ParameterSuggestion parameterSuggestion = function.Parameters[this.ParameterIndex].ParameterSuggestion;
                        this.currentParameterRequiresInQuoteProcessing = this.ParameterSuggestionRequiresInQuoteProcessing(parameterSuggestion);
                    }
                    else if ((command = currentItem as Command) != null)
                    {
                        CommandParameterMetaInfo info = command.ParametersMetaInfo.GetItemContainingParameterSuggestionFor(this.ParameterIndex + 1);
                        this.currentParameterRequiresInQuoteProcessing = info != null && this.ParameterSuggestionRequiresInQuoteProcessing(info.ParameterSuggestion);
                    }
                    else
                    {
                        this.currentParameterRequiresInQuoteProcessing = false;
                    }
                }

                return this.currentParameterRequiresInQuoteProcessing.Value;
            }
        }

        public bool CurrentParameterAllowsFileSystem
        {
            get
            {
                if (this.currentParameterAllowsFileSystem == null)
                {
                    object currentItem = this.GetCurrentItem();
                    Function function = currentItem as Function;
                    Command command;
                    if (function != null && this.ParameterIndex >= 0 && this.ParameterIndex < function.Parameters.Count)
                    {
                        this.currentParameterAllowsFileSystem = function.Parameters[this.ParameterIndex].ParameterSuggestion is FileSystemParameterSuggestion;
                    }
                    else if ((command = currentItem as Command) != null)
                    {
                        CommandParameterMetaInfo info = command.ParametersMetaInfo.GetItemContainingParameterSuggestionFor(this.ParameterIndex + 1);
                        this.currentParameterAllowsFileSystem = info != null && info.ParameterSuggestion is FileSystemParameterSuggestion;
                    }
                    else
                    {
                        this.currentParameterAllowsFileSystem = false;
                    }
                }

                return this.currentParameterAllowsFileSystem.Value;
            }
        }

        public int ParameterIndex
        {
            get
            {
                return this.parameterIndex;
            }

            set
            {
                if (this.parameterIndex != value)
                {
                    this.parameterIndex = value;
                    this.currentParameterAllowsFileSystem = null;
                    this.currentParameterRequiresInQuoteProcessing = null;
                }
            }
        }

        public bool CurrentItemIsFunction
        {
            get
            {
                return this.CurrentIndex >= this.CommandOffset && this.CurrentIndex < this.TotalNumberOfEntries;
            }
        }

        public bool CurrentItemIsCommand
        {
            get
            {
                return this.CurrentIndex >= 0 && this.CurrentIndex < this.CommandOffset;
            }
        }

        public int TotalNumberOfEntries
        {
            get
            {
                return this.entryForItem.StringFunctions.Count + this.CommandOffset;
            }
        }

        public string ItemName
        {
            get { return this.itemName; }
        }

        protected int CurrentIndex
        {
            get
            {
                return this.currentIndex;
            }

            set
            {
                string name = this.itemName.ToUpperInvariant();
                if (!this.lastIndexCache.Contains(name, CaseSensitivity.Insensitive))
                {
                    this.lastIndexCache.Add(name, -1);
                }

                this.lastIndexCache[name, CaseSensitivity.Insensitive] = value;

                if (value != this.currentIndex)
                {
                    this.currentIndex = value;
                    this.currentParameterAllowsFileSystem = null;
                    this.currentParameterRequiresInQuoteProcessing = null;

                    this.OnCurrentIndexChanged(EventArgs.Empty);
                }
            }
        }

        private int CommandOffset
        {
            get
            {
                return this.mustBeFunction ? 0 : this.entryForItem.Commands.Count;
            }
        }

        public static bool IsDownArrow(string id)
        {
            return id == "downArrow";
        }

        public void UpdateCurrentIndexToBestFit()
        {
            string name = this.itemName.ToUpperInvariant();

            bool found;
            Int32Encapsulator lastIndex = this.lastIndexCache.TryGetItem(name, CaseSensitivity.Insensitive, out found);

            if (found && lastIndex != null && lastIndex >= 0 && lastIndex < this.TotalNumberOfEntries)
            {
                this.CurrentIndex = lastIndex;
                return;
            }

            string history = InternalGlobals.CurrentProfile.History.TryFindKey(this.itemName, CaseSensitivity.Insensitive);
            if (history != null)
            {
                history = InternalGlobals.CurrentProfile.History[history, CaseSensitivity.Insensitive].EntryValue;
                if (Function.IsInFunctionSyntax(history))
                {
                    string[] nameAndParameters = Function.ExtractNameAndParametersFrom(history);

                    if (this.ParameterIndex < nameAndParameters.Length - 1)
                    {
                        for (int i = 0; i < this.entryForItem.StringFunctions.Count; i++)
                        {
                            if (this.entryForItem.StringFunctions[i].Item.Parameters.Count == nameAndParameters.Length - 1)
                            {
                                this.CurrentIndex = this.CommandOffset + i;
                            }
                        }
                    }
                    else
                    {
                        this.CycleToBestMatch();
                    }
                }
                else if (!this.mustBeFunction)
                {
                    string[] nameAndParameters = Command.ExtractNameAndParametersFrom(history);

                    if (this.ParameterIndex < nameAndParameters.Length - 1)
                    {
                        for (int i = 0; i < this.entryForItem.Commands.Count; i++)
                        {
                            if (this.entryForItem.Commands[i].Item.TakesParameterCountOf(nameAndParameters.Length - 1))
                            {
                                this.CurrentIndex = i;
                            }
                        }
                    }
                    else
                    {
                        this.CycleToBestMatch();
                    }
                }
            }
        }

        public object GetCurrentCompositeItem()
        {
            if (!this.mustBeFunction && this.CurrentIndex < this.entryForItem.Commands.Count)
            {
                return this.entryForItem.Commands[this.CurrentIndex];
            }
            else
            {
                return this.entryForItem.StringFunctions[this.CurrentIndex - this.CommandOffset];
            }
        }

        public object GetCurrentItem()
        {
            if (!this.mustBeFunction && this.CurrentIndex < this.entryForItem.Commands.Count)
            {
                return this.entryForItem.Commands[this.CurrentIndex].Item;
            }
            else
            {
                return this.entryForItem.StringFunctions[this.CurrentIndex - this.CommandOffset].Item;
            }
        }

        public List GetCurrentItemList()
        {
            if (!this.mustBeFunction && this.CurrentIndex < this.entryForItem.Commands.Count)
            {
                return this.entryForItem.Commands[this.CurrentIndex].ListFrom;
            }
            else
            {
                return this.entryForItem.StringFunctions[this.CurrentIndex - this.CommandOffset].ListFrom;
            }
        }

        public void CycleToBestMatch()
        {
            int index = this.CurrentIndex;

            bool doneCurrentIndex = false;

            while (true)
            {
                if (!this.mustBeFunction && index < this.entryForItem.Commands.Count)
                {
                    Command command = this.entryForItem.Commands[index].Item;

                    if (command.TakesParameterCountOf(this.ParameterIndex + 1))
                    {
                        break;
                    }
                }
                else if (index - this.CommandOffset < this.entryForItem.StringFunctions.Count)
                {
                    Function function = this.entryForItem.StringFunctions[index - this.CommandOffset].Item;

                    if (function.Parameters.Count > this.ParameterIndex)
                    {
                        break;
                    }
                }

                if (index == this.CurrentIndex)
                {
                    if (doneCurrentIndex)
                    {
                        break;
                    }
                    else
                    {
                        doneCurrentIndex = true;
                    }
                }

                if (index == this.TotalNumberOfEntries)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }

            this.CurrentIndex = index;
        }

        public void OffsetCurrentIndex(int quantity)
        {
            int index = this.CurrentIndex;
            index += quantity;

            int totalNumberOfEntries = this.TotalNumberOfEntries;

            if (index < 0)
            {
                index = (totalNumberOfEntries + index) % totalNumberOfEntries;
            }
            else if (index >= totalNumberOfEntries)
            {
                index = index % totalNumberOfEntries;
            }

            this.CurrentIndex = index;
        }

        // I18N
        public void Populate(ITextInfoBox infoBox)
        {
            RTGroup content = new RTGroup();

            int totalCount = this.TotalNumberOfEntries;

            if (totalCount > 1)
            {
                content.Children.Add(new KeyedImage("downArrow"));
                content.Children.Add(new Text(String.Format(CultureInfo.CurrentCulture, "{0} of {1}", this.CurrentIndex + 1, totalCount), TextStyle.Normal));
                content.Children.Add(new KeyedImage("upArrow"));
                content.Children.Add(new Space(5));
            }

            if (!this.mustBeFunction && this.CurrentIndex < this.entryForItem.Commands.Count)
            {
                Command command = this.entryForItem.Commands[this.CurrentIndex].Item;

                int? maxNumberOfParameters = command.GetMaximumNumberOfParameters();
                int minNumberOfParameters = command.GetMininumNumberOfParameters();

                content.Children.Add(Text.Generate("command", false));
                content.Children.Add(Text.Generate(
                    " " + this.itemName,
                    this.ParameterIndex < 0 || (maxNumberOfParameters != null && this.ParameterIndex >= maxNumberOfParameters)));

                RTGroup group = new RTGroup();
                content.Children.Add(group);

                string overrideParameterNameDisplay = null;

                if (maxNumberOfParameters != null)
                {
                    for (int i = 0; i < maxNumberOfParameters.Value; i++)
                    {
                        string format;
                        if (i >= minNumberOfParameters)
                        {
                            format = " [param{0}]";
                        }
                        else
                        {
                            format = " param{0}";
                        }

                        group.Children.Add(Text.Generate(String.Format(CultureInfo.CurrentCulture, format, i + 1), i == this.ParameterIndex, true));
                    }
                }
                else
                {
                    if (!command.GetWhetherJustNParameters())
                    {
                        int numberOfParametersToShow = this.ParameterIndex >= minNumberOfParameters ? this.ParameterIndex + 1 : minNumberOfParameters;
                        for (int i = 0; i < numberOfParametersToShow; i++)
                        {
                            string format;
                            if (i >= minNumberOfParameters)
                            {
                                format = " [param{0}]";
                            }
                            else
                            {
                                format = " param{0}";
                            }

                            group.Children.Add(Text.Generate(String.Format(CultureInfo.CurrentCulture, format, i + 1), i == this.ParameterIndex, true));
                        }

                        group.Children.Add(Text.Generate(String.Format(CultureInfo.CurrentCulture, " ...", minNumberOfParameters + 1), this.ParameterIndex >= minNumberOfParameters, true));
                    }
                    else
                    {
                        group.Children.Add(Text.Generate(" params", true, true));
                        overrideParameterNameDisplay = "params: ";
                    }
                }

                List<RTElement> postParameterDeclaration = new List<RTElement>();

                string documentation = command.ParametersMetaInfo.GetDescriptionFor(this.ParameterIndex + 1);

                if (!String.IsNullOrEmpty(documentation))
                {
                    if (overrideParameterNameDisplay == null)
                    {
                        postParameterDeclaration.Add(Text.Generate(String.Format(CultureInfo.CurrentCulture, "param{0}:", this.ParameterIndex + 1), true));
                    }
                    else
                    {
                        postParameterDeclaration.Add(Text.Generate(overrideParameterNameDisplay, true));
                    }

                    postParameterDeclaration.Add(Text.Generate(" " + documentation, false));
                }

                if (postParameterDeclaration.Count > 0)
                {
                    content.Children.Add(new LineBreak());
                    content.Children.Add(new LineBreak());
                    content.Children.AddRange(postParameterDeclaration);
                }
            }
            else
            {
                CompositeItem<Function, List> functionWithList = this.entryForItem.StringFunctions[this.CurrentIndex - this.CommandOffset];

                content.Children.Add(Text.Generate("string", false));
                content.Children.Add(Text.Generate(
                    " " + this.itemName,
                    this.ParameterIndex < 0 || this.ParameterIndex >= functionWithList.Item.Parameters.Count));

                AssemblyReferenceCollectionComposite prioritizedAssemblyReferenceCollection = new AssemblyReferenceCollectionComposite(InternalGlobals.CurrentProfile.Lists, functionWithList.ListFrom);

                RTGroup group = new RTGroup();
                content.Children.Add(group);

                group.Children.Add(Text.Generate("(", false));

                List<RTElement> postParameterDeclaration = new List<RTElement>();

                for (int i = 0; i < functionWithList.Item.Parameters.Count; i++)
                {
                    string prefix;
                    if (i > 0)
                    {
                        group.Children.Add(Text.Generate(",", false));
                        prefix = " ";
                    }
                    else
                    {
                        prefix = String.Empty;
                    }

                    bool isCurrentParameter = this.ParameterIndex == i;

                    LoadedFunctionInfo functionInfo = functionWithList.Item.TryGetInstance(prioritizedAssemblyReferenceCollection);

                    string parameterName = functionWithList.Item.TryGetParameterName(i, functionInfo);

                    group.Children.Add(Text.Generate(
                        String.Format(CultureInfo.CurrentCulture, "{0}{1} {2}", prefix, functionWithList.Item.Parameters[i].ValueType.ToString().ToLower(CultureInfo.CurrentCulture), parameterName),
                        isCurrentParameter));

                    if (isCurrentParameter)
                    {
                        string documentation = functionWithList.Item.TryGetParameterDocumentation(i, functionInfo);

                        if (!String.IsNullOrEmpty(documentation))
                        {
                            postParameterDeclaration.Add(Text.Generate(String.Format(CultureInfo.CurrentCulture, "{0}:", parameterName), true));
                            postParameterDeclaration.Add(Text.Generate(" " + documentation, false));
                        }
                    }
                }

                group.Children.Add(Text.Generate(")", false, false));

                if (postParameterDeclaration.Count > 0)
                {
                    content.Children.Add(new LineBreak());
                    content.Children.Add(new LineBreak());
                    content.Children.AddRange(postParameterDeclaration);
                }
            }

            infoBox.Content = content;
        }

        protected virtual void OnCurrentIndexChanged(EventArgs e)
        {
            EventHandler handler = this.CurrentIndexChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private bool ParameterSuggestionRequiresInQuoteProcessing(ParameterSuggestion parameterSuggestion)
        {
            ValueListParameterSuggestion valueListParameterSuggestion;
            FunctionReturnParameterSuggestion functionReturnParameterSuggestion;

            bool requiresInQuoteProcessing = false;

            if (parameterSuggestion is FileSystemParameterSuggestion)
            {
                requiresInQuoteProcessing = true;
            }
            else if ((valueListParameterSuggestion = parameterSuggestion as ValueListParameterSuggestion) != null)
            {
                List list = this.GetCurrentItemList();
                ValueList valueList = list.ValueLists.TryGet(valueListParameterSuggestion.ValueListName);
                if (valueList != null && valueList.UseNamespaceInterpretation)
                {
                    requiresInQuoteProcessing = true;
                }
            }
            else if ((functionReturnParameterSuggestion = parameterSuggestion as FunctionReturnParameterSuggestion) != null)
            {
                ValueList valueList = functionReturnParameterSuggestion.TryCompile(this.GetCurrentItemList());
                if (valueList != null && valueList.UseNamespaceInterpretation)
                {
                    requiresInQuoteProcessing = true;
                }
            }

            return requiresInQuoteProcessing;
        }
    }
}
