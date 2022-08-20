using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class Computer
    {
        public Computer()
        {
        }

        public void Standby()
        {
            this.StandbyCore();
        }

        public void Hibernate()
        {
            this.HibernateCore();
        }

        public void Lock()
        {
            this.LockCore();
        }

        public void LogOff()
        {
            this.LogOffCore();
        }

        public void ShutDown()
        {
            this.ShutDownCore();
        }

        public void Reboot()
        {
            this.RebootCore();
        }

        public void StartScreensaver()
        {
            this.StartScreensaverCore();
        }

        protected abstract void StandbyCore();

        protected abstract void HibernateCore();

        protected abstract void LockCore();

        protected abstract void LogOffCore();

        protected abstract void ShutDownCore();

        protected abstract void RebootCore();

        protected abstract void StartScreensaverCore();
    }
}
