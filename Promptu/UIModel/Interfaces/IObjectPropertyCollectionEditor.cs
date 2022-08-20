using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    interface IObjectPropertyCollectionEditor
    {
        OptionsGroupCollection Properties { get; set; }
    }
}
