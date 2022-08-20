using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class FunctionDiffMaker : DiffMaker<Function, FunctionDiff, FunctionCollection>
    {
        public FunctionDiffMaker()
        {
        }

        protected override FunctionDiff CreateDiff(Function baseItem, Function latestItem)
        {
            return new FunctionDiff(baseItem, latestItem);
        }

        //protected override Function GetCorrespondingItemFrom(IFunction item, FunctionCollection collection, FunctionIdentifierChangeCollection identifierChanges, out int? index)
        //{
        //    return collection.TryGet(item.Name, item.NumberOfParameters, identifierChanges, out index);
        //}
    }
}
