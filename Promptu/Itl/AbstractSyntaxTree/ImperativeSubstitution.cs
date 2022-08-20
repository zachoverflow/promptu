//-----------------------------------------------------------------------
// <copyright file="ImperativeSubstitution.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System.Extensions;

    internal class ImperativeSubstitution : ArgumentSubstitution
    {
        public ImperativeSubstitution(int? argumentNumber, int? lastArgumentNumber, bool singularSubstitution)
            : base(argumentNumber, lastArgumentNumber, singularSubstitution)
        {
        }

        public override string ConvertToString(ExecutionData data)
        {
            if (data.Arguments.Length == 0)
            {
                throw new NotEnoughArgumentsConversionException("Not enough arguments were supplied.");
            }

            if (this.SingularSubstitution)
            {
                if (this.ArgumentNumber != null)
                {
                    if (data.Arguments.Length < this.ArgumentNumber)
                    {
                        throw new NotEnoughArgumentsConversionException("Not enough arguments were supplied.");
                    }
                    else
                    {
                        return data.Arguments[this.ArgumentNumber.Value - 1];
                    }
                }
                else
                {
                    return data.Arguments.ConcatenateAll(" ");
                }
            }
            else
            {
                if (this.LastArgumentNumber == null)
                {
                    return data.Arguments.ConcatenateAll(this.ArgumentNumber.Value - 1, " ");
                }
                else
                {
                    if (this.LastArgumentNumber >= data.Arguments.Length)
                    {
                        throw new NotEnoughArgumentsConversionException("Not enough arguments were supplied.");
                    }

                    return data.Arguments.ConcatenateAllBetween(this.ArgumentNumber.Value - 1, this.LastArgumentNumber.Value - 1, " ");
                }
            }
        }
    }
}
