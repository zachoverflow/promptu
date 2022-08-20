//-----------------------------------------------------------------------
// <copyright file="IIndexedCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    using System.Collections.Generic;

    internal interface IIndexedCollection<T> : IEnumerable<T>, IHasCount
    {
        T this[int index] 
        { 
            get; 
        }
    }
}
