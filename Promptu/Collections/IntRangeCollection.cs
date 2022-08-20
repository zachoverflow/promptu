// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
