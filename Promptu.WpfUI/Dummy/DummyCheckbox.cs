using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyCheckbox : ICheckBox
    {
#pragma warning disable 0067
        public event EventHandler CheckedChanged;
#pragma warning restore 0067

        public bool Checked
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public string Text
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }

        public string ToolTipText
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }


        public bool Enabled
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public bool Visible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
    }
}
