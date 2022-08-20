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

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for UnhandledExceptionDialog.xaml
    /// </summary>
    internal partial class UnhandledExceptionDialog : Window, IUnhandledExceptionDialog
    {
        public UnhandledExceptionDialog()
        {
            InitializeComponent();
            this.icon.Source = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Error.Handle, new Int32Rect(), BitmapSizeOptions.FromEmptyOptions());
        }

        public string Message
        {
            set { this.message.Text = value; }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public IButton OkButton
        {
            get { return this.okButton; }
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
