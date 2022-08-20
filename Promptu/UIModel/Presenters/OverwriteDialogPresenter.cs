using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class OverwriteDialogPresenter : DialogPresenterBase<IOverwriteDialog>
    {
        public OverwriteDialogPresenter(bool couldBeMore)
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOverwriteDialog(couldBeMore))
        {
        }

        public OverwriteDialogPresenter(IOverwriteDialog nativeInterface)
            : base(nativeInterface)
        {
            //this.NativeInterface.ChooseMessage = Localization.UIResources.OverwriteDialogChooseAnOption;
            this.NativeInterface.DoActionForRemaining.Text = Localization.UIResources.OverwriteDialogDoForAll;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            //this.NativeInterface.Text = Localization.UIResources.OverwriteDialogText;
        }

        public string MainInstructions
        {
            set { this.NativeInterface.MainInstructions = value; }
        }

        public string Text
        {
            set { this.NativeInterface.Text = value; }
        }

        public OverwriteOption Rename
        {
            set { this.NativeInterface.Rename = value; }
        }

        public OverwriteOption Replace
        {
            set { this.NativeInterface.Replace = value; }
        }

        public OverwriteOption Skip
        {
            set { this.NativeInterface.Skip = value; }
        }

        public bool DoActionForRemaining
        {
            get { return this.NativeInterface.DoActionForRemaining.Checked; }
        }

        public MoveConflictAction Action
        {
            get { return this.NativeInterface.Action; }
        }
    }
}
