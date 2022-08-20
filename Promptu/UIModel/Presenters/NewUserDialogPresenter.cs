using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Forms;
using ZachJohnson.Promptu.SkinApi;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class NewUserDialogPresenter : DialogPresenterBase<INewUserDialog>
    {
        private NewUserAction newUserAction;
        private HotkeyControlPresenter hotkeyPresenter;
        private NewUserStep currentStep;
        private bool validHotkey = true;
        private bool onlyNewProfile;

        //public NewUserDialogPresenter()
        //     : this(false, false, true)
        //{
        //}

        public NewUserDialogPresenter(NewUserDialogContext context)
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructNewUserDialog(), context)
        {
        }

        public NewUserDialogPresenter(
            INewUserDialog nativeInterface,
            NewUserDialogContext context)
            //bool showExitPromptuButton, 
            //bool filterProfilesAsOnlyAvailable, 
            //bool showUseExistingProfileButton)
            : base(nativeInterface)
        {
            this.CurrentStep = NewUserStep.ChooseBetweenNewAndExisting;

            this.NativeInterface.Text = Localization.Promptu.AppName;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.NextButton.Text = Localization.UIResources.NextButtonText;
            this.NativeInterface.NextButton.Click += this.HandleNextButtonClick;
            this.NativeInterface.BackButton.Text = Localization.UIResources.BackButtonText;
            this.NativeInterface.BackButton.Click += this.HandleBackButtonClick;
            this.NativeInterface.ChooseNewOrExistingStep.ExitPromptu.Click += this.HandleExitPromptuButtonClick;

            this.NativeInterface.ChooseNewOrExistingStep.MainInstructions = Localization.UIResources.NewOrUsedProfileMainInstructions;
            this.NativeInterface.ChooseNewOrExistingStep.NewProfile.Label = Localization.UIResources.CreateNewProfileLabel;
            this.NativeInterface.ChooseNewOrExistingStep.NewProfile.SupplementalExplaination = Localization.UIResources.CreateNewProfileSupplement;
            this.NativeInterface.ChooseNewOrExistingStep.ExistingProfile.Label = Localization.UIResources.UseExistingProfileLabel;
            this.NativeInterface.ChooseNewOrExistingStep.ExitPromptu.Label = Localization.UIResources.ExitPromptuLabel;
            this.NativeInterface.ChooseNewOrExistingStep.ExitPromptu.Visible = false;

            this.NativeInterface.ChooseNewOrExistingStep.NewProfile.Click += this.HandleNewProfileClick;
            this.NativeInterface.ChooseNewOrExistingStep.ExistingProfile.Click += this.HandleExistingProfileClick;

            HotkeyModifierKeys modifiers;
            Keys key;
            string suggestedName;

            Profile.GetDefaults(out modifiers, out key, out suggestedName);

            this.NativeInterface.ProfileBasics.MainInstructions = Localization.UIResources.ProfileBasicsMainInstructions;
            this.NativeInterface.ProfileBasics.NameLabelText = Localization.UIResources.ProfileBasicsNameLabel;
            this.NativeInterface.ProfileBasics.HotkeyLabelText = Localization.UIResources.ProfileBasicsHotkeyLabel;
            this.NativeInterface.ProfileBasics.HotkeySupplement = Localization.UIResources.ProfileBasicsHotkeySupplement;
            this.hotkeyPresenter = new HotkeyControlPresenter(
                this.NativeInterface.ProfileBasics.Hotkey);

            this.hotkeyPresenter.HotkeyChanged += this.EvaluateHotkey;
            this.hotkeyPresenter.NativeInterface.OverrideHotkey.CheckedChanged += this.EvaluateHotkey;

            this.NativeInterface.ProfileBasics.Name.Text = suggestedName;
            this.hotkeyPresenter.UnderlyingHotkeyKey = key;
            this.hotkeyPresenter.HotkeyModifierKeys = modifiers;

            this.NativeInterface.ProfileAdvanced.MainInstructions = Localization.UIResources.ProfileAdvancedMainInstructions;
            this.NativeInterface.ProfileAdvanced.FollowMouse.Text = Localization.UIResources.ProfilePromptFollowMouse;
            this.NativeInterface.ProfileAdvanced.CurrentScreen.Text = Localization.UIResources.ProfilePromptCurrentScreen;
            this.NativeInterface.ProfileAdvanced.NoPositioning.Text = Localization.UIResources.ProfilePromptNoPositioning;
            this.NativeInterface.ProfileAdvanced.FollowMouse.Checked = true;
            //this.NativeInterface.ProfileAdvanced.SupplementalExplaination = Localization.UIResources.ProfileAdvancedSupplement;

            this.NativeInterface.ProfileFinish.MainInstructions = Localization.UIResources.ProfileFinishMainInstructions;
            this.NativeInterface.ProfileFinish.SupplementalInstructions = Localization.UIResources.ProfileFinishSupplementalInstructions;
            this.NativeInterface.ProfileFinish.StartPromptuWithComputer.Text = Localization.UIResources.ProfileFinishStartPromptuWithComputer;
            this.NativeInterface.ProfileFinish.StartPromptuWithComputer.Checked = true;

            this.NativeInterface.ExistingProfile.MainInstructions = Localization.UIResources.ExistingProfileMainInstructions;

            List<ProfilePlacemark> availableProfiles = new List<ProfilePlacemark>();

            foreach (ProfilePlacemark profile in InternalGlobals.ProfilePlacemarks)
            {
                if (!profile.IsExternallyLocked)
                {
                    availableProfiles.Add(profile);
                }
            }

            this.NativeInterface.ExistingProfile.ExistingProfiles.AddRange(availableProfiles);
            this.NativeInterface.ExistingProfile.ExistingProfiles.SelectedIndexChanged += this.HandleExistingProfileChanged;

            this.NativeInterface.NextButton.Visible = false;
            this.NativeInterface.BackButton.Visible = false;

            if (availableProfiles.Count <= 0 || context == NewUserDialogContext.NewProfile)
            {
                this.onlyNewProfile = true;
                this.MoveToNewProfile();
            }

            if (context == NewUserDialogContext.LockedProfile)
            {
                this.NativeInterface.ChooseNewOrExistingStep.MainInstructions = String.Format(CultureInfo.CurrentCulture, Localization.UIResources.LockedProfileMainInstructions, InternalGlobals.CurrentProfile.Locker);
                this.NativeInterface.ChooseNewOrExistingStep.ExistingProfile.Label = Localization.UIResources.ChooseADifferentProfile;
                this.NativeInterface.ChooseNewOrExistingStep.ExitPromptu.Visible = true;
                this.NativeInterface.ChooseNewOrExistingStep.NewProfile.SupplementalExplaination = null;
            }
            else if (context == NewUserDialogContext.CorruptedProfile)
            {
                this.NativeInterface.ChooseNewOrExistingStep.MainInstructions = Localization.UIResources.ProfileCorruptedMainInstructions;
                this.NativeInterface.ChooseNewOrExistingStep.ExistingProfile.Label = Localization.UIResources.ChooseADifferentProfile;
                this.NativeInterface.ChooseNewOrExistingStep.ExitPromptu.Visible = true;
                this.NativeInterface.ChooseNewOrExistingStep.NewProfile.SupplementalExplaination = null;
            }

            //this.NativeInterface.Text = Localization.UIResources.WelcomeSceenText;
            //this.NativeInterface.Title = Localization.UIResources.WelcomeScreenTitle;
            //this.NativeInterface.Instructions = Localization.UIResources.WelcomeScreenInstructions;
            //this.NativeInterface.UseExistingProfileText = Localization.UIResources.UseExistingProfile;
            //this.NativeInterface.CreateNewProfileText = Localization.UIResources.CreateNewProfile;
            
            //this.NativeInterface.OkButtonText = Localization.UIResources.OkButtonText;
            //this.NativeInterface.BackButtonText = Localization.UIResources.BackButtonText;
            //this.NativeInterface.ExitPromptuButtonText = Localization.UIResources.ExitPromptu;

            //this.NativeInterface.SelectedProfileChanged += this.HandleSelectedProfileChanged;
            //this.NativeInterface.BackButtonClick += this.HandleBackButtonClick;
            //this.NqqativeInterface.UseExistingProfileButtonClick += this.HandleUseExistingProfileButtonClick;
            //this.NativeInterface.ExitPromptuButtonClick += this.HandleExitPromptuButtonClick;

            //List<ProfilePlacemark> availableProfiles = new List<ProfilePlacemark>();

            //foreach (ProfilePlacemark profile in Globals.ProfilePlacemarks)
            //{
            //    if (!profile.IsExternallyLocked)
            //    {
            //        availableProfiles.Add(profile);
            //    }
            //}

            //this.NativeInterface.SetVisibleProfiles(availableProfiles);

            //if (availableProfiles.Count == 0)
            //{
            //    this.NativeInterface.ShowUseExistingProfileButton = false;
            //}

            //this.NativeInterface.ShowExitPromptuButton = showExitPromptuButton;

            //if (!showUseExistingProfileButton)
            //{
            //    this.NativeInterface.ShowUseExistingProfileButton = false;
            //}

            //if (!showExitPromptuButton && !showUseExistingProfileButton)
            //{
            //    this.NativeInterface.ShowInstructions = false;
            //}
        }

        //public void SetModeToLockedProfile()
        //{
        //    //this.NativeInterface.Text = Localization.UIResources.LockedProfileDialogText;
        //    //this.NativeInterface.Title = String.Format(Localization.UIResources.LockedProfileDialogTitle, Globals.CurrentProfile.Name, Globals.CurrentProfile.Locker);
        //}

        //public void SetModeToLoadError(ProfilePlacemark corruptedPlacemark)
        //{
        //    //this.NativeInterface.Text = Localization.UIResources.UnabletoLoadProfileDialogText;
        //    //this.NativeInterface.Title = String.Format(Localization.UIResources.UnableToLoadProfile, corruptedPlacemark.Name);
        //}

        //public static NewUserDialogPresenter FromCorrupted()
        //{
        //    return new NewUserDialogPresenter(NewUserDialogContext.CorruptedProfile);
        //}

        //public static NewUserDialogPresenter FromNormal()
        //{
        //    return new NewUserDialogPresenter(NewUserDialogContext.Normal);
        //}

        //public static NewUserDialogPresenter FromLocked()
        //{
        //    return new NewUserDialogPresenter(NewUserDialogContext.LockedProfile, null);
        //}

        public HotkeyControlPresenter HotkeyPresenter
        {
            get { return this.hotkeyPresenter; }
        }

        public string ProfileName
        {
            get { return this.NativeInterface.ProfileBasics.Name.Text; }
        }

        public bool AutostartPromptu
        {
            get { return this.NativeInterface.ProfileFinish.StartPromptuWithComputer.Checked; }
        }

        public PositioningMode PromptPositioningMode
        {
            get 
            {
                if (this.NativeInterface.ProfileAdvanced.NoPositioning.Checked)
                {
                    return PositioningMode.None;
                }
                else if (this.NativeInterface.ProfileAdvanced.CurrentScreen.Checked)
                {
                    return PositioningMode.CurrentScreen;
                }

                return PositioningMode.FollowMouse;
            }
        }

        private NewUserStep CurrentStep
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

        private void HandleBackButtonClick(object sender, EventArgs e)
        {
            NewUserStep previousStep;

            switch (this.CurrentStep)
            {
                case NewUserStep.ProfileAdvanced:
                case NewUserStep.ProfileBasics:
                    previousStep = (NewUserStep)((int)this.CurrentStep - 1);
                    break;
                case NewUserStep.ProfileFinish:
                    previousStep = (NewUserStep)((int)this.CurrentStep - 1);
                    this.NativeInterface.NextButton.Text = Localization.UIResources.NextButtonText;
                    break;
                case NewUserStep.ExistingProfile:
                    previousStep = NewUserStep.ChooseBetweenNewAndExisting;
                    this.NativeInterface.NextButton.Text = Localization.UIResources.NextButtonText;
                    break;
                default:
                    previousStep = this.CurrentStep;
                    break;
            }

            this.CurrentStep = previousStep;
        }

        private void HandleNextButtonClick(object sender, EventArgs e)
        {
            NewUserStep nextStep;

            switch (this.CurrentStep)
            {
                case NewUserStep.ProfileBasics:
                    nextStep = (NewUserStep)((int)this.CurrentStep + 1);
                    break;
                case NewUserStep.ProfileAdvanced:
                    nextStep = (NewUserStep)((int)this.CurrentStep + 1);
                    this.NativeInterface.NextButton.Text = Localization.UIResources.FinishButtonText;
                    break;
                case NewUserStep.ExistingProfile:
                case NewUserStep.ProfileFinish:
                    this.NativeInterface.CloseWithOK();
                    return;
                default:
                    nextStep = this.CurrentStep;
                    break;
            }

            this.CurrentStep = nextStep;
        }

        private void HandleNewProfileClick(object sender, EventArgs e)
        {
            this.MoveToNewProfile();
        }

        private void MoveToNewProfile()
        {
            this.newUserAction = UIModel.NewUserAction.CreateNewProfile;
            this.CurrentStep = NewUserStep.ProfileBasics;
        }

        private void HandleExistingProfileClick(object sender, EventArgs e)
        {
            this.newUserAction = UIModel.NewUserAction.UseExistingProfile;
            this.CurrentStep = NewUserStep.ExistingProfile;
            this.NativeInterface.NextButton.Text = Localization.UIResources.FinishButtonText;
        }

        private void HandleExistingProfileChanged(object sender, EventArgs e)
        {
            if (this.CurrentStep == NewUserStep.ExistingProfile)
            {
                this.NativeInterface.NextButton.Enabled = this.NativeInterface.ExistingProfile.ExistingProfiles.SelectedIndex >= 0;
            }
        }

        private void UpdateCommandButtons()
        {
            bool visible = this.CurrentStep != NewUserStep.ChooseBetweenNewAndExisting;

            bool backButtonMustBeInvisible = this.CurrentStep == NewUserStep.ProfileBasics && this.onlyNewProfile;

            this.NativeInterface.BackButton.Visible = visible && !backButtonMustBeInvisible;
            this.NativeInterface.NextButton.Visible = visible;

            if (this.CurrentStep == NewUserStep.ProfileBasics)
            {
                this.NativeInterface.NextButton.Enabled = this.validHotkey;
            }
            else if (this.CurrentStep == NewUserStep.ExistingProfile)
            {
                this.NativeInterface.NextButton.Enabled = this.NativeInterface.ExistingProfile.ExistingProfiles.SelectedIndex >= 0;
            }
            else
            {
                this.NativeInterface.NextButton.Enabled = true;
            }
        }

        public NewUserAction NewUserAction
        {
            get { return this.newUserAction; }
        }

        public ProfilePlacemark SelectedProfile
        {
            get { return (ProfilePlacemark)this.NativeInterface.ExistingProfile.ExistingProfiles.SelectedValue; }
            //get { return null; }
        }

        //private void HandleBackButtonClick()
        //{
        //    //this.newUserAction = NewUserAction.CreateNewProfile;
        //    //this.NativeInterface.BeginUpdate();
        //    //this.NativeInterface.ShowDefaultView();
        //    //this.NativeInterface.Instructions = Localization.UIResources.WelcomeScreenInstructions;
        //    //this.NativeInterface.EndUpdate();
        //}

        //private void HandleUseExistingProfileButtonClick()
        //{
        //    //this.newUserAction = NewUserAction.UseExistingProfile;
        //    //this.NativeInterface.BeginUpdate();
        //    //this.NativeInterface.ShowProfileSelectionView();
        //    //this.NativeInterface.Instructions = Localization.UIResources.UseExistingProfileInstuctions;
        //    //this.NativeInterface.EndUpdate();
        //    //this.NativeInterface.OkButtonEnabled = this.NativeInterface.ProfileIsSelected;
        //}

        //private void HandleSelectedProfileChanged()
        //{
        //    //this.NativeInterface.OkButtonEnabled = this.NativeInterface.ProfileIsSelected;
        //}

        private void HandleExitPromptuButtonClick(object sender, EventArgs e)
        {
            InternalGlobals.GuiManager.ToolkitHost.ExitApplication();
        }

        private void EvaluateHotkey(object sender, HotkeyChangedEventArgs e)
        {
            bool failed;
            this.EvaluateHotkey(out failed);
            e.Failed = failed;
        }

        private void EvaluateHotkey(object sender, EventArgs e)
        {
            bool failed;
            this.EvaluateHotkey(out failed);
        }

        private void EvaluateHotkey(out bool failed)
        {
            failed = false;
            GlobalHotkey tryHotkey = new GlobalHotkey(this.hotkeyPresenter.HotkeyModifierKeys, this.hotkeyPresenter.HotkeyKey.AssociatedKey, this.hotkeyPresenter.OverideHotkey);

            //if (Globals.CurrentProfile != null)
            //{
            //    if (Globals.CurrentProfile.Hotkey.IsTheSameAs(tryHotkey)
            //        || Globals.CurrentProfile.NotesHotkey.IsTheSameAs(tryHotkey))
            //    {
            //        return;
            //    }
            //}

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
