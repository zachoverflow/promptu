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
    /// Interaction logic for CollisionResolvingDialog.xaml
    /// </summary>
    internal partial class CollisionResolvingDialog : Window, ICollisionResolvingDialog
    {
        public CollisionResolvingDialog()
        {
            InitializeComponent();

            this.userChange.Click += this.HandleUsersChangeClick;
            this.externalChange.Click += this.HandleExternalChangeClick;
        }

        public event EventHandler ResolveToUsersClick;

        public event EventHandler ResolveToExternalClick;

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        //public string YourChangeAlsoDeletesLabelText
        //{
        //    set { }
        //}

        //public string ConflictingChangeAlsoDeletesLabelText
        //{
        //    set { }
        //}

        //public string SubText
        //{
        //    set { }
        //}

        public IButton CancelButton
        {
            get { return this.cancelButton; }
        }

        public string UsersChangeLabel
        {
            set { this.userChange.Label = value; }
        }

        public string UsersChangeSupplement
        {
            set { this.userChange.SupplementalExplaination = value; }
        }

        public string ExternalChangeLabel
        {
            set { this.externalChange.Label = value; }
        }

        public string ExternalChangeSupplement
        {
            set { this.externalChange.SupplementalExplaination = value; }
        }

        public void CloseWithOk()
        {
            this.DialogResult = true;
        }

        public ICheckBox DoForTheNextNConflicts
        {
            get { return this.doForTheRemaining; }
        }

        public void SetUsersChangeInfo(ObjectConflictInfo info)
        {
            this.userChange.DataContext = info;
        }

        public void SetExternalChangeInfo(ObjectConflictInfo info)
        {
            this.externalChange.DataContext = info;
        }

        //public void SetPriorityAlsoDeletesInfo(UserModel.Differencing.VisualDisplayInfo info)
        //{
        //}

        //public void SetSecondaryAlsoDeletesInfo(UserModel.Differencing.VisualDisplayInfo info)
        //{
        //}

        public UIModel.UIDialogResult ShowModal(object owner)
        {
            return WpfToolkitHost.ConvertToDialogResult(WpfToolkitHost.ShowDialog(this, owner as Window));
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

        private void HandleUsersChangeClick(object sender, EventArgs e)
        {
            this.OnResolveToUsersClick(EventArgs.Empty);
        }

        private void HandleExternalChangeClick(object sender, EventArgs e)
        {
            this.OnResolveToExternalClick(EventArgs.Empty);
        }

        protected virtual void OnResolveToUsersClick(EventArgs e)
        {
            EventHandler handler = this.ResolveToUsersClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnResolveToExternalClick(EventArgs e)
        {
            EventHandler handler = this.ResolveToExternalClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
