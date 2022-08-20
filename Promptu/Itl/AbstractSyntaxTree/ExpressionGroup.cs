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

namespace ZachJohnson.Promptu.Itl.AbstractSyntaxTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class ExpressionGroup : Expression
    {
        private List<Expression> expressions;

        public ExpressionGroup(List<Expression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException("expressions");
            }

            this.expressions = expressions;
        }

        public List<Expression> Expressions
        {
            get { return this.expressions; }
        }

        public override string ConvertToString(ExecutionData data)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Expression item in this.Expressions)
            {
                builder.Append(item.ConvertToString(data));
            }

            return builder.ToString();
        }
    }
}
