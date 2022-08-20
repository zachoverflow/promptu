//-----------------------------------------------------------------------
// <copyright file="GroupingConversionInfo.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    public class GroupingConversionInfo
    {
        private string groupName;
        private bool groupEditControl;

        public GroupingConversionInfo(string groupName, bool groupEditControl)
        {
            this.groupName = groupName;
            this.groupEditControl = groupEditControl;
        }

        public string GroupName
        {
            get { return this.groupName; }
        }

        public bool GroupEditControl
        {
            get { return this.groupEditControl; }
        }
    }
}
