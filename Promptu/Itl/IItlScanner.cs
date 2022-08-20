//-----------------------------------------------------------------------
// <copyright file="IItlScanner.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl
{
    using System.Collections.Generic;

    internal interface IItlScanner
    {
        List<ScanToken> Results { get; }

        bool HasJustText { get; }

        FeedbackCollection Feedback { get; }
    }
}
