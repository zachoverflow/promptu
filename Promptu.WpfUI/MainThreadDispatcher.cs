using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows;
using System.Windows.Threading;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class MainThreadDispatcher : IThreadingInvoke
    {
        private Dispatcher dispatcher;

        public MainThreadDispatcher()
        {
            //this.dispatcher = App.Current.Dispatcher;
        }

        public Dispatcher Dispatcher
        {
            get { return this.dispatcher; }
            set { this.dispatcher = value; }
        }

        public void BeginInvoke(Delegate method, object[] args)
        {
            this.dispatcher.BeginInvoke(method, args);
        }

        public object Invoke(Delegate method, object[] args)
        {
            return this.dispatcher.Invoke(method, args);
        }

        public bool InvokeRequired
        {
            get
            {
                return !this.dispatcher.CheckAccess();
            }
        }
    }
}
