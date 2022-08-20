using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Windows.Data;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    [TemplatePart(Name = "PART_List", Type = typeof(Selector))]
    [TemplatePart(Name = "PART_ItemContent", Type = typeof(ContentControl))]
    internal class SuperTabControl : Control, ITabControl
    {
        private Selector list;
        private ContentControl itemContent;
        private ObservableCollection<SuperTab> tabs = new ObservableCollection<SuperTab>();
        private int? selectedIndex;

        public SuperTabControl()
        {
            this.AddHandler(Selector.SelectionChangedEvent, new RoutedEventHandler(this.HandleSelectionChanged));
        }

        public ObservableCollection<SuperTab> Tabs
        {
            get 
            { 
                return this.tabs; 
            }

            //set 
            //{
            //    this.tabs = value;
            //    this.list.DataContext = value;
            //}
        }

        public override void OnApplyTemplate()
        {
            DependencyObject listObject = GetTemplateChild("PART_List");
            Selector list = listObject as Selector;

            if (listObject != null && list == null)
            {
                throw new ArgumentException("'PART_List' is not a 'Selector'.");
            }

            DependencyObject itemContentObject = GetTemplateChild("PART_ItemContent");
            ContentControl itemContent = itemContentObject as ContentControl;

            if (itemContentObject != null && itemContent == null)
            {
                throw new ArgumentException("'PART_ItemContent' is not a 'ContentControl'.");
            }

            list.DataContext = this.tabs;
            list.SetBinding(Selector.ItemsSourceProperty, new Binding());

            if (list != null && itemContent != null)
            {
                Binding contentBinding = new Binding("SelectedItem.Content");
                contentBinding.Source = list;
                itemContent.SetBinding(ContentControl.ContentProperty, contentBinding);
            }

            if (this.selectedIndex != null)
            {
                list.SelectedIndex = this.selectedIndex.Value;
            }

            this.list = list;
            this.itemContent = itemContent;
        }

        public event EventHandler SelectedTabChanged;

        public int SelectedTabIndex
        {
            get
            {
                return this.list != null ? this.list.SelectedIndex : (this.selectedIndex ?? -1);
            }
            set
            {
                if (this.list == null)
                {
                    this.selectedIndex = value;
                }
                else
                {
                    this.list.SelectedIndex = value;
                }
            }
        }

        public void Insert(int index, ITabPage tabPage)
        {
            bool wasZero = Tabs.Count == 0;
            this.tabs.Insert(index, (SuperTab)tabPage);

            if (wasZero)
            {
                this.selectedIndex = 0;
            }
        }

        public void Remove(ITabPage tabPage)
        {
            this.tabs.Remove((SuperTab)tabPage);
        }

        private void HandleSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (e.Source == this.list)
            {
                this.OnSelectedTabChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnSelectedTabChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedTabChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //private void HandleSelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    if (this.list == null)
        //    {
        //        return;
        //    }

        //    if (e.Source == this.list)
        //    {
        //        if (this.itemContent != null)
        //        {
        //            //this.itemContent.Content = 
        //        }
        //    }
        //}
    }
}
