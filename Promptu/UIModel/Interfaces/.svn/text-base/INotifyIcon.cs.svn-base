using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface INotifyIcon
    {
        event ParameterlessVoid TimeToOpenPrompt;

        string ToolTipText { get; set; }

        Icon Icon { get; set; }

        UIContextMenu ContextMenu { get; }

        void Hide();

        void Show();
    }
}
