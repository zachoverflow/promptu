using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZachJohnson.Promptu.UIModel;
using System.Threading.Extensions;

namespace ZachJohnson.Promptu
{
    internal class SyncSynchronizer
    {
        private int numberOfSyncsGoing;
        private int numberOfPausedSyncs;
        private int numberOfPauseRequests;
        private ManualResetEvent syncPauseEvent;
        private ManualResetEvent syncsPausedEvent;
        private ManualResetEvent allSyncsFinishedEvent;
        private bool cancelSyncs;

        public SyncSynchronizer()
        {
            this.syncsPausedEvent = new ManualResetEvent(false);
            this.syncPauseEvent = new ManualResetEvent(true);
            this.allSyncsFinishedEvent = new ManualResetEvent(true);
        }

        public void PauseSyncs()
        {
            this.numberOfPauseRequests++;
            this.syncPauseEvent.Reset();
            if (this.numberOfSyncsGoing == 0)
            {
                this.syncsPausedEvent.Set();
            }
        }

        public bool CancelSyncs
        {
            get 
            { 
                return this.cancelSyncs; 
            }

            set
            {
                if (this.numberOfSyncsGoing > 0)
                {
                    this.cancelSyncs = value;
                    //if (value)
                    //{
                    //    this.cancelSyncs = value;
                    //}
                }
            }
        }

        public void NotifySyncStarting()
        {
            this.numberOfSyncsGoing++;
            this.allSyncsFinishedEvent.Reset();
        }

        public void NotifySyncEnded()
        {
            if (this.numberOfSyncsGoing > 0)
            {
                this.numberOfSyncsGoing--;
                if (numberOfSyncsGoing == 0)
                {
                    this.cancelSyncs = false;
                    this.allSyncsFinishedEvent.Set();
                }
            }
        }

        public void NotifyEssentiallyPaused()
        {
            this.numberOfPausedSyncs++;
            if (this.numberOfPausedSyncs == this.numberOfSyncsGoing)
            {
                this.syncsPausedEvent.Set();
            }
        }

        public void UnNotifyEssentiallyPaused()
        {
            if (this.numberOfPausedSyncs > 0)
            {
                this.numberOfPausedSyncs--;
                this.syncsPausedEvent.Reset();
            }
        }

        public void CancelSyncsAndWait()
        {
            if (this.numberOfSyncsGoing > 0)
            {
                this.cancelSyncs = true;
                this.WaitUntilAllSyncsFinished();
            }
        }

        public void UnPauseSyncs()
        {
            if (this.numberOfPauseRequests > 0)
            {
                this.numberOfPauseRequests--;
                if (this.numberOfPauseRequests == 0)
                {
                    this.syncPauseEvent.Set();
                }
            }
        }

        public void WaitIfPauseSyncs()
        {
            this.numberOfPausedSyncs++;
            if (this.numberOfPausedSyncs == this.numberOfSyncsGoing)
            {
                this.syncsPausedEvent.Set();
            }

            this.syncPauseEvent.WaitOne();
            this.numberOfPausedSyncs--;
            this.syncsPausedEvent.Reset();
        }

        public void WaitUntilAllSyncsPaused()
        {
            if (this.numberOfSyncsGoing > 0)
            {
//#if DEBUG
                this.syncsPausedEvent.WaitOneCallDeadlock(10000);
                // TODO after testing change to this.syncsPausedEvent.WaitOne();
                //if (!this.syncsPausedEvent.WaitOne(10000, false))
                //{
                //    //try
                //    //{
                //    //    throw new ArgumentException();
                //    //}
                //    //catch (ArgumentException ex)
                //    //{
                //    //    ExceptionLogger.LogException(ex, "Promptu locked up.");
                //    //}

                //    ExceptionLogger.LogCurrentThreadStack("Promptu locked up");

                //    UIMessageBox.Show(
                //        "Deadlock detected.  Please contact support@PromptuLauncher.com to help us resolve this issue.",
                //        Localization.Promptu.AppName,
                //        UIMessageBoxButtons.OK,
                //        UIMessageBoxIcon.Error,
                //        UIMessageBoxResult.OK);
                //}
//#else
               // this.syncsPausedEvent.WaitOne();
//#endif
            }
        }

        public void WaitUntilAllSyncsFinished()
        {
            if (this.numberOfSyncsGoing > 0)
            {
                this.allSyncsFinishedEvent.WaitOneCallDeadlock(10000);
            }
        }
    }
}
