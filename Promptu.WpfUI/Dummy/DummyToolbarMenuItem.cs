using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    class DummyToolbarMenuItem : IToolbarMenuItem
    {
        public event EventHandler Click;

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

        public bool Available
        {
            get
            {
                return true;
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
    }
}
