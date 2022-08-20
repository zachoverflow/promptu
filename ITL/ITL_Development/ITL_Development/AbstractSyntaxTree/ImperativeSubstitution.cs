using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    internal class ImperativeSubstitution : ArgumentSubstitution
    {
        public ImperativeSubstitution(int? argumentNumber, int? lastArgumentNumber, bool singularSubstitution)
            : base(argumentNumber, lastArgumentNumber, singularSubstitution)
        {
        }
    }
}
