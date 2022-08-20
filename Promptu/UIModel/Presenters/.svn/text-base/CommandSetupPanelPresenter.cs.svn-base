using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.Collections;
using System.ComponentModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class CommandSetupPanelPresenter : SetupPanelPresenter<Command>
    {
        private FunctionCollectionComposite prioritizedFunctions;

        public CommandSetupPanelPresenter(ParameterlessVoid updateAllCallback, ParameterlessVoid settingChangedCallback)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSetupPanel(settingChangedCallback),
            updateAllCallback)
        {
        }

        public CommandSetupPanelPresenter(ISetupPanel uiObject, ParameterlessVoid updateAllCallback)
            : base(uiObject, updateAllCallback)
        {
            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.CommandSetupPanelNameHeader,
                this.GetCommandName);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.CommandSetupPanelExecutesHeader,
                this.GetCommandTarget);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.CommandSetupPanelArgumentsHeader,
                this.GetCommandArguments);

            this.CollectionPresenter.Headers.Add(
                Localization.UIResources.CommandSetupPanelNotesHeader,
                this.GetCommandNotes);

            //this.nameColumn.Text = "Name";
            //this.nameColumn.Width = 181;

            //this.executionPathColumn.Text = "Executes";
            //this.executionPathColumn.Width = 308;

            //this.argumentColumn.Text = "Arguments";
            //this.argumentColumn.Width = 72;

            //this.notesColumn.Text = "Notes";
            //this.notesColumn.Width = 400;

            //this.NativeInterface.NewButton.ToolTipText = Localization.UIResources.CommandSetupPanelNewToolTip;
            this.NativeInterface.NewButton.Image = InternalGlobals.GuiManager.ToolkitHost.Images.NewCommand;

            //this.NativeInterface.EditButton.ToolTipText = Localization.UIResources.CommandSetupPanelEditToolTip;
            //this.NativeInterface.DeleteButton.ToolTipText = Localization.UIResources.CommandSetupPanelDeleteToolTip;

            this.ItemCode = ItemCode.Command;
        }

        private object GetCommandName(object obj)
        {
            Command command = (Command)obj;
            return command.Name;
        }

        private object GetCommandTarget(object obj)
        {
            Command command = (Command)obj;
            return command.ExecutionPath;
        }

        private object GetCommandArguments(object obj)
        {
            Command command = (Command)obj;
            return command.Arguments;
        }

        private object GetCommandNotes(object obj)
        {
            Command command = (Command)obj;
            return command.Notes;
        }

        protected override void UpdateItemsListViewCore()
        {
            List<Command> sorted = new List<Command>();
            using (DdMonitor.Lock(this.ListUsing.Commands))
            {
                sorted.AddRange(this.ListUsing.Commands);
            }

            sorted.Sort(new Comparison<Command>(this.CommandComparison));
            foreach (Command command in sorted)
            {
                //List<string> values = new List<string>(this.CollectionPresenter.Headers.Count);
                //values.Add(command.Name);
                //values.Add(command.ExecutionPath);
                //values.Add(command.Arguments);
                //values.Add(command.Notes);

                this.CollectionPresenter.Add(command);
            }
        }

        protected override void UpdatedToCurrentList()
        {
            this.prioritizedFunctions = new FunctionCollectionComposite(InternalGlobals.CurrentProfile.Lists, this.ListUsing);
        }

        protected override string GetPluralItemDisplayFormat()
        {
            return Localization.UIResources.CommandCountPluralFormat;
        }

        protected override string GetSingularItemDisplayFormat()
        {
            return Localization.UIResources.CommandCountSingularFormat;
        }

        protected override Command GetItem(int index)
        {
            if (index >= 0 && index < this.CollectionPresenter.Count)
            {
                return (Command)this.CollectionPresenter[index];
            }

            return null;
        }

        private int CommandComparison(Command command1, Command command2)
        {
            return command1.Name.CompareTo(command2.Name);
        }

        protected override Command CreateNewItemCore(Command contents)
        {
            bool edited;
            return this.EditItem(new Command(Localization.Promptu.NewCommandName, String.Empty, String.Empty, false), null, true, contents, out edited);
        }

        protected override List<Command> DeleteSelectedItemsCore(bool silent)
        {
            List<Command> selectedItems = this.GetSelectedItems();

            if (selectedItems.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                if (selectedItems.Count == 1)
                {
                    message = message.AppendFormat(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteCommand, selectedItems[0].Name);
                }
                else
                {
                    message.AppendLine(Localization.MessageFormats.ConfirmDeleteCommands);
                    for (int i = 0; i < selectedItems.Count; i++)
                    {
                        message.Append(String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CommandNameDisplayFormat, selectedItems[i].Name));
                        if (i < selectedItems.Count - 1)
                        {
                            message.Append(", ");
                        }
                    }
                }

                if (silent || UIMessageBox.Show(
                    message.ToString(),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.YesNo,
                    UIMessageBoxIcon.Warning,
                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                {
                    //try
                    //{
                    InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                    //PromptuSettings.SyncSynchronizer.PauseSyncs();
                    int selectedIndex = this.CollectionPresenter.PrimarySelectedIndex;
                    foreach (Command command in selectedItems)
                    {
                        this.ListUsing.Commands.Remove(command);
                        command.RemoveEntriesFromHistory(InternalGlobals.CurrentProfile.History);
                    }

                    this.ListUsing.Commands.Save();
                    //this.OnCommandDeleted(new CommandDeletedEventArgs(command));
                    //this.OnCommandsChanged(EventArgs.Empty);
                    this.UpdateItemsListView();
                    if (this.CollectionPresenter.Count > 0)
                    {
                        if (selectedIndex >= this.CollectionPresenter.Count)
                        {
                            selectedIndex = this.CollectionPresenter.Count - 1;
                        }

                        this.CollectionPresenter.SelectedIndexes.Add(selectedIndex);
                        this.CollectionPresenter.EnsureVisible(selectedIndex);
                    }
                    //}
                    //finally
                    //{
                    //    PromptuSettings.SyncSynchronizer.UnPauseSyncs();
                    //}
                }
            }

            return selectedItems;
        }

        protected override Command EditSelectedItemCore()
        {
            if (this.CollectionPresenter.SelectedIndexes.Count > 0)
            {
                bool edited;
                Command command = this.GetSelectedItem();
                return this.EditItem(command, command.Name, false, null, out edited);
            }

            return null;
        }

        public void EditCommand(string name)
        {
            this.EditCommand(name, null);
        }

        public void EditCommand(string name, Command contents)
        {
            bool edited;
            Command command = this.EditItem(this.ListUsing.Commands[name], name, false, contents, out edited);
            if (edited)
            {
                this.OnItemEdited(new ItemEventArgs<Command>(command));
            }
        }

        private void HandleEditorClosing(object sender, CancelEventArgs e)
        {
            CommandEditorPresenter presenter = (CommandEditorPresenter)sender;
            Command assembled = presenter.AssembleCommand();
            TrieList conflicts = this.ListUsing.Commands.GetConflicts(assembled.Name, CaseSensitivity.Insensitive, GetConflictsMode.NoAliases, presenter.OriginalName);
            if (conflicts.Count > 0 && assembled.Name != presenter.OriginalName)
            {
                if (conflicts.Count == 1)
                {
                    UIMessageBox.Show(
                        String.Format(CultureInfo.CurrentCulture, Localization.Promptu.CommandNameAlreadyExists, conflicts[0]),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string confict in conflicts)
                    {
                        builder.AppendLine(confict);
                    }

                    UIMessageBox.Show(
                        String.Format(CultureInfo.CurrentCulture, Localization.Promptu.CommandNamesAlreadyExist, builder.ToString()),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }

                e.Cancel = true;
            }
            else if (Command.ReservedNames.Contains(assembled.Name.ToUpperInvariant()))
            {
                UIMessageBox.Show(
                    String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.NameIsReserved, assembled.Name),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);

                e.Cancel = true;
            }
        }

        private Command EditItem(Command command, string originalName, bool newCommand, Command contents, out bool edited)
        {
            edited = false;
            using (CommandEditorPresenter editor = new CommandEditorPresenter(
                this.ListUsing,
                command,
                newCommand,
                contents,
                this.UpdateAllCallback,
                this.prioritizedFunctions))
            {
                editor.OriginalName = originalName;
                editor.ClosingWithOk += this.HandleEditorClosing;
                //editor.ErrorListCollapsed = PromptuSettings.CurrentProfile.UISettings.CommandEditorSettings.ErrorListCollapsed;
                InternalGlobals.UISettings.CommandEditorSettings.ImpartTo(editor.NativeInterface);
               // while (true)
               // {
                if (editor.ShowDialog() == UIDialogResult.OK)
                {
                    InternalGlobals.UISettings.CommandEditorSettings.UpdateFrom(editor.NativeInterface);
                    Command assembled = editor.AssembleCommand();
                    //TrieList conflicts = this.ListUsing.Commands.GetConflicts(assembled.Name, CaseSensitivity.Insensitive, GetConflictsMode.NoAliases, originalName);
                    //if (conflicts.Count > 0 && assembled.Name != originalName)
                    //{
                    //    if (conflicts.Count == 1)
                    //    {
                    //        UIMessageBox.Show(
                    //            String.Format(Localization.Promptu.CommandNameAlreadyExists, conflicts[0]),
                    //            Localization.Promptu.AppName,
                    //            UIMessageBoxButtons.OK,
                    //            UIMessageBoxIcon.Error,
                    //            UIMessageBoxResult.OK);
                    //    }
                    //    else
                    //    {
                    //        StringBuilder builder = new StringBuilder();
                    //        foreach (string confict in conflicts)
                    //        {
                    //            builder.AppendLine(confict);
                    //        }

                    //        UIMessageBox.Show(
                    //            String.Format(Localization.Promptu.CommandNamesAlreadyExist, builder.ToString()),
                    //            Localization.Promptu.AppName,
                    //            UIMessageBoxButtons.OK,
                    //            UIMessageBoxIcon.Error,
                    //            UIMessageBoxResult.OK);
                    //    }
                    //    continue;
                    //}
                    //else if (Command.ReservedNames.Contains(assembled.Name.ToUpperInvariant()))
                    //{
                    //    UIMessageBox.Show(
                    //        String.Format(Localization.MessageFormats.NameIsReserved, assembled.Name),
                    //        Localization.Promptu.AppName,
                    //        UIMessageBoxButtons.OK,
                    //        UIMessageBoxIcon.Error,
                    //        UIMessageBoxResult.OK);
                    //    continue;
                    //}
                    //else
                    //{
                    //try
                    // {
                    InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                    //PromptuSettings.SyncSynchronizer.PauseSyncs();
                    if (originalName != assembled.Name && originalName != null)
                    {
                        command.RemoveEntriesFromHistory(InternalGlobals.CurrentProfile.History);
                        //this.ListUsing.SyncInfo.CommandIdentifierChanges.RemoveAllWithRevisedItem(command);
                        //this.ListUsing.SyncInfo.CommandIdentifierChanges.Add(new ZachJohnson.Promptu.UserModel.Differencing.CommandIdentifierChange(assembled, originalName));
                    }

                    CommandValidation validation = new CommandValidation(command, this.ListUsing, null);
                    InternalGlobals.CurrentProfile.BackgroundWorkQueue.AddWork(new ParameterlessVoid(validation.ValidateIsFileSystem));
                    //PromptuSettings.CurrentProfile.BackgroundWorkQueue.AddWork(

                    //assembled.IsFileSystemCommand(this.ListUsing, true);

                    if (assembled.Id == null)
                    {
                        assembled.Id = this.ListUsing.Commands.IdGenerator.GenerateId();
                    }

                    assembled.UpdateCacheIconAsync(this.ListUsing);

                    this.ListUsing.Commands.Remove(originalName);
                    this.ListUsing.Commands.Add(assembled);
                    this.ListUsing.Commands.Save();
                    // this.OnItemEdited(EventArgs.Empty);
                    this.UpdateItemsListView();
                    this.ClearSelectedIndices();
                    int indexOfItem = this.CollectionPresenter.IndexOf(assembled);
                    if (indexOfItem > -1)
                    {
                        this.CollectionPresenter.SelectedIndexes.Add(indexOfItem);
                        this.CollectionPresenter.EnsureVisible(indexOfItem);
                    }

                    edited = true;
                    //InternalGlobals.UISettings.CommandEditorSettings.UpdateFrom(editor.NativeInterface);
                    return assembled;
                    //}
                    //finally
                    //{
                    //    PromptuSettings.SyncSynchronizer.UnPauseSyncs();
                    //}
                    //}
                }

                    //break;
                //}

                //PromptuSettings.CurrentProfile.UISettings.CommandEditorSettings.ErrorListCollapsed = editor.ErrorListCollapsed;
                InternalGlobals.UISettings.CommandEditorSettings.UpdateFrom(editor.NativeInterface);
                return null;
            }
        }
    }
}
