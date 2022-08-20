using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ZachJohnson.Promptu
{
    internal static class PromptuUtilities
    {
        public static void ExitApplication()
        {
            InternalGlobals.CurrentProfile.SkinsSettings.TrySerialize(InternalGlobals.CurrentSkinInstance, InternalGlobals.CurrentSkin.Id);
            InternalGlobals.TryResaveAndAlertUser();
            InternalGlobals.GuiManager.ToolkitHost.ExitApplication();
        }

        public static string SanitizeContactLink(string link)
        {
            if (link.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase) 
                || link.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                return link;
            }
            else
            {
                return "http://" + link;
            }
        }
    }
}
