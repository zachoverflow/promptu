using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IValueListEditor : IDialog
    {
        string MainInstructions { set; }

        string NameLabelText { set; }

        ITextInput Name { get; }

        IButton OkButton { get; }

        IButton CancelButton { get; }

        ICheckBox NamespaceInterpretation { get; }

        ICheckBox TranslateValues { get; }

        ICollectionEditor ValueListContentsPanel { get; }

        void CloseWithOk();
    }
}
