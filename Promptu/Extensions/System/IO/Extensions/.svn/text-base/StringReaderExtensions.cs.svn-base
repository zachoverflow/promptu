//-----------------------------------------------------------------------
// <copyright file="StringReaderExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.IO.Extensions
{
    using System.IO;
    using System.Reflection;

    internal static class StringReaderExtensions
    {
        private static FieldInfo positionInfo;

        public static int GetPosition(this StringReader reader)
        {
            if (positionInfo == null)
            {
                positionInfo = reader.GetType().GetField("_pos", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            return (int)positionInfo.GetValue(reader);
        }
    }
}
