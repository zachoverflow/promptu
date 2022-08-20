//-----------------------------------------------------------------------
// <copyright file="BindingExpression.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal class BindingExpression
    {
        private List<BindingNode> chain;

        public BindingExpression(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            string[] split = path.Split('.');

            this.chain = new List<BindingNode>(split.Length);

            foreach (string property in split)
            {
                if (property.Length <= 0)
                {
                    throw new ArgumentException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Empty property name in binding expression '{0}'.",
                        path));
                }

                this.chain.Add(new BindingNode(property));
            }
        }

        public int Count
        {
            get { return this.chain.Count; }
        }

        public BindingNode this[int index]
        {
            get { return this.chain[index]; }
        }

        public object GetValue(object context)
        {
            object currentContext = context;

            for (int i = 0; i < this.chain.Count; i++)
            {
                currentContext = this.chain[i].GetValue(currentContext);
            }

            return currentContext;
        }

        public void SetValue(object context, object value)
        {
            object currentContext = context;

            for (int i = 0; i < this.chain.Count; i++)
            {
                if (i < this.chain.Count - 1)
                {
                    currentContext = this.chain[i].GetValue(currentContext);
                }
                else
                {
                    this.chain[i].SetValue(currentContext, value);
                }
            }
        }
    }
}
