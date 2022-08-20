using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.RichText
{
    public class Space : RTElement
    {
        private int width;

        public Space(int width)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width");
            }

            this.width = width;
        }

        public int Width
        {
            get { return this.width; }
        }
    }
}
