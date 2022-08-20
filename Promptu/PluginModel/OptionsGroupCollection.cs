//-----------------------------------------------------------------------
// <copyright file="OptionsGroupCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using ZachJohnson.Promptu.UIModel;

    public class OptionsGroupCollection : BindingCollection<OptionsGroup>
    {
        public OptionsGroupCollection()
        {
        }

        public void DisposeAllChildren()
        {
            foreach (OptionsGroup group in this)
            {
                foreach (object o in group)
                {
                    IDisposable disposable = o as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }
    }
}
