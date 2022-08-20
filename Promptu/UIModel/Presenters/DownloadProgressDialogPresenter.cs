using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class DownloadProgressDialogPresenter : DialogPresenterBase<IDownloadProgressDialog>
    {
        public DownloadProgressDialogPresenter()
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructDownloadProgressDialog())
        {
        }

        public DownloadProgressDialogPresenter(IDownloadProgressDialog nativeInterface)
            : base(nativeInterface)
        {
            this.NativeInterface.Text = Localization.Promptu.AppName;
            this.NativeInterface.MainInstructions = Localization.UIResources.UpdateDownloadProgressMainInstructions;
        }

        //public void UpdateStatusText(string newStatus)
        //{
        //    if (this.NativeInterface.InvokeRequired)
        //    {
        //        this.NativeInterface.Invoke(new Action<string>(this.UpdateStatusText), new object[] { newStatus });
        //    }
        //    else
        //    {
        //        this.NativeInterface.StatusText = newStatus;
        //    }
        //}

        public void Show()
        {
            this.NativeInterface.Show();
        }

        public void Close()
        {
            this.NativeInterface.Close();
        }
    }
}
