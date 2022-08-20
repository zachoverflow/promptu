using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IProfileBasics
    {
        string MainInstructions { set; }

        string NameLabelText { set; }

        ITextInput Name { get; }

        string HotkeyLabelText { set; }

        IHotkeyControl Hotkey { get; }

        string HotkeySupplement { set; }
    }
}
