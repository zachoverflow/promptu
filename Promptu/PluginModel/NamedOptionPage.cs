//-----------------------------------------------------------------------
// <copyright file="NamedOptionPage.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public class NamedOptionPage : OptionPage
    {
        private string tabName;

        public NamedOptionPage(string mainInstructions, string tabName)
            : base(mainInstructions)
        {
            this.tabName = tabName;
        }

        public string TabName
        {
            get { return this.tabName; }
        }
    }
}
