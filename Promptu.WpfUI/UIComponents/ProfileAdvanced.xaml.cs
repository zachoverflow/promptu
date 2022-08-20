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
    /// Interaction logic for ProfileAdvanced.xaml
    /// </summary>
    internal partial class ProfileAdvanced : UserControl, IProfileAdvanced
    {
        public ProfileAdvanced()
        {
            InitializeComponent();
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public IRadioButton FollowMouse
        {
            get { return this.followMouse; }
        }

        public IRadioButton CurrentScreen
        {
            get { return this.currentScreen; }
        }

        public IRadioButton NoPositioning
        {
            get { return this.none; }
        }

        //public string SupplementalExplaination
        //{
        //    set { this.supplement.Text = value; }
        //}
    }
}
