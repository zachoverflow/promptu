using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal class ListDragDropEventArgs : DragDropEventArgs
    {
        private List list;

        public ListDragDropEventArgs(List list, UIDragDropEffects allowedEffects, DataGetter dataGetter)
            : base(allowedEffects, dataGetter)
        {
            this.list = list;
        }

        public List List
        {
            get { return this.list; }
        }
    }
}
