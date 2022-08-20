using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    // TODO evaluate plugin, make sure internal uses this
    internal class UIMenuItemInternal : UIMenuItem, ILockedInternal
    {
        public UIMenuItemInternal(string id)
            : base(id)
        {
        }

        public UIMenuItemInternal(string id, string text)
            : base(id, text)
        {
        }

        //public UIMenuItemInternal(string id, string text, string toolTipText)
        //    : base(id, text, toolTipText)
        //{
        //}

        public UIMenuItemInternal(string id, string text, EventHandler clickEventHandler)
            : base(id, text, clickEventHandler)
        {
        }

        public UIMenuItemInternal(string id, string text, object image, EventHandler clickEventHandler)
            : base(id, text, image, clickEventHandler)
        {
        }

        public UIMenuItemInternal(string id, string text, string toolTipText, object image, EventHandler clickEventHandler)
            : base(id, text, toolTipText, image, clickEventHandler)
        {
        }

        public UIMenuItemInternal(string id, string text, string toolTipText, object image)
            : base(id, text, toolTipText, image, null)
        {
        }
    }
}
