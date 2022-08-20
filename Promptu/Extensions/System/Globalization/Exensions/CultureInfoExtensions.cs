//-----------------------------------------------------------------------
// <copyright file="CultureInfoExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
