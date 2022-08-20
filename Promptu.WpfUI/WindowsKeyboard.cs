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

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using ZachJohnson.Promptu.UIModel;

    internal class WindowsKeyboard : Keyboard
    {
        protected override bool KeyIsPressedCore(Keys key)
        {
            short keyState = NativeMethods.GetKeyState((int)key);
            switch (keyState)
            {
                case 0:
                case 1:
                    return false;
                default:
                    return true;
            }
        }

        protected override char ConvertToCharCore(Keys key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            uint keyValue = (uint)key;
            byte[] keyState = new byte[255];
            NativeMethods.GetKeyboardState(keyState);
            uint scanCode = NativeMethods.MapVirtualKey(keyValue, 0);
            IntPtr hkl = NativeMethods.GetKeyboardLayout(0);
            int returnCode = NativeMethods.ToUnicodeEx(keyValue, scanCode, keyState, stringBuilder, 5, 0, hkl);

            if (returnCode == 0)
            {
                throw new ArgumentException("The specified key has no translation for the current state of the keyboard.");
            }
            else if (returnCode == -1)
            {
                throw new ArgumentException("The specified key is a dead-key character (accent or diacritic).");
            }
            else
            {
                return stringBuilder[0];
            }
        }
    }
}
