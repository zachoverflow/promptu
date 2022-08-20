using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IObjectDisambiguator : IDialog
    {
        IButton OkButton { get; }

        IButton CancelButton { get; }

        //IComboInput ParameterCountComboInput { get; }

        string MainInstructions { set; }

        void SetAmbiguousObjects(IEnumerable<object> ambiguousObjects);

        object SelectedObject { get; set; }
    }
}
