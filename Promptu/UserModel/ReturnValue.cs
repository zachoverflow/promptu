using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal enum ReturnValue
    {
        // if values go beyond one digit, FullStringId methods will break
        String = 1,
        StringArray = 2,
        ValueList = 4,
    }
}
