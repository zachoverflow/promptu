//-----------------------------------------------------------------------
// <copyright file="WaitHandleExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Threading.Extensions
{
    using System;
    using ZachJohnson.Promptu;
    using ZachJohnson.Promptu.UIModel;

    internal static class WaitHandleExtensions
    {
        public static void WaitOneCallDeadlock(this WaitHandle handle, int timeout)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle");
            }

            if (!handle.WaitOne(timeout, false))
            {
                ExceptionLogger.LogCurrentThreadStack("Promptu locked up");

                UIMessageBox.Show(
                    "Deadlock detected.  Please contact support@PromptuLauncher.com to help us resolve this issue.",
                    ZachJohnson.Promptu.Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
        }
    }
}
