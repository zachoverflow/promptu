using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IOptionsProgressBar : IProgressReporter
    {
        string Text { get; set; }

        bool Visible { get; set; }

        bool IsIndeterminate { get; set; }

        double PercentComplete { get; set; }

        int Indent { set; }
    }
}
