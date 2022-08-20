//-----------------------------------------------------------------------
// <copyright file="IDictionaryExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Collections.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class IDictionaryExtensions
    {
        public static void Insert<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("The index cannot be less than zero.");
            }
            else if (index > dictionary.Count)
            {
                throw new ArgumentOutOfRangeException("The index is greater than the size of the dictionary.");
            }
            else if (index == dictionary.Count)
            {
                dictionary.Add(key, value);
                return;
            }

            Dictionary<TKey, TValue> temp = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                temp.Add(pair.Key, pair.Value);
            }

            dictionary.Clear();

            int i = 0;
            foreach (KeyValuePair<TKey, TValue> pair in temp)
            {
                if (i == index)
                {
                    dictionary.Add(key, value);
                }

                dictionary.Add(pair.Key, pair.Value);
                i++;
            }
        }
    }
}
