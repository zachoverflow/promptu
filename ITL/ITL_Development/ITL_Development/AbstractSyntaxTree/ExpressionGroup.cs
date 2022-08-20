using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class ExpressionGroup : Expression
    {
        private List<Expression> expressions;

        public ExpressionGroup(List<Expression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException("expressions");
            }

            this.expressions = expressions;
        }

        public List<Expression> Expressions
        {
            get { return this.expressions; }
        }
    }
}
