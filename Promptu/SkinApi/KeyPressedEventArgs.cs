//-----------------------------------------------------------------------
// <copyright file="KeyPressedEventArgs.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;
    using System.Windows.Forms;

    // TODO evaluate
    public class KeyPressedEventArgs : EventArgs
    {
        private Keys keyCode;
        private bool cancel;
        private bool winKeyPressed;

        public KeyPressedEventArgs(Keys keyCode, bool winKeyPressed)
        {
            this.keyCode = keyCode;
            this.winKeyPressed = winKeyPressed;
        }

        public bool Cancel
        {
            get { return this.cancel; }
            set { this.cancel = value; }
        }

        public bool ShiftKeyPressed
        {
            get { return (this.keyCode & Keys.Shift) == Keys.Shift; }
        }

        public bool AltKeyPressed
        {
            get { return (this.keyCode & Keys.Alt) == Keys.Alt; }
        }

        public bool CtrlKeyPressed
        {
            get { return (this.keyCode & Keys.Control) == Keys.Control; }
        }

        public bool WinKeyPressed
        {
            get { return this.WinKeyPressed; }
        }

        public Keys KeyCode
        {
            get { return this.keyCode; }
        }
    }
}
