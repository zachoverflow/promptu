using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfOpenFileDialog : IOpenFileDialog
    {
        private Microsoft.Win32.OpenFileDialog dialog;

        public WpfOpenFileDialog()
        {
            this.dialog = new Microsoft.Win32.OpenFileDialog();
        }

        public string Path
        {
            get { return this.dialog.FileName; }
            set { this.dialog.FileName = value; }
        }

        public string InitialDirectory
        {
            get { return this.dialog.InitialDirectory; }
            set { this.dialog.InitialDirectory = value; }
        }

        public string[] Paths
        {
            get { return this.dialog.FileNames; }
        }

        public string Filter
        {
            get { return this.dialog.Filter; }
            set { this.dialog.Filter = value; }
        }

        public bool Multiselect
        {
            get { return this.dialog.Multiselect; }
            set { this.dialog.Multiselect = value; }
        }

        public bool CheckPathExists
        {
            get { return this.dialog.CheckPathExists; }
            set { this.dialog.CheckPathExists = value; }
        }

        public bool CheckFileExists
        {
            get { return this.dialog.CheckFileExists; }
            set { this.dialog.CheckFileExists = value; }
        }

        public string Text
        {
            get { return this.dialog.Title; }
            set { this.dialog.Title = value; }
        }

        public UIModel.UIDialogResult ShowModal()
        {
            return WpfToolkitHost.ConvertToDialogResult(this.dialog.ShowDialog());
        }
    }
}
