//-----------------------------------------------------------------------
// <copyright file="TabPageEventArgs.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PTK
{
    using System;
    using ZachJohnson.Promptu.UIModel.Interfaces;

    internal class TabPageEventArgs<TPage, TTabPage> : EventArgs 
        where TPage : TabPageBase<TPage, TTabPage>
        where TTabPage : ITabPage
    {
        private TPage tabPage;

        public TabPageEventArgs(TPage tabPage)
        {
            this.tabPage = tabPage;
        }

        public TPage TabPage
        {
            get { return this.tabPage; }
        }
    }
}
