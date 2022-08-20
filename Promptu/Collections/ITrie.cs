//-----------------------------------------------------------------------
// <copyright file="ITrie.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    internal interface ITrie
    {
        int Count { get; }

        bool Contains(string value, CaseSensitivity caseSensitivity);
    }
}
