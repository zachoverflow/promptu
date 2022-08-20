using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    class DummyListBox
    {
        private List<object> items = new List<object>();

        public List<object> Items
        {
            get { return this.items; }
        }

        public object SelectedItem
        {
            get;
            set;
        }

        public int SelectedIndex
        {
            get;
            set;
        }

        public void DoPageUpOrDown(Direction direction)
        {
        }

        public void ScrollIntoView(object obj)
        {
        }

        public void Scroll(Direction direction)
        {
        }

        public int ItemHeight
        {
            get { return 16; }
        }
    }
}
