//-----------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
