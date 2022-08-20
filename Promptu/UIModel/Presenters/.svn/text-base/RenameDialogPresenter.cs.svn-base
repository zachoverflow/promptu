using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class RenameDialogPresenter : DialogPresenterBase<IRenameDialog>
    {
        private string startingName;
        private bool nameMustBeDifferent;

         public RenameDialogPresenter(string startingName)
            : this(startingName, null)
        {
        }

        public RenameDialogPresenter(string startingName, string mainInstructions)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructRenameDialog(),
            startingName,
            mainInstructions)
        {
        }

        public RenameDialogPresenter(IRenameDialog nativeInterface, string startingName, string mainInstructions)
            : base(nativeInterface)
        {
            this.startingName = startingName;
            this.NativeInterface.Text = Localization.Promptu.AppName;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.MainInstructions = mainInstructions ?? Localization.UIResources.RenameDialogMainInstuctions;

            this.NativeInterface.Value.TextChanged += this.HandleValueTextChanged;

            this.NativeInterface.Value.Text = startingName;
            this.NativeInterface.Value.SelectAll();
            this.NativeInterface.Value.Select();
            this.NativeInterface.OkButton.Enabled = false;
        }

        public bool NameMustBeDifferent
        {
            get
            {
                return this.nameMustBeDifferent;
            }

            set
            {
                this.nameMustBeDifferent = value;
                if (!value && !this.NativeInterface.OkButton.Enabled)
                {
                    this.NativeInterface.OkButton.Enabled = true;
                }
            }
        }

        public string Value
        {
            get { return this.NativeInterface.Value.Text; }
            set { this.NativeInterface.Value.Text = value; }
        }

        private void HandleValueTextChanged(object sender, EventArgs e)
        {
            this.NativeInterface.OkButton.Enabled = 
                !this.NameMustBeDifferent 
                || (this.NameMustBeDifferent && this.NativeInterface.Value.Text != this.startingName);
        }
    }
}
