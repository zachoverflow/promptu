using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyButton : IButton, IToolbarButton
    {
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

        public object Image
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public event EventHandler Click;

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


        public bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
