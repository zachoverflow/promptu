using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IGetPluginsPanel
    {
        event EventHandler<ObjectEventArgs<PromptuPlugin>> CreatorContactLinkClicked;

        event EventHandler<ObjectEventArgs<PromptuPlugin>> InstallPluginClicked;

        string MainInstructions { set; }

        void ClearPlugins();

        void AddPlugins(IEnumerable<PromptuPlugin> plugins);

        IButton PluginBrowseButton { get; }
    }
}
