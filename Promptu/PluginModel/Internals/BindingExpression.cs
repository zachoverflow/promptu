// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
