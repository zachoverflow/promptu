using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.RichText
{
    public class KeyedImage : RTElement
    {
        private string key;

        public KeyedImage(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return this.key; }
        }
    }
}
