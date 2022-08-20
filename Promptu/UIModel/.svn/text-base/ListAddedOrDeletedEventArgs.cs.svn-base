using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal class ListAddedOrDeletedEventArgs : ListEventArgs
    {
        private int index;

        public ListAddedOrDeletedEventArgs(List list, int index)
            : base(list)
        {
            this.index = index;
        }

        public int Index
        {
            get { return this.index; }
        }
    }
}
