using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal enum NewUserStep
    {
        ChooseBetweenNewAndExisting = 0,
        ProfileBasics,
        ProfileAdvanced,
        ProfileFinish,
        ExistingProfile
    }
}
