using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ICheckBoxButton : IButton
    {
        bool Checked { get; set; }

        event EventHandler CheckedChanged;
    }
}
