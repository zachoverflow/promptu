//-----------------------------------------------------------------------
// <copyright file="IdAttribute.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class IdAttribute : Attribute
    {
        private string id;

        public IdAttribute(string id)
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
    }
}
