using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IInstalledPluginsPanel
    {
        event EventHandler<ObjectEventArgs<PromptuPlugin>> CreatorContactLinkClicked;

        event EventHandler<ObjectEventArgs<PromptuPlugin>> ConfigurePluginClicked;

        event EventHandler<ObjectEventArgs<PromptuPlugin>> TogglePluginEnabledClicked;

        event EventHandler<ObjectEventArgs<PromptuPlugin>> RemovePluginClicked;

        string MainInstructions { set; }

        void ClearPlugins();

        void AddPlugins(IEnumerable<PromptuPlugin> plugins);

        IButton CheckForUpdates { get; }
    }
}
