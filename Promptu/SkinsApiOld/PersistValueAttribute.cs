using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.SkinsApi
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class PersistValueAttribute : Attribute
    {
        readonly bool persistValue;

        public PersistValueAttribute()
            : this(true)
        {
        }

        public PersistValueAttribute(bool persistValue)
        {
            this.persistValue = persistValue;
        }

        public bool PersistValue
        {
            get { return persistValue; }
        }
    }
}
