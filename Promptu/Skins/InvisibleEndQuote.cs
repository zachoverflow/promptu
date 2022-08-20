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

    internal class InvisibleEndQuote
    {
        private int indexOfOpenQuote;
        private int numberOfSpacesInQuote;

        public InvisibleEndQuote(int indexOfOpenQuote, int numberOfSpacesInQuote)
        {
            this.numberOfSpacesInQuote = numberOfSpacesInQuote;
            this.indexOfOpenQuote = indexOfOpenQuote;
        }

        public int NumberOfSpacesInQuote
        {
            get { return this.numberOfSpacesInQuote; }
            set { this.numberOfSpacesInQuote = value; }
        }

        public int IndexOfOpenQuote
        {
            get { return this.indexOfOpenQuote; }
            set { this.indexOfOpenQuote = value; }
        }

        public int GetIndex(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (this.indexOfOpenQuote >= text.Length)
            {
                throw new ArgumentOutOfRangeException("'IndexOfOpenQuote' cannot be greater than or equal to 'text.Length'.");
            }
            
            int terminatingIndex = text.Length;
            int spacesSeen = 0;

            for (int i = this.IndexOfOpenQuote; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    if (spacesSeen >= this.NumberOfSpacesInQuote)
                    {
                        terminatingIndex = i;
                        break;
                    }
                    else
                    {
                        spacesSeen++;
                    }
                }
            }

            return terminatingIndex;
        }
    }
}
