using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using ZachJohnson.Promptu.Collections;
//using ZachJohnson.Promptu.DynamicEntryModel;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UI;
using System.Extensions;
using System.Threading;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.Itl;
using System.Drawing.Extensions;
using System.Runtime.InteropServices;
using ZachJohnson.Promptu.UIModel;
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.SkinApi;
using ZachJohnson.Promptu.UIModel.RichText;
using ZachJohnson.Promptu.PluginModel.Internals;
using ZachJohnson.Promptu.PluginModel.Hooks;
using System.Globalization;

namespace ZachJohnson.Promptu.Skins
{
    internal partial class PromptHandler
    {
        internal class SeparateSuggestionHandler : SuggestionHandler
        {
            //private static int CancelIconLoadingMaxJoinTime = 1000;
            
            private static Comparison<SuggestionItem> SuggestionItemComparison = new Comparison<SuggestionItem>(CompareSuggestionItems);
            
            //private ISuggester suggester;
            private PromptHandler promptHandler;
            private bool ignoreActivationLost = false;
            private string filter = String.Empty;
            //private OptimizedStringKeyedDictionary<Bitmap> fileExtensionsAndAssociatedBitmaps = new OptimizedStringKeyedDictionary<Bitmap>(SortMode.Alphabetical);
            
            //private FindOptimizedStringCollection folderSuggestions = new FindOptimizedStringCollection(SortMode.DecendingFromLastAdded);
            //private Bitmap folderIcon = null;
            
            //private bool stopIconLoading = false;
            
            private int beginningOfSuggestionArea;
            //private List<Image> firstLevelImages = new List<Image>();
            //private CompositeList<SuggestionItem> firstLevelSuggestionItems;
            //private OptimizedStringKeyedDictionary<Int32Encapsulator> firstLevelSuggestionItemsAndIndexes;
            private int? lastSuggestedIndex = null;
            
            //private IInformationBox showingStandardInfoBox;
            private System.Timers.Timer showInfoBoxTimer = new System.Timers.Timer(1000);
            private System.Timers.Timer closeInfoBoxTimer = new System.Timers.Timer(10000);
            //private WindowManager windowManager = new WindowManager();
            private InformationBoxManager informationBoxManager;
            private ParameterHelpContext mainInfoContextInfo;
            private FunctionSuggestionProvider functionSuggestionProvider = new FunctionSuggestionProvider();
            private PopulationProvider populationProvider;
            private SuggestionProvider suggestionProvider;
            //private bool addQuoteWithSpace;
            private InvisibleEndQuote invisibleEndQuote;
            private TrieDictionary<Int32Encapsulator> lastIndexCache = new TrieDictionary<Int32Encapsulator>(SortMode.Default);
            private bool autocorrectedLastKeyPress;
            private bool forceNextExecute;
            private CommandQueue commandQueue;
            private UIMenuItem deleteItem;
            private UIMenuItem editItem;
            private UIMenuItemInternal contextNewCommand;
            //private int previousQuoteIndex = -1;
            //private IntPtr standardInfoBoxHandle;
            //private Find

            // private bool ignoreSuggesterWM_Activate = false;

