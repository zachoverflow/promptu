//-----------------------------------------------------------------------
// <copyright file="OptionPage.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public class OptionPage
    {
        private string mainInstructions;
        private OptionsGroupCollection groups;

        public OptionPage()
            : this(null)
        {
        }

        public OptionPage(string mainInstructions)
        {
            this.mainInstructions = mainInstructions;
            this.groups = new OptionsGroupCollection();
        }

        public string MainInstructions
        {
            get { return this.mainInstructions; }
        }

        public OptionsGroupCollection Groups
        {
            get { return this.groups; }
        }
    }
}
