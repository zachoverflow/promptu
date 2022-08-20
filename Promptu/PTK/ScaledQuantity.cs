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

namespace ZachJohnson.Promptu.PTK
{
    internal struct ScaledQuantity
    {
        private float value;
        private float? scalePpi;

        public ScaledQuantity(float value, float? scalePpi)
        {
            this.value = value;
            this.scalePpi = scalePpi;
        }

        public float Value
        {
            get { return this.value; }
        }

        public float? ScalePpi
        {
            get { return this.scalePpi; }
        }

        public static ScaledQuantity At96Ppi(float value)
        {
            return new ScaledQuantity(value, 96);
        }

        public static ScaledQuantity At120Ppi(float value)
        {
            return new ScaledQuantity(value, 120);
        }
    }
}
