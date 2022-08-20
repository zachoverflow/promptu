using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    interface IProfileConfigPanel
    {
        IComboInput CurrentProfiles { get; }

        IButton DeleteButton { get; }

        IButton NewProfileButton { get; }

        IButton RenameButton { get; }
    }
}
