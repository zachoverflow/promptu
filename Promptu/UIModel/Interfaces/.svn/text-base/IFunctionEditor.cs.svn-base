using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IFunctionEditor : IDialog
    {
        string NameLabelText { set; }

        string AssemblyLabelText { set; }

        string ClassLabelText { set; }

        string MethodLabelText { set; }

        string ReturnsLabelText { set; }

        string MainInstructions { set; }

        ITextInput Name { get; }

        IComboInput Assembly { get; }

        ITextInput Class { get; }

        ITextInput Method { get; }

        IComboInput ReturnValue { get; }

        IButton TestFunctionButton { get; }

        IButton OkButton { get; }

        IButton CancelButton { get; }

        ICollectionEditor FunctionParameterPanel { get; }

        void CloseWithOK();
    }
}
