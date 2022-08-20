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
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for HotkeyBindingWrapper.xaml
    /// </summary>
    internal partial class HotkeyBindingWrapper : UserControl
    {
        public static readonly DependencyProperty HotkeyModifierKeysProperty =
            DependencyProperty.Register(
                "HotkeyModifierKeys",
                typeof(HotkeyModifierKeys),
                typeof(HotkeyBindingWrapper),
                new PropertyMetadata(HandleHotkeyModifierKeysChanged));

        public static readonly DependencyProperty HotkeyKeyProperty =
            DependencyProperty.Register(
                "HotkeyKey",
                typeof(System.Windows.Forms.Keys),
                typeof(HotkeyBindingWrapper),
                new PropertyMetadata(HandleHotkeyKeyChanged));

        public static readonly DependencyProperty OverrideHotkeyProperty =
            DependencyProperty.Register(
                "OverrideHotkey",
                typeof(bool),
                typeof(HotkeyBindingWrapper),
                new PropertyMetadata(HandleOverrideHotkeyChanged));

        public static readonly DependencyProperty HotkeyProperty =
            DependencyProperty.Register(
                "Hotkey",
                typeof(GlobalHotkey),
                typeof(HotkeyBindingWrapper),
                new PropertyMetadata(HandleHotkeyChanged));

        private HotkeyControlPresenter presenter;
        private HotkeyEvaluationContext hotkeyEvaulator;

        public HotkeyBindingWrapper()
        {
            InitializeComponent();

            this.presenter = new HotkeyControlPresenter(this.hotkeyControl);
            this.hotkeyEvaulator = new HotkeyEvaluationContext(this.presenter);
            this.hotkeyEvaulator.IsEnabled = false;
        }

        public override void OnApplyTemplate()
        {
            Binding overrideBinding = new Binding("Hotkey.OverrideIfNecessary");
            overrideBinding.RelativeSource = RelativeSource.Self;
            this.SetBinding(OverrideHotkeyProperty, overrideBinding);

            Binding keyBinding = new Binding("Hotkey.Key");
            keyBinding.RelativeSource = RelativeSource.Self;
            this.SetBinding(HotkeyKeyProperty, keyBinding);

            Binding modiferBinding = new Binding("Hotkey.ModifierKeys");
            modiferBinding.RelativeSource = RelativeSource.Self;
            this.SetBinding(HotkeyModifierKeysProperty, modiferBinding);

            this.hotkeyEvaulator.IsEnabled = true;
            this.hotkeyEvaulator.EvaluateHotkey();
        }

        public HotkeyModifierKeys HotkeyModifierKeys
        {
            get { return (HotkeyModifierKeys)this.GetValue(HotkeyModifierKeysProperty); }
            set { this.SetValue(HotkeyModifierKeysProperty, value); }
        }

        public System.Windows.Forms.Keys HotkeyKey
        {
            get { return (System.Windows.Forms.Keys)this.GetValue(HotkeyKeyProperty); }
            set { this.SetValue(HotkeyKeyProperty, value); }
        }

        public GlobalHotkey Hotkey
        {
            get { return (GlobalHotkey)this.GetValue(HotkeyProperty); }
            set { this.SetValue(HotkeyProperty, value); }
        }

        private static void HandleHotkeyModifierKeysChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HotkeyBindingWrapper wrapper = obj as HotkeyBindingWrapper;
            if (wrapper != null)
            {
                wrapper.presenter.HotkeyModifierKeys = (HotkeyModifierKeys)e.NewValue;
            }
        }

        private static void HandleHotkeyKeyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HotkeyBindingWrapper wrapper = obj as HotkeyBindingWrapper;
            if (wrapper != null)
            {
                wrapper.presenter.UnderlyingHotkeyKey = (System.Windows.Forms.Keys)e.NewValue;
            }
        }

        private static void HandleOverrideHotkeyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HotkeyBindingWrapper wrapper = obj as HotkeyBindingWrapper;
            if (wrapper != null)
            {
                wrapper.presenter.OverideHotkey = (bool)e.NewValue;
            }
        }

        private static void HandleHotkeyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HotkeyBindingWrapper wrapper = obj as HotkeyBindingWrapper;
            if (wrapper != null)
            {
                wrapper.hotkeyEvaulator.EvaluationHotkey = (GlobalHotkey)e.NewValue;
            }
        }
    }
}
