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
    /// Interaction logic for PluginTabPanel.xaml
    /// </summary>
    internal partial class PluginsTabPanel : UserControl, IPluginsTabPanel
    {
        //private BulkObservableCollection<PromptuPlugin> plugins;

        public PluginsTabPanel()
        {
            InitializeComponent();

            //this.plugins = new BulkObservableCollection<PromptuPlugin>();
            //this.pluginsListBox.DataContext = plugins;
            //this.pluginsListBox.AddHandler(Button.ClickEvent, new RoutedEventHandler(this.HandleItemButtonClick));
            ////this.skinsListBox.SelectionChanged += this.HandleSkinListBoxSelectionChanged;
        }

        //public event EventHandler<ObjectEventArgs<PromptuPlugin>> CreatorContactLinkClicked;

        //public event EventHandler<ObjectEventArgs<PromptuPlugin>> ConfigurePluginClicked;

        //public event EventHandler<ObjectEventArgs<PromptuPlugin>> TogglePluginEnabledClicked;

        //public string MainInstructions
        //{
        //    set { this.mainInstructions.Text = value; }
        //}

        //private void HandleItemButtonClick(object sender, RoutedEventArgs e)
        //{
        //    Button button = e.OriginalSource as Button;
        //    //string tag;
        //    //if (button == null || (tag = button.Tag as String) == null)
        //    //{
        //    //    return;
        //    //}

        //    switch (button.Name)
        //    {
        //        case "ToggleEnabled":
        //            this.OnTogglePluginEnabledClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)button.DataContext));
        //            break;
        //        case "Configure":
        //            this.OnConfigurePluginClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)button.DataContext));
        //            break;
        //        case "ContactLink":
        //            if ((System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        //            {
        //                this.OnCreatorContactLinkClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)button.DataContext));
        //            }
        //            else
        //            {
        //                e.Handled = false;
        //            }

        //            break;
        //        default:
        //            e.Handled = false;
        //            break;
        //    }
        //}

        ////public PromptuSkin SelectedSkin
        ////{
        ////    get 
        ////    {
        ////        //int selectedIndex = this.skinsListBox.SelectedIndex;
        ////        //if (selectedIndex >= 0 && selectedIndex < this.skins.Count)
        ////        //{
        ////        //    return this.skins[selectedIndex];
        ////        //}

        ////        //return null;
        ////        return (PromptuSkin)this.skinsListBox.SelectedItem;
        ////    }

        ////    set 
        ////    {
        ////        this.skinsListBox.SelectedItem = value;
        ////        //this.skinsListBox.SelectedIndex = this.skins.IndexOf(value);
        ////    }
        ////}

        //public void ClearPlugins()
        //{
        //    this.plugins.Clear();
        //}

        //public void AddPlugins(IEnumerable<PromptuPlugin> plugins)
        //{
        //    this.plugins.AddRange(plugins);
        //}

        ////public void PluginCreatorLinkClick(object sender, RoutedEventArgs e)
        ////{
        ////    if ((System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        ////    {
        ////        this.OnCreatorContactLinkClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)((FrameworkElement)sender).DataContext));
        ////    }
        ////    else
        ////    {
        ////        e.Handled = false;
        ////    }
        ////}

        ////public void HandleConfigurePluginClick(object sender, EventArgs e)
        ////{
        ////    this.OnConfigurePluginClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)((FrameworkElement)sender).DataContext));
        ////}

        ////public void HandleToggleEnabledPluginClick(object sender, EventArgs e)
        ////{
        ////    this.OnTogglePluginEnabledClicked(new ObjectEventArgs<PromptuPlugin>((PromptuPlugin)((FrameworkElement)sender).DataContext));
        ////}

        //protected virtual void OnCreatorContactLinkClicked(ObjectEventArgs<PromptuPlugin> e)
        //{
        //    EventHandler<ObjectEventArgs<PromptuPlugin>> handler = this.CreatorContactLinkClicked;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        //protected virtual void OnTogglePluginEnabledClicked(ObjectEventArgs<PromptuPlugin> e)
        //{
        //    EventHandler<ObjectEventArgs<PromptuPlugin>> handler = this.TogglePluginEnabledClicked;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        //protected virtual void OnConfigurePluginClicked(ObjectEventArgs<PromptuPlugin> e)
        //{
        //    EventHandler<ObjectEventArgs<PromptuPlugin>> handler = this.ConfigurePluginClicked;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        ////protected virtual void OnSelectedSkinChanged(EventArgs e)
        ////{
        ////    EventHandler handler = this.SelectedSkinChanged;
        ////    if (handler != null)
        ////    {
        ////        handler(this, e);
        ////    }
        ////}

        ////private void HandleSkinListBoxSelectionChanged(object sender, RoutedEventArgs e)
        ////{
        ////    if (e.Source == this.skinsListBox)
        ////    {
        ////        this.OnSelectedSkinChanged(EventArgs.Empty);
        ////    }
        ////}

        public ITabControl SuperTabs
        {
            get { return this.tabs; }
        }
    }
}
