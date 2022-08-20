//-----------------------------------------------------------------------
// <copyright file="IAppliable.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public interface IAppliable
    {
        void ApplyTo(object obj);
    }
}
