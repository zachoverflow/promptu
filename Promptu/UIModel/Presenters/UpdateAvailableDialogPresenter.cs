using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class UpdateAvailableDialogPresenter : DialogPresenterBase<IUpdateAvailableDialog>
    {
        public UpdateAvailableDialogPresenter()
            : base(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructUpdateAvailableDialog())
        {
            this.NativeInterface.MainInstructions = Localization.UIResources.UpdateAvailableMainInstructions;
            this.NativeInterface.SupplementalInstructions = Localization.UIResources.UpdateAvailableSupplement;
            this.NativeInterface.Text = Localization.Promptu.AppName;
            this.NativeInterface.RemindMeLater.Text = Localization.UIResources.RemindMeLaterButtonText;
            this.NativeInterface.InstallNow.Text = Localization.UIResources.InstallNowButtonText;
        }
    }
}
