using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.PluginModel.Internals;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IPluginUpdateDialog : IDialog
    {
        IButton InstallUpdatesButton { get; }

        IButton CancelButton { get; }

        string MainInstructions { set; }

        void SetPluginUpdates(IEnumerable<PluginUpdate> pluginUpdates);

        void CloseWithOk();
    }
}
