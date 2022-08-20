using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using System.Threading;
using System.ComponentModel;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class FunctionsCommandsCompositeMediator
    {
        private List<FunctionsCommandsComposite> clients = new List<FunctionsCommandsComposite>();
        private ListCollection lists;
        private bool allowRegenerate = true;
        private object regenerationSyncToken = new object();
        private object regenerationNoAccessToken = new object();
        private object changingWaitingRegenerations = new object();
        private int waitingRegenerations = 0;
        private ManualResetEvent regenerationFinished = new ManualResetEvent(true);
        private ManualResetEvent allRegenerationsFinished = new ManualResetEvent(true);
        private bool ignoreNewRegenerations;

        public FunctionsCommandsCompositeMediator(ListCollection lists)
        {
            if (lists == null)
            {
                throw new ArgumentNullException("lists");
            }

            this.lists = lists;

            foreach (List list in this.lists)
            {
                this.AttachToList(list);
            }

            this.lists.ItemAdded += this.ListAdded;
            this.lists.ItemRemoved += this.ListRemoved;
        }

        public event EventHandler ClientsRegenerated;

        public object RegenerationSyncToken
        {
            get { return this.regenerationSyncToken; }
        }

        public bool IgnoreNewRegenerations
        {
            get { return this.ignoreNewRegenerations; }
            set { this.ignoreNewRegenerations = value; }
        }

        public object RegenerationNoAccessToken
        {
            get { return this.regenerationNoAccessToken; }
        }

        public ManualResetEvent RegenerationFinished
        {
            get { return this.regenerationFinished; }
        }

        public bool AllowRegenerate
        {
            get { return this.allowRegenerate; }
            set { this.allowRegenerate = value; }
        }

        public void AddClient(FunctionsCommandsComposite client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (client.Mediator == this)
            {
                return;
            }

            client.Mediator = this;
            this.clients.Add(client);
        }

        public bool RemoveClient(FunctionsCommandsComposite client)
        {
            return this.clients.Remove(client);
        }

        private void ListAdded(object sender, ItemAndIndexEventArgs<List> e)
        {
            this.RegenerateAll();
            this.AttachToList(e.Item);
        }

        private void ListRemoved(object sender, ItemAndIndexEventArgs<List> e)
        {
            this.RegenerateAll();
            e.Item.Commands.ItemAdded -= this.SomethingChanged;
            e.Item.Commands.ItemRemoved -= this.SomethingChanged;
            e.Item.Functions.ItemAdded -= this.SomethingChanged;
            e.Item.Functions.ItemRemoved -= this.SomethingChanged;
        }

        private void AttachToList(List list)
        {
            list.Commands.ItemAdded += this.SomethingChanged;
            list.Commands.ItemRemoved += this.SomethingChanged;
            list.Functions.ItemAdded += this.SomethingChanged;
            list.Functions.ItemRemoved += this.SomethingChanged;
        }
        
        private void SomethingChanged(object sender, EventArgs e)
        {
            this.RegenerateAll();
        }

        public void RegenerateAllInvokeOnMainThread(ParameterlessVoid actionOnceDone)
        {
            this.RegenerateAll();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate { this.DoAfterRegenerationsFinished(actionOnceDone); };
            worker.RunWorkerAsync();
            //this.allRegenerationsFinished.WaitOne();
        }

        private void DoAfterRegenerationsFinished(ParameterlessVoid action)
        {
            this.allRegenerationsFinished.WaitOne();
            InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.BeginInvoke(action, null);
        }

        public void RegenerateAll()
        {
            if (this.ignoreNewRegenerations)
            {
                return;
            }

            using (DdMonitor.Lock(this.changingWaitingRegenerations))
            {
                if (this.waitingRegenerations < 0)
                {
                    this.waitingRegenerations = 0;
                }

                this.waitingRegenerations++;

                if (this.waitingRegenerations == 1)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += this.RegenerateAllCore;
                    worker.RunWorkerAsync();
                    this.allRegenerationsFinished.Reset();
                    //Thread thread = new Thread(this.RegenerateAllCore);
                    //thread.IsBackground = true;
                    //thread.Start();
                }
            }
        }

        private void RegenerateAllCore(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                using (DdMonitor.Lock(this.changingWaitingRegenerations))
                {
                    if (this.waitingRegenerations <= 0)
                    {
                        this.allRegenerationsFinished.Set();
                        break;
                    }
                }

                using (DdMonitor.Lock(this.regenerationSyncToken))
                {
                    using (DdMonitor.Lock(this.changingWaitingRegenerations))
                    {
                        this.waitingRegenerations--;
                    }

                    if (this.allowRegenerate && this.waitingRegenerations <= 0)
                    {
                        this.regenerationFinished.Reset();
                        //lock (this.regenerationNoAccessToken)
                        //{
                        foreach (FunctionsCommandsComposite client in this.clients)
                        {
                            client.Regenerate();
                        }
                        //}

                        //Thread.Sleep(5000);
                        this.regenerationFinished.Set();

                        if (this.waitingRegenerations <= 0)
                        {
                            this.OnClientsRegenerated(EventArgs.Empty);
                        }
                    }
                }
            }
        }

        protected virtual void OnClientsRegenerated(EventArgs e)
        {
            EventHandler handler = this.ClientsRegenerated;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
