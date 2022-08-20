using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IProfileAdvanced
    {
        string MainInstructions { set; }

        IRadioButton FollowMouse { get; }

        IRadioButton CurrentScreen { get; }

        IRadioButton NoPositioning { get; }

        //string SupplementalExplaination { set; }
    }
}
