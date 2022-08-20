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
using ZachJohnson.Promptu.PluginModel;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for GetPluginsPanel.xaml
    /// </summary>
    internal partial class GetPluginsPanel : UserControl, IGetPluginsPanel
    {
        private BulkObservableCollection<PromptuPlugin> plugins;

        public GetPluginsPanel()
        {
            InitializeComponent();

            this.plugins = new BulkObservableCollection<PromptuPlugin>();
            this.pluginsListBox.DataContext = plugins;
            this.pluginsListBox.AddHandler(Button.ClickEvent, new RoutedEventHandler(this.HandleItemButtonClick));
        }

        public event EventHandler<ObjectEventArgs<PromptuPlugin>> CreatorContactLinkClicked;

        public event EventHandler<ObjectEventArgs<PromptuPlugin>> InstallPluginClicked;

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public IButton PluginBrowseButton
        {
            get { return this.pluginBrowseButton; }
        }

        private void HandleItemButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;

            switch (button.Name)
            {
                //case "ToggleEnabled":
                //    this.OnTogglePluginEnabledClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)button.DataContext));
                //    break;
                case "Install":
                    this.OnInstallPluginClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)button.DataContext));
                    break;
                case "ContactLink":
                    if ((System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        this.OnCreatorContactLinkClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)button.DataContext));
                    }
                    else
                    {
                        e.Handled = false;
                    }

                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        public void ClearPlugins()
        {
            this.plugins.Clear();
        }

        public void AddPlugins(IEnumerable<PromptuPlugin> plugins)
        {
            this.plugins.AddRange(plugins);
        }

        protected virtual void OnCreatorContactLinkClicked(ObjectEventArgs<PromptuPlugin> e)
        {
            EventHandler<ObjectEventArgs<PromptuPlugin>> handler = this.CreatorContactLinkClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //protected virtual void OnTogglePluginEnabledClicked(ObjectEventArgs<PromptuPlugin> e)
        //{
        //    EventHandler<ObjectEventArgs<PromptuPlugin>> handler = this.TogglePluginEnabledClicked;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        protected virtual void OnInstallPluginClicked(ObjectEventArgs<PromptuPlugin> e)
        {
            EventHandler<ObjectEventArgs<PromptuPlugin>> handler = this.InstallPluginClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
