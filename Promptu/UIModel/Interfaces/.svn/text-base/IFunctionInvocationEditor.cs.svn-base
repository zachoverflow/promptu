using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IFunctionInvocationEditor : IDialog, IThreadingInvoke
    {
        string MainInstructions { set; }

        ITextInput Expression { get; }

        IButton OkButton { get; }

        IButton CancelButton { get; }

        IErrorPanel ErrorPanel { get; }

        bool IsCreatedAndNotDisposing { get; }
    }
}
