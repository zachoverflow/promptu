using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal class IntRange
    {
        int? lower;
        int? upper;

        public IntRange(int? lower, int? upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public int? Lower
        {
            get { return lower; }
        }

        public int? Upper
        {
            get { return this.upper; }
        }

        public bool Contains(IntRange otherRange)
        {
            if (otherRange == null)
            {
                throw new ArgumentNullException("otherRange");
            }

            if (this.Lower.HasValue)
            {
                if (!otherRange.Lower.HasValue || otherRange.Lower < this.lower)
                {
                    return false;
                }
            }

            if (this.Upper.HasValue)
            {
                if (!otherRange.Upper.HasValue || otherRange.Upper > this.upper)
                {
                    return false;
                }
            }

            return true;
        }

        public List<IntRange> Subtract(IntRange range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }

            List<IntRange> remainingRange = new List<IntRange>();

            if (!range.Contains(this))
            {
                if (range.Lower != null && (this.Upper == null || range.Lower < this.Upper))
                {
                    remainingRange.Add(new IntRange(this.Lower, range.Lower - 1));
                }

                if (range.Upper != null && (this.Lower == null || range.Upper > this.Lower))
                {
                    remainingRange.Add(new IntRange(range.Upper + 1, this.Upper));
                }

                if (remainingRange.Count == 0)
                {
                    remainingRange.Add(this);
                }
            }

            return remainingRange;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "({0})-({1})", this.Lower == null ? "n" : this.Lower.Value.ToString(CultureInfo.CurrentCulture), this.Upper == null ? "n" : this.Upper.Value.ToString(CultureInfo.CurrentCulture));
        }
    }
}
