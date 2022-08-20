using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ListViewSelectedIndexCollection : IEnumerable<int>
    {
        private ListView listView;

        public ListViewSelectedIndexCollection(ListView listView)
        {
            this.listView = listView;
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (object item in this.listView.SelectedItems)
            {
                yield return this.listView.Items.IndexOf(item);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
