//-----------------------------------------------------------------------
// <copyright file="PointExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Drawing.Extensions
{
    using System;

    internal static class PointExtensions
    {
        public static Point Invert(this Point pt)
        {
            return new Point(-pt.X, -pt.Y);
        }

        public static int DistanceTo(this Point thisPt, Point pt)
        {
            return (int)Math.Sqrt(Math.Pow((thisPt.X - pt.X), 2) + Math.Pow((thisPt.Y - pt.Y), 2));
        }
    }
}
