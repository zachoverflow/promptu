using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl
{
    internal class ScanToken
    {
        private object value;
        private int position;
        private int endPosition;

        public ScanToken(object value, int position)
            : this(value, position, position)
        {
        }

        public ScanToken(object value, int position, int endPosition)
        {
            this.value = value;
            this.position = position;
            this.endPosition = endPosition;
        }

        public object Value
        {
            get { return this.value; }
        }

        public int Position
        {
            get { return this.position; }
        }

        public int EndPosition
        {
            get { return this.endPosition; }
        }
    }
}
