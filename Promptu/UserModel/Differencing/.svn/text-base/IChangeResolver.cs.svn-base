using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal interface IChangeResolver<TDiffDiff>
    {
        void ResolveToRevised(TDiffDiff diffDiff, DiffVersion version);

        bool GetWhetherImplicitlyResolved(Id id);
    }
}
