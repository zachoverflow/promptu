using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using ZachJohnson.Promptu.Collections;
using System.IO;
using System.Xml;
using ZachJohnson.Promptu.SkinApi;
using System.Runtime.Serialization;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UI;
using System.Extensions;
using System.Threading;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.Itl;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.UIModel;
using System.Windows.Forms;
using ZachJohnson.Promptu.PluginModel.Internals;
using System.Globalization;
using ZachJohnson.Promptu.PluginModel.Hooks;

namespace ZachJohnson.Promptu.Skins
{
    internal partial class PromptHandler : IDisposable
    {
        private const int MilisecondsToResetPromptText = 60000;
        private const int MilisecondsToRedetermineValidPathCommands = 30 * 60000;
        private static string PathSeparatorString = new string(Path.DirectorySeparatorChar, 1);
        private static string NamespaceSeparatorString = new string('.', 1);
        private static char[] PathSeparatorCharArray = new char[] { Path.DirectorySeparatorChar };
        private static char[] spaceAndDotChars = new char[] { '.', ' ' };
        private static PromptHandler instance;
        private static bool isInitializing;
        //private IPrompt prompt;
        //private PromptuSkin currentSkin;
        private string lastSuggested = String.Empty;
        //private SetupDialogPresenter setupDialog;
        private SetupDialogManager setupDialog;
        private SeparateSuggestionHandler suggestionHandler;
        //private bool suggestionIsCommandBasedPath;
        //private FindOptimizedStringCollection blacklistedDirectories = new FindOptimizedStringCollection(SortMode.DecendingFromLastAdded);
        //private System.Timers.Timer promptTextResetTimer;
        //private FindOptimizedStringCollection alternateSuggestionBase = null;
        //private FindOptimizedStringCollection lastPathSuggestionDirectoryContents;
        //private bool couldBePath = false;
        //private bool suggestionIsNamespace;
       // private bool suggestionIsPath;
        private SuggestionMode suggestionMode = SuggestionMode.Normal;
        private int? promptClosedAt;
        // HACK private Notes notesDialog = new Notes();

        protected PromptHandler()
        {
            isInitializing = true;
            this.suggestionHandler = new SeparateSuggestionHandler(this);
            this.SwitchToProfile(InternalGlobals.CurrentProfile, true);

            this.setupDialog = new SetupDialogManager();
            //this.setupDialog.HotkeyChanged += this.HandleHotkeyChanged;
            //this.setupDialog = new SetupDialogPresenter();
            // HACK next 2 disabled
            //this.setupDialog.HotkeyChanged += this.HandleHotkeyChanged;
            //this.setupDialog.SkinChanged += this.SkinChanged;
            //IntPtr setupDialogHandle = this.setupDialog.Handle;
            isInitializing = false;

            IsCreated = true;

            //this.promptTextResetTimer = new System.Timers.Timer(60000);
            //this.promptTextResetTimer.AutoReset = false;
            //this.promptTextResetTimer.SynchronizingObject = this.setupDialog;
            //this.promptTextResetTimer.Elapsed += this.HandlePromptTextResetTimerElapsed;
        }

        // HACK disabled
        //public Notes NotesDialog
        //{
        //    get { return this.notesDialog; }
        //}

        public static bool IsCreated
        {
            get;
            private set;
        }

        public void GiveContextualError(string message)
        {
            MessageBoxProvider.GiveError(message, this.suggestionHandler, this.suggestionHandler.Suggester, 1);
        }

        public static object GetDialogOwner()
        {
            if (IsInitializing)
            {
                return null;
            }
            else
            {
                return GetInstance().SetupDialog.DialogOwner;
            }
        }

        public static bool IsInitializing
        {
            get { return isInitializing; }
        }

        public event EventHandler HotkeyChanged;

        public event EventHandler ListsChanged;

        public static PromptHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new PromptHandler();
            }

