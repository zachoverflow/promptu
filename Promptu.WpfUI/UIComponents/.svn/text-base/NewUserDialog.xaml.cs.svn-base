using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Interop;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for NewUserDialog.xaml
    /// </summary>
    internal partial class NewUserDialog : Window, INewUserDialog
    {
        private NewUserStep? currentStep;
        private ChooseNewOrExistingProfile chooseNewOrExistingStep;
        private ProfileBasics profileBasics;
        private ProfileAdvanced profileAdvanced;
        private ProfileFinish profileFinish;
        private ExistingProfile existingProfile;

        public NewUserDialog()
        {
            InitializeComponent();

            this.chooseNewOrExistingStep = new ChooseNewOrExistingProfile();
            this.profileBasics = new ProfileBasics();
            this.profileAdvanced = new ProfileAdvanced();
            this.profileFinish = new ProfileFinish();
            this.existingProfile = new ExistingProfile();

            this.Loaded += delegate { this.Activate(); };
            //System.Drawing.Icon icon = new System.Drawing.Icon(WpfToolkitImages.NativeAppIcon, new System.Drawing.Size(48, 48));

            //this.promptuIcon.Source = Imaging.CreateBitmapSourceFromHIcon(
            //    WpfToolkitImages.NativeAppIconHandle, 
            //    new Int32Rect(0,0,32,32), 
            //    BitmapSizeOptions.FromEmptyOptions());
            //this.promptuIcon.Source = WpfToolkitImages.NativeAppIcon;
        }

        public NewUserStep CurrentStep
        {
            set 
            {
                if (value != this.currentStep)
                {
                    switch (value)
                    {
                        case NewUserStep.ChooseBetweenNewAndExisting:
                            this.stepContainer.Child = this.chooseNewOrExistingStep;
                            this.chooseNewOrExistingStep.newProfile.Focus();
                            break;
                        case NewUserStep.ProfileBasics:
                            this.stepContainer.Child = this.profileBasics;
                            this.profileBasics.name.Focus();
                            break;
                        case NewUserStep.ProfileAdvanced:
                            this.stepContainer.Child = this.profileAdvanced;
                            break;
                        case NewUserStep.ProfileFinish:
                            this.stepContainer.Child = this.profileFinish;
                            break;
                        case NewUserStep.ExistingProfile:
                            this.stepContainer.Child = this.existingProfile;
                            break;
                        default:
                            break;
                    }

                    this.currentStep = value;
                }
            }
        }

        public IChooseNewOrExistingProfile ChooseNewOrExistingStep
        {
            get { return this.chooseNewOrExistingStep; }
        }

        public IProfileBasics ProfileBasics
        {
            get { return this.profileBasics; }
        }

        public IProfileAdvanced ProfileAdvanced
        {
            get { return this.profileAdvanced; }
        }

        public IProfileFinish ProfileFinish
        {
            get { return this.profileFinish; }
        }

        public IExistingProfile ExistingProfile
        {
            get { return this.existingProfile; } 
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public IButton NextButton
        {
            get { return this.nextButton; }
        }

        public IButton BackButton
        {
            get { return this.backButton; }
        }

        public bool HideNextButton
        {
            set { this.nextButton.Visibility = value ? Visibility.Collapsed : Visibility.Visible; }
        }

        public bool HideBackButton
        {
            set { this.backButton.Visibility = value ? Visibility.Collapsed : Visibility.Visible; }
        }

        public string Text
        {
            get { return this.Title; }
            set { this.Title = value; }
        }

        public UIModel.UIDialogResult ShowModal()
        {
            return WpfToolkitHost.ShowDialogUIDialogResult(this);
        }

        public void CloseWithOK()
        {
            this.DialogResult = true;
        }

        //void IActivatable.Activate()
        //{
        //    this.Activate();
        //}
    }
}
