using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class PassthroughNativeContextMenu : INativeContextMenu
    {
        private INativeUICollection<IGenericMenuItem> items;

        public PassthroughNativeContextMenu(INativeUICollection<IGenericMenuItem> items)
        {
            this.items = items;
        }

        public INativeUICollection<IGenericMenuItem> Items
        {
            get { return this.items; }
        }
    }
}
