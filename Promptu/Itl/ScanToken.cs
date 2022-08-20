//-----------------------------------------------------------------------
// <copyright file="ScanToken.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl
{
    internal class ScanToken
    {
        private object value;
        private int position;
        private int endPosition;

        public ScanToken(object value, int position)
            : this(value, position, position + 1)
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

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
