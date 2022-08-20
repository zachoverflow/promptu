using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class OptionalSubsitution : ArgumentSubstitution
    {
        private Expression defaultValue;

        public OptionalSubsitution(int? argumentNumber, int? lastArgumentNumber, bool singularSubstitution, Expression defaultValue)
            : base(argumentNumber, lastArgumentNumber, singularSubstitution)
        {
            this.defaultValue = defaultValue;
        }

        public Expression DefaultValue
        {
            get { return this.defaultValue; }
        }
    }
}
