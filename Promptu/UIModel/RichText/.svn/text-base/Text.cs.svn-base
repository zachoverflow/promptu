using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.RichText
{
    public class Text : RTElement
    {
        private string value;
        private TextStyle style;

        public Text(string value, TextStyle style)
        {
            this.value = value;
            this.style = style;
        }

        public string Value
        {
            get { return this.value; }
        }

        public TextStyle Style
        {
            get { return this.style; }
        }

        public static Text Generate(string value, bool bold)
        {
            return Generate(value, bold, false);
        }

        public static Text Generate(string value, bool bold, bool flow)
        {
            // TODO handle flow
            return new Text(value, bold ? TextStyle.Bold : TextStyle.Normal);
        }
    }
}
