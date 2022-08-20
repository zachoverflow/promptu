//-----------------------------------------------------------------------
// <copyright file="FileConversionInfo.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public class FileConversionInfo
    {
        private string filter;
        private FileDialogType dialogType;

        public FileConversionInfo(string filter, FileDialogType dialogType)
        {
            this.filter = filter;
            this.dialogType = dialogType;
        }

        public string Filter
        {
            get { return this.filter; }
        }

        public FileDialogType DialogType
        {
            get { return this.dialogType; }
        }
    }
}