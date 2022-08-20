using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfTabControl : TabControl, ITabControl
    {
        public WpfTabControl()
        {
        }

        public event EventHandler SelectedTabChanged;

        public int SelectedTabIndex
        {
            get
            {
                return this.SelectedIndex;
            }
            set
            {
                this.SelectedIndex = value;
            }
        }

        public void Insert(int index, ITabPage tabPage)
        {
            this.Items.Insert(index, tabPage);
        }

        public void Remove(ITabPage tabPage)
        {
            this.Items.Remove(tabPage);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.Source == this)
            {
                base.OnSelectionChanged(e);
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
    }
}
