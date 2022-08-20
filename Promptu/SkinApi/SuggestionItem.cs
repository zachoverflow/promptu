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

namespace ZachJohnson.Promptu.SkinApi
{
    using System;
    using ZachJohnson.Promptu.UI;

    public class SuggestionItem : IHasImageIndex, IHasFallbackImageIndex
    {
        private SuggestionItemType type;
        private string text;
        private int? fallbackImageIndex;
        private int imageIndex;

        public SuggestionItem(SuggestionItemType type, string text, int imageIndex)
            : this(type, text, imageIndex, null)
        {
        }

        public SuggestionItem(SuggestionItemType type, string text, int imageIndex, int? fallbackImageIndex)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.text = text;
            this.type = type;
            this.imageIndex = imageIndex;
            this.fallbackImageIndex = fallbackImageIndex;
        }

        public SuggestionItemType Type
        {
            get { return this.type; }
        }

        public string Text
        {
            get { return this.text; }
        }

        public int ImageIndex
        {
            get { return this.imageIndex; }
            set { this.imageIndex = value; }
        }

        public int? FallbackImageIndex
        {
            get { return this.fallbackImageIndex; }
        }

        public override string ToString()
        {
            return this.text;
        }
    }
}
