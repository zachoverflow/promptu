using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class Sequence : Expression
    {
        private Expression firstExpression;
        private Expression secondExpression;

        public Sequence(Expression firstExpression, Expression secondExpression)
        {
            if (firstExpression == null)
            {
                throw new ArgumentNullException("firstExpression");
            }
            else if (secondExpression == null)
            {
                throw new ArgumentNullException("secondExpression");
            }

            this.firstExpression = firstExpression;
            this.secondExpression = secondExpression;
        }

        public Expression FirstExpression
        {
            get { return this.firstExpression; }
        }

        public Expression SecondExpression
        {
            get { return this.secondExpression; }
        }
    }
}
