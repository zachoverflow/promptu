using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ZachJohnson.Promptu.UserModel;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfListListBox : ListBox
    {
        public static readonly DependencyProperty ItemContextMenuProperty =
            DependencyProperty.Register(
            "ItemContextMenu",
            typeof(ContextMenu),
            typeof(WpfListListBox));

        private ObservableCollection<List> lists = new ObservableCollection<List>();

        public WpfListListBox()
        {
            this.DataContext = lists;
        }

        public ObservableCollection<List> Lists
        {
            get { return this.lists; }
        }

        public ContextMenu ItemContextMenu
        {
            get { return (ContextMenu)this.GetValue(ItemContextMenuProperty); }
            set { this.SetValue(ItemContextMenuProperty, value); }
        }

        //public new event System.Windows.Forms.KeyEventHandler KeyDown;

        //public event EventHandler SelectedIndexChanged;

        //public int Count
        //{
        //    get { return this.Items.Count; }
        //}

        //public void Clear()
        //{
        //    this.Items.Clear();
        //}

        //public void Insert(int index, IRadioListItem item)
        //{
        //    this.Items.Insert(index, item);
        //}

        //public void RemoveAt(int index)
        //{
        //    this.Items.RemoveAt(index);
        //}

        //public IRadioListItem this[int index]
        //{
        //    get { return (IRadioListItem)this.Items[index]; }
        //}

        //IRadioListItem IRadioList.SelectedItem
        //{
        //    get { return (IRadioListItem)this.SelectedItem; }
        //}

        //protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        //{
        //    base.OnSelectionChanged(e);
        //    this.OnSelectedIndexChanged(EventArgs.Empty);
        //}

        //protected virtual void OnSelectedIndexChanged(EventArgs e)
        //{
        //    EventHandler handler = this.SelectedIndexChanged;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}
    }
}
