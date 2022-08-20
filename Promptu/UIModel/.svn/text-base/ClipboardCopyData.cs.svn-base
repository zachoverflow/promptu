using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal class ClipboardCopyData
    {
        private static ClipboardCopyData lastSetData;
        private ItemCopyInfo info;
        private string listId;
        private bool cutNotCopy;

        public ClipboardCopyData(ItemCopyInfo info, string listId, bool cutNotCopy)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            else if (listId == null)
            {
                throw new ArgumentNullException("listId");
            }

            this.info = info;
            this.listId = listId;
            this.cutNotCopy = cutNotCopy;
        }

        public static ClipboardCopyData LastSetData
        {
            get { return lastSetData; }
            set { lastSetData = value; }
        }

        public ItemCopyInfo Info
        {
            get { return this.info; }
        }

        public string ListId
        {
            get { return this.listId; }
        }

        public bool CutNotCopy
        {
            get { return this.cutNotCopy; }
        }
    }
}
