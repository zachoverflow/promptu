using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal class AssemblyReferenceDiffDiffMaker : DiffDiffMaker<AssemblyReferenceDiffDiff, AssemblyReferenceDiff, AssemblyReference, AssemblyReferenceCollection>
    {
        public AssemblyReferenceDiffDiffMaker()
        {
        }

        protected override AssemblyReferenceDiffDiff CreateDiffDiff(AssemblyReferenceDiff priorityDiff, AssemblyReferenceDiff secondaryDiff)
        {
            return new AssemblyReferenceDiffDiff(priorityDiff, secondaryDiff);
        }
    }
}
