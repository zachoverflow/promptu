using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class FunctionChangeFinder : ChangeFinder<
        Function,
        FunctionDiff,
        FunctionDiffMaker,
        FunctionDiffDiff,
        FunctionDiffDiffMaker,
        FunctionCollection>
    {
        public FunctionChangeFinder(FunctionCollection baseCollection,
            //FunctionIdentifierChangeCollection baseCollectionIdentifierChanges,
            FunctionCollection priorityCollection,
            //FunctionIdentifierChangeCollection priorityCollectionIdentifierChanges,
            FunctionCollection secondaryCollection)
            //FunctionIdentifierChangeCollection secondaryCollectionIdentifierChanges)
            : base(baseCollection,
            //baseCollectionIdentifierChanges,
            priorityCollection,
            //priorityCollectionIdentifierChanges,
            secondaryCollection)
            //secondaryCollectionIdentifierChanges)
        {
        }
    }
}
