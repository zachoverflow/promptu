using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummySplashScreen : ISplashScreen
    {
        public void Show(int? suggestedDuration, System.Threading.ManualResetEvent closedSignal)
        {
            closedSignal.Set();
        }

        public void Close()
        {
        }
    }
}
