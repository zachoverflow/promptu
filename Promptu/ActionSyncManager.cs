using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal class ActionSyncManager
    {
        private object actionSyncToken;
        private object waitingActionsSyncToken;
        private int waitingActions;

        public ActionSyncManager()
        {
            this.actionSyncToken = new object();
            this.waitingActionsSyncToken = new object();
        }

        public object ActionSyncToken
        {
            get { return this.actionSyncToken; }
        }

        public int WaitingActions
        {
            get 
            { 
                return this.waitingActions;
            }

            set
            {
                using (DdMonitor.Lock(this.waitingActionsSyncToken))
                {
                    this.waitingActions = value;
                }
            }
        }
    }
}
