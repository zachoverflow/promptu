using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu
{
    class HotkeyChangedEventArgs : EventArgs
    {
        HotkeyModifierKeys newModifierKey;
        Keys newKey;
        bool failed;
        bool forceOverride;

        public HotkeyChangedEventArgs(HotkeyModifierKeys newModifierKey, Keys newKey, bool forceOverride)
        {
            this.newKey = newKey;
            this.newModifierKey = newModifierKey;
            this.forceOverride = forceOverride;
        }

        public HotkeyModifierKeys NewModifierKey
        {
            get { return this.newModifierKey; }
        }

        public Keys NewKey
        {
            get { return this.newKey; }
        }

        public bool ForceOverride
        {
            get { return this.forceOverride; }
        }

        public bool Failed
        {
            get { return this.failed; }
            set { this.failed = value; }
        }

        public void ImpartTo(GlobalHotkey hotkey)
        {
            hotkey.SwitchTo(this.NewModifierKey, this.NewKey, this.ForceOverride);
        }
    }
}
