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

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for OverwriteDialog.xaml
    /// </summary>
    internal partial class OverwriteDialog : Window, IOverwriteDialog
    {
        private MoveConflictAction action = MoveConflictAction.Cancel;

        public OverwriteDialog(bool showDoForRemaining)
        {
            InitializeComponent();

            this.rename.Click += this.HandleRenameClick;
            this.replace.Click += this.HandleReplaceClick;
            this.skip.Click += this.HandleSkipClick;


            if (!showDoForRemaining)
            {
                this.doForTheRemaining.Visibility = Visibility.Collapsed;
            }
        }

        public UIModel.MoveConflictAction Action
        {
            get { return this.action; }
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public UIModel.OverwriteOption Rename
        {
            set { this.rename.DataContext = value; }
        }

        public UIModel.OverwriteOption Replace
        {
            set { this.replace.DataContext = value; }
        }

        public UIModel.OverwriteOption Skip
        {
            set { this.skip.DataContext = value; }
        }

        public ICheckBox DoActionForRemaining
        {
            get { return this.doForTheRemaining; }
        }

        public IButton CancelButton
        {
            get { return this.cancelButton; }
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

        private void HandleRenameClick(object sender, EventArgs e)
        {
            this.action = MoveConflictAction.Rename;
            this.DialogResult = true;
        }

        private void HandleReplaceClick(object sender, EventArgs e)
        {
            this.action = MoveConflictAction.Replace;
            this.DialogResult = true;
        }

        private void HandleSkipClick(object sender, EventArgs e)
        {
            this.action = MoveConflictAction.Skip;
            this.DialogResult = true;
        }
    }
}
