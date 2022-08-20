using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IChooseNewOrExistingProfile
    {
        string MainInstructions { set; }

        ICommandLink NewProfile { get; }

        ICommandLink ExistingProfile { get; }

        ICommandLink ExitPromptu { get; }
    }
}
