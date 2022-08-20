//-----------------------------------------------------------------------
// <copyright file="CharExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
