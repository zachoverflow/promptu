using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface IPromptuOptionsPanel
    {
        string MainInstructions { set; }

        IObjectPropertyCollectionEditor Editor { get; }
    }
}
