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
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class WindowsHotkeyHook
    {
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private LowLevelKeyboardProc proc;
        private IntPtr hookID = IntPtr.Zero;
        private bool started;
        private bool keyPressed;
        private Keys key;
        private HotkeyModifierKeys modifierKeys;

        public WindowsHotkeyHook(Keys key, HotkeyModifierKeys modifierKeys)
        {
            this.proc = this.HookCallback;
            this.key = key;
            this.modifierKeys = modifierKeys;
        }

        ~WindowsHotkeyHook()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.Stop();
        }

        public event EventHandler Pressed;

        public void Start()
        {
            if (!started)
            {
                hookID = SetHook(proc);
                started = true;
            }
        }

        public void Stop()
        {
            if (started)
            {
                UnhookWindowsHookEx(hookID);
                started = false;
            }
        }

        private void OnPressed(EventArgs e)
        {
            EventHandler handler = Pressed;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        public Keys Key
        {
            get
            {
                return this.key;
            }

            set
            {
                bool restart = this.started;
                this.Stop();
                this.key = value;
                if (restart)
                {
                    this.Start();
                }
            }
        }

        public HotkeyModifierKeys ModifierKeys
        {
            get
            {
                return this.modifierKeys;
            }

            set
            {
                bool restart = this.started;
                this.Stop();
                this.modifierKeys = value;
                if (restart)
                {
                    this.Start();
                }
            }
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            IntPtr? retValue = null;
            if (nCode == 0)
            {
                int wParamInt = (int)wParam;
                if (this.modifierKeys == HotkeyModifierKeys.Win)
                {
                    switch (wParamInt)
                    {
                        case WM_KEYDOWN:
                        case WM_SYSKEYDOWN:
                        case WM_KEYUP:
                        case WM_SYSKEYUP:
                            int vkCode = Marshal.ReadInt32(lParam);
                            Keys pressedKey = (Keys)vkCode;
                            if ((wParamInt == WM_SYSKEYUP || wParamInt == WM_KEYUP))
                            {
                                if (pressedKey == Keys.RWin || pressedKey == Keys.LWin)
                                {
                                    if (keyPressed)
                                    {
                                        uint ctrlKeyScanCode = NativeMethods.MapVirtualKey((int)Keys.LControlKey, 0);
                                        byte scanCode = (byte)(ctrlKeyScanCode & 0xff);
                                        uint baseFlags = (uint)(((ctrlKeyScanCode << 8) & 0xff) != 0 ? KEYEVENTF_EXTENDEDKEY : 0);

                                        keybd_event(Keys.LControlKey, scanCode, baseFlags, (UIntPtr)0);
                                        keybd_event(Keys.LControlKey, scanCode, baseFlags | KEYEVENTF_KEYUP, (UIntPtr)0);
                                    }
                                }
                                else if (pressedKey == this.key)
                                {
                                    if (keyPressed)
                                    {
                                        keyPressed = false;
                                        this.OnPressed(EventArgs.Empty);
                                    }
                                }
                            }
                            else
                            {
                                if (pressedKey == this.key
                                    && (IsPressed(Keys.RWin) || IsPressed(Keys.LWin))
                                    && !IsPressed(Keys.RShiftKey)
                                    && !IsPressed(Keys.LShiftKey)
                                    && !IsPressed(Keys.RControlKey)
                                    && !IsPressed(Keys.LControlKey)
                                    && !IsPressed(Keys.RMenu)
                                    && !IsPressed(Keys.LMenu))
                                {
                                    if (!keyPressed)
                                    {
                                        keyPressed = true;
                                    }

                                    return (IntPtr)1;
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (wParamInt)
                    {
                        case WM_KEYDOWN:
                        case WM_SYSKEYDOWN:
                            int vkCode = Marshal.ReadInt32(lParam);
                            Keys pressedKey = (Keys)vkCode;

                            if (pressedKey == this.key && this.ModifiersAreInCorrectState)
                            {
                                this.OnPressed(EventArgs.Empty);
                                return (IntPtr)1;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            if (retValue == null)
            {
                return CallNextHookEx(hookID, nCode, wParam, lParam);
            }
            else
            {
                return retValue.Value;
            }
        }

        private bool ModifiersAreInCorrectState
        {
            get
            {
                return (((this.modifierKeys & HotkeyModifierKeys.Win) != 0) == (IsPressed(Keys.RWin) || IsPressed(Keys.LWin)))
                    && (((this.modifierKeys & HotkeyModifierKeys.Ctrl) != 0) == (IsPressed(Keys.RControlKey) || IsPressed(Keys.LControlKey)))
                    && (((this.modifierKeys & HotkeyModifierKeys.Shift) != 0) == (IsPressed(Keys.RShiftKey) || IsPressed(Keys.LShiftKey)))
                    && (((this.modifierKeys & HotkeyModifierKeys.Alt) != 0) == (IsPressed(Keys.RMenu) || IsPressed(Keys.LMenu)));
            }
        }

        private static bool IsPressed(Keys key)
        {
            return (GetKeyState(key) & 0x8000) != 0;
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags,
           UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern short GetKeyState(Keys key);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

