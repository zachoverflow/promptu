using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ConfirmDialogPresenter : DialogPresenterBase<IConfirmDialog>
    {
        public ConfirmDialogPresenter(
            UIMessageBoxIcon icon,
            string title,
            string mainInstructions,
            string supplementalInstructions,
            string affirmativeButtonText,
            string negativeButtonText)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructConfirmDialog(icon),
            title,
            mainInstructions,
            supplementalInstructions,
            affirmativeButtonText,
            negativeButtonText)
        {
        }

        public ConfirmDialogPresenter(
            IConfirmDialog nativeInterface,
            string title,
            string mainInstructions,
            string supplementalInstructions,
            string affirmativeButtonText,
            string negativeButtonText)
            : base(nativeInterface)
        {
            this.NativeInterface.Text = title;
            this.NativeInterface.MainInstructions = mainInstructions;
            this.NativeInterface.SupplementalInstructions = supplementalInstructions;
            this.NativeInterface.AffirmativeButton.Text = affirmativeButtonText;
            this.NativeInterface.NegativeButton.Text = negativeButtonText;
        }
    }
}
