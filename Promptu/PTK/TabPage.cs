//-----------------------------------------------------------------------
// <copyright file="TabPage.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal class TabPage : TabPageBase<TabPage, ITabPage>
    {
        public TabPage(string id)
            : base(id)
        {
            this.NativeInterface = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructTabPage();
        }
    }
}
