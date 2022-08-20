using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.PTK;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    interface IStackWidget : ICollectionWidget
    {
        void SetOrientation(Orientation orientation);

        void SetAutoSizeBasedOn(int index, bool value);

        int? PhysicalSizeContraOrientation { set; }
    }
}