            public SeparateSuggestionHandler(PromptHandler promptHandler)
            {
                if (promptHandler == null)
                {
                    throw new ArgumentNullException("promptHandler");
                }

                this.commandQueue = new CommandQueue();

                this.populationProvider = new PopulationProvider(this);
                this.suggestionProvider = new SuggestionProvider(this);

                this.informationBoxManager = new InformationBoxManager(InternalGlobals.GuiManager.ToolkitHost.WindowManager, new ParameterlessVoid(this.ActivateCurrentPrompt));
                this.informationBoxManager.ParameterHelpImageClick += this.HandleMainInfomationBoxContentMouseDown;

                this.editItem = new UIMenuItem("Promptu.EditSelectedItem", Localization.UIResources.SuggesterEditItemText);
                this.editItem.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Edit;
                this.editItem.Click += this.HandleEditClick;
                InternalGlobals.SuggestionItemContextMenu.Items.Add(this.editItem);

                this.deleteItem = new UIMenuItem("Promptu.DeleteSelectedItem", Localization.UIResources.SuggesterDeleteItemText);
                this.deleteItem.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Delete;
                this.deleteItem.Click += this.HandleDeleteClick;
                InternalGlobals.SuggestionItemContextMenu.Items.Add(this.deleteItem);

                this.contextNewCommand = new UIMenuItemInternal(
                    "Promptu.NewCommand");

                this.contextNewCommand.Image = InternalGlobals.GuiManager.ToolkitHost.Images.NewCommand;
                this.contextNewCommand.Click += this.HandleCreateNewCommand;

                InternalGlobals.PromptContextMenu.Items.Add(this.contextNewCommand);
                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuSeparatorInternal("Promptu.NewCommandSeparator"));

                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuItemInternal(
                "Promptu.Setup",
                Localization.UIResources.IconSetupText,
                new EventHandler(this.ShowSetup)));

                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuSeparatorInternal("Promptu.SetupSeparator"));
                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuItemInternal(
                    "Promptu.Help",
                    Localization.UIResources.IconHelpText,
                    new EventHandler(this.ShowHelp)));

                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuItemInternal(
                    "Promptu.About",
                    Localization.UIResources.IconAboutText,
                    new EventHandler(this.ShowAbout)));

                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuSeparatorInternal("Promptu.HelpSeparator"));
                InternalGlobals.PromptContextMenu.Items.Add(new UIMenuItemInternal(
                    "Promptu.Quit",
                    Localization.UIResources.IconQuitText,
                    new EventHandler(this.QuitClick)));

                //UIMenuItem test = new UIMenuItem("Promptu.DeleteSelectedItem", Localization.UIResources.SuggesterDeleteItemText);
                //test.Image = InternalGlobals.GuiManager.ToolkitHost.Images.Delete;
                ////this.deleteItem.Click += this.HandleDeleteClick;
                //InternalGlobals.PromptContextMenu.Items.Add(test);
                
                //else if (suggester == null)
                //{
                //    throw new ArgumentNullException("suggester");
                //}

                //this.Suggester = suggester;
                //this.Suggester.Deactivate += this.SuggesterDeactivate;
                //this.Suggester.KeyPressedByUser += this.SuggesterKeyPressedByUser;
                //this.Suggester.WM_ActivateRecieved += this.SuggesterWM_ActivateRecieved;

                //this.Suggester.EndOfItemsScroll += this.MoveFocusBackToPrompt;
                //this.Suggester.NonClientAreaMouseDown += this.MoveFocusBackToPrompt;
                //this.Suggester.MouseDown += this.MouseDown;

                this.promptHandler = promptHandler;
                this.promptHandler.ListsChanged += this.HandleListsChanged;
                //Application.Idle += this.ApplicationIsIdle;

                //this.iconLoadingThread = new Thread(this.LoadIcons);
                //this.iconLoadingThread.IsBackground = true;
                //try
                //{
                //    this.iconLoadingThread.Start();
                //}
                //catch (ThreadStartException)
                //{
                //}

                this.showInfoBoxTimer.AutoReset = false;
                this.showInfoBoxTimer.Elapsed += this.ShowInfoForSelectedItemThreadSafe;
                this.closeInfoBoxTimer.AutoReset = false;
                this.closeInfoBoxTimer.Elapsed += this.HideStandardInfoBoxThreadSafe;

                InternalGlobals.GuiManager.ToolkitHost.WindowManager.ActivationLost += this.HandleActivationLost;
            }

            public SuggestionProvider SuggestionProvider
            {
                get { return this.suggestionProvider; }
            }

            public ISuggestionProvider Suggester
            {
                get { return InternalGlobals.CurrentSkinInstance.SuggestionProvider; }
            }

            public PromptHandler PromptHandler
            {
                get { return this.promptHandler; }
            }

            public SuggestionMode SuggestionMode
            {
                get { return this.promptHandler.suggestionMode; }
            }

            public InformationBoxManager InformationBoxMananger
            {
                get { return this.informationBoxManager; }
            }

            public IPrompt Prompt
            {
                get { return InternalGlobals.CurrentSkinInstance.Prompt; }
            }

            private void ActivateCurrentPrompt()
            {
                InternalGlobals.CurrentSkinInstance.Prompt.Activate();
            }

            private void HideStandardInfoBoxThreadSafe(object sender, EventArgs e)
            {
                this.promptHandler.InvokeOnMainThread(new ParameterlessVoid(this.HideItemInfoBox));
            }

            private void HideItemInfoBox()
            {
                this.closeInfoBoxTimer.Stop();
                IInfoBox infoBox = this.informationBoxManager.ItemInfoBox;
                if (infoBox != null)
                {
                    infoBox.Hide();
                    this.informationBoxManager.Unregister(infoBox);
                }
            }

            private void StopAndHideItemInfoBox()
            {
                this.HideItemInfoBox();
                this.showInfoBoxTimer.Stop();
            }

            private void HandleMainInfomationBoxContentMouseDown(object sender, ImageClickEventArgs e)
            {
                //ITextInfoBox box = this.informationBoxManager.ParameterHelpBox as ITextInfoBox;
                //if (box != null)
                //{
                //    // this.OffsetMainInformationBoxIndex(ParameterHelpContext.IsDownArrow(box.Content.HitTest(e.Location)) ? -1 : 1);
                //}

                this.OffsetMainInformationBoxIndex(ParameterHelpContext.IsDownArrow(e.Key) ? -1 : 1);
            }

            private void OffsetMainInformationBoxIndex(int offset)
            {
                if (this.mainInfoContextInfo != null)
                {
                    this.mainInfoContextInfo.OffsetCurrentIndex(offset);
                    this.ShowParameterHelpDependingOnFilter(true);
                }
            }

            private void HandleItemContextChanged(object sender, EventArgs e)
            {
                this.populationProvider.ResetFilterAtLastPopulation();
            }

            private void ShowParameterDependingOnFilter()
            {
                this.ShowParameterHelpDependingOnFilter(false);
            }

            private ParameterHelpContext GetContextInfoFor(string text)
            {
                string itemName = SuggestionUtilities.GetItemNameFrom(text);

                if (itemName != null)
                {
                    bool found;
                    GroupedCompositeItem compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(itemName, out found);
                    if (compositeItem != null && found)
                    {
                        ParameterHelpContext contextInfo = new ParameterHelpContext(
                            itemName,
                            Function.IsInFunctionSyntax(text), compositeItem.GetUniqueItems(),
                            0,
                            this.lastIndexCache);
                        contextInfo.UpdateCurrentIndexToBestFit();
                        return contextInfo;
                    }
                }

                return null;
            }

            private void HandleListsChanged(object sender, EventArgs e)
            {
                this.UpdateNewCommandItems();
            }

            public void UpdateNewCommandItems()
            {
                if (InternalGlobals.CurrentProfile == null || InternalGlobals.CurrentProfile.Lists.Count <= 0)
                {
                    this.contextNewCommand.Available = false;
                }
                else
                {
                    this.contextNewCommand.Available = true;
                    this.contextNewCommand.SubItems.Clear();
                    this.contextNewCommand.Tag = null;

                    if (InternalGlobals.CurrentProfile.Lists.Count == 1)
                    {
                        this.contextNewCommand.Text = Localization.UIResources.IconCreateNewCommandText;
                        this.contextNewCommand.Tag = InternalGlobals.CurrentProfile.Lists[0].FolderName;
                    }
                    else
                    {
                        this.contextNewCommand.Text = Localization.UIResources.IconCreateNewCommandInText;

                        foreach (UserModel.List list in InternalGlobals.CurrentProfile.Lists)
                        {
                            UIMenuItem subMenuItem = new UIMenuItemInternal(
                                String.Format(CultureInfo.InvariantCulture, "Promptu.List={0}", list.FolderName),
                                list.Name);

                            subMenuItem.Tag = list.FolderName;
                            subMenuItem.Click += this.HandleCreateNewCommand;
                            this.contextNewCommand.SubItems.Add(subMenuItem);
                        }
                    }
                }
            }

            private void HandleCreateNewCommand(object sender, EventArgs e)
            {
                UIMenuItem menuItem = sender as UIMenuItem;

                if (menuItem != null)
                {
                    string folderName = menuItem.Tag as string;

                    if (folderName != null)
                    {
                        if (!PromptHandler.IsInitializing)
                        {
                            PromptHandler promptHandler = PromptHandler.GetInstance();
                            promptHandler.ShowSetupDialog();
                            promptHandler.SetupDialog.CreateNewCommand(folderName);
                        }
                    }
                }
            }

            private void ShowParameterHelpDependingOnFilter(bool forceRefresh)
            {
                string itemName = SuggestionUtilities.GetItemNameFrom(this.filter);

                if (itemName == null)
                {
                    this.HideAndDestroyMainInfoBox();
                    return;
                }

                bool showingNew = false;

                if (this.mainInfoContextInfo == null || this.mainInfoContextInfo.ItemName.ToUpperInvariant() != itemName.ToUpperInvariant())
                {
                    bool found;
                    GroupedCompositeItem compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(itemName, out found);
                    if (compositeItem != null && found)
                    {
                        showingNew = true;
                        bool mustBeFunction = Function.IsInFunctionSyntax(this.PromptSuggestionAreaText);
                         
                        GroupedCompositeItem uniqueItems = compositeItem.GetUniqueItems();

                        if (!mustBeFunction || uniqueItems.StringFunctions.Count > 0)
                        {
                            this.mainInfoContextInfo = new ParameterHelpContext(
                                itemName,
                                mustBeFunction,
                                uniqueItems,
                                0,
                                this.lastIndexCache);
                            this.mainInfoContextInfo.CurrentIndexChanged += this.HandleItemContextChanged;
                            this.mainInfoContextInfo.UpdateCurrentIndexToBestFit();
                        }
                        else
                        {
                            this.HideAndDestroyMainInfoBox();
                            return;
                        }
                    }
                    else
                    {
                        this.HideAndDestroyMainInfoBox();
                        return;
                    }
                }

                int currentParamIndex = SuggestionUtilities.GetCurrentParameterIndex(this.SuggestionAreaAndBeyond, this.Prompt.SelectionStart - this.beginningOfSuggestionArea);

                if (forceRefresh || showingNew || currentParamIndex != this.mainInfoContextInfo.ParameterIndex)
                {
                    bool cycleToBestMatch = currentParamIndex > this.mainInfoContextInfo.ParameterIndex || (showingNew && currentParamIndex == 0);
                    this.mainInfoContextInfo.ParameterIndex = currentParamIndex;

                    if (cycleToBestMatch)
                    {
                        this.mainInfoContextInfo.CycleToBestMatch();
                    }

                    ITextInfoBox box = this.informationBoxManager.ParameterHelpBox as ITextInfoBox;

                    bool register = false;
                    if (box == null)
                    {
                        register = true;
                        box = InternalGlobals.CurrentSkinInstance.CreateTextInfoBox();//new Default.DefaultInformationBox();
                        //REVISIT Globals.CurrentSkin.ToolTipSettings.ApplyTo(box);
                    }

                    this.mainInfoContextInfo.Populate(box);

                    box.Refresh();
                    //Size preferredSize = box.GetPreferredSize(Size.Empty);

                    //int maximumWidth = PromptuUtilities.GetWidthOfThreeFifthsOfScreen(this.promptHandler.prompt.LocationOnScreen);

                    //if (preferredSize.Width > maximumWidth)
                    //{
                    //    preferredSize = box.GetPreferredSize(new Size(maximumWidth, 0));

                    //    if (preferredSize.Width > maximumWidth)
                    //    {
                    //        preferredSize.Width = maximumWidth;
                    //    }
                    //}

                    //box.Size = preferredSize;

                    bool suggesterWasVisible = this.Suggester.Visible;
                    if (suggesterWasVisible)
                    {
                        this.HideSuggester("going to reopen, suggester was visible and showing main info box");
                    }

                    if (register)
                    {
                        box.Location = new Point(-10000, -10000);
                        this.informationBoxManager.RegisterAndShow(box, false, InformationBoxType.ParameterHelp, this.Suggester);
                    }

                    Size preferredSize;
                    InternalGlobals.CurrentSkinInstance.LayoutManager.PositionParameterHelpBox(
                        this.CreatePositioningContext(),
                        box,
                        out preferredSize);

                    //box.Location = InformationBoxLayoutManager.Position(new Rectangle(this.promptHandler.prompt.LocationOnScreen, this.promptHandler.prompt.PromptSize), preferredSize);

                    if (register)
                    {
                        //box.BringToFront();
                        box.Size = preferredSize;
                        this.ActivateCurrentPrompt();
                        //NativeMethods.SetOwner(box, this.Suggester);
                    }

                    if (suggesterWasVisible)
                    {
                        this.ShowSuggester();
                    }

                    box.Refresh();
                }
            }

            //private static List<string> GetDisplayValues(SuggestionItemType type)
            //{
            //    switch (type)
            //    {
            //        case SuggestionItemType.NativePromptuCommand:
            //            return "Promptu-command";
            //        case SuggestionItemType.None:
            //            return "item";
            //        default:
            //            return type.ToString().ToLower();
            //    }
            //}

            private void ShowHelp(object sender, EventArgs e)
            {
                if (PromptHandler.IsInitializing)
                {
                    return;
                }

                PromptHandler.GetInstance().TakeUserToHelp();
            }

            private void ShowAbout(object sender, EventArgs e)
            {
                if (PromptHandler.IsInitializing)
                {
                    return;
                }

                PromptHandler.GetInstance().SetupDialog.ShowUserToAbout();
            }

            private void ShowSetup(object sender, EventArgs e)
            {
                if (PromptHandler.IsInitializing)
                {
                    return;
                }

                PromptHandler.GetInstance().ShowSetupDialog();
            }

            private void QuitClick(object sender, EventArgs e)
            {
                PromptuUtilities.ExitApplication();
            }

            private void ShowInfoForSelectedItemThreadSafe(object sender, EventArgs e)
            {
                if (Control.MouseButtons != 0)
                {
                    this.promptHandler.InvokeOnMainThread(new ParameterlessVoid(this.ResetShowInfoBoxTimer));
                }
                else
                {
                    this.promptHandler.InvokeOnMainThread(new ParameterlessVoid(this.ShowInfoForSelectedItem));
                }
            }

            // I18N
            private static string GetOverloadText(int count)
            {
                count--;
                if (count == 1)
                {
                    return " (+ 1 variant)";
                }
                else if (count > 1)
                {
                    return String.Format(CultureInfo.CurrentCulture, " (+ {0} variants)", count);
                }
                else
                {
                    return String.Empty;
                }
            }

            private static string ConstructEntry(string basic, string documentation, int countLeft)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(basic);
                if (!String.IsNullOrEmpty(documentation))
                {
                    builder.Append(System.Environment.NewLine);
                    builder.Append(documentation);
                }

                if (countLeft > 0)
                {
                    builder.Append(System.Environment.NewLine);
                }

                return builder.ToString();
            }

            private void HandleSuggesterActivate(ISuggestionProvider suggester)
            {
                this.informationBoxManager.HideAndDestroyAllExceptParameterHelp();
            }

            //I18N
            private void ShowInfoForSelectedItem()
            {
                int selectedIndex = this.Suggester.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    SuggestionItem suggestedItem = this.Suggester.GetItem(selectedIndex);
                    if (this.Suggester.Visible && suggestedItem != null && !this.Suggester.SuppressItemInfoToolTips)
                    {
                        this.showInfoBoxTimer.Stop();
                        this.closeInfoBoxTimer.Stop();
                        //Rectangle rect = this.Suggester.GetItemBounds(selectedIndex);

                        ITextInfoBox informationBox = InternalGlobals.CurrentSkinInstance.CreateTextInfoBox();

                        informationBox.InfoType = InfoType.Default;
                        string fullName = suggestedItem.Text;

                        bool constructDefaultFullName = true;

                        if (this.filter.Length > 0 && !SuggestionUtilities.IsFilepath(this.filter))
                        {
                            int parameterIndex = SuggestionUtilities.GetCurrentParameterIndex(this.filter, this.filter.Length);
                            if (parameterIndex >= 0)
                            {
                                constructDefaultFullName = false;
                                bool inQuote = filter.CountOf('"', true) % 2 != 0;

                                string[] nameAndParameters = SuggestionUtilities.ExtractNameAndParametersFrom(this.filter);
                                string actualFilter;
                                if (parameterIndex + 1 < nameAndParameters.Length)
                                {
                                    string fullFilter = nameAndParameters[parameterIndex + 1];
                                    if (inQuote)
                                    {
                                        fullFilter = fullFilter.Substring(1).Unescape();
                                    }

                                    actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, this.mainInfoContextInfo);

                                    //if (inQuote)
                                    //{
                                    //    actualFilter = actualFilter.Unescape();
                                    //}
                                }
                                else
                                {
                                    actualFilter = String.Empty;
                                }

                                fullName = actualFilter + suggestedItem.Text;
                            }
                        }

                        if (constructDefaultFullName)
                        {
                            fullName = this.filter + suggestedItem.Text;
                        }

                        bool found;
                        GroupedCompositeItem groupForFullName = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(fullName, out found);

                        RTGroup content = new RTGroup();
                        string[] splitType = suggestedItem.Type.ToString().Split(',');

                        for (int i = 0; i < splitType.Length; i++)
                        {
                            if (i > 0)
                            {
                                content.Children.Add(new LineBreak());
                            }

                            int countLeft = splitType.Length - 1 - i;
                            //string newLine = count > 0 ? System.Environment.NewLine + System.Environment.NewLine : String.Empty;
                            string text;
                            string documentation = null;
                            string trimmed = splitType[i].Trim();
                            switch (trimmed)
                            {
                                case "Folder":
                                case "File":
                                case "Namespace":
                                case "History":
                                    content.Children.Add(Text.Generate(
                                        ConstructEntry(String.Format(CultureInfo.CurrentCulture, "{0} {1}", trimmed.ToLowerInvariant(), fullName), null, countLeft), false));
                                    //    informationBox.RichTextBox.SelectAll();
                                    //    informationBox.RichTextBox.SelectionLength = 0;
                                    ////informationBox.GraphicalEntries.Add(new GraphicalTextEntry(
                                    //    ,
                                    //    ,
                                    //    PromptuColors.InfoColor), 0, i);

                                    break;
                                case "NativePromptuCommand":
                                    //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(
                                    content.Children.Add(Text.Generate(
                                    ConstructEntry(String.Format(CultureInfo.CurrentCulture, "promptu-command {0}", fullName), Command.GetPromptuCommandDocumentation(fullName), countLeft), false));
                                    //PromptuFonts.InfoFont,
                                    //PromptuColors.InfoColor), 0, i);

                                    break;
                                case "Function":
                                    text = String.Format(CultureInfo.CurrentCulture, "function {0}", fullName);

                                    if (groupForFullName != null)
                                    {
                                        CompositeItem<Function, List> function = groupForFullName.StringFunctions[0];
                                        AssemblyReferenceCollectionComposite prioritizedAssemblyReferences = new AssemblyReferenceCollectionComposite(InternalGlobals.CurrentProfile.Lists, function.ListFrom);

                                        text = String.Format(
                                            CultureInfo.CurrentCulture, 
                                            "{0}{1}{2}",
                                            text,
                                            function.Item.GetNamedParametersIfPossible(prioritizedAssemblyReferences, true),
                                            GetOverloadText(groupForFullName.GetUniqueStringFunctions().Count));
                                        documentation = function.Item.TryGetDocumentation(prioritizedAssemblyReferences, true);
                                    }



                                    //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(
                                    content.Children.Add(Text.Generate(
                                    ConstructEntry(text, documentation, countLeft), false));
                                    //PromptuFonts.InfoFont,
                                    //PromptuColors.InfoColor), 0, i);

                                    break;
                                case "Command":
                                    text = String.Format(CultureInfo.CurrentCulture, "command {0}", fullName);

                                    if (groupForFullName != null)
                                    {
                                        text += GetOverloadText(groupForFullName.GetUniqueCommands().Count);
                                        documentation = groupForFullName.Commands[0].Item.Notes;
                                    }

                                    //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(
                                    content.Children.Add(Text.Generate(
                                        ConstructEntry(text, documentation, countLeft), false));
                                    //PromptuFonts.InfoFont,
                                    //PromptuColors.InfoColor), 0, i);

                                    break;
                                case "ValueListItem":
                                    content.Children.Add(Text.Generate(
                                        ConstructEntry(String.Format(CultureInfo.CurrentCulture, "value {0}", fullName), null, countLeft), false));
                                    break;
                                default:
                                    //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(
                                    content.Children.Add(Text.Generate(
                                        ConstructEntry(String.Format(CultureInfo.CurrentCulture, "item {0}", fullName), null, countLeft), false));
                                    //PromptuFonts.InfoFont,
                                    //PromptuColors.InfoColor), 0, i);

                                    break;
                            }
                        }

                        informationBox.Content = content;

                        //informationBox.GraphicalEntries.Add(new GraphicalTextEntry("function", PromptuFonts.InfoFont, PromptuColors.InfoColor), 0, 1);
                        //informationBox.GraphicalEntries.Add(new GraphicalTextEntry("myFunction", new Font(PromptuFonts.InfoFont, FontStyle.Bold), PromptuColors.InfoColor), 1, 1);

                        //informationBox.Content.Controls.Add(generator.GenerateLabel("Hello There", true));
                        //string text = String.Format("{0} {1}", GetDisplayValue(suggestedItem.Type), fullName);

                        //informationBox.GraphicalEntries.Add(new GraphicalTextEntry(text, PromptuFonts.InfoFont, PromptuColors.InfoColor), 0, 0);
                        //informationBox.Content.ResumeLayout(false);
                        //informationBox.Content.PerformLayout();

                        //Size preferredSize = informationBox.GetPreferredSize(Size.Empty);

                        //int maximumWidth = PromptuUtilities.GetWidthOfThreeFifthsOfScreen(this.promptHandler.prompt.LocationOnScreen) - 5;

                        //Rectangle suggesterFootprint = new Rectangle(this.Suggester.Location, this.Suggester.Size);

                        //Rectangle screen = Screen.GetWorkingArea(suggesterFootprint);

                        //int rightSpace = screen.Right - suggesterFootprint.Right - 5;
                        //int leftSpace = suggesterFootprint.Left - screen.Left - 5;

                        //if (maximumWidth > rightSpace)
                        //{
                        //    if (leftSpace > rightSpace)
                        //    {
                        //        if (maximumWidth > leftSpace)
                        //        {
                        //            maximumWidth = leftSpace;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        maximumWidth = rightSpace;
                        //    }
                        //}

                        //if (preferredSize.Width > maximumWidth)
                        //{
                        //    preferredSize = informationBox.GetPreferredSize(new Size(maximumWidth, 0));

                        //    if (preferredSize.Width > maximumWidth)
                        //    {
                        //        preferredSize.Width = maximumWidth;
                        //    }
                        //}

                        //informationBox.Size = preferredSize;
                        ////informationBox.Size = new Size(5, 5);

                        //informationBox.Location = InformationBoxLayoutManager.PositionRightOrLeft(
                        //    suggesterFootprint,
                        //    this.Suggester.Location.Y + rect.Y + ((rect.Height - informationBox.Size.Height) / 2), 
                        //    preferredSize);
                            
                            //new Point(
                            //this.Suggester.Location.X + this.Suggester.Size.Width,
                            //this.Suggester.Location.Y + rect.Y + ((rect.Height - informationBox.Size.Height) / 2));
                        Size preferredSize;

                        informationBox.Location = new Point(-10000, -10000);

                        this.informationBoxManager.RegisterAndShow(informationBox, true, InformationBoxType.ItemInfo, this.Suggester);

                        InternalGlobals.CurrentSkinInstance.LayoutManager.PositionItemInfoBox(
                            this.CreatePositioningContext(),
                            informationBox,
                            out preferredSize);

                        //if (this.Suggester.Visible)
                       // {
                            //this.showingInformationBoxes.Add(informationBox);
                            //informationBox.Show();
                            //this.showingStandardInfoBox = informationBox;
                            //this.informationBoxManager.RegisterAndShow(informationBox, true, InformationBoxType.ItemInfo, this.Suggester);
                            informationBox.Size = preferredSize;
                            
                            this.closeInfoBoxTimer.Start();
//}

                    }
                }
            }

            protected override void HandleSuggestionAffectingChangeCore()
            {
                ////this.filterAtLastPopulation = null;
                ParameterlessVoid populateFirstLevelSuggestions = new ParameterlessVoid(this.PopulateFirstLevelSuggestions);
                if (!PromptHandler.IsInitializing && InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.InvokeRequired)
                {
                    this.promptHandler.InvokeOnMainThread(populateFirstLevelSuggestions);
                }
                else
                {
                    populateFirstLevelSuggestions();
                }
            }

            private void PopulateFirstLevelSuggestions()
            {
                this.populationProvider.PopulateFirstLevelSuggestions(this.Suggester);
                this.populationProvider.ResetFilterAtLastPopulation();
            }

            private void HandleEditClick(object sender, EventArgs e)
            {
                this.EditSelectedItem();
            }

            private void EditSelectedItem()
            {
                SuggestionItem item = this.SelectedSuggestionItem;
                if (item != null)
                {
                    this.Edit(item);
                }
            }

            private void HandleDeleteClick(object sender, EventArgs e)
            {
                this.DeleteSelectedItem();
            }

            private void DeleteSelectedItem()
            {
                SuggestionItem item = this.SelectedSuggestionItem;
                if (item != null)
                {
                    this.Delete(item);
                }
            }

            private void Edit(SuggestionItem item)
            {
                InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                string name = this.filter + item.Text;

                if ((item.Type & SuggestionItemType.Command) != 0 || (item.Type & SuggestionItemType.Function) != 0)
                {
                    bool found;
                    CompositeItem<Command, List> command;
                    GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);

                    object editingObject = null;

                    if (found && compositeItem != null)
                    {
                        if (compositeItem.StringFunctions.Count == 0 && (command = compositeItem.TryGetCommand(name)) != null)
                        {
                            editingObject = command;
                        }
                        else if (compositeItem.StringFunctions.Count == 1 && compositeItem.Commands.Count <= 0)
                        {
                            editingObject = compositeItem.StringFunctions[0];
                        }
                        else if (compositeItem.StringFunctions.Count > 0 || compositeItem.Commands.Count > 0)
                        {
                            List<object> ambiguousObjects = new List<object>();

                            foreach (CompositeItem<Command, List> ambiguous in compositeItem.Commands)
                            {
                                ambiguousObjects.Add(ambiguous);
                            }

                            foreach (CompositeItem<Function, List> ambiguous in compositeItem.StringFunctions)
                            {
                                ambiguousObjects.Add(ambiguous);
                            }

                            ObjectDisambiguatorPresenter disambiguator = new ObjectDisambiguatorPresenter(
                                Localization.UIResources.EditObjectDisambiguatorMainInstructions,
                                ambiguousObjects);

                            if (disambiguator.ShowDialog() == UIDialogResult.OK)
                            {
                                editingObject = disambiguator.SelectedObject;
                            }
                        }
                    }

                    if (editingObject == null)
                    {
                        return;
                    }

                    CompositeItem<Command, List> editingCommand;
                    CompositeItem<Function, List> editingFunction;

                    if ((editingCommand = editingObject as CompositeItem<Command, List>) != null)
                    {
                        this.PromptHandler.ShowSetupDialog();
                        this.PromptHandler.SetupDialog.EditCommand(editingCommand.Item.Name, editingCommand.ListFrom);
                        //this.ClosePrompt();
                        //this.HideSuggester("none");
                    }
                    else if ((editingFunction = editingObject as CompositeItem<Function, List>) != null)
                    {
                        this.PromptHandler.ShowSetupDialog();
                        this.PromptHandler.SetupDialog.EditFunction(name, editingFunction.Item.ParameterSignature, editingFunction.ListFrom);
                        //this.ClosePrompt();
                        //this.HideSuggester("none");
                    }
                }

                //if ((item.Type & SuggestionItemType.Command) != 0)
                //{
                //    bool found;
                //    CompositeItem<Command, List> command;
                //    GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
                //    if (found && compositeItem != null && (command = compositeItem.TryGetCommand(name)) != null)
                //    {
                //        //this.promptHandler.SetupDialog..SetSelectedList(command.ListFrom);
                //        this.promptHandler.ShowSetupDialog();
                //        this.promptHandler.setupDialog.EditCommand(name);
                //    }
                //}
                //else if ((item.Type & SuggestionItemType.Function) != 0)
                //{
                //    bool found;
                //    GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
                //    if (found && compositeItem != null && compositeItem.StringFunctions.Count > 0)
                //    {
                //        string parameterSignature;
                //        if (compositeItem.StringFunctions.Count == 1)
                //        {
                //            parameterSignature = compositeItem.StringFunctions[0].Item.ParameterSignature;
                //        }
                //        else
                //        {
                //            List<Function> conflictingFunctions = new List<Function>();
                //            foreach (CompositeItem<Function, List> functionItem in compositeItem.StringFunctions)
                //            {
                //                conflictingFunctions.Add(functionItem.Item);
                //            }

                //            ObjectDisambiguatorPresenter disambiguator = new ObjectDisambiguatorPresenter(
                //                name,
                //                Localization.UIResources.FunctionDisambiguatorInformationTextEdit,
                //                conflictingFunctions);
                //            if (disambiguator.ShowDialog() != UIDialogResult.OK || disambiguator.SelectedFunction == null)
                //            {
                //                return;
                //            }

                //            parameterSignature = disambiguator.SelectedFunction.ParameterSignature;
                //        }

                //        CompositeItem<Function, List> function = compositeItem.TryGetStringFunction(name, parameterSignature);
                //        if (function != null)
                //        {
                //            //this.promptHandler.setupDialog.ListSelector.SetSelectedList(function.ListFrom);
                //            this.promptHandler.ShowSetupDialog();
                //            this.promptHandler.setupDialog.EditFunction(name, parameterSignature);
                //        }
                //    }
                //}
            }

            private void Delete(SuggestionItem item)
            {
                InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                string name = this.filter + item.Text;

                if ((item.Type & SuggestionItemType.Command) != 0 || (item.Type & SuggestionItemType.Function) != 0)
                {
                    bool found;
                    CompositeItem<Command, List> command;
                    GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);

                    bool userHasAlreadyBeenPrompted = false;

                    object editingObject = null;

                    if (found && compositeItem != null)
                    {
                        if (compositeItem.StringFunctions.Count == 0 && (command = compositeItem.TryGetCommand(name)) != null)
                        {
                            editingObject = command;
                        }
                        else if (compositeItem.StringFunctions.Count == 1 && compositeItem.Commands.Count <= 0)
                        {
                            editingObject = compositeItem.StringFunctions[0];
                        }
                        else if (compositeItem.StringFunctions.Count > 0 || compositeItem.Commands.Count > 0)
                        {
                            List<object> ambiguousObjects = new List<object>();

                            foreach (CompositeItem<Command, List> ambiguous in compositeItem.Commands)
                            {
                                ambiguousObjects.Add(ambiguous);
                            }

                            foreach (CompositeItem<Function, List> ambiguous in compositeItem.StringFunctions)
                            {
                                ambiguousObjects.Add(ambiguous);
                            }

                            ObjectDisambiguatorPresenter disambiguator = new ObjectDisambiguatorPresenter(
                                Localization.UIResources.DeleteObjectDisambiguatorMainInstructions,
                                ambiguousObjects);

                            //try
                            //{
                            //    this.ignoreActivationLost = true;
                                if (disambiguator.ShowDialog() == UIDialogResult.OK)
                                {
                                    editingObject = disambiguator.SelectedObject;
                                }
                            //}
                            //finally
                            //{
                            //    this.ignoreActivationLost = false;
                            //}

                            userHasAlreadyBeenPrompted = true;
                        }
                    }

                    if (editingObject == null)
                    {
                        return;
                    }

                    CompositeItem<Command, List> editingCommand;
                    CompositeItem<Function, List> editingFunction;

                    if ((editingCommand = editingObject as CompositeItem<Command, List>) != null)
                    {
                        try
                        {
                            this.ignoreActivationLost = true;
                            if (userHasAlreadyBeenPrompted || UIMessageBox.Show(
                                String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteCommand, editingCommand.Item.Name),
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.YesNo,
                                UIMessageBoxIcon.Warning,
                                UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
                            {
                                this.showInfoBoxTimer.Stop();
                                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.IgnoreNewRegenerations = true;
                                //int selectedIndex = this.Suggester.SelectedIndex;
                                editingCommand.ListFrom.Commands.Remove(editingCommand.Item);
                                editingCommand.Item.RemoveEntriesFromHistory(Globals.CurrentProfile.History);

                                editingCommand.ListFrom.Commands.Save();

                                this.HideSuggester("command deleted, should reopen");
                                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.IgnoreNewRegenerations = false;
                                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAllInvokeOnMainThread(delegate
                                {
                                    this.promptHandler.OpenPrompt();
                                    this.ShowSuggester();

                                    //if (selectedIndex >= this.Suggester.ItemCount)
                                    //{
                                    //    selectedIndex = this.Suggester.ItemCount - 1;
                                    //}

                                    this.MakeSuggestion(true);

                                    //this.Suggester.SelectedIndex = selectedIndex;
                                    //this.Suggester.CenterSelectedItem();
                                    //this.showInfoBoxTimer.Start();

                                    this.promptHandler.SetupDialog.UpdateCurrentListCommands();
                                });
                            }
                        }
                        finally
                        {
                            Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.IgnoreNewRegenerations = false;
                            this.ignoreActivationLost = false;
                        }
                    }
                    else if ((editingFunction = editingObject as CompositeItem<Function, List>) != null)
                    {
                        try
                        {
                            this.ignoreActivationLost = true;
                            if (userHasAlreadyBeenPrompted || UIMessageBox.Show(
                                String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteFunction, editingFunction.Item.Name),
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.YesNo,
                                UIMessageBoxIcon.Warning,
                                UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
                            {
                                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.IgnoreNewRegenerations = true;
                                int selectedIndex = this.Suggester.SelectedIndex;
                                editingFunction.ListFrom.Functions.Remove(editingFunction.Item);
                                editingFunction.Item.RemoveEntriesFromHistory(Globals.CurrentProfile.History);

                                editingFunction.ListFrom.Functions.Save();
                                this.HideSuggester("deleting function, should reopen");
                                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.IgnoreNewRegenerations = false;
                                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAllInvokeOnMainThread(delegate
                                {
                                    this.promptHandler.OpenPrompt();
                                    this.ShowSuggester();

                                    //if (selectedIndex >= this.Suggester.ItemCount)
                                    //{
                                    //    selectedIndex = this.Suggester.ItemCount - 1;
                                    //}

                                    //this.Suggester.SelectedIndex = selectedIndex;
                                    //this.Suggester.CenterSelectedItem();
                                    this.MakeSuggestion(true);

                                    this.promptHandler.SetupDialog.UpdateCurrentListFunctions();
                                });
                            }
                        }
                        finally
                        {
                            Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.IgnoreNewRegenerations = false;
                            this.ignoreActivationLost = false;
                        }
                    }
                }
                else if ((item.Type & SuggestionItemType.History) != 0)
                {
                    //bool wasRaisingChanged = PromptuSettings.CurrentProfile.History.RaiseChanged;
                    //PromptuSettings.CurrentProfile.History.RaiseChanged = false;

                    Globals.CurrentProfile.History.AddChangeEventBlocker();

                    string itemId = null;

                    ParameterHelpContext contextInfo = this.mainInfoContextInfo;
                    if (contextInfo != null)
                    {
                        string promptSuggestionAreaText = this.PromptSuggestionAreaText;

                        List<string> parameterizedPartTyped = new List<string>(SuggestionUtilities.ExtractNameAndParametersFrom(promptSuggestionAreaText));
                        string actualFilter;
                        if (contextInfo.ParameterIndex + 1 < parameterizedPartTyped.Count)
                        {
                            string fullFilter = parameterizedPartTyped[contextInfo.ParameterIndex + 1];

                            //bool inQuote = fullFilter.CountOf('"', true) % 2 != 0;

                            //if (inQuote)
                            //{
                            //    fullFilter = fullFilter.Substring(1);
                            //}

                            actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, contextInfo);
                        }
                        else
                        {
                            actualFilter = String.Empty;
                        }

                        string lookingFor = actualFilter + item.Text;
                        if (promptSuggestionAreaText.CountOf('"', true) % 2 == 0)
                        {
                            lookingFor = lookingFor.Escape();
                        }

                        lookingFor = String.Format(CultureInfo.CurrentCulture, "\"{0}\"", lookingFor);

                        object currentItem = contextInfo.GetCurrentItem();
                        Command command = currentItem as Command;
                        Function function;
                        if (command != null)
                        {
                            itemId = Command.GenerateItemId(command, contextInfo.GetCurrentItemList());

                            List<HistoryDetails> historyItems = new List<HistoryDetails>(Globals.CurrentProfile.History.Count);
                            for (int i = 0; i < Globals.CurrentProfile.History.Count; i++)
                            {
                                HistoryDetails details = Globals.CurrentProfile.History[i];
                                if (details.ItemId == itemId)
                                {
                                    string proposedValue = SuggestionUtilities.ReplaceCommandParameter(
                                        details.EntryValue,
                                        contextInfo.ParameterIndex,
                                        lookingFor,
                                        "&?;");

                                    historyItems.Add(new HistoryDetails(proposedValue, details.ItemId));
                                }
                                else
                                {
                                    historyItems.Add(details.Clone());
                                }
                            }

                            Globals.CurrentProfile.History.Clear(true);
                            Globals.CurrentProfile.History.AddRange(historyItems);
                        }
                        else if ((function = currentItem as Function) != null)
                        {
                            List<HistoryDetails> historyItems = new List<HistoryDetails>(Globals.CurrentProfile.History.Count);
                            for (int i = 0; i < Globals.CurrentProfile.History.Count; i++)
                            {
                                HistoryDetails details = Globals.CurrentProfile.History[i];
                                if (details.EntryValue.StartsWith(function.Name + '(', StringComparison.CurrentCultureIgnoreCase))
                                {
                                    string proposedValue = SuggestionUtilities.ReplaceFunctionParameterIfMatch(
                                        details.EntryValue,
                                        function.Parameters.Count,
                                        contextInfo.ParameterIndex,
                                        lookingFor,
                                        "&?;");

                                    historyItems.Add(new HistoryDetails(proposedValue, details.ItemId));
                                }
                                else
                                {
                                    historyItems.Add(details.Clone());
                                }
                            }

                            Globals.CurrentProfile.History.Clear(true);
                            Globals.CurrentProfile.History.AddRange(historyItems);
                        }
                    }
                    else
                    {
                        //int numberOfCharactersToRemove = e.Item.Text.Length + 1;
                        if (this.SuggestionMode == SuggestionMode.History)
                        {
                            Globals.CurrentProfile.History.ComplexHistory.Remove(item.Text);
                        }
                        else
                        {
                            string secondaryFilter = this.filter + item.Text;
                            string primaryFilter = secondaryFilter + " ";

                            //if (this.filter.Length > 0)
                            //{
                            //    numberOfCharactersToRemove++;
                            //}

                            //PromptuSettings.CurrentProfile.History.RemoveAllThatStartWith(secondaryFilter);
                            Globals.CurrentProfile.History.RemoveAllThatStartWith(primaryFilter);
                            Globals.CurrentProfile.History.Remove(secondaryFilter);
                            //PromptuSettings.CurrentProfile.History.ComplexHistory.RemoveAllThatStartWith(primaryFilter);
                        }

                        // if (!PromptuSettings.CurrentProfile.History.RaiseChanged)
                        //{
                        Globals.CurrentProfile.History.RemoveChangeEventBlocker();
                        //}

                        //numberOfCharactersToRemove--;

                        //PromptuSettings.CurrentProfile.History.ComplexHistory.RemoveAllThatStartWith(secondaryFilter);
                    }

                    Globals.CurrentProfile.History.Save();
                    this.HideSuggester("deleting history item, should reopen");
                    Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAllInvokeOnMainThread(delegate
                    {
                        bool showHistoryWindow = this.SuggestionMode == SuggestionMode.History;
                        this.ResetAll();
                        this.populationProvider.ResetFilterAtLastPopulation();

                        if (showHistoryWindow)
                        {
                            this.promptHandler.suggestionMode = SuggestionMode.History;
                        }

                        this.UpdateFilter();
                        this.ShowSuggester();
                        this.MakeSuggestion(false);
                    });
                }
            }
            
            //private void PerformOperationOnSuggestionItem(SuggestionItem item)
            //{
            //    InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
            //    string name = this.filter + e.Item.Text;
            //    switch (e.Operation)
            //    {
            //        //case SuggestionItemOperation.Edit:
            //        //    if ((e.Item.Type & SuggestionItemType.Command) != 0)
            //        //    {
            //        //        bool found;
            //        //        CompositeItem<Command, List> command;
            //        //        GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
            //        //        if (found && compositeItem != null && (command = compositeItem.TryGetCommand(name)) != null)
            //        //        {
            //        //            this.promptHandler.setupDialog.ListSelector.SetSelectedList(command.ListFrom);
            //        //            this.promptHandler.ShowSetupDialog();
            //        //            this.promptHandler.setupDialog.EditCommand(name);
            //        //        }
            //        //    }
            //        //    else if ((e.Item.Type & SuggestionItemType.Function) != 0)
            //        //    {
            //        //        bool found;
            //        //        GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
            //        //        if (found && compositeItem != null && compositeItem.StringFunctions.Count > 0)
            //        //        {
            //        //            string parameterSignature;
            //        //            if (compositeItem.StringFunctions.Count == 1)
            //        //            {
            //        //                parameterSignature = compositeItem.StringFunctions[0].Item.ParameterSignature;
            //        //            }
            //        //            else
            //        //            {
            //        //                List<Function> conflictingFunctions = new List<Function>();
            //        //                foreach (CompositeItem<Function, List> functionItem in compositeItem.StringFunctions)
            //        //                {
            //        //                    conflictingFunctions.Add(functionItem.Item);
            //        //                }

            //        //                FunctionDisambiguatorPresenter disambiguator = new FunctionDisambiguatorPresenter(
            //        //                    name,
            //        //                    Localization.UIResources.FunctionDisambiguatorInformationTextEdit,
            //        //                    conflictingFunctions);
            //        //                if (disambiguator.ShowDialog() != UIDialogResult.OK || disambiguator.SelectedFunction == null)
            //        //                {
            //        //                    return;
            //        //                }

            //        //                parameterSignature = disambiguator.SelectedFunction.ParameterSignature;
            //        //            }

            //        //            CompositeItem<Function, List> function = compositeItem.TryGetStringFunction(name, parameterSignature);
            //        //            if (function != null)
            //        //            {
            //        //                this.promptHandler.setupDialog.ListSelector.SetSelectedList(function.ListFrom);
            //        //                this.promptHandler.ShowSetupDialog();
            //        //                this.promptHandler.setupDialog.EditFunction(name, parameterSignature);
            //        //            }
            //        //        }
            //        //    }

            //        //    break;
            //        case SuggestionItemOperation.Delete:
            //            //if ((e.Item.Type & SuggestionItemType.Command) != 0)
            //            //{
            //            //    bool found;
            //            //    CompositeItem<Command, List> command;
            //            //    GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
            //            //    if (found && compositeItem != null && (command = compositeItem.TryGetCommand(name)) != null)
            //            //    {
            //            //        this.ignoreActivationLost = true;
            //            //        if (UIMessageBox.Show(
            //            //            String.Format(Localization.MessageFormats.ConfirmDeleteCommand, command.Item.Name),
            //            //            Localization.Promptu.AppName,
            //            //            UIMessageBoxButtons.YesNo,
            //            //            UIMessageBoxIcon.Warning,
            //            //            UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
            //            //        {
            //            //            int selectedIndex = this.Suggester.SelectedIndex;
            //            //            command.ListFrom.Commands.Remove(command.Item);
            //            //            command.Item.RemoveEntriesFromHistory(Globals.CurrentProfile.History);

            //            //            command.ListFrom.Commands.Save();

            //            //            this.HideSuggester("command deleted, should reopen");
            //            //            Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
            //            //            this.ShowSuggester();

            //            //            if (selectedIndex >= this.Suggester.ItemCount)
            //            //            {
            //            //                selectedIndex = this.Suggester.ItemCount - 1;
            //            //            }

            //            //            this.Suggester.SelectedIndex = selectedIndex;
            //            //            this.Suggester.CenterSelectedItem();

            //            //            this.promptHandler.setupDialog.CommandSetupPanel.UpdateItemsListView();
            //            //        }

            //            //        this.ignoreActivationLost = false;
            //            //    }
            //            //}
            //            //else if ((e.Item.Type & SuggestionItemType.Function) != 0)
            //            //{
            //            //    bool found;
            //            //    GroupedCompositeItem compositeItem = Globals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(name, out found);
            //            //    if (found && compositeItem != null && compositeItem.StringFunctions.Count > 0)
            //            //    {
            //            //        string parameterSignature;
            //            //        if (compositeItem.StringFunctions.Count == 1)
            //            //        {
            //            //            parameterSignature = compositeItem.StringFunctions[0].Item.ParameterSignature;
            //            //        }
            //            //        else
            //            //        {
            //            //            List<Function> conflictingFunctions = new List<Function>();
            //            //            foreach (CompositeItem<Function, List> functionItem in compositeItem.StringFunctions)
            //            //            {
            //            //                conflictingFunctions.Add(functionItem.Item);
            //            //            }

            //            //            FunctionDisambiguatorPresenter disambiguator = new FunctionDisambiguatorPresenter(
            //            //                name,
            //            //                Localization.UIResources.FunctionDisambiguatorInformationTextDelete,
            //            //                conflictingFunctions);
            //            //            if (disambiguator.ShowDialog() != UIDialogResult.OK || disambiguator.SelectedFunction == null)
            //            //            {
            //            //                return;
            //            //            }

            //            //            parameterSignature = disambiguator.SelectedFunction.ParameterSignature;
            //            //        }

            //            //        CompositeItem<Function, List> function = compositeItem.TryGetStringFunction(name, parameterSignature);
            //            //        if (function != null)
            //            //        {
            //            //            this.ignoreActivationLost = true;
            //            //            if (UIMessageBox.Show(
            //            //                String.Format(Localization.MessageFormats.ConfirmDeleteFunction, function.Item.Name),
            //            //                Localization.Promptu.AppName,
            //            //                UIMessageBoxButtons.YesNo,
            //            //                UIMessageBoxIcon.Warning,
            //            //                UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
            //            //            {
            //            //                int selectedIndex = this.Suggester.SelectedIndex;
            //            //                function.ListFrom.Functions.Remove(function.Item);
            //            //                function.Item.RemoveEntriesFromHistory(Globals.CurrentProfile.History);

            //            //                function.ListFrom.Functions.Save();
            //            //                this.HideSuggester("deleing function, should reopen");
            //            //                Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
            //            //                this.ShowSuggester();

            //            //                if (selectedIndex >= this.Suggester.ItemCount)
            //            //                {
            //            //                    selectedIndex = this.Suggester.ItemCount - 1;
            //            //                }

            //            //                this.Suggester.SelectedIndex = selectedIndex;
            //            //                this.Suggester.CenterSelectedItem();

            //            //                this.promptHandler.setupDialog.FunctionSetupPanel.UpdateItemsListView();
            //            //            }

            //            //            this.ignoreActivationLost = false;
            //            //        }
            //            //    }
            //            //}
            //            //else if ((e.Item.Type & SuggestionItemType.History) != 0)
            //            //{
            //            //    //bool wasRaisingChanged = PromptuSettings.CurrentProfile.History.RaiseChanged;
            //            //    //PromptuSettings.CurrentProfile.History.RaiseChanged = false;

            //            //    Globals.CurrentProfile.History.AddChangeEventBlocker();

            //            //    string itemId = null;

            //            //    MainInfoContextInfo contextInfo = this.mainInfoContextInfo;
            //            //    if (contextInfo != null)
            //            //    {
            //            //        string promptSuggestionAreaText = this.PromptSuggestionAreaText;

            //            //        List<string> parameterizedPartTyped = new List<string>(SuggestionUtilities.ExtractNameAndParametersFrom(promptSuggestionAreaText));
            //            //        string actualFilter;
            //            //        if (contextInfo.ParameterIndex + 1 < parameterizedPartTyped.Count)
            //            //        {
            //            //            string fullFilter = parameterizedPartTyped[contextInfo.ParameterIndex + 1];

            //            //            //bool inQuote = fullFilter.CountOf('"', true) % 2 != 0;

            //            //            //if (inQuote)
            //            //            //{
            //            //            //    fullFilter = fullFilter.Substring(1);
            //            //            //}

            //            //            actualFilter = SuggestionUtilities.GetWhatFilterShouldBe(fullFilter, fullFilter.Length, contextInfo);
            //            //        }
            //            //        else
            //            //        {
            //            //            actualFilter = String.Empty;
            //            //        }

            //            //        string lookingFor = actualFilter + e.Item.Text;
            //            //        if (promptSuggestionAreaText.CountOf('"', true) % 2 == 0)
            //            //        {
            //            //            lookingFor = lookingFor.Escape();
            //            //        }

            //            //        lookingFor = String.Format("\"{0}\"", lookingFor);

            //            //        object currentItem = contextInfo.GetCurrentItem();
            //            //        Command command = currentItem as Command;
            //            //        Function function;
            //            //        if (command != null)
            //            //        {
            //            //            itemId = Command.GenerateItemId(command, contextInfo.GetCurrentItemList());

            //            //            List<HistoryDetails> historyItems = new List<HistoryDetails>(Globals.CurrentProfile.History.Count);
            //            //            for (int i = 0; i < Globals.CurrentProfile.History.Count; i++)
            //            //            {
            //            //                HistoryDetails details = Globals.CurrentProfile.History[i];
            //            //                if (details.ItemId == itemId)
            //            //                {
            //            //                    string proposedValue = SuggestionUtilities.ReplaceCommandParameter(
            //            //                        details.EntryValue,
            //            //                        contextInfo.ParameterIndex,
            //            //                        lookingFor,
            //            //                        "&?;");
                                            
            //            //                    historyItems.Add(new HistoryDetails(proposedValue, details.ItemId));
            //            //                }
            //            //                else
            //            //                {
            //            //                    historyItems.Add(details.Clone());
            //            //                }
            //            //            }

            //            //            Globals.CurrentProfile.History.Clear(true);
            //            //            Globals.CurrentProfile.History.AddRange(historyItems);
            //            //        }
            //            //        else if ((function = currentItem as Function) != null)
            //            //        {
            //            //            List<HistoryDetails> historyItems = new List<HistoryDetails>(Globals.CurrentProfile.History.Count);
            //            //            for (int i = 0; i < Globals.CurrentProfile.History.Count; i++)
            //            //            {
            //            //                HistoryDetails details = Globals.CurrentProfile.History[i];
            //            //                if (details.EntryValue.StartsWith(function.Name + '(', StringComparison.CurrentCultureIgnoreCase))
            //            //                {
            //            //                    string proposedValue = SuggestionUtilities.ReplaceFunctionParameterIfMatch(
            //            //                        details.EntryValue,
            //            //                        function.Parameters.Count,
            //            //                        contextInfo.ParameterIndex,
            //            //                        lookingFor,
            //            //                        "&?;");

            //            //                    historyItems.Add(new HistoryDetails(proposedValue, details.ItemId));
            //            //                }
            //            //                else
            //            //                {
            //            //                    historyItems.Add(details.Clone());
            //            //                }
            //            //            }

            //            //            Globals.CurrentProfile.History.Clear(true);
            //            //            Globals.CurrentProfile.History.AddRange(historyItems);
            //            //        }
            //            //    }
            //            //    else
            //            //    {
            //            //        //int numberOfCharactersToRemove = e.Item.Text.Length + 1;
            //            //        if (this.SuggestionMode == SuggestionMode.History)
            //            //        {
            //            //            Globals.CurrentProfile.History.ComplexHistory.Remove(e.Item.Text);
            //            //        }
            //            //        else
            //            //        {
            //            //            string secondaryFilter = this.filter + e.Item.Text;
            //            //            string primaryFilter = secondaryFilter + " ";

            //            //            //if (this.filter.Length > 0)
            //            //            //{
            //            //            //    numberOfCharactersToRemove++;
            //            //            //}

            //            //            //PromptuSettings.CurrentProfile.History.RemoveAllThatStartWith(secondaryFilter);
            //            //            Globals.CurrentProfile.History.RemoveAllThatStartWith(primaryFilter);
            //            //            Globals.CurrentProfile.History.Remove(secondaryFilter);
            //            //            //PromptuSettings.CurrentProfile.History.ComplexHistory.RemoveAllThatStartWith(primaryFilter);
            //            //        }

            //            //       // if (!PromptuSettings.CurrentProfile.History.RaiseChanged)
            //            //        //{
            //            //        Globals.CurrentProfile.History.RemoveChangeEventBlocker();
            //            //        //}

            //            //        //numberOfCharactersToRemove--;

            //            //        //PromptuSettings.CurrentProfile.History.ComplexHistory.RemoveAllThatStartWith(secondaryFilter);
            //            //    }

            //            //    Globals.CurrentProfile.History.Save();
            //            //    this.HideSuggester("deleting history item, should reopen");
            //            //    Globals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
            //            //    bool showHistoryWindow = this.SuggestionMode == SuggestionMode.History;
            //            //    this.ResetAll();
            //            //    this.populationProvider.ResetFilterAtLastPopulation();

            //            //    if (showHistoryWindow)
            //            //    {
            //            //        this.promptHandler.suggestionMode = SuggestionMode.History;
            //            //    }

            //            //    this.UpdateFilter();
            //            //    this.ShowSuggester();
            //            //    this.MakeSuggestion(false);
            //            //}

            //            break;
            //        default:
            //            break;
            //    }
            //}

            private SuggestionItem SelectedSuggestionItem
            {
                get
                {
                    int index = this.Suggester.SelectedIndex;
                    if (index >= 0 && index < this.Suggester.ItemCount)
                    {
                        return this.Suggester.GetItem(index);
                    }

                    return null;
                }
            }

            private void HandleSuggesterVisibleChanged(object sender, EventArgs e)
            {
                if (this.Suggester.Visible)
                {
                    this.populationProvider.StartProcessingIconLoadOrders();
                }
            }

            private void HandleSuggesterMouseDoubleClick(object sender, MouseEventArgs e)
            {
                this.forceNextExecute = false;
                this.autocorrectedLastKeyPress = false;
                if (this.Suggester.SelectedIndex >= 0)
                {
                    KeyboardSnapshot keyboardSnapshot = InternalGlobals.GuiManager.ToolkitHost.TakeKeyboardSnapshot();
                    //bool shiftKeyPressed = Globals.GuiManager.ToolkitHost.Keyboard.ShiftKeyPressed;
                    //bool ctrlKeyPressed = Globals.GuiManager.ToolkitHost.Keyboard.CtrlKeyPressed;
                    SuggestionItem item = this.SelectedSuggestionItem;
                    string suggestion = item.Text;
                    string text = this.filter + suggestion;

                    bool continueSuggesting = false;

                    if (item.Type == SuggestionItemType.Folder)
                    {
                        this.AcceptCurrentSuggestion(true);
                        continueSuggesting = true;
                        if (!text.EndsWith(PathSeparatorString))
                        {
                            text += Path.DirectorySeparatorChar;
                        }
                    }
                    else if (item.Type == SuggestionItemType.File)
                    {
                        this.ExecuteCurrent(keyboardSnapshot.CtrlKeyPressed, keyboardSnapshot.ShiftKeyPressed, false);
                    }
                    else if (item.Type == SuggestionItemType.Namespace)
                    {
                        this.AcceptCurrentSuggestion(true);
                        continueSuggesting = true;
                        if (!text.EndsWith(NamespaceSeparatorString))
                        {
                            text += '.';
                        }
                    }
                    else
                    {
                        this.ExecuteCurrent(keyboardSnapshot.CtrlKeyPressed, keyboardSnapshot.ShiftKeyPressed, false);
                        this.UpdateFilter();
                    }
                    //else
                    //{
                    //    //continueSuggesting = true;
                    //}

                    if (continueSuggesting)
                    {
                        this.PromptSuggestionAreaText = text;
                        InternalGlobals.CurrentSkinInstance.Prompt.SelectionStart = this.beginningOfSuggestionArea + text.Length;

                        this.filter = text;

                        this.ShowSuggester();
                    }
                }
            }

            private void HandlePromptTextualInputMouseWheel(object sender, MouseEventArgs e)
            {
                if (e.Delta > 0)
                {
                    this.Suggester.ScrollSuggestions(Direction.Up);
                }
                else
                {
                    this.Suggester.ScrollSuggestions(Direction.Down);
                }

                this.StopAndHideItemInfoBox();
            }

            //private void SuggesterWM_ActivateRecieved(object sender, MessageEventArgs e)
            //{
            //    //if (e.WParam != (IntPtr)WindowsMessages.WA_INACTIVE)
            //    //{
            //    //    bool ignoreBefore = this.ignorePromptLostFocus;
            //    //    this.ignorePromptLostFocus = true;
            //    //    this.promptHandler.prompt.Activate();
            //    //    this.promptHandler.prompt.TextualInput.Focus();
            //    //    this.ignorePromptLostFocus = ignoreBefore;
            //    //}
            //}

            private void MoveFocusBackToPrompt(object sender, EventArgs e)
            {
                if (this.Suggester.ContainsFocus || InternalGlobals.CurrentSkinInstance.Prompt.ContainsFocus)
                {
                    InternalGlobals.CurrentSkinInstance.Prompt.Activate();
                    InternalGlobals.CurrentSkinInstance.Prompt.FocusOnTextInput();
                }
                else
                {
                    this.HideSuggester("focus was not on the suggester or the prompt.");
                    this.ClosePrompt();
                }
            }

            private void ClosePrompt()
            {
                this.HideAndDestroyMainInfoBox();
                this.promptHandler.ClosePrompt();
                this.DisposeOfLastPopulationInfo();
            }

            //private void HideInformationBoxes()
            //{
                
            //}

            private void HideAndDestroyMainInfoBox()
            {
                if (this.mainInfoContextInfo != null)
                {
                    this.mainInfoContextInfo.CurrentIndexChanged -= this.HandleItemContextChanged;
                    this.mainInfoContextInfo = null;
                }

                this.informationBoxManager.HideAndDestroy(this.informationBoxManager.ParameterHelpBox);
            }

            private void ResetSuggestion()
            {
                this.ResetSuggestion(true);
            }

            private void ResetSuggestion(bool hideInformationBoxes)
            {
                //this.ignorePromptLostFocus = true;
                this.lastSuggestedIndex = null;
                this.filter = String.Empty;
                if (this.Suggester.Visible)
                {
                    this.HideSuggester("resetting suggestion");
                }

                this.showInfoBoxTimer.Stop();

                if (hideInformationBoxes)
                {
                    this.informationBoxManager.HideAndDestroyAllExceptParameterHelp();
                }

                this.Suggester.SelectedIndex = -1;
                //this.ignorePromptLostFocus = false;
                this.promptHandler.lastSuggested = string.Empty;
                //this.promptHandler.lastPathSuggestionDirectoryContents = null;
                //this.promptHandler.couldBePath = false;
                //this.addQuoteWithSpace = false;
                //this.promptHandler.suggestionIsNamespace = false;
                this.promptHandler.suggestionMode = SuggestionMode.Normal;
                this.beginningOfSuggestionArea = 0;
            }

            //private void ApplicationIsIdle(object sender, EventArgs e)
            //{

            //}

            private PositioningContext CreatePositioningContext()
            {
                return new PositioningContext(
                    InternalGlobals.CurrentSkin,
                    InternalGlobals.CurrentSkinInstance,
                    new InfoBoxes(this.informationBoxManager)); 
            }

            private bool ShowSuggester()
            {
                //ExceptionLogger.LogCurrentThreadStack("nothing");
                this.ignoreActivationLost = true;
                this.Suggester.EnsureCreated();
                bool success = this.populationProvider.PopulateSuggester(this.Suggester, this.filter, true, false, this.mainInfoContextInfo);

                string suggestionAreaText = this.PromptSuggestionAreaText;

                if (
                     !this.Suggester.Visible
                     && this.Suggester.ItemCount > 0
                     && (this.promptHandler.suggestionMode == SuggestionMode.History ||
                     ((suggestionAreaText.Length < 1 || (suggestionAreaText[0] != '/' && suggestionAreaText[0] != '\\')))))
                {
                    if (PromptuHookManager.RaiseShowingSuggestionProvider() == HookAction.Continue)
                    {
                        //suggestionAreaText.Length > 0 && this.Prompt.Text[0] != '=' && 
                        //this.Suggester.Location = new Point(this.promptHandler.prompt.LocationOnScreen.X, this.promptHandler.prompt.LocationOnScreen.Y + this.promptHandler.prompt.PromptHeight);

                        //Trace.WriteLine("Showing suggester.");

                        InternalGlobals.CurrentSkinInstance.LayoutManager.PositionSuggestionProvider(
                            this.CreatePositioningContext());

                        this.Suggester.Show();
                        //this.Prompt.Show(this.Suggester);
                        this.Suggester.BringToFront();
                        //this.Suggester.TopMost = true;

                        if (this.informationBoxManager.ParameterHelpBox != null && this.informationBoxManager.ParameterHelpBox.Visible)
                        {
                            this.informationBoxManager.ParameterHelpBox.BringToFront();
                        }
                    }

                    //NativeMethods.SetParent(this.Prompt.Handle, this.Suggester.Handle);
                    //this.Prompt.BringToFront();
                }
                else
                {
                    //Trace.WriteLine(String.Format("NOT showing suggester. !this.Suggester.Visible: '{0}' this.Suggester.ItemCount = '{1}' this.promptHandler.suggestionMode = '{2}' suggestionAreaText.Length = '{3}'",
                    //    !this.Suggester.Visible,
                    //    this.Suggester.ItemCount,
                    //    this.promptHandler.suggestionMode,
                    //    suggestionAreaText.Length));
                }

                InternalGlobals.CurrentSkinInstance.Prompt.Activate();
                InternalGlobals.CurrentSkinInstance.Prompt.FocusOnTextInput();

                this.ignoreActivationLost = false;

                return success;
            }

            protected override void NotifyPromptOpenedCore(bool fullReset)
            {
                this.HideAndDestroyMainInfoBox();
                this.populationProvider.ResetFilterAtLastPopulation();
                if (fullReset)
                {
                    this.ResetAll();
                }
                else
                {
                    this.ResetSuggestion();
                }
            }

            private void DisposeOfLastPopulationInfo()
            {
                this.suggestionProvider.PopulationInfo = null;
                this.populationProvider.ResetFilterAtLastPopulation();
            }

            private void ResetAll()
            {
                this.DisposeOfLastPopulationInfo();
                this.ResetSuggestion();
                this.invisibleEndQuote = null;
            }

            //private void CancelIconLoading()
            //{
            //    if (this.iconLoadingThread != null && this.iconLoadingThread.IsAlive)
            //    {
            //        this.stopIconLoading = true;
            //        //if (!this.iconLoadingThread.Join(CancelIconLoadingMaxJoinTime))
            //        //{
            //        //    this.iconLoadingThread.Abort();
            //        //}
            //    }
            //}

            //private void RepopulateSuggester(bool resizePrompt)
            //{
            //    this.PopulateSuggester(resizePrompt);
            //    if (this.suggestionItemsAndIndexes.Count > 0)
            //    {
            //        if (this.Suggester.Visible)
            //        {
            //            this.HideSuggester();
            //        }
            //    }
            //}

            private void HandleSuggestionProviderItemContextMenuOpening(object sender, CancelEventArgs e)
            {
                SuggestionItem selectedItem = this.SelectedSuggestionItem;
                if (selectedItem != null)
                {
                    bool isHistoryItem = (selectedItem.Type & SuggestionItemType.History) != 0;
                    bool isCommandOrFunction = (selectedItem.Type & SuggestionItemType.Command) != 0 || (selectedItem.Type & SuggestionItemType.Function) != 0;
                    this.deleteItem.Available = isCommandOrFunction || isHistoryItem;
                    this.editItem.Available = isCommandOrFunction;

                    if (!isCommandOrFunction && !isHistoryItem)
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }

            protected override void AttachToCurrentPromptCore()
            {
                this.Suggester.UserInteractionFinished += this.MoveFocusBackToPrompt;
                this.Suggester.MouseDoubleClick += this.HandleSuggesterMouseDoubleClick;
                this.Suggester.VisibleChanged += this.HandleSuggesterVisibleChanged;
                this.Suggester.DesiredIconSizeChanged += this.HandleSuggesterDesiredIconSizeChanged;
                this.Suggester.SelectedItemContextMenuOpening += this.HandleSuggestionProviderItemContextMenuOpening;
                InternalGlobals.SuggestionItemContextMenuBridge.BridgedCollection = this.Suggester.SelectedItemContextMenu.NativeContextMenuInterface.Items;
                InternalGlobals.PromptContextMenuBridge.BridgedCollection = this.Prompt.PromptContextMenu.NativeContextMenuInterface.Items;
                //REVISIT this.Suggester.PerformOperation += this.HandleSuggesterPerformOperation;
                this.Suggester.SelectedIndexChanged += this.HandleSuggesterSelectedIndexChanged;
                InternalGlobals.CurrentSkinInstance.Prompt.MouseWheel += this.HandlePromptTextualInputMouseWheel;
                InternalGlobals.CurrentSkinInstance.Prompt.MouseDown += this.HandlePromptTextualInputMouseDown;
                InternalGlobals.CurrentSkinInstance.Prompt.MouseUp += this.HandlePromptTextualInputMouseUp;
                //this.promptHandler.prompt.MouseDownOnPrompt += this.HandlePromptMouseDown;
                //try
                //{
                    InternalGlobals.GuiManager.ToolkitHost.WindowManager.TryRegister(
                        InternalGlobals.CurrentSkinInstance.Prompt, true);
                //}
                //catch (OopsTryThatAgainException)
                //{

                //}

                InternalGlobals.GuiManager.ToolkitHost.WindowManager.TryRegister(
                    InternalGlobals.CurrentSkinInstance.SuggestionProvider,
                    InternalGlobals.CurrentSkinInstance.SuggestionProvider,
                    new Action<ISuggestionProvider>(this.HandleSuggesterActivate),
                    true);

                this.DetermineIconSize();
                //this.windowManager.RegisterWindow(this.promptHandler.prompt.Handle);
                //this.windowManager.RegisterWindow(suggester.Handle, suggester, new Action<ISuggester>(this.HandleSuggesterActivate));
            }

            protected override void DetachFromCurrentPromptCore()
            {
                if (this.Suggester != null)
                {
                    this.Suggester.UserInteractionFinished -= this.MoveFocusBackToPrompt;
                    this.Suggester.MouseDoubleClick -= this.HandleSuggesterMouseDoubleClick;
                    this.Suggester.VisibleChanged -= this.HandleSuggesterVisibleChanged;
                    this.Suggester.DesiredIconSizeChanged -= this.HandleSuggesterDesiredIconSizeChanged;
                    this.Suggester.SelectedItemContextMenuOpening -= this.HandleSuggestionProviderItemContextMenuOpening;
                    InternalGlobals.SuggestionItemContextMenuBridge.BridgedCollection = null;
                    InternalGlobals.PromptContextMenuBridge.BridgedCollection = null;
                    //REVISIT this.Suggester.PerformOperation -= this.HandleSuggesterPerformOperation;
                    this.Suggester.SelectedIndexChanged -= this.HandleSuggesterSelectedIndexChanged;
                    InternalGlobals.CurrentSkinInstance.Prompt.MouseWheel -= this.HandlePromptTextualInputMouseWheel;
                    InternalGlobals.CurrentSkinInstance.Prompt.MouseDown -= this.HandlePromptTextualInputMouseDown;
                    InternalGlobals.CurrentSkinInstance.Prompt.MouseUp -= this.HandlePromptTextualInputMouseUp;
                    
                    InternalGlobals.GuiManager.ToolkitHost.WindowManager.Unregister(
                        InternalGlobals.CurrentSkinInstance.Prompt);

                    InternalGlobals.GuiManager.ToolkitHost.WindowManager.Unregister(
                        InternalGlobals.CurrentSkinInstance.SuggestionProvider);
                    
                    //this.promptHandler.prompt.MouseDownOnPrompt -= this.HandlePromptMouseDown;
                    //this.windowManager.UnregisterWindow(suggester.Handle);
                    //this.windowManager.UnregisterWindow(this.promptHandler.prompt.Handle);
                }

                //this.Suggester = null;
            }

            private static int CompareSuggestionItems(SuggestionItem x, SuggestionItem y)
            {
                return x.Text.CompareTo(y.Text);
            }

            private void HideSuggester(string reason)
            {
                //Trace.WriteLine(String.Format("Hiding suggester. reason = '{0}'", reason));
                //InternalGlobals.CurrentProfile.SuggesterSize = this.Suggester.SaveSize;
                this.HideItemInfoBox();
                //qNativeMethods.SetParent(this.Prompt.Handle, IntPtr.Zero);
                this.Suggester.Hide();
                this.beginningOfSuggestionArea = 0;
            }

            private void AcceptCurrentSuggestion(bool close)
            {
                this.AcceptCurrentSuggestion(close, null);
            }

            //private void AcceptCurrentSuggestion(bool close, string addToSuggestion)
            //{
            //    int areaLength;
            //    this.AcceptCurrentSuggestion(close, addToSuggestion, out areaLength);
            //}

            private void AcceptCurrentSuggestion(bool close, char addToSuggestion)
            {
                this.AcceptCurrentSuggestion(close, new string(addToSuggestion, 1));
            }

            private void AcceptCurrentSuggestion(bool close, string addToSuggestion)
            {
                //textLength = -1;
                if (this.Suggester.SelectedIndex >= 0)
                {
                    SuggestionItem selectedItem = this.Suggester.GetItem(this.Suggester.SelectedIndex);
                    //string promptText = this.promptHandler.prompt.TextualInput.InputText;
                    string itemText = selectedItem.Text;

                    string text = this.filter;
                    //int textEndOffset = 0;

                    int lastIndexOfSpace;

                    bool addQuote = false;

                    if (!Utilities.LooksLikeValidPath(this.Prompt.Text)
                        && this.promptHandler.suggestionMode == SuggestionMode.Normal 
                        && (itemText.Contains(" ") || itemText.Contains("\""))
                        && text.CountOf('"', true) % 2 == 0
                        && (lastIndexOfSpace = text.LastIndexOf(" ")) >= 0)
                    {
                        string before = text.Substring(0, lastIndexOfSpace + 1);
                        string after = text.Substring(lastIndexOfSpace + 1);

                        text = String.Format(CultureInfo.CurrentCulture, "{0}\"{1}", before, after.Escape());
                        //textEndOffset = 1;
                        itemText = itemText.Escape();
                        this.invisibleEndQuote = new InvisibleEndQuote(
                            this.beginningOfSuggestionArea + before.Length,
                            after.CountOf(' ', false) + itemText.CountOf(' ', false));
                        //this.addQuoteWithSpace = true;
                        //this.previousQuoteIndex = this.beginingOfSuggestionArea + before.Length + 1;
                        //this.filter = text;
                    }
                    else if (this.invisibleEndQuote != null)
                    {
                        this.invisibleEndQuote.NumberOfSpacesInQuote += itemText.CountOf(' ', false);
                    }

                    //if (this.

                    text += itemText;

                    this.populationProvider.ResetFilterAtLastPopulation();

                    if (!string.IsNullOrEmpty(addToSuggestion))
                    {
                        text += addToSuggestion;
                    }

                    if (addQuote)
                    {
                        text += "\"";
                    }
                    //int indexTo = promptText.IndexOfNextBreakingChar(this.beginingOfSuggestionArea);
                    //if (indexTo == -1)
                    //{
                    //    indexTo = promptText.Length - 1;
                    //}

                    //string textBefore = promptText.Substring(0, this.beginingOfSuggestionArea);
                    //string textAfter = promptText.Substring(indexTo + 1);

                    if (this.promptHandler.suggestionMode == SuggestionMode.History)
                    {
                        this.Prompt.Text = text;
                    }
                    else
                    {
                        this.PromptSuggestionAreaText = text;
                    }
                    //text = textBefore + text + textAfter;

                    //this.promptHandler.prompt.TextualInput.InputText = text;
                    this.Prompt.SelectionStart = this.beginningOfSuggestionArea + text.Length; //+ textEndOffset;
                    this.promptHandler.lastSuggested = string.Empty;
                    //this.promptHandler.lastPathSuggestionDirectoryContents = null;
                    //this.promptHandler.couldBePath = false;
                    //this.promptHandler.suggestionIsNamespace = false;
                    if (close)
                    {
                        this.ResetSuggestion();
                    }
                }
            }

            //private void SuggesterDeactivate(object sender, EventArgs e)
            //{
            //    if (!this.promptHandler.prompt.ContainsFocus)
            //    {
            //        this.ignorePromptLostFocus = false;
            //        this.HideSuggester();
            //        this.promptHandler.prompt.ClosePrompt();
            //    }
            //}

            private void HandlePromptTextualInputMouseDown(object sender, MouseEventArgs e)
            {
                this.ResetSuggestion();
            }

            private void HandlePromptTextualInputMouseUp(object sender, MouseEventArgs e)
            {
                this.MakeInvisibleEndQuoteVisibleIfOutOfBounds(this.Prompt.SelectionStart);
            }

            private void HandlePromptMouseDown(object sender, MouseEventArgs e)
            {
                this.ResetSuggestion();
            }

            private void HandleActivationLost(object sender, EventArgs e)
            {
                if (!this.ignoreActivationLost && !InternalGlobals.IgnoreActivationLost)
                {
                    this.ResetSuggestion();
                    this.ClosePrompt();
                }
            }

            //protected override void HandlePromptWM_ActivatedReceivedCore(MessageEventArgs e)
            //{
            //    if (e.Message.WParam == (IntPtr)WindowsMessages.WA_INACTIVE)
            //    {
            //        bool hide = true;

            //        if (e.Message.LParam == this.Suggester.Handle)
            //        {
            //            hide = false;
            //        }

            //        if (hide)
            //        {
            //            foreach (IInformationBox informationBox in this.showingInformationBoxes)
            //            {
            //                if (e.Message.LParam == informationBox.Handle)
            //                {
            //                    hide = false;
            //                    break;
            //                }
            //            }
            //        }

            //        if (hide && !this.ignorePromptLostFocus)
            //        {
            //            this.ResetSuggestion();
            //            this.promptHandler.ClosePrompt();
            //        }
            //    }
            //}

            private void MakeInvisibleEndQuoteVisible()
            {
                InvisibleEndQuote invisibleEndQuote = this.invisibleEndQuote;
                if (invisibleEndQuote != null)
                {
                    int currentStartOfSelection = this.Prompt.SelectionStart;
                    int lengthOfSelection = this.Prompt.SelectionLength;

                    string insert = "\"";

                    string currentText = this.Prompt.Text;

                    int indexOfInvisibleEndQuote = invisibleEndQuote.GetIndex(currentText);

                    if (indexOfInvisibleEndQuote > 0 && currentText.CountReverseCharRun('\\', indexOfInvisibleEndQuote - 1) % 2 != 0)
                    {
                        insert = "\\" + insert;
                    }

                    this.Prompt.Text = currentText.Insert(indexOfInvisibleEndQuote, insert);

                    this.Prompt.SelectionStart = 
                        currentStartOfSelection >= indexOfInvisibleEndQuote ? currentStartOfSelection + insert.Length : currentStartOfSelection;

                    if (currentStartOfSelection <= indexOfInvisibleEndQuote && indexOfInvisibleEndQuote <= currentStartOfSelection + lengthOfSelection)
                    {
                        lengthOfSelection += insert.Length;
                    }

                    this.Prompt.SelectionLength = lengthOfSelection;

                    this.invisibleEndQuote = null;
                }
            }

            private void ExecuteCommand(string execute, ExecuteMode mode, bool saveInHistory, ParameterHelpContext contextInfo)
            {
                this.ResetSuggestion();

                

                //if (execute.StartsWith("="))
                //{
                //    try
                //    {
                //        string newText = "=" + Calculator.RegexCalc.Evaluate(execute.Substring(1)).ToString("F15").TrimEnd('0');
                //        if (newText.EndsWith("."))
                //        {
                //            newText = newText.Substring(0, newText.Length - 1);
                //        }

                //        this.Prompt.Text = newText;
                //        this.Prompt.SelectionStart = newText.Length;
                //    }
                //    catch (ConversionException ex)
                //    {
                //        MessageBoxProvider.GiveError(ex.Message, this, this.Suggester, 1);
                //    }
                //}
                //else
                if (PromptuHookManager.RaiseCommandExecuting(execute, mode) != HookAction.Return)
                {
                    this.Prompt.Text = String.Empty;
                    this.ClosePrompt();

                    this.commandQueue.AddCommand(new ParameterlessVoid(delegate()
                    {
                        this.promptHandler.ExecuteCommand(execute, saveInHistory, mode, contextInfo);
                    }));

                    //newThread.SetApartmentState(System.Threading.ApartmentState.STA);
                    //newThread.Start();
                }
            }

            private bool ExecuteCurrent(bool ctrlKeyPressed, bool shiftKeyPressed, bool force)
            {
                ExecuteMode mode = ExecuteMode.Default;

                if (ctrlKeyPressed)
                {
                    mode = ExecuteMode.ToClipboard;
                }

                string execute;

                if (!this.Suggester.Visible || this.Suggester.SelectedIndex < 0)
                {
                    this.MakeInvisibleEndQuoteVisible();
                    execute = this.Prompt.Text;

                    //if (this.addQuoteWithSpace)
                    //{
                    //    this.Prompt.Text += "\"";
                    //    this.addQuoteWithSpace = false;
                    //}
                }
                else
                {
                    SuggestionItem item = this.Suggester.GetItem(this.Suggester.SelectedIndex);

                    string itemText = item.Text;

                    if (item.Type == SuggestionItemType.Folder)
                    {
                        itemText += Path.DirectorySeparatorChar;
                    }

                    string suggestionAreaText = this.filter;

                    int lastIndexOfSpace;

                    //bool addInvisibleEndQuote;

                    if (!Utilities.LooksLikeValidPath(this.Prompt.Text)
                        && this.promptHandler.suggestionMode == SuggestionMode.Normal
                        && (itemText.Contains(" ") || itemText.Contains("\""))
                        && suggestionAreaText.CountOf('"', true) % 2 == 0
                        && (lastIndexOfSpace = suggestionAreaText.LastIndexOf(" ")) >= 0)
                    {
                        string before = suggestionAreaText.Substring(0, lastIndexOfSpace + 1);
                        string after = suggestionAreaText.Substring(lastIndexOfSpace + 1);

                        suggestionAreaText = String.Format(CultureInfo.CurrentCulture, "{0}\"{1}", before, after.Escape());
                        itemText = itemText.Escape();
                        //addInvisibleEndQuote = true;
                        this.invisibleEndQuote = new InvisibleEndQuote(
                            this.beginningOfSuggestionArea + before.Length,
                            after.CountOf(' ', false) + itemText.CountOf(' ', false));
                        //this.addQuoteWithSpace = true;
                    }

                    suggestionAreaText += itemText;

                    //if (addInvisibleEndQuote)
                    //{
                    //    this.invisibleEndQuote = new InvisibleEndQuote(
                    //}

                    //if (this.addQuoteWithSpace)
                    //{
                    //    suggestionAreaText += "\"";
                    //    this.addQuoteWithSpace = false;
                    //}

                    this.PromptSuggestionAreaText = suggestionAreaText;

                    if (!force)
                    {
                        GroupedCompositeItem compositeItem = null;

                        bool compositeItemFound;
                        if ((compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(this.Prompt.Text, out compositeItemFound)) != null
                            && compositeItemFound)
                        {
                            if ((compositeItem.ContainsCommandThatTakesAtLeastOneParameter || compositeItem.ContainsStringFunctionThatTakesAtLeastOneParameter)
                                && (!compositeItem.ContainsCommandThatTakesZeroParameters && !compositeItem.ContainsStringFunctionThatTakesZeroParameters))
                            {
                                //string addToText = String.Empty;

                                //if (this.addQuoteWithSpace)
                                //{
                                //    addToText += "\" ";
                                //    this.addQuoteWithSpace = false;
                                //}
                                //else
                                //{
                                //    addToText = " ";
                                //}
                                bool found;
                                HistoryDetails details = InternalGlobals.CurrentProfile.History.TryGetItem(this.Prompt.Text, CaseSensitivity.Insensitive, out found);
                                if (!found || !String.IsNullOrEmpty(details.ItemId))
                                {
                                    this.AcceptCurrentSuggestion(true, " ");
                                    this.autocorrectedLastKeyPress = true;
                                    //this.ShowSuggester();
                                    return false;
                                }
                            }
                        }
                    }

                    this.MakeInvisibleEndQuoteVisible();
                    execute = this.Prompt.Text;
                }

                ParameterHelpContext contextInfo = null;

                if (!Function.IsInFunctionSyntax(execute))
                {
                    contextInfo = this.GetContextInfoFor(execute);
                }
                
                this.ExecuteCommand(execute, mode, !shiftKeyPressed, contextInfo);
                this.ResetAll();
                return true;
            }

            private string GetSuggestionTextBlock()
            {
                int startOfSearch;
                int indexOfSuggestionBreakOff;
                return this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);
            }

            private string GetSuggestionTextBlock(out int startOfSearch, out int indexOfSuggestionBreakOff)
            {
                string text = this.Prompt.Text;
                //if (text.Length > 0)
                //{
                startOfSearch = this.Prompt.SelectionStart;

                if (text.Length > 0)
                {
                    if (startOfSearch >= text.Length)
                    {
                        startOfSearch = text.Length;
                        indexOfSuggestionBreakOff = startOfSearch;
                    }
                    else
                    {
                        indexOfSuggestionBreakOff = text.IndexOfNextBreakingChar(startOfSearch, text.Length - startOfSearch, ' ');
                        //bool indexOfSuggestionBreakOffWasNegative;
                        if (indexOfSuggestionBreakOff < 0)
                        {
                            //indexOfSuggestionBreakOffWasNegative = true;
                            indexOfSuggestionBreakOff = text.Length;
                        }
                    }
                }
                else
                {
                    indexOfSuggestionBreakOff = 0;
                }

                return text.Substring(this.beginningOfSuggestionArea, (indexOfSuggestionBreakOff) - this.beginningOfSuggestionArea);
                //}

                //startOfSearch = 0;
                //indexOfSuggestionBreakOff = 0;
                //return String.Empty;
            }

            private string SuggestionAreaAndBeyond
            {
                get { return this.Prompt.Text.Substring(this.beginningOfSuggestionArea); }
            }

            private string PromptSuggestionAreaText
            {
                get
                {
                    return this.GetSuggestionTextBlock();
                }

                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("value");
                    }

                    int startOfSearch;
                    int indexOfSuggestionBreakOff;
                    string currentValue = this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);
                    string newText = this.Prompt.Text.ReplaceSubstring(
                        value,
                        this.beginningOfSuggestionArea,
                        (indexOfSuggestionBreakOff) - this.beginningOfSuggestionArea);
                    this.Prompt.Text = newText;
                }
            }

            private bool SelectedSuggestionIs(SuggestionItemType type)
            {
                if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0)
                {
                    SuggestionItem item = this.SelectedSuggestionItem;
                    if (item != null)
                    {
                        return (item.Type & type) != 0;
                    }
                }

                return false;
            }

            private bool SelectedSuggestionIsOnly(SuggestionItemType type)
            {
                if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0)
                {
                    SuggestionItem item = this.SelectedSuggestionItem;
                    if (item != null)
                    {
                        return item.Type == type;
                    }
                }

                return false;
            }

            private void MakeInvisibleEndQuoteVisibleIfOutOfBounds(int index)
            {
                InvisibleEndQuote invisibleEndQuote = this.invisibleEndQuote;
                if (invisibleEndQuote != null)
                {
                    if (index <= invisibleEndQuote.IndexOfOpenQuote || index > invisibleEndQuote.GetIndex(this.Prompt.Text))
                    {
                        this.MakeInvisibleEndQuoteVisible();
                    }
                }
            }

            private void HandleSuggesterSelectedIndexChanged(object sender, EventArgs e)
            {
                this.ResetShowInfoBoxTimer();
            }

            private void ResetShowInfoBoxTimer()
            {
                this.HideItemInfoBox();
                this.showInfoBoxTimer.Stop();
                if (this.Suggester.SelectedIndex >= 0)
                {
                    this.showInfoBoxTimer.Start();
                }
            }

            protected override void HandleKeyPressCore(KeyPressedEventArgs e)
            {
                bool executeIfBackspaceAndExecute = this.autocorrectedLastKeyPress;
                bool forceExecute = this.forceNextExecute;
                this.forceNextExecute = false;
                this.autocorrectedLastKeyPress = false;
                Keys keyCode = (Keys)e.KeyCode;
                switch (keyCode)
                {
                    case Keys.Alt:
                    case Keys.Apps:
                    case Keys.Attn:
                    case Keys.BrowserBack:
                    case Keys.BrowserFavorites:
                    case Keys.BrowserForward:
                    case Keys.BrowserHome:
                    case Keys.BrowserRefresh:
                    case Keys.BrowserSearch:
                    case Keys.ControlKey | Keys.Control:
                    case Keys.Shift:
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                    case Keys.ShiftKey | Keys.Shift:
                    case Keys.LWin:
                    case Keys.RWin:
                        return;
                    case Keys.Escape:
                        e.Cancel = true;
                        
                        if (this.Suggester.Visible)
                        {
                            this.ResetSuggestion();
                        }
                        else if (this.Prompt.Text.Length > 0)
                        {
                            this.ResetSuggestion();
                            this.Prompt.Text = String.Empty;
                            this.HideAndDestroyMainInfoBox();
                        }
                        else
                        {
                            this.ResetSuggestion();
                            this.ClosePrompt();
                        }

                        break;
                    case Keys.Up:
                        if (this.Suggester.Visible)
                        {
                            if (this.Suggester.SelectedIndex > 0)
                            {
                                this.Suggester.SelectedIndex--;
                                this.Suggester.EnsureVisible(this.Suggester.SelectedIndex);
                            }
                        }
                        else if (this.mainInfoContextInfo != null)
                        {
                            this.OffsetMainInformationBoxIndex(1);
                        }
                        else
                        {
                            if (InternalGlobals.CurrentProfile.History.ComplexHistory.Count > 0)
                            {
                                this.promptHandler.suggestionMode = SuggestionMode.History;
                                this.ShowSuggester();
                                this.MakeSuggestion(false);
                            }
                        }

                        e.Cancel = true;
                        break;
                    case Keys.Down:
                        if (this.Suggester.Visible)
                        {
                            if (this.Suggester.SelectedIndex < this.Suggester.ItemCount - 1)
                            {
                                this.Suggester.SelectedIndex++;
                                this.Suggester.EnsureVisible(this.Suggester.SelectedIndex);
                            }
                        }
                        else if (this.mainInfoContextInfo != null)
                        {
                            this.OffsetMainInformationBoxIndex(-1);
                        }
                        else
                        {
                            if (InternalGlobals.CurrentProfile.History.ComplexHistory.Count > 0)
                            {
                                this.promptHandler.suggestionMode = SuggestionMode.History;
                                this.ShowSuggester();
                                this.MakeSuggestion(false);
                            }
                        }

                        e.Cancel = true;
                        break;
                    case Keys.Left | Keys.Shift:
                    case Keys.Right | Keys.Shift:
                    case Keys.Left | Keys.Control:
                    case Keys.Right | Keys.Control:
                    case Keys.Home:
                    case Keys.End:
                        this.MakeInvisibleEndQuoteVisible();
                        this.ResetSuggestion();
                        this.HideAndDestroyMainInfoBox();
                        break;
                    case Keys.Left:
                    case Keys.Right:
                        {
                            int currentPosition = this.Prompt.SelectionStart;
                            string currentText = this.Prompt.Text;
                            if (currentPosition >= 0 && currentPosition <= currentText.Length)
                            {
                                int startOfSearch;
                                int indexOfSuggestionBreakOff;
                                this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);

                                int newPosition = currentPosition + (keyCode == Keys.Left ? -1 : 1);

                                if (newPosition >= 0 && newPosition <= currentText.Length)
                                {
                                    this.Prompt.SelectionStart = newPosition;
                                    this.Prompt.SelectionLength = 0;
                                }

                                if (newPosition < this.beginningOfSuggestionArea || newPosition > indexOfSuggestionBreakOff)
                                {
                                    this.ResetSuggestion();
                                }
                                else
                                {
                                    
                                    //char? jumpedOver = null;

                                    int lengthOfSuggestionArea;

                                    int indexOfSuggestionArea = SuggestionUtilities.GetIndexOfSuggestionArea(
                                        this.SuggestionAreaAndBeyond,
                                        currentPosition - this.beginningOfSuggestionArea,
                                        this.mainInfoContextInfo,
                                        out lengthOfSuggestionArea);

                                    if (keyCode == Keys.Left)
                                    {
                                        if (newPosition < indexOfSuggestionArea)
                                        {
                                            this.ResetSuggestion();
                                        }

                                        //if (newPosition >= 0)
                                        //{
                                        //    //jumpedOver = currentText[newPosition];
                                        //}
                                    }
                                    else
                                    {
                                        if (newPosition > indexOfSuggestionArea + lengthOfSuggestionArea)
                                        {
                                            this.ResetSuggestion();
                                        }
                                        //if (currentPosition < currentText.Length)
                                        //{
                                        //    //jumpedOver = currentText[currentPosition];
                                        //}
                                    }

                                    //if (jumpedOver != null && SuggestionUtilities.IsBreakingChar(jumpedOver.Value)) //TODO determine if breaking char in context
                                    //{
                                    //    this.HideSuggester;
                                    //}
                                }

                                //if (keyCode == (Keys.Left | Keys.Shift))
                                //{
                                //    if (newPosition < this.Prompt.SelectionStart)
                                //    {
                                //        this.Prompt.SelectionStart--;
                                //        this.Prompt.SelectionLength++;
                                //    }
                                //    else
                                //    {
                                //        this.Prompt.SelectionLength--;
                                //    }
                                //}
                                //else if (keyCode == (Keys.Right | Keys.Shift))
                                //{
                                //    if (currentPosition < this.Prompt.SelectionStart)
                                //    {
                                //        this.Prompt.SelectionStart++;
                                //        this.Prompt.SelectionLength--;
                                //    }
                                //    else
                                //    {

                                //        this.Prompt.SelectionLength++;
                                //    }
                                //}
                                //else
                                //{

                                    
                                //}

                                this.MakeInvisibleEndQuoteVisibleIfOutOfBounds(newPosition);

                                this.UpdateFilter(this.Suggester.Visible);
                                this.ShowParameterDependingOnFilter();
                                if (this.Suggester.Visible)
                                {
                                    this.MakeSuggestion(false);
                                }

                                e.Cancel = true;
                            }
                        }

                        break;
                    default:
                        {
                            bool doNotShowSuggester = false;
                            bool skipKey = false;
                            bool mustUpdateFilter = false;
                            switch (keyCode)
                            {
                                //TODO evaluate paste for mac and linux
                                case Keys.Control | Keys.V:
                                    e.Cancel = true;

                                    string pasteText = null;

                                    if (InternalGlobals.GuiManager.ToolkitHost.Clipboard.ContainsText)
                                    {
                                        pasteText = InternalGlobals.GuiManager.ToolkitHost.Clipboard.GetText();
                                    }
                                    else if (InternalGlobals.GuiManager.ToolkitHost.Clipboard.ContainsFileDrop)
                                    {
                                        foreach (string file in InternalGlobals.GuiManager.ToolkitHost.Clipboard.GetFileDrop())
                                        {
                                            pasteText = file;
                                            break;
                                        }
                                    }
                                    
                                    if (pasteText == null)
                                    {
                                        return;
                                    }

                                    pasteText = pasteText.Replace(Environment.NewLine, String.Empty);

                                    int newSelectionStart = this.Prompt.SelectionStart + pasteText.Length;

                                    this.Prompt.Text = this.GetWhatPromptTextWillBe(pasteText);
                                    this.Prompt.SelectionStart = newSelectionStart;
                                    this.Prompt.SelectionLength = 0;

                                    mustUpdateFilter = true;
                                    skipKey = true;
                                    doNotShowSuggester = true;

                                    break;
                                case Keys.Control | Keys.A:
                                    e.Cancel = true;
                                    this.Prompt.SelectionStart = 0;
                                    this.Prompt.SelectionLength = this.Prompt.Text.Length;
                                    mustUpdateFilter = true;
                                    skipKey = true;
                                    doNotShowSuggester = true;

                                    break;
                                case Keys.PageDown:
                                    if (this.Suggester.Visible)
                                    {
                                        this.Suggester.DoPageUpOrDown(Direction.Down);
                                        e.Cancel = true;
                                    }

                                    return;
                                case Keys.PageUp:
                                    if (this.Suggester.Visible)
                                    {
                                        this.Suggester.DoPageUpOrDown(Direction.Up);
                                        e.Cancel = true;
                                    }

                                    return;
                                case Keys.Control | Keys.PageUp:
                                    if (this.Suggester.Visible)
                                    {
                                        this.Suggester.ScrollToTop();
                                    }

                                    return;
                                case Keys.Control | Keys.PageDown:
                                    if (this.Suggester.Visible)
                                    {
                                        this.Suggester.ScrollToEnd();
                                    }

                                    return;
                                case Keys.Shift | Keys.Tab:
                                case Keys.Tab:
                                    if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0)
                                    {
                                        SuggestionItem suggestionItem = this.SelectedSuggestionItem;
                                        string fullPath = this.filter + suggestionItem.Text;
                                        if (fullPath == this.PromptSuggestionAreaText)
                                        {
                                            int offset;
                                            if ((keyCode & Keys.Shift) != 0)
                                            {
                                                offset = -1;
                                            }
                                            else
                                            {
                                                offset = 1;
                                            }

                                            int newIndex = this.Suggester.SelectedIndex + offset;
                                            if (newIndex < 0)
                                            {
                                                newIndex = this.Suggester.ItemCount - 1;
                                            }
                                            else if (newIndex >= this.Suggester.ItemCount)
                                            {
                                                newIndex = 0;
                                            }

                                            this.Suggester.SelectedIndex = newIndex;
                                        }

                                        this.AcceptCurrentSuggestion(false);
                                        this.Suggester.ScrollSelectedItemIntoView();
                                        //this.Suggester.CenterSelectedItem();

                                        return;
                                    }

                                    skipKey = true;

                                    break;
                                case Keys.Space | Keys.Control:
                                    e.Cancel = true;
                                    skipKey = true;
                                    mustUpdateFilter = true;
                                    doNotShowSuggester = false;
                                    //this.MakeSuggestion(false);
                                    //return;
                                    break;
                                case Keys.Return | Keys.Shift:
                                case Keys.Return | Keys.Alt:
                                case Keys.Return | Keys.Control:
                                case Keys.Return:
                                    e.Cancel = true;
                                    bool execute = true;
                                    if (this.Suggester.Visible && this.SelectedSuggestionIsOnly(SuggestionItemType.Namespace))
                                    {
                                        this.AcceptCurrentSuggestion(true);

                                        string text = InternalGlobals.CurrentSkinInstance.Prompt.Text;
                                        execute = InternalGlobals.CurrentProfile.History.Contains(text, CaseSensitivity.Insensitive)
                                            || InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(text);
                                        if (!execute)
                                        {
                                            text = this.PromptSuggestionAreaText;
                                            text += '.';
                                            this.PromptSuggestionAreaText = text;
                                            this.filter = text;
                                            InternalGlobals.CurrentSkinInstance.Prompt.SelectionStart = this.beginningOfSuggestionArea + this.filter.Length;
                                            //this.promptHandler.suggestionIsNamespace = false;
                                            this.ShowSuggester();
                                            skipKey = true;
                                            doNotShowSuggester = true;
                                        }
                                    }

                                    if (execute)
                                    {
                                        if (this.ExecuteCurrent(e.CtrlKeyPressed, e.ShiftKeyPressed, forceExecute))
                                        {
                                            return;
                                        }

                                        doNotShowSuggester = true;
                                        mustUpdateFilter = true;
                                        skipKey = true;
                                    }

                                    break;
                                case Keys.F2:
                                    {
                                        if (this.Suggester.Visible)
                                        {
                                            e.Cancel = true;
                                            skipKey = true;

                                            this.EditSelectedItem();
                                            doNotShowSuggester = true;
                                        }
                                    }

                                    break;
                                case Keys.Delete | Keys.Shift:
                                    {
                                        if (this.Suggester.Visible)
                                        {
                                            e.Cancel = true;
                                            skipKey = true;

                                            this.DeleteSelectedItem();
                                            return;
                                        }
                                    }

                                    break;
                                case Keys.Delete:
                                    {
                                        skipKey = true;
                                        mustUpdateFilter = !this.Suggester.Visible;
                                        int currentPosition = this.Prompt.SelectionStart;
                                        if (this.SuggestionMode == SuggestionMode.Normal 
                                            && currentPosition >= 0
                                            && currentPosition <= this.Prompt.Text.Length)
                                        {
                                            int startOfSearch;
                                            int indexOfSuggestionBreakOff;
                                            this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);

                                            int lengthOfSelection = this.Prompt.SelectionLength;

                                            int newPosition = currentPosition;
                                            int indexOfEndDelete;
                                            bool reset = false;
                                            if (lengthOfSelection > 0)
                                            {
                                                indexOfEndDelete = currentPosition + lengthOfSelection;
                                                reset = currentPosition <= this.beginningOfSuggestionArea || indexOfEndDelete > indexOfSuggestionBreakOff;
                                            }
                                            else
                                            {
                                                indexOfEndDelete = currentPosition + 1;
                                                reset = currentPosition < this.beginningOfSuggestionArea || indexOfEndDelete > indexOfSuggestionBreakOff;
                                            }

                                            if (reset)
                                            {
                                                this.ResetSuggestion();
                                                return;
                                            }

                                            e.Cancel = true;

                                            string currentText = this.Prompt.Text;

                                            if (this.invisibleEndQuote != null)
                                            {
                                                if (lengthOfSelection > 0)
                                                {
                                                    if (currentPosition <= this.invisibleEndQuote.IndexOfOpenQuote && this.invisibleEndQuote.IndexOfOpenQuote < currentPosition + lengthOfSelection)
                                                    {
                                                        this.invisibleEndQuote = null;
                                                    }
                                                    else
                                                    {
                                                        this.invisibleEndQuote.NumberOfSpacesInQuote -= this.Prompt.Text.CountOf(' ', false, currentPosition, lengthOfSelection);
                                                    }
                                                }
                                                else
                                                {
                                                    if (currentPosition == this.invisibleEndQuote.IndexOfOpenQuote)
                                                    {
                                                        this.invisibleEndQuote = null;
                                                    }
                                                    else
                                                    {
                                                        if (currentText[currentPosition] == ' ' && this.invisibleEndQuote != null)
                                                        {
                                                            this.invisibleEndQuote.NumberOfSpacesInQuote--;
                                                        }
                                                    }
                                                }
                                            }

                                            string before = currentText.Substring(0, currentPosition);
                                            //if (currentPosition - 1 > 0)
                                            //{

                                            //}
                                            //else
                                            //{
                                            //    before = String.Empty;
                                            //}

                                            string after = currentText.Substring(indexOfEndDelete);
                                            this.Prompt.Text = before + after;
                                            this.Prompt.SelectionStart = currentPosition;

                                            this.promptHandler.lastSuggested = String.Empty;
                                            this.ShowParameterDependingOnFilter();
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }

                                    break;
                                case Keys.Back | Keys.Alt:
                                case Keys.Back:
                                    {
                                        skipKey = true;
                                        mustUpdateFilter = !this.Suggester.Visible;
                                        int currentPosition = this.Prompt.SelectionStart;
                                        if (this.SuggestionMode == SuggestionMode.Normal && currentPosition >= 0 && currentPosition <= this.Prompt.Text.Length)
                                        {
                                            if (executeIfBackspaceAndExecute)
                                            {
                                                this.forceNextExecute = true;
                                            }

                                            int startOfSearch;
                                            int indexOfSuggestionBreakOff;
                                            string suggestionTextBlock = this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);

                                            int lengthOfSelection = this.Prompt.SelectionLength;

                                            int newPosition;
                                            if (lengthOfSelection > 0)
                                            {
                                                newPosition = currentPosition;
                                            }
                                            else
                                            {
                                                newPosition = currentPosition - 1;
                                            }

                                            int indexOfSuggestionBreak;
                                            if (newPosition > this.beginningOfSuggestionArea && newPosition - this.beginningOfSuggestionArea < suggestionTextBlock.Length)
                                            {
                                                indexOfSuggestionBreak = suggestionTextBlock.LastIndexOfBreakingChar(newPosition - this.beginningOfSuggestionArea, false, '.', ' ', Path.DirectorySeparatorChar);
                                            }
                                            else
                                            {
                                                indexOfSuggestionBreak = -1;
                                            }

                                            if (newPosition <= this.beginningOfSuggestionArea || newPosition + lengthOfSelection > indexOfSuggestionBreakOff)
                                            {
                                                this.ResetSuggestion();
                                                this.ShowParameterDependingOnFilter();
                                                return;
                                            }

                                            string currentText = this.Prompt.Text;

                                            if (this.invisibleEndQuote != null)
                                            {
                                                if (lengthOfSelection > 0)
                                                {
                                                    if (currentPosition <= this.invisibleEndQuote.IndexOfOpenQuote && this.invisibleEndQuote.IndexOfOpenQuote < currentPosition + lengthOfSelection)
                                                    {
                                                        this.invisibleEndQuote = null;
                                                    }
                                                    else
                                                    {
                                                        this.invisibleEndQuote.NumberOfSpacesInQuote -= this.Prompt.Text.CountOf(' ', false, currentPosition, lengthOfSelection);
                                                    }
                                                }
                                                else
                                                {
                                                    if (currentPosition - 1 == this.invisibleEndQuote.IndexOfOpenQuote)
                                                    {
                                                        this.invisibleEndQuote = null;
                                                    }
                                                    else
                                                    {
                                                        if (currentText[currentPosition - 1] == ' ' && this.invisibleEndQuote != null)
                                                        {
                                                            this.invisibleEndQuote.NumberOfSpacesInQuote--;
                                                        }
                                                    }
                                                }
                                            }

                                            e.Cancel = true;

                                            char? deleting = null;
                                            if (lengthOfSelection == 0)
                                            {
                                                deleting = currentText[currentPosition - 1];

                                                if (deleting == '\\'
                                                    && this.mainInfoContextInfo != null
                                                    && this.mainInfoContextInfo.CurrentParameterAllowsFileSystem
                                                    && currentText.CountOf('"', true) % 2 != 0
                                                    && Path.DirectorySeparatorChar == deleting
                                                    && currentText.CountReverseCharRun('\\', currentPosition - 1) % 2 == 0)
                                                {
                                                    newPosition--;
                                                }
                                            }

                                            string before = currentText.Substring(0, newPosition);
                                            string after = currentText.Substring(currentPosition + lengthOfSelection);
                                            this.Prompt.Text = before + after;
                                            this.Prompt.SelectionStart = newPosition;

                                            this.promptHandler.lastSuggested = String.Empty;

                                            if ((deleting != null && currentPosition > 0 && SuggestionUtilities.IsBreakingChar(deleting.Value))
                                                || (indexOfSuggestionBreak > -1 && indexOfSuggestionBreak + this.beginningOfSuggestionArea >= newPosition))
                                            {
                                                this.ResetSuggestion();
                                                mustUpdateFilter = true;
                                                this.populationProvider.ResetFilterAtLastPopulation();
                                            }

                                            //if (this.addQuoteWithSpace && this.previousQuoteIndex >= 0)
                                            //{
                                            //    if (newPosition < this.previousQuoteIndex)
                                            //    {
                                            //        this.addQuoteWithSpace = false;
                                            //    }
                                            //}
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }

                                    break;
                                case Keys.Back | Keys.Control:
                                case Keys.Back | Keys.Shift:
                                    {
                                        e.Cancel = true;
                                        if (this.promptHandler.suggestionMode == SuggestionMode.Normal)
                                        {
                                            bool reset = true;
                                            string text = this.PromptSuggestionAreaText;
                                            if (Utilities.LooksLikeValidPath(text))
                                            {
                                                int indexOfLastDirectorySeparator = text.LastIndexOf(Path.DirectorySeparatorChar);

                                                if (indexOfLastDirectorySeparator > -1)
                                                {
                                                    reset = false;
                                                    int removeTo = text.Length;
                                                    if (indexOfLastDirectorySeparator == text.Length - 1 && text.Length > 0)
                                                    {
                                                        int indexOfNextToLastDirectorySeparator = text.LastIndexOf(Path.DirectorySeparatorChar, text.Length - 2, text.Length - 1);
                                                        if (indexOfNextToLastDirectorySeparator == -1)
                                                        {
                                                            reset = true;
                                                        }
                                                        else
                                                        {
                                                            removeTo = indexOfNextToLastDirectorySeparator;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        removeTo = indexOfLastDirectorySeparator;
                                                    }

                                                    if (!reset)
                                                    {

                                                        reset = text.IndexOf(Path.DirectorySeparatorChar, 0, removeTo + 1) < 0;

                                                        if (!reset)
                                                        {
                                                            this.ResetSuggestion();
                                                            text = text.Substring(0, removeTo + 1);
                                                            this.PromptSuggestionAreaText = text;
                                                            this.Prompt.SelectionStart = this.beginningOfSuggestionArea + text.Length;
                                                            this.filter = text;
                                                            this.ShowSuggester();
                                                            skipKey = true;
                                                            doNotShowSuggester = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                reset = false;
                                                string realText = this.Prompt.Text;
                                                string fullText = this.PromptSuggestionAreaText;
                                                int length;
                                                int index = SuggestionUtilities.GetIndexOfBlockToRemove(fullText, this.IndexInSuggestionArea, this.mainInfoContextInfo, out length);
                                                this.PromptSuggestionAreaText = String.Format(
                                                    CultureInfo.CurrentCulture, 
                                                    "{0}{1}",
                                                    fullText.Substring(0, index),
                                                    fullText.Substring(index + length));
                                                this.Prompt.SelectionStart = this.beginningOfSuggestionArea + index;
                                                this.UpdateFilter();
                                                this.ShowSuggester();
                                                skipKey = true;
                                                doNotShowSuggester = true;

                                                if (this.invisibleEndQuote != null)
                                                {
                                                    int realIndex = this.beginningOfSuggestionArea + index;
                                                    if (realIndex > this.invisibleEndQuote.IndexOfOpenQuote)
                                                    {
                                                        int supposedIndex = this.invisibleEndQuote.GetIndex(text);
                                                        int lengthToExamine = length;
                                                        if (supposedIndex < realIndex + length)
                                                        {
                                                            lengthToExamine = supposedIndex - realIndex;
                                                        }

                                                        this.invisibleEndQuote.NumberOfSpacesInQuote -= realText.CountOf(' ', false, realIndex, lengthToExamine);
                                                    }
                                                    else
                                                    {
                                                        this.invisibleEndQuote = null;
                                                    }
                                                }

                                                //int indexOfLastBreakingChar = text.LastIndexOfBreakingChar(false, spaceAndDotChars);

                                                //if (indexOfLastBreakingChar > -1)
                                                //{
                                                //    reset = false;
                                                //    int removeTo = text.Length;
                                                //    if (indexOfLastBreakingChar == text.Length - 1 && text.Length > 0)
                                                //    {
                                                //        int indexOfNextToLastBreakingChar = text.LastIndexOfBreakingChar(text.Length - 2, false, spaceAndDotChars);
                                                //        if (indexOfNextToLastBreakingChar == -1)
                                                //        {
                                                //            reset = true;
                                                //        }
                                                //        else
                                                //        {
                                                //            removeTo = indexOfNextToLastBreakingChar;
                                                //        }
                                                //    }
                                                //    else
                                                //    {
                                                //        removeTo = indexOfLastBreakingChar;
                                                //    }

                                                //    if (!reset)
                                                //    {

                                                //        reset = text.IndexOfNextBreakingChar(0, removeTo + 1, false, spaceAndDotChars) < 0;

                                                //        if (!reset)
                                                //        {
                                                //            //int deltaRemoveTo;
                                                //            //if (text[removeTo] == ' ')
                                                //            //{
                                                //            //    deltaRemoveTo = 2;
                                                //            //}
                                                //            //else
                                                //            //{
                                                //            //    deltaRemoveTo = 1;
                                                //            //}

                                                //            this.ResetSuggestion();
                                                //            text = text.Substring(0, removeTo + 1);
                                                //            this.promptHandler.prompt.TextualInput.InputText = text;
                                                //            this.promptHandler.prompt.TextualInput.StartOfSelection = text.Length;
                                                //            this.filter = text;
                                                //            this.ShowSuggester();
                                                //            skipKey = true;
                                                //            doNotShowSuggester = true;
                                                //        }
                                                //    }
                                                //}
                                            }

                                            if (reset)
                                            {
                                                this.ResetSuggestion();
                                                this.PromptSuggestionAreaText = String.Empty;
                                                //this.filter = String.Empty;
                                                this.ShowSuggester();
                                                skipKey = true;
                                                doNotShowSuggester = true;
                                            }
                                        }
                                    }

                                    break;
                                default:
                                    break;
                            }

                            //string oldSuggestionTextBlock = this.GetSuggestionTextBlock();
                            //int startOfSearch;
                            //int indexOfSuggestionBreakOff;
                            //string suggestionTextBlock = null this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);
                            ////bool showSuggestion = true;
                            //if (keyCode == PromptuSettings.CurrentProfile.Hotkey.Key)
                            //{
                            //    if (e.AltKeyPressed && (HotkeyModifierKeys.Alt & PromptuSettings.CurrentProfile.Hotkey.ModifierKeys) != 0)
                            //    {
                            //        showSuggestion = false;
                            //    }
                            //    else if (e.CtrlKeyPressed && (HotkeyModifierKeys.Ctrl & PromptuSettings.CurrentProfile.Hotkey.ModifierKeys) != 0)
                            //    {
                            //        showSuggestion = false;
                            //    }
                            //    else if (e.ShiftKeyPressed && (HotkeyModifierKeys.Shift & PromptuSettings.CurrentProfile.Hotkey.ModifierKeys) != 0)
                            //    {
                            //        showSuggestion = false;
                            //    }
                            //    else if (Keyboard.WinKeyPressed && (HotkeyModifierKeys.Win & PromptuSettings.CurrentProfile.Hotkey.ModifierKeys) != 0)
                            //    {
                            //        showSuggestion = false;
                            //    }
                            //}q

                            //if (showSuggestion)
                            //{
                            if (!skipKey)
                            {
                                char key;

                                try
                                {
                                    key = InternalGlobals.GuiManager.ToolkitHost.Keyboard.ConvertToChar(keyCode);
                                }
                                catch (ArgumentException)
                                {
                                    return;
                                }

                                e.Cancel = true;

                                //if (this.promptHandler.lastSuggested.Length <= 0 || !this.Suggester.Visible)
                                //{
                                //    this.promptHandler.DetermineIfValueCoundBePath(this.PromptSuggestionAreaText);
                                //}

                                //string currentText = this.promptHandler.prompt.TextualInput.InputText;
                                //bool isForFunction;
                                bool doDefaultForKey = false;
                                bool allowDefaultForKeyToForceFilterUpdate = true;
                                if (this.promptHandler.suggestionMode == SuggestionMode.Normal)
                                {
                                    if (keyCode == Keys.Space)
                                    {
                                        bool returnNow = true;
                                        e.Cancel = false;

                                        if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0)
                                        {
                                            if (!SuggestionUtilities.IsFilepath(this.Prompt.Text) && !this.FilterIsInFunctionSyntax && (!this.FilterIsInQuote || this.invisibleEndQuote != null))
                                            {
                                                e.Cancel = InternalGlobals.CurrentProfile.SuggestionConfig.Spacebar.EatCharacter;

                                                if (InternalGlobals.CurrentProfile.SuggestionConfig.Spacebar.AcceptsSuggestion)
                                                {
                                                    returnNow = false;
                                                    e.Cancel = true;

                                                    string addToSuggestion;
                                                    if (this.SelectedSuggestionIs(SuggestionItemType.Folder))
                                                    {
                                                        addToSuggestion = PathSeparatorString;
                                                    }
                                                    else
                                                    {
                                                        addToSuggestion = String.Empty;
                                                    }

                                                    this.AcceptCurrentSuggestion(true, addToSuggestion);
                                                    this.MakeInvisibleEndQuoteVisible();
                                                    string newText = this.PromptSuggestionAreaText + " ";
                                                    this.PromptSuggestionAreaText = newText;
                                                    this.filter = newText;
                                                    this.Prompt.SelectionStart = this.beginningOfSuggestionArea + this.filter.Length;
                                                    //this.promptHandler.suggestionIsNamespace = false;
                                                    //this.RepopulateSuggester(true);
                                                    //this.HideSuggester();
                                                    this.ShowParameterDependingOnFilter();
                                                    //doNotShowSuggester = true;
                                                    doNotShowSuggester = true;

                                                    //this.ShowSuggester();
                                                    //if (this.Suggester.ItemCount == 0)
                                                    //{
                                                    //    returnNow = true;
                                                    //    this.ResetSuggestion(false);
                                                    //}
                                                }
                                            }
                                        }
                                        else
                                        {
                                            returnNow = false;
                                            e.Cancel = true;
                                            this.MakeInvisibleEndQuoteVisible();

                                            int oldSelectionStart = this.Prompt.SelectionStart;

                                            this.Prompt.Text = this.GetWhatPromptTextWillBe(key);
                                            this.Prompt.SelectionStart = oldSelectionStart + 1;
                                            //string newText = this.PromptSuggestionAreaText + " ";
                                            //if (this.addQuoteWithSpace)
                                            //{
                                            //    newText += "\" ";
                                            //    this.addQuoteWithSpace = false;
                                            //}
                                            //else
                                            //{
                                            //    newText += " ";
                                            //}

                                            //this.PromptSuggestionAreaText = newText;
                                            //this.filter = newText;
                                            //this.promptHandler.prompt.TextualInput.StartOfSelection = this.beginingOfSuggestionArea + this.filter.Length;
                                            this.UpdateFilter();
                                            this.ShowParameterDependingOnFilter();
                                        }

                                        if (returnNow)
                                        {
                                            return;
                                        }
                                    }
                                    else if (((key == '(' || key == ',') && this.SelectedSuggestionIs(SuggestionItemType.Function))
                                        || (key == '.' && this.SelectedSuggestionIs(SuggestionItemType.Namespace)))
                                    {
                                        this.AcceptCurrentSuggestion(true, key);
                                        //string newText = this.PromptSuggestionAreaText + key;
                                        //this.PromptSuggestionAreaText = newText;
                                        //this.filter = newText;
                                        //this.promptHandler.prompt.TextualInput.StartOfSelection = this.beginingOfSuggestionArea + newText.Length;
                                        //this.promptHandler.suggestionIsNamespace = false;

                                        // this.RepopulateSuggester(true);
                                        
                                        this.UpdateFilter();
                                        //this.ShowMainInformationBoxDependingOnFilter();
                                        if (key != '(')
                                        {
                                            this.ShowSuggester();
                                        }
                                        // return;
                                    }
                                    else if (key == ',')
                                    {
                                        doDefaultForKey = true;
                                        mustUpdateFilter = true;
                                    }
                                    else if (key == '"')
                                    {
                                        doDefaultForKey = true;
                                        mustUpdateFilter = true;
                                        if (this.invisibleEndQuote != null)
                                        {
                                            int startOfSelection = this.Prompt.SelectionStart;
                                            if (startOfSelection > 0 && this.Prompt.Text.CountReverseCharRun('\\', startOfSelection - 1) % 2 == 0)
                                            {
                                                this.invisibleEndQuote = null;
                                            }
                                        }

                                        if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0 && this.filter.CountOf('"', true) % 2 != 0)
                                        {
                                            int startOfSelection = this.Prompt.SelectionStart;
                                            if (startOfSelection > 0 && this.Prompt.Text.CountReverseCharRun('\\', startOfSelection - 1) % 2 == 0)
                                            {
                                                SuggestionItem item = this.SelectedSuggestionItem;
                                                if (item != null &&
                                                    ((item.Type & SuggestionItemType.Command) != 0
                                                    || (item.Type & SuggestionItemType.File) != 0
                                                    || (item.Type & SuggestionItemType.Folder) != 0
                                                    || (item.Type & SuggestionItemType.ValueListItem) != 0
                                                    || (item.Type & SuggestionItemType.History) != 0))
                                                {
                                                    this.AcceptCurrentSuggestion(true, "\"");
                                                    doDefaultForKey = false;
                                                    doNotShowSuggester = true;
                                                    this.UpdateFilter(false);
                                                }
                                            }
                                        }
                                    }
                                    //    bool escaped = this.Prompt.SelectionStart > 0
                                    //        && (this.Prompt.Text.CountReverseCharRun('\\', this.Prompt.SelectionStart - 1) % 2) > 0;

                                    //    if (!escaped)
                                    //    {
                                    //        this.AcceptCurrentSuggestion(true);
                                    //        string newText = this.PromptSuggestionAreaText + key;
                                    //        this.PromptSuggestionAreaText = newText;
                                    //        this.filter = newText;
                                    //        this.promptHandler.prompt.TextualInput.StartOfSelection = this.beginingOfSuggestionArea + this.filter.Length;
                                    //        this.promptHandler.suggestionIsNamespace = false;

                                    //        // this.RepopulateSuggester(true);
                                    //        mustUpdateFilter = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        doDefaultForKey = true;
                                    //    }
                                    //}
                                    else if (key == Path.DirectorySeparatorChar
                                        && (this.SelectedSuggestionIs(SuggestionItemType.Function)
                                        || this.SelectedSuggestionIs(SuggestionItemType.Command)
                                        || this.SelectedSuggestionIs(SuggestionItemType.Folder) || !this.Suggester.Visible))
                                    {
                                        e.Cancel = true;

                                        if (!this.Suggester.Visible)
                                        {
                                            this.UpdateFilter(false);
                                        }

                                        //bool justSuggestionArea = true;

                                        string whatWouldBeText;
                                        if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0)
                                        {
                                            string addToSuggestion = String.Empty;

                                            string currentItemText = this.SelectedSuggestionItem.Text;
                                            //whatWouldBeText = this.filter + this.SelectedSuggestionItem.Text;
                                            bool endsWithSeparatorChar = currentItemText.EndsWith(PathSeparatorString);
                                            if (!endsWithSeparatorChar)
                                            {
                                                addToSuggestion += Path.DirectorySeparatorChar;
                                            }

                                            if ((FilterIsInQuote
                                            && Path.DirectorySeparatorChar == '\\'
                                            && this.mainInfoContextInfo != null
                                            && this.mainInfoContextInfo.CurrentParameterAllowsFileSystem)
                                            || this.filter.Contains(" ") && currentItemText.Contains(" "))
                                            {
                                                addToSuggestion += Path.DirectorySeparatorChar;
                                            }

                                            if (this.Suggester.Visible && this.Suggester.SelectedIndex >= 0)
                                            {
                                                this.AcceptCurrentSuggestion(true, addToSuggestion);
                                            }
                                            else
                                            {
                                                this.ResetSuggestion();
                                            }

                                            //this.PromptSuggestionAreaText = whatWouldBeText;
                                            //this.promptHandler.prompt.TextualInput.StartOfSelection = this.beginingOfSuggestionArea + whatWouldBeText.Length;

                                            mustUpdateFilter = true;
                                            //this.ShowSuggester();

                                            //if (whatWouldBeText.Length > 1)
                                            //{
                                            //    this.filter = whatWouldBeText;
                                            //    //this.PopulateSuggester();
                                            //    this.ShowSuggester();
                                            //    doNotShowSuggester = true;
                                            //}

                                            //this.CloseSuggesterIfNoItems();
                                        }
                                        else
                                        {
                                            //e.Cancel = false;
                                            //doDefaultForKey = true;
                                            //justSuggestionArea = false;
                                            string valueToAdd = new string(Path.DirectorySeparatorChar, 1);

                                            //if (FilterIsInQuote
                                            //    && Path.DirectorySeparatorChar == '\\'
                                            //    && this.mainInfoContextInfo != null
                                            //    && this.mainInfoContextInfo.CurrentParameterAllowsFileSystem)
                                            //{
                                            //    valueToAdd += Path.DirectorySeparatorChar;
                                            //}

                                            whatWouldBeText = this.GetWhatPromptTextWillBe(key);

                                            int currentStartOfSelection = this.Prompt.SelectionStart;
                                            this.Prompt.Text = whatWouldBeText;
                                            this.Prompt.SelectionStart = currentStartOfSelection + valueToAdd.Length;
                                            mustUpdateFilter = true;
                                        }

                                        //this.promptHandler.DetermineIfValueCoundBePath(whatWouldBeText);

                                        //if (this.promptHandler.couldBePath)


                                        //else
                                        //{
                                        //    this.filter = whatWouldBeText;
                                        //}

                                        //if (this.suggestionItemsAndIndexes.Count > 0)
                                        //{

                                        //    this.ShowSuggester();
                                        //}
                                        //}
                                    }
                                    else if (key == ')' && Function.IsInFunctionSyntax(this.filter))
                                    {
                                        doDefaultForKey = true;
                                        doNotShowSuggester = true;
                                        allowDefaultForKeyToForceFilterUpdate = false;
                                        this.UpdateFilter(false);
                                    }
                                    else
                                    {
                                        doDefaultForKey = true;
                                    }
                                }
                                else
                                {
                                    doDefaultForKey = true;
                                }
                                
                                if (doDefaultForKey)
                                {
                                    int oldSelectionStart = this.Prompt.SelectionStart;

                                    this.Prompt.Text = this.GetWhatPromptTextWillBe(key);
                                    this.Prompt.SelectionStart = oldSelectionStart + 1;

                                    //if (!this.promptHandler.suggestionIsPath)
                                    //{
                                    if (allowDefaultForKeyToForceFilterUpdate)
                                    {
                                        mustUpdateFilter = true;
                                    }

                                    //int lookForIndex;// = this.beginingOfSuggestionArea;
                                    //if (
                                    //int startOfSearch = this.promptHandler.prompt.TextualInput.StartOfSelection - 1;
                                    //if (startOfSearch < 0)
                                    //{
                                    //    startOfSearch = 0;
                                    //}

                                    //int indexOfSuggestionBreakOff = text.IndexOfNextBreakingChar(startOfSearch, ' ');
                                    ////bool indexOfSuggestionBreakOffWasNegative;
                                    //if (indexOfSuggestionBreakOff < 0)
                                    //{
                                    //    //indexOfSuggestionBreakOffWasNegative = true;
                                    //    indexOfSuggestionBreakOff = text.Length - 1;
                                    //}
                                    //else
                                    //{
                                    //    //indexOfSuggestionBreakOffWasNegative = false;
                                    //}



                                    //}
                                }
                            }

                            if (mustUpdateFilter)
                            {
                                this.UpdateFilter(!doNotShowSuggester);
                            }
                            //}

                            //int firstIndexOfSpaceChar = this.promptHandler.prompt.TextualInput.InputText.IndexOf(' ');

                            //if (suggestionTextBlock == null)
                            //{
                            //    int startOfSearch;
                            //    int indexOfSuggestionBreakOff;
                            //    suggestionTextBlock = this.GetSuggestionTextBlock(out startOfSearch, out indexOfSuggestionBreakOff);
                            //}

                            this.MakeSuggestion(doNotShowSuggester);

                            //string textAtSuggestion = this.PromptSuggestionAreaText;

                            //string suggestion = null;

                            //suggestion = this.promptHandler.Suggest(textAtSuggestion);

                            //if (!doNotShowSuggester)
                            //{
                            //    this.HideInformationBoxes();
                            //}

                            //if (suggestion == null)
                            //{
                            //    this.Suggester.SelectedIndex = -1;

                            //    if (!this.Suggester.Visible && !doNotShowSuggester)
                            //    {
                            //        this.ShowSuggester();
                            //    }
                            //}
                            //else
                            //{
                            //    if (suggestion.ToUpperInvariant() == this.filter.ToUpperInvariantNullSafe())
                            //    {
                            //        this.Suggester.SelectedIndex = -1;

                            //        if (!this.Suggester.Visible && !doNotShowSuggester)
                            //        {
                            //            this.ShowSuggester();
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (this.promptHandler.suggestionIsPath)
                            //        {
                            //            string upToCaret = textAtSuggestion.Substring(this.beginingOfSuggestionArea, this.promptHandler.prompt.TextualInput.StartOfSelection - this.beginingOfSuggestionArea);
                            //            int splitIndex = upToCaret.OccurenceCountOf(Path.DirectorySeparatorChar);
                            //            if (upToCaret.EndsWith(PathSeparatorString))
                            //            {
                            //                splitIndex--;
                            //            }

                            //            string[] split = suggestion.Split(PathSeparatorCharArray);
                            //            suggestion = split[splitIndex];
                            //            if (splitIndex == 0)
                            //            {
                            //                if (this.filter.Length == 0 && !this.promptHandler.suggestionIsCommandBasedPath && !suggestion.EndsWith(PathSeparatorString))
                            //                {
                            //                    suggestion += Path.DirectorySeparatorChar;
                            //                }
                            //                else
                            //                {
                            //                    int namespaceSplitIndex = textAtSuggestion.CountBreaks('.');
                            //                    //if (splitIndex >= 0)
                            //                    //{
                            //                    //int splitIndex = this.promptHandler.prompt.TextualInput.InputText.OccurenceCountOf(' ');
                            //                    string[] namespaceSplit = suggestion.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');//suggestion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            //                    if (namespaceSplit.Length == 0)
                            //                    {
                            //                        suggestion = String.Empty;
                            //                    }
                            //                    else
                            //                    {
                            //                        suggestion = namespaceSplit[namespaceSplitIndex];
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            int splitIndex = textAtSuggestion.CountBreaks('.');
                            //            //if (splitIndex >= 0)
                            //            //{
                            //            //int splitIndex = this.promptHandler.prompt.TextualInput.InputText.OccurenceCountOf(' ');
                            //            string[] split = suggestion.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');//suggestion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            //            suggestion = split[splitIndex];
                            //            //}
                            //        }

                            //        if (this.Suggester.SelectedIndex != this.lastSuggestedIndex || this.promptHandler.lastSuggested != suggestion)
                            //        {
                            //            if (!this.Suggester.Visible && !doNotShowSuggester)
                            //            {
                            //                this.ShowSuggester();
                            //            }

                            //            //string lookup;

                            //            //if (this.promptHandler.couldBePath)
                            //            //{
                            //            //    lookup = suggestion;
                            //            //}
                            //            //else
                            //            //{
                            //            //    lookup = suggestion.Substring(suggestion.LastIndexOfAny(spaceAndDotChars) + 1);
                            //            //}

                            //            bool found;
                            //            Int32Encapsulator index = this.suggestionItemsAndIndexes.TryGetItem(suggestion, CaseSensitivity.Insensitive, out found);
                            //            if (!found || index == null)
                            //            {
                            //                this.Suggester.SelectedIndex = -1;
                            //            }
                            //            else
                            //            {
                            //                this.promptHandler.lastSuggested = suggestion;
                            //                this.Suggester.SelectedIndex = index;
                            //            }

                            //            this.lastSuggestedIndex = this.Suggester.SelectedIndex;
                            //        }

                            //        this.Suggester.CenterSelectedItem();
                            //    }
                            //}
                        }

                        break;
                }
            }

            private bool FilterIsInFunctionSyntax
            {
                get { return Function.IsInFunctionSyntax(this.filter); }
            }

            private bool FilterIsInQuote
            {
                get { return this.filter.CountOf('"', true) % 2 != 0; }
            }

            private string GetWhatFilterShouldBe()
            {
                string whatShouldBeFilter;
                if (this.promptHandler.suggestionMode == SuggestionMode.Normal)
                {
                    whatShouldBeFilter = SuggestionUtilities.GetWhatFilterShouldBe(this.PromptSuggestionAreaText, this.IndexInSuggestionArea, this.mainInfoContextInfo);

                    if (this.mainInfoContextInfo == null && whatShouldBeFilter.CountOf('"', true) % 2 != 0)
                    {
                        this.ShowParameterDependingOnFilter();
                        whatShouldBeFilter = SuggestionUtilities.GetWhatFilterShouldBe(this.PromptSuggestionAreaText, this.IndexInSuggestionArea, this.mainInfoContextInfo);
                    }//string text = this.PromptSuggestionAreaText;
                    //int indexOfFilterBreakChar = this.beginingOfSuggestionArea;

                    //int startOfSelection = this.promptHandler.prompt.TextualInput.StartOfSelection - this.beginingOfSuggestionArea;
                    //int lastIndexOfDirectorySeparator = text.LastIndexOf(Path.DirectorySeparatorChar, startOfSelection - 1);
                    //int lengthToSearch = startOfSelection - this.beginingOfSuggestionArea;
                    //if (lengthToSearch > 0 && startOfSelection > 0)
                    //{
                    //    int lastIndexOfBreakingChar = text.LastIndexOfBreakingChar(false, '.', ' ', '(');
                    //    if (lastIndexOfBreakingChar >= 0)
                    //    {
                    //        indexOfFilterBreakChar = lastIndexOfBreakingChar;
                    //    }
                    //}

                    //int firstIndexOfDirectorySeparator = text.IndexOf(Path.DirectorySeparatorChar);

                    //if (lastIndexOfDirectorySeparator >= 0 && Utilities.IsValidPath(text))
                    //{
                    //    whatShouldBeFilter = text.Substring(0, lastIndexOfDirectorySeparator + 1);
                    //}
                    //else
                    //{
                    //    if (indexOfFilterBreakChar > this.beginingOfSuggestionArea)
                    //    {
                    //        whatShouldBeFilter = this.promptHandler.prompt.TextualInput.InputText.Substring(this.beginingOfSuggestionArea, (indexOfFilterBreakChar + 1) - this.beginingOfSuggestionArea);
                    //    }
                    //    else
                    //    {
                    //        whatShouldBeFilter = String.Empty;
                    //    }
                    //}
                }
                else
                {
                    whatShouldBeFilter = String.Empty;
                }

                return whatShouldBeFilter;
            }

            private void UpdateFilter()
            {
                this.UpdateFilter(true);
            }

            private void UpdateFilter(bool showSuggesterIfHidden)
            {
                this.beginningOfSuggestionArea = SuggestionUtilities.GetStartOfCurrentLevel(this.Prompt.Text, this.Prompt.SelectionStart);
                string whatShouldBeFilter = this.GetWhatFilterShouldBe();

                if (whatShouldBeFilter != this.filter)
                {
                    this.filter = whatShouldBeFilter;
                    this.ShowParameterDependingOnFilter();
                    if (showSuggesterIfHidden && !this.Suggester.Visible)
                    {
                        this.ShowSuggester();
                    }
                    else
                    {
                        this.populationProvider.PopulateSuggester(this.Suggester, this.filter, true, false, this.mainInfoContextInfo);
                        if (this.Suggester.ItemCount <= 0)
                        {
                            this.HideSuggester("updating filter, 0 items in suggester");
                        }
                    }
                }
                else
                {
                    this.ShowParameterDependingOnFilter();
                }
            }

            private string GetCurrentSuggestionAreaText()
            {
                string text = this.PromptSuggestionAreaText;
                int indexOfFilterBreakChar = this.beginningOfSuggestionArea;
                int startOfSelection = this.Prompt.SelectionStart - this.beginningOfSuggestionArea;
                int lastIndexOfDirectorySeparator = text.LastIndexOf(Path.DirectorySeparatorChar, startOfSelection - 1);
                int lengthToSearch = startOfSelection - this.beginningOfSuggestionArea;
                if (lengthToSearch > 0 && startOfSelection > 0)
                {
                    int lastIndexOfBreakingChar = text.LastIndexOfBreakingChar(false, '.', ' ', '(');
                    if (lastIndexOfBreakingChar >= 0)
                    {
                        indexOfFilterBreakChar = lastIndexOfBreakingChar;
                    }
                }

                int firstIndexOfDirectorySeparator = text.IndexOf(Path.DirectorySeparatorChar);
                string suggestionAreaText;

                if (lastIndexOfDirectorySeparator >= 0 && Utilities.LooksLikeValidPath(text))
                {
                    suggestionAreaText = text.Substring(lastIndexOfDirectorySeparator + 1);
                }
                else
                {
                    if (indexOfFilterBreakChar > this.beginningOfSuggestionArea)
                    {
                        suggestionAreaText = InternalGlobals.CurrentSkinInstance.Prompt.Text.Substring((indexOfFilterBreakChar + 1) - this.beginningOfSuggestionArea);
                    }
                    else
                    {
                        suggestionAreaText = text;
                    }
                }

                return suggestionAreaText;
            }

            //private void CloseSuggesterIfNoItems()
            //{
            //    if (this.Suggester.ItemCount <= 0)
            //    {
            //        this.HideSuggester();
            //    }
            //}

            private int IndexInSuggestionArea
            {
                get { return this.Prompt.SelectionStart - this.beginningOfSuggestionArea; }
            }

            public int BeginningOfSuggestionArea
            {
                get { return this.beginningOfSuggestionArea; }
            }

            private void MakeSuggestion(bool doNotShowSuggester)
            {
                string textAtSuggestion = this.PromptSuggestionAreaText;

                if (!this.Suggester.Visible && !doNotShowSuggester)
                {
                    this.ShowSuggester();
                }

                int index = this.suggestionProvider.SuggestIndex(textAtSuggestion, this.IndexInSuggestionArea, this.mainInfoContextInfo);

                if (this.Suggester.SelectedIndex != index)
                {
                    if (index < 0 || index >= this.Suggester.ItemCount)
                    {
                        this.showInfoBoxTimer.Stop();
                        this.Suggester.SelectedIndex = -1;
                    }
                    else
                    {
                        this.Suggester.SelectedIndex = index;
                    }

                    this.lastSuggestedIndex = index;

                    this.Suggester.CenterSelectedItem();
                }

                //string suggestion = null;

                //MainInfoContextInfo mainContextInfo = this.mainInfoContextInfo;
                //Function function;

                //if (mainInfoContextInfo != null && (function = mainInfoContextInfo.GetCurrentItem() as Function) != null)
                //{
                //    suggestion = this.functionSuggestionProvider.GetSuggestion(textAtSuggestion, mainInfoContextInfo.ParameterIndex, function);
                //}
                //else
                //{
                //    suggestion = this.promptHandler.Suggest(textAtSuggestion);
                //}

                //if (!doNotShowSuggester)
                //{
                //    this.informationBoxManager.HideAndDestroyAllExceptMain();
                //}

                //if (suggestion == null)
                //{
                //    this.Suggester.SelectedIndex = -1;

                //    if (!this.Suggester.Visible && !doNotShowSuggester)
                //    {
                //        this.ShowSuggester();
                //    }
                //}
                //else
                //{
                //    if (suggestion.ToUpperInvariant() == this.filter.ToUpperInvariantNullSafe())
                //    {
                //        this.Suggester.SelectedIndex = -1;

                //        if (!this.Suggester.Visible && !doNotShowSuggester)
                //        {
                //            this.ShowSuggester();
                //        }
                //    }
                //    else
                //    {
                //        //if (this.promptHandler.suggestionMode == SuggestionMode.Normal)
                //        //{
                //        //    if (this.promptHandler.suggestionIsPath)
                //        //    {
                //        //        string upToCaret = textAtSuggestion.Substring(this.beginingOfSuggestionArea, this.promptHandler.prompt.TextualInput.StartOfSelection - this.beginingOfSuggestionArea);
                //        //        int splitIndex = upToCaret.OccurenceCountOf(Path.DirectorySeparatorChar);
                //        //        if (upToCaret.EndsWith(PathSeparatorString))
                //        //        {
                //        //            splitIndex--;
                //        //        }

                //        //        string[] split = suggestion.Split(PathSeparatorCharArray);
                //        //        suggestion = split[splitIndex];
                //        //        if (splitIndex == 0)
                //        //        {
                //        //            if (this.filter.Length == 0 && !this.promptHandler.suggestionIsCommandBasedPath && !suggestion.EndsWith(PathSeparatorString))
                //        //            {
                //        //                suggestion += Path.DirectorySeparatorChar;
                //        //            }
                //        //            else
                //        //            {
                //        //                int namespaceSplitIndex = textAtSuggestion.CountBreaks('.');
                //        //                //if (splitIndex >= 0)
                //        //                //{
                //        //                //int splitIndex = this.promptHandler.prompt.TextualInput.InputText.OccurenceCountOf(' ');
                //        //                string[] namespaceSplit = suggestion.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');//suggestion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //        //                if (namespaceSplit.Length == 0)
                //        //                {
                //        //                    suggestion = String.Empty;
                //        //                }
                //        //                else
                //        //                {
                //        //                    suggestion = namespaceSplit[namespaceSplitIndex];
                //        //                }
                //        //            }
                //        //        }
                //        //    }
                //        //    else
                //        //    {
                //        //        int splitIndex = textAtSuggestion.CountBreaks('.');
                //        //        //if (splitIndex >= 0)
                //        //        //{
                //        //        //int splitIndex = this.promptHandler.prompt.TextualInput.InputText.OccurenceCountOf(' ');
                //        //        string[] split = suggestion.BreakApart(Quotes.Include, Spaces.Break, BreakingCharAction.Eat, '.');//suggestion.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //        //        suggestion = split[splitIndex];
                //        //        //}
                //        //    }
                //        //}

                //        if (this.Suggester.SelectedIndex != this.lastSuggestedIndex || this.promptHandler.lastSuggested != suggestion)
                //        {
                //            if (!this.Suggester.Visible && !doNotShowSuggester)
                //            {
                //                this.ShowSuggester();
                //            }

                //            //string lookup;

                //            //if (this.promptHandler.couldBePath)
                //            //{
                //            //    lookup = suggestion;
                //            //}
                //            //else
                //            //{
                //            //    lookup = suggestion.Substring(suggestion.LastIndexOfAny(spaceAndDotChars) + 1);
                //            //}

                //            //bool found;
                //            //Int32Encapsulator index = this.suggestionItemsAndIndexes.TryGetItem(suggestion, CaseSensitivity.Insensitive, out found);
                //            //if (!found || index == null)
                //            //{
                //            //    this.showInfoBoxTimer.Stop();
                //            //    this.Suggester.SelectedIndex = -1;
                //            //}
                //            //else
                //            //{
                //            //    this.promptHandler.lastSuggested = suggestion;
                //            //    this.Suggester.SelectedIndex = index;
                //            //    //this.ResetShowInfoBoxTimer();
                //            //}

                //            bool found;
                //            Int32Encapsulator index = this.suggestionItemsAndIndexes.TryGetItem(suggestion, CaseSensitivity.Insensitive, out found);
                //            if (!found || index == null)
                //            {
                //                this.showInfoBoxTimer.Stop();
                //                this.Suggester.SelectedIndex = -1;
                //            }
                //            else
                //            {
                //                this.promptHandler.lastSuggested = suggestion;
                //                this.Suggester.SelectedIndex = index;
                //                //this.ResetShowInfoBoxTimer();
                //            }

                //            this.lastSuggestedIndex = this.Suggester.SelectedIndex;
                //        }

                //        this.Suggester.CenterSelectedItem();
                //    }
                //}
            }

            private string GetWhatPromptTextWillBe(char newChar)
            {
                string currentText = InternalGlobals.CurrentSkinInstance.Prompt.Text;
                return String.Format(
                    CultureInfo.CurrentCulture, 
                    "{0}{1}{2}",
                    currentText.Substring(0, InternalGlobals.CurrentSkinInstance.Prompt.SelectionStart),
                    newChar,
                    currentText.Substring(InternalGlobals.CurrentSkinInstance.Prompt.SelectionStart + InternalGlobals.CurrentSkinInstance.Prompt.SelectionLength));
            }

            private string GetWhatPromptTextWillBe(string newText)
            {
                string currentText = InternalGlobals.CurrentSkinInstance.Prompt.Text;
                return String.Format(
                    CultureInfo.CurrentCulture, 
                    "{0}{1}{2}",
                    currentText.Substring(0, InternalGlobals.CurrentSkinInstance.Prompt.SelectionStart),
                    newText,
                    currentText.Substring(InternalGlobals.CurrentSkinInstance.Prompt.SelectionStart + InternalGlobals.CurrentSkinInstance.Prompt.SelectionLength));
            }

            private void HandleSuggesterDesiredIconSizeChanged(object sender, EventArgs e)
            {
                this.DetermineIconSize();
            }

            private void DetermineIconSize()
            {
                int size = this.Suggester.DesiredIconSize;

                if (size < 20)
                {
                    this.populationProvider.IconSize = IconSize.Small;
                }
                else
                {
                    this.populationProvider.IconSize = IconSize.Large;
                }
            }
        }
    }
}
