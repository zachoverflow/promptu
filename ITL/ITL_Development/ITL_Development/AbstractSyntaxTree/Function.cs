using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class Function : Expression
    {
        private Identifier identifier;
        private List<Expression> parameters;

        public Function(Identifier identifier, List<Expression> parameters)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }
            else if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            this.identifier = identifier;
            this.parameters = parameters;
        }

        public Identifier Identifier
        {
            get { return this.identifier; }
        }

        public List<Expression> Parameters
        {
            get { return this.parameters; }
        }
    }
}
