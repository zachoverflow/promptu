//-----------------------------------------------------------------------
// <copyright file="SuperTabPage.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal class SuperTabPage : TabPageBase<SuperTabPage, ISuperTabPage>
    {
        public SuperTabPage(string id)
            : base(id)
        {
            this.NativeInterface = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSuperTabPage();
        }

        public object Image
        {
            get { return this.NativeInterface.Image; }
            set { this.NativeInterface.Image = value; }
        }
    }
}
