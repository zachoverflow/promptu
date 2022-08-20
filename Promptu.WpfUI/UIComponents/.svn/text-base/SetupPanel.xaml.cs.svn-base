// Some code copyright (C) Josh Smith - January 2007
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
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI
{
	/// <summary>
	/// Interaction logic for SetupPanel.xaml
	/// </summary>
	internal partial class SetupPanel : UserControl, ISetupPanel
	{
        private UIModel.UIContextMenu itemContextMenu;
        private ParameterlessVoid settingChangedCallback;
        private Point oldMouseLocation;
        private bool dragCouldHappen;
        private List<int> selectedAtMouseDown = new List<int>();
        private ListViewItem captured;
        private Rect dragStartBounds;

		public SetupPanel()
		{
            this.itemContextMenu = new UIModel.UIContextMenu();
			this.InitializeComponent();

            this.collectionViewer.ContextMenuOpening += this.HandleItemContextMenuOpening;
            this.collectionViewer.PreviewMouseDoubleClick += this.HandleItemsMouseDoubleClick;
            this.collectionViewer.PreviewMouseMove += this.HandleMouseMove;
            this.collectionViewer.PreviewMouseDown += this.HandleMouseDown;
            this.collectionViewer.ColumnWidthChanged += this.HandleColumnWidthChanged;
            this.collectionViewer.PreviewMouseUp += this.HandleMouseUp;
		}

        public ParameterlessVoid SettingChangedCallback
        {
            get { return this.settingChangedCallback; }
            set { this.settingChangedCallback = value; }
        }

        public event EventHandler ItemDoubleClick;

        public event CancelEventHandler ItemContextMenuOpening;

        public event System.Windows.Forms.KeyEventHandler ItemKeyDown;

        public event EventHandler ItemDrag;

        public ContextMenu ItemContextMenuInternal
        {
            get { return (ContextMenu)this.ItemContextMenu.NativeContextMenuInterface; }
        }

        public UIModel.UIContextMenu ItemContextMenu
        {
            get { return this.itemContextMenu; }
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = false;
            if (this.dragCouldHappen)
            {
                this.Select(this.selectedAtMouseDown);
                Point mouseLocation = WpfUtilities.GetCursorPosition(this.collectionViewer, e.GetPosition(this.collectionViewer));

                if (!this.dragStartBounds.Contains(mouseLocation))
                {
                    //this.selectedAtMouseDown.Clear();

                    //foreach (int index in this.collectionViewer.SelectedIndexes)
                    //{
                    //    this.selectedAtMouseDown.Add(index);
                    //}
                    //Mouse.Capture(null);
                    this.dragCouldHappen = false;
                    
                    //ListViewItem capturedItem = this.captured;
                    //if (capturedItem != null)
                    //{
                    //    capturedItem.MouseMove -= this.HandleMouseMove;
                    //    this.captured = null;
                    //    Mouse.Capture(null);
                    //}

                    this.Uncapture();

                    this.OnItemDrag(EventArgs.Empty);

                    //this.Select(this.selectedAtMouseDown);
                }
            }
        }

        private void Select(IEnumerable<int> indexes)
        {
            this.collectionViewer.SelectedItems.Clear();

            foreach (int index in indexes)
            {
                this.collectionViewer.SelectIndex(index);
            }
        }

        //private void HandleItemMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = false;
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.selectedAtMouseDown.Clear();

        //        foreach (int index in this.collectionViewer.SelectedIndexes)
        //        {
        //            this.selectedAtMouseDown.Add(index);
        //        }
        //    }
        //}

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            if (e.ChangedButton == MouseButton.Left)
            {
                Point mouseLocation = WpfUtilities.GetCursorPosition(this.collectionViewer, e.GetPosition(this.collectionViewer));
                ListViewItem listViewItem = WpfUtilities.HitTestAncestorGet<ListViewItem>(this.collectionViewer, mouseLocation);
                if (listViewItem != null)
                {
                    if (!listViewItem.IsSelected)
                    {
                        KeyStates lShift = Keyboard.GetKeyStates(Key.LeftShift);
                        KeyStates rShift = Keyboard.GetKeyStates(Key.RightShift);
                        KeyStates lCtrl = Keyboard.GetKeyStates(Key.LeftCtrl);
                        KeyStates rCtrl = Keyboard.GetKeyStates(Key.RightCtrl);

                        bool shiftPressed = (lShift & KeyStates.Down)== KeyStates.Down
                            || (rShift & KeyStates.Down) == KeyStates.Down;

                        bool ctrlPressed = (lCtrl & KeyStates.Down) == KeyStates.Down
                            || (rCtrl & KeyStates.Down) == KeyStates.Down;

                        if (shiftPressed || ctrlPressed)
                        {
                            this.dragCouldHappen = false;
                            return;
                        }

                        this.selectedAtMouseDown.Clear();
                        this.selectedAtMouseDown.Add(this.collectionViewer.IndexOf(listViewItem.DataContext));
                    }
                    else
                    {
                        this.selectedAtMouseDown.Clear();

                        foreach (int index in this.collectionViewer.SelectedIndexes)
                        {
                            this.selectedAtMouseDown.Add(index);
                        }
                    }

                    Rect bounds = VisualTreeHelper.GetDescendantBounds(listViewItem);
                    Point ptInItem = WpfUtilities.GetCursorPosition(listViewItem, e.GetPosition(listViewItem));
                    double topOffset = Math.Abs(ptInItem.Y);
			        double btmOffset = Math.Abs(bounds.Height - ptInItem.Y);
			        double vertOffset = Math.Min(topOffset, btmOffset);

			        double width = SystemParameters.MinimumHorizontalDragDistance * 2;
			        double height = Math.Min( SystemParameters.MinimumVerticalDragDistance, vertOffset ) * 2;
			        Size szThreshold = new Size( width, height );

			        Rect rect = new Rect(mouseLocation, szThreshold );
				    rect.Offset( szThreshold.Width / -2, szThreshold.Height / -2 );

                    this.dragStartBounds = rect;

                    
                    
                    //this.captured = listViewItem;

                    //listViewItem.PreviewMouseMove += this.HandleMouseMove;
                    //listViewItem.PreviewMouseUp += this.HandleMouseUp;
                    //listViewItem.PreviewMouseDown += this.HandleItemMouseDown;

                    //e.Handled = true;
                    this.oldMouseLocation = mouseLocation;
                    //this.OnItemDrag(EventArgs.Empty);
                    this.dragCouldHappen = true;

                    //Mouse.Capture((IInputElement)sender);
                }
            }
        }

        private void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            if (e.ChangedButton == MouseButton.Left && this.dragCouldHappen)
            {
                this.Uncapture();
                this.dragCouldHappen = false;
            }
        }

        private void Uncapture()
        {
            ListViewItem capturedItem = this.captured;
            if (capturedItem != null)
            {
                //capturedItem.PreviewMouseMove -= this.HandleMouseMove;
                //capturedItem.PreviewMouseUp -= this.HandleMouseUp;
                //capturedItem.PreviewMouseDown -= this.HandleItemMouseDown;
                this.captured = null;
            }

            //Mouse.Capture(null);
        }

        //private bool Is(DependencyObject child)
        //{
        //    DataGridCell cell = null;

        //    while (child != null)
        //    {
        //        cell = child as DataGridCell;
        //        if (cell != null)
        //        {
        //            return cell;
        //        }

        //        child = VisualTreeHelper.GetParent(child);
        //    }

        //    return null;
        //}

        public bool MouseIsOverAnItem(out bool notOnHeaders)
        {
            HitTestResult result = VisualTreeHelper.HitTest(this.collectionViewer, Mouse.GetPosition(this.collectionViewer));
            //if (result.
            DependencyObject parent = result.VisualHit;

            while (parent != null && !(parent is ListBoxItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            notOnHeaders = true;
            return parent != null;

            //if (VisualTreeHelper.GetParent(object);
            //throw new NotImplementedException();
            //if (this.IsMouseOver)
            //{
            //}
            //notOnHeaders = true;
            //return true;
            //throw new NotImplementedException();
            //if (this.collectionViewer.
            //return this.collectionViewer.ItemContainerGenerator.
        }

        public IToolbarButton EditButton
        {
            get { return this.editItemButton; }
        }

        public IToolbarButton NewButton
        {
            get { return this.newItemButton; }
        }

        public IToolbarButton DeleteButton
        {
            get { return this.deleteItemButton; }
        }

        public string CountDisplayText
        {
            get
            {
                return this.itemsCountDisplay.Text;
            }
            set
            {
                this.itemsCountDisplay.Text = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.IsEnabled;
            }
            set
            {
                this.IsEnabled = value;
            }
        }

        public ISimpleCollectionViewer CollectionViewer
        {
            get { return this.collectionViewer; }
        }

        public void FocusCollectionViewer()
        {
            this.collectionViewer.Focus();
        }

        public void SelectAndUpdate()
        {
            this.collectionViewer.Select();
        }

        public void DoDragDrop(object data, UIModel.UIDragDropEffects allowedEffects)
        {
            DragDrop.DoDragDrop(this.collectionViewer, data, WpfToolkitHost.CastToDragDropEffects(allowedEffects));
        }

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        private void HandleItemsMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            bool notOnHeaders;
            if (this.MouseIsOverAnItem(out notOnHeaders) && notOnHeaders)
            {
                this.OnItemDoubleClick(EventArgs.Empty);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            ModifierKeys modifiers = Keyboard.Modifiers;

            System.Windows.Forms.KeyEventArgs eventArgs =
                new System.Windows.Forms.KeyEventArgs(WpfToolkitHost.ConvertKey(e.Key, modifiers));

            this.OnItemKeyDown(eventArgs);
            e.Handled = eventArgs.SuppressKeyPress || eventArgs.Handled;

            base.OnPreviewKeyDown(e);
        }

        protected virtual void OnItemDoubleClick(EventArgs e)
        {
            EventHandler handler = this.ItemDoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemDrag(EventArgs e)
        {
            EventHandler handler = this.ItemDrag;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.KeyEventHandler handler = this.ItemKeyDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnItemContextMenuOpening(CancelEventArgs e)
        {
            CancelEventHandler handler = this.ItemContextMenuOpening;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleItemContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            CancelEventArgs eventArgs = new CancelEventArgs(e.Handled);
            this.OnItemContextMenuOpening(eventArgs);
            e.Handled = eventArgs.Cancel;
        }

        private void HandleColumnWidthChanged(object sender, EventArgs e)
        {
            this.NotifySettingChanged();
        }

        private void NotifySettingChanged()
        {
            ParameterlessVoid callback = this.settingChangedCallback;
            if (callback != null)
            {
                callback();
            }
        }
    }
}