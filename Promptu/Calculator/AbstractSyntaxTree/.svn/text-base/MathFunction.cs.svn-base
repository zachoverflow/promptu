using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Calculator.AbstractSyntaxTree
{
    internal class MathFunction : Expression
    {
        private string functionName;
        private Expression innerExpression;

        public MathFunction(string functionName, Expression innerExpression)
        {
            if (functionName == null)
            {
                throw new ArgumentNullException("functionName");
            }
            else if (innerExpression == null)
            {
                throw new ArgumentNullException("innerExpression");
            }

            this.functionName = functionName;
            this.innerExpression = innerExpression;
        }

        public static bool IsValidMathFunctionName(string functionName)
        {
            switch (functionName.ToUpperInvariant())
            {
                case "SIN":
                case "COS":
                case "TAN":
                case "CSC":
                case "SEC":
                case "COT":
                case "ARCSIN":
                case "ARCCOS":
                case "ARCTAN":
                case "ARCCSC":
                case "ARCSEC":
                case "ARCCOT":
                case "LOG":
                case "LN":
                case "LG":
                case "ABS":
                    return true;
                default:
                    return false;
            }
        }

        public override double ConvertToDouble()
        {
            double innerResult = this.innerExpression.ConvertToDouble();
            switch (this.functionName.ToUpperInvariant())
            {
                case "SIN":
                    return Math.Sin(innerResult);
                case "COS":
                    return Math.Cos(innerResult);
                case "TAN":
                    return Math.Tan(innerResult);
                case "CSC":
                    return 1 / Math.Sin(innerResult);
                case "SEC":
                    return 1 / Math.Cos(innerResult);
                case "COT":
                    return 1 / Math.Tan(innerResult);
                case "ARCSIN":
                    return Math.Asin(innerResult);
                case "ARCCOS":
                    return Math.Acos(innerResult);
                case "ARCTAN":
                    return Math.Atan(innerResult);
                case "ARCCSC":
                    return 1 / Math.Asin(innerResult);
                case "ARCSEC":
                    return 1 / Math.Acos(innerResult);
                case "ARCCOT":
                    return 1 / Math.Atan(innerResult);
                case "LOG":
                    return Math.Log10(innerResult);
                case "LN":
                    return Math.Log(innerResult, Math.E);
                case "LG":
                    return Math.Log(innerResult, 2);
                case "ABS":
                    return Math.Abs(innerResult);
                default:
                    throw new ConversionException(String.Format("Unknown function type: '{0}'.", this.functionName));
            }
        }
    }
}
