using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Calculator.AbstractSyntaxTree
{
    internal abstract class Expression
    {
        public abstract double ConvertToDouble();
    }
}
