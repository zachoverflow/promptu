using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Extensions;

namespace ZachJohnson.Promptu
{
    internal class BackgroundWorkQueue
    {
        private Queue<ParameterlessVoid> work = new Queue<ParameterlessVoid>();
        private Thread workThread;
        private bool stop;
        private ManualResetEvent pauseEvent = new ManualResetEvent(false);
        private ManualResetEvent pausedEvent = new ManualResetEvent(true);

        public BackgroundWorkQueue()
        {
        }

        public bool IsWorking
        {
            get 
            {
                return (this.workThread != null && this.workThread.IsAlive);
            }
        }

        public void WaitUntilPaused()
        {
            this.pausedEvent.WaitOne();
        }

        public void Pause()
        {
            this.pauseEvent.Reset();
        }

        public void Unpause()
        {
            this.pauseEvent.Set();
            this.pausedEvent.Reset();
        }

        public void AddWork(ParameterlessVoid workItem)
        {
            this.work.Enqueue(workItem);
            if (!this.IsWorking)
            {
                this.stop = false;
                this.workThread = new Thread(this.DoWork);
                this.workThread.IsBackground = true;
                this.workThread.SetApartmentState(ApartmentState.STA);
                this.workThread.Start();
                this.pausedEvent.Reset();
            }
        }

        private void DoWork()
        {
            while (work.Count > 0)
            {
                this.pausedEvent.Set();
                this.pauseEvent.WaitOne();
                this.pausedEvent.Reset();

                work.Dequeue()();

                if (stop)
                {
                    this.stop = false;
                    break;
                }
            }

            this.pausedEvent.Set();
        }

        public void Stop()
        {
            this.stop = true;
        }
    }
}
