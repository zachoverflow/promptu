// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
