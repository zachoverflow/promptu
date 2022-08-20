﻿//-----------------------------------------------------------------------
// <copyright file="OptionalSubsitution.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;
    using System.Extensions;

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

        public override string ConvertToString(ExecutionData data)
        {
            if (data.Arguments.Length == 0 || (this.ArgumentNumber != null && data.Arguments.Length < this.ArgumentNumber))
            {
                if (this.defaultValue == null)
                {
                    return String.Empty;
                }
                else
                {
                    return this.defaultValue.ConvertToString(data);
                }
            }

            if (this.SingularSubstitution)
            {
                if (this.ArgumentNumber != null)
                {
                    if (data.Arguments.Length < this.ArgumentNumber)
                    {
                        if (this.defaultValue == null)
                        {
                            return String.Empty;
                        }
                        else
                        {
                            return this.defaultValue.ConvertToString(data);
                        }
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
                        if (this.defaultValue == null)
                        {
                            return String.Empty;
                        }
                        else
                        {
                            return this.defaultValue.ConvertToString(data);
                        }
                    }

                    return data.Arguments.ConcatenateAllBetween(this.ArgumentNumber.Value - 1, this.LastArgumentNumber.Value - 1, " ");
                }
            }
        }
    }
}