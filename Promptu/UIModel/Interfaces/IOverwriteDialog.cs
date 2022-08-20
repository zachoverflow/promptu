using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IOverwriteDialog : IDialog
    {
        MoveConflictAction Action { get; }

        string MainInstructions { set; }

        OverwriteOption Rename { set; }

        OverwriteOption Replace { set; }

        OverwriteOption Skip { set; }

        ICheckBox DoActionForRemaining { get; }

        IButton CancelButton { get; }
    }
}
