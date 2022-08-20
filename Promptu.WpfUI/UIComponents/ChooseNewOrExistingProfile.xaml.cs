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
    /// Interaction logic for ChooseNewOrExistingProfile.xaml
    /// </summary>
    internal partial class ChooseNewOrExistingProfile : UserControl, IChooseNewOrExistingProfile
    {
        public ChooseNewOrExistingProfile()
        {
            InitializeComponent();
        }

        public string MainInstructions
        {
            set { this.mainInstructions.Text = value; }
        }

        public ICommandLink NewProfile
        {
            get { return this.newProfile; }
        }

        public ICommandLink ExistingProfile
        {
            get { return this.existingProfile; }
        }

        public ICommandLink ExitPromptu
        {
            get { return this.exitPromptu; }
        }
    }
}
