//-----------------------------------------------------------------------
// <copyright file="TabWidget.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal class TabWidget : TabWidgetBase<TabPage, ITabPage>
    {
        public TabWidget(string id)
            : this(id, InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructTabControl())
        {
        }

        internal TabWidget(string id, ITabControl nativeInterface)
            : base(id, nativeInterface)
        {
        }
    }
}
