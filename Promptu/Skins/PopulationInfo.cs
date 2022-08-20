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

namespace ZachJohnson.Promptu.Skins
{
    using System;
    using ZachJohnson.Promptu.Collections;

    internal class PopulationInfo
    {
        private bool success;
        private TrieDictionary<Int32Encapsulator> suggestionItemsAndIndexes;

        public PopulationInfo(bool success, TrieDictionary<Int32Encapsulator> suggestionItemsAndIndexes)
        {
            this.success = success;
            this.suggestionItemsAndIndexes = suggestionItemsAndIndexes;
        }

        public bool Success
        {
            get { return this.success; }
        }

        public int TranslateToIndex(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            bool found;
            Int32Encapsulator index = this.suggestionItemsAndIndexes.TryGetItem(value, CaseSensitivity.Insensitive, out found);

            if (!found || index == null)
            {
                return -1;
            }
            else
            {
                return index;
            }
        }

        public bool ContainsItemName(string value)
        {
            return this.suggestionItemsAndIndexes.Contains(value, CaseSensitivity.Insensitive);
        }

        public int TranslateToNearestMatchIndex(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string nearestMatch = this.suggestionItemsAndIndexes.TryFindKey(value, CaseSensitivity.Insensitive);
            if (nearestMatch != null)
            {
                return this.TranslateToIndex(nearestMatch);
            }
            else
            {
                return -1;
            }
        }
    }
}
