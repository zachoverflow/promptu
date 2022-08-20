//-----------------------------------------------------------------------
// <copyright file="Widget.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using System;

    internal abstract class Widget
    {
        private string id;
        private IWidgetHost currentHost;

        public Widget(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.id = id;
        }

        public string Id
        {
            get { return this.id; }
        }

        internal abstract object NativeObject { get; }

        internal IWidgetHost CurrentHost
        {
            get { return this.currentHost; }
            set { this.currentHost = value; }
        }

        internal void UnhostIfNecessary()
        {
            if (this.currentHost != null)
            {
                this.currentHost.Remove(this);
            }
        }
    }
}
