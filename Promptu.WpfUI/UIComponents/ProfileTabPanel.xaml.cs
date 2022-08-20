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
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for ProfileTabPanel.xaml
    /// </summary>
    internal partial class ProfileTabPanel : UserControl, IProfileTabPanel
    {
        private static readonly DependencyProperty ListSelectorWidthProperty =
            DependencyProperty.Register(
                "ListSelector",
                typeof(double),
                typeof(ProfileTabPanel),
                new PropertyMetadata(HandleListSelectorWidthPropertyChanged));

        private ParameterlessVoid settingChangedCallback;

        public ProfileTabPanel()
        {
            InitializeComponent();
            Binding widthBinding = new Binding("Width");
            widthBinding.Source = this.listSelector;
            widthBinding.Mode = BindingMode.TwoWay;
            this.SetBinding(ListSelectorWidthProperty, widthBinding);
            //this.listSelector.Size
            //((INotifyPropertyChanged)this.listSelector).PropertyChanged += this.HandleListSelectorPropertyChanged;
        }

        public ITabControl ListTabs
        {
            get { return this.listTabs; }
        }

        public IListSelector ListSelector
        {
            get { return this.listSelector; }
        }

        public double ListSelectorWidth
        {
            get { return (double)this.GetValue(ListSelectorWidthProperty);}
            set { this.SetValue(ListSelectorWidthProperty, value);}
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public ParameterlessVoid SettingChangedCallback
        {
            set { this.settingChangedCallback = value; }
        }

        private static void HandleListSelectorWidthPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ProfileTabPanel panel = (ProfileTabPanel)obj;
            panel.NotifySettingChanged();
        }

        //private void HandleListSelectorPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Width")
        //    {
        //        this.NotifySettingChanged();
        //    }
        //}

        private void NotifySettingChanged()
        {
            ParameterlessVoid callback = settingChangedCallback;
            if (callback != null)
            {
                callback();
            }
        }
    }
}
