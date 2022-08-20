//-----------------------------------------------------------------------
// <copyright file="WidgetEventArgs.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using System;

    internal class WidgetEventArgs : EventArgs
    {
        private Widget widget;

        public WidgetEventArgs(Widget widget)
        {
            this.widget = widget;
        }

        public Widget Widget
        {
            get { return this.widget; }
        }
    }
}
