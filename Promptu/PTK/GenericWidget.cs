//-----------------------------------------------------------------------
// <copyright file="GenericWidget.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using System;

    internal class GenericWidget<TNativeInterface> : Widget
    {
        private TNativeInterface nativeInterface;

        public GenericWidget(string id, TNativeInterface nativeInterface)
            : base(id)
        {
            if (nativeInterface == null)
            {
                throw new ArgumentNullException("nativeInterface");
            }

            this.nativeInterface = nativeInterface;
        }

        internal override object NativeObject
        {
            get { return this.nativeInterface; }
        }

        internal TNativeInterface NativeInterface
        {
            get { return this.nativeInterface; }
        }
    }
}
