// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
