using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.PTK;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class OptionsDialogPresenter : DialogPresenterBase<IOptionsDialog>
    {
        private SuperTabWidget tabs;

        public OptionsDialogPresenter()
            : base(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsDialog())
        {
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.Text = Localization.UIResources.OptionsDialogDefaultText;
            //this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;

            this.tabs = new SuperTabWidget("Tabs", this.NativeInterface.SuperTabs);
        }

        public string Text
        {
            get { return this.NativeInterface.Text; }
            set { this.NativeInterface.Text = value; }
        }

        public SuperTabWidget Tabs
        {
            get { return this.tabs; }
        }
    }
}
