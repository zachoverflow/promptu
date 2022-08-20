using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Calculator.AbstractSyntaxTree
{
    internal class Constant : Expression
    {
        private string constantName;

        public Constant(string constantName)
        {
            if (constantName == null)
            {
                throw new ArgumentNullException("constantName");
            }

            this.constantName = constantName;
        }

        public static bool IsValidConstantName(string constantName)
        {
            switch (constantName.ToUpperInvariant())
            {
                case "PI":
                case "E":
                    return true;
                default:
                    return false;
            }
        }

        public override double ConvertToDouble()
        {
            switch (this.constantName.ToUpperInvariant())
            {
                case "PI":
                    return Math.PI;
                case "E":
                    return Math.E;
                default:
                    throw new ConversionException(String.Format("'{0}' is not a valid constant name.", this.constantName));
            }
        }
    }
}
