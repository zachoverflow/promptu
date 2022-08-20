using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class AssemblyReferenceDiffMaker : DiffMaker<
        AssemblyReference,
        AssemblyReferenceDiff, 
        AssemblyReferenceCollection>
    {
        public AssemblyReferenceDiffMaker()
        {
        }

        protected override AssemblyReferenceDiff CreateDiff(AssemblyReference baseItem, AssemblyReference latestItem)
        {
            return new AssemblyReferenceDiff(baseItem, latestItem);
        }

        //protected override AssemblyReference GetCorrespondingItemFrom(
        //    IAssemblyReference item, 
        //    AssemblyReferenceCollection collection,
        //    AssemblyReferenceIdentifierChangeCollection identifierChanges,
        //    out int? index)
        //{
        //    return collection.TryGet(item.Name, identifierChanges, out index);
        //}
    }
}
