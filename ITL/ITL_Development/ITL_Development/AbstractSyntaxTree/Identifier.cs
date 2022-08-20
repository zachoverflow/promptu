using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class Identifier
    {
        private string name;

        public Identifier(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
