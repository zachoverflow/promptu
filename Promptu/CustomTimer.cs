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

namespace ZachJohnson.Promptu
{
    using System;
    using System.Timers;

    internal class CustomTimer : IDisposable
    {
        private Timer timer;
        private bool timerWasGoingAtHalt;
        private bool disposed;
        private bool frozen;

        public CustomTimer(double interval)
        {
            this.timer = new Timer(interval);
            this.timer.AutoReset = false;
            this.timer.Elapsed += this.NotifyElapsed;
            this.timer.Stop();
        }

        ~CustomTimer()
        {
            this.Dispose(false);
        }

        public event EventHandler Elapsed;

        public event EventHandler TimerReset;

        public event EventHandler Halted;

        public bool Frozen
        {
            get { return this.frozen; }
            set { this.frozen = value; }
        }

        public bool AutoReset
        {
            get { return this.timer.AutoReset; }
            set { this.timer.AutoReset = value; }
        }

        public void Reset()
        {
            if (this.frozen)
            {
                return;
            }

            this.ValidateNotDisposed();
            this.timer.Stop();
            this.timer.Start();
            this.OnTimerReset(EventArgs.Empty);
        }

        public void Start()
        {
            if (this.frozen)
            {
                return;
            }

            this.ValidateNotDisposed();
            this.timer.Start();
        }

        public void ResetIfGoing()
        {
            if (this.frozen)
            {
                return;
            }

            this.ValidateNotDisposed();
            if (this.timer.Enabled == true)
            {
                this.Reset();
            }
        }

        public void ResetIfWasGoingAtLastHalt()
        {
            if (this.frozen)
            {
                return;
            }

            this.ValidateNotDisposed();
            if (this.timerWasGoingAtHalt)
            {
                this.timer.Stop();
                this.timer.Start();
            }
        }

        public void Halt()
        {
            if (this.frozen)
            {
                return;
            }

            this.ValidateNotDisposed();
            if (this.timer.Enabled)
            {
                this.timerWasGoingAtHalt = true;
            }
            else
            {
                this.timerWasGoingAtHalt = false;
            }

            this.timer.Stop();

            this.OnHalted(EventArgs.Empty);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.disposed = true;
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }
        }

        protected virtual void OnElapsed(EventArgs e)
        {
            EventHandler handler = this.Elapsed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTimerReset(EventArgs e)
        {
            EventHandler handler = this.TimerReset;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnHalted(EventArgs e)
        {
            EventHandler handler = this.Halted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void ValidateNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("CustomTimer");
            }
        }

        private void NotifyElapsed(object sender, ElapsedEventArgs e)
        {
            this.OnElapsed(EventArgs.Empty);
        }
    }
}
