//-----------------------------------------------------------------------
// <copyright file="Expression.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
