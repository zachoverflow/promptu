using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal interface IHasNonIdIdentifier
    {
        string GetFormattedIdentifier();
    }
}
