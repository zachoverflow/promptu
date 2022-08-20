//-----------------------------------------------------------------------
// <copyright file="ErrorConsole.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    internal static class ErrorConsole
    {
        public static void WriteLineFormat(string category, string format, object arg0)
        {
            WriteLine(category, String.Format(CultureInfo.InvariantCulture, format, arg0));
        }

        public static void WriteLineFormat(string category, string format, object arg0, object arg1)
        {
            WriteLine(category, String.Format(CultureInfo.InvariantCulture, format, arg0, arg1));
        }

        public static void WriteLineFormat(string category, string format, object arg0, object arg1, object arg2)
        {
            WriteLine(category, String.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2));
        }

        public static void WriteLineFormat(string category, string format, params object[] args)
        {
            WriteLine(category, String.Format(CultureInfo.InvariantCulture, format, args));
        }

        public static void WriteLine(string category, string message)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "[{0}] {1}", category, message));
        }
    }
}
