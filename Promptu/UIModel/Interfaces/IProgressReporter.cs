using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IProgressReporter
    {
        void ReportProgress(double value);
    }
}
