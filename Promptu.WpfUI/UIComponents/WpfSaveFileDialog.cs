using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using Microsoft.Win32;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class WpfSaveFileDialog : ISaveFileDialog
    {
        private SaveFileDialog dialog;

        public WpfSaveFileDialog()
        {
            this.dialog = new SaveFileDialog();
        }

        public string[] Paths
        {
            get { return this.dialog.FileNames; }
        }

        public bool CheckPathExists
        {
            get { return this.dialog.CheckPathExists; }
            set { this.dialog.CheckPathExists = value; }
        }

        public string InitialDirectory
        {
            get { return this.dialog.InitialDirectory; }
            set { this.dialog.InitialDirectory = value; }
        }

        public bool CheckFileExists
        {
            get { return this.dialog.CheckFileExists; }
            set { this.dialog.CheckFileExists = value; }
        }

        public string Path
        {
            get { return this.dialog.FileName; }
            set { this.dialog.FileName = value; }
        }

        public string Filter
        {
            get { return this.dialog.Filter; }
            set { this.dialog.Filter = value; }
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

