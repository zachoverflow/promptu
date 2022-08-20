//-----------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Collections.Generic.Extensions
{
    using System.Collections.Generic;
    using ZachJohnson.Promptu;

    internal static class IEnumerableExtensions
    {
        public static bool Contains<T>(this IEnumerable<T> collection, NameGetter<T> nameGetter, string name, bool caseSensitive)
        {
            string nameToCompare = name;

            if (!caseSensitive)
            {
                nameToCompare = name.ToUpperInvariant();
            }

            foreach (T collectionItem in collection)
            {
                string itemName = nameGetter(collectionItem);
                if (!caseSensitive)
                {
                    itemName = itemName.ToUpperInvariant();
                }

                if (itemName == nameToCompare)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
