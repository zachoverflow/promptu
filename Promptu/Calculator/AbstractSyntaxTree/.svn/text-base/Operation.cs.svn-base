using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Calculator.AbstractSyntaxTree
{
    internal class Operation : Expression
    {
        private char op;
        private Expression first;
        private Expression second;

        public Operation(char op, Expression first, Expression second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            this.op = op;
            this.first = first;
            this.second = second;
        }

        public static bool IsValidOperator(char op)
        {
            switch (Char.ToUpperInvariant(op))
            {
                case '*':
                case '/':
                case '+':
                case '-':
                case '%':
                case '^':
                    return true;
                default:
                    return false;
            }
        }

        public override double ConvertToDouble()
        {
            double firstResult = this.first.ConvertToDouble();
            double secondResult = this.second.ConvertToDouble();
            switch (Char.ToUpperInvariant(this.op))
            {
                case '*':
                    return firstResult * secondResult;
                case '/':
                    return firstResult / secondResult;
                case '+':
                    return firstResult + secondResult;
                case '-':
                    return firstResult - secondResult;
                case '%':
                    return firstResult % secondResult;
                case '^':
                    return Math.Pow(firstResult, secondResult);
                default:
                    throw new ConversionException(String.Format("'{0}' is not a valid operator.", this.op));
            }
        }
    }
}
