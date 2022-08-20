using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.PluginModel.Internals;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class PluginUpdateDialogPresenter : DialogPresenterBase<IPluginUpdateDialog>
    {
        private List<PluginUpdate> pluginUpdates;
        private BackgroundWorker worker;

        public PluginUpdateDialogPresenter(List<PluginUpdate> pluginUpdates)
            : base(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructPluginUpdateDialog())
        {
            this.pluginUpdates = pluginUpdates;
            this.NativeInterface.InstallUpdatesButton.Text = Localization.UIResources.PluginUpdateInstallUpdatesText;
            this.NativeInterface.InstallUpdatesButton.Click += this.HandleInstallUpdatesButtonClick;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.PluginUpdateLaterButonText;
            this.NativeInterface.Text = Localization.Promptu.AppName;

            this.NativeInterface.MainInstructions = pluginUpdates.Count == 1 ?
                Localization.UIResources.PluginUpdatesMainInstructionsSingular :
                Localization.UIResources.PluginUpdatesMainInstructionsPlural;

            this.NativeInterface.SetPluginUpdates(pluginUpdates);
        }

        private void HandleInstallUpdatesButtonClick(object sender, EventArgs e)
        {
            this.NativeInterface.InstallUpdatesButton.Enabled = false;
            this.NativeInterface.CancelButton.Enabled = false;
            if (this.worker == null)
            {
                worker = new BackgroundWorker();
                worker.DoWork += this.RunUpdates;
                worker.RunWorkerCompleted += this.UpdatesFinished;
                worker.RunWorkerAsync();
            }
        }

        private void UpdatesFinished(object sender, EventArgs e)
        {
            this.NativeInterface.CloseWithOk();
        }

        private void RunUpdates(object sender, DoWorkEventArgs e)
        {
            bool errors = false;
            foreach (PluginUpdate update in this.pluginUpdates)
            {
                update.RunUpdate();
                if (update.UpdateError)
                {
                    errors = true;
                }
            }

            if (errors)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}