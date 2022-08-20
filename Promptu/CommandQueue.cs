using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace ZachJohnson.Promptu
{
    internal class CommandQueue
    {
        private Queue<ParameterlessVoid> commands = new Queue<ParameterlessVoid>();
        private ManualResetEvent goEvent = new ManualResetEvent(true);

        public CommandQueue()
        {
        }

        public void AddCommand(ParameterlessVoid action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate { action(); };
            worker.RunWorkerAsync();

            //this.commands.Enqueue(action);
            //this.StartIfNotStarted();
        }

        //private void StartIfNotStarted()
        //{
        //    goEvent.Set();

        //    if (!this.started)
        //    {
        //        Thread commandThread = new Thread(this.DoCommands);
        //        commandThread.Name = "Command execution thread";
        //        commandThread.IsBackground = true;
        //        commandThread.SetApartmentState(ApartmentState.STA);
        //        commandThread.Start();
        //        this.started = true;
        //    }
        //}

        //private void DoCommands()
        //{
        //    while (true)
        //    {
        //        while (true)
        //        {
        //            ParameterlessVoid command;

        //            using (DdMonitor.Lock(this.commands))
        //            {
        //                if (commands.Count <= 0)
        //                {
        //                    break;
        //                }

        //                command = commands.Dequeue();
        //            }

        //            command();
        //        }

        //        goEvent.Reset();
        //        goEvent.WaitOne();
        //    }
        //}

        //private void ThrottleNextCommand()
        //{
        //    ParameterlessVoid command;

        //    using (DdMonitor.Lock(this.commands))
        //    {
        //        if (commands.Count <= 0)
        //        {
        //            return;
        //        }

        //        command = commands.Dequeue();
        //    }

        //    command();
        //}
    }
}
