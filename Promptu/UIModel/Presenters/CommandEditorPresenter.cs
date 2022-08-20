using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UserModel.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using ZachJohnson.Promptu.Skins;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;
using ZachJohnson.Promptu.Itl;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class CommandEditorPresenter : DialogPresenterBase<ICommandEditor>, IDisposable
    {
        private const int SecondsToValidationAfterLastChange = 2;
        private ValidationManager validationManager;
        private List currentList;
        private FunctionCollectionComposite prioritizedFunctions;
        private string executesFeedbackLocation;
        private string argumentsFeedbackLocation;
        private string startupDirectoryFeedbackLocation;
        private Command command;
        private bool newCommand;
        //private string originalExecutes;
        private CommandParameterMetaInfoPanelPresenter metaInfoPanel;
        private ErrorPanelPresenter errorPanel;
        private int? maximumNumberOfParameters;
        private bool invalidName;

        public CommandEditorPresenter(
            List currentList,
            Command command,
            bool newCommand,
            Command contents,
            ParameterlessVoid updateAllCallback,
            FunctionCollectionComposite prioritizedFunctions)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructCommandEditor(),
            currentList,
            command,
            newCommand,
            contents,
            updateAllCallback,
            prioritizedFunctions)
        {
        }

        public CommandEditorPresenter(
            ICommandEditor nativeInterface, 
            List currentList, 
            Command command, 
            bool newCommand,
            Command contents,
            ParameterlessVoid updateAllCallback, 
            FunctionCollectionComposite prioritizedFunctions)
            : base(nativeInterface)
        {
            this.newCommand = newCommand;
            this.NativeInterface.NameLabelText = Localization.UIResources.CommandEditorNameLabelText;
            this.NativeInterface.ExecutesLabelText = Localization.UIResources.CommandEditorExecutesLabelText;
            this.NativeInterface.ArgumentsLabelText = Localization.UIResources.CommandEditorArgumentsLabelText;
            this.NativeInterface.RunAsAdmin.Text = Localization.UIResources.CommandEditorRunAsAdminText;
            this.NativeInterface.StartupStateLabelText = Localization.UIResources.CommandEditorStartingWindowStateLabelText;

            this.NativeInterface.StartupState.Values = new string[] {
            Localization.UIResources.StartingWindowStateNormal,
            Localization.UIResources.StartingWindowStateMinimized,
            Localization.UIResources.StartingWindowStateMaximized };

            this.NativeInterface.WorkingDirectoryLabelText = Localization.UIResources.CommandEditorStartupDirectoryLabelText;
            this.NativeInterface.NotesLabelText = Localization.UIResources.CommandEditorNotesLabelText;
            this.NativeInterface.ShowParamHistory.Text = Localization.UIResources.CommandEditorShowParamHistory;
            this.NativeInterface.TestButton.Text = Localization.UIResources.CommandEditorTestButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.ViewAvailableFunctionsButton.Text = Localization.UIResources.CommandEditorViewAvailableFunctionsText;
            this.NativeInterface.Text = Localization.UIResources.CommandEditorText;
            this.NativeInterface.MainInstructions = Localization.UIResources.CommandEditorMainInstructions;

            this.metaInfoPanel = new CommandParameterMetaInfoPanelPresenter(
                this.NativeInterface.CommandParameterMetaInfoPanel,
                updateAllCallback,
                prioritizedFunctions);

            this.errorPanel = new ErrorPanelPresenter(this.NativeInterface.ErrorPanel);

            this.validationManager = new ValidationManager(SecondsToValidationAfterLastChange);
            this.validationManager.TimeToValidate += this.ValidateItl;

            this.NativeInterface.EyedropperToolTip = Localization.UIResources.CommandEditorDropperToolTip;

            this.NativeInterface.Name.Cue = Localization.UIResources.CommandEditorNameCue;
            this.NativeInterface.Target.Cue = Localization.UIResources.CommandEditorTargetCue;

            this.NativeInterface.GuessWorkingDirectory.Text = Localization.UIResources.CommandEditorGuessWorkingDirectory;

            this.NativeInterface.RestartPromptu = this.RestartPromptu;

            Command baseCommand = contents == null ? command : contents;

            this.NativeInterface.Name.Text = baseCommand.Name;
            this.NativeInterface.Name.TextValidator = this.ValidateNameText;
            this.NativeInterface.Target.Text = baseCommand.ExecutionPath;
            this.NativeInterface.Target.TextChanged += this.HandleExecutesTextChanged;
            this.NativeInterface.Arguments.Text = baseCommand.Arguments;
            this.NativeInterface.Arguments.TextChanged += this.HandleArgumentsTextChanged;
            // I18N: fix so not based on string value
            this.NativeInterface.StartupState.SelectedValue = baseCommand.StartingWindowState.ToString();
            this.NativeInterface.WorkingDirectory.Text = baseCommand.StartupDirectory;
            this.NativeInterface.WorkingDirectory.TextChanged += this.HandleStartupDirectoryTextChanged;
            this.NativeInterface.RunAsAdmin.Checked = baseCommand.RunAsAdministrator;
            this.NativeInterface.Notes.Text = baseCommand.Notes;
            this.NativeInterface.Name.KeyDown += this.HandleNameKeyDown;
            this.metaInfoPanel.AssignItems(baseCommand.ParametersMetaInfo);
            this.NativeInterface.Name.TextChanged += this.HandleNameAndExecutesTextChanged;

            this.NativeInterface.ShowParamHistory.Checked = baseCommand.ShowParameterHistory;
            this.NativeInterface.ShowParamHistory.CheckedChanged += this.HandleShowParamHistoryChanged;

            this.prioritizedFunctions = prioritizedFunctions;

            this.NativeInterface.GuessWorkingDirectory.Checked = baseCommand.UseExecutionDirectoryAsStartupDirectory;

            this.argumentsFeedbackLocation = Localization.UIResources.ArgumentsFeedbackLocation;
            this.executesFeedbackLocation = Localization.UIResources.ExecutesFeedbackLocation;
            this.startupDirectoryFeedbackLocation = Localization.UIResources.StartupDirectoryFeedbackLocation;

            this.errorPanel.MessageSelected += this.HandleMessageSelected;

            this.command = command;

            this.currentList = currentList;

            if (newCommand)
            {
                this.NativeInterface.OkButton.Enabled = false;
                this.NativeInterface.TestButton.Enabled = false;
            }

            this.NativeInterface.TestButton.Click += this.HandleTestButtonClick;
            this.NativeInterface.OkButton.Click += this.HandleOkButtonClick;

            this.ValidateItl();

            this.NativeInterface.ViewAvailableFunctionsButton.Click += this.HandleViewAvailableFunctionsClick;
            //this.NativeInterface.Closing += this.HandleClosing;
            //this.NativeInterface.ViewAvailableFunctionsButton.Visible = false;

            this.UpdateEnableChangeableUIElements();
        }

        public event CancelEventHandler ClosingWithOk;

        public string OriginalName
        {
            get;
            set;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.validationManager.Dispose();
            this.ClosingWithOk = null;
        }

        //override 
        //    this.validationManager.Dispose();
        //    IDisposable disposableNativeInterface = this.NativeInterface as IDisposable;

        //    if (disposableNativeInterface != null)
        //    {
        //        disposableNativeInterface.Dispose();
        //    }
        //}

        private void HandleShowParamHistoryChanged(object sender, EventArgs e)
        {
            this.metaInfoPanel.DefaultShowHistory = this.NativeInterface.ShowParamHistory.Checked;
        }

        private ValueValidationResult ValidateNameText(string value)
        {
            if (value.Length <= 0 || value == null)
            {
                return new ValueValidationResult(true, null);
            }

            string[] aliases = Command.GetAliasesFromName(value);

            foreach (string alias in aliases)
            {
                for (int i = 0; i < alias.Length; i++)
                {
                    char c = alias[i];
                    if (i == 0 && c == '.')
                    {
                        this.SetInvalidName(true);
                        string message = aliases.Length > 1 ?
                            Localization.MessageFormats.CommandNameInvalidDotAtStartAlias :
                            Localization.MessageFormats.CommandNameInvalidDotAtStartName;
                        return new ValueValidationResult(false, message);
                    }

                    if (Command.IllegalNameChars.Contains(c))
                    {
                        this.SetInvalidName(true);
                        string message;
                        if (c == ' ')
                        {
                            message = Localization.MessageFormats.CommandNameNoSpaces;
                        }
                        else
                        {
                            message = String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CommandNameInvalidChar, c);
                        }

                        return new ValueValidationResult(false, message);
                    }
                }
            }

            this.SetInvalidName(false);
            return new ValueValidationResult(true, null);
        }

        private void SetInvalidName(bool value)
        {
            this.invalidName = value;
            this.UpdateEnableChangeableUIElements();
        }

        private void HandleNameKeyDown(object sender, KeyEventArgs e)
        {
            //this.NativeInterface.Name.ClearTextValidationError();
            switch (e.KeyCode)
            {
                case Keys.Back:
                case Keys.Escape:
                    return;
                case Keys.Enter:
                case Keys.Space:
                    e.SuppressKeyPress = true;
                    break;
                default:
                    //char c;

                    //try
                    //{
                    //    c = Globals.GuiManager.ToolkitHost.Keyboard.ConvertToChar(e.KeyCode);
                    //}
                    //catch (ArgumentException)
                    //{
                    //    return;
                    //}

                    //string message = null;
                    //bool valid = true;

                    //if (Command.IllegalNameChars.Contains(c))
                    //{
                    //    valid = false;
                    //    message = String.Format(Localization.MessageFormats.CommandNameInvalidChar, c);
                    //}

                    //if (c == '.')
                    //{
                    //    int indexBefore = this.NativeInterface.Name.SelectionStart - 1;
                    //    if ((indexBefore >= 0 && this.NativeInterface.Name.Text[indexBefore] == ',') || indexBefore == -1)
                    //    {
                    //        valid = false;
                    //        message = Localization.MessageFormats.CommandNameInvalidDotAtStart;
                    //    }
                    //}

                    //if (!valid)
                    //{
                    //    //this.NativeInterface.Name.GiveTextValidationError(message);
                    //}

                    //e.SuppressKeyPress = !valid;
                    break;
            }
        }

        private void HandleExecutesTextChanged(object sender, EventArgs e)
        {
            this.validationManager.NotifyChangeHappened();
            this.HandleNameAndExecutesTextChanged(sender, e);
        }

        private void HandleArgumentsTextChanged(object sender, EventArgs e)
        {
            this.validationManager.NotifyChangeHappened();
        }

        private void HandleStartupDirectoryTextChanged(object sender, EventArgs e)
        {
            this.validationManager.NotifyChangeHappened();
        }

        private void HandleNameAndExecutesTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnableChangeableUIElements();
        }

        private void UpdateEnableChangeableUIElements()
        {
            this.NativeInterface.OkButton.Enabled = !this.invalidName &&
                !this.NativeInterface.Name.CueDisplayed && 
                this.NativeInterface.Name.Text.Length > 0 && 
                !this.NativeInterface.Target.CueDisplayed && 
                this.NativeInterface.Target.Text.Length > 0;

            this.NativeInterface.TestButton.Enabled =
                !this.NativeInterface.Target.CueDisplayed &&
                this.NativeInterface.Target.Text.Length > 0;
        }

        private void HandleOkButtonClick(object sender, EventArgs e)
        {
            string nameText = this.NativeInterface.Name.Text;
            if (nameText.Contains(" "))
            {
                UIMessageBox.Show(
                    Localization.MessageFormats.CommandNameNoSpaces,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);

                return;
            }

            foreach (char aliasSplitChar in Command.AliasSplitChars)
            {
                if (nameText.StartsWith(new string(aliasSplitChar, 1)))
                {
                    UIMessageBox.Show(
                        String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CommandNameCharacterCannotBeAtStart, aliasSplitChar),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);

                    return;
                }
                else if (nameText.EndsWith(new string(aliasSplitChar, 1)))
                {
                    UIMessageBox.Show(
                        String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CommandNameCharacterCannotBeAtEnd, aliasSplitChar),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);

                    return;
                }
            }

            string[] aliases = this.NativeInterface.Name.Text.Split(Command.AliasSplitChars);
            foreach (string alias in aliases)
            {
                if (alias.EndsWith("."))
                {
                    string message = aliases.Length > 1 ? 
                        Localization.MessageFormats.CommandNameInvalidDotAtEndAlias :
                        Localization.MessageFormats.CommandNameInvalidDotAtEndName;

                    UIMessageBox.Show(
                        message,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);

                    return;
                }
                else if (alias.StartsWith("."))
                {
                    string message = aliases.Length > 1 ?
                        Localization.MessageFormats.CommandNameInvalidDotAtStartAlias :
                        Localization.MessageFormats.CommandNameInvalidDotAtStartName;

                    UIMessageBox.Show(
                        message,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                    return;
                }
            }

            CancelEventArgs eventArgs = new CancelEventArgs();
            this.OnClosingWithOk(eventArgs);

            if (!eventArgs.Cancel)
            {
                this.NativeInterface.CloseWithOK();
            }
        }

        private void HandleViewAvailableFunctionsClick(object sender, EventArgs e)
        {
            FunctionViewerPresenter viewer = new FunctionViewerPresenter(InternalGlobals.CurrentProfile, this.currentList);
            InternalGlobals.UISettings.FunctionViewerSettings.ImpartTo(viewer.NativeInterface);
            viewer.Show(this.NativeInterface);
            viewer.Closed += this.HandleFunctionViewerClosed;
            this.NativeInterface.ViewAvailableFunctionsButton.Enabled = false;
        }

        private void HandleFunctionViewerClosed(object sender, EventArgs e)
        {
            this.NativeInterface.ViewAvailableFunctionsButton.Enabled = true;
            FunctionViewerPresenter functionViewer = sender as FunctionViewerPresenter;
            if (functionViewer != null)
            {
                InternalGlobals.UISettings.FunctionViewerSettings.UpdateFrom(functionViewer.NativeInterface);
            }
        }

        public Command AssembleCommand()
        {
            return new Command(
                this.NativeInterface.Name.Text,
                this.NativeInterface.Target.Text,
                this.NativeInterface.Arguments.Text,
                this.NativeInterface.RunAsAdmin.Checked,
                // I18N: fix
                (ProcessWindowStyle)Enum.Parse(typeof(ProcessWindowStyle), this.NativeInterface.StartupState.Text),
                this.NativeInterface.Notes.Text,
                this.NativeInterface.WorkingDirectory.Text,
                this.NativeInterface.ShowParamHistory.Checked,
                this.NativeInterface.GuessWorkingDirectory.Checked,
                this.command.Id,
                this.metaInfoPanel.AssembleItems());
        }

        private void HandleTestButtonClick(object sender, EventArgs e)
        {
            if (!this.ValidateItl())
            {
                UIMessageBox.Show(
                    Localization.MessageFormats.ItlMustBeFixedBeforeTest,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
                return;
            }

            Command command = this.AssembleCommand();
            string arguments = String.Empty;
            int? maxParameters = command.GetMaximumNumberOfParameters();
            if (maxParameters == null || maxParameters > 0)
            {
                ArgumentDialogPresenter dialog = new ArgumentDialogPresenter();
                if (dialog.ShowDialog() == UIDialogResult.Cancel)
                {
                    return;
                }

                arguments = dialog.Arguments;
            }

            string[] args;
            if (arguments.Length > 0)
            {
                args = arguments.Split(' ');
            }
            else
            {
                args = new string[0];
            }

            bool nameAndArgsRun;
            bool executed = PromptHandler.GetInstance().ExecuteCommand(
                false,
                command,
                this.currentList,
                args,
                false,
                ExecuteMode.Default,
                new TrieList(SortMode.DecendingFromLastAdded),
                out nameAndArgsRun);

            if (executed)
            {
                string message;
                if (InternalGlobals.CurrentProfile.UseSentimentalSlickRunFeatures)
                {
                    message = Localization.Promptu.TestCommandSuceededMessageForSlickRunUsers;
                }
                else
                {
                    message = Localization.Promptu.TestCommandSuceededMessageDefault;
                }

                UIMessageBox.Show(
                    message,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.None,
                    UIMessageBoxResult.OK);
            }
        }

        public void ValidateFunctionsExist(Expression expression, FeedbackCollection feedback)
        {
            if (expression is ExpressionGroup)
            {
                foreach (Expression innerExpression in ((ExpressionGroup)expression).Expressions)
                {
                    this.ValidateFunctionsExist(innerExpression, feedback);
                }
            }
            else if (expression is FunctionCall)
            {
                FunctionCall functionCall = expression as FunctionCall;
                if (!this.prioritizedFunctions.ContainsAnyNamed(functionCall.Identifier.Name, ReturnValue.String))
                {
                    feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.MissingStringFunction, functionCall.Identifier.Name));
                }
                else if (!this.prioritizedFunctions.Contains(functionCall.Identifier.Name, ReturnValue.String, functionCall.GetParameterSignature()))
                {
                    if (functionCall.Parameters.Count == 1)
                    {
                        feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.InvalidParameterCountSingular, functionCall.Identifier.Name, functionCall.Parameters.Count));
                    }
                    else
                    {
                        feedback.AddError(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.InvalidParameterCountPlural, functionCall.Identifier.Name, functionCall.Parameters.Count));
                    }
                }

                foreach (Expression innerExpression in functionCall.Parameters)
                {
                    this.ValidateFunctionsExist(innerExpression, feedback);
                }
            }
            else if (expression is OptionalSubsitution)
            {
                OptionalSubsitution substitution = expression as OptionalSubsitution;

                if (substitution.DefaultValue != null)
                {
                    this.ValidateFunctionsExist(substitution.DefaultValue, feedback);
                }
            }
        }

        private void ValidateItl(object sender, EventArgs e)
        {
            this.ValidateItl();
        }

        private bool ValidateItl()
        {
            ItlCompiler compiler = new ItlCompiler();

            FeedbackCollection executesFeedback = null;
            FeedbackCollection argumentsFeedback = null;
            FeedbackCollection startupDirectoryFeedback = null;

            bool justText = true;

            //int? mininumNumberOfParameters = null;
            int? maximumNumberOfParameters = 0;

            if (this.NativeInterface.Target.Text.Length > 0)
            {
                Expression compiledExecutes = compiler.Compile(ItlType.Standard, this.NativeInterface.Target.Text, out executesFeedback, out justText);
                this.ValidateFunctionsExist(compiledExecutes, executesFeedback);
                //Command.GetMininumNumberOfParameters(compiledExecutes, ref mininumNumberOfParameters);
                Command.GetMaximumNumberOfParameters(compiledExecutes, ref maximumNumberOfParameters);
            }

            if (this.NativeInterface.Arguments.Text.Length > 0)
            {
                bool argumentsJustText;
                Expression compiledArguments = compiler.Compile(ItlType.Standard, this.NativeInterface.Arguments.Text, out argumentsFeedback, out argumentsJustText);
                this.ValidateFunctionsExist(compiledArguments, argumentsFeedback);
                if (!argumentsJustText)
                {
                    justText = argumentsJustText;
                }

                //Command.GetMininumNumberOfParameters(compiledArguments, ref mininumNumberOfParameters);
                Command.GetMaximumNumberOfParameters(compiledArguments, ref maximumNumberOfParameters);
            }

            if (this.NativeInterface.WorkingDirectory.Text.Length > 0)
            {
                bool startupDirectoryJustText;
                Expression compiledStartupDirectory = compiler.Compile(ItlType.Standard, this.NativeInterface.WorkingDirectory.Text, out startupDirectoryFeedback, out startupDirectoryJustText);
                this.ValidateFunctionsExist(compiledStartupDirectory, argumentsFeedback);
                if (!startupDirectoryJustText)
                {
                    justText = startupDirectoryJustText;
                }

                //Command.GetMininumNumberOfParameters(compiledArguments, ref mininumNumberOfParameters);
                Command.GetMaximumNumberOfParameters(compiledStartupDirectory, ref maximumNumberOfParameters);
            }

            //this.mininumNumberOfParameters = mininumNumberOfParameters == null ? 0 : mininumNumberOfParameters.Value;
            this.maximumNumberOfParameters = maximumNumberOfParameters;

            FeedbackCollection totalFeedback = new FeedbackCollection();

            if (executesFeedback != null)
            {
                foreach (FeedbackMessage message in executesFeedback)
                {
                    message.Location = this.executesFeedbackLocation;
                    totalFeedback.Add(message);
                }
            }

            if (argumentsFeedback != null)
            {
                foreach (FeedbackMessage message in argumentsFeedback)
                {
                    message.Location = this.argumentsFeedbackLocation;
                    totalFeedback.Add(message);
                }
            }

            if (startupDirectoryFeedback != null)
            {
                foreach (FeedbackMessage message in startupDirectoryFeedback)
                {
                    message.Location = this.startupDirectoryFeedbackLocation;
                    totalFeedback.Add(message);
                }
            }

            bool hasErrors = totalFeedback.Has(FeedbackType.Error);

            //if (!this.Disposing && this.IsHandleCreated)
            if (this.NativeInterface.IsCreatedAndNotDisposing)
            {
                try
                {
                    this.NativeInterface.Invoke(new ParameterlessVoid(delegate
                    {
                        this.UpdateParameterObjectsVisiblity();
                        this.errorPanel.SetMessages(totalFeedback, false);
                        if (hasErrors)
                        {
                            this.NativeInterface.EnsureErrorListVisible();
                            //this.ErrorListCollapsed = false;
                        }

                        //this.ErrorListVisible = !justText;
                        this.NativeInterface.MustShowErrorList = !justText;
                        this.NativeInterface.ViewAvailableFunctionsButton.Visible = !justText;
                    }), null);
                }
                catch (ObjectDisposedException)
                {
                }
            }

            return !hasErrors;
        }

        private void HandleMessageSelected(object sender, MessageSelectedEventArgs e)
        {
            if (e.Message.CanLocate)
            {
                ITextInput textInputToSelect = null;
                if (e.Message.Location == this.argumentsFeedbackLocation)
                {
                    textInputToSelect = this.NativeInterface.Arguments;
                }
                else if (e.Message.Location == this.executesFeedbackLocation)
                {
                    textInputToSelect = this.NativeInterface.Target;
                }
                else if (e.Message.Location == this.startupDirectoryFeedbackLocation)
                {
                    textInputToSelect = this.NativeInterface.WorkingDirectory;
                }

                if (textInputToSelect != null)
                {
                    textInputToSelect.SelectionStart = e.Message.Start;
                    textInputToSelect.SelectionLength = e.Message.Length;
                    textInputToSelect.Select();
                }
            }
        }

        private void RestartPromptu()
        {
            try
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();

                document.AppendChild(this.AssembleCommand().ToXml(document));

                string tempFile = Path.GetTempFileName();

                document.Save(tempFile);

                ProcessStartInfo startInfo = new ProcessStartInfo(Application.ExecutablePath, String.Format(
                    CultureInfo.InvariantCulture, 
                    "wait-for {0} edit {1}:{2} {3}",
                    System.Diagnostics.Process.GetCurrentProcess().Id, 
                    currentList.FolderName, 
                    this.newCommand ? "new" : command.Id.ToString(),
                    tempFile));

                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                switch (ex.NativeErrorCode)
                {
                    case 1223: // 1223 means UAC cancelled
                        return;
                    default:
                        UIMessageBox.Show(
                            Localization.UIResources.ErrorRestartingPromptu,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);

                        break;
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                UIMessageBox.Show(
                    Localization.UIResources.ErrorRestartingPromptu,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }

            PromptuUtilities.ExitApplication();
        }

        private void UpdateParameterObjectsVisiblity()
        {
            bool showParameterObjects = this.maximumNumberOfParameters == null || this.maximumNumberOfParameters > 0;
            this.NativeInterface.SetUIForParameters(showParameterObjects);
            //this.metaInfoPanel.Visible = showParameterObjects;
            //this.showParamHistory.Visible = showParameterObjects;
        }

        //protected void HandleClosing(object sender, CancelEventArgs e)
        //{
        //    this.OnClosing(e);
        //}

        protected virtual void OnClosingWithOk(CancelEventArgs e)
        {
            CancelEventHandler handler = this.ClosingWithOk;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
