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
    /// Interaction logic for ChooseOverrideOrNewHotkey.xaml
    /// </summary>
    internal partial class ChooseOverrideOrNewHotkey : UserControl, IChooseOverrideOrNewHotkey
    {
        public ChooseOverrideOrNewHotkey()
        {
            InitializeComponent();
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public ICommandLink OverrideHotkey
        {
            get { return this.overrideHotkey; }
        }

        public ICommandLink ChooseNewHotkey
        {
            get { return this.newHotkey; }
        }
    }
}
