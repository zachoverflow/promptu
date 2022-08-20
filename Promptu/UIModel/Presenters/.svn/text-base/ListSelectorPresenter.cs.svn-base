using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;
using System.IO;
using ZachJohnson.Promptu.UserModel.Differencing;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ListSelectorPresenter : PresenterBase<IListSelector>
    {
        private ParameterlessVoid syncListsMethod;
        private IOpenFileDialog importDialog;
        private IOpenFileDialog importOrLinkDialog;
        private ISaveFileDialog publishDialog;

        private UIContextMenu listContextMenu;
        private UIMenuItemInternal contextEnable;
        private UIMenuItemInternal contextDisable;
        private UIMenuSeparator contextEnableDisableSeparator;
        private UIMenuItemInternal contextRename;
        private UIMenuSeparatorInternal contextRenameSeparator;
        private UIMenuItemInternal contextUnsubscribe;
        private UIMenuSeparatorInternal contextUnsubscribeSeparator;
        private UIMenuItemInternal contextPublish;
        private UIMenuItemInternal contextExport;
        private UIMenuSeparatorInternal contextPublishExportSeparator;
        private UIMenuItemInternal contextDelete;

        private bool synchronizing;

        public ListSelectorPresenter(IListSelector nativeInterface)
            : base(nativeInterface)
        {
            this.syncListsMethod = new ParameterlessVoid(this.SyncLists);

            this.importDialog = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOpenFileDialog();
            this.importDialog.Filter = Localization.UIResources.SlickRunImportDialogFilter;
            this.importDialog.Text = Localization.UIResources.SlickRunImportDialogTitle;

            this.publishDialog = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSaveFileDialog();
            this.publishDialog.Filter = Localization.UIResources.PublishDialogFilter;
            this.publishDialog.Text = Localization.UIResources.PublishDialogTitleForPublish;

            this.importOrLinkDialog = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOpenFileDialog();
            this.importOrLinkDialog.Filter = Localization.UIResources.ImportOrLinkDialogFilter;

            this.listContextMenu = new UIContextMenu();
            this.NativeInterface.ListContextMenu = this.listContextMenu.NativeContextMenuInterface;

            this.contextEnable = new UIMenuItemInternal(
                "Promptu.EnableList",
                Localization.UIResources.ContextEnableText,
                Localization.UIResources.ContextEnableToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.EnableList,
                new EventHandler(this.ToggleSelectedListEnabled));

            this.listContextMenu.Items.Add(this.contextEnable);

            this.contextDisable = new UIMenuItemInternal(
                "Promptu.DisableList",
                Localization.UIResources.ContextDisableText,
                Localization.UIResources.ContextDisableToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.DisableList,
                new EventHandler(this.ToggleSelectedListEnabled));

            this.listContextMenu.Items.Add(this.contextDisable);

            this.contextEnableDisableSeparator = new UIMenuSeparatorInternal("Promptu.EnableDisableSeparator");
            this.listContextMenu.Items.Add(this.contextEnableDisableSeparator);

            this.contextRename = new UIMenuItemInternal(
                "Promptu.RenameList",
                Localization.UIResources.ContextRenameText,
                Localization.UIResources.RenameListToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.RenameList,
                new EventHandler(this.RenameSelectedList));

            this.listContextMenu.Items.Add(this.contextRename);

            this.contextRenameSeparator = new UIMenuSeparatorInternal("Promptu.RenameSeparator");
            this.listContextMenu.Items.Add(this.contextRenameSeparator);

            this.contextUnsubscribe = new UIMenuItemInternal(
                "Promptu.UnsubscribeList",
                Localization.UIResources.ContextUnsubscribeText,
                Localization.UIResources.ContextUnsubscribeToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.UnsubscribeList,
                new EventHandler(this.UnsubscribeList));

            this.listContextMenu.Items.Add(this.contextUnsubscribe);

            this.contextUnsubscribeSeparator = new UIMenuSeparatorInternal("Promptu.UnsubscribeSeparator");
            this.listContextMenu.Items.Add(this.contextUnsubscribeSeparator);

            this.contextPublish = new UIMenuItemInternal(
                "Promptu.PublishList",
                Localization.UIResources.PublishListText,
                Localization.UIResources.PublishListToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.PublishList,
                new EventHandler(this.PublishList));

            this.listContextMenu.Items.Add(this.contextPublish);

            this.contextExport = new UIMenuItemInternal(
                "Promptu.ExportList",
                Localization.UIResources.ExportText,
                Localization.UIResources.ExportToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.ExportList,
                new EventHandler(this.ExportClick));

            this.listContextMenu.Items.Add(this.contextExport);

            this.contextPublishExportSeparator = new UIMenuSeparatorInternal("Promptu.PublishExportSeparator");
            this.listContextMenu.Items.Add(this.contextPublishExportSeparator);

            this.contextDelete = new UIMenuItemInternal(
                "Promptu.DeleteList",
                Localization.UIResources.ContextDeleteText,
                Localization.UIResources.DeleteListToolTip,
                InternalGlobals.GuiManager.ToolkitHost.Images.Delete,
                new EventHandler(this.DeleteSelectedList));

            this.listContextMenu.Items.Add(this.contextDelete);

            this.NativeInterface.ListsKeyDown += this.HandleListsKeyDown;
            this.NativeInterface.SelectedIndexChanged += this.ForwardSelectedItemChanged;

            this.NativeInterface.NoListsMessageText = Localization.UIResources.NoListsMessage;

            this.NativeInterface.SyncButton.ToolTipText = Localization.UIResources.SyncToolTip;
            this.NativeInterface.NewListButton.ToolTipText = Localization.UIResources.NewListToolTip;
            this.NativeInterface.NewEmptyListMenuItem.Text = Localization.UIResources.EmptyListText;
            this.NativeInterface.NewEmptyListMenuItem.ToolTipText = Localization.UIResources.EmptyListToolTip;
            this.NativeInterface.NewDefaultListMenuItem.Text = Localization.UIResources.DefaultListText;
            this.NativeInterface.NewDefaultListMenuItem.ToolTipText = Localization.UIResources.DefaultListToolTip;
            this.NativeInterface.LinkMenuItem.ToolTipText = Localization.UIResources.LinkToolTip;
            this.NativeInterface.LinkMenuItem.Text = Localization.UIResources.LinkText;
            this.NativeInterface.ImportMenuItem.ToolTipText = Localization.UIResources.ImportToolTip;
            this.NativeInterface.ImportMenuItem.Text = Localization.UIResources.ImportText;
            this.NativeInterface.SlickRunImportMenuItem.ToolTipText = Localization.UIResources.SlickRunImportToolTip;
            this.NativeInterface.SlickRunImportMenuItem.Text = Localization.UIResources.SlickRunImportText;
            this.NativeInterface.PublishMenuItem.ToolTipText = Localization.UIResources.PublishListToolTip;
            this.NativeInterface.PublishMenuItem.Text = Localization.UIResources.PublishListText;
            this.NativeInterface.ExportMenuItem.ToolTipText = Localization.UIResources.ExportToolTip;
            this.NativeInterface.ExportMenuItem.Text = Localization.UIResources.ExportText;
            this.NativeInterface.EnableButton.ToolTipText = Localization.UIResources.EnableButtonToolTip;
            this.NativeInterface.DisableButton.ToolTipText = Localization.UIResources.DisableButtonToolTip;
            this.NativeInterface.UnsubscribeButton.ToolTipText = Localization.UIResources.UnsubscribeToolTip;
            this.NativeInterface.RenameButton.ToolTipText = Localization.UIResources.RenameListToolTip;
            this.NativeInterface.DeleteButton.ToolTipText = Localization.UIResources.DeleteListToolTip;
            this.NativeInterface.MoveListUpButton.ToolTipText = Localization.UIResources.MoveListUpToolTip;
            this.NativeInterface.MoveListDownButton.ToolTipText = Localization.UIResources.MoveListDownToolTip;

            this.NativeInterface.SyncButton.Click += this.SyncListsAsync;
            this.NativeInterface.NewListButton.ButtonClick += this.AddEmptyList;
            this.NativeInterface.NewEmptyListMenuItem.Click += this.AddEmptyList;
            this.NativeInterface.NewDefaultListMenuItem.Click += this.AddDefaultList;
            this.NativeInterface.LinkMenuItem.Click += this.LinkList;
            this.NativeInterface.ImportMenuItem.Click += this.AppendList;
            this.NativeInterface.SlickRunImportMenuItem.Click += this.ImportButtonClick;
            this.NativeInterface.PublishMenuItem.Click += this.PublishList;
            this.NativeInterface.ExportMenuItem.Click += this.ExportClick;
            this.NativeInterface.EnableButton.Click += this.ToggleSelectedListEnabled;
            this.NativeInterface.DisableButton.Click += this.ToggleSelectedListEnabled;
            this.NativeInterface.UnsubscribeButton.Click += this.UnsubscribeList;
            this.NativeInterface.RenameButton.Click += this.RenameSelectedList;
            this.NativeInterface.DeleteButton.Click += this.DeleteSelectedList;
            this.NativeInterface.MoveListUpButton.Click += this.MoveListUp;
            this.NativeInterface.MoveListDownButton.Click += this.MoveListDown;

            this.NativeInterface.ListDragEnter += this.HandleListDragEnter;
            this.NativeInterface.ListDrop += this.HandleListDrop;

            //this.NativeInterface.ListPrecedenceHintToolTipText = Localization.UIResources.ListSearchOrderToolTip;
            this.NativeInterface.Title = Localization.UIResources.CommandListSelectorLabelText;

            this.UpdateLists();
        }

        public event EventHandler SelectedItemChanged;

        public event EventHandler ManualSyncStarting;

        public event EventHandler ManualSyncFinished;

        public event EventHandler<ListMovedEventArgs> ListMoved;

        public event EventHandler<ListAddedOrDeletedEventArgs> ListAdded;

        public event EventHandler<ListAddedOrDeletedEventArgs> ListDeleted;

        public event EventHandler UpdateSelectedItem;

        public int SelectedIndex
        {
            get { return this.NativeInterface.SelectedIndex; }
            set { this.NativeInterface.SelectedIndex = value; }
        }

        public void SetSelectedList(List list)
        {
            int index = InternalGlobals.CurrentProfile.Lists.IndexOf(list);
            this.SelectedIndex = index;
        }

        private void RenameSelectedList(object sender, EventArgs e)
        {
            this.RenameSelectedList();
        }

        private void RenameSelectedList()
        {
            List selectedList = InternalGlobals.CurrentProfile.Lists[this.SelectedIndex];
            //IRadioListItem item = this.NativeInterface.Lists[this.SelectedIndex];
            RenameDialogPresenter dialog = new RenameDialogPresenter(selectedList.Name);
            dialog.NativeInterface.MainInstructions = Localization.UIResources.RenameListDialogMainInstructions;
            if (dialog.ShowDialog() == UIDialogResult.OK)
            {
                selectedList.Name = dialog.Value;
                //item.Text = dialog.Value;
                //this.UpdateListInfoDisplay();
                //this.NativeInterface.RefreshItems();
            }
        }

        private void ToggleSelectedListEnabled(object sender, EventArgs e)
        {
            List selectedList = InternalGlobals.CurrentProfile.Lists[this.SelectedIndex];
            selectedList.Enabled = !selectedList.Enabled;
            this.NativeInterface.EnableButton.Available = !selectedList.Enabled;
            this.NativeInterface.DisableButton.Available = selectedList.Enabled;
            this.contextEnable.Available = this.NativeInterface.EnableButton.Available;
            this.contextDisable.Available = this.NativeInterface.DisableButton.Available;

            InternalGlobals.CurrentProfile.SaveConfig();
            InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
            //PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.Regenerate();

            //IRadioListItem item = this.NativeInterface.SelectedIndex;

            //if (item != null)
            //{
            //    if (selectedList.Enabled)
            //    {
            //        //item.Button.ForeColor = SystemColors.ControlText;
            //        item.Image = EmbeddedImages.GreenCheckmark;
            //    }
            //    else
            //    {
            //        item.Image = EmbeddedImages.GrayX;
            //        //item.Button.ForeColor = SystemColors.ControlDarkDark;
            //    }

            //    item.ShowEnabledAppearance = selectedList.Enabled;
            //}
            //else
            //{
                //this.UpdateLists();
            //}
            //this.NativeInterface.RefreshItems();
        }

        public void UpdateLists()
        {
            this.NativeInterface.BeginUpdate();
            this.NativeInterface.Lists.Clear();

            //bool anyLists = false;

            foreach (List list in InternalGlobals.CurrentProfile.Lists)
            {
                this.AddList(list);
                //anyLists = true;
            }

            //this.NativeInterface.ListDisplayVisible = anyLists;
            //this.splitContainer.Panel2Collapsed = collapsePanel;

            //this.listSearchOrderPictureBox.Visible = Globals.CurrentProfile.Lists.Count > 1;
            //this.NativeInterface.ShowListPrecedenceHint = Globals.CurrentProfile.Lists.Count > 1;

            this.NativeInterface.EndUpdate();
        }

        private void AddList(List list)
        {
            this.InsertList(this.NativeInterface.Lists.Count, list);
        }

        private void InsertList(int index, List list)
        {
            //IRadioListItem item = Globals.GuiManager.ToolkitHost.Factory.ConstructRadioListItem();
            ////RichListBoxItem item = new RichListBoxItem();
            //item.Text = list.Name;
            //item.ContextMenu = this.listContextMenu;
            ////item.Button.ContextMenuStrip = this.listContextMenu;
            ////item.Button.KeyDown += this.ListKeyDown;
            ////*item.Button.TextImageRelation = TextImageRelation.ImageBeforeText;
            ////*item.Button.ImageAlign = ContentAlignment.MiddleLeft;

            //if (list.Enabled)
            //{
            //    //*item.Button.ForeColor = SystemColors.ControlText;
            //    item.Image = EmbeddedImages.GreenCheckmark;
            //}
            //else
            //{
            //    item.Image = EmbeddedImages.GrayX;
            //    //*item.Button.ForeColor = SystemColors.ControlDarkDark;
            //}

            //item.ShowEnabledAppearance = list.Enabled;

            this.NativeInterface.Lists.Insert(index, list);
            //this.NativeInterface.ShowListPrecedenceHint = Globals.CurrentProfile.Lists.Count > 1;

            //this.SelectCurrentIndex();
        }

        private void HandleListsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.V:
                        Skins.PromptHandler.GetInstance().SetupDialog.DoItemPaste();
                        break;
                    default:
                        break;
                }
            }
            else if (!e.Alt && !e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        this.DeleteSelectedList();
                        break;
                    case Keys.F2:
                        this.RenameSelectedList();
                        break;
                    default:
                        break;
                }
            }
        }

        private void RemoveList(int index)
        {
            this.NativeInterface.Lists.RemoveAt(index);
            //this.NativeInterface.ShowListPrecedenceHint = Globals.CurrentProfile.Lists.Count > 1;
        }

        private string GetDescriptionFor(List list)
        {
            StringBuilder description = new StringBuilder();
            if (list.SyncLocation != null)
            {
                description.AppendLine(Localization.UIResources.ListShared);
                if (list.LastUpdateCheckTimestamp != null)
                {
                    description.AppendLine(String.Format(
                        CultureInfo.CurrentCulture, 
                        Localization.UIResources.ListSynchronizeTimestampFormat,
                        list.LastUpdateCheckTimestamp.Value.ToLocalTime().ToString(Localization.UIResources.ListSynchronizeTimestampToStringFormat, CultureInfo.CurrentCulture)));
                }

                description.AppendFormat(CultureInfo.CurrentCulture, Localization.UIResources.ListLocationFormat, list.SyncLocation.Path);
            }
            else
            {
                description.AppendLine(Localization.UIResources.ListNotShared);
            }

            return description.ToString();
        }

        //public void UpdateListInfoDisplay()
        //{
        //    ParameterlessVoid method = new ParameterlessVoid(this.UpdateListInfoDisplayInternal);
        //    if (this.NativeInterface.InvokeRequired)
        //    {
        //        this.NativeInterface.Invoke(method, null);
        //    }
        //    else
        //    {
        //        method();
        //    }
        //}

        protected virtual void OnManualSyncStarting(EventArgs e)
        {
            EventHandler handler = this.ManualSyncStarting;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnManualSyncFinished(EventArgs e)
        {
            EventHandler handler = this.ManualSyncFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //private void UpdateListInfoDisplayInternal()
        //{
        //    int selectedIndex = this.SelectedIndex;
        //    if (selectedIndex >= 0 && selectedIndex < this.NativeInterface.Lists.Count)
        //    {
        //        List list = Globals.CurrentProfile.Lists[selectedIndex];
        //        this.NativeInterface.ListInfoName = list.Name;
        //        this.NativeInterface.ListInfoDescription = this.GetDescriptionFor(list);
        //        this.NativeInterface.ListDisplayVisible = true;
        //    }
        //}

        private void UpdateNoListsMessageVisibility()
        {
            this.NativeInterface.ShowNoListsMessage = this.NativeInterface.Lists.Count <= 0;
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            int selectedIndex = this.SelectedIndex;
            this.NativeInterface.MoveListUpButton.Enabled = selectedIndex > 0;
            this.NativeInterface.MoveListDownButton.Enabled = selectedIndex < this.NativeInterface.Lists.Count - 1 && selectedIndex >= 0;

            if (selectedIndex >= 0 && selectedIndex < this.NativeInterface.Lists.Count)
            {
                this.NativeInterface.RenameButton.Enabled = true;

                List list = InternalGlobals.CurrentProfile.Lists[selectedIndex];
                //this.NativeInterface.ListInfoName = list.Name;
                //this.NativeInterface.ListInfoDescription = this.GetDescriptionFor(list);
                //this.NativeInterface.ListDisplayVisible = true;
                //this.splitContainer.Panel2Collapsed = false;
                this.NativeInterface.EnableButton.Available = !list.Enabled;
                this.NativeInterface.DisableButton.Available = list.Enabled;
                this.NativeInterface.DisableButton.Enabled = true;
                this.contextEnable.Available = this.NativeInterface.EnableButton.Available;
                this.contextDisable.Available = this.NativeInterface.DisableButton.Available;
                this.NativeInterface.ExportMenuItem.Enabled = true;
                this.NativeInterface.DeleteButton.Enabled = true;
                this.NativeInterface.EnableButton.Enabled = true;
                this.NativeInterface.ListOutButton.Enabled = true;
                this.NativeInterface.SyncButton.Enabled = true;
                if (list.SyncLocation != null)
                {
                    this.contextUnsubscribeSeparator.Available = true;
                    this.NativeInterface.UnsubscribeButton.Available = true;
                    this.contextUnsubscribe.Available = true;
                }
                else
                {
                    this.contextUnsubscribeSeparator.Available = false;
                    this.NativeInterface.UnsubscribeButton.Available = false;
                    this.contextUnsubscribe.Available = false;
                }
            }
            else
            {
                this.NativeInterface.ListOutButton.Enabled = false;
                this.NativeInterface.SyncButton.Enabled = false;
                this.NativeInterface.RenameButton.Enabled = false;
                //this.NativeInterface.ListDisplayVisible = false;
                this.NativeInterface.ExportMenuItem.Enabled = false;
                this.NativeInterface.UnsubscribeButton.Available = false;

                this.contextUnsubscribeSeparator.Available = false;
                this.contextUnsubscribe.Available = false;
                this.NativeInterface.DeleteButton.Enabled = false;
                this.NativeInterface.EnableButton.Enabled = false;
                this.NativeInterface.EnableButton.Available = true;
                this.NativeInterface.DisableButton.Available = false;
                this.NativeInterface.DisableButton.Enabled = false;
            }

            this.UpdateNoListsMessageVisibility();

            EventHandler handler = this.SelectedItemChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void UpdateUI()
        {
            this.SelectedIndex = InternalGlobals.CurrentProfile.SelectedListIndex;
            this.OnSelectedItemChanged(EventArgs.Empty);
        }

        protected virtual void OnListMoved(ListMovedEventArgs e)
        {
            EventHandler<ListMovedEventArgs> handler = this.ListMoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnListAdded(ListAddedOrDeletedEventArgs e)
        {
            EventHandler<ListAddedOrDeletedEventArgs> handler = this.ListAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnListDeleted(ListAddedOrDeletedEventArgs e)
        {
            EventHandler<ListAddedOrDeletedEventArgs> handler = this.ListDeleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnUpdateSelectedItem(EventArgs e)
        {
            EventHandler handler = this.UpdateSelectedItem;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ForwardSelectedItemChanged(object sender, EventArgs e)
        {
            this.OnSelectedItemChanged(e);
        }

        //private void SelectCurrentIndex()
        //{
        //    /*
        //    IRadioListItem item = this.NativeInterface.Lists.SelectedItem;
        //    if (item != null)
        //    {
        //        item.Select();
        //    }
        //     */
        //}

        private void DeleteSelectedList(object sender, EventArgs e)
        {
            this.DeleteSelectedList();
        }

        private void DeleteSelectedList()
        {
            if (this.SelectedIndex > -1 && InternalGlobals.CurrentProfile != null)
            {
                List listToDelete = InternalGlobals.CurrentProfile.Lists[this.SelectedIndex];
                if (UIMessageBox.Show(
                    String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmDeleteList, listToDelete.Name),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.YesNo,
                    UIMessageBoxIcon.Information,
                    UIMessageBoxResult.No) != UIMessageBoxResult.No)
                {
                    try
                    {
                        this.NativeInterface.SetCursorToWait();
                        InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                        //Point listPanelAutoScroll = this.listPanel.AutoScrollPosition;

                        //object scrollToken = this.NativeInterface.SaveScrollPositions();

                        this.NativeInterface.BeginUpdate();

                        int index = this.SelectedIndex;
                        List list = InternalGlobals.CurrentProfile.Lists[index];
                        list.Commands.RemoveEntriesFromHistory(InternalGlobals.CurrentProfile.History);
                        list.Functions.RemoveEntriesFromHistory(InternalGlobals.CurrentProfile.History);
                        InternalGlobals.CurrentProfile.DeleteList(index);
                        this.RemoveList(index);
                        this.OnListDeleted(new ListAddedOrDeletedEventArgs(listToDelete, index));
                        //this.NativeInterface.UpdateScrollPositions(scrollToken);
                        //this.listPanel.AutoScrollPosition = listPanelAutoScroll.Invert();
                        //this.listBox.PerformLayout();
                        this.NativeInterface.EndUpdate();

                        this.NativeInterface.SetCursorToDefault();
                        this.UpdateUI();
                        //this.SelectCurrentIndex();
                    }
                    catch (IOException ex)
                    {
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.IOExceptionOnDeleteList, ex.Message),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }
                }
            }
        }

        private void AddEmptyList(object sender, EventArgs e)
        {
            this.AddList(null, true);
        }

        private void AddDefaultList(object sender, EventArgs e)
        {
            this.AddList(null, false);
        }

        private List AddList(string baseNameFormat, bool empty)
        {
            if (InternalGlobals.CurrentProfile != null)
            {
                this.NativeInterface.SetCursorToWait();
                this.NativeInterface.BeginUpdate();
                List list = InternalGlobals.CurrentProfile.CreateNewList(baseNameFormat, empty);
                this.AddList(list);
                this.OnListAdded(new ListAddedOrDeletedEventArgs(list, InternalGlobals.CurrentProfile.Lists.Count - 1));
                this.NativeInterface.EndUpdate();
                this.NativeInterface.SetCursorToDefault();

                return list;
            }

            return null;
        }

        private void MoveListUp(object sender, EventArgs e)
        {
            this.MoveList(-1);
        }

        private void MoveListDown(object sender, EventArgs e)
        {
            this.MoveList(1);
        }

        private void MoveList(int displacement)
        {
            if (this.SelectedIndex > -1 && InternalGlobals.CurrentProfile != null)
            {
                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                this.NativeInterface.BeginUpdate();
                int index = this.SelectedIndex;
                List list = InternalGlobals.CurrentProfile.Lists[index];
                int newIndex = index + displacement;
                this.RemoveList(index);
                this.InsertList(newIndex, list);
                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;
                InternalGlobals.CurrentProfile.Lists.Remove(list);
                InternalGlobals.CurrentProfile.Lists.Insert(newIndex, list);
                this.OnListMoved(new ListMovedEventArgs(list, newIndex));
                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
                InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
                this.NativeInterface.EndUpdate();
                Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        private void SyncListsAsync(object sender, EventArgs e)
        {
            if (!this.synchronizing)
            {
                //this.SyncLists();
                System.Threading.Thread workThread = new System.Threading.Thread(this.SyncLists);
                workThread.IsBackground = true;
                workThread.Start();
            }
        }

        public static void SyncListsStatic()
        {
            List<DiffDiffBase> needToAskUserAbout = new List<DiffDiffBase>();
            ListCollection needToSave;
            Dictionary<List, Exception> exceptions = InternalGlobals.CurrentProfile.Sync(ref needToAskUserAbout, out needToSave);

            //System.Threading.Thread.Sleep(3500);

            ParameterlessVoid cleanup = new ParameterlessVoid(delegate
            {
                if (exceptions.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    if (exceptions.Count == 1)
                    {
                        errorMessage.AppendLine(Localization.UIResources.SyncErrorMessageHeaderSingular);
                    }
                    else
                    {
                        errorMessage.AppendLine(Localization.UIResources.SyncErrorMessageHeaderPlural);
                    }

                    foreach (KeyValuePair<List, Exception> entry in exceptions)
                    {
                        errorMessage.AppendFormat(Localization.UIResources.SyncErrorListEntry, entry.Key.Name);
                        if (entry.Value is IOException)
                        {
                            errorMessage.AppendLine(String.Format(Localization.MessageFormats.GeneralError, entry.Value.Message));
                        }
                        else if (entry.Value is NewerPackageVersionException)
                        {
                            errorMessage.AppendLine(String.Format(Localization.MessageFormats.NewerPackageUnreadableMessageFormat, entry.Key.SyncLocation.Path));
                        }
                        else if (entry.Value is CorruptPackageException
                            || entry.Value is FileFileSystem.FileFileSystemException
                            || entry.Value is System.Xml.XmlException
                            || entry.Value is LoadException)
                        {
                            errorMessage.AppendLine(Localization.MessageFormats.InternalPackageError);
                        }
                    }

                    UIMessageBox.Show(
                        errorMessage.ToString(),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }

                if (needToAskUserAbout.Count > 0)
                {
                    CollisionResolvingDialogPresenter dialog = new CollisionResolvingDialogPresenter(needToAskUserAbout);
                    //InternalGlobals.UISettings.CollisionResolvingDialogSettings.ImpartTo(dialog.NativeInterface);
                    dialog.ShowDialog(Skins.PromptHandler.GetDialogOwner());
                    //InternalGlobals.UISettings.CollisionResolvingDialogSettings.UpdateFrom(dialog.NativeInterface);
                    needToSave.SaveAll();
                }
            });

            if (InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.InvokeRequired)
            {
                InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(cleanup, null);
            }
            else
            {
                cleanup();
            }
        }

        public void SyncLists()
        {
            this.synchronizing = true;
            this.OnManualSyncStarting(EventArgs.Empty);
            this.NativeInterface.NotifySyncStarting();
            this.NativeInterface.SetCursorToWait();
            List<DiffDiffBase> needToAskUserAbout = new List<DiffDiffBase>();
            ListCollection needToSave;
            Dictionary<List, Exception> exceptions = InternalGlobals.CurrentProfile.Sync(ref needToAskUserAbout, out needToSave);

            //System.Threading.Thread.Sleep(3500);

            ParameterlessVoid cleanup = new ParameterlessVoid(delegate
            {
                this.NativeInterface.NotifySyncEnded();
                this.OnUpdateSelectedItem(EventArgs.Empty);
                if (exceptions.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    if (exceptions.Count == 1)
                    {
                        errorMessage.AppendLine(Localization.UIResources.SyncErrorMessageHeaderSingular);
                    }
                    else
                    {
                        errorMessage.AppendLine(Localization.UIResources.SyncErrorMessageHeaderPlural);
                    }

                    foreach (KeyValuePair<List, Exception> entry in exceptions)
                    {
                        errorMessage.AppendFormat(Localization.UIResources.SyncErrorListEntry, entry.Key.Name);
                        if (entry.Value is IOException)
                        {
                            errorMessage.AppendLine(String.Format(Localization.MessageFormats.GeneralError, entry.Value.Message));
                        }
                        else if (entry.Value is NewerPackageVersionException)
                        {
                            errorMessage.AppendLine(String.Format(Localization.MessageFormats.NewerPackageUnreadableMessageFormat, entry.Key.SyncLocation.Path));
                        }
                        else if (entry.Value is CorruptPackageException
                            || entry.Value is FileFileSystem.FileFileSystemException
                            || entry.Value is System.Xml.XmlException
                            || entry.Value is LoadException)
                        {
                            errorMessage.AppendLine(Localization.MessageFormats.InternalPackageError);
                        }
                    }

                    UIMessageBox.Show(
                        errorMessage.ToString(),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }

                this.NativeInterface.SetCursorToDefault();

                if (needToAskUserAbout.Count > 0)
                {
                    CollisionResolvingDialogPresenter dialog = new CollisionResolvingDialogPresenter(needToAskUserAbout);
                    //InternalGlobals.UISettings.CollisionResolvingDialogSettings.ImpartTo(dialog.NativeInterface);
                    dialog.ShowDialog(Skins.PromptHandler.GetDialogOwner());
                    //InternalGlobals.UISettings.CollisionResolvingDialogSettings.UpdateFrom(dialog.NativeInterface);
                    needToSave.SaveAll();
                }
            });

            if (this.NativeInterface.InvokeRequired)
            {
                this.NativeInterface.Invoke(cleanup, null);
            }
            else
            {
                cleanup();
            }

            this.OnManualSyncFinished(EventArgs.Empty);
            this.synchronizing = false;
        }

        private void AppendList(object sender, EventArgs e)
        {
            this.AppendOrLink(true);
        }

        private void LinkList(object sender, EventArgs e)
        {
            this.AppendOrLink(false);
        }

        private void AppendOrLink(bool appendRatherThanLink)
        {
            if (appendRatherThanLink)
            {
                this.importOrLinkDialog.Text = Localization.UIResources.ImportOrLinkDialogTitleForAppend;
            }
            else
            {
                this.importOrLinkDialog.Text = Localization.UIResources.ImportOrLinkDialogTitleForLink;
            }

            if (this.importOrLinkDialog.ShowModal() == UIDialogResult.OK)
            {
                if (InternalGlobals.CurrentProfile != null)
                {
                    //Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    this.NativeInterface.BeginUpdate();
                    this.NativeInterface.SetCursorToWait();
                    List list = InternalGlobals.CurrentProfile.CreateNewList(null, true);
                    list.IsOwnedByUser = false;

                    list.SyncLocation = new SyncLocation(this.importOrLinkDialog.Path);
                    list.SyncInfo.SyncBase = list.Clone(InternalGlobals.CurrentProfile);
                    //MessageCollection messages = new MessageCollection();
                    bool error = false;
                    try
                    {
                        List<DiffDiffBase> needToAskUserAbout = new List<DiffDiffBase>();
                        list.SyncIfNecessary(ref needToAskUserAbout, false);
                    }
                    catch (IOException ex)
                    {
                        this.NativeInterface.EndUpdate();
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.GeneralError, ex.Message),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        error = true;
                    }
                    catch (System.Xml.XmlException)
                    {
                        this.NativeInterface.EndUpdate();
                        UIMessageBox.Show(
                            Localization.MessageFormats.InternalPackageError,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        error = true;
                    }
                    catch (LoadException)
                    {
                        this.NativeInterface.EndUpdate();
                        UIMessageBox.Show(
                            Localization.MessageFormats.InternalPackageError,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        error = true;
                    }
                    catch (CorruptPackageException)
                    {
                        this.NativeInterface.EndUpdate();
                        UIMessageBox.Show(
                            Localization.MessageFormats.InternalPackageError,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        error = true;
                    }
                    catch (NewerPackageVersionException)
                    {
                        this.NativeInterface.EndUpdate();
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.NewerPackageUnreadableMessageFormat, this.importOrLinkDialog.Path),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        error = true;
                    }
                    catch (FileFileSystem.FileFileSystemException)
                    {
                        this.NativeInterface.EndUpdate();
                        UIMessageBox.Show(
                            Localization.MessageFormats.InternalPackageError,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                        error = true;
                    }

                    if (!appendRatherThanLink && !error)
                    {
                        foreach (List otherList in InternalGlobals.CurrentProfile.Lists)
                        {
                            if (otherList != list)
                            {
                                if (list.SyncLocation.HasSameLocationAs(otherList.SyncLocation))
                                {
                                    this.NativeInterface.EndUpdate();
                                    UIMessageBox.Show(
                                        String.Format(CultureInfo.CurrentCulture, Localization.UIResources.ListHasAlreadyBeenAdded, otherList.Name),
                                        Localization.Promptu.AppName,
                                        UIMessageBoxButtons.OK,
                                        UIMessageBoxIcon.Error,
                                        UIMessageBoxResult.OK);

                                    InternalGlobals.CurrentProfile.DeleteList(list);
                                    this.NativeInterface.EndUpdate();
                                    this.NativeInterface.SetCursorToDefault();
                                    return;
                                }
                            }
                        }
                    }

                    //if (messages.Count > 0)
                    //{
                    //    MessageBox.Show("Error
                    //}
                    if (appendRatherThanLink && error)
                    {
                        InternalGlobals.CurrentProfile.DeleteList(list);
                    }
                    else
                    {
                        if (appendRatherThanLink)
                        {
                            list.SyncLocation = null;
                        }

                        this.AddList(list);
                        this.OnListAdded(new ListAddedOrDeletedEventArgs(list, InternalGlobals.CurrentProfile.Lists.Count - 1));
                    }

                    this.NativeInterface.EndUpdate();
                    this.NativeInterface.SetCursorToDefault();
                    //this.listPanel.ResumeDrawing();
                    //this.listPanel.Refresh();
                    //Cursor.Current = System.Windows.Forms.Cursors.Default;
                }
            }
        }

        private void UnsubscribeList(object sender, EventArgs e)
        {
            this.UnsubscribeOrUnpublish(true);
        }

        private void UnpublishList(object sender, EventArgs e)
        {
            this.UnsubscribeOrUnpublish(false);
        }

        private void UnsubscribeOrUnpublish(bool unsubscribeRatherThanUnpublish)
        {
            List currentList = InternalGlobals.CurrentProfile.Lists[this.SelectedIndex];

            if (!unsubscribeRatherThanUnpublish && currentList.SyncLocation.CanDelete && UIMessageBox.Show(
                    String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.UnpublishAlsoDeleteFile, currentList.SyncLocation.Path),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.YesNo,
                    UIMessageBoxIcon.Information,
                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
            {
                currentList.SyncLocation.Delete();
            }

            currentList.SyncLocation = null;
            InternalGlobals.CurrentProfile.SaveConfig();
            //this.UpdateListInfoDisplay();
            //this.unpublishButton.Visible = false;
            //this.contextUnpublish.Visible = false;
            this.NativeInterface.UnsubscribeButton.Available = false;
            this.contextUnsubscribe.Available = false;
            this.contextUnsubscribeSeparator.Available = false;
        }

        private void PublishList(object sender, EventArgs e)
        {
            this.PublishOrExport(true);
        }

        private void ExportClick(object sender, EventArgs e)
        {
            this.PublishOrExport(false);
        }

        private void PublishOrExport(bool publish)
        {
            List currentList = InternalGlobals.CurrentProfile.Lists[this.SelectedIndex];

            if (publish && currentList.SyncLocation != null)
            {
                if (UIMessageBox.Show(
                    String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.ConfirmChangePublishLocation, currentList.Name, currentList.SyncLocation.Path),
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.YesNo,
                    UIMessageBoxIcon.Information,
                    UIMessageBoxResult.Yes) != UIMessageBoxResult.Yes)
                {
                    return;
                }
            }

            // I18N?
            this.publishDialog.Path = currentList.Name.Replace(' ', '_') + ".pdc";
            if (publish)
            {
                this.publishDialog.Text = Localization.UIResources.PublishDialogTitleForPublish;
            }
            else
            {
                this.publishDialog.Text = Localization.UIResources.PublishDialogTitleForExport;
            }

            if (this.publishDialog.ShowModal() == UIDialogResult.OK)
            {
                this.NativeInterface.SetCursorToWait();
                if (publish)
                {
                    this.contextUnsubscribeSeparator.Available = true;
                    this.NativeInterface.UnsubscribeButton.Available = true;
                    this.contextUnsubscribe.Available = true;
                    currentList.SyncLocation = new SyncLocation(this.publishDialog.Path);
                    //this.UpdateListInfoDisplay();
                    currentList.PublishChanges(true);
                    InternalGlobals.CurrentProfile.SaveConfig();
                }
                else
                {
                    try
                    {
                        using (FileStream stream = new FileStream(this.publishDialog.Path, FileMode.Create))
                        {
                            currentList.Export(stream, false);
                            stream.Close();
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.UnauthorizedAccessExceptionGeneral, ex.Message),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }
                }

                this.NativeInterface.SetCursorToDefault();
            }
        }

        private void ImportButtonClick(object sender, EventArgs e)
        {
            if (this.importDialog.ShowModal() == UIDialogResult.OK)
            {
                // I18N
                if (Path.GetExtension(this.importDialog.Path).ToUpperInvariant() == ".QRS")
                {
                    this.NativeInterface.SetCursorToWait();
                    InternalGlobals.SyncSynchronizer.CancelSyncsAndWait();
                    List list = this.AddList(Localization.UIResources.SlickRunNameExpansionFormat, true);//PromptuSettings.CurrentProfile.Lists[this.SelectedIndex];
                    InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;
                    CommandCollection importedCommands = Interop.SlickRun.ImportMagicWords(
                        this.importDialog.Path, 
                        list.Commands.IdGenerator);

                    foreach (Command newCommand in importedCommands)
                    {
                        TrieList conflicts = list.Commands.GetConflicts(newCommand.Name, CaseSensitivity.Insensitive, GetConflictsMode.ReturnOnlyAliases, null);
                        bool addCommand = true;
                        if (conflicts.Count > 0)
                        {
                            UIDialogResult dialogResult = UIDialogResult.Cancel; //ListDataManager.GetUserConflictDecision(conflicts, newCommand.Name, list.Name);
                            if (dialogResult == UIDialogResult.OK)
                            {
                                foreach (string conflict in conflicts)
                                {
                                    list.Commands.Remove(newCommand.Name);
                                }
                            }
                            else if (dialogResult == UIDialogResult.Cancel)
                            {
                                list.Commands.Save();
                                return;
                            }
                            else
                            {
                                addCommand = false;
                            }
                        }

                        if (addCommand)
                        {
                            list.Commands.Add(newCommand);
                        }
                    }

                    InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
                    InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();

                    this.OnUpdateSelectedItem(EventArgs.Empty);
                    this.NativeInterface.SetCursorToDefault();

                    list.Commands.Save();

                    // I18N
                    string pluralOrSingular = String.Empty;
                    if (importedCommands.Count != 1)
                    {
                        pluralOrSingular = "s";
                    }

                    Skins.PromptHandler.GetInstance().SetupDialog.SetSelectedListTab(0);

                    UIMessageBox.Show(
                        String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.SlickRunImportMessage,
                        importedCommands.Count,
                        pluralOrSingular),
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.OK);
                }
            }
        }

        private void HandleListDragEnter(object sender, ListDragDropEventArgs e)
        {
            if (e.List != InternalGlobals.CurrentProfile.Lists[this.SelectedIndex])
            {
                ItemCopyInfo info = e.GetData(typeof(ItemCopyInfo)) as ItemCopyInfo;
                if (info != null)
                {
                    switch (info.ItemCode)
                    {
                        case ItemCode.AssemblyReference:
                        case ItemCode.Command:
                        case ItemCode.Function:
                        case ItemCode.ValueList:
                            if ((e.AllowedEffects & UIDragDropEffects.Move) != 0)
                            {
                                e.Effects = UIDragDropEffects.Move;
                                return;
                            }
                            else if ((e.AllowedEffects & UIDragDropEffects.Copy) != 0)
                            {
                                e.Effects = UIDragDropEffects.Copy;
                                return;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            e.Effects = UIDragDropEffects.None;
        }

        private void HandleListDrop(object sender, ListDragDropEventArgs e)
        {
            if (e.List != InternalGlobals.CurrentProfile.Lists[this.SelectedIndex])
            {
                ItemCopyInfo info = e.GetData(typeof(ItemCopyInfo)) as ItemCopyInfo;
                List listTo = e.List;
                if (info != null)
                {
                    switch (info.ItemCode)
                    {
                        case ItemCode.AssemblyReference:
                        case ItemCode.Command:
                        case ItemCode.Function:
                        case ItemCode.ValueList:
                            if ((e.AllowedEffects & UIDragDropEffects.Move) != 0)
                            {
                                e.Effects = UIDragDropEffects.Move;
                                ListDataManager.PasteData(listTo, InternalGlobals.CurrentProfile.SelectedList, info, true, true);
                                return;
                            }
                            else if ((e.AllowedEffects & UIDragDropEffects.Copy) != 0)
                            {
                                e.Effects = UIDragDropEffects.Copy;
                                ListDataManager.PasteData(listTo, InternalGlobals.CurrentProfile.SelectedList, info, false, true);
                                return;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            e.Effects = UIDragDropEffects.None;
        }
    }
}
