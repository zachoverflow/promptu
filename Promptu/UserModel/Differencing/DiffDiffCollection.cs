using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class DiffDiffCollection<TDiffDiff, TDiff, TItem> : List<TDiffDiff>
        where TItem : IDiffable
        where TDiffDiff : DiffDiff<TDiff, TItem, TDiffDiff>
        where TDiff : Diff<TItem, TDiff>
    {
    }
}
