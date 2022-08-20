using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ValueListDiffMaker : DiffMaker<ValueList, ValueListDiff, ValueListCollection>
    {
        public ValueListDiffMaker()
        {
        }

        protected override ValueListDiff CreateDiff(ValueList baseItem, ValueList latestItem)
        {
            return new ValueListDiff(baseItem, latestItem);
        }
    }
}
