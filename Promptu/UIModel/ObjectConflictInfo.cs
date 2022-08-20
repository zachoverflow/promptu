using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class ObjectConflictInfo
    {
        ItemCompareEntry name;
        List<ItemCompareEntry> attributes = new List<ItemCompareEntry>();
        ConflictObjectType objectType;

        public ObjectConflictInfo(ItemCompareEntry name, ConflictObjectType objectType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
            this.objectType = objectType;
        }

        public ItemCompareEntry Name
        {
            get { return this.name; }
        }

        public List<ItemCompareEntry> Attributes
        {
            get { return this.attributes; }
        }

        public ConflictObjectType ObjectType
        {
            get { return this.objectType; }
        }
    }
}
