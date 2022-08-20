using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface ITabControl
    {
        //INativeUICollection<ITabPage> Tabs { get; }

        int SelectedTabIndex { get; set; }

        void Insert(int index, ITabPage tabPage);

        void Remove(ITabPage tabPage);

        event EventHandler SelectedTabChanged;
    }
}
