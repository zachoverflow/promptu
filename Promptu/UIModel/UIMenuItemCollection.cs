using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    public class UIMenuItemCollection : UIComponentCollection<UIMenuItemBase, IGenericMenuItem>
    {
        private INativeUICollection<IGenericMenuItem> correspondingCollection;

        internal UIMenuItemCollection(INativeUICollection<IGenericMenuItem> correspondingCollection)
        {
            if (correspondingCollection == null)
            {
                throw new ArgumentNullException("correspondingCollection");
            }

            this.correspondingCollection = correspondingCollection;
        }

        protected override void RemoveFromUnderlyingCollection(UIMenuItemBase item)
        {
            this.correspondingCollection.Remove(item.NativeInterface);
        }

        protected override void InsertIntoUnderlyingCollection(int index, UIMenuItemBase item)
        {
            this.correspondingCollection.Insert(index, item.NativeInterface);
        }
    }
}
