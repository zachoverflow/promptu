//-----------------------------------------------------------------------
// <copyright file="ImageExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Drawing.Extensions
{
    using System;

    internal static class ImageExtensions
    {
        public static Bitmap ConvertToBitmap(this Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            Bitmap b = image as Bitmap;
            if (b == null)
            {
                b = new Bitmap(image);
            }

            return b;
        }
    }
}
