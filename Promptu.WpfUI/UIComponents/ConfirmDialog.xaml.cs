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
using ZachJohnson.Promptu.UIModel;
using System.Windows.Interop;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    internal partial class ConfirmDialog : Window, IConfirmDialog
    {
        public ConfirmDialog(UIMessageBoxIcon icon)
        {
            InitializeComponent();

            System.Drawing.Icon systemIconToUse;

            switch (icon)
            {
                case UIMessageBoxIcon.Asterisk:
                    systemIconToUse = System.Drawing.SystemIcons.Asterisk;
                    break;
                case UIMessageBoxIcon.Error:
                    systemIconToUse = System.Drawing.SystemIcons.Error;
                    break;
                case UIMessageBoxIcon.Exclamation:
                    systemIconToUse = System.Drawing.SystemIcons.Exclamation;
                    break;
                case UIMessageBoxIcon.Hand:
                    systemIconToUse = System.Drawing.SystemIcons.Hand;
                    break;
                case UIMessageBoxIcon.Information:
                    systemIconToUse = System.Drawing.SystemIcons.Information;
                    break;
                case UIMessageBoxIcon.Question:
                    systemIconToUse = System.Drawing.SystemIcons.Question;
                    break;
                case UIMessageBoxIcon.Warning:
                    systemIconToUse = System.Drawing.SystemIcons.Warning;
                    break;
                default:
                    systemIconToUse = null;
                    break;
            }

            if (systemIconToUse != null)
            {
                this.icon.Source = Imaging.CreateBitmapSourceFromHIcon(systemIconToUse.Handle, new Int32Rect(), BitmapSizeOptions.FromEmptyOptions());
            }
        }

        public IButton AffirmativeButton
        {
            get { return this.affirmativeButton; }
        }

        public IButton NegativeButton
        {
            get { return this.negativeButton; }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public string SupplementalInstructions
        {
            set { this.supplementalInstuctions.Text = value; }
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
