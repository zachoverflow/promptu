//-----------------------------------------------------------------------
// <copyright file="ItemValidator.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Skins
{
    using ZachJohnson.Promptu.SkinApi;

    internal class ItemValidator
    {
        public ItemValidator()
        {
        }

        public virtual bool Include(string name, SuggestionItemType itemType)
        {
            return true;
        }
    }
}
