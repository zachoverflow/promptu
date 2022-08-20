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
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Drawing;
    using System.Windows.Interop;
    using System.Windows.Media;

    public static class NativeMethods
    {
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        internal const int SHGFI_USEFILEATTRIBUTES = 0x000000010;
        internal const int SHGFI_ICON = 0x000000100;
        internal const int SHGFI_LARGEICON = 0x000000000;
        internal const int SHGFI_SMALLICON = 0x000000001;
        internal const int SHGFI_OPENICON = 0x000000002;
        internal const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        internal const int FILE_ATTRIBUTE_NORMAL = 0x00000080;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers,
                                          int vk);
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        internal static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        internal static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        internal static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        internal static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[]
           lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
           int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);

        [DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(Point lpPoint);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            Int32 msg,
            IntPtr wParam,
            IntPtr lParam,
            uint fuFlags,
            uint timeout,
            out IntPtr res);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            Int32 msg,
            IntPtr wParam,
            StringBuilder lParam,
            uint fuFlags,
            uint timeout,
            out IntPtr res);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("shell32.dll")]
        internal static extern void SHGetSettings(out long lpsfs, uint dwMask);

        [DllImport("shell32.dll")]
        internal static extern void SHGetSetSettings(ref SHELLSTATE lpss, uint dwMask, bool bSet);

        [Flags]
        internal enum SHELLFLAGS : uint
        {
            fShowAllObjects = 0x00000001,
            fShowExtensions = 0x00000002,
            fNoConfirmRecycle = 0x00000004,
            fShowSysFiles = 0x00000008,
            fShowCompColor = 0x00000010,
            fDoubleClickInWebView = 0x00000020,
            fDesktopHTML = 0x00000040,
            fWin95Classic = 0x00000080,
            fDontPrettyPath = 0x00000100,
            fShowAttribCol = 0x00000200,
            fMapNetDrvBtn = 0x00000400,
            fShowInfoTip = 0x00000800,
            fHideIcons = 0x00001000,
        }

#pragma warning disable 0649

        [StructLayoutAttribute(LayoutKind.Sequential)]
        internal struct SHELLSTATE
        {
            public uint bitVector1;
            public uint dwWin95Unused;
            public uint uWin95Unused;
            public int lParamSort;
            public int iSortDirection;
            public uint version;
            public uint uNotUsed;
            public uint bitVector2;
        }

        //        typedef struct {
        //  BOOL fShowAllObjects  :1;
        //  BOOL fShowExtensions  :1;
        //  BOOL fNoConfirmRecycle  :1;
        //  BOOL fShowSysFiles  :1;
        //  BOOL fShowCompColor  :1;
        //  BOOL fDoubleClickInWebView  :1;
        //  BOOL fDesktopHTML  :1;
        //  BOOL fWin95Classic  :1;
        //  BOOL fDontPrettyPath  :1;
        //  BOOL fShowAttribCol  :1;
        //  BOOL fMapNetDrvBtn  :1;
        //  BOOL fShowInfoTip  :1;
        //  BOOL fHideIcons  :1;
        //  BOOL fAutoCheckSelect  :1;
        //  BOOL fIconsOnly  :1;
        //  UINT fRestFlags  :3;
        //} SHELLFLAGSTATE, *LPSHELLFLAGSTATE;

        //internal struct SHELLFLAGSTATE
        //{
        //    internal bool fShowAllObjects;
        //    internal bool fShowExtensions;
        //    internal bool fNoConfirmRecycle;
        //    internal bool fShowSysFiles;
        //    internal bool fShowCompColor;
        //    internal bool fDoubleClickInWebView;
        //    internal bool fDesktopHTML;
        //    internal bool fWin95Classic;
        //    internal bool fDontPrettyPath;
        //    internal bool fShowAttribCol;
        //    internal bool fMapNetDrvBtn;
        //    internal bool fShowInfoTip;
        //    internal bool fHideIcons;
        //    internal bool fAutoCheckSelect;
        //    internal bool fIconsOnly;
        //    internal uint fRestFlags;
        //}

        public struct POINT
        {
            public int x;
            public int y;
        }

        internal struct MinMaxInfo
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        internal struct WindowPos
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int width;
            public int height;
            public uint flags;
        }

