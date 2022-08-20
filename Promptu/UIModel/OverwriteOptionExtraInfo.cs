using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class OverwriteOptionExtraInfo
    {
        private ItemInfo itemInfo;
        private ConflictObjectType objectType;
        private int itemCount;

        public OverwriteOptionExtraInfo(
            ConflictObjectType objectType, 
            ItemInfo itemInfo, 
            int itemCount)
        {
            this.objectType = objectType;
            this.itemInfo = itemInfo;
            this.itemCount = itemCount;
        }

        public ItemInfo ItemInfo
        {
            get { return this.itemInfo; }
        }

        public ConflictObjectType ObjectType
        {
            get { return this.objectType; }
        }

        public int ItemCount
        {
            get { return this.itemCount; }
        }
    }
}
