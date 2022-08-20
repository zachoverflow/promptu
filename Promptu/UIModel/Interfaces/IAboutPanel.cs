using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IAboutPanel
    {
        IButton CheckForUpdates { get; }

        string Version { set; }

        string Copyright { set; }

        ReleaseType ReleaseType { set; }

        string WebsiteLinkText { set; }

        event EventHandler WebsiteLinkClicked;
    }
}
