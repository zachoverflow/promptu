//-----------------------------------------------------------------------
// <copyright file="SuggestionItemType.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;

    [Flags]
    public enum SuggestionItemType
    {
        None = 0,
        History = 1,
        NativePromptuCommand = 2,
        Command = 4,
        Function = 8,
        Folder = 16,
        File = 32,
        Namespace = 64,
        ValueListItem = 128
    }
}
