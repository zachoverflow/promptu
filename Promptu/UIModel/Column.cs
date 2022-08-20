using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class Column
    {
        private string text;
        private bool readOnly;

        public Column(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public bool ReadOnly
        {
            get { return this.readOnly; }
            set { this.readOnly = value; }
        }
    }
}
