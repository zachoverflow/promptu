using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IHotkeyInUseDialog : IDialog //INotifyInitFinished
    {
        event CancelEventHandler ClosingWithCancel;

        IButton OkButton { get; }

        IButton CancelButton { get; }

        HotkeyInUseResolutionStep CurrentStep { set; }

        IChooseOverrideOrNewHotkey ChooseOverrideOrNewHotkey { get; }

        IHotkeyInUseNewHotkey NewHotkey { get; }

        void CloseWithOK();
    }
}
