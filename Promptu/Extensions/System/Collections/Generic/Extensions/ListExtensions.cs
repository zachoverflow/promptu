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

namespace System.Collections.Generic.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class ListExtensions
    {
        public static void AddRange<TListItem, TAddingAs>(this List<TListItem> list, IEnumerable<TAddingAs> collection)
            where TAddingAs : TListItem
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (TAddingAs item in collection)
            {
                list.Add(item);
            }
        }

        public static bool IndexIsDefined<T>(this List<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return index >= 0 && index < list.Count;
        }
    }
}