            return instance;
        }

        public bool InvokeOnMainThreadRequired
        {
            get { return InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.InvokeRequired; }
        }

        public void InvokeOnMainThread(Delegate method, params object[] args)
        {
            //if (args.Length > 0)
            //{
                InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(method, args);
            //}
            //else
            //{
            //    InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(method);
            //}
        }

        public void InvokeOnMainThreadAsync(Delegate method, params object[] args)
        {
            //if (args.Length > 0)
            //{
            InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.BeginInvoke(method, args);
            //}
            //else
            //{
            //    InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(method);
            //}
        }

        public void ShowSetupDialog()
        {
            this.setupDialog.Show();
        }

        public void ShowAbout()
        {
            this.SetupDialog.ShowUserToAbout();
        }

        public SetupDialogManager SetupDialog
        {
            get { return this.setupDialog; }
        }

        //private void HandlePromptTextResetTimerElapsed(object sender, EventArgs e)
        //{
        //    this.promptTextResetTimer.Stop();
        //    this.prompt.TextualInput.InputText = String.Empty;
        //}

        private void HandlePromptLocationOnScreenChanged(object sender, EventArgs e)
        {
            if (InternalGlobals.CurrentProfile.PromptPositioningMode == PositioningMode.None && InternalGlobals.CurrentSkinInstance.Prompt.IsCreated)
            {
                InternalGlobals.CurrentProfile.PromptLocation = InternalGlobals.CurrentSkinInstance.Prompt.Location;
            }
        }

        public static void NotifyListsChanged()
        {
            if (!IsInitializing)
            {
                //REVISIT GetInstance().prompt.UpdateNewCommandItems();
                GetInstance().OnListsChanged(EventArgs.Empty);
            }
        }

        private void AttachToPrompt()
        {
            InternalGlobals.CurrentSkinInstance.Prompt.KeyPressed += this.KeyPressedByUser;
            InternalGlobals.CurrentSkinInstance.Prompt.LocationChanged += this.HandlePromptLocationOnScreenChanged;
            this.suggestionHandler.AttachToCurrentPrompt();
            //REVISIT this.prompt.UpdateNewCommandItems();
        }

        private void DetachFromPrompt()
        {
            InternalGlobals.CurrentSkinInstance.Prompt.KeyPressed -= this.KeyPressedByUser;
            InternalGlobals.CurrentSkinInstance.Prompt.LocationChanged -= this.HandlePromptLocationOnScreenChanged;
            this.suggestionHandler.DetachFromCurrentPrompt();
        }

        private void HandleSuggestionAffectingChange(object sender, EventArgs e)
        {
            this.NotifySuggestionAffectingChange();
        }

        public void NotifySuggestionAffectingChange()
        {
            this.suggestionHandler.HandleSuggestionAffectingChange();
        }

        private void UpdateHistoryItemsAreValidPaths()
        {
            string[] keys = new string[InternalGlobals.CurrentProfile.History.Count];
            InternalGlobals.CurrentProfile.History.Keys.CopyTo(keys);
            foreach (string key in keys)
            {
                bool found;
                HistoryDetails details = InternalGlobals.CurrentProfile.History.TryGetItem(key, CaseSensitivity.Sensitive, out found);
                if (details != null && found)
                {
                    bool isValidPath = details.IsValidPath;
                }
            }
        }

        private void RestartPromptu()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Application.ExecutablePath, String.Format(
                CultureInfo.InvariantCulture,
                "wait-for {0}",
                System.Diagnostics.Process.GetCurrentProcess().Id));

            System.Diagnostics.Process.Start(startInfo);

            PromptuUtilities.ExitApplication();
        }

        public void SwitchToProfile(Profile profile, bool doNotRegenerateComposites)
        {
            Profile currentProfile = InternalGlobals.CurrentProfile;
            if (currentProfile != null)
            {
                InternalGlobals.TryResaveAndAlertUser();
                InternalGlobals.FailedToSaveFiles.Clear();

                currentProfile.Hotkey.Unregister();
                currentProfile.Hotkey.Pressed -= this.HotKeyPressed;
                currentProfile.Hotkey.Changed -= this.HandleHotkeyChanged;
                currentProfile.Hotkey.RegistrationChanged -= this.HandleHotkeyChanged;
                //currentProfile.NotesHotkey.Unregister();
                //currentProfile.NotesHotkey.Pressed -= this.HandleNotesHotkeyPressed;
                currentProfile.ListsSynchronized -= this.ListsSynchronized;
                currentProfile.DisableSynchronization();
                currentProfile.BackgroundWorkQueue.Stop();
                currentProfile.DeleteLockFile();
                InternalGlobals.AvailablePlugins.DisableAndResetAll();
                InternalGlobals.GuiManager.ToolkitHost.NotifyProfileUnloading();
            }

            if (InternalGlobals.CurrentSkinInstance != null)
            {
                InternalGlobals.CurrentSkinInstance.Prompt.Hide();
                this.DetachFromPrompt();
                InternalGlobals.CurrentProfile.SkinsSettings.TrySerialize(InternalGlobals.CurrentSkinInstance, InternalGlobals.CurrentSkin.Id);
            }

            if (InternalGlobals.CurrentProfile != profile)
            {
                InternalGlobals.CurrentProfile = profile;
            }

            if (InternalGlobals.CurrentProfile.SkinId == null)
            {
                InternalGlobals.CurrentProfile.SkinId = ToolkitHost.DefaultSkinId;
            }

            PromptuSkin skin = InternalGlobals.Skins.TryGet(InternalGlobals.CurrentProfile.SkinId);

            if (skin == null)
            {
                InternalGlobals.CurrentProfile.SkinId = ToolkitHost.DefaultSkinId;
                skin = InternalGlobals.Skins.TryGet(InternalGlobals.CurrentProfile.SkinId);
            }

            PromptuSkinInstance skinInstance = skin.InstanceFactory.CreateNewInstance();

            InternalGlobals.CurrentSkin = skin;
            InternalGlobals.CurrentSkinInstance = skinInstance;
            profile.SkinsSettings.TryRehydrate(skinInstance, skin.Id);

            profile.EnableSynchronization();

            //this.prompt = Globals.CurrentSkin.GetInstance();
            //Globals.CurrentSkin = Globals.GuiManager.ToolkitHost.CreateDefaultSkinInstance();

            Point? promptLocation = InternalGlobals.CurrentProfile.PromptLocation;

            profile.ListsSynchronized += this.ListsSynchronized;

            profile.QueueCommandFileSystemValidations();

            profile.History.Changed += this.HandleSuggestionAffectingChange;
            profile.CompositeFunctionsAndCommandsMediator.ClientsRegenerated += this.HandleSuggestionAffectingChange;
            profile.PluginMeta.LoadMetadata(InternalGlobals.AvailablePlugins);

            InternalGlobals.GuiManager.LoadUISettingsFromCurrentProfile();

            GlobalHotkey hotkey = profile.Hotkey;
            hotkey.Pressed += this.HotKeyPressed;
            hotkey.Changed += this.HandleHotkeyChanged;
            hotkey.RegistrationChanged += this.HandleHotkeyChanged;
            while (true)
            {
                try
                {
                    hotkey.Register();
                    break;
                }
                catch (HotkeyException)
                {
                    HotkeyInUseDialogPresenter dialog = new HotkeyInUseDialogPresenter(false, hotkey);
                    //dialog.HotkeySetupPanel.HotkeyModifierKeys = hotkey.ModifierKeys;
                    //dialog.HotkeySetupPanel.UnderlyingHotkeyKeyValue = hotkey.Key;
                    //dialog.HotkeyPresenter.UpdateTo(hotkey);

                    if (dialog.ShowDialog() == UIDialogResult.OK)
                    {
                        if (dialog.Result == HotkeyInUseResult.NewHotkey)
                        {
                            dialog.HotkeyPresenter.ImpartTo(hotkey);
                        }
                        else
                        {
                            hotkey.Unregister();
                            hotkey.OverrideIfNecessary = true;
                            hotkey.Register();
                        }

                        profile.SaveConfig();
                        //hotkey.SwitchHotkey(dialog.HotkeySetupPanel.HotkeyModifierKeys, dialog.HotkeySetupPanel.UnderlyingHotkeyKeyValue);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            InternalGlobals.NotifyIcon.UpdateToolTipText();

            //try
            //{
            //    InternalGlobals.CurrentProfile.NotesHotkey.Register();
            //}
            //catch (HotkeyException)
            //{
            //}

            //InternalGlobals.CurrentProfile.NotesHotkey.Pressed += this.HandleNotesHotkeyPressed;

            //bool failed = false;

            try
            {
                this.AttachToPrompt();
            }
            catch (OopsTryThatAgainException)
            {
                //failed = true;
                // Handling logic for strange NullReferenceException at NativeWindow.AssignHandle()
                for (int i = 0; i < 5; i++)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(String.Empty);
                    System.Diagnostics.Debug.WriteLine(String.Empty);
                    System.Diagnostics.Debug.WriteLine(String.Empty);
                    System.Diagnostics.Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Handled OopsTryThatAgain! Try {0}.", i));
                    System.Diagnostics.Debug.WriteLine(String.Empty);
                    System.Diagnostics.Debug.WriteLine(String.Empty);
                    System.Diagnostics.Debug.WriteLine(String.Empty);

                    //MessageBox.Show("Handle aquisition failed.");
#endif
                    this.DetachFromPrompt();
                    skinInstance = skin.InstanceFactory.CreateNewInstance();
                    InternalGlobals.CurrentSkinInstance = skinInstance;
                    profile.SkinsSettings.TryRehydrate(skinInstance, skin.Id);

                    try
                    {
                        this.AttachToPrompt();
                    }
                    catch (OopsTryThatAgainException)
                    {
                        continue;
                    }

                    break;
                }
            }

            //if (!failed)
            //{
            //    RestartPromptu();
            //}

            InternalGlobals.CurrentSkinInstance.Prompt.Text = String.Empty;
            this.OnListsChanged(EventArgs.Empty);

            profile.UpdateLockFile();

            if (this.setupDialog != null)
            {
                //this.setupDialog.ListSelector.UpdateLists();
                this.setupDialog.UpdateListSelectorLists();
                this.setupDialog.UpdateInstalledPlugins();
            }

            profile.BackgroundWorkQueue.AddWork(new ParameterlessVoid(this.UpdateHistoryItemsAreValidPaths));

            if (!doNotRegenerateComposites)
            {
                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
            }

            InternalGlobals.GuiManager.ToolkitHost.NotifyProfileLoaded();
            PromptuHookManager.RaiseCurrentProfileChanged();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InternalGlobals.CurrentProfile.Hotkey.Unregister();
            //this.notesDialog.Dispose();
            //this.setupDialog.Dispose();
        }

        private void HotKeyPressed(object sender, EventArgs e)
        {
            this.OpenPrompt();
        }

        public void ChangeSkin(PromptuSkin changeTo)
        {
            this.ClosePrompt();
            //this.prompt.ClosePrompt();
            this.DetachFromPrompt();
            
            //this.prompt = changeTo.GetInstance();
            InternalGlobals.CurrentProfile.SkinsSettings.TrySerialize(InternalGlobals.CurrentSkinInstance, InternalGlobals.CurrentSkin.Id);
            InternalGlobals.CurrentProfile.SkinId = changeTo.Id;
            InternalGlobals.CurrentSkin = changeTo;
            InternalGlobals.CurrentSkinInstance = changeTo.InstanceFactory.CreateNewInstance();

            InternalGlobals.CurrentProfile.SkinsSettings.TryRehydrate(InternalGlobals.CurrentSkinInstance, changeTo.Id);
            this.AttachToPrompt();
            InternalGlobals.CurrentSkinInstance.Prompt.Text = String.Empty;
            //this.prompt.TextualInput.InputText = String.Empty;
        }

        private void ListsSynchronized(object sender, EventArgs e)
        {
            if (this.setupDialog != null)
            {
                this.setupDialog.UpdateToCurrentListThreadSafe();
            }
        }

        private void KeyPressedByUser(object sender, KeyPressedEventArgs e)
        {
            this.suggestionHandler.HandleKeyPress(e);
        }

        public void TakeUserToHelp()
        {
            try
            {
                Process.Start("http://www.promptulauncher.com/docs/");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                UIMessageBox.Show(
                    Localization.Promptu.ErrorVisitingHelpOnWebsite,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
            catch (FileNotFoundException)
            {
                UIMessageBox.Show(
                    Localization.Promptu.ErrorVisitingHelpOnWebsite,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
        }

        //private MessageBoxOptions GetMessageBoxOptions()
        //{
        //    MessageBoxOptions options = (MessageBoxOptions)0;

        //    if (System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
        //    {
        //        options |= MessageBoxOptions.RightAlign & MessageBoxOptions.RtlReading;
        //    }

        //    return options;
        //}

        internal void ExecuteCommand(string input, bool saveInHistory, ExecuteMode executeMode, ParameterHelpContext contextInfo)
        {
            if (input.Trim().Length <= 0)
            {
                return;
            }

            string historyItem = input;
            string itemId = null;
            bool isFunction = false;
            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                {
                    InternalGlobals.CurrentSkinInstance.Prompt.Text = String.Empty;
                }));

            string[] split = SuggestionUtilities.ExtractSimpleNameAndParametersFrom(input);
            //int argumentCount = split.Length - 1;

            //if (Function.IsInFunctionSyntax(input) && split.Length == 2)
            //{
            //    if (split[1].Length <= 0)
            //    {
            //        argumentCount = 0;
            //    }
            //}

            string[] args = new string[split.Length - 1];

            for (int i = 1; i < split.Length; i++)
            {
                args[i - 1] = split[i];
            }

            bool quit = false;

            string commandName = split[0];
            try
            {
                bool notACommandOrFunction = true;

                int indexOfFirstDirectorySeparatorChar = commandName.IndexOf(Path.DirectorySeparatorChar);
                if (indexOfFirstDirectorySeparatorChar > -1)
                {
                    string beginningFolderName = input.Substring(0, indexOfFirstDirectorySeparatorChar);

                    bool found;
                    CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder = null;

                    if (!(contextInfo != null && (parameterlessCommandNamedLikeFirstFolder = contextInfo.GetCurrentCompositeItem() as CompositeItem<Command, List>) != null))
                    {
                        parameterlessCommandNamedLikeFirstFolder = null;
                        GroupedCompositeItem groupedCompositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
                        if (found && groupedCompositeItem != null)
                        {
                            parameterlessCommandNamedLikeFirstFolder = groupedCompositeItem.TryGetCommand(beginningFolderName, 0);
                        }
                        else
                        {
                            commandName = input;
                        }
                    }

                    if (parameterlessCommandNamedLikeFirstFolder != null)
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
                                commandName = proposedDirectory + input.Substring(indexOfFirstDirectorySeparatorChar);
                            }
                        }
                        catch (ParseException ex)
                        {
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                            {
                                if (UIMessageBox.Show(
                                    String.Format(Localization.MessageFormats.ItlParseErrorOnCommandExecute, ex.Message, beginningFolderName),
                                    Localization.Promptu.AppName,
                                    UIMessageBoxButtons.YesNo,
                                    UIMessageBoxIcon.Error,
                                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                                {
                                    this.ShowSetupDialog();
                                    this.setupDialog.EditCommand(beginningFolderName);
                                }

                            }));
                        }
                        catch (ConversionException ex)
                        {
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                            {
                                UIMessageBox.Show(
                                        ex.Message,
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.OK,
                                        UIMessageBoxIcon.Error,
                                        UIMessageBoxResult.OK);
                            }));
                        }
                        catch (SelfReferencingCommandException ex)
                        {
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                            {
                                UIMessageBox.Show(
                                        ex.Message,
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.OK,
                                        UIMessageBoxIcon.Error,
                                        UIMessageBoxResult.OK);
                            }));
                        }
                    }

                    args = new string[0];
                }
                else
                {
                    GroupedCompositeItem compositeItem = null;

                    bool compositeItemFound;
                    if ((compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(commandName, out compositeItemFound)) != null
                        && compositeItemFound)
                    {
                        notACommandOrFunction = false;
                        bool mustBeFunction = false;
                        //if (commandName.Length < input.Length)
                        //{
                            mustBeFunction = Function.IsInFunctionSyntax(input);
                        //}
                        //else
                        //Command command = compositeItem.Item as Command;
                        //Function function;
                        CompositeItem<Command, List> command = null;
                        CompositeItem<Function, List> function = null;

                        bool useFunctionIfPossible = false;

                        bool useDefaultValidation = true;

                        if (contextInfo != null)
                        {
                            object selectedCompositeItem = contextInfo.GetCurrentCompositeItem();

                            if ((command = selectedCompositeItem as CompositeItem<Command, List>) != null)
                            {
                                useDefaultValidation = false;
                            }
                            else if ((function = selectedCompositeItem as CompositeItem<Function, List>) != null)
                            {
                                useFunctionIfPossible = true;
                                useDefaultValidation = false;
                            }
                        }

                        if (useDefaultValidation)
                        {
                            function = compositeItem.TryGetStringFunction(commandName, Function.CreateAllStringParameterSignature(args.Length));
                            if ((mustBeFunction && compositeItem.StringFunctions.Count > 0) 
                            || (((command = compositeItem.TryGetCommand(commandName)) == null || !command.Item.TakesParameterCountOf(args.Length))
                            && function != null))
                            {
                                useFunctionIfPossible = true;
                            }
                        }

                        if (useFunctionIfPossible && (function != null || mustBeFunction))
                        {
                            isFunction = true;
                            //Function function = compositeItem.Functions[commandName, args.Length];
                            //historyItem = commandName;
                            if (mustBeFunction)
                            {
                                bool leaveInPrompt;
                                saveInHistory = this.ExecuteFunction(input, executeMode, out leaveInPrompt);
                            }
                            else
                            {
                                saveInHistory = this.ExecuteFunction(function.Item, function.ListFrom, args, true, executeMode);
                            }
                        }
                        else if (!mustBeFunction && command != null)
                        {
                            bool nameAndArgsRun;
                            itemId = Command.GenerateItemId(command.Item, command.ListFrom);
                            saveInHistory = this.ExecuteCommand(
                                true,
                                command.Item, 
                                command.ListFrom,
                                args, 
                                true, 
                                executeMode, 
                                new TrieList(SortMode.DecendingFromLastAdded),
                                out nameAndArgsRun);

                            if (saveInHistory)
                            {
                                if (!nameAndArgsRun)
                                {
                                    if (!command.Item.ShowParameterHistory)
                                    {
                                        bool anyShowingHistory = false;
                                        foreach (CommandParameterMetaInfo info in command.Item.ParametersMetaInfo)
                                        {
                                            if (info.ShowHistory)
                                            {
                                                anyShowingHistory = true;
                                                break;
                                            }
                                        }

                                        if (!anyShowingHistory)
                                        {
                                            historyItem = commandName;
                                        }
                                    }
                                }
                                else
                                {
                                    itemId = null;
                                }
                            }
                        }
                        else
                        {
                            notACommandOrFunction = true;
                        }
                        //else if ((function = compositeItem.Item as Function) != null)
                        //{

                        //}
                    }
                }

                if (notACommandOrFunction)
                {
                    switch (commandName.ToUpperInvariant())
                    {
                        case "SETUP":
                            this.ShowSetupDialog();
                            break;
                        case "QUIT":
                            quit = true;
                            break;
                        case "ABOUT":
                            this.InvokeOnMainThread(new ParameterlessVoid(this.ShowAbout));
                            break;
                        case "HELP":
                            Process.Start("http://www.promptulauncher.com/docs/");
                            break;
                        case "SYNCHRONIZE":
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                                {
                                    this.setupDialog.SyncLists();

                                    UIMessageBox.Show(
                                        Localization.UIResources.SynchronizationComplete,
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.OK,
                                        UIMessageBoxIcon.Information,
                                        UIMessageBoxResult.OK);
                                }));
                            break;
                        default:
                            if (commandName.Length > 0)
                            {
                                StringBuilder arguments = new StringBuilder();

                                bool appendSpace = false;
                                foreach (string arg in args)
                                {
                                    if (appendSpace)
                                    {
                                        arguments.Append(" ");
                                    }
                                    else
                                    {
                                        appendSpace = true;
                                    }

                                    arguments.Append(arg);
                                }

                                if (executeMode == ExecuteMode.ToClipboard)
                                {
                                    this.InvokeOnMainThread(new ParameterlessVoid(delegate
                                        {
                                            string data = commandName;
                                            if (arguments.Length > 0)
                                            {
                                                data += " " + arguments;
                                            }

                                             InternalGlobals.GuiManager.ToolkitHost.Clipboard.SetText(data);
                                        }));
                                }
                                else
                                {
                                    ProcessStartInfo startInfo = new ProcessStartInfo(commandName, arguments.ToString());
                                    FileSystemFile executingFile = commandName;
                                    if (executingFile.Exists)
                                    {
                                        startInfo.WorkingDirectory = executingFile.GetParentDirectory();
                                    }

                                    System.Diagnostics.Process.Start(startInfo);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Win32Exception ex)
            {
                //if (ex.NativeErrorCode != 1223) // 1223 means UAC cancelled
                //{
                //    this.GiveExecuteFailedMessage(input);
                //}

                switch (ex.NativeErrorCode)
                {
                    case 1223: // 1223 means UAC cancelled
                        break;
                    case 32: // locked by another process
                        this.GiveExecuteCommandFailedMessage(
                            input,
                            null,
                            false,
                            Localization.MessageFormats.CommandExecutionFailedLocked,
                            Localization.MessageFormats.CommandExecutionFailedLockedNoEdit);

                        break;
                    default:
                        this.GiveExecuteCommandFailedMessage(
                            input,
                            null,
                            false,
                            Localization.MessageFormats.CommandExecutionFailedWin32,
                            Localization.MessageFormats.CommandExecutionFailedWin32NoEdit);

                        break;
                }

                return;
            }
            catch (System.IO.FileNotFoundException)
            {
                this.GiveExecuteFailedMessage(input);
                return;
            }

            if (saveInHistory)
            {
                using (DdMonitor.Lock(this))
                {
                    //PromptuSettings.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;
                    if (SuggestionUtilities.IsConsideredComplex(historyItem))
                    {
                        string complexItem = Function.AppendImplicitCloseParentheses(historyItem);
                        InternalGlobals.CurrentProfile.History.ComplexHistory.Add(complexItem, new HistoryDetails(complexItem, null));
                    }

                    if (isFunction && Function.IsInFunctionSyntax(historyItem))
                    {
                        historyItem = Function.AppendImplicitCloseParentheses(historyItem);
                        List<StringBuilder> splitFunctions = Function.Split(historyItem);
                        for (int i = 0; i < splitFunctions.Count; i++)
                        {
                            string functionCall = splitFunctions[splitFunctions.Count - 1 - i].ToString();
                            HistoryDetails details = new HistoryDetails(functionCall, null);
                            InternalGlobals.CurrentProfile.History.Add(functionCall, details);
                        }

                        InternalGlobals.CurrentProfile.History.Save();
                    }
                    else
                    {
                        if (isFunction)
                        {
                            historyItem = Function.ConvertToFunctionSyntax(historyItem);
                        }
                        else
                        {
                            historyItem = SuggestionUtilities.QuotizeCommandExecution(historyItem);
                        }

                        HistoryDetails details = new HistoryDetails(historyItem, itemId);
                        InternalGlobals.CurrentProfile.History.Add(historyItem, details);
                        InternalGlobals.CurrentProfile.History.Save();
                        InternalGlobals.CurrentProfile.BackgroundWorkQueue.AddWork(new ParameterlessVoid(delegate { bool isPath = details.IsValidPath; }));
                    }

                    //PromptuSettings.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
                    //PromptuSettings.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
                }
            }

            if (quit)
            {
                this.InvokeOnMainThread(new ParameterlessVoid(delegate
                    {
                        PromptuUtilities.ExitApplication();
                    }));
            }
        }

        public bool ExecuteFunction(string input, ExecuteMode mode, out bool leaveInPrompt)
        {
            ItlCompiler compiler = new ItlCompiler();
            FeedbackCollection feedback;
            Expression expression = compiler.Compile(ItlType.InlineExecution, input, true, out feedback);

            if (feedback.Has(FeedbackType.Error))
            {
                leaveInPrompt = true;

                StringBuilder builder = new StringBuilder();
                foreach (FeedbackMessage message in feedback)
                {
                    builder.AppendLine(message.ToString());
                }

                this.InvokeOnMainThread(new ParameterlessVoid(delegate
                    {
                        UIMessageBox.Show(
                            String.Format(Localization.MessageFormats.ExecuteFunctionCompileError, input, builder.ToString()),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }));
            }
            else
            {
                leaveInPrompt = false;

                try
                {
                    this.ExecuteExpression(expression, new ExecutionData(new string[0], null, InternalGlobals.CurrentProfile.Lists), mode);
                    return true;
                }
                catch (ConversionException ex)
                {
                    this.InvokeOnMainThread(new ParameterlessVoid(delegate
                    {
                        UIMessageBox.Show(
                            ex.Message,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }));
                }
            }

            return false;
        }

        public bool ExecuteExpression(Expression expression, ExecutionData data, ExecuteMode mode)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            else if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string result = expression.ConvertToString(data);//new ExecutionData(args, listFrom, PromptuSettings.CurrentProfile.Lists));
                
                //function.Invoke(
                //        args,
                //        ,
                //        new AssemblyReferenceCollectionComposite(PromptuSettings.CurrentProfile.Lists, listFrom));

            if (result != null)
            {
                if (mode == ExecuteMode.ToClipboard)
                {
                    this.InvokeOnMainThread(new ParameterlessVoid(delegate
                        {
                            InternalGlobals.GuiManager.ToolkitHost.Clipboard.SetText(result);
                        }));
                }
                else
                {
                    UIMessageBox.Show(
                        result,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.None,
                       UIMessageBoxResult.OK);

                }
            }

            return true;
        }

        public bool ExecuteFunction(Function function, List listFrom, string[] args, bool onFailAllowUserToEdit, ExecuteMode mode)
        {
            //if (function == null)
            //{
            //    throw new ArgumentNullException();
            //}
            //if (parameters == null)
            //{
            //    throw new ArgumentNullException("parameters");
            //}

            //string[] args = parameters.BreakApart(BreakApartMode.EatQuotes);
            if (function.Parameters.Count > args.Length)
            {
                this.InvokeOnMainThread(new ParameterlessVoid(delegate
                {
                    if (onFailAllowUserToEdit)
                    {
                        if (UIMessageBox.Show(
                            String.Format(Localization.MessageFormats.TooLittleNumberOfParametersForFunction, function.Name),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.YesNo,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                        {
                            this.ShowSetupDialog();
                            this.setupDialog.EditFunction(function.Name, function.ParameterSignature);
                        }
                    }
                    else
                    {
                        UIMessageBox.Show(
                            Localization.MessageFormats.TooLittleNumberOfParametersForFunctionNoEdit,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }
                }));
            }
            else if (function.Parameters.Count < args.Length)
            {
                this.InvokeOnMainThread(new ParameterlessVoid(delegate
                {
                    if (onFailAllowUserToEdit)
                    {
                        if (UIMessageBox.Show(
                            String.Format(Localization.MessageFormats.TooManyParametersForFunction, function.Name),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.YesNo,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                        {
                            this.ShowSetupDialog();
                            this.setupDialog.EditFunction(function.Name, function.ParameterSignature);
                        }
                    }
                    else
                    {
                        UIMessageBox.Show(
                            Localization.MessageFormats.TooManyParametersForFunctionNoEdit,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }
                }));
            }
            else
            {
                try
                {
                    object result = function.Invoke(
                        args,
                        new ExecutionData(args, listFrom, InternalGlobals.CurrentProfile.Lists),
                        new AssemblyReferenceCollectionComposite(InternalGlobals.CurrentProfile.Lists, listFrom));

                    string displayValue;

                    string[] arrayResult = result as string[];
                    if (arrayResult != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("String[]:");
                        bool doneFirst = false;
                        foreach (string element in arrayResult)
                        {
                            if (doneFirst)
                            {
                                builder.Append(", ");
                            }

                            builder.AppendFormat("\"{0}\"", element);
                            doneFirst = true;
                        }

                        displayValue = builder.ToString();
                    }
                    else
                    {
                        displayValue = result.ToStringNullSafe();
                    }

                    if (result != null)
                    {
                        if (mode == ExecuteMode.ToClipboard)
                        {
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                                {
                                    InternalGlobals.GuiManager.ToolkitHost.Clipboard.SetText(displayValue);
                                }));
                        }
                        else
                        {
                            UIMessageBox.Show(
                                displayValue,
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.OK,
                                UIMessageBoxIcon.None,
                                UIMessageBoxResult.OK);

                        }
                    }

                    return true;
                }
                catch (LoadException ex)
                {
                    this.InvokeOnMainThread(new ParameterlessVoid(delegate
                    {
                        if (onFailAllowUserToEdit)
                        {
                            if (UIMessageBox.Show(
                                String.Format(Localization.MessageFormats.ErrorOnFunctionInvoke, ex.Message, function.Name),
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.YesNo,
                                UIMessageBoxIcon.Error,
                                UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                            {
                                this.ShowSetupDialog();
                                this.setupDialog.EditFunction(function.Name, function.ParameterSignature);
                            }
                        }
                        else
                        {
                            UIMessageBox.Show(
                                ex.Message,
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.OK,
                                UIMessageBoxIcon.Error,
                                UIMessageBoxResult.OK);
                        }
                    }));
                }
            }

            return false;
        }

        //private string GetResolvedExecutionPath(Command command, List listFrom,)
        //{
        //    if (command.TakesParameterCountOf(args.Length))
        //    {
        //        ExecutionData executionData = new ExecutionData(args, listFrom, PromptuSettings.CurrentProfile.Lists);

        //        try
        //        {
        //            string resolved = command.GetSubstitutedExecutionPath(executionData);

        //        string[] pathSplit;
        //        if (Utilities.IsValidPath(resolved, out pathSplit) && pathSplit.Length > 0)
        //        {
        //            string beginningFolderName = pathSplit[0];
        //            bool found;
        //            GroupedCompositeItem compositeItem = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(beginningFolderName, out found);
        //            CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
        //            if (found && compositeItem != null && (parameterlessCommandNamedLikeFirstFolder = compositeItem.TryGetCommand(beginningFolderName, 0)) != null)
        //            {
        //                FileSystemDirectory proposedDirectory = this.GetResolvedExecutionPath(
        //                    parameterlessCommandNamedLikeFirstFolder.Item, 
        //                    parameterlessCommandNamedLikeFirstFolder.ListFrom, 
        //                    new string[0], 
        //                    alreadyCalledCommandNames, 
        //                    false);

        //                if (proposedDirectory.Exists)
        //                {
        //                    if (alreadyCalledCommandNames.Contains(beginningFolderName, CaseSensitivity.Insensitive))
        //                    {
        //                        if (alreadyCalledMultiCommandNames.Contains(command.Name, CaseSensitivity.Insensitive))
        //                        {
        //                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
        //                            {
        //                                MessageBox.Show(
        //                                Localization.MessageFormats.SelfReferencingRecursiveMultiCall,
        //                                Localization.Promptu.AppName,
        //                                UIMessageBoxButtons.OK,
        //                                UIMessageBoxIcon.Error,
        //                                MessageBoxDefaultButton.Button1,
        //                                this.GetMessageBoxOptions());
        //                            }));
        //                            return false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        resolved = proposedDirectory + resolved.Substring(beginningFolderName.Length);
        //                        alreadyCalledCommandNames.Add(beginningFolderName);
        //                    }
        //                }
        //                //try
        //                //{
        //                //    ExecutionData itemExecutionData = new ExecutionData(
        //                //        new string[0],
        //                //        parameterlessCommandNamedLikeFirstFolder.ListFrom,
        //                //        PromptuSettings.CurrentProfile.Lists);
        //                //    FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
        //                //    if (proposedDirectory.Exists)
        //                //    {
        //                //        directory = proposedDirectory + populationFilter.Substring(indexOfFirstDirectorySeparatorChar, indexOfLastDirectorySeparatorChar - indexOfFirstDirectorySeparatorChar + 1);
        //                //    }
        //                //}
        //                //catch (Itl.ParseException)
        //                //{
        //                //}
        //                //catch (ConversionException)
        //                //{
        //                //}
        //            }
        //        }
        //        }
        //        catch (Itl.ParseException)
        //        {
        //            if (throwOnFailure)
        //            {
        //                throw;
        //            }
        //        }
        //        catch (ConversionException)
        //        {
        //            if (throwOnFailure)
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //}
        private bool TryRun(string commandName, string[] args, ExecuteMode executeMode)
        {
            try
            {
                if (commandName.Length > 0)
                {
                    StringBuilder arguments = new StringBuilder();

                    bool appendSpace = false;
                    foreach (string arg in args)
                    {
                        if (appendSpace)
                        {
                            arguments.Append(" ");
                        }
                        else
                        {
                            appendSpace = true;
                        }

                        arguments.Append(arg);
                    }

                    if (executeMode == ExecuteMode.ToClipboard)
                    {
                        this.InvokeOnMainThread(new ParameterlessVoid(delegate
                        {
                            string data = commandName;
                            if (arguments.Length > 0)
                            {
                                data += " " + arguments;
                            }

                            InternalGlobals.GuiManager.ToolkitHost.Clipboard.SetText(data);
                        }));
                    }
                    else
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(commandName, arguments.ToString());
                        FileSystemFile executingFile = commandName;
                        if (executingFile.Exists)
                        {
                            startInfo.WorkingDirectory = executingFile.GetParentDirectory();
                        }

                        System.Diagnostics.Process.Start(startInfo);
                    }
                }
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (System.IO.FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        public bool ExecuteCommand(bool allowTryRunOnFail, Command command, List listFrom, string[] args, bool onFailAllowUserToEdit, ExecuteMode mode, TrieList alreadyCalledMultiCommandNames, out bool nameAndArgsRun)
        {
            nameAndArgsRun = false;
            if (alreadyCalledMultiCommandNames == null)
            {
                throw new ArgumentNullException("alreadyCalledMultiCommandNames");
            }

            string[] realArgs = new string[args.Length];

            for (int i = 0; i < realArgs.Length; i++)
            {
                realArgs[i] = command.TranslateArgument(args[i], i, listFrom);
            }

            string executing = string.Empty;
            string arguments = null;
            string startupDirectory = null;
            try
            {
                //FunctionCollection functions = new FunctionCollection();
                //functions.Add(new Function("mo", "FunctionTesting.Test", "ConvertMonth", new AssemblyReference(@"C:\zjfiles\Projects\Penguin\FunctionTesting\FunctionTesting\bin\Debug\FunctionTesting.dll")));
                ExecutionData executionData = new ExecutionData(realArgs, listFrom, InternalGlobals.CurrentProfile.Lists);
                executing = command.GetSubstitutedExecutionPath(executionData);
                arguments = command.GetSubstitutedArguments(executionData);
                startupDirectory = command.GetSubstitutedStartupDirectory(executionData);
            }
            catch (ParseException ex)
            {
                this.InvokeOnMainThread(new ParameterlessVoid(delegate
                    {
                        if (onFailAllowUserToEdit)
                        {
                            if (UIMessageBox.Show(
                                String.Format(Localization.MessageFormats.ItlParseErrorOnCommandExecute, ex.Message, command.Name),
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.YesNo,
                                UIMessageBoxIcon.Error,
                                UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                            {
                                this.ShowSetupDialog();
                                this.setupDialog.EditCommand(command.Name);
                            }
                        }
                        else
                        {
                            UIMessageBox.Show(
                                ex.Message,
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.OK,
                                UIMessageBoxIcon.Error,
                                UIMessageBoxResult.OK);
                        }
                    }));

                return false;
            }
            catch (ConversionException ex)
            {
                if (!allowTryRunOnFail || !TryRun(command.Name, args, mode))
                {
                    this.InvokeOnMainThread(new ParameterlessVoid(delegate
                        {
                            if (onFailAllowUserToEdit)
                            {
                                if (UIMessageBox.Show(
                                    String.Format(Localization.MessageFormats.ConversionExceptionOnExecuteCommand, ex.Message, command.Name),
                                    Localization.Promptu.AppName,
                                    UIMessageBoxButtons.YesNo,
                                    UIMessageBoxIcon.Error,
                                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                                {
                                    this.ShowSetupDialog();
                                    this.setupDialog.EditCommand(command.Name);
                                }
                            }
                            else
                            {
                                UIMessageBox.Show(
                                        ex.Message,
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.OK,
                                        UIMessageBoxIcon.Error,
                                        UIMessageBoxResult.OK);
                            }
                        }));
                    return false;
                }
                else if (allowTryRunOnFail)
                {
                    nameAndArgsRun = true;
                    return true;
                }
            }
            catch (SelfReferencingCommandException ex)
            {
                this.InvokeOnMainThread(new ParameterlessVoid(delegate
                {
                    if (onFailAllowUserToEdit && (ex.CommandName != null && ex.ListFrom != null))
                    {
                        if (UIMessageBox.Show(
                            String.Format(Localization.MessageFormats.SelfReferenceOnExecuteCommand, ex.Message, ex.CommandName),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.YesNo,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                        {
                            this.ShowSetupDialog();
                            this.setupDialog.EditCommand(ex.CommandName, ex.ListFrom);
                        }
                    }
                    else
                    {
                        UIMessageBox.Show(
                                ex.Message,
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.OK,
                                UIMessageBoxIcon.Error,
                                UIMessageBoxResult.OK);
                    }
                }));

                return false;
            }

            switch (executing.Trim().ToUpperInvariant())
            {
                case "@REBOOT@":
                    if (UIMessageBox.Show(
                        Localization.Promptu.ConfirmReboot,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        InternalGlobals.GuiManager.ToolkitHost.Computer.Reboot();
                    }

                    return true;
                case "@SHUTDOWN@":
                    if (UIMessageBox.Show(
                        Localization.Promptu.ConfirmShutdown,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        InternalGlobals.GuiManager.ToolkitHost.Computer.ShutDown();
                    }

                    return true;
                case "@LOGOFF@":
                    if (UIMessageBox.Show(
                        Localization.Promptu.ConfirmLogoff,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        InternalGlobals.GuiManager.ToolkitHost.Computer.LogOff();
                    }

                    return true;
                case "@LOCK@":
                    InternalGlobals.GuiManager.ToolkitHost.Computer.Lock();
                    return true;
                case "@STANDBY@":
                    InternalGlobals.GuiManager.ToolkitHost.Computer.Standby();
                    return true;
                case "@HIBERNATE@":
                    if (UIMessageBox.Show(
                        Localization.Promptu.ConfirmHibernate,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        InternalGlobals.GuiManager.ToolkitHost.Computer.Hibernate();
                    }

                    return true;
                case "@SCREENSAVER@":
                    InternalGlobals.GuiManager.ToolkitHost.Computer.StartScreensaver();
                    return true;
                case "@MULTI@":
                    if (alreadyCalledMultiCommandNames.Contains(command.Name, CaseSensitivity.Insensitive))
                    {
                        this.InvokeOnMainThread(new ParameterlessVoid(delegate
                        {
                            UIMessageBox.Show(
                            String.Format(Localization.MessageFormats.SelfReferencingRecursiveMultiCall, command.Name),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        }));
                        return false;
                    }

                    alreadyCalledMultiCommandNames.Add(command.Name);

                    ExecutionData executionData = new ExecutionData(realArgs, listFrom, InternalGlobals.CurrentProfile.Lists);

                    bool anyValidNames = false;

                    string argumentsToUse;
                    if (arguments == null)
                    {
                        argumentsToUse = String.Empty;
                    }
                    else
                    {
                        argumentsToUse = arguments;
                    }

                    argumentsToUse = argumentsToUse.Replace("*", "*star*").Replace("&&", "*amp*");
                    string[] commandExpressions = argumentsToUse.BreakApart(Quotes.Include, Spaces.DoNotBreak, BreakingCharAction.Eat, '&');

                    foreach (string commandExpression in commandExpressions)
                    {
                        string commandExpressionUnEscaped = commandExpression.Replace("*star*", "*").Replace("*amp*", "&");
                        string[] splitExpression = commandExpressionUnEscaped.Trim().BreakApart();
                        if (splitExpression.Length < 1)
                        {
                            continue;
                        }

                        string commandName = splitExpression[0];
                        if (commandName.Length <= 0)
                        {
                            continue;
                        }

                        string[] commandArgs = new string[splitExpression.Length - 1];
                        for (int i = 1; i < splitExpression.Length; i++)
                        {
                            commandArgs[i - 1] = splitExpression[i];
                        }

                        anyValidNames = true;

                        GroupedCompositeItem compositeItem = null;

                        bool foundCommand = false;

                        bool compositeItemFound;
                        if ((compositeItem = InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(commandName, out compositeItemFound)) != null
                            && compositeItemFound)
                        {
                            CompositeItem<Command, List> commandToCall;
                            if ((commandToCall = compositeItem.TryGetCommand(commandName)) != null)
                            {
                                foundCommand = true;
                                if (!commandToCall.Item.TakesParameterCountOf(commandArgs.Length))
                                {
                                    string parameterCountFormat;
                                    if (commandArgs.Length == 1)
                                    {
                                        parameterCountFormat = Localization.MessageFormats.ParameterFormatSingular;
                                    }
                                    else
                                    {
                                        parameterCountFormat = Localization.MessageFormats.ParameterFormatPlural;
                                    }

                                    string parameterCount = String.Format(CultureInfo.CurrentCulture, parameterCountFormat, commandArgs.Length);

                                    this.InvokeOnMainThread(new ParameterlessVoid(delegate
                                    {
                                        if (onFailAllowUserToEdit)
                                        {
                                            if (UIMessageBox.Show(
                                                String.Format(Localization.MessageFormats.MultiCalledCommandInvalidParameterCount, commandToCall.Item.Name, command.Name, parameterCount),
                                                Localization.Promptu.AppName,
                                                UIMessageBoxButtons.YesNo,
                                                UIMessageBoxIcon.Error,
                                                UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                                            {
                                                this.ShowSetupDialog();
                                                this.setupDialog.EditCommand(command.Name);
                                            }
                                        }
                                        else
                                        {
                                            UIMessageBox.Show(
                                                String.Format(Localization.MessageFormats.MultiCalledCommandInvalidParameterCountNoEdit, commandToCall.Item.Name, command.Name, parameterCount),
                                                Localization.Promptu.AppName,
                                                UIMessageBoxButtons.OK,
                                                UIMessageBoxIcon.Error,
                                                UIMessageBoxResult.OK);
                                        }
                                    }));
                                }
                                else
                                {
                                    bool innerNameAndArgsRun;
                                    this.ExecuteCommand(true, commandToCall.Item, commandToCall.ListFrom, commandArgs, true, mode, alreadyCalledMultiCommandNames, out innerNameAndArgsRun);
                                }
                            }
                        }

                        if (!foundCommand)
                        {
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                            {
                                if (onFailAllowUserToEdit)
                                {
                                    if (UIMessageBox.Show(
                                        String.Format(Localization.MessageFormats.MultiCommandMissingCommand, commandName, command.Name),
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.YesNo,
                                        UIMessageBoxIcon.Error,
                                        UIMessageBoxResult.OK) == UIMessageBoxResult.Yes)
                                    {
                                        this.ShowSetupDialog();
                                        this.setupDialog.EditCommand(command.Name);
                                    }
                                }
                                else
                                {
                                    UIMessageBox.Show(
                                        String.Format(Localization.MessageFormats.MultiCommandMissingCommandNoEdit, commandName, command.Name),
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.OK,
                                        UIMessageBoxIcon.Error,
                                        UIMessageBoxResult.OK);
                                }
                            }));

                            return false;
                        }
                    }

                    if (!anyValidNames)
                    {
                        this.InvokeOnMainThread(new ParameterlessVoid(delegate
                        {
                            if (onFailAllowUserToEdit)
                            {
                                if (UIMessageBox.Show(
                                    String.Format(Localization.MessageFormats.EmptyMultiCommand, command.Name),
                                    Localization.Promptu.AppName,
                                    UIMessageBoxButtons.YesNo,
                                    UIMessageBoxIcon.Error,
                                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                                {
                                    this.ShowSetupDialog();
                                    this.setupDialog.EditCommand(command.Name);
                                }
                            }
                            else
                            {
                                UIMessageBox.Show(
                                    String.Format(Localization.MessageFormats.EmptyMultiCommandNoEdit, command.Name),
                                    Localization.Promptu.AppName,
                                    UIMessageBoxButtons.OK,
                                    UIMessageBoxIcon.Error,
                                    UIMessageBoxResult.OK);
                            }
                        }));

                        return false;
                    }

                    return true;
                default:
                    break;
            }

            // Added hook as per Jarrod Dixon's feature request with 
            // Chrome.Launch https://twitter.com/#!/jarrod_dixon/status/79037955599122433
            if (PromptuHookManager.RaiseCommandFullyResolvedExecuting(
                mode,
                //command, TODO enable this once API fully ready
                executing,
                arguments,
                startupDirectory) != HookAction.Return)
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(executing, arguments);
                    if (command.RunAsAdministrator)
                    {
                        startInfo.Verb = "runas";
                    }

                    startInfo.WindowStyle = command.StartingWindowState;
                    if (!String.IsNullOrEmpty(startupDirectory))
                    {
                        startInfo.WorkingDirectory = startupDirectory;
                    }
                    else if (command.UseExecutionDirectoryAsStartupDirectory)
                    {
                        FileSystemFile executingFile = executing;
                        if (executingFile.Exists)
                        {
                            startInfo.WorkingDirectory = executingFile.GetParentDirectory();
                        }
                    }

                    if (executing.Length > 0)
                    {
                        if (mode == ExecuteMode.ToClipboard)
                        {
                            this.InvokeOnMainThread(new ParameterlessVoid(delegate
                                {
                                    InternalGlobals.GuiManager.ToolkitHost.Clipboard.SetText(executing);
                                }));
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(startInfo);
                        }
                    }
                    else
                    {
                        //removed as per Jarrod Dixon's feature request https://twitter.com/#!/jarrod_dixon/status/78974764315713537
                        this.GiveExecuteCommandFailedMessage(executing, command, onFailAllowUserToEdit);
                    }
                }
                catch (Win32Exception ex)
                {
                    switch (ex.NativeErrorCode)
                    {
                        case 1223: // 1223 means UAC cancelled
                            break;
                        case 32: // locked by another process
                            this.GiveExecuteCommandFailedMessage(
                                executing,
                                command,
                                onFailAllowUserToEdit,
                                Localization.MessageFormats.CommandExecutionFailedLocked,
                                Localization.MessageFormats.CommandExecutionFailedLockedNoEdit);

                            break;
                        default:
                            this.GiveExecuteCommandFailedMessage(
                                executing,
                                command,
                                onFailAllowUserToEdit,
                                Localization.MessageFormats.CommandExecutionFailedWin32,
                                Localization.MessageFormats.CommandExecutionFailedWin32NoEdit);

                            break;
                    }

                    return false;
                }
                catch (System.IO.FileNotFoundException)
                {
                    this.GiveExecuteCommandFailedMessage(executing, command, onFailAllowUserToEdit);
                    return false;
                }
            }

            return true;
        }

        private void GiveExecuteCommandFailedMessage(string executing, Command command, bool allowUserToEdit, string regularMessage, string noEditMessage)
        {
            this.InvokeOnMainThread(new ParameterlessVoid(delegate
            {
                if (allowUserToEdit)
                {
                    if (UIMessageBox.Show(
                        String.Format(regularMessage, executing, command.Name),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        this.ShowSetupDialog();
                        this.setupDialog.EditCommand(command.Name);
                    }
                }
                else
                {
                    UIMessageBox.Show(
                        String.Format(noEditMessage, executing),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }
            }));
        }

        //private void GiveExecuteCommandFailedMessage(string executing, Command command, bool allowUserToEdit, int errorCode)
        //{
        //    this.InvokeOnMainThread(new ParameterlessVoid(delegate
        //    {
        //        if (allowUserToEdit)
        //        {
        //            if (UIMessageBox.Show(
        //                String.Format(Localization.MessageFormats.CommandExecutionFailedWin32, executing, command.Name, errorCode),
        //                Localization.Promptu.AppName,
        //                UIMessageBoxButtons.YesNo,
        //                UIMessageBoxIcon.Error,
        //                UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
        //            {
        //                this.ShowSetupDialog();
        //                this.setupDialog.EditCommand(command.Name);
        //            }
        //        }
        //        else
        //        {
        //            UIMessageBox.Show(
        //                String.Format(noEditMessage, executing),
        //                Localization.Promptu.AppName,
        //                UIMessageBoxButtons.OK,
        //                UIMessageBoxIcon.Error,
        //                UIMessageBoxResult.OK);
        //        }
        //    }));
        //}

        private void GiveExecuteCommandFailedMessage(string executing, Command command, bool allowUserToEdit)
        {
            this.InvokeOnMainThread(new ParameterlessVoid(delegate
            {
                if (allowUserToEdit)
                {
                    if (UIMessageBox.Show(
                        String.Format(Localization.MessageFormats.CommandExecutionFailed, executing, command.Name),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        this.ShowSetupDialog();
                        this.setupDialog.EditCommand(command.Name);
                    }
                }
                else
                {
                    UIMessageBox.Show(
                        String.Format(Localization.MessageFormats.CommandExecutionFailedNoEdit, executing),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }
            }));
        }

        private void GiveExecuteCommandFailedLockedMessage(string executing, Command command, bool allowUserToEdit)
        {
            this.InvokeOnMainThread(new ParameterlessVoid(delegate
            {
                if (allowUserToEdit)
                {
                    if (UIMessageBox.Show(
                        String.Format(Localization.MessageFormats.CommandExecutionFailedLocked, executing, command.Name),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.YesNo,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                    {
                        this.ShowSetupDialog();
                        this.setupDialog.EditCommand(command.Name);
                    }
                }
                else
                {
                    UIMessageBox.Show(
                        String.Format(Localization.MessageFormats.CommandExecutionFailedLockedNoEdit, executing),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }
            }));
        }

        private void GiveExecuteFailedMessage(string command)
        {
            this.InvokeOnMainThread(new ParameterlessVoid(delegate
            {
                UIMessageBox.Show(
                    String.Format(Localization.MessageFormats.AllExceptCommandExecutionFailed, command),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }));
        }

        public void OpenPrompt()
        {
            InternalGlobals.CurrentSkinInstance.Prompt.EnsureCreated();
            bool fullReset = false;

            if (this.promptClosedAt != null)
            {
                int difference = Environment.TickCount - this.promptClosedAt.Value;

                if (difference >= MilisecondsToResetPromptText)
                {
                    InternalGlobals.CurrentSkinInstance.Prompt.Text = String.Empty;
                    fullReset = true;
                }

                if (difference >= MilisecondsToRedetermineValidPathCommands)
                {
                    InternalGlobals.CurrentProfile.QueueCommandFileSystemValidations();
                }
            }

            //this.promptTextResetTimer.Stop();
            Point? position = null;
            //if (Globals.CurrentProfile.FollowMouse)
            //{
            //    //Point mousePosition = Control.MousePosition;
            //    //position = new Point(mousePosition.X, mousePosition.Y - this.prompt.PromptHeight);
            //}
            //else
            if (InternalGlobals.CurrentProfile.PromptPositioningMode == PositioningMode.None)
            {
                if (InternalGlobals.CurrentProfile.PromptLocation == null)
                {
                    InternalGlobals.CurrentProfile.PromptLocation = InternalGlobals.CurrentSkinInstance.Prompt.Location;
                }

                position = InternalGlobals.CurrentProfile.PromptLocation.Value;
            }

            //Point previousLocation = Globals.CurrentSkin.Prompt.Location;

            //this.prompt.OpenPrompt(position);
            InternalGlobals.CurrentSkinInstance.LayoutManager.PositionPrompt(
                this.CreatePositioningContext(),
                InternalGlobals.CurrentProfile.PromptPositioningMode,
                position);

            InternalGlobals.CurrentSkinInstance.Prompt.Show();
            InternalGlobals.CurrentSkinInstance.Prompt.Activate();
            this.suggestionHandler.NotifyPromptOpened(fullReset);
            InternalGlobals.CurrentSkinInstance.Prompt.FocusOnTextInput();
        }

        private PositioningContext CreatePositioningContext()
        {
            return new PositioningContext(
                    InternalGlobals.CurrentSkin,
                    InternalGlobals.CurrentSkinInstance,
                    new InfoBoxes(this.suggestionHandler.InformationBoxMananger)); 
        }

        public void ClosePrompt()
        {
            InternalGlobals.CurrentSkinInstance.Prompt.Hide();
            //if (this.prompt.TextualInput.InputText.Length > 0)
            //{
            //    this.promptTextResetTimer.Start();
            //}

            this.promptClosedAt = Environment.TickCount;
        }

        private void HandleHotkeyChanged(object sender, EventArgs e)
        {
            this.OnHotkeyChanged(e);
        }

        protected virtual void OnHotkeyChanged(EventArgs e)
        {
            EventHandler handler = this.HotkeyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnListsChanged(EventArgs e)
        {
            EventHandler handler = this.ListsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
