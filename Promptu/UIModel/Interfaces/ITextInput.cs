using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZachJohnson.Promptu.PTK;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ITextInput
    {
        string Text { get; set; }

        bool Enabled { get; set; }

        string Cue { get; set; }

        bool CueDisplayed { get; }

        Validator<string> TextValidator { get; set; }

        void GiveTextValidationError(string message);

        void ClearTextValidationError();

        //bool Multiline { get; set; }

        //int? PhysicalWidth { set; }

        event EventHandler TextChanged;

        event KeyEventHandler KeyDown;

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }

        void Select();

        void SelectAll();

        void Select(int start, int length);

        //bool AutoSize { get; set; }
    }
}