#pragma warning restore 0649

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        internal static extern void GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr processId);

        internal delegate bool EnumWindowsCallback(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        internal static extern void EnumChildWindows(IntPtr hWnd, EnumWindowsCallback lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

        internal static void SetOwner(IWin32Window child, IWin32Window owner)
        {
            NativeMethods.SetWindowLong(new HandleRef(child, child.Handle), -8, new HandleRef(owner, owner.Handle));
        }

        [DllImport("User32.dll")]
        internal static extern IntPtr GetDC(HandleRef hWnd);

        [DllImport("User32.dll")]
        internal static extern int ReleaseDC(HandleRef hWnd, HandleRef hDC);

        [DllImport("GDI32.dll")]
        internal static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool LockWorkStation();

        [DllImport("user32.dll")]
        internal static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        internal struct LUID
        {
            public int LowPart;
            public int HighPart;
        }

        internal struct LUID_AND_ATTRIBUTES
        {
            public LUID pLuid;
            public int Attributes;
        }

        internal struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }

        [DllImport("advapi32.dll")]
        internal static extern int OpenProcessToken(IntPtr ProcessHandle,
                             int DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            UInt32 BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength);

        [DllImport("advapi32.dll")]
        internal static extern int LookupPrivilegeValue(string lpSystemName,
                               string lpName, out LUID lpLuid);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, bool wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHAppBarMessage(uint dwMessage, [In] ref APPBARDATA pData);

        [StructLayout(LayoutKind.Sequential)]
        internal struct APPBARDATA
        {
            public int cbSize; // initialize this field using: Marshal.SizeOf(typeof(APPBARDATA));
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public ABE uEdge;
            public RECT rc;
            public int lParam;

            public static APPBARDATA NewAPPBARDATA()
            {
                APPBARDATA abd = new APPBARDATA();
                abd.cbSize = Marshal.SizeOf(typeof(APPBARDATA));
                return abd;
            }
        }

        internal enum ABE : uint
        {
            Left = 0,
            Top = 1,
            Right = 2,
            Bottom = 3
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        internal enum ABM : uint
        {
            New = 0x00000000,
            Remove = 0x00000001,
            QueryPos = 0x00000002,
            SetPos = 0x00000003,
            GetState = 0x00000004,
            GetTaskbarPos = 0x00000005,
            Activate = 0x00000006,
            GetAutoHideBar = 0x00000007,
            SetAutoHideBar = 0x00000008,
            WindowPosChanged = 0x00000009,
            SetState = 0x0000000A,
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern internal IntPtr LoadIcon(IntPtr hInstance, string lpIconName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern internal IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern int GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        internal static int GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetWindowLongPtr64(hWnd, nIndex);
            }
            else
            {
                return GetWindowLongPtr32(hWnd, nIndex);
            }
        }

        internal static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr64(hWnd, nIndex, (IntPtr)dwNewLong);
            }
            else
            {
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong));
            }
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [StructLayout(LayoutKind.Sequential)]
        internal struct ICONINFO
        {
            public bool IsIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr MaskBitmap;
            public IntPtr ColorBitmap;
        };

        [DllImport("user32.dll")]
        internal static extern IntPtr CreateIconIndirect([In] ref ICONINFO iconInfo);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes,
           uint cBitsPerPel, IntPtr lpvBits);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("DwmApi.dll")]
        internal static extern IntPtr DwmExtendFrameIntoClientArea(
            IntPtr hWnd,
            ref Margins pMarInset);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Margins
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        //[StructLayout(LayoutKind.Sequential)]
        //public struct RECT
        //{
        //    public int Left;
        //    public int Top;
        //    public int Right;
        //    public int Bottom;

        //    public RECT(int left, int top, int right, int bottom)
        //    {
        //        this.Left = left;
        //        this.Top = top;
        //        this.Right = right;
        //        this.Bottom = bottom;
        //    }
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct POINT
        //{
        //    public int X;
        //    public int Y;

        //    public POINT(int x, int y)
        //    {
        //        this.X = x;
        //        this.Y = y;
        //    }
        //}

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT minPosition;
            public POINT maxPosition;
            public RECT normalPosition;
        }

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        internal const int SW_SHOWNORMAL = 1;
        internal const int SW_SHOWMINIMIZED = 2;

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref POINT pt);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        static extern void keybd_event(System.Windows.Forms.Keys bVk, byte bScan, uint dwFlags,
           UIntPtr dwExtraInfo);

        public static void CloseStartMenuIfOpen()
        {
            IntPtr startmenu = FindWindowByCaption(IntPtr.Zero, "Start menu");

            if (startmenu != IntPtr.Zero)
            {
                if (IsWindowVisible(startmenu))
                {
                    uint winKeyScanCode = MapVirtualKey((int)System.Windows.Forms.Keys.LWin, 0);
                    byte scanCode = (byte)(winKeyScanCode & 0xff);
                    uint baseFlags = (uint)(((winKeyScanCode << 8) & 0xff) != 0 ? KEYEVENTF_EXTENDEDKEY : 0);

                    keybd_event(System.Windows.Forms.Keys.LWin, scanCode, baseFlags, (UIntPtr)0);
                    keybd_event(System.Windows.Forms.Keys.LWin, scanCode, baseFlags | KEYEVENTF_KEYUP, (UIntPtr)0);
                }
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct StockIconInfo
        {
            internal UInt32 StuctureSize;
            internal IntPtr Handle;
            internal Int32 ImageIndex;
            internal Int32 Identifier;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string Path;
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        internal static extern int SHGetStockIconInfo(StockIconIdentifier identifier, StockIconOptions flags, ref StockIconInfo info);

        internal static ImageSource MakeImage(StockIconIdentifier identifier, StockIconOptions flags)
        {
            IntPtr iconHandle = GetIcon(identifier, flags);
            ImageSource imageSource;

            try
            {
                imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(iconHandle, System.Windows.Int32Rect.Empty, null);
            }
            finally
            {
                DestroyIcon(iconHandle);
            }

            return imageSource;
        }

        internal static IntPtr GetIcon(StockIconIdentifier identifier, StockIconOptions flags)
        {
            StockIconInfo info = new StockIconInfo();
            info.StuctureSize = (UInt32)Marshal.SizeOf(typeof(StockIconInfo));
            int hResult = SHGetStockIconInfo(identifier, flags, ref info);

            if (hResult < 0)
            {
                throw new COMException("SHGetStockIconInfo execution failure", hResult);
            }

            return info.Handle;
        }
    }
}