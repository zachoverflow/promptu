//-----------------------------------------------------------------------
// <copyright file="IHasOptionsAndProperties.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using ZachJohnson.Promptu.PluginModel;

    public interface IHasOptionsAndProperties
    {
        OptionPage Options { get; }

        ObjectPropertyCollection SavingProperties { get; }
    }
}
