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
using ZachJohnson.Promptu.SkinApi;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for AppearanceTabPanel.xaml
    /// </summary>
    internal partial class AppearanceTabPanel : UserControl, IAppearanceTabPanel
    {
        private BulkObservableCollection<PromptuSkin> skins;

        public AppearanceTabPanel()
        {
            InitializeComponent();

            this.skins = new BulkObservableCollection<PromptuSkin>();
            this.skinsListBox.DataContext = skins;
            this.skinsListBox.SelectionChanged += this.HandleSkinListBoxSelectionChanged;
            this.skinsListBox.AddHandler(Button.ClickEvent, new RoutedEventHandler(this.HandleItemButtonClick));
        }

        public event EventHandler<ObjectEventArgs<PromptuSkin>> CreatorContactLinkClicked;

        public event EventHandler<ObjectEventArgs<PromptuSkin>> ConfigureSkinClicked; 

        public event EventHandler SelectedSkinChanged;

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public PromptuSkin SelectedSkin
        {
            get 
            {
                //int selectedIndex = this.skinsListBox.SelectedIndex;
                //if (selectedIndex >= 0 && selectedIndex < this.skins.Count)
                //{
                //    return this.skins[selectedIndex];
                //}

                //return null;
                return (PromptuSkin)this.skinsListBox.SelectedItem;
            }

            set 
            {
                this.skinsListBox.SelectedItem = value;
                //this.skinsListBox.SelectedIndex = this.skins.IndexOf(value);
            }
        }

        public void ClearSkins()
        {
            this.skins.Clear();
        }

        public void AddSkins(IEnumerable<PromptuSkin> skins)
        {
            this.skins.AddRange(skins);
        }

        private void HandleItemButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            //string tag;
            //if (button == null || (tag = button.Tag as String) == null)
            //{
            //    return;
            //}

            switch (button.Name)
            {
                case "Configure":
                    this.OnConfigureSkinClicked(new ObjectEventArgs<PromptuSkin>((PromptuSkin)(button).DataContext));
                    break;
                case "ContactLink":
                    if ((System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        this.OnCreatorContactLinkClicked(new ObjectEventArgs<PromptuSkin>((PromptuSkin)(button).DataContext));
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

        //public void SkinCreatorLinkClick(object sender, RoutedEventArgs e)
        //{
        //    if ((System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        //    {
        //        this.OnCreatorContactLinkClicked(new ObjectEventArgs<PromptuSkin>((PromptuSkin)((FrameworkElement)sender).DataContext));
        //    }
        //    else
        //    {
        //        e.Handled = false;
        //    }
        //}

        protected virtual void OnCreatorContactLinkClicked(ObjectEventArgs<PromptuSkin> e)
        {
            EventHandler<ObjectEventArgs<PromptuSkin>> handler = this.CreatorContactLinkClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnConfigureSkinClicked(ObjectEventArgs<PromptuSkin> e)
        {
            EventHandler<ObjectEventArgs<PromptuSkin>> handler = this.ConfigureSkinClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedSkinChanged(EventArgs e)
        {
            EventHandler handler = this.SelectedSkinChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleSkinListBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (e.Source == this.skinsListBox)
            {
                this.OnSelectedSkinChanged(EventArgs.Empty);
            }
        }
    }
}
