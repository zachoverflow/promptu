using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ValueListDiffDiffMaker : DiffDiffMaker<ValueListDiffDiff, ValueListDiff, ValueList, ValueListCollection>
    {
        public ValueListDiffDiffMaker()
        {
        }

        protected override ValueListDiffDiff CreateDiffDiff(ValueListDiff priorityDiff, ValueListDiff secondaryDiff)
        {
            return new ValueListDiffDiff(priorityDiff, secondaryDiff);
        }
    }
}
