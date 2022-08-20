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
    using System.Collections;
    using System.Collections.Generic;

    internal class OptimizedStringKeyedDictionaryListAbstractionLayer<TValue> : IList<TValue>
        where TValue : class
    {
        private TrieDictionary<TValue> collection;

        public OptimizedStringKeyedDictionaryListAbstractionLayer(TrieDictionary<TValue> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.collection = collection;
        }

        public int Count
        {
            get { return this.collection.Count; }
        }

        public TrieDictionary<TValue> UnderlyingCollection
        {
            get { return this.collection; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TValue this[int index]
        {
            get { return this.collection[index]; }
            set { this.collection[index] = value; }
        }

        public void Add(TValue value)
        {
            throw new NotSupportedException("Cannot add a value because that requires a string key.  Please use the UnderlyingCollection.");
        }

        public bool Remove(TValue item)
        {
            foreach (string key in this.collection)
            {
                if (this.collection[key, CaseSensitivity.Sensitive] == item)
                {
                    this.collection.Remove(key);
                    return true;
                }
            }

            return false;
        }

        public bool Contains(TValue item)
        {
            foreach (string key in this.collection)
            {
                if (this.collection[key, CaseSensitivity.Sensitive] == item)
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Count && i + arrayIndex < array.Length; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public void Insert(int index, TValue item)
        {
            throw new NotSupportedException("Cannot add a value because that requires a string key.  Please use the UnderlyingCollection.");
        }

        public int IndexOf(TValue item)
        {
            for (int i = 0; i < this.collection.Count; i++)
            {
                if (this.collection[this.collection.Keys[i], CaseSensitivity.Sensitive] == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void RemoveAt(int index)
        {
            this.collection.Remove(this.collection.Keys[index]);
        }

        public void Clear()
        {
            this.collection.Clear();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < this.collection.Count; i++)
            {
                yield return this.collection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
