using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ListSelector.xaml
    /// </summary>
    internal partial class ListSelector : UserControl, IListSelector
    {
        private Storyboard syncAnimation;
        private bool keepSyncAnimationGoing;

        public ListSelector()
        {
            InitializeComponent();

            this.syncAnimation = (Storyboard)this.FindResource("SyncAnimation");
            this.syncAnimation.Completed += this.HandleSyncAnimationCompleted;

            this.listsListBox.SelectionChanged += this.RaiseSelectedIndexChanged;
            this.listsListBox.PreviewKeyDown += this.HandleListsListBoxKeyDown;
            this.listsListBox.Drop += this.HandleListsListBoxDrop;
            this.listsListBox.DragEnter += this.HandleListsListBoxDragEnter;
        }

        public event EventHandler SelectedIndexChanged;

        public event System.Windows.Forms.KeyEventHandler ListsKeyDown;

        public event EventHandler<ZachJohnson.Promptu.UIModel.ListDragDropEventArgs> ListDragEnter;

        public event EventHandler<ZachJohnson.Promptu.UIModel.ListDragDropEventArgs> ListDrop;

        public IList<ZachJohnson.Promptu.UserModel.List> Lists
        {
            get { return this.listsListBox.Lists; }
        }

        public int SelectedIndex
        {
            get { return this.listsListBox.SelectedIndex; }
            set { this.listsListBox.SelectedIndex = value; }
        }

        public INativeContextMenu ListContextMenu
        {
            set { this.listsListBox.ItemContextMenu = (ContextMenu)value; }
        }

        public bool ShowNoListsMessage
        {
            set { this.noListsMessage.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string NoListsMessageText
        {
            set { this.noListsMessage.Text = value; }
        }

        public ISplitToolbarButton NewListButton
        {
            get { return this.newListButton; }
        }

        public IToolbarButton DeleteButton
        {
            get { return this.deleteButton; }
        }

        public IToolbarButton MoveListUpButton
        {
            get { return this.moveListUpButton; }
        }

        public IToolbarButton MoveListDownButton
        {
            get { return this.moveListDownButton; }
        }

        public IToolbarButton RenameButton
        {
            get { return this.renameButton; }
        }

        public IToolbarButton SyncButton
        {
            get { return this.syncButton; }
        }

        public IToolbarButton UnsubscribeButton
        {
            get { return this.unsubscribeButton; }
        }

        public IToolbarButton EnableButton
        {
            get { return this.enableButton; }
        }

        public IToolbarButton DisableButton
        {
            get { return this.disableButton; }
        }

        public IToolbarMenuItem PublishMenuItem
        {
            get { return this.publishMenuItem; }
        }

        public IToolbarMenuItem ImportMenuItem
        {
            get { return this.appendMenuItem; }
        }

        public IToolbarMenuItem LinkMenuItem
        {
            get { return this.linkMenuItem; }
        }

        public IToolbarMenuItem SlickRunImportMenuItem
        {
            get { return this.importMenuItem; }
        }

        public IToolbarMenuItem ExportMenuItem
        {
            get { return this.exportMenuItem; }
        }

        public IToolbarMenuItem NewEmptyListMenuItem
        {
            get { return this.newEmptyListMenuItem; }
        }

        public IToolbarMenuItem NewDefaultListMenuItem
        {
            get { return this.newDefaultListMenuItem; }
        }

        public IMenuButton ListInButton
        {
            get { return this.incomingButton; }
        }

        public IMenuButton ListOutButton
        {
            get { return this.outgoingButton; }
        }

        public void SetCursorToWait()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.SetCursorToWait), null);
            }
            else
            {
                //this.Cursor = Cursors.Wait;
            }
        }

        public void SetCursorToDefault()
        {
            //this.ClearValue(CursorProperty);
        }

        public string Title
        {
            get
            {
                return this.title.Text;
            }
            set
            {
                this.title.Text = value;
            }
        }

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        public void BeginInvoke(Delegate method, object[] args)
        {
            this.Dispatcher.BeginInvoke(method, args);
        }

        public object Invoke(Delegate method, object[] args)
        {
            return this.Dispatcher.Invoke(method, args);
        }

        public bool InvokeRequired
        {
            get { return !this.Dispatcher.CheckAccess(); }
        }

        private void RaiseSelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSelectedIndexChanged(EventArgs.Empty);
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void RefreshItems()
        {
            this.listsListBox.Items.Refresh();
        }

        public void NotifySyncStarting()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.NotifySyncStarting), null);
            }
            else
            {
                this.keepSyncAnimationGoing = true;
                this.syncButton.FullDisable = false;
                this.syncButton.IsEnabled = false;
                this.syncAnimation.Begin();
            }
        }

        public void NotifySyncEnded()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ParameterlessVoid(this.NotifySyncEnded), null);
            }
            else
            {
                this.keepSyncAnimationGoing = false;
            }
        }

        private void HandleSyncAnimationCompleted(object sender, EventArgs e)
        {
            if (this.keepSyncAnimationGoing)
            {
                this.syncAnimation.Begin();
            }
            else
            {
                this.syncButton.FullDisable = true;
                this.syncButton.IsEnabled = true;
            }
        }

        private void HandleListsListBoxKeyDown(object sender, KeyEventArgs e)
        {
            ModifierKeys modifiers = Keyboard.Modifiers;

            System.Windows.Forms.KeyEventArgs eventArgs =
                new System.Windows.Forms.KeyEventArgs(WpfToolkitHost.ConvertKey(e.Key, modifiers));

            this.OnListsKeyDown(eventArgs);
            e.Handled = eventArgs.SuppressKeyPress || eventArgs.Handled;
        }

        protected virtual void OnListsKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.KeyEventHandler handler = this.ListsKeyDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnListDragEnter(ZachJohnson.Promptu.UIModel.ListDragDropEventArgs e)
        {
            EventHandler<ZachJohnson.Promptu.UIModel.ListDragDropEventArgs> handler = this.ListDragEnter;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnListDrop(ZachJohnson.Promptu.UIModel.ListDragDropEventArgs e)
        {
            EventHandler<ZachJohnson.Promptu.UIModel.ListDragDropEventArgs> handler = this.ListDrop;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleListsListBoxDragEnter(object sender, DragEventArgs e)
        {
            ZachJohnson.Promptu.UIModel.ListDragDropEventArgs eventArgs = ConvertDragEventArgs(e);
            if (eventArgs != null)
            {
                this.OnListDragEnter(eventArgs);
                ApplyPromptuEventArgs(eventArgs, e);
            }
        }

        private void HandleListsListBoxDrop(object sender, DragEventArgs e)
        {
            ZachJohnson.Promptu.UIModel.ListDragDropEventArgs eventArgs = ConvertDragEventArgs(e);
            if (eventArgs != null)
            {
                this.OnListDrop(eventArgs);
                ApplyPromptuEventArgs(eventArgs, e);
            }
        }

        private static ZachJohnson.Promptu.UIModel.ListDragDropEventArgs ConvertDragEventArgs(DragEventArgs e)
        {
            Visual originalSource = e.OriginalSource as Visual;
            if (originalSource == null)
            {
                return null;
            }

            ListBoxItem item = originalSource.GetThisOrAncestor<ListBoxItem>();

            if (item != null)
            {
                ZachJohnson.Promptu.UserModel.List list = item.DataContext as ZachJohnson.Promptu.UserModel.List;
                if (list != null)
                {
                    return new UIModel.ListDragDropEventArgs(
                        list,
                        WpfToolkitHost.CastToUIDragDropEffects(e.AllowedEffects),
                        delegate(Type type) { return e.Data.GetData(type); });
                }
            }

            return null;
        }

        private static void ApplyPromptuEventArgs(
            ZachJohnson.Promptu.UIModel.ListDragDropEventArgs promptuEventArgs, 
            DragEventArgs applyTo)
        {
            applyTo.Effects = WpfToolkitHost.CastToDragDropEffects(promptuEventArgs.Effects);
        }
    }
}
