using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.SkinsApi
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class UserEditableAttribute : Attribute
    {
        private bool editableByUser;

        public UserEditableAttribute()
            : this(true)
        {
        }

        public UserEditableAttribute(bool editableByUser)
        {
            this.editableByUser = editableByUser;
        }

        public bool EditableByUser
        {
            get { return this.editableByUser; }
        }
    }
}
