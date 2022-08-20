//-----------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Collections.Generic.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class ListExtensions
    {
        public static void AddRange<TListItem, TAddingAs>(this List<TListItem> list, IEnumerable<TAddingAs> collection)
            where TAddingAs : TListItem
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            else if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (TAddingAs item in collection)
            {
                list.Add(item);
            }
        }

        public static bool IndexIsDefined<T>(this List<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return index >= 0 && index < list.Count;
        }
    }
}
