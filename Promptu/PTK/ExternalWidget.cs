//-----------------------------------------------------------------------
// <copyright file="ExternalWidget.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using System;

    internal class ExternalWidget : Widget
    {
        private object nativeObject;

        public ExternalWidget(string id, object nativeObject)
            : base(id)
        {
            if (nativeObject == null)
            {
                throw new ArgumentNullException("nativeObject");
            }

            this.nativeObject = nativeObject;
        }

        internal override object NativeObject
        {
            get { return this.nativeObject; }
        }
    }
}
