using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IDownloadProgressDialog : IDialog, IThreadingInvoke
    {
        string MainInstructions { set; }

        void Show();

        void Close();

        IIndicatesProgress ProgressIndicator { set; }
    }
}
