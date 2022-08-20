//-----------------------------------------------------------------------
// <copyright file="StringLiteral.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;
    using System.Extensions;

    internal class StringLiteral : Expression
    {
        private string value;

        public StringLiteral(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }

        public override string ConvertToString(ExecutionData data)
        {
            return this.value.ReplaceIntrinsics();
        }
    }
}
