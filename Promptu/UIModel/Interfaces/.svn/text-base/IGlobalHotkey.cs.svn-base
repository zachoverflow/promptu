using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IGlobalHotkey : IDisposable
    {
        event EventHandler Pressed;

        event EventHandler Changed;

        event EventHandler RegistrationChanged;

        bool Enabled { get; set; }

        bool CanOverrideIfNecessary { get; }

        bool OverrideIfNecessary { get; set; }

        bool Registered { get; }

        HotkeyModifierKeys ModifierKeys { get; set; }

        Keys Key { get; set; }

        void Register();

        void Unregister();
    }
}
