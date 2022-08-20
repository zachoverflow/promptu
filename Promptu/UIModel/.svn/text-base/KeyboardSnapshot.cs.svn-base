using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class KeyboardSnapshot
    {
        public KeyboardSnapshot()
        {
        }

        public bool WinKeyPressed
        {
            get { return AnyKeysPressed(Keys.RWin, Keys.LWin); }
        }

        public bool ShiftKeyPressed
        {
            get { return AnyKeysPressed(Keys.ShiftKey, Keys.LShiftKey, Keys.RShiftKey); }
        }

        public bool CtrlKeyPressed
        {
            get { return AnyKeysPressed(Keys.ControlKey, Keys.LControlKey, Keys.RControlKey); }
        }

        public bool AltKeyPressed
        {
            get { return AnyKeysPressed(Keys.LMenu, Keys.RMenu); }
        }

        public bool AnyKeysPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (KeyPressed(key))
                {
                    return true;
                }
            }

            return false;
        }

        public bool KeyPressed(Keys key)
        {
            return KeyPressedCore(key);
        }

        protected abstract bool KeyPressedCore(Keys key);
    }
}
