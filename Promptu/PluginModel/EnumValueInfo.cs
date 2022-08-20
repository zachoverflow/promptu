//-----------------------------------------------------------------------
// <copyright file="EnumValueInfo.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;

    public class EnumValueInfo
    {
        private Enum value;
        private string displayString;

        public EnumValueInfo(Enum value, string displayString)
        {
            this.value = value;
            this.displayString = displayString;
        }

        public Enum Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return this.displayString ?? this.value.ToString();
        }
    }
}
