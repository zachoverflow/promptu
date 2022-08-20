using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal class ItemEventArgs<T> : EventArgs
    {
        private T item;

        public ItemEventArgs(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get { return this.item; }
        }
    }
}
