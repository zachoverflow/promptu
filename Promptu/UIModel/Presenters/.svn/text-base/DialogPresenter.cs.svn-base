using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal abstract class DialogPresenterBase<T> : PresenterBase<T> where T : IDialog
    {
        public DialogPresenterBase(T nativeInterface)
            : base(nativeInterface)
        {
        }

        public UIDialogResult ShowDialog()
        {
            return this.NativeInterface.ShowModal();
        }
    }
}
