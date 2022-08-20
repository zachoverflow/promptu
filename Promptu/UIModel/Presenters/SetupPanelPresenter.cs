//-----------------------------------------------------------------------
// <copyright file="SetupPanelPresenter.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UI;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Globalization;

    internal abstract class SetupPanelPresenter<T> : PresenterBase<ISetupPanel> where T : IHasId
    {
        private ParameterlessVoid updateAllCallback;
        private List listUsing;
        private ItemCode itemCode;
        private bool raiseItemDoubleClick;
        private bool editItemOnDoubleClick = true;
        private SimpleCollectionPresenter collectionPresenter;

        private UIMenuItem contextPaste;
        private UIMenuItem contextCopy;
        private UIMenuItem contextCut;
        private UIMenuSeparator separator;
        private UIMenuItem contextDeleteItem;
        private UIMenuItem contextEditItem;
        private List<int> threadsUpdatingOn = new List<int>(1);

        public SetupPanelPresenter(ISetupPanel nativeInterface, ParameterlessVoid updateAllCallback)
            : base(nativeInterface)
        {
            if (updateAllCallback == null)
            {
                throw new ArgumentNullException("updateAllCallback");
            }

            this.updateAllCallback = updateAllCallback;

            this.NativeInterface.ItemDoubleClick += this.HandleItemDoubleClick;
            this.NativeInterface.ItemKeyDown += this.HandleItemKeyDown;
            this.NativeInterface.ItemDrag += this.HandleItemDrag;
            //this.NativeInterface.UISettingChanged += this.HandleUISettingChanged;

            this.collectionPresenter = new SimpleCollectionPresenter(this.NativeInterface.CollectionViewer);
            this.collectionPresenter.SelectedIndexChanged += this.HandleSelectedItemChanged;

            this.NativeInterface.ItemContextMenuOpening += this.ItemContextMenuOpening;

            this.contextCut = new UIMenuItem(
                "Promptu.Item.Cut", 
                Localization.UIResources.CutMenuItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Cut,
                new EventHandler(this.Cut));

            this.NativeInterface.ItemContextMenu.Items.Add(this.contextCut);

            this.contextCopy = new UIMenuItem(
                "Promptu.Item.Copy",
                Localization.UIResources.CopyMenuItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Copy,
                new EventHandler(this.Copy));

            this.NativeInterface.ItemContextMenu.Items.Add(this.contextCopy);

            this.contextPaste = new UIMenuItem(
                "Promptu.Item.Paste",
                Localization.UIResources.PasteMenuItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Paste,
                new EventHandler(this.Paste));

            this.NativeInterface.ItemContextMenu.Items.Add(this.contextPaste);

            this.separator = new UIMenuSeparator("Promptu.ItemActionsSeparator");
            this.NativeInterface.ItemContextMenu.Items.Add(this.separator);

            this.contextEditItem = new UIMenuItem(
                "Promptu.Item.Edit",
                Localization.UIResources.SetupPanelContextEditItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Edit,
                new EventHandler(this.EditSelectedItem));

            this.NativeInterface.ItemContextMenu.Items.Add(this.contextEditItem);

            this.contextDeleteItem = new UIMenuItem(
                "Promptu.Item.Delete",
                Localization.UIResources.SetupPanelContextDeleteItemText,
                InternalGlobals.GuiManager.ToolkitHost.Images.Delete,
                new EventHandler(this.DeleteSelectedItems));

            this.NativeInterface.ItemContextMenu.Items.Add(this.contextDeleteItem);

            this.NativeInterface.NewButton.Text = Localization.UIResources.NewButtonText;

            //this.NativeInterface.EditButton.Image = EmbeddedImages.Edit;
            //this.NativeInterface.EditButton.ToolTipText = Localization.UIResources.SetupPanelEditToolTip;
            this.NativeInterface.EditButton.Text = Localization.UIResources.EditButtonText;

            //this.NativeInterface.DeleteButton.Image = EmbeddedImages.RedX;
            //this.NativeInterface.DeleteButton.ToolTipText = Localization.UIResources.SetupPanelDeleteToolTip;
            this.NativeInterface.DeleteButton.Text = Localization.UIResources.DeleteButtonText;

            this.NativeInterface.NewButton.Click += this.CreateNewItem;
            this.NativeInterface.EditButton.Click += this.EditSelectedItem;
            this.NativeInterface.DeleteButton.Click += this.DeleteSelectedItems;

            this.UpdateToCurrentList();

            if (this.collectionPresenter.Count > 0)
            {
                this.collectionPresenter.SelectedIndexes.Add(0);
            }
        }

        //public event EventHandler UISettingChanged;

        public event EventHandler<ItemEventArgs<T>> ItemCreated;

        public event EventHandler<ItemEventArgs<T>> ItemEdited;

        public event EventHandler<ItemsEventArgs<T>> ItemsDeleted;

        public event EventHandler ItemDoubleClick;

        public event EventHandler SelectedItemChanged;

        public bool RaiseItemDoubleClick
        {
            get { return this.raiseItemDoubleClick; }
            set { this.raiseItemDoubleClick = value; }
        }

        public bool EditItemOnDoubleClick
        {
            get { return this.editItemOnDoubleClick; }
            set { this.editItemOnDoubleClick = value; }
        }

        internal SimpleCollectionPresenter CollectionPresenter
        {
            get { return this.collectionPresenter; }
        }

        public ItemCode ItemCode
        {
            get { return this.itemCode; }
            protected set { this.itemCode = value; }
        }

        public void SelectAndUpdate()
        {
            this.NativeInterface.SelectAndUpdate();
            this.UpdateEnabledStates();
        }

        public void UpdateToCurrentList()
        {
            if (InternalGlobals.CurrentProfile == null)
            {
                this.listUsing = null;
            }
            else
            {
                this.listUsing = InternalGlobals.CurrentProfile.SelectedList;
            }

            this.UpdateItemsListView();
            this.UpdatedToCurrentList();
        }

        public void SelectIndices(List<int> indices)
        {
            this.collectionPresenter.SelectedIndexes.Clear();

            foreach (int index in indices)
            {
                if (index >= 0 && index < this.collectionPresenter.Count)
                {
                    this.collectionPresenter.SelectedIndexes.Add(index);
                }
            }
        }

        public void CreateNewItem()
        {
            this.CreateNewItem(default(T));
        }

        public void CreateNewItem(T contents)
        {
            T item = this.CreateNewItemCore(contents);
            if (item != null)
            {
                this.OnItemCreated(new ItemEventArgs<T>(item));
            }
        }

        public void Focus()
        {
            this.NativeInterface.FocusCollectionViewer();
        }

        public void ClearSelectedIndices()
        {
            this.collectionPresenter.SelectedIndexes.Clear();
        }

        public void SelectIndex(int index)
        {
            this.collectionPresenter.SelectedIndexes.Clear();
            this.collectionPresenter.SelectedIndexes.Add(index);
        }

        public T GetSelectedItem()
        {
            return this.GetItem(this.collectionPresenter.PrimarySelectedIndex);
        }

        public bool MultiSelect
        {
            get { return this.CollectionPresenter.MultiSelect; }
            set { this.CollectionPresenter.MultiSelect = value; }
        }

        public void UpdateItemsListView()
        {
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            if (!this.threadsUpdatingOn.Contains(threadId))
            {
                try
                {
                    this.threadsUpdatingOn.Add(threadId);

                    this.NativeInterface.BeginUpdate();

                    List<int> selectedIndices = this.GetSelectedIndices();
                    //ListViewItem topItem = this.itemsListView.TopItem;
                    int topItemIndex = this.collectionPresenter.TopIndex;
                    //if (topItem == null)
                    //{
                    //    topItemIndex = 0;
                    //}
                    //else
                    //{
                    //    topItemIndex = topItem.Index;
                    //    ListViewItem item = this.itemsListView.Items[topItemIndex];
                    //}

                    this.collectionPresenter.Clear();

                    if (this.listUsing == null)
                    {
                        this.NativeInterface.Enabled = false;
                    }
                    else
                    {
                        this.NativeInterface.Enabled = true;

                        this.UpdateItemsListViewCore();
                    }

                    this.NativeInterface.CountDisplayText = this.FormatItemCount(this.collectionPresenter.Count);

                    this.SelectIndices(selectedIndices);

                    if (this.collectionPresenter.Count > 0)
                    {
                        if (topItemIndex >= this.collectionPresenter.Count)
                        {
                            topItemIndex = this.collectionPresenter.Count - 1;
                        }

                        this.collectionPresenter.TopIndex = topItemIndex;

                        //ListViewItem item = this.itemsListView.Items[topItemIndex];
                        //this.itemsListView.TopItem = item;
                        //this.itemsListView.TopItem = item;
                        //this.itemsListView.TopItem = item;
                    }

                    this.NativeInterface.EndUpdate();
                }
                finally
                {
                    this.threadsUpdatingOn.Remove(threadId);
                }
            }
        }

        protected virtual void UpdatedToCurrentList()
        {
        }

        protected List<int> GetSelectedIndices()
        {
            List<int> indices = new List<int>();
            foreach (int index in this.collectionPresenter.SelectedIndexes)
            {
                indices.Add(index);
            }

            return indices;
        }

        protected ParameterlessVoid UpdateAllCallback
        {
            get { return this.updateAllCallback; }
        }

        protected List ListUsing
        {
            get { return this.listUsing; }
        }

        protected abstract string GetPluralItemDisplayFormat();

        protected abstract string GetSingularItemDisplayFormat();

        protected abstract void UpdateItemsListViewCore();

        protected virtual void OnItemsDeleted(ItemsEventArgs<T> e)
        {
            EventHandler<ItemsEventArgs<T>> handler = this.ItemsDeleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemEdited(ItemEventArgs<T> e)
        {
            EventHandler<ItemEventArgs<T>> handler = this.ItemEdited;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemCreated(ItemEventArgs<T> e)
        {
            EventHandler<ItemEventArgs<T>> handler = this.ItemCreated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //protected virtual void OnUISettingChanged(EventArgs e)
        //{
        //    EventHandler handler = this.UISettingChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        protected abstract List<T> DeleteSelectedItemsCore(bool silent);

        protected abstract T CreateNewItemCore(T contents);

        protected abstract T EditSelectedItemCore();

        protected abstract T GetItem(int index);

        private void CreateNewItem(object sender, EventArgs e)
        {
            this.CreateNewItem();
        }

        protected virtual void OnItemDoubleClick(EventArgs e)
        {
            EventHandler handler = this.ItemDoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual UIDragDropEffects GetDragDropEffects()
        {
            if (InternalGlobals.GuiManager.ToolkitHost.Keyboard.CtrlKeyPressed)
            {
                return UIDragDropEffects.Copy;
            }
            else
            {
                return UIDragDropEffects.Move;
            }
        }

        protected List<T> GetSelectedItems()
        {
            List<T> selectedItems = new List<T>();
            foreach (int index in this.CollectionPresenter.SelectedIndexes)
            {
                T item = this.GetItem(index);
                if (item != null)
                {
                    selectedItems.Add(item);
                }
            }

            return selectedItems;
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedItemChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //private void HandleUISettingChanged(object sender, EventArgs e)
        //{
        //    this.OnUISettingChanged(EventArgs.Empty);
        //}

        private void ItemContextMenuOpening(object sender, CancelEventArgs e)
        {
            bool notOnHeaders;
            bool onItem = this.NativeInterface.MouseIsOverAnItem(out notOnHeaders);
            e.Cancel = !notOnHeaders;

            if (!e.Cancel)
            {
                this.contextPaste.Enabled = ListDataManager.GetIfCanPaste(this.ItemCode);
                this.separator.Available = onItem;
                this.contextDeleteItem.Available = onItem;
                this.contextEditItem.Available = onItem;
                this.contextCopy.Enabled = onItem;
                this.contextCut.Enabled = onItem;
            }
        }

        private string FormatItemCount(int itemCount)
        {
            string format;
            if (itemCount == 1)
            {
                format = this.GetSingularItemDisplayFormat();
            }
            else
            {
                format = this.GetPluralItemDisplayFormat();
            }

            return String.Format(CultureInfo.CurrentCulture, format, itemCount);
        }

        private void HandleSelectedItemChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledStates();
            this.OnSelectedItemChanged(EventArgs.Empty);
        }

        private void UpdateEnabledStates()
        {
            bool itemSelected = this.collectionPresenter.SelectedIndexes.Count > 0;
            this.NativeInterface.EditButton.Enabled = itemSelected;
            this.NativeInterface.DeleteButton.Enabled = itemSelected;
        }

        private void HandleItemDrag(object sender, EventArgs e)
        {
            List<T> items = this.GetSelectedItems();
            if (items.Count > 0)
            {
                this.NativeInterface.DoDragDrop(new ItemCopyInfo(this.itemCode, items), this.GetDragDropEffects());
            }
        }

        private void HandleItemDoubleClick(object sender, EventArgs e)
        {
            if (this.EditItemOnDoubleClick)
            {
                this.EditSelectedItem();
            }

            if (this.RaiseItemDoubleClick)
            {
                this.OnItemDoubleClick(EventArgs.Empty);
            }
        }

        private void HandleItemKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        this.SelectAllItems();
                        break;
                    case Keys.C:
                        this.Copy();
                        break;
                    case Keys.X:
                        this.Cut();
                        break;
                    case Keys.V:
                        this.Paste();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        this.DeleteSelectedItems(false);
                        break;
                    case Keys.Enter:
                    case Keys.F2:
                        this.EditSelectedItem();
                        break;
                    default:
                        break;
                }
            }
        }

        private void EditSelectedItem(object sender, EventArgs e)
        {
            this.EditSelectedItem();
        }

        private void EditSelectedItem()
        {
            T item = this.EditSelectedItemCore();
            if (item != null)
            {
                this.OnItemEdited(new ItemEventArgs<T>(item));
            }
        }

        private void DeleteSelectedItems(object sender, EventArgs e)
        {
            this.DeleteSelectedItems(false);
        }

        private void DeleteSelectedItems(bool silent)
        {
            InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = false;
            List<T> items = this.DeleteSelectedItemsCore(silent);
            InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.AllowRegenerate = true;
            InternalGlobals.CurrentProfile.CompositeFunctionsAndCommandsMediator.RegenerateAll();
            if (items != null && items.Count > 0)
            {
                this.OnItemsDeleted(new ItemsEventArgs<T>(items));
            }
        }

        private void UploadClipboardData(bool cut)
        {
            try
            {
                int start = Environment.TickCount;
                Cursor.Current = Cursors.WaitCursor;
                List<T> items = this.GetSelectedItems();

                if (cut)
                {
                    this.DeleteSelectedItems(true);
                }

                ClipboardCopyData.LastSetData = new ClipboardCopyData(new ItemCopyInfo(this.itemCode, items), this.ListUsing.FolderName, cut);
                InternalGlobals.GuiManager.ToolkitHost.Clipboard.Clear();
                InternalGlobals.GuiManager.ToolkitHost.Clipboard.SetData("Promptu-Cut/Copy", "0");

                while (Environment.TickCount - start < 50)
                {
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void Copy(object sender, EventArgs e)
        {
            this.Copy();
        }

        private void Copy()
        {
            this.UploadClipboardData(false);
        }

        private void Cut(object sender, EventArgs e)
        {
            this.Cut();
        }

        private void Cut()
        {
            this.UploadClipboardData(true);
        }

        private void SelectAllItems()
        {
            for (int i = 0; i < this.collectionPresenter.Count; i++)
            {
                this.collectionPresenter.SelectedIndexes.Add(i);
            }
        }

        private void Paste(object sender, EventArgs e)
        {
            this.Paste();
        }

        public void Paste()
        {
            if (ListDataManager.GetIfCanPaste(this.ItemCode))
            {
                List<Id> idsToSelect = ListDataManager.DownloadClipboardData(this.ItemCode);

                //this.SelectIndices(idsToSelect);

                this.collectionPresenter.SelectedIndexes.Clear();

                for (int i = 0; i < this.collectionPresenter.Count; i++)
                {
                    T realItem = this.GetItem(i);
                    if (realItem != null && idsToSelect.Contains(realItem.Id))
                    {
                        this.collectionPresenter.SelectedIndexes.Add(i);
                    }
                }

                this.Focus();
            }
        }
    }
}
