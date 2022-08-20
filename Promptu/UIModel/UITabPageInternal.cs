using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class UITabPageInternal : UITabPage, ILockedInternal
    {
        public UITabPageInternal(string id)
            : base(id)
        {
        }
    }
}
