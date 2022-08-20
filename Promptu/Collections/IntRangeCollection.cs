//-----------------------------------------------------------------------
// <copyright file="IntRangeCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    using System;
    using System.Collections.Generic;

    internal class IntRangeCollection : List<IntRange>
    {
        public IntRangeCollection()
        {
        }

        public bool IsTaken(IntRange range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }

            List<IntRange> rangesLeft = new List<IntRange>();
            rangesLeft.Add(range);

            foreach (IntRange item in this)
            {
                List<IntRange> newRangesLeft = new List<IntRange>();
                foreach (IntRange rangeLeft in rangesLeft)
                {
                    newRangesLeft.AddRange(rangeLeft.Subtract(item));
                }

                rangesLeft = newRangesLeft;

                if (rangesLeft.Count <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
