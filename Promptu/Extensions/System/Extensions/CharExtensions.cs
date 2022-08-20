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

namespace System.Extensions
{
    using System;
    using System.Globalization;

    internal static class CharExtensions
    {
        public static char ReverseCasing(this char c)
        {
            return ReverseCasing(c, CultureInfo.CurrentCulture);
        }

        public static char ReverseCasing(this char c, CultureInfo culture)
        {
            if (Char.IsLower(c))
            {
                return Char.ToUpper(c, culture);
            }
            else if (Char.IsUpper(c))
            {
                return Char.ToLower(c, culture);
            }

            return c;
        }

        public static bool Contains(this char[] array, char character)
        {
            foreach (char ch in array)
            {
                if (character == ch)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
