//-----------------------------------------------------------------------
// <copyright file="BooleanConversionInfo.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    internal class BooleanConversionInfo
    {
        private string radioGroup;

        public BooleanConversionInfo(string radioGroup)
        {
            this.radioGroup = radioGroup;
        }

        public string RadioGroup
        {
            get { return this.radioGroup; }
        }
    }
}
