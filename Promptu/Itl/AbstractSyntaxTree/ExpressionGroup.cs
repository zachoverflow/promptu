//-----------------------------------------------------------------------
// <copyright file="ExpressionGroup.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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

        public override string ConvertToString(ExecutionData data)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Expression item in this.Expressions)
            {
                builder.Append(item.ConvertToString(data));
            }

            return builder.ToString();
        }
    }
}
