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

namespace System.Drawing.Extensions
{
    internal static class ColorExtensions
    {
        public static Color BlendByAlphaOnto(this Color sourceColor, Color backgroundColor)
        {
            // Formula: displayColor = sourceColor × sourceAlpha / 255 + backgroundColor × (255 – sourceAlpha) / 255
            int a = (sourceColor.A * sourceColor.A / 255) + (backgroundColor.A * (255 - sourceColor.A) / 255);
            int r = (sourceColor.R * sourceColor.A / 255) + (backgroundColor.R * (255 - sourceColor.A) / 255);
            int g = (sourceColor.G * sourceColor.A / 255) + (backgroundColor.G * (255 - sourceColor.A) / 255);
            int b = (sourceColor.B * sourceColor.A / 255) + (backgroundColor.B * (255 - sourceColor.A) / 255);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
