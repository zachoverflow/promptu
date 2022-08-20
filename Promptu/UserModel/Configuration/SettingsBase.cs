using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Configuration
{
    internal abstract class SettingsBase
    {
        private bool raiseSettingChanged = true;

        public SettingsBase()
        {
        }

        public event EventHandler SettingChanged;

        protected bool RaiseSettingChanged
        {
            get { return this.raiseSettingChanged; }
            set { this.raiseSettingChanged = value; }
        }

        protected virtual void OnSettingChanged(EventArgs e)
        {
            if (!this.raiseSettingChanged)
            {
                return;
            }

            EventHandler handler = this.SettingChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaiseSettingChangedEvent(object sender, EventArgs e)
        {
            this.OnSettingChanged(EventArgs.Empty);
        }
    }
}
