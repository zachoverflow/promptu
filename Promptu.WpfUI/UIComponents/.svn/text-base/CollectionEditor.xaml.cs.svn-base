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
using ZachJohnson.Promptu.UIModel;
using ZachJohnson.Promptu.WpfUI.Collections;
using System.ComponentModel;
using System.Globalization;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for CollectionEditor.xaml
    /// </summary>
    internal partial class CollectionEditor : UserControl, ICollectionEditor
    {
        private bool allowPaste;
        private VirtualizingCollection collection = new VirtualizingCollection();
        private DataGridSelectedCellsBridge selectionBridge;
        private Dictionary<int, int> buttonColumnVisibilityMap = new Dictionary<int, int>();
        private UIContextMenu dataContextMenu;

        public CollectionEditor()
        {
            InitializeComponent();
            this.dataGrid.DataContext = collection;
            //this.dataGrid.PreviewMouseDown += this.HandleDataGridMouseDown;
            this.dataGrid.SelectionChanged += this.HandleDataGridSelectionChanged;
            this.dataGrid.CellEditEnding += HandleDataGridCellEditEnding;
            this.dataGrid.PreviewKeyDown += this.HandleDataGridPreviewKeyDown;

            this.collection.CellValueNeeded += this.HandleCollectionCellValueNeeded;
            this.collection.CellValuePushed += this.HandleCollectionCellValuePushed;
            this.collection.CellValidating += this.HandleCollectionCellValidating;
            this.selectionBridge = new DataGridSelectedCellsBridge(this.dataGrid);

            WpfContextMenu contextMenu = new WpfContextMenu();
            this.dataGrid.ContextMenu = contextMenu;
            this.dataContextMenu = new UIContextMenu(contextMenu);
            this.dataGrid.ContextMenuOpening += this.HandleDataContextMenuOpening;
        }

        public event EventHandler<UIModel.CellValueEventArgs> CellValueNeeded;

        public event EventHandler<UIModel.CellValueEventArgs> CellValuePushed;

        public event EventHandler<UIModel.CellValidatingEventArgs> CellValidating;

        public event EventHandler SelectionChanged;

        public event CancelEventHandler DataContextMenuOpening;

        public event EventHandler<UIModel.CellEventArgs> CellClicked;

        public event System.Windows.Forms.KeyEventHandler EditorKeyDown;

        public IButton AddButton
        {
            get { return this.addButton; }
        }

        public IButton DeleteButton
        {
            get { return this.deleteButton; }
        }

        public IToolbarButton MoveUpButton
        {
            get { return this.upArrow; }
        }

        public IToolbarButton MoveDownButton
        {
            get { return this.downArrow; }
        }

        public IButton PasteNewRowsButton
        {
            get { return this.pasteButton; }
        }

        public UIModel.UIContextMenu DataContextMenu
        {
            get { return this.dataContextMenu; }
        }

        public bool MessageVisible
        {
            set 
            { 
                this.message.Visibility = value ? Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.dataGrid.Visibility = !value ? Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        public bool AllowPaste
        {
            get
            {
                return this.allowPaste;
            }
            set
            {
                this.allowPaste = value;
                this.dataGrid.SelectionMode = value ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single;
                this.pasteButton.Visibility = value ? Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        public bool DoMinimalCellValidating
        {
            get { return false; }
        }

        public string Message
        {
            set
            {
                this.message.Text = value;
            }
        }

        public int RowCount
        {
            get
            {
                return this.collection.Count;
            }
            set
            {
                this.collection.Count = value;
            }
        }

        public void EditCurrentCell()
        {
            this.dataGrid.BeginEdit();
        }

        public void Refresh()
        {
            IInputElement focusedElement = System.Windows.Input.Keyboard.FocusedElement;
            this.collection.RefreshBinding();
            
            System.Windows.Input.Keyboard.Focus(focusedElement);
        }

        public void ClearSelection()
        {
            if (this.dataGrid.SelectionMode == DataGridSelectionMode.Single)
            {
                this.dataGrid.SelectedItem = null;
            }
            else
            {
                this.dataGrid.SelectedItems.Clear();
            }
        }

        public int SelectedCellsCount
        {
            get { return this.dataGrid.SelectedCells.Count; }
        }

        public UIModel.CellAddress? CurrentCell
        {
            get
            {
                //DataGridCellInfo cell = this.dataGrid.CurrentCell;
                object rowItem = this.dataGrid.SelectedItem;
                if (rowItem != null)
                {
                    return new CellAddress(this.collection.IndexOf(rowItem), 0);
                }

                return null;
            }
            set
            {

                if (value != null)
                {
                    DataGridCellInfo info = this.ConvertAddress(value.Value);
                    this.dataGrid.SelectedItem = info.Item;
                    this.dataGrid.CurrentCell = info;
                    // TODO handle focus issues
                }
                else
                {
                    this.ClearSelection();
                }
            }
        }

        public bool IsSelected(int row, int column)
        {
            return this.dataGrid.SelectedCells.Contains(this.ConvertAddress(row, column));
        }

        private CellAddress ConvertCellInfo(DataGridCellInfo cell)
        {
            return new CellAddress(dataGrid.Items.IndexOf(cell.Item), dataGrid.Columns.IndexOf(cell.Column));
        }

        private DataGridCellInfo ConvertAddress(int row, int column)
        {
            return ConvertAddress(new CellAddress(row, column));
        }

        private DataGridCellInfo ConvertAddress(CellAddress address)
        {
            return new DataGridCellInfo(this.collection[address.Row], this.dataGrid.Columns[address.Column]);
        }

        public IEnumerable<UIModel.CellAddress> SelectedCells
        {
            get { return this.selectionBridge; }
        }

        public void InsertColumn(int index, Column column)
        {
            DataGridColumn dataGridColumn;

            ComboBoxColumn comboBoxColumn = column as ComboBoxColumn;

            Binding binding = new Binding(String.Format(CultureInfo.InvariantCulture, "[{0}]", this.ColumnCount));
            binding.Mode = BindingMode.TwoWay;

            if (comboBoxColumn != null)
            {
                //DataGridTemplateColumn dgComboBoxColumn = new DataGridTemplateColumn();
                ////dgComboBoxColumn.SelectedItemBinding = new Binding(bindingExpression);
                ////dgComboBoxColumn.ItemsSource = comboBoxColumn.Values;
                //FrameworkElementFactory factory = new FrameworkElementFactory(typeof(ComboBox));
                //factory.SetBinding(ComboBox.TextProperty, binding);
                //factory.SetValue(ComboBox.ItemsSourceProperty, comboBoxColumn.Values);

                //DataTemplate template = new DataTemplate();
                //template.VisualTree = factory;
                //dgComboBoxColumn.CellTemplate = template;

                //dataGridColumn = dgComboBoxColumn;

                DataGridComboBoxColumn dgComboBoxColumn = new DataGridComboBoxColumn();
                dgComboBoxColumn.SelectedItemBinding = binding;
                dgComboBoxColumn.ItemsSource = comboBoxColumn.Values;
                dataGridColumn = dgComboBoxColumn;
            }
            else if (column is CheckBoxColumn)
            {
                //DataGridTemplateColumn dgCheckBoxColumn = new DataGridTemplateColumn();

                //FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox));
                //factory.SetBinding(CheckBox.IsCheckedProperty, binding);

                //DataTemplate template = new DataTemplate();
                //template.VisualTree = factory;
                //dgCheckBoxColumn.CellTemplate = template;
                DataGridCheckBoxColumn dgCheckBoxColumn = new DataGridCheckBoxColumn();
                dgCheckBoxColumn.Binding = binding;

                dataGridColumn = dgCheckBoxColumn;
            }
            else if (column is ButtonColumn)
            {
                DataGridTemplateColumn dgButtonColumn = new DataGridTemplateColumn();

                int visibilityIndex = this.buttonColumnVisibilityMap.Count;

                Binding visibilityBinding = new Binding(String.Format(
                    CultureInfo.InvariantCulture,
                    "ButtonVisibility[{0}].Visibility",
                    visibilityIndex)); 

                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(WpfToolbarButton));
                factory.SetBinding(Button.ContentProperty, binding);
                factory.SetBinding(Button.VisibilityProperty, visibilityBinding);

                DataTemplate template = new DataTemplate();
                template.VisualTree = factory;
                dgButtonColumn.CellTemplate = template;
                //dgButtonColumn.t
                //DataGridViewButtonColumn dgvButtonColumn = new DataGridViewButtonColumn();
                //dgvButtonColumn.CellTemplate = new CustomDataGridViewButtonCell();
                dataGridColumn = dgButtonColumn;

                this.buttonColumnVisibilityMap.Add(this.ColumnCount, visibilityIndex);
            }
            else
            {
                DataGridTextColumn dgTextColumn = new DataGridTextColumn();
                dgTextColumn.Binding = binding;
                dataGridColumn = dgTextColumn;
            }

            dataGridColumn.Header = column.Text;
            dataGridColumn.IsReadOnly = column.ReadOnly;

            this.dataGrid.Columns.Add(dataGridColumn);
        }

        public int ColumnCount
        {
            get { return this.dataGrid.Columns.Count; }
        }

        public void RemoveColumnAt(int index)
        {
            this.dataGrid.Columns.RemoveAt(index);
        }

        public string Text
        {
            set
            {
                this.titleBox.Header = value;
            }
        }

        public bool IsVisibleButton(UIModel.CellAddress cell)
        {
            int index;
            if (this.buttonColumnVisibilityMap.TryGetValue(cell.Column, out index))
            {
                return this.collection.GetButtonVisibility(cell.Row, index) == System.Windows.Visibility.Visible;
            }

            return false;
        }

        public void SetButtonVisibility(UIModel.CellAddress cell, bool value)
        {
            int index;
            if (this.buttonColumnVisibilityMap.TryGetValue(cell.Column, out index))
            {
                this.collection.SetButtonVisibility(cell.Row, index, value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public void InvalidateCell(UIModel.CellAddress cell)
        {
            this.collection.RefreshRow(cell.Row);
        }

        public void InvalidateRow(int row)
        {
            this.collection.RefreshRow(row);
        }

        public bool IsVisibleColumn(int index)
        {
            return this.dataGrid.Columns[index].Visibility == System.Windows.Visibility.Visible;
        }

        public void SetColumnVisibility(int index, bool value)
        {
            this.dataGrid.Columns[index].Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        //public object GetClipboardData()
        //{
        //    return this.dataGrid.
        //}

        public void BeginUpdate()
        {
        }

        public void EndUpdate()
        {
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell != null && 
                !cell.IsEditing && 
                !cell.IsReadOnly &&
                !InternalGlobals.GuiManager.ToolkitHost.Keyboard.ShiftKeyPressed &&
                !InternalGlobals.GuiManager.ToolkitHost.Keyboard.CtrlKeyPressed)
            {
                if (!cell.IsFocused)
                {
                    cell.Focus();
                }
                DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                if (dataGrid != null)
                {
                    if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
                    {
                        if (!cell.IsSelected)
                            cell.IsSelected = true;
                    }
                    else
                    {
                        DataGridRow row = FindVisualParent<DataGridRow>(cell);
                        if (row != null && !row.IsSelected)
                        {
                            row.IsSelected = true;
                        }
                    }
                }
            }
        }

        static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        private void HandleCollectionCellValueNeeded(object sender, CellValueEventArgs e)
        {
            this.OnCellValueNeeded(e);
        }

        private void HandleCollectionCellValuePushed(object sender, CellValueEventArgs e)
        {
            this.OnCellValuePushed(e);
        }

        private void HandleCollectionCellValidating(object sender, CellValidatingEventArgs e)
        {
            this.OnCellValidating(e);
        }

        protected virtual void OnCellValueNeeded(CellValueEventArgs e)
        {
            EventHandler<CellValueEventArgs> handler = this.CellValueNeeded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCellValuePushed(CellValueEventArgs e)
        {
            EventHandler<CellValueEventArgs> handler = this.CellValuePushed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCellValidating(CellValidatingEventArgs e)
        {
            EventHandler<CellValidatingEventArgs> handler = this.CellValidating;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //private void HandleDataGridMouseDown(object sender, MouseEventArgs e)
        //{
        //    DataGridCell cell = GetDataGridCell(e.OriginalSource as DependencyObject);

        //    if (cell != null)
        //    {
        //        DataGridCellInfo cellInfo = new DataGridCellInfo(cell);
        //        if (!this.dataGrid.SelectedCells.Contains(cellInfo))
        //        {
        //            this.dataGrid.SelectedCells.Add(cellInfo);
        //        }

        //        //this.dataGrid.CurrentCell = cellInfo;
        //        //this.dataGrid.BeginEdit();
        //    }
        //}

        private DataGridCell GetDataGridCell(DependencyObject child)
        {
            DataGridCell cell = null;

            while (child != null)
            {
                cell = child as DataGridCell;
                if (cell != null)
                {
                    return cell;
                }

                child = VisualTreeHelper.GetParent(child);
            }

            return null;
        }

        private void HandleDataGridSelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = e.OriginalSource as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.IsDropDownOpen)
                {
                    this.dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }
            }
            else if (e.Source == this.dataGrid)
            {
                this.OnSelectionChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            EventHandler handler = this.SelectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleDataGridCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(Key.Enter) &&
                (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
            {
                TextBox tb = e.EditingElement as TextBox;
                if (tb != null)
                {
                    int selectionStart = tb.SelectionStart;
                    string newText = tb.Text.Remove(selectionStart, tb.SelectionLength);
                    tb.Text = newText.Insert(selectionStart, Environment.NewLine);
                    tb.Select(selectionStart + 1, 0);
                    e.Cancel = true;
                }
            }
        }

        private void HandleDataGridPreviewKeyDown(object sender, KeyEventArgs e)
        {
            ModifierKeys modifiers = System.Windows.Input.Keyboard.Modifiers;

            System.Windows.Forms.KeyEventArgs eventArgs =
                new System.Windows.Forms.KeyEventArgs(WpfToolkitHost.ConvertKey(e.Key, modifiers));

            this.OnEditorKeyDown(eventArgs);
            e.Handled = eventArgs.SuppressKeyPress || eventArgs.Handled;
        }

        protected virtual void OnEditorKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.KeyEventHandler handler = this.EditorKeyDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleDataGridCellClicked(object sender, EventArgs e)
        {
            DataGridCell cell = GetDataGridCell((DependencyObject)sender);
            this.OnCellClicked(new CellEventArgs(dataGrid.Columns.IndexOf(cell.Column), this.collection.IndexOf(cell.DataContext)));
        }

        protected virtual void OnCellClicked(CellEventArgs e)
        {
            EventHandler<UIModel.CellEventArgs> handler = this.CellClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DoCopy()
        {
            ApplicationCommands.Copy.Execute(null, this.dataGrid);
        }

        protected virtual void OnDataContextMenuOpening(CancelEventArgs e)
        {
            CancelEventHandler handler = this.DataContextMenuOpening;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleDataContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            CancelEventArgs eventArgs = new CancelEventArgs(e.Handled);
            this.OnDataContextMenuOpening(eventArgs);
            e.Handled = eventArgs.Cancel;
        }
    }
}
