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
    /// Interaction logic for HotkeyControl.xaml
    /// </summary>
    internal partial class HotkeyControl : UserControl, IHotkeyControl
    {
        //public static readonly DependencyProperty HotkeyStateProperty =
        //    DependencyProperty.Register(
        //        "HotkeyState",
        //        typeof(UIModel.HotkeyState),
        //        typeof(HotkeyControl));

        public HotkeyControl()
        {
            InitializeComponent();
        }

        public ICheckBox Ctrl
        {
            get { return this.ctrl; }
        }

        public ICheckBox Shift
        {
            get { return this.shift; }
        }

        public ICheckBox Alt
        {
            get { return this.alt; }
        }

        public ICheckBox Win
        {
            get { return this.win; }
        }

        public ICheckBox OverrideHotkey
        {
            get { return this.overrideCheckBox; }
        }

        public ICheckBox HotkeyEnabled
        {
            get { return new Dummy.DummyCheckbox(); }
        }

        public IComboInput Key
        {
            get { return this.key; }
        }

        public string HotkeyStateText
        {
            set { this.hotkeyState.Content = value; }
        }

        public UIModel.HotkeyState HotkeyState
        {
            get { return (UIModel.HotkeyState)Custom.GetHotkeyState(this.hotkeyState); }
            set {Custom.SetHotkeyState(this.hotkeyState, value); }
        }
    }
}
