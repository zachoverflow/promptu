using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UIModel
{
    internal class CellValidatingEventArgs : CancelEventArgs
    {
        private object formattedValue;
        private int column;
        private int row;

        public CellValidatingEventArgs(int column, int row, object formattedValue)
        {
            this.column = column;
            this.row = row;
            this.formattedValue = formattedValue;
        }

        public object FormattedValue
        {
            get { return this.formattedValue; } 
        }

        public int Column
        {
            get { return this.column; }
        }

        public int Row
        {
            get { return this.row; }
        }
    }
}
