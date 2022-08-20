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

    internal class CompositeList<T>
    {
        private List<IList<T>> lists;

        public CompositeList()
        {
            this.lists = new List<IList<T>>();
        }

        public int Count
        {
            get
            {
                int count = 0;

                foreach (IList<T> list in this.lists)
                {
                    count += list.Count;
                }

                return count;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("Index cannot be less than zero.");
                }

                foreach (IList<T> list in this.lists)
                {
                    if (index >= list.Count)
                    {
                        index -= list.Count;
                    }
                    else
                    {
                        return list[index];
                    }
                }

                throw new ArgumentOutOfRangeException("Index cannot be greater than or equal to 'Count'.");
            }
        }

        public void ClearLists()
        {
            this.lists.Clear();
        }

        public void AddListToComposite(IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            this.lists.Add(list);
        }
    }
}
