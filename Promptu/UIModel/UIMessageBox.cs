using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal static class UIMessageBox
    {
        public static UIMessageBoxResult Show(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon)
        {
            return Show(text,
                caption,
                buttons,
                icon,
                UIMessageBoxResult.None);
        }

        public static UIMessageBoxResult Show(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon,
            UIMessageBoxResult defaultResult)
        {
            return InternalGlobals.GuiManager.ToolkitHost.ShowMessageBox(text,
                caption,
                buttons,
                icon,
                defaultResult);
        }

        public static UIMessageBoxResult Show(
            string text,
            string caption,
            UIMessageBoxButtons buttons,
            UIMessageBoxIcon icon,
            UIMessageBoxResult defaultResult,
            UIMessageBoxOptions options)
        {
            return InternalGlobals.GuiManager.ToolkitHost.ShowMessageBox(text,
                caption,
                buttons,
                icon,
                defaultResult,
                options);
        }
    }
}
