using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class UnhandledExceptionDialogPresenter : DialogPresenterBase<IUnhandledExceptionDialog>
    {
        public UnhandledExceptionDialogPresenter(bool unmanagedException)
            : this(
                InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructUnhandledExceptionDialog(),
                unmanagedException)
        {
        }

        public UnhandledExceptionDialogPresenter(
            IUnhandledExceptionDialog nativeInterface, 
            bool unmanagedException)
            : base(nativeInterface)
        {
            this.NativeInterface.MainInstructions = unmanagedException ?
                Localization.Promptu.UnmanagedUnhandledMainInstructions : 
                Localization.Promptu.ManagedUnhandledMainInstructions;

            this.NativeInterface.Text = Localization.Promptu.AppName;

            this.NativeInterface.Message = unmanagedException ? 
                Localization.Promptu.UnmanagedUnhandledException : 
                Localization.Promptu.ManagedUnhandledException;

            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
        }
    }
}
