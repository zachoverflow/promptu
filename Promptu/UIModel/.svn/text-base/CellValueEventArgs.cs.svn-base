using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class CellValueEventArgs : CellEventArgs
    {
        private object value;

        public CellValueEventArgs(int column, int row, object value)
            : base(column, row)
        {
            this.value = value;
        }

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
