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
    using System.Collections.Generic;

    internal abstract class Expression
    {
        public static void GetFunctionCalls(Expression component, List<FunctionCall> functionCalls)
        {
            ExpressionGroup grouping;
            FunctionCall functionalSubstitution;

            if ((grouping = component as ExpressionGroup) != null)
            {
                foreach (Expression childComponent in grouping.Expressions)
                {
                    GetFunctionCalls(childComponent, functionCalls);
                }
            }
            else if ((functionalSubstitution = component as FunctionCall) != null)
            {
                functionCalls.Add(functionalSubstitution);
                foreach (Expression childComponent in functionalSubstitution.Parameters)
                {
                    GetFunctionCalls(childComponent, functionCalls);
                }
            }
        }

        public abstract string ConvertToString(ExecutionData data);
    }
}
