using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class SimpleCollectionViewer : ListView, ISimpleCollectionViewer
    {
        private DataTemplate cellTemplate;
        private ListViewSelectedIndexCollection selectedIndexCollection;
        private ObservableCollection<object> items = new ObservableCollection<object>();
        private bool widthChanged;

        public SimpleCollectionViewer()
        {
            this.selectedIndexCollection = new ListViewSelectedIndexCollection(this);
            this.DataContext = this.items;
            this.ItemsSource = this.items;
        }

        public event EventHandler SelectedIndexChanged;

        public event EventHandler ColumnWidthChanged;

        public int ItemCount
        {
            get { return this.items.Count; }
        }

        public object this[int index]
        {
            get { return this.items[index]; }
        }

        public DataTemplate CellTemplate
        {
            get { return this.cellTemplate; }
            set { this.cellTemplate = value; }
        }

        public int HeaderCount
        {
            get { return this.GridView.Columns.Count; }
        }

        protected GridView GridView
        {
            get { return (GridView)this.View; }
        }

        public int SelectedIndexesCount
        {
            get { return this.SelectedItems.Count; }
        }

        public bool MultiSelect
        {
            get
            {
                return this.SelectionMode == SelectionMode.Extended;
            }
            set
            {
                this.SelectionMode = value ? SelectionMode.Extended : SelectionMode.Single;
            }
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public int IndexOf(object item)
        {
            return this.items.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            //ListViewItem lvItem = new ListViewItem();
            //lvItem.Content = item;
            //lvItem.Tag = tag;

            this.items.Insert(index, item);


            //this.Items.Insert(index, lvItem);
        }

        public void RemoveAt(int index)
        {
            this.items.RemoveAt(index);
            //this.Items.RemoveAt(index);
        }

        public void ClearHeaders()
        {
            this.GridView.Columns.Clear();
        }

        public void InsertHeader(int index, string text, ZachJohnson.Promptu.UIModel.CellValueGetter cellValueGetter)
        {
            GridViewColumn newColumn = new GridViewColumn();
            TextBlock header = new TextBlock();
            header.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            header.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            header.TextAlignment = TextAlignment.Left;
            header.Padding = new Thickness(4, 0, 4, 0);
            header.Text = text;
            newColumn.Header = header;
            //newColumn.DisplayMemberBinding = new Binding(String.Format("[{0}]", index));
            //newColumn.CellTemplate = (DataTemplate)this.FindResource("ListViewCellTemplate");

            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.MinWidthProperty, 100d);

            Binding binding = new Binding();
            binding.Converter = new CellValueGetterConverter(cellValueGetter);

            factory.SetBinding(TextBlock.TextProperty, binding);
            factory.SetValue(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis);

            DataTemplate template = new DataTemplate();
            template.VisualTree = factory;
            newColumn.CellTemplate = template;

            this.AttachToColumn(newColumn);

            //BindingOperations.SetBinding(newColumn.CellTemplate, 
            //newColumn.CellTemplate.Resources.Add("cellValueBinding", new Binding(String.Format("[{0}]", index)));
            //DataTemplateAttachments.SetBindingExpression(newColumn.CellTemplate, new Binding(String.Format("[{0}]", index)));
            this.GridView.Columns.Add(newColumn);
        }

        private void AttachToColumn(GridViewColumn column)
        {
            ((INotifyPropertyChanged)column).PropertyChanged += this.HandleColumnPropertyChanged;
        }

        private void DetachFromColumn(GridViewColumn column)
        {
            ((INotifyPropertyChanged)column).PropertyChanged -= this.HandleColumnPropertyChanged;
        }

        private void HandleColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width")
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    this.widthChanged = true;
                }
                else
                {
                    this.OnColumnWidthChanged(EventArgs.Empty);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (widthChanged)
            {
                widthChanged = false;
                this.OnColumnWidthChanged(EventArgs.Empty);
            }
        }

        public void RemoveHeaderAt(int index)
        {
            if (index >= 0 && index < this.GridView.Columns.Count)
            {
                this.DetachFromColumn(this.GridView.Columns[index]);
            }

            this.GridView.Columns.RemoveAt(index);
        }

        public void SelectIndex(int index)
        {
            if (this.MultiSelect)
            {
                this.SelectedItems.Add(this.items[index]);
            }
            else
            {
                this.SelectedIndex = index;
            }
        }

        public void UnselectIndex(int index)
        {
            if (this.MultiSelect)
            {
                this.SelectedItems.Remove(this.items[index]);
            }
            else
            {
                this.SelectedIndex = -1;
            }
        }

        public IEnumerable<int> SelectedIndexes
        {
            get { return this.selectedIndexCollection; }
        }

        public int PrimarySelectedIndex
        {
            get { return this.SelectedIndex; }
        }

        public void ClearSelectedIndexes()
        {
            if (this.MultiSelect)
            {
                this.SelectedItems.Clear();
            }
            else
            {
                this.SelectedIndex = -1;
            }
        }

        public int TopIndex
        {
            get
            {
                return -1;
                //throw new NotImplementedException();
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        //public object GetTag(int index)
        //{
        //    return this.items[index].Item2;
        //}

        public void EnsureVisible(int index)
        {
            this.ScrollIntoView(this.items[index]);
        }

        public void Select()
        {
            int index = this.SelectedIndex;
            if (index >= 0)
            {
                ListViewItem listViewItem = (ListViewItem)this.ItemContainerGenerator.ContainerFromIndex(index);
                if (listViewItem == null)
                {
                    this.ScrollIntoView(index);
                    listViewItem = (ListViewItem)this.ItemContainerGenerator.ContainerFromIndex(index);
                }

                if (listViewItem != null)
                {
                    this.UpdateLayout();
                    Keyboard.Focus(listViewItem);
                }
                else
                {
                    this.Focus();
                }
            }
            else
            {
                this.Focus();
            }
        }

        //public string GetTextAt(int row, int column)
        //{
        //    //return String.Empty;
        //    return ((List<string>)(this.items[row].Item1))[column];
        //}

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.Source == this)
            {
                this.OnSelectedIndexChanged(EventArgs.Empty);
            }

            base.OnSelectionChanged(e);
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnColumnWidthChanged(EventArgs e)
        {
            EventHandler handler = this.ColumnWidthChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
