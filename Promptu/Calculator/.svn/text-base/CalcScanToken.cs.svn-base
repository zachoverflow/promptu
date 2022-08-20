using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Calculator
{
    internal class CalcScanToken
    {
        private object value;
        private int position;
        private int endPosition;

        public CalcScanToken(object value, int position)
            : this(value, position, position + 1)
        {
        }

        public CalcScanToken(object value, int position, int endPosition)
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

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
