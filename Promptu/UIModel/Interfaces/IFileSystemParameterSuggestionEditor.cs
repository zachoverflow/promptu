using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IFileSystemParameterSuggestionEditor : IDialog, IThreadingInvoke
    {
        IErrorPanel ErrorPanel { get; }

        IButton OkButton { get; }

        IButton CancelButton { get; }

        ITextInput Filter { get; }

        string MainInstructions { set; }

        string FilterSupplementalInstructions { set; }

        bool IsCreatedAndNotDisposing { get; }
    }
}
