using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.RichText
{
    public class RTGroup : RTElement
    {
        private List<RTElement> children = new List<RTElement>();

        public RTGroup()
        {
        }

        public List<RTElement> Children
        {
            get { return this.children; }
        }
    }
}
