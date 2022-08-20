using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu
{
    internal abstract class IconLoadOrder
    {
        private int indexTo;

        public IconLoadOrder(int indexTo)
        {
            this.indexTo = indexTo;
        }

        public int IndexTo
        {
            get { return this.indexTo; }
        }

        public void Load(ISuggestionProvider suggestionProvider, IconSize iconSize)
        {
            this.LoadCore(suggestionProvider, iconSize);
        }

        protected abstract void LoadCore(ISuggestionProvider suggestionProvider, IconSize iconSize);
    }
}
