using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ICheckBox
    {
        event EventHandler CheckedChanged;

        bool Checked { get; set; }

        string Text { get; set; }

        string ToolTipText { get; set; }

        bool Enabled { get; set; }

        bool Visible { get; set; }
    }
}
