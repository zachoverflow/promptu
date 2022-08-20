using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class ValueListChangeFinder : ChangeFinder<
        ValueList,
        ValueListDiff,
        ValueListDiffMaker,
        ValueListDiffDiff,
        ValueListDiffDiffMaker,
        ValueListCollection>
    {
        public ValueListChangeFinder(ValueListCollection baseCollection,
            ValueListCollection priorityCollection,
            ValueListCollection secondaryCollection)
            : base(baseCollection,
            priorityCollection,
            secondaryCollection)
        {
        }
    }
}
