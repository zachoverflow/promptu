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
    using ZachJohnson.Promptu.UIModel;
    using System.Windows.Forms;
    using System.Diagnostics;

    internal class WindowsComputer : Computer
    {
        const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        const short SE_PRIVILEGE_ENABLED = 2;
        const uint EWX_SHUTDOWN = 1;
        const short TOKEN_ADJUST_PRIVILEGES = 32;
        const short TOKEN_QUERY = 8;

        protected override void StandbyCore()
        {
            Application.SetSuspendState(PowerState.Suspend, true, false);
        }

        protected override void HibernateCore()
        {
            Application.SetSuspendState(PowerState.Hibernate, true, false);
        }

        protected override void LockCore()
        {
            NativeMethods.LockWorkStation();
        }

        protected override void LogOffCore()
        {
            EnsureSE_SHUTDOWN_NAME();
            NativeMethods.ExitWindowsEx(0, 0);
        }

        protected override void ShutDownCore()
        {
            EnsureSE_SHUTDOWN_NAME();
            NativeMethods.ExitWindowsEx(1, 0);
        }

        protected override void RebootCore()
        {
            EnsureSE_SHUTDOWN_NAME();
            NativeMethods.ExitWindowsEx(2, 0);
        }

        protected override void StartScreensaverCore()
        {
            NativeMethods.SendMessage(
                NativeMethods.GetDesktopWindow(), (
                int)WindowsMessages.WM_SYSCOMMAND,
                (IntPtr)WindowsMessages.SC_SCREENSAVE,
                (IntPtr)0);
        }

        private static void EnsureSE_SHUTDOWN_NAME()
        {
            IntPtr hToken;
            NativeMethods.TOKEN_PRIVILEGES tokenPrivileges;

            NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
                  TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken);
            tokenPrivileges.PrivilegeCount = 1;
            tokenPrivileges.Privileges.Attributes = SE_PRIVILEGE_ENABLED;
            NativeMethods.LookupPrivilegeValue("", SE_SHUTDOWN_NAME, out tokenPrivileges.Privileges.pLuid);
            NativeMethods.AdjustTokenPrivileges(hToken, false, ref tokenPrivileges, 0U, IntPtr.Zero,
            IntPtr.Zero);
        }
    }
}
