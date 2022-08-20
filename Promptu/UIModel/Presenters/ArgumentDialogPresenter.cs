using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ArgumentDialogPresenter : DialogPresenterBase<IArgumentDialog>
    {
        public ArgumentDialogPresenter()
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructArgumentDialog())
        {
        }

        public ArgumentDialogPresenter(IArgumentDialog nativeInterface)
            : base(nativeInterface)
        {
            this.NativeInterface.Text = Localization.Promptu.AppName;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.MainInstructions = Localization.UIResources.ArgumentDialogInstructions;
            this.NativeInterface.Arguments.Text = String.Empty;
        }

        public string Arguments
        {
            get { return this.NativeInterface.Arguments.Text; }
            set { this.NativeInterface.Arguments.Text = value; }
        }
    }
}
