using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ZachJohnson.Promptu.UserModel
{
    internal static class WindowsROverride
    {
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private static LowLevelKeyboardProc proc = HookCallback;
        private static IntPtr hookID = IntPtr.Zero;
        private static bool started;
        private static bool rPressed;
        private static bool rComeUp = true;
        private static bool winComeUp = true;
        //private static bool winDown;

        //public static void Main()
        //{
        //    _hookID = SetHook(_proc);
        //    Application.Run();
        //    UnhookWindowsHookEx(_hookID);
        //}

        public static event EventHandler Pressed;

        public static void Start()
        {
            if (!started)
            {
                hookID = SetHook(proc);
                started = true;
            }
        }

        public static void Stop()
        {
            if (started)
            {
                UnhookWindowsHookEx(hookID);
                started = false;
            }
        }

        private static void OnPressed(EventArgs e)
        {
            EventHandler handler = Pressed;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        //public static void TEST()
        //{
        //    keybd_event(Keys.RWin, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
        //    keybd_event(Keys.RWin, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
        //}

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            IntPtr? retValue = null;
            if (nCode == 0)
            {
                int wParamInt = (int)wParam;
                switch (wParamInt)
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                    case WM_KEYUP:
                    case WM_SYSKEYUP:
                        
                        int vkCode = Marshal.ReadInt32(lParam);
                        Keys key = (Keys)vkCode;

                        if ((wParamInt == WM_SYSKEYUP || wParamInt == WM_KEYUP))
                        {
                            if (key == Keys.RWin || key == Keys.LWin)
                            {
                                if (rPressed)
                                {
                                    winComeUp = true;
                                    if (rComeUp)
                                    {
                                        rPressed = false;
                                        //keybd_event(Keys.RWin, 0, 0, (UIntPtr)0);
                                        //keybd_event(Keys.RWin, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                                        //PromptuSettings.IgnoreActivationLost = false;
                                        OnPressed(EventArgs.Empty);
                                    }
                                    else
                                    {
                                        //rPressed = false;
                                        retValue = CallNextHookEx(hookID, nCode, wParam, lParam);
                                        //System.Threading.Thread.Sleep(10);
                                        keybd_event(Keys.Escape, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                                        keybd_event(Keys.Escape, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
                                        //OnPressed(EventArgs.Empty);
                                        //rPressed = true;
                                    }
                                }
                            }
                            else if (key == Keys.R)
                            {
                                rComeUp = true;
                                if (rPressed && winComeUp)
                                {
                                    rPressed = false;
                                    //PromptuSettings.IgnoreActivationLost = false;
                                    //keybd_event(Keys.RWin, 0, 0, (UIntPtr)0);
                                    //keybd_event(Keys.RWin, 0, KEYEVENTF_KEYUP, (UIntPtr)0);
                                    OnPressed(EventArgs.Empty);
                                }
                            }
                            //OnPressed(EventArgs.Empty);
                            //if ((key == Keys.RWin || key == Keys.LWin) && winDown)
                            //{
                            //    winDown = false;
                            //    IntPtr result = CallNextHookEx(hookID, nCode, wParam, lParam);

                            //    OnPressed(EventArgs.Empty);
                            //    return result;
                            //}
                            //else if ((key == Keys.R && rDown))
                            //{
                            //    winDown = false;
                            //    IntPtr result = CallNextHookEx(hookID, nCode, wParam, lParam);

                            //    OnPressed(EventArgs.Empty);
                            //    return result;
                            //}
                        }
                        else
                        {
                            if (key == Keys.R 
                                && (IsPressed(Keys.RWin) || IsPressed(Keys.LWin))
                                && !IsPressed(Keys.RShiftKey)
                                && !IsPressed(Keys.LShiftKey)
                                && !IsPressed(Keys.RControlKey)
                                && !IsPressed(Keys.LControlKey)
                                && !IsPressed(Keys.RMenu)
                                && !IsPressed(Keys.LMenu))
                            {
                                if (!rPressed)
                                {
                                    rPressed = true;
                                    //OnPressed(EventArgs.Empty);
                                    //PromptuSettings.IgnoreActivationLost = true;
                                    Skins.PromptHandler.GetInstance().ClosePrompt();
                                }

                                rComeUp = false;
                                winComeUp = false;
                                return (IntPtr)1;
                            }
                        }

                        break;
                    default:
                        break;
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
