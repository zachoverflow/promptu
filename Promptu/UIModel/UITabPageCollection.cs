using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class UITabPageCollection : UIComponentCollection<UITabPage, ITabPage>
    {
        private INativeUICollection<ITabPage> correspondingCollection;

        public UITabPageCollection(INativeUICollection<ITabPage> correspondingCollection)
        {
            if (correspondingCollection == null)
            {
                throw new ArgumentNullException("correspondingCollection");
            }

            this.correspondingCollection = correspondingCollection;
        }

        protected override void RemoveFromUnderlyingCollection(UITabPage item)
        {
            this.correspondingCollection.Remove(item.NativeInterface);
        }

        protected override void InsertIntoUnderlyingCollection(int index, UITabPage item)
        {
            this.correspondingCollection.Insert(index, item.NativeInterface);
        }
    }
}
