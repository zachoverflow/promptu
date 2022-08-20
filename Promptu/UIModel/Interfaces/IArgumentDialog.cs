using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IArgumentDialog : IDialog
    {
        IButton OkButton { get; }

        IButton CancelButton { get; }

        ITextInput Arguments { get; }

        string MainInstructions { set; }
    }
}
