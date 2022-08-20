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
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for HotkeyInUseDialog.xaml
    /// </summary>
    internal partial class HotkeyInUseDialog : Window, IHotkeyInUseDialog
    {
        private HotkeyInUseResolutionStep? currentStep;
        private ChooseOverrideOrNewHotkey chooseOverrideOrNewHotkeyStep;
        private HotkeyInUseNewHotkey newHotkey;

        public HotkeyInUseDialog()
        {
            InitializeComponent();

            this.chooseOverrideOrNewHotkeyStep = new ChooseOverrideOrNewHotkey();
            this.newHotkey = new HotkeyInUseNewHotkey();
        }

        public event System.ComponentModel.CancelEventHandler ClosingWithCancel;

        public IButton OkButton
        {
            get { return this.okButton; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public IChooseOverrideOrNewHotkey ChooseOverrideOrNewHotkey
        {
            get { return this.chooseOverrideOrNewHotkeyStep; }
        }

        public IHotkeyInUseNewHotkey NewHotkey
        {
            get { return this.newHotkey; }
        }

        public HotkeyInUseResolutionStep CurrentStep
        {
            set
            {
                if (value != this.currentStep)
                {
                    switch (value)
                    {
                        case HotkeyInUseResolutionStep.ChooseOverrideOrNewHotkey:
                            this.stepContainer.Child = this.chooseOverrideOrNewHotkeyStep;
                            this.chooseOverrideOrNewHotkeyStep.overrideHotkey.Focus();
                            break;
                        case HotkeyInUseResolutionStep.NewHotkey:
                            this.stepContainer.Child = this.newHotkey;
                            break;
                        default:
                            break;
                    }

                    this.currentStep = value;
                }
            }
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == false || this.DialogResult == null)
            {
                this.OnClosingWithCancel(e);
            }

            base.OnClosing(e);
        }

        protected virtual void OnClosingWithCancel(CancelEventArgs e)
        {
            CancelEventHandler handler = this.ClosingWithCancel;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
