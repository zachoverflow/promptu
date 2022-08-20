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
    using System.Collections.Generic;
    using System.Collections.Generic.Extensions;
    using System.Drawing;
    using System.Drawing.Extensions;
    using System.Extensions;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using ZachJohnson.Promptu.Collections;
    using ZachJohnson.Promptu.Itl;
    using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
    using ZachJohnson.Promptu.SkinApi;
    using ZachJohnson.Promptu.UI;
    using ZachJohnson.Promptu.UIModel.RichText;
    using ZachJohnson.Promptu.UserModel;

    internal class PopulationProvider
    {
        private static string PathSeparatorString = new string(Path.DirectorySeparatorChar, 1);
        private static Comparison<SuggestionItem> SuggestionItemComparison = new Comparison<SuggestionItem>(CompareSuggestionItems);
        private object folderIcon = null;
        private static TrieList AlwaysGetIconFor = new TrieList(SortMode.DecendingFromLastAdded, new string[] { ".exe", ".lnk" });
        private TrieDictionary<Int32Encapsulator> fileExtensionsAndImageIndexes = new TrieDictionary<Int32Encapsulator>(SortMode.DecendingFromLastAdded);
        private Queue<IconLoadOrder> iconLoadOrders = new Queue<IconLoadOrder>(0);
        //public static PopulateFrom
        private PromptHandler.SeparateSuggestionHandler suggestionHandler;
        private string filterAtLastPopulation = null;
        private SuggestionMode suggestionModeAsLastPopulation = SuggestionMode.Normal;
        private List<object> firstLevelImages = new List<object>();
        private CompositeList<SuggestionItem> firstLevelSuggestionItems;
        private TrieDictionary<Int32Encapsulator> firstLevelSuggestionItemsAndIndexes;
        private Thread iconLoadingThread;
        private EventWaitHandle continueLoadingIcons = new AutoResetEvent(true);
        private Filter<string, HistoryDetails> functionHistoryFilter;
        private Filter<string, HistoryDetails> quoteHistoryFilter;
        private IconSize iconSize;
        private Queue<IconLoadOrder> firstLevelIconLoadOrders = new Queue<IconLoadOrder>();

        public PopulationProvider(PromptHandler.SeparateSuggestionHandler suggestionHandler)
        {
            if (suggestionHandler == null)
            {
                throw new ArgumentNullException("suggestionHandler");
            }

            this.suggestionHandler = suggestionHandler;
            this.functionHistoryFilter = new Filter<string, HistoryDetails>(
                new EntryValidator<string, HistoryDetails>(IsValidFunctionHistory),
                new EntryTranslator<string, HistoryDetails>(TranslateHistoryKey));

            this.quoteHistoryFilter = new Filter<string, HistoryDetails>(null, new EntryTranslator<string, HistoryDetails>(TranslateFunctionQuoteHistoryKey));
        }

        public IconSize IconSize
        {
            get 
            { 
                return this.iconSize; 
            }

            set
            {
                if (this.iconSize != value)
                {
                    this.iconSize = value;
                    this.folderIcon = null;
                }   
            }
        }

        private static bool IsValidFunctionHistory(string key, HistoryDetails value)
        {
            return key.EndsWith(";");
        }

        private static string TranslateHistoryKey(string key, HistoryDetails value)
        {
            if (key.Length > 4)
            {
                return key.Substring(3, key.Length - 4);
            }
            else
            {
                return key;
            }
        }

        private static string TranslateFunctionQuoteHistoryKey(string key, HistoryDetails value)
        {
            if (key.Length > 2)
            {
                return key.Substring(1, key.Length - 2).Unescape();
            }
            else
            {
                return key;
            }
        }

        private static int CompareSuggestionItems(SuggestionItem x, SuggestionItem y)
        {
            return x.Text.CompareTo(y.Text);
        }

        //public EventWaitHandle ContinueLoadingIcons
        //{
        //    get { return this.continueLoadingIcons; }
        //}

        //public int IconLoadOrderCount
        //{
        //    get { return this.iconLoadOrders.Count; }
        //}

        // TODO see if this holds ref when changing skin
        public void StartProcessingIconLoadOrders()
        {
            if (this.iconLoadOrders.Count > 0)
            {
                this.continueLoadingIcons.Set();
                if (this.iconLoadingThread == null || (this.iconLoadingThread != null && !this.iconLoadingThread.IsAlive))
                {
                    this.iconLoadingThread = new Thread(this.ProcessIconLoadOrders);
                    this.iconLoadingThread.Priority = ThreadPriority.BelowNormal;
                    this.iconLoadingThread.IsBackground = true;
                    try
                    {
                        this.iconLoadingThread.Start();
                    }
                    catch (ThreadStartException)
                    {
                    }
                }
            }
        }

        public void ResetFilterAtLastPopulation()
        {
            this.filterAtLastPopulation = null;
        }

        //private void ProcessIconLoadOrders(object obj)
        //{
        //    ISuggestionProvider suggester = obj as ISuggestionProvider;
        //    if (suggester == null)
        //    {
        //        throw new ArgumentException("'obj' is not of the type 'ISuggestionProvider'.");
        //    }

        //    this.ProcessIconLoadOrders(suggester);
        //}

        private void ProcessIconLoadOrders()
        {
            while (true)
            {
                this.continueLoadingIcons.WaitOne();
                ISuggestionProvider suggester = this.suggestionHandler.Suggester;
                if (suggester == null)
                {
                    this.iconLoadOrders.Clear();
                }
                else
                {
                    if (this.iconLoadOrders != null)
                    {
                        while (this.iconLoadOrders.Count > 0)
                        {
                            IconLoadOrder order = this.iconLoadOrders.Dequeue();
                            order.Load(suggester, this.iconSize);
                            //if (!this.fileExtensionsAndImageIndexes.Contains(key, CaseSensitivity.Insensitive))
                            //{

                            //}


                            //if (this.stopIconLoading)
                            //{

                            //}
                        }
                    }
                }

                //if (this.filterAtLastPopulation != null && this.filterAtLastPopulation.Length <= 0)
                //{
                //    this.firstLevelImages.Clear();
                //    foreach (object image in suggester.Images)
                //    {
                //        this.firstLevelImages.Add(image);
                //    }
                //}

                this.continueLoadingIcons.Reset();
                //while (this.stopIconLoading)
                //{
                //}
            }
        }

        public void PopulateFirstLevelSuggestions(ISuggestionProvider suggester)
        {
            this.PopulateSuggester(suggester, String.Empty, false, true, null);
        }

        public bool PopulateSuggester(ISuggestionProvider suggester, string filter, bool resizePrompt, bool populateFirstLevel, ParameterHelpContext contextInfo)
        {
            int itemCount = 0;
            try
            {
                //Trace.WriteLine(String.Format("pop start. filter:\"{0}\"", filter));
                Queue<IconLoadOrder> iconLoadOrders = this.iconLoadOrders;
                if (!populateFirstLevel)
                {
                    if (filter == this.filterAtLastPopulation && this.suggestionHandler.SuggestionMode == this.suggestionModeAsLastPopulation)
                    {
                        return true;
                    }

                    iconLoadOrders.Clear();
                }
                else
                {
                    suggester = new MockSuggester();
                    iconLoadOrders = new Queue<IconLoadOrder>();
                }

                InternalGlobals.SyncSynchronizer.PauseSyncs();
                InternalGlobals.SyncSynchronizer.WaitUntilAllSyncsPaused();

                if (!populateFirstLevel)
                {
                    suggester.ClearItems();
                    suggester.Images.Clear();

                    this.suggestionModeAsLastPopulation = this.suggestionHandler.SuggestionMode;
                }

                DoPopulationInfo populationInfo = new DoPopulationInfo(suggester, contextInfo, iconLoadOrders);

                if (populateFirstLevel || this.suggestionHandler.SuggestionMode == SuggestionMode.Normal)
                {
                    if (!populateFirstLevel && filter.Length == 0 && this.firstLevelImages != null && this.firstLevelSuggestionItems != null && this.firstLevelSuggestionItemsAndIndexes != null)
                    {
                        this.LoadCachedFirstLevel(populationInfo);
                    }
                    else
                    {
                        if (!this.DoStandardPopulation(filter, populateFirstLevel, populationInfo))
                        {
                            return false;
                        }
                    }

                    for (int i = 0; i < populationInfo.SuggestionItems.Count; i++)
                    {
                        SuggestionItem item = populationInfo.SuggestionItems[i];
                        populationInfo.Suggester.AddItem(item);
                        populationInfo.SuggestionItemsAndIndexes[item.Text, CaseSensitivity.Insensitive] = i;
                    }

                    if (populateFirstLevel)
                    {
                        this.firstLevelImages.Clear();
                        this.firstLevelImages.AddRange(populationInfo.Suggester.Images);

                        //while (populationInfo.IconLoadOrders.Count > 0)
                        //{
                        //    this.firstLevelIconLoadOrders.Enqueue(populationInfo.IconLoadOrders.Dequeue());
                        //}
                        this.firstLevelIconLoadOrders = populationInfo.IconLoadOrders;

                        this.firstLevelSuggestionItems = populationInfo.SuggestionItems;
                        this.firstLevelSuggestionItemsAndIndexes = populationInfo.SuggestionItemsAndIndexes;
                    }
                    //else
                    //{
                    //    //this.iconLoadOrders.Clear();
                    //    //while (populationInfo.IconLoadOrders.Count > 0)
                    //    //{
                    //    //    this.iconLoadOrders.Enqueue(populationInfo.IconLoadOrders.Dequeue());
                    //    //}
                    //}
                }
                else if (this.suggestionHandler.SuggestionMode == SuggestionMode.History)
                {
                    this.DoFullItemHistoryPopulation(populationInfo);
                }

                //if (resizePrompt)
                //{
                //    //Rectangle protectedArea = new Rectangle(this.suggestionHandler.Prompt.LocationOnScreen, this.suggestionHandler.Prompt.PromptSize);

                //    //IInformationBox mainBox = this.suggestionHandler.InformationBoxMananger.MainInformationBox;
                //    //if (mainBox != null && mainBox.Visible)
                //    //{
                //    //    protectedArea = protectedArea.GetBoundingRectangleWith(new Rectangle(mainBox.Location, mainBox.Size));
                //    //}

                //    //populationInfo.Suggester.SizeAndPositionYourself(
                //    //    protectedArea,
                //    //    new List<Rectangle>(),
                //    //    Globals.CurrentProfile.SuggesterSize);
                //}

                if (!populateFirstLevel)
                {
                    this.suggestionHandler.SuggestionProvider.PopulationInfo = new PopulationInfo(true, populationInfo.SuggestionItemsAndIndexes);
                }

                itemCount = populationInfo.SuggestionItemsAndIndexes.Count;
                InternalGlobals.SyncSynchronizer.UnPauseSyncs();
                return true;
            }
            finally
            {
                InternalGlobals.SyncSynchronizer.UnPauseSyncs();
                //Trace.WriteLine(String.Format("pop end. item-count:{0}", itemCount));
            }
        }

        private void DoFullItemHistoryPopulation(DoPopulationInfo populationInfo)
        {
            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.History);
            populationInfo.SuggestionItemsAndIndexes = new TrieDictionary<Int32Encapsulator>(SortMode.DecendingFromLastAdded);
            TrieDictionary<SuggestionItem> historyItems = new TrieDictionary<SuggestionItem>(SortMode.DecendingFromLastAdded);

            //System.Collections.ObjectModel.ReadOnlyCollection<string> history = .AllKeys;

            for (int i = 0; i < InternalGlobals.CurrentProfile.History.ComplexHistory.Count; i++)
            {
                string item = InternalGlobals.CurrentProfile.History.ComplexHistory[InternalGlobals.CurrentProfile.History.ComplexHistory.Count - 1 - i].EntryValue;
                if (InternalGlobals.CurrentProfile.Lists.AnyListsAreDisabled && !InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(item)
                            && InternalGlobals.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(item))
                {
                    continue;
                }

                if (!SuggestionUtilities.IsConsideredComplex(item))
                {
                    continue;
                }

                SuggestionItemType suggestionItemType = SuggestionItemType.History;
                //HistoryDetails details = PromptuSettings.CurrentProfile.History[item, CaseSensitivity.Sensitive];
                int imageIndex = 0;

                if (!populationInfo.SuggestionItemsAndIndexes.Contains(item, CaseSensitivity.Insensitive))
                {
                    historyItems.Add(item, new SuggestionItem(suggestionItemType, item, imageIndex));
                    populationInfo.SuggestionItemsAndIndexes.Add(item.ToUpperInvariant(), 0);
                }
            }

            for (int i = 0; i < historyItems.Count; i++)
            {
                SuggestionItem item = historyItems[historyItems.Count - 1 - i];
                populationInfo.Suggester.AddItem(item);
                populationInfo.SuggestionItemsAndIndexes[item.Text, CaseSensitivity.Insensitive] = i;
            }
        }

        private void LoadCachedFirstLevel(DoPopulationInfo populationInfo)
        {
            populationInfo.Suggester.ClearItems();
            populationInfo.Suggester.Images.Clear();

            foreach (object image in this.firstLevelImages)
            {
                populationInfo.Suggester.Images.Add(image);
            }

            foreach (IconLoadOrder order in this.firstLevelIconLoadOrders)
            {
                populationInfo.IconLoadOrders.Enqueue(order);
            }

            populationInfo.SuggestionItems = this.firstLevelSuggestionItems;
            populationInfo.SuggestionItemsAndIndexes = this.firstLevelSuggestionItemsAndIndexes;
        }

        private bool MoreThanBreakCharIsTyped(IPrompt prompt)
        {
            if (prompt.SelectionStart > 0)
            {
                bool previousIsBreakChar = prompt.Text.PreviousCharIs(prompt.SelectionStart - 1, true, '(', ')', ',');
                if (!previousIsBreakChar)
                {
                    if (prompt.Text.CountOf(
                        '"', 
                        true,
                        0,
                        prompt.SelectionStart) % 2 == 0)
                    {
                        previousIsBreakChar = prompt.Text.PreviousCharIs(prompt.SelectionStart - 1, true, '"'); ;
                    }
                }

                return !previousIsBreakChar;
            }
            
            return false;
        }

        private bool PopulateFromFunctionReturn(string expression, string[] previousArgs, TrieDictionary<SuggestionItem> standardItems, List priorityList, string filter, int filterDotCount, DoPopulationInfo populationInfo, bool escapeItemEntry, ref bool allowNamespaces)
        {
            ItlCompiler compiler = new ItlCompiler();
            FeedbackCollection feedback;
            Expression expressionObject = compiler.Compile(ItlType.SingleFunction, FunctionReturnParameterSuggestion.FormatExpression(expression), out feedback);

            if (feedback.Has(FeedbackType.Error))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(Localization.MessageFormats.UnableToPopulateSuggestionProviderCompileError, expression);
                builder.AppendLine();
                builder.AppendLine();
                foreach (FeedbackMessage message in feedback)
                {
                    builder.AppendLine(message.ToString());
                }

                this.GiveError(builder.ToString(), populationInfo.Suggester);
                this.filterAtLastPopulation = null;
                return false;
            }
            else
            {
                ExpressionGroup group = expressionObject as ExpressionGroup;
                if (group != null)
                {
                    group = group.Expressions[0] as ExpressionGroup;
                    if (group != null)
                    {
                        FunctionCall functionCall = null;

                        foreach (Expression innerExpression in group.Expressions)
                        {
                            functionCall = innerExpression as FunctionCall;
                            if (functionCall != null)
                            {
                                break;
                            }
                        }

                        if (functionCall != null)
                        {
                            try
                            {
                                ExecutionData data = new ExecutionData(previousArgs, priorityList, InternalGlobals.CurrentProfile.Lists);
                                ValueList valueList = functionCall.ConvertToValueList(data);
                                allowNamespaces = valueList.UseNamespaceInterpretation;

                                this.PopulateFromValueList(filter, filterDotCount, standardItems, valueList, populationInfo, escapeItemEntry);
                            }
                            catch (ConversionException ex)
                            {
                                StringBuilder builder = new StringBuilder();
                                builder.AppendFormat(Localization.MessageFormats.UnableToPopulateSuggestionProviderInvocationError, expression);
                                builder.AppendLine();
                                builder.AppendLine();
                                if (ex is NotEnoughArgumentsConversionException)
                                {
                                    builder.AppendLine("Error: The expression references the current parameter or a parameter which comes after the current parameter.");
                                }
                                else
                                {
                                    builder.AppendLine(ex.Message);
                                }


                                this.GiveError(builder.ToString(), populationInfo.Suggester);
                                this.filterAtLastPopulation = null;
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void GiveError(string message, object windowOwner)
        {
            //StringBuilder fullMessage = new StringBuilder(message);
            //StandardButtons buttons = StandardButtons.None;

            //string name = null;

            //Command command = errorOn as Command;

            //if (command != null)
            //{
            //    name = command.Name;
            //}
            //else
            //{
            //    Function function = errorOn as Function;
            //    if (function != null)
            //    {
            //        name = function.Name;
            //    }
            //}
            
            //if (name != null)
            //{
            //    fullMessage.AppendLine();
            //    fullMessage.AppendLine();
            //    fullMessage.AppendFormat("Would you like to edit '{0}'", name);

            //    buttons = StandardButtons.Yes | StandardButtons.No;
            //}

            //REVISIT do sizing?
            ITextInfoBox informationBox = InternalGlobals.CurrentSkinInstance.CreateTextInfoBox();
            //Globals.CurrentSkin.ToolTipSettings.ApplyTo(informationBox);
            informationBox.InfoType = InfoType.Error;

            //informationBox.Content.Font = PromptuFonts.UnauthorizedAccessFont;
            //informationBox.Content.ForeColor = Color.Tomato;
            informationBox.Content = new Text(message, TextStyle.Normal);

            Size preferredSize;// = informationBox.GetPreferredSize(Size.Empty);
            //int maximumWidth = PromptuUtilities.GetWidthOfThreeFifthsOfScreen(this.suggestionHandler.Prompt.LocationOnScreen);

            //if (preferredSize.Width > maximumWidth)
            //{
            //    preferredSize = informationBox.GetPreferredSize(new Size(maximumWidth, 0));

            //    if (preferredSize.Width > maximumWidth)
            //    {
            //        preferredSize.Width = maximumWidth;
            //    }
            //}

            //informationBox.Size = preferredSize;

            //informationBox.Location = InformationBoxLayoutManager.Position(new Rectangle(this.suggestionHandler.Prompt.Location, this.suggestionHandler.Prompt.Size), preferredSize);
            InternalGlobals.CurrentSkinInstance.LayoutManager.PositionInfoBox(
                this.CreatePositioningContext(),
                informationBox,
                out preferredSize);

            this.suggestionHandler.InformationBoxMananger.RegisterAndShow(informationBox, true, windowOwner);
            informationBox.Size = preferredSize;
            //informationBox.BringToFront();
            //this.suggestionHandler.Prompt.Activate();
        }

        private bool PopulateFromParameterSuggestion(
            string[] previousArgs,
            TrieDictionary<SuggestionItem> standardItems,
            List listFrom,
            string actualFilter, 
            int actualFilterDotCount,
            ParameterSuggestion parameterSuggestion,
            DoPopulationInfo populationInfo,
            bool escapeItemEntry,
            ref bool skipNormal, 
            ref bool ignoreHistory,
            ref bool allowNamespaces)
        {
            ValueListParameterSuggestion valueListParameterSuggestion;
            FunctionReturnParameterSuggestion functionReturnParameterSuggestion;
            FileSystemParameterSuggestion fileSystemParameterSuggestion;

            if ((fileSystemParameterSuggestion = parameterSuggestion as FileSystemParameterSuggestion) != null)
            {
                allowNamespaces = true;
                int indexOfLastDirectorySeparatorChar = actualFilter.LastIndexOf(Path.DirectorySeparatorChar);
                int indexOfFirstDirectorySeparatorChar = actualFilter.IndexOf(Path.DirectorySeparatorChar);

                WildCardExpression fileFilter;
                if (string.IsNullOrEmpty(fileSystemParameterSuggestion.Filter))
                {
                    fileFilter = null;
                }
                else
                {
                    ItlCompiler compiler = new ItlCompiler();
                    FeedbackCollection feedback;
                    Expression expressionObject = compiler.Compile(ItlType.Standard, fileSystemParameterSuggestion.Filter, out feedback);

                    if (feedback.Has(FeedbackType.Error))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendFormat(Localization.MessageFormats.UnableToPopulateSuggestionProviderCompileError, fileSystemParameterSuggestion.Filter);
                        builder.AppendLine();
                        builder.AppendLine();
                        foreach (FeedbackMessage message in feedback)
                        {
                            builder.AppendLine(message.ToString());
                        }

                        this.GiveError(builder.ToString(), populationInfo.Suggester);
                        this.filterAtLastPopulation = null;
                        return false;
                    }
                    else
                    {
                        try
                        {
                            ExecutionData data = new ExecutionData(previousArgs, listFrom, InternalGlobals.CurrentProfile.Lists);
                            fileFilter = new WildCardExpression(expressionObject.ConvertToString(data), false);
                        }
                        catch (ConversionException ex)
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.AppendFormat(Localization.MessageFormats.UnableToPopulateSuggestionProviderInvocationError, fileSystemParameterSuggestion.Filter);
                            builder.AppendLine();
                            builder.AppendLine();
                            if (ex is NotEnoughArgumentsConversionException)
                            {
                                builder.AppendLine("Error: The expression references the current parameter or a parameter which comes after the current parameter.");
                            }
                            else
                            {
                                builder.AppendLine(ex.Message);
                            }


                            this.GiveError(builder.ToString(), populationInfo.Suggester);
                            this.filterAtLastPopulation = null;
                            return false;
                        }
                    }
                }

                if (indexOfLastDirectorySeparatorChar > -1)
                {
                    skipNormal = true;
                    ignoreHistory = true;

                    if (!this.PopulateFromFileSystem(actualFilter, indexOfLastDirectorySeparatorChar, indexOfFirstDirectorySeparatorChar, populationInfo, escapeItemEntry, fileFilter))
                    {
                        return false;
                    }
                }
            }
            else if ((valueListParameterSuggestion = parameterSuggestion as ValueListParameterSuggestion) != null)
            {
                ValueList valueList = listFrom.ValueLists.TryGet(valueListParameterSuggestion.ValueListName);

                allowNamespaces = valueList.UseNamespaceInterpretation;

                if (valueList != null)
                {
                    this.PopulateFromValueList(actualFilter, actualFilterDotCount, standardItems, valueList, populationInfo, escapeItemEntry);
                }
            }
            else if ((functionReturnParameterSuggestion = parameterSuggestion as FunctionReturnParameterSuggestion) != null)
            {
                this.PopulateFromFunctionReturn(
                    functionReturnParameterSuggestion.Expression,
                    previousArgs,
                    standardItems,
                    listFrom,
                    actualFilter,
                    actualFilterDotCount,
                    populationInfo,
                    escapeItemEntry,
                    ref allowNamespaces);
            }

            return true;
        }

        private bool PopulateFromCommand(Command command, List listFrom, string filter, int parameterIndex, DoPopulationInfo populationInfo)
        {
            TrieDictionary<SuggestionItem> standardItems = new TrieDictionary<SuggestionItem>(SortMode.DecendingFromLastAdded);

            int? commandAndNamespaceIndex = null;
            int? functionAndNamespaceIndex = null;
            int? nativeCommandAndNamespaceIndex = null;
            int? historyAndNamespaceIndex = null;

            bool inQuote = filter.CountOf('"', true) % 2 != 0;

            string[] nameAndParameters = SuggestionUtilities.ExtractNameAndParametersFrom(filter);
            string actualFilter;
            if (parameterIndex + 1 < nameAndParameters.Length)
            {
                string fullFilter = nameAndParameters[parameterIndex + 1];
                if (inQuote)
                {
                    fullFilter = fullFilter.Substring(1).Unescape();
                }

                actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, populationInfo.ContextInfo);

                if (!Function.IsInFunctionSyntax(filter) 
                    && !inQuote)
                {
                    if (fullFilter.Length > 1 
                        && fullFilter[fullFilter.Length - 1] == '"' 
                        && fullFilter.CountReverseCharRun('\\', fullFilter.Length - 2) % 2 == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                actualFilter = String.Empty;
            }

            int actualFilterDotCount = actualFilter.CountBreaks('.');

            IEnumerable<string> compositeFunctionsAndCommands;

            if (actualFilter.Length > 0)
            {
                compositeFunctionsAndCommands = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.FindAllThatStartWith(actualFilter).Keys;
            }
            else
            {
                compositeFunctionsAndCommands = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.AllKeys;
            }

            bool skipNormal = false;

            int? maxNumberOfParameters = command.GetMaximumNumberOfParameters();

            if (parameterIndex >= 0 && (maxNumberOfParameters == null || parameterIndex < maxNumberOfParameters))
            {
                bool ignoreHistory = false;
                CommandParameterMetaInfo itemContainingSuggestionInfo = command.ParametersMetaInfo.GetItemContainingParameterSuggestionFor(parameterIndex + 1);

                ParameterSuggestion parameterSuggestion = itemContainingSuggestionInfo == null ? null : itemContainingSuggestionInfo.ParameterSuggestion;

                string[] simpleNameAndParameters = Command.ExtractSimpleNameAndParametersFrom(filter);
                int numberOfPreviousArgs = parameterIndex;
                if (numberOfPreviousArgs > simpleNameAndParameters.Length - 1)
                {
                    numberOfPreviousArgs = simpleNameAndParameters.Length - 1;
                }

                string[] previousArgs = new string[numberOfPreviousArgs];

                for (int i = 0; i < previousArgs.Length; i++)
                {
                    previousArgs[i] = command.TranslateArgument(simpleNameAndParameters[i + 1], i, listFrom);
                }

                bool allowNamespaces = false;

                if (!this.PopulateFromParameterSuggestion(
                    previousArgs,
                    standardItems,
                    listFrom,
                    actualFilter,
                    actualFilterDotCount,
                    parameterSuggestion,
                    populationInfo,
                    inQuote,
                    ref skipNormal,
                    ref ignoreHistory,
                    ref allowNamespaces))
                {
                    return false;
                }

                if (!skipNormal)
                {
                    FunctionAndCommandContextItemValidator itemValidator = new FunctionAndCommandContextItemValidator(
                        parameterSuggestion, 
                        parameterIndex, 
                        false, 
                        inQuote);

                    if (populationInfo.ContextInfo.CurrentParameterAllowsFileSystem && !InternalGlobals.CurrentProfile.PrimaryFileSystemCommandValidationCompleted)
                    {
                        this.DoVisibleCommandFileSystemValidations(populationInfo.Suggester);
                    }

                    this.PopulateFromFunctionsAndCommands(
                        compositeFunctionsAndCommands,
                        actualFilter,
                        actualFilterDotCount,
                        standardItems,
                        populationInfo,
                        inQuote,
                        ref commandAndNamespaceIndex,
                        ref functionAndNamespaceIndex,
                        ref nativeCommandAndNamespaceIndex,
                        itemValidator);
                }

                if (!ignoreHistory && ((itemContainingSuggestionInfo != null && itemContainingSuggestionInfo.ShowHistory) || command.ShowParameterHistory))
                {
                    //this.PopulateFromHistory(
                    //    PromptuSettings.CurrentProfile.History.FindAllThatStartWith(filter, CaseSensitivity.Insensitive).Keys,
                    //    filter,
                    //    filter.CountOf('.', false),
                    //    standardItems,
                    //    PromptuSettings.CurrentProfile.History,
                    //    populationInfo,
                    //    null,
                    //    ref historyAndNamespaceIndex);

                    string id = Command.GenerateItemId(command, listFrom);
                    bool found;
                    CommandParameterHistory commandParameterHistory = InternalGlobals.CurrentProfile.History.CommandParameterHistory.TryGetItem(id, CaseSensitivity.Insensitive, out found);
                    if (found && commandParameterHistory != null && commandParameterHistory.ParameterHistory.Count > parameterIndex)
                    {
                        TrieDictionary<HistoryDetails> historyDetailsLookup = commandParameterHistory.ParameterHistory[parameterIndex];
                        IEnumerable<string> history;
                        //Filter<string, HistoryDetails> itemFilter;

                        //itemFilter = null;
                        history = historyDetailsLookup.FindAllThatStartWith("\"", CaseSensitivity.Insensitive).Keys;

                        this.PopulateFromHistory(
                            history, 
                            actualFilter, 
                            actualFilterDotCount, 
                            standardItems, 
                            historyDetailsLookup, 
                            populationInfo, 
                            null,//itemFilter, 
                            inQuote,
                            false,
                            ref historyAndNamespaceIndex,
                            false,
                            allowNamespaces);
                    }
                }
            }

            standardItems.SortKeys();
            populationInfo.SuggestionItems.AddListToComposite(new OptimizedStringKeyedDictionaryListAbstractionLayer<SuggestionItem>(standardItems));

            return true;
        }

        private bool PopulateFromFunction(Function function, List listFrom, string filter, int parameterIndex, DoPopulationInfo populationInfo)
        {
            TrieDictionary<SuggestionItem> standardItems = new TrieDictionary<SuggestionItem>(SortMode.DecendingFromLastAdded);

            int? commandAndNamespaceIndex = null;
            int? functionAndNamespaceIndex = null;
            int? nativeCommandAndNamespaceIndex = null;
            int? historyAndNamespaceIndex = null;

            bool filterIsInFunctionSyntax = Function.IsInFunctionSyntax(filter);
            bool inQuote = filter.CountOf('"', true) % 2 != 0;
            bool includeFunctions = filterIsInFunctionSyntax && !inQuote;

            if (!MoreThanBreakCharIsTyped(this.suggestionHandler.Prompt))
            {
                this.ResetFilterAtLastPopulation();
                return true;
            }

            string[] nameAndParameters = SuggestionUtilities.ExtractNameAndParametersFrom(filter);
            string actualFilter;
            if (parameterIndex + 1 < nameAndParameters.Length)
            {
                string fullFilter = nameAndParameters[parameterIndex + 1];
                if (inQuote)
                {
                    fullFilter = fullFilter.Substring(1).Unescape();
                }

                actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, populationInfo.ContextInfo);

                //if (inQuote)
                //{
                //    actualFilter = actualFilter.Unescape();
                //}
            }
            else
            {
                actualFilter = String.Empty;
            }

            int actualFilterDotCount = actualFilter.CountBreaks('.');

            IEnumerable<string> compositeFunctionsAndCommands;

            if (actualFilter.Length > 0)
            {
                compositeFunctionsAndCommands = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.FindAllThatStartWith(actualFilter).Keys;
            }
            else
            {
                compositeFunctionsAndCommands = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.AllKeys;
            }

            bool skipNormal = false;

            if (parameterIndex >= 0 && parameterIndex < function.Parameters.Count)
            {
                bool ignoreHistory = false;
                FunctionParameter parameter = function.Parameters[parameterIndex];

                int numberOfPreviousArgs = parameterIndex;
                if (numberOfPreviousArgs > nameAndParameters.Length - 1)
                {
                    numberOfPreviousArgs = nameAndParameters.Length - 1;
                }

                string[] previousArgs = new string[numberOfPreviousArgs];

                for (int i = 0; i < previousArgs.Length; i++)
                {
                    previousArgs[i] = function.TranslateArgument(nameAndParameters[i + 1], i, listFrom);
                }

                bool allowNamespaces = false;

                if (!filterIsInFunctionSyntax || inQuote)
                {
                    if (!this.PopulateFromParameterSuggestion(
                        previousArgs,
                        standardItems,
                        listFrom,
                        actualFilter,
                        actualFilterDotCount,
                        parameter.ParameterSuggestion,
                        populationInfo,
                        inQuote,
                        ref skipNormal,
                        ref ignoreHistory,
                        ref allowNamespaces))
                    {
                        return false;
                    }
                }

                if (!skipNormal)
                {
                    FunctionAndCommandContextItemValidator itemValidator = new FunctionAndCommandContextItemValidator(parameter.ParameterSuggestion, parameterIndex, filterIsInFunctionSyntax, inQuote);

                    if (populationInfo.ContextInfo.CurrentParameterAllowsFileSystem && (!filterIsInFunctionSyntax || inQuote) && !InternalGlobals.CurrentProfile.PrimaryFileSystemCommandValidationCompleted)
                    {
                        this.DoVisibleCommandFileSystemValidations(populationInfo.Suggester);
                    }

                    this.PopulateFromFunctionsAndCommands(
                        compositeFunctionsAndCommands,
                        actualFilter,
                        actualFilterDotCount,
                        standardItems,
                        populationInfo,
                        inQuote,
                        ref commandAndNamespaceIndex,
                        ref functionAndNamespaceIndex,
                        ref nativeCommandAndNamespaceIndex,
                        itemValidator);
                }

                if (!ignoreHistory && parameter.ShowHistory)
                {
                    string id = function.GetStringId();
                    bool found;
                    FunctionHistory functionHistory = InternalGlobals.CurrentProfile.History.FunctionHistory.TryGetItem(id, CaseSensitivity.Insensitive, out found);
                    if (found && functionHistory != null && functionHistory.ParameterHistory.Count > parameterIndex)
                    {
                        TrieDictionary<HistoryDetails> historyDetailsLookup = functionHistory.ParameterHistory[parameterIndex];
                        IEnumerable<string> history;
                        Filter<string, HistoryDetails> itemFilter;
                        if (!filterIsInFunctionSyntax || inQuote)
                        {
                            itemFilter = this.quoteHistoryFilter;
                            history = historyDetailsLookup.FindAllThatStartWith("\"", CaseSensitivity.Insensitive).Keys;
                            this.PopulateFromHistory(
                                history, 
                                actualFilter, 
                                actualFilterDotCount, 
                                standardItems, 
                                historyDetailsLookup, 
                                populationInfo, 
                                itemFilter, 
                                inQuote,
                                false,
                                ref historyAndNamespaceIndex,
                                false,
                                allowNamespaces);
                        }
                        //else
                        //{
                        //    itemFilter = this.functionHistoryFilter;
                        //    history = historyDetailsLookup.FindAllThatStartWith("&f:", CaseSensitivity.Insensitive).Keys;
                        //}

                        
                    }
                }
            }

            standardItems.SortKeys();

            populationInfo.SuggestionItems.AddListToComposite(new OptimizedStringKeyedDictionaryListAbstractionLayer<SuggestionItem>(standardItems));

            return true;
        }

        private PositioningContext CreatePositioningContext()
        {
            return new PositioningContext(
                InternalGlobals.CurrentSkin, 
                InternalGlobals.CurrentSkinInstance, 
                new InfoBoxes(this.suggestionHandler.InformationBoxMananger));
        }

        public void DoVisibleCommandFileSystemValidations(object windowOwner)
        {
            try
            {
                InternalGlobals.CurrentProfile.BackgroundWorkQueue.Pause();
                int totalNumber = 0;

                foreach (List list in InternalGlobals.CurrentProfile.Lists)
                {
                    using (DdMonitor.Lock(list.Commands))
                    {
                        foreach (Command command in list.Commands)
                        {
                            if (command.TakesParameterCountOf(0))
                            {
                                totalNumber++;
                            }
                        }
                    }
                }

                if (totalNumber > 0)
                {
                    IProgressInfoBox progressBox = InternalGlobals.CurrentSkinInstance.CreateProgressInfoBox();
                    progressBox.Maximum = totalNumber * 10;
                    progressBox.Mininum = 0;
                    progressBox.Text = Localization.UIResources.GenerationDirectoryCache;

                    Size preferredSize;// = progressWindow.GetPreferredSize(Size.Empty);
                    InternalGlobals.CurrentSkinInstance.LayoutManager.PositionProgressBox(
                        this.CreatePositioningContext(),
                        progressBox,
                        out preferredSize);
                    this.suggestionHandler.InformationBoxMananger.RegisterAndShow(progressBox, false, windowOwner);
                    //progressWindow.Location = InformationBoxLayoutManager.Position(new Rectangle(this.suggestionHandler.Prompt.Location, this.suggestionHandler.Prompt.Size), preferredSize);
                    //progressWindow.Size = preferredSize;
                    //progressWindow.BringToFront();

                    foreach (List list in InternalGlobals.CurrentProfile.Lists)
                    {
                        using (DdMonitor.Lock(list.Commands))
                        {
                            foreach (Command command in list.Commands)
                            {
                                if (command.TakesParameterCountOf(0))
                                {
                                    int amountLeft = 10;

                                    CommandValidation validation = new CommandValidation(command, list, null);
                                    ParameterlessVoid validate = new ParameterlessVoid(validation.ValidateIsFileSystem);

                                    IAsyncResult asyncResult = validate.BeginInvoke(null, null);

                                    while (!asyncResult.IsCompleted)
                                    {
                                        if (amountLeft > 0)
                                        {
                                            progressBox.Value++;
                                            amountLeft--;
                                            progressBox.Refresh();
                                            //System.Threading.Thread.Sleep(5);
                                        }
                                        //System.Threading.Thread.Sleep();
                                    }

                                    validate.EndInvoke(asyncResult);

                                    if (amountLeft > 0)
                                    {
                                        progressBox.Value += amountLeft;
                                        progressBox.Refresh();
                                    }
                                }
                            }
                        }
                    }

                    progressBox.Hide();
                    InternalGlobals.CurrentProfile.CancelFileSystemValidations();
                    //Globals.CurrentProfile.PrimaryFileSystemCommandValidationCompleted = true;
                    this.suggestionHandler.InformationBoxMananger.Unregister(progressBox);
                }
            }
            finally
            {
                InternalGlobals.CurrentProfile.BackgroundWorkQueue.Unpause();
            }
        }

        private bool DoStandardPopulation(string filter, bool populateFirstLevel, DoPopulationInfo populationInfo)
        {
            string populationFilter;
            if (populateFirstLevel)
            {
                populationFilter = String.Empty;
                //this.filterAtLastPopulation = null;
            }
            else
            {
                populationFilter = filter;
                this.filterAtLastPopulation = filter;
            }

            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.Command);
            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.Function);
            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.History);
            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.NativeCommand);
            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.Namespace);

            // CLEAR PATH SUGGESTION CONTENTS this.promptHandler.lastPathSuggestionDirectoryContents = null;

            IEnumerable<string> compositeFunctionsAndCommands;
            IEnumerable<string> history;

            bool skipNormal = false;

            if (populationFilter.Length > 0)
            {
                compositeFunctionsAndCommands = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.FindAllThatStartWith(populationFilter).Keys;
                history = InternalGlobals.CurrentProfile.History.FindAllThatStartWith(populationFilter, CaseSensitivity.Insensitive).Keys;
                int parameterIndex;

                if (populationInfo.ContextInfo != null
                    && populationInfo.ContextInfo.CurrentItemIsFunction
                    && (parameterIndex = SuggestionUtilities.GetCurrentParameterIndex(filter, filter.Length)) >= 0)
                {
                    Function function = populationInfo.ContextInfo.GetCurrentItem() as Function;
                    if (function != null)
                    {
                        skipNormal = true;

                        if (!this.PopulateFromFunction(function, populationInfo.ContextInfo.GetCurrentItemList(), filter, parameterIndex, populationInfo))
                        {
                            return false;
                        }
                    }
                }
                else if (populationInfo.ContextInfo != null
                    && populationInfo.ContextInfo.CurrentItemIsCommand
                    && (parameterIndex = SuggestionUtilities.GetCurrentParameterIndex(filter, filter.Length)) >= 0)
                {
                    Command command = populationInfo.ContextInfo.GetCurrentItem() as Command;
                    if (command != null)
                    {
                        skipNormal = true;

                        if (!this.PopulateFromCommand(command, populationInfo.ContextInfo.GetCurrentItemList(), filter, parameterIndex, populationInfo))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    int indexOfLastDirectorySeparatorChar = populationFilter.LastIndexOf(Path.DirectorySeparatorChar);
                    int indexOfFirstDirectorySeparatorChar = populationFilter.IndexOf(Path.DirectorySeparatorChar);

                    if (indexOfLastDirectorySeparatorChar > -1)
                    {
                        skipNormal = true;

                        if (!this.PopulateFromFileSystem(populationFilter, indexOfLastDirectorySeparatorChar, indexOfFirstDirectorySeparatorChar, populationInfo, false, null))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                compositeFunctionsAndCommands = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.AllKeys;
                history = InternalGlobals.CurrentProfile.History.Keys;
            }

            if (!skipNormal)
            {
                this.PopulateNormalItems(populationFilter, compositeFunctionsAndCommands, history, populationInfo);
            }

            return true;
        }
 
        private bool PopulateFromFileSystem(
            string populationFilter, 
            int indexOfLastDirectorySeparatorChar, 
            int indexOfFirstDirectorySeparatorChar,
            DoPopulationInfo populationInfo,
            bool escapeItemEntry,
            WildCardExpression fileFilter)
        {
            int indexOfFileImage = populationInfo.Suggester.Images.Count;
            populationInfo.Suggester.Images.Add(Images.File);
            string beginningFolderName = populationFilter.Substring(0, indexOfFirstDirectorySeparatorChar);
            string firstPartOfUserTyped = populationFilter.Substring(0, indexOfLastDirectorySeparatorChar + 1);

            FileSystemDirectory? directory = null;
            bool found;
            GroupedCompositeItem compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
            CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
            if (found && compositeItem != null && (parameterlessCommandNamedLikeFirstFolder = compositeItem.TryGetCommand(beginningFolderName, 0)) != null)
            {
                try
                {
                    ExecutionData executionData = new ExecutionData(
                        new string[0],
                        parameterlessCommandNamedLikeFirstFolder.ListFrom,
                        InternalGlobals.CurrentProfile.Lists);
                    FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
                    if (proposedDirectory.LooksValid)
                    {
                        directory = proposedDirectory + populationFilter.Substring(indexOfFirstDirectorySeparatorChar, indexOfLastDirectorySeparatorChar - indexOfFirstDirectorySeparatorChar + 1);
                    }
                }
                catch (ParseException)
                {
                }
                catch (ConversionException)
                {
                }
                catch (SelfReferencingCommandException)
                {
                }
            }

            if (directory == null)
            {
                directory = firstPartOfUserTyped;
            }

            bool addedFolderIcon = false;
            int indexOfFolderIcon = -1;

            List<SuggestionItem> directories = new List<SuggestionItem>();

            bool showHidden = InternalGlobals.CurrentProfile.ShowHiddenFilesAndFolders;
            bool showSystem = InternalGlobals.CurrentProfile.ShowSystemFilesAndFolders;

            if (InternalGlobals.CurrentProfile.AutoDetectFileAndFolderVisibility)
            {
                UIModel.SystemFileVisibilitySettings visibilitySettings = InternalGlobals.GuiManager.ToolkitHost.GetSystemFileVisibility();
                showHidden = visibilitySettings.ShowHiddenFiles;
                showSystem = visibilitySettings.ShowSystemFiles;
            }

            if (directory.Value.Exists)
            {
                try
                {
                    foreach (FileSystemDirectory subDirectory in directory.Value.GetDirectories())
                    {
                         if ((!showHidden && subDirectory.IsHidden) || (!showSystem && subDirectory.IsSystem))
                        {
                            continue;
                        }

                        string name = subDirectory.Name;
                        if (!populationInfo.SuggestionItemsAndIndexes.Contains(name, CaseSensitivity.Insensitive))
                        {
                            if (!addedFolderIcon)
                            {
                                if (this.folderIcon == null)
                                {
                                    Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractDirectoryIcon(subDirectory, this.iconSize);
                                    if (icon == null)
                                    {
                                        this.folderIcon = null;
                                    }
                                    else
                                    {
                                        this.folderIcon = InternalGlobals.GuiManager.ToolkitHost.ConvertImage(icon.ToBitmap());
                                        icon.Dispose();
                                    }

                                }

                                if (indexOfFolderIcon == -1)
                                {
                                    indexOfFolderIcon = populationInfo.Suggester.Images.Count;
                                    populationInfo.Suggester.Images.Add(this.folderIcon);
                                }
                                else
                                {
                                    populationInfo.Suggester.Images[indexOfFolderIcon] = this.folderIcon;
                                }

                                addedFolderIcon = this.folderIcon != null;
                            }

                            if (escapeItemEntry)
                            {
                                name = name.Escape();
                            }

                            directories.Add(new SuggestionItem(SuggestionItemType.Folder, name, indexOfFolderIcon));
                            populationInfo.SuggestionItemsAndIndexes.Add(name.ToUpperInvariant(), 0);
                            populationInfo.FolderSuggestions.Add(name);
                        }
                    }

                    this.fileExtensionsAndImageIndexes = new TrieDictionary<Int32Encapsulator>(SortMode.DecendingFromLastAdded);
                    List<SuggestionItem> files = new List<SuggestionItem>();

                    int nextImageIndex = populationInfo.Suggester.Images.Count;

                    foreach (FileSystemFile file in directory.Value.GetFiles())
                    {
                        if ((!showHidden && file.IsHidden) || (!showSystem && file.IsSystem) || (fileFilter != null && !fileFilter.IsMatch(file.Name)))
                        {
                            continue;
                        }

                        int imageIndex = -1;
                        string extension = file.Extension;
                        bool foundAlreadyLoadedIndex;
                        bool alwaysGetIcon = AlwaysGetIconFor.Contains(extension, CaseSensitivity.Insensitive);
                        Int32Encapsulator alreadyLoadedIndex = this.fileExtensionsAndImageIndexes.TryGetItem(extension, CaseSensitivity.Insensitive, out foundAlreadyLoadedIndex);
                        if (foundAlreadyLoadedIndex && alreadyLoadedIndex != null && !alwaysGetIcon)
                        {
                            imageIndex = alreadyLoadedIndex;
                        }
                        else
                        {
                            imageIndex = nextImageIndex;
                            nextImageIndex++;
                            populationInfo.IconLoadOrders.Enqueue(new FileIconLoadOrder(file, imageIndex));
                            if (!alwaysGetIcon)
                            {
                                this.fileExtensionsAndImageIndexes.Add(extension, imageIndex);
                            }
                        }

                        string name = file.Name;

                        if (escapeItemEntry)
                        {
                            name = name.Escape();
                        }

                        files.Add(new SuggestionItem(SuggestionItemType.File, name, imageIndex, 5));
                        populationInfo.SuggestionItemsAndIndexes.Add(name.ToUpperInvariant(), 0);
                    }

                    directories.Sort(SuggestionItemComparison);
                    files.Sort(SuggestionItemComparison);
                    populationInfo.SuggestionItems.AddListToComposite(directories);
                    populationInfo.SuggestionItems.AddListToComposite(files);
                }
                catch (UnauthorizedAccessException)
                {
                    string message;
                    if (Utilities.PromptuIsRunningElevated)
                    {
                        message = String.Format(
                            CultureInfo.CurrentCulture,
                            Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccessRunningElevated,
                            directory.Value);
                    }
                    else
                    {
                        message = String.Format(
                            CultureInfo.CurrentCulture,
                            Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccess,
                            directory.Value);
                    }

                    MessageBoxProvider.GiveError(message, this.suggestionHandler, populationInfo.Suggester, 0);

                    //ITextualInformationBox informationBox = new Default.DefaultInformationBox();
                    //PromptuSettings.CurrentSkin.ToolTipSettings.ApplyTo(informationBox);

                    //informationBox.InformationType = InformationType.Error;
                    //string message;
                    //if (Utilities.PromptuIsRunningElevated)
                    //{
                    //    message = String.Format(
                    //        Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccessRunningElevated,
                    //        directory.Value);
                    //}
                    //else
                    //{
                    //    message = String.Format(
                    //        Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccess,
                    //        directory.Value);
                    //}

                    //informationBox.Size = new Size(informationBox.Size.Height, 500);

                    //informationBox.Content.Font = PromptuFonts.UnauthorizedAccessFont;
                    //informationBox.Content.ForeColor = Color.Tomato;

                    //informationBox.Content.Text = XmlFormatLabel.GenerateXmlTag(message);

                    //informationBox.SizeAndPositionYourself(new Rectangle(this.suggestionHandler.Prompt.LocationOnScreen,
                    //    this.suggestionHandler.Prompt.PromptSize), new List<Rectangle>(), Size.Empty);

                    //this.suggestionHandler.InformationBoxMananger.RegisterAndShow(informationBox, true, populationInfo.Suggester);
                    this.filterAtLastPopulation = null;
                    return false;
                }
            }

            return true;
        }

        private void PopulateFromFunctionsAndCommands(
            IEnumerable<string> compositeFunctionsAndCommands, 
            string populationFilter, 
            int filterDotCount,
            TrieDictionary<SuggestionItem> standardItems,
            DoPopulationInfo populationInfo,
            bool escapeItemEntry,
            ref int? commandAndNamespaceIndex,
            ref int? functionAndNamespaceIndex,
            ref int? nativeCommandAndNamespaceIndex,
            ItemValidator validator)
        {
            //Trace.WriteLine("F&C pop start.");
            foreach (string key in compositeFunctionsAndCommands)
            {
                //Trace.WriteLine(String.Format("k:{0}", key));
                SuggestionItemType suggestionItemType = default(SuggestionItemType);
                string valueToAdd = key;
                //string keyToUpper;
                GroupedCompositeItem compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands[key];
                int? imageIndex = null;
                int? fallbackImageIndex = null;
                bool removePrevious = false;

                if (compositeItem != null)
                {
                    string[] split = key.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');
                    valueToAdd = split[filterDotCount];
                    //Trace.WriteLline(String.Format("vta:{0}", valueToAdd));
                    if (split.Length > filterDotCount + 1)
                    {
                        if (!validator.Include(populationFilter + valueToAdd + '.', SuggestionItemType.Namespace))
                        {
                            continue;
                        }

                        suggestionItemType = SuggestionItemType.Namespace;
                        imageIndex = 4;
                    }
                    else
                    {
                        if (compositeItem.Commands.Count > 0 && validator.Include(key, SuggestionItemType.Command))
                        {
                            CompositeItem<Command, List> commandItem = compositeItem.Commands[0];
                            FileSystemFile? iconFile = commandItem.Item.GetIconPath(commandItem.ListFrom);
                            //Bitmap commandIcon = commandItem.Item.GetIcon(commandItem.ListFrom);

                            suggestionItemType = SuggestionItemType.Command;
                            bool applyNamespaceOverlay = false;
                            if (InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null
                                && validator.Include(key, SuggestionItemType.Namespace))
                            {
                                applyNamespaceOverlay = true;
                                //if (commandIcon == null)
                                //{
                                    if (commandAndNamespaceIndex == null)
                                    {
                                        populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.CommandAndNamespace);
                                        commandAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
                                    }

                                    fallbackImageIndex = commandAndNamespaceIndex;
                                //}
                                //else
                                //{
                                    populationInfo.Suggester.Images.Add(null);
                                    //    InternalGlobals.GuiManager.ToolkitHost.Images.CreateCompositeWithNamespaceOverlay(commandIcon));
                                    imageIndex = populationInfo.Suggester.Images.Count - 1;
                                //}

                                removePrevious = true;
                                suggestionItemType |= SuggestionItemType.Namespace;

                                if (compositeItem.StringFunctions.Count > 0)
                                {
                                    suggestionItemType |= SuggestionItemType.Function;
                                }
                            }
                            else
                            {
                                //if (commandIcon == null)
                                //{
                                    fallbackImageIndex = 0; // command image index
                                //}
                                //else
                                //{
                                    populationInfo.Suggester.Images.Add(null);//InternalGlobals.GuiManager.ToolkitHost.ConvertImage(commandIcon));
                                    imageIndex = populationInfo.Suggester.Images.Count - 1;
                                //}
                            }

                            if (iconFile != null && InternalGlobals.CurrentProfile.ShowCommandTargetIcons)
                            {
                                populationInfo.IconLoadOrders.Enqueue(new CommandIconLoadOrder(iconFile.Value, applyNamespaceOverlay, imageIndex.Value));
                            }

                            if (compositeItem.StringFunctions.Count > 0
                                && validator.Include(key, SuggestionItemType.Function))
                            {
                                suggestionItemType |= SuggestionItemType.Function;
                            }
                        }
                        else if (compositeItem.StringFunctions.Count > 0 && validator.Include(key, SuggestionItemType.Function))
                        {
                            suggestionItemType = SuggestionItemType.Function;
                            if (InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null
                                && validator.Include(key, SuggestionItemType.Namespace))
                            {
                                if (functionAndNamespaceIndex == null)
                                {
                                    populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.FunctionAndNamespace);
                                    functionAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
                                }

                                imageIndex = functionAndNamespaceIndex;
                                removePrevious = true;
                                suggestionItemType |= SuggestionItemType.Namespace;
                            }
                            else
                            {
                                imageIndex = 1;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //else if ((keyToUpper = key.ToUpperInvariant()) == "SETUP" || keyToUpper == "QUIT")
                    //{
                    //    imageIndex = 4;
                    //}
                }
                else
                {
                    if (validator.Include(key, SuggestionItemType.NativePromptuCommand))
                    {
                        suggestionItemType = SuggestionItemType.NativePromptuCommand;
                        if (InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null
                            && validator.Include(key, SuggestionItemType.Namespace))
                        {
                            if (nativeCommandAndNamespaceIndex == null)
                            {
                                populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.NativeCommandAndNamespace);
                                nativeCommandAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
                            }

                            imageIndex = nativeCommandAndNamespaceIndex;
                            removePrevious = true;
                            suggestionItemType |= SuggestionItemType.Namespace;
                        }
                        else
                        {
                            imageIndex = 3;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (imageIndex != null)
                {
                    if (escapeItemEntry)
                    {
                        valueToAdd = valueToAdd.Escape();
                    }

                    if (removePrevious)
                    {
                        //Trace.WriteLine("remove-pre");
                        string removeKey = populationInfo.SuggestionItemsAndIndexes.TryFindWholeKey(valueToAdd, CaseSensitivity.Insensitive);
                        if (removeKey != null && removeKey.Length == valueToAdd.Length)
                        {
                            populationInfo.SuggestionItemsAndIndexes.Remove(removeKey);
                            standardItems.Remove(removeKey);
                        }
                    }

                    if (!populationInfo.SuggestionItemsAndIndexes.Contains(valueToAdd, CaseSensitivity.Insensitive))
                    {
                        string valueToAddUpper = valueToAdd.ToUpperInvariant();
                        standardItems.Add(valueToAddUpper, new SuggestionItem(suggestionItemType, valueToAdd, imageIndex.Value, fallbackImageIndex));
                        populationInfo.SuggestionItemsAndIndexes.Add(valueToAddUpper, 0);
                        //Trace.WriteLine(String.Format("added item", key, valueToAdd));
                    }
                }
            }

            //Trace.WriteLine("F&C pop end.");
        }

        private void PopulateNormalItems(string populationFilter, IEnumerable<string> compositeFunctionsAndCommands, IEnumerable<string> history, DoPopulationInfo populationInfo)
        {
            TrieDictionary<SuggestionItem> standardItems = new TrieDictionary<SuggestionItem>(SortMode.DecendingFromLastAdded);
            int filterDotCount = populationFilter.CountBreaks('.');

            int? commandAndNamespaceIndex = null;
            int? functionAndNamespaceIndex = null;
            int? nativeCommandAndNamespaceIndex = null;
            int? historyAndNamespaceIndex = null;

            //foreach (string key in compositeFunctionsAndCommands)
            //{
            //    SuggestionItemType suggestionItemType = default(SuggestionItemType);
            //    string valueToAdd = key;
            //    //string keyToUpper;
            //    GroupedCompositeItem compositeItem = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands[key];
            //    int? imageIndex = null;
            //    bool removePrevious = false;

            //    if (compositeItem != null)
            //    {
            //        string[] split = key.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');
            //        valueToAdd = split[filterDotCount];
            //        if (split.Length > filterDotCount + 1)
            //        {
            //            suggestionItemType = SuggestionItemType.Namespace;
            //            imageIndex = 4;
            //        }
            //        else
            //        {
            //            if (compositeItem.Commands.Count > 0)
            //            {
            //                suggestionItemType = SuggestionItemType.Command;
            //                if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null)
            //                {
            //                    if (commandAndNamespaceIndex == null)
            //                    {
            //                        populationInfo.Suggester.Images.Add(CompositeImages.CommandAndNamespace);
            //                        commandAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
            //                    }

            //                    imageIndex = commandAndNamespaceIndex;
            //                    removePrevious = true;
            //                    suggestionItemType |= SuggestionItemType.Namespace;

            //                    if (compositeItem.Functions.Count > 0)
            //                    {
            //                        suggestionItemType |= SuggestionItemType.Function;
            //                    }
            //                }
            //                else
            //                {
            //                    imageIndex = 0;
            //                }
            //            }
            //            else if (compositeItem.Functions.Count > 0)
            //            {
            //                suggestionItemType = SuggestionItemType.Function;
            //                if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null)
            //                {
            //                    if (functionAndNamespaceIndex == null)
            //                    {
            //                        populationInfo.Suggester.Images.Add(CompositeImages.FunctionAndNamespace);
            //                        functionAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
            //                    }

            //                    imageIndex = functionAndNamespaceIndex;
            //                    removePrevious = true;
            //                    suggestionItemType |= SuggestionItemType.Namespace;
            //                }
            //                else
            //                {
            //                    imageIndex = 1;
            //                }
            //            }
            //        }
            //        //else if ((keyToUpper = key.ToUpperInvariant()) == "SETUP" || keyToUpper == "QUIT")
            //        //{
            //        //    imageIndex = 4;
            //        //}
            //    }
            //    else
            //    {
            //        suggestionItemType = SuggestionItemType.NativePromptuCommand;
            //        if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null)
            //        {
            //            if (nativeCommandAndNamespaceIndex == null)
            //            {
            //                populationInfo.Suggester.Images.Add(CompositeImages.NativeCommandAndNamespace);
            //                nativeCommandAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
            //            }

            //            imageIndex = nativeCommandAndNamespaceIndex;
            //            removePrevious = true;
            //            suggestionItemType |= SuggestionItemType.Namespace;
            //        }
            //        else
            //        {
            //            imageIndex = 3;
            //        }
            //    }

            //    if (imageIndex != null)
            //    {
            //        if (removePrevious)
            //        {
            //            string removeKey = populationInfo.SuggestionItemsAndIndexes.TryFindWholeKey(valueToAdd, CaseSensitivity.Insensitive);
            //            if (removeKey != null && removeKey.Length == valueToAdd.Length)
            //            {
            //                populationInfo.SuggestionItemsAndIndexes.Remove(removeKey);
            //                standardItems.Remove(removeKey);
            //            }
            //        }

            //        if (!populationInfo.SuggestionItemsAndIndexes.Contains(valueToAdd, CaseSensitivity.Insensitive))
            //        {
            //            string valueToAddUpper = valueToAdd.ToUpperInvariant();
            //            standardItems.Add(valueToAddUpper, new SuggestionItem(suggestionItemType, valueToAdd, imageIndex.Value));
            //            populationInfo.SuggestionItemsAndIndexes.Add(valueToAddUpper, 0);
            //        }
            //    }
            //}

            this.PopulateFromFunctionsAndCommands(
                compositeFunctionsAndCommands, 
                populationFilter, 
                filterDotCount, 
                standardItems, 
                populationInfo,
                false,
                ref commandAndNamespaceIndex,
                ref functionAndNamespaceIndex,
                ref nativeCommandAndNamespaceIndex,
                new ItemValidator());

            this.PopulateFromHistory(
                history, 
                populationFilter, 
                filterDotCount, 
                standardItems, 
                InternalGlobals.CurrentProfile.History, 
                populationInfo, 
                null, 
                false,
                true,
                ref historyAndNamespaceIndex,
                true,
                true);

            //foreach (string historyItem in history)
            //{
            //    string item = historyItem;
            //    if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled && !PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(item)
            //        && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(item))
            //    {
            //        continue;
            //    }

            //    bool removePrevious = false;
            //    SuggestionItemType suggestionItemType = SuggestionItemType.History;
            //    HistoryDetails details = PromptuSettings.CurrentProfile.History[item, CaseSensitivity.Sensitive];
            //    string itemToAdd;
            //    int imageIndex = 2;

            //    bool isEnd = false;

            //    int[] breakBeginIndexes;

            //    if (item.OccurenceCountOf(Path.DirectorySeparatorChar) > 0 && details.IsValidPath)
            //    {
            //        int sectionNumber = populationFilter.OccurenceCountOf(Path.DirectorySeparatorChar);
            //        string partAdding = details.PathSplit[sectionNumber];
            //        if (filterDotCount == 0)
            //        {
            //            continue;
            //        }

            //        string[] split = partAdding.BreakApart(Quotes.None, Spaces.None, BreakingCharAction.Eat, out breakBeginIndexes, '.');
            //        itemToAdd = split[filterDotCount];

            //        if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled)
            //        {
            //            string firstItemOfPathSplit = details.PathSplit[0];
            //            if (!PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(firstItemOfPathSplit)
            //                && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(firstItemOfPathSplit))
            //            {
            //                continue;
            //            }
            //        }

            //        if (split.Length > filterDotCount + 1)
            //        {
            //            if (item.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
            //            {
            //                suggestionItemType = SuggestionItemType.Namespace;
            //                imageIndex = 4;
            //            }
            //            else
            //            {
            //                isEnd = true;
            //            }
            //        }
            //        else
            //        {
            //            isEnd = true;
            //            if (sectionNumber == 0 && populationFilter.Length == 0)
            //            {
            //                if (!itemToAdd.EndsWith(PathSeparatorString) && !PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(itemToAdd))
            //                {
            //                    itemToAdd += Path.DirectorySeparatorChar;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        char? breakChar;
            //        int indexOfFirstBreak = item.IndexOfNextBreakingChar(false, out breakChar, ' ', '(');

            //        if (breakChar == '(')
            //        {
            //            item = item.Substring(0, indexOfFirstBreak);
            //        }

            //        string[] split = item.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, out breakBeginIndexes, '.');


            //        itemToAdd = split[filterDotCount];
            //        if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled)
            //        {
            //            string firstPart;
            //            if (indexOfFirstBreak > -1)
            //            {
            //                firstPart = item.Substring(0, indexOfFirstBreak);
            //            }
            //            else
            //            {
            //                firstPart = item;
            //            }

            //            if (!PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(firstPart)
            //                && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(firstPart))
            //            {
            //                continue;
            //            }
            //        }

            //        if (item.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
            //        {
            //            suggestionItemType = SuggestionItemType.Namespace;
            //            imageIndex = 4;
            //        }
            //        else
            //        {
            //            isEnd = true;
            //        }
            //    }

            //    if (isEnd)
            //    {
            //        bool found;
            //        string fullName = populationFilter + itemToAdd;
            //        PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(fullName, out found);
            //        if (!found && PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(fullName + '.') != null)
            //        {
            //            if (historyAndNamespaceIndex == null)
            //            {
            //                populationInfo.Suggester.Images.Add(CompositeImages.HistoryAndNamespace);
            //                historyAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
            //            }

            //            imageIndex = historyAndNamespaceIndex.Value;
            //            removePrevious = true;
            //            suggestionItemType |= SuggestionItemType.Namespace;
            //        }
            //    }

            //    if (removePrevious)
            //    {
            //        string removeKey = populationInfo.SuggestionItemsAndIndexes.TryFindWholeKey(itemToAdd, CaseSensitivity.Insensitive);
            //        if (removeKey != null && removeKey.Length == itemToAdd.Length)
            //        {
            //            populationInfo.SuggestionItemsAndIndexes.Remove(removeKey);
            //            standardItems.Remove(removeKey);
            //        }
            //    }

            //    if (!populationInfo.SuggestionItemsAndIndexes.Contains(itemToAdd, CaseSensitivity.Insensitive))
            //    {
            //        standardItems.Add(itemToAdd, new SuggestionItem(suggestionItemType, itemToAdd, imageIndex));
            //        populationInfo.SuggestionItemsAndIndexes.Add(itemToAdd.ToUpperInvariant(), 0);
            //    }
            //}

            standardItems.SortKeys();
            populationInfo.SuggestionItems.AddListToComposite(new OptimizedStringKeyedDictionaryListAbstractionLayer<SuggestionItem>(standardItems));
        }

        private void PopulateFromValueList(string filter, int filterDotCount, TrieDictionary<SuggestionItem> standardItems, ValueList valueList, DoPopulationInfo populationInfo, bool escapeItemEntry)
        {
            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.Namespace);
            int namespaceImageIndex = populationInfo.Suggester.Images.Count - 1;

            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.Command);
            int defaultImageIndex = populationInfo.Suggester.Images.Count - 1;
            
            int? itemImageAndNamespaceIndex = null;

            List<SuggestionItem> offsetNeededItems = new List<SuggestionItem>();

            Dictionary<int, int> overlayMap = new Dictionary<int, int>();

            foreach (ValueListItem item in valueList)
            {
                if (valueList.UseNamespaceInterpretation && !item.Value.StartsWith(filter))
                {
                    continue;
                }

                string itemToAdd;
                bool removePrevious = false;
                SuggestionItemType suggestionItemType = SuggestionItemType.ValueListItem;
                int imageIndex = item.ImageIndex;

                bool addToOffsetNeeded = true;

                if (valueList.UseNamespaceInterpretation)
                {
                    int[] breakBeginIndexes;
                    string[] split = item.Value.BreakApart(Quotes.None, Spaces.None, BreakingCharAction.Eat, out breakBeginIndexes, '.');

                    if (filterDotCount >= split.Length)
                    {
                        continue;
                    }

                    itemToAdd = split[filterDotCount];

                    if (item.Value.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
                    {
                        addToOffsetNeeded = false;
                        suggestionItemType = SuggestionItemType.Namespace;
                        imageIndex = namespaceImageIndex;
                    }
                    else
                    {
                        if (valueList.TryFindValue(item.Value + '.') != null)
                        {
                            addToOffsetNeeded = false;

                            bool useRegularItem = false;

                            if (valueList.Images.IndexIsDefined(imageIndex))
                            {
                                Image image = valueList.Images[imageIndex];
                                if (image == null)
                                {
                                    useRegularItem = true;
                                }
                                else
                                {
                                    if (!overlayMap.ContainsKey(imageIndex))
                                    {
                                        populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.CreateCompositeWithNamespaceOverlay(image.ConvertToBitmap()));
                                        overlayMap.Add(imageIndex, populationInfo.Suggester.Images.Count - 1);
                                    }

                                    imageIndex = overlayMap[imageIndex];
                                }
                            }
                            else
                            {
                                useRegularItem = true;
                            }

                            if (useRegularItem)
                            {
                                if (itemImageAndNamespaceIndex == null)
                                {
                                    populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.CommandAndNamespace);
                                    itemImageAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
                                }

                                imageIndex = itemImageAndNamespaceIndex.Value;
                            }

                            removePrevious = true;
                            suggestionItemType |= SuggestionItemType.Namespace;
                        }
                    }
                }
                else
                {
                    itemToAdd = item.Value;
                }

                if (escapeItemEntry)
                {
                    itemToAdd = itemToAdd.Escape();
                }

                if (removePrevious)
                {
                    string removeKey = populationInfo.SuggestionItemsAndIndexes.TryFindWholeKey(itemToAdd, CaseSensitivity.Insensitive);
                    if (removeKey != null && removeKey.Length == itemToAdd.Length)
                    {
                        populationInfo.SuggestionItemsAndIndexes.Remove(removeKey);
                        standardItems.Remove(removeKey);
                    }
                }

                if (!populationInfo.SuggestionItemsAndIndexes.Contains(itemToAdd, CaseSensitivity.Insensitive))
                {
                    SuggestionItem suggestionItem = new SuggestionItem(suggestionItemType, itemToAdd, imageIndex);
                    standardItems.Add(itemToAdd, suggestionItem);
                    populationInfo.SuggestionItemsAndIndexes.Add(itemToAdd, 0);

                    if (addToOffsetNeeded)
                    {
                        if (valueList.Images.IndexIsDefined(imageIndex))
                        {
                            offsetNeededItems.Add(suggestionItem);
                        }
                        else
                        {
                            suggestionItem.ImageIndex = defaultImageIndex;
                        }
                    }
                }
            }

            int offset = populationInfo.Suggester.Images.Count;

            foreach (SuggestionItem item in offsetNeededItems)
            {
                if (item.ImageIndex >= 0)
                {
                    item.ImageIndex += offset;
                }
            }

            foreach (Image image in valueList.Images)
            {
                populationInfo.Suggester.Images.Add(image);
            }
        }

        private void PopulateFromHistory(
            IEnumerable<string> history, 
            string populationFilter, 
            int filterDotCount, 
            TrieDictionary<SuggestionItem> standardItems,
            TrieDictionary<HistoryDetails> historyDetailsLookup,
            DoPopulationInfo populationInfo,
            Filter<string, HistoryDetails> itemFilter,
            bool escapeItemEntry,
            bool allowFileSystem,
            ref int? historyAndNamespaceIndex,
            bool accountForNonHistory,
            bool allowNamespaces)
        {
            int sectionNumber = populationFilter.OccurenceCountOf(Path.DirectorySeparatorChar);

            foreach (string historyItem in history)
            {
                string item = historyItem;
                if (InternalGlobals.CurrentProfile.Lists.AnyListsAreDisabled && !InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(item)
                    && InternalGlobals.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(item))
                {
                    continue;
                }

                bool removePrevious = false;
                SuggestionItemType suggestionItemType = SuggestionItemType.History;
                HistoryDetails details = historyDetailsLookup[item, CaseSensitivity.Sensitive];
                item = details.EntryValue;

                if (itemFilter != null)
                {
                    if (!itemFilter.IsValid(historyItem, details))
                    {
                        continue;
                    }

                    item = itemFilter.TranslateKey(item, details);
                }
                else
                {
                }

                string itemToAdd;
                int imageIndex = 2;

                bool isEnd = false;

                int[] breakBeginIndexes;

                if (allowFileSystem && !Function.IsInFunctionSyntax(item) && item.OccurenceCountOf(Path.DirectorySeparatorChar) > 0 && details.IsValidPath)
                {
                    string partAdding = details.PathSplit[sectionNumber];

                    string[] split = partAdding.BreakApart(Quotes.None, Spaces.None, BreakingCharAction.Eat, out breakBeginIndexes, '.');

                    if (sectionNumber == 0 && split.Length >= 0 || !split[0].Contains(PathSeparatorString))
                    {
                        continue;
                    }

                    if (filterDotCount >= split.Length)
                    {
                        continue;
                    }

                    itemToAdd = split[filterDotCount];

                    if (InternalGlobals.CurrentProfile.Lists.AnyListsAreDisabled)
                    {
                        string firstItemOfPathSplit = details.PathSplit[0];
                        if (!InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(firstItemOfPathSplit)
                            && InternalGlobals.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(firstItemOfPathSplit))
                        {
                            continue;
                        }
                    }

                    if (split.Length > filterDotCount + 1)
                    {
                        if (item.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
                        {
                            suggestionItemType = SuggestionItemType.Namespace;
                            imageIndex = 4;
                        }
                        else
                        {
                            isEnd = true;
                        }
                    }
                    else
                    {
                        isEnd = true;
                        if (sectionNumber == 0 && populationFilter.Length == 0)
                        {
                            if (!itemToAdd.EndsWith(PathSeparatorString) && !InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(itemToAdd))
                            {
                                itemToAdd += Path.DirectorySeparatorChar;
                            }
                        }
                    }
                }
                else
                {
                    char? breakChar;
                    int indexOfFirstBreak = item.IndexOfNextBreakingChar(false, out breakChar, ' ', '(');

                    if (breakChar == '(')
                    {
                        item = item.Substring(0, indexOfFirstBreak);
                    }

                    string[] split = item.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, out breakBeginIndexes, '.');

                    if (allowNamespaces)
                    {
                        if (filterDotCount >= split.Length)
                        {
                            continue;
                        }

                        itemToAdd = split[filterDotCount];
                    }
                    else
                    {
                        itemToAdd = item;
                    }

                    if (InternalGlobals.CurrentProfile.Lists.AnyListsAreDisabled)
                    {
                        string firstPart;
                        if (indexOfFirstBreak > -1)
                        {
                            firstPart = item.Substring(0, indexOfFirstBreak);
                        }
                        else
                        {
                            firstPart = item;
                        }

                        if (!InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(firstPart)
                            && InternalGlobals.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(firstPart))
                        {
                            continue;
                        }
                    }

                    if (allowNamespaces && item.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
                    {
                        suggestionItemType = SuggestionItemType.Namespace;
                        imageIndex = 4;
                    }
                    else
                    {
                        isEnd = true;
                    }
                }

                if (isEnd && allowNamespaces && accountForNonHistory)
                {
                    bool found;
                    string fullName = populationFilter + itemToAdd;
                    InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(fullName, out found);
                    if (!found && InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryFind(fullName + '.') != null)
                    {
                        if (historyAndNamespaceIndex == null)
                        {
                            populationInfo.Suggester.Images.Add(InternalGlobals.GuiManager.ToolkitHost.Images.HistoryAndNamespace);
                            historyAndNamespaceIndex = populationInfo.Suggester.Images.Count - 1;
                        }

                        imageIndex = historyAndNamespaceIndex.Value;
                        removePrevious = true;
                        suggestionItemType |= SuggestionItemType.Namespace;
                    }
                }

                if (escapeItemEntry)
                {
                    itemToAdd = itemToAdd.Escape();
                }

                if (removePrevious)
                {
                    string removeKey = populationInfo.SuggestionItemsAndIndexes.TryFindWholeKey(itemToAdd, CaseSensitivity.Insensitive);
                    if (removeKey != null && removeKey.Length == itemToAdd.Length)
                    {
                        populationInfo.SuggestionItemsAndIndexes.Remove(removeKey);
                        standardItems.Remove(removeKey);
                    }
                }

                if (!populationInfo.SuggestionItemsAndIndexes.Contains(itemToAdd, CaseSensitivity.Insensitive))
                {
                    standardItems.Add(itemToAdd, new SuggestionItem(suggestionItemType, itemToAdd, imageIndex));
                    populationInfo.SuggestionItemsAndIndexes.Add(itemToAdd.ToUpperInvariant(), 0);
                }
            }
        }

        //private bool PopulateSuggester(bool resizePrompt, bool populateFirstLevel)
        //{
            //if (!populateFirstLevel)
            //{
            //    if (this.filter == this.filterAtLastPopulation && this.suggestionHandler.Pr.suggestionMode == this.suggestionModeAsLastPopulation)
            //    {
            //        return true;
            //    }
            //}

            //this.promptHandler.alternateSuggestionBase = null;

            //int start = Environment.TickCount;
            //this.CancelIconLoading();
            //PromptuSettings.SyncSynchronizer.PauseSyncs();
            //PromptuSettings.SyncSynchronizer.WaitUntilAllSyncsPaused();
            //this.iconLoadOrders.Clear();
            //this.suggester.ClearItems();
            //this.suggester.Images.Clear();

            //this.suggestionModeAsLastPopulation = this.promptHandler.suggestionMode;

            //CompositeList<SuggestionItem> suggestionItems;

            //if (this.promptHandler.suggestionMode == SuggestionMode.Normal)
            //{
            //    if (!populateFirstLevel && this.filter.Length == 0 && this.firstLevelImages != null && this.firstLevelSuggestionItems != null && this.firstLevelSuggestionItemsAndIndexes != null)
            //    {
            //        foreach (Image image in this.firstLevelImages)
            //        {
            //            this.suggester.Images.Add(image);
            //        }

            //        suggestionItems = this.firstLevelSuggestionItems;
            //        this.suggestionItemsAndIndexes = this.firstLevelSuggestionItemsAndIndexes;
            //    }
                //else
                //{
                    //string populationFilter;
                    //if (populateFirstLevel)
                    //{
                    //    populationFilter = String.Empty;
                    //    this.filterAtLastPopulation = null;
                    //}
                    //else
                    //{
                    //    //int indexOfLastBreakingChar; 
                    //    //if (this.filter.Length > 0 && (indexOfLastBreakingChar = this.filter.LastIndexOfBreakingChar(true, '(', ',')) > -1)
                    //    //{
                    //    //    populationFilter = this.filter.Substring(indexOfLastBreakingChar).TrimStart();
                    //    //}
                    //    //else
                    //    //{
                    //    populationFilter = this.filter;
                    //    //}

                    //    this.filterAtLastPopulation = this.filter;
                    //}

                    //this.suggester.Images.Add(Images.Command);
                    //this.suggester.Images.Add(Images.Function);
                    //this.suggester.Images.Add(Images.History);
                    //this.suggester.Images.Add(Images.NativeCommand);
                    //this.suggester.Images.Add(Images.Namespace);
                    //List<SuggestionItem> items = new List<SuggestionItem>();
                    //suggestionItems = new CompositeList<SuggestionItem>();
                    //this.suggestionItemsAndIndexes = new OptimizedStringKeyedDictionary<Int32Encapsulator>(SortMode.DecendingFromLastAdded);
                    //this.folderSuggestions = new FindOptimizedStringCollection(SortMode.DecendingFromLastAdded);
                    //this.promptHandler.lastPathSuggestionDirectoryContents = null;
                    //IEnumerable<string> compositeFunctionsAndCommands;
                    //IEnumerable<string> history;

                    //bool skipNormal = false;

                    //if (populationFilter.Length > 0)
                    //{
                    //    compositeFunctionsAndCommands = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.FindAllThatStartWith(populationFilter).Keys;
                    //    history = PromptuSettings.CurrentProfile.History.FindAllThatStartWith(populationFilter, CaseSensitivity.Insensitive).Keys;

                        //int indexOfLastDirectorySeparatorChar = populationFilter.LastIndexOf(Path.DirectorySeparatorChar);
                        //int indexOfFirstDirectorySeparatorChar = populationFilter.IndexOf(Path.DirectorySeparatorChar);
                        //if (indexOfLastDirectorySeparatorChar > -1)
                        //{
                            //skipNormal = true;
                            //this.suggester.Images.Add(Images.File);
                            //string beginningFolderName = populationFilter.Substring(0, indexOfFirstDirectorySeparatorChar);
                            //string firstPartOfUserTyped = populationFilter.Substring(0, indexOfLastDirectorySeparatorChar + 1);

                            //FileSystemDirectory? directory = null;
                            //bool found;
                            //GroupedCompositeItem compositeItem = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
                            //CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
                            //if (found && compositeItem != null && (parameterlessCommandNamedLikeFirstFolder = compositeItem.TryGetCommand(beginningFolderName, 0)) != null)
                            //{
                            //    try
                            //    {
                            //        ExecutionData executionData = new ExecutionData(
                            //            new string[0],
                            //            parameterlessCommandNamedLikeFirstFolder.ListFrom,
                            //            PromptuSettings.CurrentProfile.Lists);
                            //        FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
                            //        if (proposedDirectory.Exists)
                            //        {
                            //            directory = proposedDirectory + populationFilter.Substring(indexOfFirstDirectorySeparatorChar, indexOfLastDirectorySeparatorChar - indexOfFirstDirectorySeparatorChar + 1);
                            //        }
                            //    }
                            //    catch (Itl.ParseException)
                            //    {
                            //    }
                            //    catch (ConversionException)
                            //    {
                            //    }
                            //}

                            //if (directory == null)
                            //{
                            //    directory = firstPartOfUserTyped;
                            //}

                            //bool addedFolderIcon = false;
                            //int indexOfFolderIcon = -1;

                            //List<SuggestionItem> directories = new List<SuggestionItem>();

                            //bool showHidden = PromptuSettings.CurrentProfile.NavigateHiddenFilesAndFolders;
                            //bool showSystem = PromptuSettings.CurrentProfile.NavigateSystemFilesAndFolders;

                            //if (directory.Value.Exists)
                            //{
                            //    //int startLoadDir = System.Environment.TickCount;
                            //    try
                            //    {
                            //        foreach (FileSystemDirectory subDirectory in directory.Value.GetDirectories())
                            //        {
                                        //bool hidden = (subDirectory.Attributes & FileAttributes.Hidden) != 0;
                                        //bool system = (
                                    //    if ((!showHidden && subDirectory.IsHidden) || (!showSystem && subDirectory.IsSystem))
                                    //    {
                                    //        continue;
                                    //    }

                                    //    string name = subDirectory.Name;
                                    //    if (!this.suggestionItemsAndIndexes.Contains(name, CaseSensitivity.Insensitive))
                                    //    {
                                    //        if (!addedFolderIcon)
                                    //        {
                                    //            if (this.folderIcon == null)
                                    //            {
                                    //                Icon icon = Utilities.ExtractDirectoryIcon(subDirectory, IconSize.Small, true);
                                    //                if (icon == null)
                                    //                {
                                    //                    this.folderIcon = null;
                                    //                }
                                    //                else
                                    //                {
                                    //                    this.folderIcon = icon.ToBitmap();
                                    //                    icon.Dispose();
                                    //                }

                                    //            }

                                    //            if (indexOfFolderIcon == -1)
                                    //            {
                                    //                this.suggester.Images.Add(this.folderIcon);
                                    //                indexOfFolderIcon = this.suggester.Images.Count - 1;
                                    //            }
                                    //            else
                                    //            {
                                    //                this.suggester.Images[indexOfFolderIcon] = this.folderIcon;
                                    //            }

                                    //            addedFolderIcon = this.folderIcon != null;
                                    //        }

                                    //        directories.Add(new SuggestionItem(SuggestionItemType.Folder, name, indexOfFolderIcon));
                                    //        this.suggestionItemsAndIndexes.Add(name, 0);
                                    //        this.folderSuggestions.Add(name);
                                    //    }
                                    //}


                                    //int totalTime = Environment.TickCount - startLoadDir;

                                //    this.fileExtensionsAndImageIndexes = new OptimizedStringKeyedDictionary<Int32Encapsulator>(SortMode.DecendingFromLastAdded);
                                //    List<SuggestionItem> files = new List<SuggestionItem>();

                                //    int nextImageIndex = this.suggester.Images.Count;

                                //    foreach (FileSystemFile file in directory.Value.GetFiles())
                                //    {
                                //        if ((!showHidden && file.IsHidden) || (!showSystem && file.IsSystem))
                                //        {
                                //            continue;
                                //        }

                                //        int imageIndex = -1;
                                //        string extension = file.Extension;
                                //        bool foundAlreadyLoadedIndex;
                                //        bool alwaysGetIcon = AlwaysGetIconFor.Contains(extension, CaseSensitivity.Insensitive);
                                //        Int32Encapsulator alreadyLoadedIndex = this.fileExtensionsAndImageIndexes.TryGetItem(extension, CaseSensitivity.Insensitive, out foundAlreadyLoadedIndex);
                                //        if (foundAlreadyLoadedIndex && alreadyLoadedIndex != null && !alwaysGetIcon)
                                //        {
                                //            imageIndex = alreadyLoadedIndex;
                                //        }
                                //        else
                                //        {
                                //            //bool foundCached = false;
                                //            //Bitmap bitmap = null;
                                //            //if (!alwaysGetIcon)
                                //            //{
                                //            //    Int32Encapsulator index = this.fileExtensionsAndImageIndexes.TryGetItem(extension, CaseSensitivity.Insensitive, out foundCached);
                                //            //    if (index == null)
                                //            //    {
                                //            //        foundCached = false;
                                //            //    }
                                //            //    else
                                //            //    {
                                //            //        imageIndex = index;
                                //            //    }
                                //            //}

                                //            //if (!foundCached)
                                //            //{
                                //            imageIndex = nextImageIndex;
                                //            nextImageIndex++;
                                //            this.iconLoadOrders.Enqueue(new IconLoadOrder(file, imageIndex));
                                //            if (!alwaysGetIcon)
                                //            {
                                //                this.fileExtensionsAndImageIndexes.Add(extension, imageIndex);
                                //            }
                                //            //}

                                //            //this.suggester.Images.Add(bitmap);

                                //            //if (!alwaysGetIcon)
                                //            //{
                                //            //    this.fileExtensionsAndImageIndexes.Add(extension, imageIndex);
                                //            //}
                                //        }

                                //        string name = file.Name;

                                //        files.Add(new SuggestionItem(SuggestionItemType.File, name, imageIndex, 5));
                                //        this.suggestionItemsAndIndexes.Add(name, 0);
                                //    }

                                //    directories.Sort(SuggestionItemComparison);
                                //    files.Sort(SuggestionItemComparison);
                                //    suggestionItems.AddListToComposite(directories);
                                //    suggestionItems.AddListToComposite(files);
                                //}
                                //catch (UnauthorizedAccessException)
                                //{
                                //    IInformationBox informationBox = new Default.DefaultInformationBox();

                                //    informationBox.InformationType = InformationType.Error;
                                //    string message;
                                //    if (Utilities.PromptuIsRunningElevated)
                                //    {
                                //        message = String.Format(
                                //            Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccessRunningElevated,
                                //            directory.Value);
                                //    }
                                //    else
                                //    {
                                //        message = String.Format(
                                //            Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccess,
                                //            directory.Value);
                                //    }

                                //    informationBox.Size = new Size(informationBox.Size.Height, 500);

                                //    //informationBox.RichTextBox.Text = message;
                                //    //informationBox.
                                //    informationBox.Content.Font = PromptuFonts.UnauthorizedAccessFont;
                                //    informationBox.Content.ForeColor = Color.Tomato;

                                //    informationBox.Content.Text = XmlFormatLabel.GenerateXmlTag(message);

                                //    //informationBox.RichTextBox.SelectAll();
                                //    //informationBox.RichTextBox.SelectionFont = PromptuFonts.UnauthorizedAccessFont;
                                //    //informationBox.RichTextBox.SelectionColor = Color.Tomato;
                                //    //informationBox.RichTextBox.SelectionLength = 0;

                                //    //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(message, , Color.Tomato), 0, 0);

                                //    informationBox.SizeAndPositionYourself(new Rectangle(this.promptHandler.prompt.LocationOnScreen,
                                //        this.promptHandler.prompt.PromptSize), new List<Rectangle>(), Size.Empty);

                                //    this.informationBoxManager.RegisterAndShow(informationBox, true);
                                //    this.filterAtLastPopulation = null;
                                //    return false;
                                //}
                        //    }
                        //}
                    //}
                    //else
                    //{
                    //    compositeFunctionsAndCommands = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.AllKeys;
                    //    history = PromptuSettings.CurrentProfile.History.AllKeys;
                    //}

                    //if (!skipNormal)
                    //{
                        //OptimizedStringKeyedDictionary<SuggestionItem> standardItems = new OptimizedStringKeyedDictionary<SuggestionItem>(SortMode.DecendingFromLastAdded);
                        //int filterDotCount = populationFilter.CountBreaks('.');

                        //int? commandAndNamespaceIndex = null;
                        //int? functionAndNamespaceIndex = null;
                        //int? nativeCommandAndNamespaceIndex = null;
                        //int? historyAndNamespaceIndex = null;

                        //foreach (string key in compositeFunctionsAndCommands)
                        //{
                        //    SuggestionItemType suggestionItemType = default(SuggestionItemType);
                        //    string valueToAdd = key;
                        //    //string keyToUpper;
                        //    GroupedCompositeItem compositeItem = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands[key];
                        //    int? imageIndex = null;
                        //    bool removePrevious = false;

                        //    if (compositeItem != null)
                        //    {
                        //        string[] split = key.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');
                        //        valueToAdd = split[filterDotCount];
                        //        if (split.Length > filterDotCount + 1)
                        //        {
                        //            suggestionItemType = SuggestionItemType.Namespace;
                        //            imageIndex = 4;
                        //        }
                        //        else
                        //        {
                        //            if (compositeItem.Commands.Count > 0)
                        //            {
                        //                suggestionItemType = SuggestionItemType.Command;
                        //                if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null)
                        //                {
                        //                    if (commandAndNamespaceIndex == null)
                        //                    {
                        //                        this.suggester.Images.Add(CompositeImages.CommandAndNamespace);
                        //                        commandAndNamespaceIndex = this.suggester.Images.Count - 1;
                        //                    }

                        //                    imageIndex = commandAndNamespaceIndex;
                        //                    removePrevious = true;
                        //                    suggestionItemType |= SuggestionItemType.Namespace;

                        //                    if (compositeItem.Functions.Count > 0)
                        //                    {
                        //                        suggestionItemType |= SuggestionItemType.Function;
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    imageIndex = 0;
                        //                }
                        //            }
                        //            else if (compositeItem.Functions.Count > 0)
                        //            {
                        //                suggestionItemType = SuggestionItemType.Function;
                        //                if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null)
                        //                {
                        //                    if (functionAndNamespaceIndex == null)
                        //                    {
                        //                        this.suggester.Images.Add(CompositeImages.FunctionAndNamespace);
                        //                        functionAndNamespaceIndex = this.suggester.Images.Count - 1;
                        //                    }

                        //                    imageIndex = functionAndNamespaceIndex;
                        //                    removePrevious = true;
                        //                    suggestionItemType |= SuggestionItemType.Namespace;
                        //                }
                        //                else
                        //                {
                        //                    imageIndex = 1;
                        //                }
                        //            }
                        //        }
                        //        //else if ((keyToUpper = key.ToUpperInvariant()) == "SETUP" || keyToUpper == "QUIT")
                        //        //{
                        //        //    imageIndex = 4;
                        //        //}
                        //    }
                        //    else
                        //    {
                        //        suggestionItemType = SuggestionItemType.NativePromptuCommand;
                        //        if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(populationFilter + valueToAdd + '.') != null)
                        //        {
                        //            if (nativeCommandAndNamespaceIndex == null)
                        //            {
                        //                this.suggester.Images.Add(CompositeImages.NativeCommandAndNamespace);
                        //                nativeCommandAndNamespaceIndex = this.suggester.Images.Count - 1;
                        //            }

                        //            imageIndex = nativeCommandAndNamespaceIndex;
                        //            removePrevious = true;
                        //            suggestionItemType |= SuggestionItemType.Namespace;
                        //        }
                        //        else
                        //        {
                        //            imageIndex = 3;
                        //        }
                        //    }

                        //    if (imageIndex != null)
                        //    {
                        //        if (removePrevious)
                        //        {
                        //            string removeKey = this.suggestionItemsAndIndexes.TryFindWholeKey(valueToAdd, CaseSensitivity.Insensitive);
                        //            if (removeKey != null && removeKey.Length == valueToAdd.Length)
                        //            {
                        //                this.suggestionItemsAndIndexes.Remove(removeKey);
                        //                standardItems.Remove(removeKey);
                        //            }
                        //        }

                        //        if (!this.suggestionItemsAndIndexes.Contains(valueToAdd, CaseSensitivity.Insensitive))
                        //        {
                        //            standardItems.Add(valueToAdd, new SuggestionItem(suggestionItemType, valueToAdd, imageIndex.Value));
                        //            this.suggestionItemsAndIndexes.Add(valueToAdd, 0);
                        //        }
                        //    }
                        //}

                        //foreach (string historyItem in history)
                        //{
                        //    string item = historyItem;
                        //    if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled && !PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(item)
                        //        && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(item))
                        //    {
                        //        continue;
                        //    }

                        //    bool removePrevious = false;
                        //    SuggestionItemType suggestionItemType = SuggestionItemType.History;
                        //    HistoryDetails details = PromptuSettings.CurrentProfile.History[item, CaseSensitivity.Sensitive];
                        //    string itemToAdd;
                        //    int imageIndex = 2;

                        //    bool isEnd = false;

                        //    //string[] pathSplit;

                        //    int[] breakBeginIndexes;

                        //    if (item.OccurenceCountOf(Path.DirectorySeparatorChar) > 0 && details.IsValidPath)//Utilities.IsValidPath(item, out pathSplit))
                        //    {
                        //        int sectionNumber = populationFilter.OccurenceCountOf(Path.DirectorySeparatorChar);
                        //        string partAdding = details.PathSplit[sectionNumber];
                        //        //int filterDotCount = populationFilter.OccurenceCountOf('.');
                        //        if (filterDotCount == 0)
                        //        {
                        //            continue;
                        //        }

                        //        string[] split = partAdding.BreakApart(Quotes.None, Spaces.None, BreakingCharAction.Eat, out breakBeginIndexes, '.');
                        //        itemToAdd = split[filterDotCount];
                        //        //int index = 0;
                        //        //for (int i = 0; i <= filterDotCount; i++)
                        //        //{
                        //        //    index += split[filterDotCount].Length;
                        //        //}string firstItemOfSplit = split[0];
                        //        if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled)
                        //        {
                        //            string firstItemOfPathSplit = details.PathSplit[0];
                        //            if (!PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(firstItemOfPathSplit)
                        //                && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(firstItemOfPathSplit))
                        //            {
                        //                continue;
                        //            }
                        //        }

                        //        if (split.Length > filterDotCount + 1)
                        //        {
                        //            if (item.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
                        //            {
                        //                suggestionItemType = SuggestionItemType.Namespace;
                        //                imageIndex = 4;
                        //            }
                        //            else
                        //            {
                        //                isEnd = true;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            isEnd = true;
                        //            if (sectionNumber == 0 && populationFilter.Length == 0)
                        //            {
                        //                if (!itemToAdd.EndsWith(PathSeparatorString) && !PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(itemToAdd))
                        //                {
                        //                    itemToAdd += Path.DirectorySeparatorChar;
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        char? breakChar;
                        //        int indexOfFirstBreak = item.IndexOfNextBreakingChar(false, out breakChar, ' ', '(');

                        //        if (breakChar == '(')
                        //        {
                        //            item = item.Substring(0, indexOfFirstBreak);
                        //        }

                        //        string[] split = item.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, out breakBeginIndexes, '.');


                        //        itemToAdd = split[filterDotCount];
                        //        if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled)
                        //        {
                        //            string firstPart;
                        //            if (indexOfFirstBreak > -1)
                        //            {
                        //                firstPart = item.Substring(0, indexOfFirstBreak);
                        //            }
                        //            else
                        //            {
                        //                firstPart = item;
                        //            }

                        //            if (!PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(firstPart)
                        //                && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(firstPart))
                        //            {
                        //                continue;
                        //            }
                        //        }

                        //        if (item.NextCharIs('.', breakBeginIndexes[filterDotCount] + itemToAdd.Length, true))
                        //        {
                        //            suggestionItemType = SuggestionItemType.Namespace;
                        //            imageIndex = 4;
                        //        }
                        //        else
                        //        {
                        //            isEnd = true;
                        //        }
                        //    }

                        //    if (isEnd)
                        //    {
                        //        bool found;
                        //        string fullName = populationFilter + itemToAdd;
                        //        PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(fullName, out found);
                        //        if (!found && PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryFind(fullName + '.') != null)
                        //        {
                        //            if (historyAndNamespaceIndex == null)
                        //            {
                        //                this.suggester.Images.Add(CompositeImages.HistoryAndNamespace);
                        //                historyAndNamespaceIndex = this.suggester.Images.Count - 1;
                        //            }

                        //            imageIndex = historyAndNamespaceIndex.Value;
                        //            removePrevious = true;
                        //            suggestionItemType |= SuggestionItemType.Namespace;
                        //        }
                        //    }

                        //    if (removePrevious)
                        //    {
                        //        string removeKey = this.suggestionItemsAndIndexes.TryFindWholeKey(itemToAdd, CaseSensitivity.Insensitive);
                        //        if (removeKey != null && removeKey.Length == itemToAdd.Length)
                        //        {
                        //            this.suggestionItemsAndIndexes.Remove(removeKey);
                        //            standardItems.Remove(removeKey);
                        //        }
                        //    }

                        //    if (!this.suggestionItemsAndIndexes.Contains(itemToAdd, CaseSensitivity.Insensitive))
                        //    {
                        //        standardItems.Add(itemToAdd, new SuggestionItem(suggestionItemType, itemToAdd, imageIndex));
                        //        this.suggestionItemsAndIndexes.Add(itemToAdd, 0);
                        //    }
                    //    //}

                        
                    //}
                //}

                //for (int i = 0; i < suggestionItems.Count; i++)
                //{
                //    SuggestionItem item = suggestionItems[i];
                //    this.suggester.AddItem(item);
                //    this.suggestionItemsAndIndexes[item.Text, CaseSensitivity.Insensitive] = i;
                //}

                //if (populateFirstLevel)
                //{
                //    this.firstLevelImages.Clear();
                //    this.firstLevelImages.AddRange(this.suggester.Images);

                //    this.firstLevelSuggestionItems = suggestionItems;
                //    this.firstLevelSuggestionItemsAndIndexes = this.suggestionItemsAndIndexes;
                //}
            //}
            //else if (this.promptHandler.suggestionMode == SuggestionMode.History)
            //{
            //    this.suggester.Images.Add(Images.History);
            //    this.suggestionItemsAndIndexes = new OptimizedStringKeyedDictionary<Int32Encapsulator>(SortMode.DecendingFromLastAdded);
            //    OptimizedStringKeyedDictionary<SuggestionItem> historyItems = new OptimizedStringKeyedDictionary<SuggestionItem>(SortMode.DecendingFromLastAdded);

            //    System.Collections.ObjectModel.ReadOnlyCollection<string> history = PromptuSettings.CurrentProfile.History.AllKeys;

            //    for (int i = 0; i < history.Count; i++)
            //    {
            //        string item = history[history.Count - 1 - i];
            //        if (PromptuSettings.CurrentProfile.Lists.AnyListsAreDisabled && !PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(item)
            //                    && PromptuSettings.CurrentProfile.AllCompositeFunctionsAndCommands.Contains(item))
            //        {
            //            continue;
            //        }

            //        if (PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Contains(item))
            //        {
            //            continue;
            //        }

            //        SuggestionItemType suggestionItemType = SuggestionItemType.History;
            //        //HistoryDetails details = PromptuSettings.CurrentProfile.History[item, CaseSensitivity.Sensitive];
            //        int imageIndex = 0;

            //        if (!this.suggestionItemsAndIndexes.Contains(item, CaseSensitivity.Insensitive))
            //        {
            //            historyItems.Add(item, new SuggestionItem(suggestionItemType, item, imageIndex));
            //            this.suggestionItemsAndIndexes.Add(item, 0);
            //        }
            //    }

            //    this.promptHandler.alternateSuggestionBase = new FindOptimizedStringCollection(SortMode.DecendingFromLastAdded);

            //    for (int i = 0; i < historyItems.Count; i++)
            //    {
            //        SuggestionItem item = historyItems[i];
            //        this.suggester.AddItem(item);
            //        this.suggestionItemsAndIndexes[item.Text, CaseSensitivity.Insensitive] = i;
            //        this.promptHandler.alternateSuggestionBase.Add(historyItems[historyItems.Count - 1 - i].Text);
            //    }
            //}

        //    if (resizePrompt)
        //    {
        //        Rectangle protectedArea = new Rectangle(this.promptHandler.prompt.LocationOnScreen, this.promptHandler.prompt.PromptSize);

        //        IInformationBox mainBox = this.informationBoxManager.MainInformationBox;
        //        if (mainBox != null && mainBox.Visible)
        //        {
        //            protectedArea = protectedArea.GetBoundingRectangleWith(new Rectangle(mainBox.Location, mainBox.Size));
        //        }

        //        this.suggester.SizeAndPositionYourself(
        //            protectedArea,
        //            new List<Rectangle>(),
        //            PromptuSettings.CurrentProfile.SuggesterSize);
        //    }

        //    //if (this.suggester.Visible)
        //    //{
        //    //    this.iconLoadingThread = new Thread(this.LoadIcons);
        //    //    this.iconLoadingThread.IsBackground = true;
        //    //    try
        //    //    {
        //    //        this.iconLoadingThread.Start();
        //    //    }
        //    //    catch (ThreadStartException)
        //    //    {
        //    //    }
        //    //}

        //    //int totalTime = Environment.TickCount - start;
        //    //this.StartProcessingIconLoadOrders();
        //    PromptuSettings.SyncSynchronizer.UnPauseSyncs();
        //    return true;
        //}

        //private void PopulateFromFileSystem(string populationFilter, PopulationInfo info, int indexOfLastDirectorySeparatorChar, int indexOfFirstDirectorySeparatorChar)
        //{
        //    ////int indexOfLastDirectorySeparatorChar = populationFilter.LastIndexOf(Path.DirectorySeparatorChar);
        //    ////int indexOfFirstDirectorySeparatorChar = populationFilter.IndexOf(Path.DirectorySeparatorChar);
        //    //if (indexOfLastDirectorySeparatorChar > -1)
        //    //{
        //    //    skipNormal = true;
        //    info.Suggester.Images.Add(Images.File);
        //    string beginningFolderName = populationFilter.Substring(0, indexOfFirstDirectorySeparatorChar);
        //    string firstPartOfUserTyped = populationFilter.Substring(0, indexOfLastDirectorySeparatorChar + 1);

        //    FileSystemDirectory? directory = null;
        //    bool found;
        //    GroupedCompositeItem compositeItem = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
        //    CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
        //    if (found && compositeItem != null && (parameterlessCommandNamedLikeFirstFolder = compositeItem.TryGetCommand(beginningFolderName, 0)) != null)
        //    {
        //        try
        //        {
        //            ExecutionData executionData = new ExecutionData(
        //                new string[0],
        //                parameterlessCommandNamedLikeFirstFolder.ListFrom,
        //                PromptuSettings.CurrentProfile.Lists);
        //            FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
        //            if (proposedDirectory.Exists)
        //            {
        //                directory = proposedDirectory + populationFilter.Substring(indexOfFirstDirectorySeparatorChar, indexOfLastDirectorySeparatorChar - indexOfFirstDirectorySeparatorChar + 1);
        //            }
        //        }
        //        catch (Itl.ParseException)
        //        {
        //        }
        //        catch (ConversionException)
        //        {
        //        }
        //    }

        //    if (directory == null)
        //    {
        //        directory = firstPartOfUserTyped;
        //    }

        //    bool addedFolderIcon = false;
        //    int indexOfFolderIcon = -1;

        //    List<SuggestionItem> directories = new List<SuggestionItem>();

        //    bool showHidden = PromptuSettings.CurrentProfile.NavigateHiddenFilesAndFolders;
        //    bool showSystem = PromptuSettings.CurrentProfile.NavigateSystemFilesAndFolders;

        //    if (directory.Value.Exists)
        //    {
        //        //int startLoadDir = System.Environment.TickCount;
        //        try
        //        {
        //            foreach (FileSystemDirectory subDirectory in directory.Value.GetDirectories())
        //            {
        //                //bool hidden = (subDirectory.Attributes & FileAttributes.Hidden) != 0;
        //                //bool system = (
        //                if ((!showHidden && subDirectory.IsHidden) || (!showSystem && subDirectory.IsSystem))
        //                {
        //                    continue;
        //                }

        //                string name = subDirectory.Name;
        //                if (!info.SuggestionItemsAndIndexes.Contains(name, CaseSensitivity.Insensitive))
        //                {
        //                    if (!addedFolderIcon)
        //                    {
        //                        if (FolderIcon == null)
        //                        {
        //                            Icon icon = Utilities.ExtractDirectoryIcon(subDirectory, IconSize.Small, true);
        //                            if (icon == null)
        //                            {
        //                                FolderIcon = null;
        //                            }
        //                            else
        //                            {
        //                                FolderIcon = icon.ToBitmap();
        //                                icon.Dispose();
        //                            }

        //                        }

        //                        if (indexOfFolderIcon == -1)
        //                        {
        //                            info.Suggester.Images.Add(FolderIcon);
        //                            indexOfFolderIcon = info.Suggester.Images.Count - 1;
        //                        }
        //                        else
        //                        {
        //                            info.Suggester.Images[indexOfFolderIcon] = FolderIcon;
        //                        }

        //                        addedFolderIcon = FolderIcon != null;
        //                    }

        //                    directories.Add(new SuggestionItem(SuggestionItemType.Folder, name, indexOfFolderIcon));
        //                    info.SuggestionItemsAndIndexes.Add(name, 0);
        //                    this.folderSuggestions.Add(name);
        //                }
        //            }


        //            //int totalTime = Environment.TickCount - startLoadDir;

        //            List<SuggestionItem> files = new List<SuggestionItem>();

        //            int nextImageIndex = info.Suggester.Images.Count;

        //            foreach (FileSystemFile file in directory.Value.GetFiles())
        //            {
        //                if ((!showHidden && file.IsHidden) || (!showSystem && file.IsSystem))
        //                {
        //                    continue;
        //                }

        //                int imageIndex = -1;
        //                string extension = file.Extension;
        //                bool foundAlreadyLoadedIndex;
        //                bool alwaysGetIcon = AlwaysGetIconFor.Contains(extension, CaseSensitivity.Insensitive);
        //                Int32Encapsulator alreadyLoadedIndex = FileExtensionsAndImageIndexes.TryGetItem(extension, CaseSensitivity.Insensitive, out foundAlreadyLoadedIndex);
        //                if (foundAlreadyLoadedIndex && alreadyLoadedIndex != null && !alwaysGetIcon)
        //                {
        //                    imageIndex = alreadyLoadedIndex;
        //                }
        //                else
        //                {
        //                    //bool foundCached = false;
        //                    //Bitmap bitmap = null;
        //                    //if (!alwaysGetIcon)
        //                    //{
        //                    //    Int32Encapsulator index = this.fileExtensionsAndImageIndexes.TryGetItem(extension, CaseSensitivity.Insensitive, out foundCached);
        //                    //    if (index == null)
        //                    //    {
        //                    //        foundCached = false;
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        imageIndex = index;
        //                    //    }
        //                    //}

        //                    //if (!foundCached)
        //                    //{
        //                    imageIndex = nextImageIndex;
        //                    nextImageIndex++;
        //                    IconLoadOrders.Enqueue(new IconLoadOrder(file, imageIndex));
        //                    if (!alwaysGetIcon)
        //                    {
        //                        FileExtensionsAndImageIndexes.Add(extension, imageIndex);
        //                    }
        //                    //}

        //                    //this.suggester.Images.Add(bitmap);

        //                    //if (!alwaysGetIcon)
        //                    //{
        //                    //    this.fileExtensionsAndImageIndexes.Add(extension, imageIndex);
        //                    //}
        //                }

        //                string name = file.Name;

        //                files.Add(new SuggestionItem(SuggestionItemType.File, name, imageIndex, 5));
        //                info.SuggestionItemsAndIndexes.Add(name, 0);
        //            }

        //            directories.Sort(SuggestionItemComparison);
        //            files.Sort(SuggestionItemComparison);
        //            suggestionItems.AddListToComposite(directories);
        //            suggestionItems.AddListToComposite(files);
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            IInformationBox informationBox = new Default.DefaultInformationBox();

        //            informationBox.InformationType = InformationType.Error;
        //            string message;
        //            if (Utilities.PromptuIsRunningElevated)
        //            {
        //                message = String.Format(
        //                    Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccessRunningElevated,
        //                    directory.Value);
        //            }
        //            else
        //            {
        //                message = String.Format(
        //                    Localization.MessageFormats.FileSystemSuggestionUnauthorizedAccess,
        //                    directory.Value);
        //            }

        //            informationBox.Size = new Size(informationBox.Size.Height, 500);

        //            //informationBox.RichTextBox.Text = message;
        //            //informationBox.
        //            informationBox.Content.Font = PromptuFonts.UnauthorizedAccessFont;
        //            informationBox.Content.ForeColor = Color.Tomato;

        //            informationBox.Content.Text = XmlFormatLabel.GenerateXmlTag(message);

        //            //informationBox.RichTextBox.SelectAll();
        //            //informationBox.RichTextBox.SelectionFont = PromptuFonts.UnauthorizedAccessFont;
        //            //informationBox.RichTextBox.SelectionColor = Color.Tomato;
        //            //informationBox.RichTextBox.SelectionLength = 0;

        //            //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(message, , Color.Tomato), 0, 0);

        //            informationBox.SizeAndPositionYourself(new Rectangle(this.suggestionHandler.Prompt.LocationOnScreen,
        //                this.suggestionHandler.Prompt.PromptSize), new List<Rectangle>(), Size.Empty);

        //            this.suggestionHandler.InformationBoxMananger.RegisterAndShow(informationBox, true);
        //            this.filterAtLastPopulation = null;
        //            return false;
        //        }
        //    }
        //    //}
        //}
    }
}
