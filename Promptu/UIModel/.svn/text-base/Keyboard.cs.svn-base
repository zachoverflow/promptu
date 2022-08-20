using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class Keyboard
    {
        public bool WinKeyPressed
        {
            get { return AnyKeysArePressed(Keys.RWin, Keys.LWin); }
        }

        public bool ShiftKeyPressed
        {
            get { return AnyKeysArePressed(Keys.Shift, Keys.ShiftKey, Keys.LShiftKey, Keys.RShiftKey); }
        }

        public bool CtrlKeyPressed
        {
            get { return AnyKeysArePressed(Keys.Control, Keys.ControlKey, Keys.LControlKey, Keys.RControlKey); }
        }

        public bool AltKeyPressed
        {
            get { return AnyKeysArePressed(Keys.Alt, Keys.LMenu, Keys.RMenu); }
        }

        private bool AnyKeysArePressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (KeyIsPressed(key))
                {
                    return true;
                }
            }

            return false;
        }

        public char ConvertToChar(Keys key)
        {
            return this.ConvertToCharCore(key);
        }

        public bool KeyIsPressed(Keys key)
        {
            return this.KeyIsPressedCore(key);
        }

        protected abstract bool KeyIsPressedCore(Keys key);

        protected abstract char ConvertToCharCore(Keys key);
    }
}
