using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UserModel
{
    internal class CompositeItem<TItem, TList> : INotifyPropertyChanged
    {
        private TItem item;
        private TList listFrom;

        public CompositeItem(TItem item, TList listFrom)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            else if (listFrom == null)
            {
                throw new ArgumentNullException("listFrom");
            }

            this.item = item;
            this.listFrom = listFrom;
        }

        public TItem Item
        {
            get { return this.item; }
        }

        public TList ListFrom
        {
            get { return this.listFrom; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
