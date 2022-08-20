//-----------------------------------------------------------------------
// <copyright file="ObjectPropertyChangedEventArgs.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;

    public class ObjectPropertyChangedEventArgs : EventArgs
    {
        private ObjectPropertyBase property;

        public ObjectPropertyChangedEventArgs(ObjectPropertyBase property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.property = property;
        }

        public ObjectPropertyBase Property
        {
            get { return this.property; }
        }
    }
}
