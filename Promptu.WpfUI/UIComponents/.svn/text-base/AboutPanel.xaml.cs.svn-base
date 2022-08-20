using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for AboutPanel.xaml
    /// </summary>
    internal partial class AboutPanel : UserControl, IAboutPanel
    {
        public AboutPanel()
        {
            InitializeComponent();
            this.websiteLink.Click += this.HandleWebsiteLinkClicked;
        }

        public event EventHandler WebsiteLinkClicked;

        public IButton CheckForUpdates
        {
            get { return this.checkForUpdatesButton; }
        }

        public string Version
        {
            set { this.versionInformation.Text = value; }
        }

        public string Copyright
        {
            set { this.copyright.Text = value; }
        }

        public ReleaseType ReleaseType
        {
            set
            {
                switch (value)
                {
                    case ReleaseType.Beta:
                        this.releaseTypeOverlay.Fill = (Brush)this.FindResource("BetaOverlay");
                        break;
                    case ReleaseType.Alpha:
                        this.releaseTypeOverlay.Fill = (Brush)this.FindResource("AlphaOverlay");
                        break;
                    default:
                        break;
                }
            }
        }

        public string WebsiteLinkText
        {
            set { this.websiteLink.Content = value; }
        }

        private void HandleWebsiteLinkClicked(object sender, EventArgs e)
        {
            this.OnWebsiteLinkClicked(EventArgs.Empty);
        }

        protected virtual void OnWebsiteLinkClicked(EventArgs e)
        {
            EventHandler handler = this.WebsiteLinkClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
