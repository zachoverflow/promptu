//-----------------------------------------------------------------------
// <copyright file="SuperTabWidget.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal class SuperTabWidget : TabWidgetBase<SuperTabPage, ISuperTabPage>
    {
        public SuperTabWidget(string id)
            : this(id, InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSuperTabControl())
        {
        }

        internal SuperTabWidget(string id, ITabControl nativeInterface)
            : base(id, nativeInterface)
        {
        }
    }
}
