//-----------------------------------------------------------------------
// <copyright file="WindowLocator.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.WpfUI
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using ZachJohnson.Promptu.WpfUI.Collections;
    using System.Management;
    using System.Globalization;

    internal static class WindowLocator
    {
        //private const string SE_DEBUG_NAME = "SeDebugPrivilege";
        //private const short SE_PRIVILEGE_ENABLED = 2;
        //private const short TOKEN_ADJUST_PRIVILEGES = 32;
        //private const short TOKEN_QUERY = 8;

        private static WindowDataExtractions windowDataExtractions = WindowDataExtractions.FromFile(((FileSystemDirectory)System.Windows.Forms.Application.StartupPath) + "Config\\WindowDataExtractions.xml");
        private static string lastSuggested = String.Empty;
        private static bool lastExecutablePathNull;
        private static IntPtr lastWindowHandle = IntPtr.Zero;
        //private static bool setSeDebugPrivilege;

        public static string GetPathFromWindowAt(Point point, bool excludeThisProcess, out bool executablePathNull)
        {
            executablePathNull = false;
            IntPtr hWnd = NativeMethods.WindowFromPoint(point);

            if (hWnd != IntPtr.Zero)
            {
                if (hWnd == lastWindowHandle)
                {
                    executablePathNull = lastExecutablePathNull;
                    return lastSuggested;
                }

                lastExecutablePathNull = false;
                lastWindowHandle = hWnd;

                Win32Window window = new Win32Window(hWnd);

                Process process = window.GetProcess();
                if (excludeThisProcess && process.Id == Process.GetCurrentProcess().Id)
                {
                    lastSuggested = null;
                    return lastSuggested;
                }

                string className = window.ClassName;
                string text = window.Text;

                foreach (WindowDataExtraction extraction in windowDataExtractions.GetExtractionsForProcess(process.ProcessName))
                {
                    if (extraction.ClassName == className)
                    {
                        string value = extraction.Extract(text);
                        if (value != null)
                        {
                            lastSuggested = value;
                            return value;
                        }
                    }
                }

                //if (!setSeDebugPrivilege)
                //{
                //    EnsureSeDebugPrivilege();
                //    setSeDebugPrivilege = true;
                //}

                //Process.EnterDebugMode();

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(String.Format(
                    "SELECT ExecutablePath FROM Win32_Process WHERE ProcessID = {0}",
                    process.Id.ToString(CultureInfo.InvariantCulture)));

                ManagementObjectCollection collection = searcher.Get();
                ManagementObject managementObject = null;

                foreach (ManagementObject obj in collection)
                {
                    managementObject = obj;
                    break;
                }

                if (managementObject != null)
                {
                    object executablePath = managementObject["ExecutablePath"];

                    if (executablePath == null)
                    {
                        executablePathNull = true;
                        lastExecutablePathNull = executablePathNull;
                        lastSuggested = null;
                    }
                    else
                    {
                        lastSuggested = executablePath.ToString();
                    }

                    return lastSuggested;
                }

                //if (process.Modules.Count > 0)
                //{
                //    lastSuggested = process.MainModule.FileName;
                //    return lastSuggested;
                //}
            }

            lastExecutablePathNull = false;
            lastSuggested = String.Empty;
            return lastSuggested;
        }

        //private static void EnsureSeDebugPrivilege()
        //{
        //    IntPtr hToken;
        //    NativeMethods.TOKEN_PRIVILEGES tokenPrivileges;

        //    NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
        //          TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken);
        //    tokenPrivileges.PrivilegeCount = 1;
        //    tokenPrivileges.Privileges.Attributes = SE_PRIVILEGE_ENABLED;
        //    NativeMethods.LookupPrivilegeValue("", SE_DEBUG_NAME, out tokenPrivileges.Privileges.pLuid);
        //    NativeMethods.AdjustTokenPrivileges(hToken, false, ref tokenPrivileges, 0U, IntPtr.Zero,
        //    IntPtr.Zero);
        //}
    }
}
