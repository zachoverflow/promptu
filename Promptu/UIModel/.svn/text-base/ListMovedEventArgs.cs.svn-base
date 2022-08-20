using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal class ListMovedEventArgs : ListEventArgs
    {
        private int newIndex;

        public ListMovedEventArgs(List list, int newIndex)
            : base(list)
        {
            this.newIndex = newIndex;
        }

        public int NewIndex
        {
            get { return this.newIndex; }
        }
    }
}
