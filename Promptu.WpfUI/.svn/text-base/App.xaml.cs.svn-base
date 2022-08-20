using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Interop;

namespace ZachJohnson.Promptu.WpfUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        HwndSource hwndSource;

        public void Initialize()
        {
            hwndSource = new HwndSource(new HwndSourceParameters());
            System.Uri resourceLocator = new System.Uri("/Promptu.WpfUI;component/app.xaml", System.UriKind.Relative);

            //System.Windows.Application.LoadComponent(this, resourceLocator);

//#if !NO_WPF
            System.Windows.Application.LoadComponent(this, resourceLocator);
//#endif
        }
    }
}
