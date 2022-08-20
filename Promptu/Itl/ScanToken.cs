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
