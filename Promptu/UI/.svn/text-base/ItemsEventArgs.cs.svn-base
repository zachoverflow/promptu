using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UI
{
    internal class ItemsEventArgs<T> : EventArgs
    {
        private IEnumerable<T> items;

        public ItemsEventArgs(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.items = items;
        }

        public IEnumerable<T> Items
        {
            get { return this.items; }
        }
    }
}
