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
    /// Interaction logic for ProfileConfigPanel.xaml
    /// </summary>
    internal partial class ProfileConfigPanel : UserControl, IProfileConfigPanel
    {
        public ProfileConfigPanel()
        {
            InitializeComponent();
        }

        public IComboInput CurrentProfiles
        {
            get { return this.currentProfiles; }
        }

        public IButton DeleteButton
        {
            get { return this.deleteButton; }
        }

        public IButton NewProfileButton
        {
            get { return this.newProfileButton; }
        }

        public IButton RenameButton
        {
            get { return this.renameButton; }
        }
    }
}
