using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal class ListEventArgs : EventArgs
    {
        private List list;

        public ListEventArgs(List list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            this.list = list;
        }

        public List List
        {
            get { return this.list; }
        }
    }
}
