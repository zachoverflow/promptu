//-----------------------------------------------------------------------
// <copyright file="IPropertyEditorFactory.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public interface IPropertyEditorFactory
    {
        IPropertyEditor CreateEditor();
    }
}
