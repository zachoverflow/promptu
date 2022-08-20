using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Forms;
using System.Xml;

namespace ZachJohnson.Promptu
{
    public class GlobalHotkey : IDisposable
    {
        private IGlobalHotkey nativeInterface;

        public GlobalHotkey(HotkeyModifierKeys modifierKeys, Keys key, bool overrideIfNecessary)
            : this(modifierKeys, key, overrideIfNecessary, true)
        {
        }

        public GlobalHotkey(HotkeyModifierKeys modifierKeys, Keys key, bool overrideIfNecessary, bool enabled)
        {
            this.nativeInterface = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructGlobalHotkey(modifierKeys, key, overrideIfNecessary, enabled);
            this.nativeInterface.Pressed += this.ForwardPressed;
            this.nativeInterface.Changed += this.ForwardChanged;
            this.nativeInterface.RegistrationChanged += this.ForwardRegistrationChanged;
        }

        ~GlobalHotkey()
        {
            this.Dispose(false);
        }

        public event EventHandler Pressed;

        public event EventHandler Changed;

        public event EventHandler RegistrationChanged;

        public bool Enabled
        {
            get { return this.nativeInterface.Enabled; }
            set { this.nativeInterface.Enabled = value; }
        }

        public bool Registered
        {
            get { return this.nativeInterface.Registered; }
        }

        public bool OverrideIfNecessary
        {
            get { return this.nativeInterface.OverrideIfNecessary; }
            set { this.nativeInterface.OverrideIfNecessary = value; }
        }

        public bool CanOverrideIfNecessary
        {
            get { return this.nativeInterface.CanOverrideIfNecessary; }
        }

        public HotkeyModifierKeys ModifierKeys
        {
            get { return this.nativeInterface.ModifierKeys; }
            set { this.nativeInterface.ModifierKeys = value; }
        }

        public Keys Key
        {
            get { return this.nativeInterface.Key; }
            set { this.nativeInterface.Key = value; }
        }

        public static GlobalHotkey FromXml(XmlNode node)
        {
            GlobalHotkey hotkey = new GlobalHotkey(HotkeyModifierKeys.Win, Keys.Q, false);
            hotkey.UpdateFromXml(node);
            return hotkey;
            //return new GlobalHotkey(convertedModifierKeys, convertedKey, overrideIfNecessary, enabled);
        }

        public void UpdateFromXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            string modifierKeys = null;
            string key = null;
            bool overrideIfNecessary = false;
            bool enabled = true;

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.Name.ToUpperInvariant())
                {
                    case "MODIFIERKEYS":
                        modifierKeys = attribute.Value;
                        break;
                    case "KEY":
                        key = attribute.Value;
                        break;
                    case "FORCE":
                    case "OVERRIDE":
                        overrideIfNecessary = Utilities.TryParseBoolean(attribute.Value, overrideIfNecessary);
                        //try
                        //{
                        //    overrideIfNecessary = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    case "ENABLED":
                        enabled = Utilities.TryParseBoolean(attribute.Value, enabled);
                        //try
                        //{
                        //    enabled = Convert.ToBoolean(attribute.Value);
                        //}
                        //catch (FormatException)
                        //{
                        //}

                        break;
                    default:
                        break;
                }
            }

            if (modifierKeys == null)
            {
                throw new LoadException("The node is missing the 'modifierKeys' attribute.");
            }
            else if (key == null)
            {
                throw new LoadException("The node is missing the 'key' attribute.");
            }

            HotkeyModifierKeys convertedModifierKeys;
            Keys convertedKey;

            try
            {
                convertedModifierKeys = (HotkeyModifierKeys)Enum.Parse(typeof(HotkeyModifierKeys), modifierKeys);
            }
            catch (ArgumentException ex)
            {
                throw new LoadException(ex.Message);
            }

            try
            {
                convertedKey = (Keys)Enum.Parse(typeof(Keys), key);
            }
            catch (ArgumentException ex)
            {
                throw new LoadException(ex.Message);
            }

            this.ModifierKeys = convertedModifierKeys;
            this.Key = convertedKey;
            this.OverrideIfNecessary = overrideIfNecessary;
            this.Enabled = enabled;
        }

        public void ToXml(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            node.Attributes.Append(XmlUtilities.CreateAttribute("modifierKeys", this.ModifierKeys, node.OwnerDocument));
            node.Attributes.Append(XmlUtilities.CreateAttribute("key", this.Key, node.OwnerDocument));

            if (this.OverrideIfNecessary)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("override", this.OverrideIfNecessary, node.OwnerDocument));
            }

            if (!this.Enabled)
            {
                node.Attributes.Append(XmlUtilities.CreateAttribute("enabled", this.Enabled, node.OwnerDocument));
            }
        }

        public bool IsTheSameAs(GlobalHotkey hotkey)
        {
            return hotkey.Key == this.Key && hotkey.ModifierKeys == this.ModifierKeys;
        }

        public void Register()
        {
            this.nativeInterface.Register();
        }

        public void Unregister()
        {
            this.nativeInterface.Unregister();
        }

        public void SwitchTo(HotkeyModifierKeys newModifierKeys, Keys newKey, bool overrideIfNecessary)
        {
            this.Unregister();
            this.ModifierKeys = newModifierKeys;
            this.Key = newKey;
            this.OverrideIfNecessary = overrideIfNecessary;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.nativeInterface.Dispose();
        }

        protected virtual void OnPressed(EventArgs e)
        {
            EventHandler handler = this.Pressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ForwardPressed(object sender, EventArgs e)
        {
            this.OnPressed(EventArgs.Empty);
        }

        private void ForwardChanged(object sender, EventArgs e)
        {
            this.OnChanged(EventArgs.Empty);
        }

        private void ForwardRegistrationChanged(object sender, EventArgs e)
        {
            this.OnRegistrationChanged(EventArgs.Empty);
        }

        public override string ToString()
        {
            return Utilities.ConvertHotkeyToString(this.ModifierKeys, this.Key);
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
    }
}
