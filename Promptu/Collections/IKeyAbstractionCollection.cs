//-----------------------------------------------------------------------
// <copyright file="IKeyAbstractionCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    using System.Collections.Generic;

    internal interface IKeyAbstractionCollection : IEnumerable<string>
    {
        int Count { get; }

        string this[int index]
        {
            get;
        }

        bool Contains(string key);
    }
}
