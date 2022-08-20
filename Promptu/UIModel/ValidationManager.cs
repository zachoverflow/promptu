//-----------------------------------------------------------------------
// <copyright file="ValidationManager.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.UIModel
{
    using System;
    using System.Threading;

    internal class ValidationManager : IDisposable
    {
        //private const int SecondsToValidationAfterLastChange = 2;
        private CustomTimer validationTimer;
        private Thread validationThread;

        public ValidationManager(int numberOfSecondsToValidationAfterChange)
        {
            this.validationTimer = new CustomTimer(numberOfSecondsToValidationAfterChange * 1000);
            this.validationTimer.Halted += this.HaltValidationThread;
            this.validationTimer.TimerReset += this.HaltValidationThread;
            this.validationTimer.Elapsed += this.BeginValidationAsync;
        }

        //~ValidationManager()
        //{
        //    this.Dispose(false);
        //}

        public event EventHandler TimeToValidate;

        public void NotifyChangeHappened()
        {
            this.validationTimer.Reset();
        }

        public void Dispose()
        {
            this.Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.validationThread != null)
            {
                this.validationThread.Abort();
            }

            this.validationTimer.Dispose();
        }

        protected virtual void OnTimeToValidate(EventArgs e)
        {
            EventHandler handler = this.TimeToValidate;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RaiseTimeToValidate()
        {
            this.OnTimeToValidate(EventArgs.Empty);
        }

        private void BeginValidationAsync(object sender, EventArgs e)
        {
            this.BeginValidationAsync();
        }

        private void BeginValidationAsync()
        {
            this.validationThread = new Thread(this.RaiseTimeToValidate);
            this.validationThread.IsBackground = true;
            this.validationThread.Start();
        }

        private void HaltValidationThread(object sender, EventArgs e)
        {
            if (this.validationThread != null)
            {
                this.validationThread.Abort();
            }
        }
    }
}
