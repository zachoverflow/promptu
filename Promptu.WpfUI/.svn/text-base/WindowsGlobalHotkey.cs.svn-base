//-----------------------------------------------------------------------
// <copyright file="WindowsGlobalHotkey.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using ZachJohnson.Promptu.UIModel.Interfaces;
    using ZachJohnson.Promptu.UserModel;

    internal class WindowsGlobalHotkey : IGlobalHotkey
    {
        private HotkeyModifierKeys modifierKeys;
        private Keys key;
        private HotKeyReciever receiver;
        private bool registered;
        private WindowsHotkeyHook hotkeyHook;
        private bool overrideIfNecessary;
        private bool enabled = true;

        public WindowsGlobalHotkey(HotkeyModifierKeys modifierKeys, Keys key, bool overrideIfNecessary, bool enabled)
        {
            this.receiver = new HotKeyReciever(this);
            this.modifierKeys = modifierKeys;
            this.key = key;
            this.hotkeyHook = new WindowsHotkeyHook(key, modifierKeys);
            this.hotkeyHook.Pressed += this.ForwardHookHotkeyPressed;
            this.overrideIfNecessary = overrideIfNecessary;
            this.enabled = enabled;
        }

        ~WindowsGlobalHotkey()
        {
            this.Dispose(false);
        }

        public event EventHandler Pressed;

        public event EventHandler Changed;

        public event EventHandler RegistrationChanged;

        public bool Enabled
        {
            get 
            {
                return this.enabled; 
            }

            set 
            { 
                this.enabled = value;
                if (!value)
                {
                    this.Unregister();
                }
            }
        }

        public HotkeyModifierKeys ModifierKeys
        {
            get { return this.modifierKeys; }
            set { this.SwitchHotkey(value, this.Key, this.overrideIfNecessary); }
        }

        public bool Registered
        {
            get { return this.registered; }
        }

        public bool CanOverrideIfNecessary
        {
            get { return true; }
        }

        public bool OverrideIfNecessary
        {
            get { return this.overrideIfNecessary; }
            set { this.SwitchHotkey(this.ModifierKeys, this.Key, value); }
        }

        public Keys Key
        {
            get { return this.key; }
            set { this.SwitchHotkey(this.ModifierKeys, value, this.overrideIfNecessary); }
        }

        public bool IsTheSameAs(WindowsGlobalHotkey hotkey)
        {
            return hotkey.Key == this.Key && hotkey.ModifierKeys == this.ModifierKeys;
        }

        public void Register()
        {
            if (this.enabled && this.receiver.Handle != IntPtr.Zero && !this.registered)
            {
                if (this.key == Keys.R && this.modifierKeys == HotkeyModifierKeys.Win)
                {
                    this.hotkeyHook.Start();
                }
                else
                {
                    if (!NativeMethods.RegisterHotKey(this.receiver.Handle, 1, (uint)this.modifierKeys, (int)key))
                    {
                        if (this.overrideIfNecessary)
                        {
                            this.hotkeyHook.Start();
                        }
                        else
                        {
                            throw new HotkeyException("Hotkey already registered.");
                        }
                    }
                }

                this.registered = true;
                this.OnRegistrationChanged(EventArgs.Empty);
            }
        }

        public void Unregister()
        {
            if (this.registered)
            {
                this.hotkeyHook.Stop();
                NativeMethods.UnregisterHotKey(this.receiver.Handle, 1);
                this.registered = false;
                this.OnRegistrationChanged(EventArgs.Empty);
            }
        }

        public void SwitchHotkey(HotkeyModifierKeys newModifierKeys, Keys newKey, bool overrideIfNecessary)
        {
            this.Unregister();
            this.Enabled = enabled;
            this.modifierKeys = newModifierKeys;
            this.key = newKey;
            this.overrideIfNecessary = overrideIfNecessary;
            this.hotkeyHook.Key = newKey;
            this.hotkeyHook.ModifierKeys = newModifierKeys;
            this.OnChanged(EventArgs.Empty);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ForwardHookHotkeyPressed(object sender, EventArgs e)
        {
            this.OnPressed(EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.Unregister();
            this.hotkeyHook.Dispose();
        }

        protected virtual void OnPressed(EventArgs e)
        {
            EventHandler handler = this.Pressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnChanged(EventArgs e)
        {
            EventHandler handler = this.Changed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRegistrationChanged(EventArgs e)
        {
            EventHandler handler = this.RegistrationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private class HotKeyReciever : NativeWindow
        {
            private WeakReference<WindowsGlobalHotkey> hotKey;
            public HotKeyReciever(WindowsGlobalHotkey hotKey)
            {
                CreateParams createParams = new CreateParams();
                this.CreateHandle(createParams);
                this.hotKey = new WeakReference<WindowsGlobalHotkey>(hotKey);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case (int)WindowsMessages.WM_HOTKEY:
                        WindowsGlobalHotkey hotkeyReference = this.hotKey.Target;
                        if (hotkeyReference != null)
                        {
                            NativeMethods.CloseStartMenuIfOpen();
                            hotkeyReference.OnPressed(EventArgs.Empty);
                        }

                        break;
                    default:
                        break;
                }

                base.WndProc(ref m);
            }
        }
    }
}
