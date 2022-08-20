//-----------------------------------------------------------------------
// <copyright file="WindowsKeyboardSnapshot.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using ZachJohnson.Promptu.UIModel;

    internal class WindowsKeyboardSnapshot : KeyboardSnapshot
    {
        private byte[] keyStates;

        public WindowsKeyboardSnapshot()
        {
            this.keyStates = new byte[255];
            NativeMethods.GetKeyboardState(this.keyStates);
        }

        protected override bool KeyPressedCore(Keys key)
        {
            int index = (int)key;
            if (index >= 0 && index < this.keyStates.Length)
            {
                byte keyState = this.keyStates[index];
                switch (keyState >> 7)
                {
                    case 1:
                        return true;
                    default:
                        break;
                }
            }

            return false;
        }
    }
}
