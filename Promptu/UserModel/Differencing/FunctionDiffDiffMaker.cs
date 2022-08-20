using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class FunctionDiffDiffMaker : DiffDiffMaker<FunctionDiffDiff, FunctionDiff, Function, FunctionCollection>
    {
        public FunctionDiffDiffMaker()
        {
        }

        protected override FunctionDiffDiff CreateDiffDiff(FunctionDiff priorityDiff, FunctionDiff secondaryDiff)
        {
            return new FunctionDiffDiff(priorityDiff, secondaryDiff);
        }
    }
}
