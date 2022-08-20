using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UI;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class VisualDisplayInfo
    {
        //private string header;
        //private string content;
        private List<ItemCompareEntry> entries;

        public VisualDisplayInfo(ItemCompareEntry entry)
        {
            this.entries = new List<ItemCompareEntry>();
            this.entries.Add(entry);
        }

        public VisualDisplayInfo(List<ItemCompareEntry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException("entries");
            }

            this.entries = entries;
            //if (header == null)
            //{
            //    throw new ArgumentNullException("header");
            //}

            //this.header = header;
            //this.content = content;
        }

        public List<ItemCompareEntry> Entries
        {
            get { return this.entries; }
        }

        //public string Header
        //{
        //    get { return this.header; }
        //}

        //public string Content
        //{
        //    get { return this.content; }
        //}
    }
}
