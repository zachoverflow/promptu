using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class AssemblyReferenceChangeFinder : ChangeFinder<
        AssemblyReference,
        AssemblyReferenceDiff,
        AssemblyReferenceDiffMaker,
        AssemblyReferenceDiffDiff,
        AssemblyReferenceDiffDiffMaker,
        AssemblyReferenceCollection>
    {
        public AssemblyReferenceChangeFinder(AssemblyReferenceCollection baseCollection,
            //AssemblyReferenceIdentifierChangeCollection baseCollectionIdentifierChanges,
            AssemblyReferenceCollection priorityCollection,
            //AssemblyReferenceIdentifierChangeCollection priorityCollectionIdentifierChanges,
            AssemblyReferenceCollection secondaryCollection)
            //AssemblyReferenceIdentifierChangeCollection secondaryCollectionIdentifierChanges)
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
