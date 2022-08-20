//-----------------------------------------------------------------------
// <copyright file="PluginConfigWindowManager.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System.Collections.Generic;
    using System.Globalization;
    using ZachJohnson.Promptu.PTK;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using ZachJohnson.Promptu.UIModel.Presenters;

    internal class PluginConfigWindowManager
    {
        private Dictionary<PromptuPluginEntryPoint, OptionsDialogPresenter> openDialogs = new Dictionary<PromptuPluginEntryPoint, OptionsDialogPresenter>();

        public PluginConfigWindowManager()
        {
        }

        public void ShowConfigFor(PromptuPluginEntryPoint entryPoint)
        {
            // TODO handle if no options
            OptionsDialogPresenter optionsDialog;
            if (this.openDialogs.TryGetValue(entryPoint, out optionsDialog))
            {
                optionsDialog.NativeInterface.ActivateAndBringToFront();
                return;
            }
            
            optionsDialog = new OptionsDialogPresenter();

            int pageId = 0;
            foreach (NamedOptionPage page in entryPoint.Options)
            {
                if (page != null)
                {
                    SuperTabPage superTabPage = new SuperTabPage(pageId.ToString(CultureInfo.InvariantCulture));
                    superTabPage.Text = page.TabName;
                    optionsDialog.Tabs.Add(superTabPage);

                    IPromptuOptionsPanel promptPanel = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructOptionsPanel();
                    promptPanel.MainInstructions = page.MainInstructions;
                    superTabPage.HostedWidget = new ExternalWidget("panel", promptPanel);
                    promptPanel.Editor.Properties = page.Groups;
                }

                pageId++;
            }

            this.openDialogs.Add(entryPoint, optionsDialog);
            InternalGlobals.UISettings.PluginOptionsDialogSettings.ImpartTo(optionsDialog.NativeInterface);
            optionsDialog.ShowDialog();
            InternalGlobals.UISettings.PluginOptionsDialogSettings.UpdateFrom(optionsDialog.NativeInterface);
            this.openDialogs.Remove(entryPoint);
        }
    }
}
