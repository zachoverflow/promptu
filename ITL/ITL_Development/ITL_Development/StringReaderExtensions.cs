using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace ZachJohnson.Promptu.Itl
{
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
