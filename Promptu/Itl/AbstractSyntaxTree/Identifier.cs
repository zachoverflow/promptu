//-----------------------------------------------------------------------
// <copyright file="Identifier.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;

    internal class Identifier
    {
        private string name;

        public Identifier(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
