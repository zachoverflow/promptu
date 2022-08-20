using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZachJohnson.Promptu.WpfUI
{
    [Flags]
    public enum StockIconOptions : uint
    {
        Small = 0x000000001,       // Retrieve the small version of the icon, as specified by the SM_CXSMICON and SM_CYSMICON system metrics.
        ShellSize = 0x000000004,   // Retrieve the shell-sized icons rather than the sizes specified by the system metrics.
        Handle = 0x000000100,      // The hIcon member of the SHSTOCKICONINFO structure receives a handle to the specified icon.
        SystemIndex = 0x000004000, // The iSysImageImage member of the SHSTOCKICONINFO structure receives the index of the specified icon in the system imagelist.
        LinkOverlay = 0x000008000, // Add the link overlay to the file's icon.
        Selected = 0x000010000     // Blend the icon with the system highlight color.
    }
}
