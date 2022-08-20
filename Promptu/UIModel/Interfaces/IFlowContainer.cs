using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.PTK;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IFlowContainer : ICollectionWidget
    {
        void SetOrientation(Orientation orientation);
    }
}
