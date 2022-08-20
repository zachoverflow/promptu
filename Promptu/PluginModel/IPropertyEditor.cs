//-----------------------------------------------------------------------
// <copyright file="IPropertyEditor.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public interface IPropertyEditor
    {
        ObjectPropertyBase Context { set; }
    }
}
