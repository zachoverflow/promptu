using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyNotifyIcon : INotifyIcon
    {
        private UIContextMenu menu = new UIContextMenu();

        public event ParameterlessVoid TimeToOpenPrompt;

        public string ToolTipText
        {
            get;
            set;
        }

        public System.Drawing.Icon Icon
        {
            get;
            set;
        }

        public UIModel.UIContextMenu ContextMenu
        {
            get { return this.menu; }
        }

        public void Hide()
        {
        }

        public void Show()
        {
        }
    }
}
