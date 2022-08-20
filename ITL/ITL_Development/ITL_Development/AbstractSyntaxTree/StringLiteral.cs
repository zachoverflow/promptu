using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class StringLiteral : Expression
    {
        private string value;

        public StringLiteral(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }
    }
}
