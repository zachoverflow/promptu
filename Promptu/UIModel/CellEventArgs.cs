using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class CellEventArgs : EventArgs
    {
        private readonly int column;
        private readonly int row;

        public CellEventArgs(int column, int row)
        {
            this.column = column;
            this.row = row;
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
