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

namespace System.Globalization.Exensions
{
    using System;
    using System.Windows.Forms;

    internal static class CultureInfoExtensions
    {
        public static MessageBoxOptions GetMessageBoxOptions(this CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            MessageBoxOptions options = (MessageBoxOptions)0;

            if (culture.TextInfo.IsRightToLeft)
            {
                options |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
            }

            return options;
        }
    }
}
