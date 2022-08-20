using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using ZachJohnson.Promptu.UserModel;
using System.Security.Principal;
using System.Windows.Forms;
using System.Globalization.Exensions;
using ZachJohnson.Promptu.UIModel;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal static class Utilities
    {
        internal static double? TryParseDouble(string value, double? defaultValue)
        {
            try
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToDouble(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        internal static DateTime? TryParseBinaryDateTime(string value, DateTime? defaultValue)
        {
            try
            {
                return DateTime.FromBinary(Convert.ToInt64(value, CultureInfo.InvariantCulture));
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return DateTime.FromBinary(Convert.ToInt64(value, CultureInfo.CurrentCulture));
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        internal static bool? TryParseBoolean(string value, bool? defaultValue)
        {
            try
            {
                return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }

            try
            {
                return Convert.ToBoolean(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }

            return defaultValue;
        }

        internal static bool TryParseBoolean(string value, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }

            try
            {
                return Convert.ToBoolean(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }

            return defaultValue;
        }

        internal static int? TryParseInt32(string value, int? defaultValue)
        {
            try
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToInt32(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        internal static int TryParseInt32(string value, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToInt32(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        internal static long? TryParseInt64(string value, long? defaultValue)
        {
            try
            {
                return Convert.ToInt64(value, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            try
            {
                return Convert.ToInt64(value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return defaultValue;
        }

        // I18N
        public static string ConvertHotkeyToString(HotkeyModifierKeys modifierKeys, Keys key)
        {
            StringBuilder hotkeyInfo = new StringBuilder();

            if ((modifierKeys & HotkeyModifierKeys.Ctrl) != 0)
            {
                hotkeyInfo.Append("Ctrl");
            }

            if ((modifierKeys & HotkeyModifierKeys.Alt) != 0)
            {
                if (hotkeyInfo.Length > 0)
                {
                    hotkeyInfo.Append("+");
                }

                hotkeyInfo.Append("Alt");
            }

            if ((modifierKeys & HotkeyModifierKeys.Shift) != 0)
            {
                if (hotkeyInfo.Length > 0)
                {
                    hotkeyInfo.Append("+");
                }

                hotkeyInfo.Append("Shift");
            }

            if ((modifierKeys & HotkeyModifierKeys.Win) != 0)
            {
                if (hotkeyInfo.Length > 0)
                {
                    hotkeyInfo.Append("+");
                }

                hotkeyInfo.Append("Win");
            }

            return String.Format(CultureInfo.CurrentCulture, "{0}+{1}", hotkeyInfo.ToString(), ValidHotkeyKeys.GetStringRepresentation(key));
        }

        public static bool PromptuIsRunningElevated
        {
            get
            {
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(currentIdentity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static bool IsNamespace(string value)
        {
            int indexOfNextDot = value.IndexOf('.');
            if (indexOfNextDot != -1)
            {
                return true;
            }
            else
            {
                return InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.TryFind(value + '.') != null;
            }
        }

        public static bool IsNamespaceAndNotCommandOrFunction(string value)
        {
            if (InternalGlobals.CurrentProfile.CompositeFunctionsAndCommands.Contains(value))
            {
                return false;
            }

            return IsNamespace(value);
        }

        //public static bool IsValidPath(string value)
        //{
        //    string[] pathSplit;
        //    return IsValidPath(value, out pathSplit);
        //}

        public static bool LooksLikeValidPath(string value)
        {
            string[] pathSplit;
            return LooksLikeValidPath(value, out pathSplit);
        }

        public static bool LooksLikeValidPath(string value, out string[] pathSplit)
        {
            bool isPath = false;
            pathSplit = value.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.None);
            if (pathSplit.Length > 0)
            {
                string firstSegment = pathSplit[0];
                isPath = value.IndexOf(Path.DirectorySeparatorChar) > -1;
                if (firstSegment.Contains(" ") || Function.IsInFunctionSyntax(firstSegment))
                {
                    isPath = false;
                }
                else
                {
                    //isPath = true;
                }
            }

            return isPath;
        }

        //public static bool IsValidPath(string value, out string[] pathSplit)
        //{
        //    bool isPath = false;
        //    pathSplit = value.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.None);
        //    if (pathSplit.Length > 0)
        //    {
        //        string firstSegment = pathSplit[0];
        //        isPath = value.IndexOf(Path.DirectorySeparatorChar) > -1;
        //        if (firstSegment.Contains(" ") || Function.IsInFunctionSyntax(firstSegment))
        //        {
        //            isPath = false;
        //        }
        //        else
        //        {
        //            bool found;
        //            GroupedCompositeItem compositeItem = PromptuSettings.CurrentProfile.CompositeFunctionsAndCommands.TryGetItem(firstSegment, out found);
        //            CompositeItem<Command, List> parameterlessCommandNamedLikeFirstFolder;
        //            if (found && compositeItem != null && (parameterlessCommandNamedLikeFirstFolder = compositeItem.TryGetCommand(firstSegment, 0)) != null)
        //            {
        //                try
        //                {
        //                    ExecutionData executionData = new ExecutionData(
        //                        new string[0],
        //                        parameterlessCommandNamedLikeFirstFolder.ListFrom,
        //                        PromptuSettings.CurrentProfile.Lists);
        //                    FileSystemDirectory proposedDirectory = parameterlessCommandNamedLikeFirstFolder.Item.GetSubstitutedExecutionPath(executionData);
        //                    if (proposedDirectory.Exists)
        //                    {
        //                        isPath = true;
        //                    }
        //                }
        //                catch (Itl.ParseException)
        //                {
        //                }
        //                catch (ConversionException)
        //                {
        //                }
        //                catch (SelfReferencingCommandException)
        //                {
        //                }
        //            }
        //        }
        //    }

        //    return isPath;
        //}

        public static void AddPathInformationAndThrow(Exception ex, string path)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            ex.Data.Add(InternalGlobals.ExceptionPathToken, path);
            throw ex;
        }

        public static UIMessageBoxResult ShowPromptuErrorMessageBox(Exception ex)
        {
            string message;
            object path;
            if (ex.Data.Contains(InternalGlobals.ExceptionPathToken) && (path = ex.Data[InternalGlobals.ExceptionPathToken]) != null)
            {
                message = String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.CorruptedFile, path, ex.Message);
            }
            else
            {
                message = ex.Message;
            }

            return ShowPromptuErrorMessageBox(message);
        }

        public static UIMessageBoxResult ShowPromptuErrorMessageBox(string text)
        {
            return ShowPromptuMessageBox(text, UIMessageBoxButtons.OK, UIMessageBoxResult.OK, UIMessageBoxIcon.Error);
        }

        public static UIMessageBoxResult ShowPromptuMessageBox(string text, UIMessageBoxButtons buttons, UIMessageBoxResult defaultResult, UIMessageBoxIcon icon)
        {
            return UIMessageBox.Show(text,
                Localization.Promptu.AppName,
                buttons,
                icon,
                defaultResult);
        }

        //public static string GenerateXmlTag(string text, bool bold)
        //{
        //    return GenerateXmlTag(text, bold, false);
        //}

        //public static string GenerateXmlImageTag(int index, string id, Padding margin)
        //{
        //    return string.Format("<img index=\"{0}\" id=\"{1}\" margin=\"{2}\" />", index, EscapeXml(id), new PaddingConverter().ConvertToString(margin));
        //}

        //public static string GenerateXmlTag(string text)
        //{
        //    return GenerateXmlTag(text, false);
        //}

        //public static string GenerateXmlTag(string text, bool bold, bool flow)
        //{
        //    if (bold)
        //    {
        //        return string.Format("<b v=\"{0}\"{1}/>", EscapeXml(text), flow ? " flow=\"true\"" : String.Empty);
        //    }
        //    else
        //    {
        //        return string.Format("<t v=\"{0}\"{1}/>", EscapeXml(text), flow ? " flow=\"true\"" : String.Empty);
        //    }
        //}

        //private static string EscapeXml(string text)
        //{
        //    return text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
        //}
    }
}
