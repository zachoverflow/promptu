using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class HotkeyInUseDialogPresenter : DialogPresenterBase<IHotkeyInUseDialog>
    {
        private HotkeyControlPresenter hotkeyPresenter;
        private bool exitPromptuIfCanceled;
        private GlobalHotkey invalidHotkey;
        private HotkeyInUseResolutionStep currentStep;
        private bool validHotkey = true;
        private HotkeyInUseResult result;

        public HotkeyInUseDialogPresenter(bool exitPromptuIfCanceled, GlobalHotkey invalidHotkey)
           : this(
                InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructHotkeyInUseDialog(),
                exitPromptuIfCanceled,
                invalidHotkey)
        {
        }

        public HotkeyInUseDialogPresenter(IHotkeyInUseDialog nativeInterface, bool exitPromptuIfCanceled, GlobalHotkey invalidHotkey)
            : base(nativeInterface)
        {
            this.CurrentStep = HotkeyInUseResolutionStep.ChooseOverrideOrNewHotkey;

            this.NativeInterface.Text = Localization.Promptu.AppName;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;

            this.NativeInterface.ChooseOverrideOrNewHotkey.MainInstructions 
                = Localization.UIResources.HotkeyInUseMainInstructions;

            this.NativeInterface.ChooseOverrideOrNewHotkey.OverrideHotkey.Label
                = Localization.UIResources.HotkeyInUseOverrideLabel;

            this.NativeInterface.ChooseOverrideOrNewHotkey.OverrideHotkey.SupplementalExplaination
                = Localization.UIResources.HotkeyInUseOverrideSupplement;

            this.NativeInterface.ChooseOverrideOrNewHotkey.ChooseNewHotkey.Label
                = Localization.UIResources.HotkeyInUseNewHotkeyLabel;

            this.NativeInterface.NewHotkey.MainInstructions = Localization.UIResources.HotkeyInUseNewHotkeyMainInstructions;
            this.hotkeyPresenter = new HotkeyControlPresenter(
                this.NativeInterface.NewHotkey.Hotkey);

            this.hotkeyPresenter.HotkeyChanged += this.EvaluateHotkey;
            this.hotkeyPresenter.NativeInterface.OverrideHotkey.CheckedChanged += this.EvaluateHotkey;

            this.hotkeyPresenter.UpdateTo(invalidHotkey);

            this.NativeInterface.ChooseOverrideOrNewHotkey.ChooseNewHotkey.Click += this.HandleChooseNewHotkeyClick;
            this.NativeInterface.ChooseOverrideOrNewHotkey.OverrideHotkey.Click += this.HandleOverrideHotkeyClick;
            this.NativeInterface.ClosingWithCancel += this.HandleClosingWithCancel;

            //this.hotkeyPresenter = new HotkeyControlPresenter(this.dialog.HotkeyControl);

            //this.dialog.Text = Localization.Promptu.AppName;
            //this.dialog.MainInstructions = message;
            //this.dialog.OkButton.Text = Localization.UIResources.OkButtonText;

            this.invalidHotkey = invalidHotkey;
            this.exitPromptuIfCanceled = exitPromptuIfCanceled;

            //this.hotkeyPresenter.HotkeyChanged += this.EvaluateHotkey;

            //this.dialog.NotifyInitFinished();
        }

        public HotkeyInUseResult Result
        {
            get { return this.result; }
        }

        private HotkeyInUseResolutionStep CurrentStep
        {
            get 
            { 
                return this.currentStep; 
            }

            set 
            { 
                this.NativeInterface.CurrentStep = value;
                this.currentStep = value;
                this.UpdateCommandButtons();
            }
        }

        private void HandleOverrideHotkeyClick(object sender, EventArgs e)
        {
            this.result = HotkeyInUseResult.OverrideHotkey;
            this.NativeInterface.CloseWithOK();
        }

        private void HandleChooseNewHotkeyClick(object sender, EventArgs e)
        {
            this.CurrentStep = HotkeyInUseResolutionStep.NewHotkey;
            this.result = HotkeyInUseResult.NewHotkey;
        }

        private void UpdateCommandButtons()
        {
            this.NativeInterface.OkButton.Visible = this.CurrentStep != HotkeyInUseResolutionStep.ChooseOverrideOrNewHotkey;
            this.NativeInterface.OkButton.Enabled = this.validHotkey;
        }

        public HotkeyControlPresenter HotkeyPresenter
        {
            get { return this.hotkeyPresenter; }
        }

        private void HandleClosingWithCancel(object sender, CancelEventArgs e)
        {
            if (this.exitPromptuIfCanceled)
            {
                if (UIMessageBox.Show(
                   Localization.UIResources.ConfirmExitPromptu,
                   Localization.Promptu.AppName,
                   UIMessageBoxButtons.YesNo,
                   UIMessageBoxIcon.Information,
                   UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                if (UIMessageBox.Show(
                   Localization.UIResources.ConfirmIgnoreHotkey,
                   Localization.Promptu.AppName,
                   UIMessageBoxButtons.YesNo,
                   UIMessageBoxIcon.Information,
                   UIMessageBoxResult.No) == UIMessageBoxResult.Yes)
                {
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void EvaluateHotkey(object sender, HotkeyChangedEventArgs e)
        {
            bool failed = e.Failed;
            this.EvaluateHotkey(ref failed);
            e.Failed = failed;
        }

        private void EvaluateHotkey(object sender, EventArgs e)
        {
            bool failed = false;
            this.EvaluateHotkey(ref failed);
        }

        private void EvaluateHotkey(ref bool failed)
        {
            //if (this.invalidHotkey != null)
            //{
            //    this.NativeInterface.OkButton.Enabled = this.hotkeyPresenter.UnderlyingHotkeyKey != this.invalidHotkey.Key
            //        || this.hotkeyPresenter.HotkeyModifierKeys != this.invalidHotkey.ModifierKeys;
            //}

            GlobalHotkey tryHotkey = new GlobalHotkey(this.hotkeyPresenter.HotkeyModifierKeys, this.hotkeyPresenter.HotkeyKey.AssociatedKey, this.hotkeyPresenter.OverideHotkey);

            try
            {
                tryHotkey.Register();
            }
            catch (HotkeyException)
            {
                if (!this.hotkeyPresenter.RollingBack)
                {
                    failed = true;
                    //UIMessageBox.Show(
                    //    String.Format(Localization.MessageFormats.HotkeyUnavailable, Utilities.ConvertHotkeyToString(tryHotkey.ModifierKeys, tryHotkey.Key)),
                    //    Localization.Promptu.AppName,
                    //    UIMessageBoxButtons.OK,
                    //    UIMessageBoxIcon.Error);

                    //this.hotkeyPresenter.RollbackToPevious();
                }
            }
            finally
            {
                tryHotkey.Unregister();
                //if (!failed)
                //{
                //    this.NativeInterface.OkButton.Enabled = true;
                //}
            }

            this.hotkeyPresenter.NativeInterface.HotkeyStateText = failed
                ? Localization.UIResources.HotkeyStateUnavailable
                : Localization.UIResources.HotkeyStateAvailable;
            this.hotkeyPresenter.NativeInterface.HotkeyState = failed ? UIModel.HotkeyState.Taken : HotkeyState.Available;
            this.validHotkey = !failed;
            this.UpdateCommandButtons();
        }
    }
}
