using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyErrorPanel : IErrorPanel
    {
        public event EventHandler ItemActivated;

        public string Caption
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }

        public ICheckBoxButton ErrorsButton
        {
            get { return new DummyRadioButton(); }
        }

        public ICheckBoxButton WarningsButton
        {
            get { return new DummyRadioButton(); }
        }

        public ICheckBoxButton MessagesButton
        {
            get { return new DummyRadioButton(); }
        }

        public int PrimarySelectedIndex
        {
            get { return -1; }
        }

        public bool SomethingIsSelected
        {
            get { return false; }
        }

        public void SetMessages(FeedbackCollection messages)
        {
        }
    }
}
