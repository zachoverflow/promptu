using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IOpenFileDialog : IDialog
    {
        string Path { get; set; }

        string[] Paths { get; }

        string InitialDirectory { get; set; }

        string Filter { get; set; }

        bool Multiselect { get; set; }

        bool CheckPathExists { get; set; }

        bool CheckFileExists { get; set; }
    }
}
