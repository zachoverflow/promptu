using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    // TODO test custom bindings at multi level
    internal class OverwriteOption
    {
        private string label;
        private string supplementalExplaination;
        private OverwriteOptionExtraInfo extraInfo;

        public OverwriteOption(
            string label, 
            string supplementalExplaination, 
            OverwriteOptionExtraInfo extraInfo)
        {
            this.label = label;
            this.supplementalExplaination = supplementalExplaination;
            this.extraInfo = extraInfo;
        }

        public OverwriteOptionExtraInfo ExtraInfo
        {
            get { return this.extraInfo; }
        }

        public string Label
        {
            get { return this.label; }
        }

        public string SupplementalExplaination
        {
            get { return this.supplementalExplaination; }
        }
    }
}
