//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.UIModel.Interfaces;
//using ZachJohnson.Promptu.UserModel;
//using System.Windows.Forms;
//using System.ComponentModel;

//namespace ZachJohnson.Promptu.UIModel.Presenters
//{
//    internal class NewProfileDialogPresenter
//    {
//        private INewProfileDialog dialog;
//        private bool showFurtherInstructions;
//        private Profile profile;
//        private HotkeyControlPresenter hotkeyPresenter;
//        private int step;

//        public NewProfileDialogPresenter(INewProfileDialog dialog, Profile profile, bool showFurtherInstructions)
//        {
//            if (dialog == null)
//            {
//                throw new ArgumentNullException("dialog");
//            }

//            this.dialog = dialog;

//            this.hotkeyPresenter = new HotkeyControlPresenter(dialog.HotkeyControl);

//            this.showFurtherInstructions = showFurtherInstructions;
//            this.profile = profile;

//            this.dialog.Text = Localization.UIResources.NewProfileSetupDialogText;
//            this.dialog.NextButtonText = Localization.UIResources.NextButtonText;
//            this.dialog.BackButtonText = Localization.UIResources.BackButtonText;
//            this.dialog.NameInstructions = Localization.UIResources.NewProfileSetupDialogNameInstructions;
//            this.dialog.ProfileName = profile.Name;

//            try
//            {
//                this.hotkeyPresenter.UnderlyingHotkeyKey = profile.Hotkey.Key;
//            }
//            catch (ArgumentException)
//            {
//                profile.Hotkey.Key = Keys.Q;
//                this.hotkeyPresenter.UnderlyingHotkeyKey = Keys.Q;
//            }

//            this.hotkeyPresenter.HotkeyModifierKeys = profile.Hotkey.ModifierKeys;
//            this.hotkeyPresenter.HotkeyChanged += this.EvaluateHotkey;

//            this.dialog.ClosingWithCancel += this.HandleClosingWithCancel;

//            if (this.dialog.MoveForwardAndBackOnInit)
//            {
//                this.GoForwardOneStep();
//                this.GoBackOneStep();
//            }
//        }

//        public HotkeyControlPresenter HotkeyPresenter
//        {
//            get { return this.hotkeyPresenter; }
//        }

//        public string ProfileName
//        {
//            get { return this.dialog.ProfileName; }
//            set { this.dialog.ProfileName = value; }
//        }

//        public UIDialogResult ShowDialog()
//        {
//            return this.dialog.ShowModal();
//        }

//        private void EvaluateHotkey(object sender, HotkeyChangedEventArgs e)
//        {
//            GlobalHotkey tryHotkey = new GlobalHotkey(this.hotkeyPresenter.HotkeyModifierKeys, this.hotkeyPresenter.HotkeyKey.AssociatedKey, this.hotkeyPresenter.OverideHotkey);

//            if (Globals.CurrentProfile != null)
//            {
//                if (Globals.CurrentProfile.Hotkey.IsTheSameAs(tryHotkey)
//                    || Globals.CurrentProfile.NotesHotkey.IsTheSameAs(tryHotkey))
//                {
//                    return;
//                }
//            }

//            try
//            {
//                tryHotkey.Register();
//            }
//            catch (HotkeyException)
//            {
//                if (!this.hotkeyPresenter.RollingBack)
//                {
//                    e.Failed = true;
//                    UIMessageBox.Show(
//                        String.Format(Localization.MessageFormats.HotkeyUnavailable, Utilities.ConvertHotkeyToString(tryHotkey.ModifierKeys, tryHotkey.Key)),
//                        Localization.Promptu.AppName,
//                        UIMessageBoxButtons.OK,
//                        UIMessageBoxIcon.Error);

//                    this.hotkeyPresenter.RollbackToPevious();
//                }
//            }
//            finally
//            {
//                tryHotkey.Unregister();
//            }
//        }

//        private void UpdateFurtherInstructions()
//        {
//            this.dialog.FurtherInstructions = String.Format(
//                Localization.UIResources.NewProfileFurtherInstructions,
//                Utilities.ConvertHotkeyToString(this.hotkeyPresenter.HotkeyModifierKeys, this.hotkeyPresenter.HotkeyKey.AssociatedKey));
//        }

//        private void HandleClosingWithCancel(object sender, CancelEventArgs e)
//        {
//            e.Cancel = UIMessageBox.Show(
//                    Localization.UIResources.ConfirmExitCreateNewProfileDialog,
//                    Localization.Promptu.AppName,
//                    UIMessageBoxButtons.YesNoCancel,
//                    UIMessageBoxIcon.Warning,
//                    UIMessageBoxResult.No) != UIMessageBoxResult.Yes;
//        }

//        private void GoForwardOneStep()
//        {
//            switch (this.step)
//            {
//                case 0:
//                    this.dialog.BackButtonEnabled = true;
//                    if (!this.showFurtherInstructions)
//                    {
//                        this.dialog.NextButtonText = Localization.UIResources.FinishButtonText;
//                    }

//                    break;
//                case 1:
//                    if (this.showFurtherInstructions)
//                    {
//                        this.UpdateFurtherInstructions();
//                        this.dialog.NextButtonText = Localization.UIResources.FinishButtonText;
//                    }
//                    else
//                    {
//                        this.dialog.CloseWithOk();
//                    }

//                    break;
//                default:
//                    this.dialog.CloseWithOk();
//                    break;
//            }

//            this.dialog.MoveToNextPanel(this.showFurtherInstructions);
//            this.step++;
//        }

//        private void GoBackOneStep()
//        {
//            switch (this.step)
//            {
//                case 1:
//                    this.dialog.BackButtonEnabled = false;
//                    if (!this.showFurtherInstructions)
//                    {
//                        this.dialog.NextButtonText = Localization.UIResources.NextButtonText;
//                    }

//                    break;
//                case 2:
//                    this.dialog.NextButtonText = Localization.UIResources.NextButtonText;
//                    break;
//                default:
//                    return;
//            }

//            this.dialog.MoveToPreviousPanel();
//            this.step--;
//        }
//    }
//}
