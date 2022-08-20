//-----------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToStringNullSafe(this object o)
        {
            if (o == null)
            {
                return null;
            }
            else
            {
                return o.ToString();
            }
        }
    }
}
