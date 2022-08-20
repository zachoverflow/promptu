using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal class HistoryDetails
    {
        private string entryValue;
        private bool? isValidPath;
        private string[] pathSplit = null;
        private string itemId = null;

        public HistoryDetails(string entryValue, string itemId)
        {
            this.entryValue = entryValue;
            this.itemId = itemId;
        }

        public string ItemId
        {
            get { return this.itemId; }
        }

        public string EntryValue
        {
            get { return this.entryValue; }
        }

        public string[] PathSplit
        {
            get { return this.pathSplit; }
        }

        public bool IsValidPath
        {
            get
            {
                if (this.isValidPath == null)
                {
                    this.isValidPath = Utilities.LooksLikeValidPath(this.entryValue, out this.pathSplit);
                }


                return this.isValidPath.Value;
            }
        }

        public HistoryDetails Clone()
        {
            HistoryDetails clone = new HistoryDetails(this.entryValue, this.itemId);
            clone.isValidPath = this.isValidPath;
            clone.pathSplit = this.pathSplit;
            return clone;
        }
    }
}
