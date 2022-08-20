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

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for UpdateAvailableDialog.xaml
    /// </summary>
    internal partial class UpdateAvailableDialog : Window, IUpdateAvailableDialog
    {
        public UpdateAvailableDialog()
        {
            InitializeComponent();
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public string SupplementalInstructions
        {
            set { this.supplementalInstuctions.Text = value; }
        }

        public IButton InstallNow
        {
            get { return this.installNowButton; }
        }

        public IButton RemindMeLater
        {
            get { return this.remindMeLaterButton; }
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
    }
}
