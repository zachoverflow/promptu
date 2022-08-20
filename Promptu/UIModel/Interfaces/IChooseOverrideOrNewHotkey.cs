using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IChooseOverrideOrNewHotkey
    {
        string MainInstructions { set; }

        ICommandLink OverrideHotkey { get; }

        ICommandLink ChooseNewHotkey { get; }
    }
}
