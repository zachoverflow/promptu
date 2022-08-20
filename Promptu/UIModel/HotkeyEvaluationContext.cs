using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UIModel.Presenters;

namespace ZachJohnson.Promptu.UIModel
{
    internal class HotkeyEvaluationContext
    {
        private GlobalHotkey evaulationHotkey;
        private HotkeyControlPresenter hotkeyPresenter;
        private bool validHotkey;
        private bool isEnabled = true;

        public HotkeyEvaluationContext(HotkeyControlPresenter hotkeyPresenter)
        {
            if (hotkeyPresenter == null)
            {
                throw new ArgumentNullException("hotkeyPresenter");
            }

            this.hotkeyPresenter = hotkeyPresenter;
            this.hotkeyPresenter.HotkeyChanged += this.EvaluateHotkey;
            this.hotkeyPresenter.NativeInterface.OverrideHotkey.CheckedChanged += this.EvaluateHotkey;
        }

        public event EventHandler EvaluationFinished;

        public bool ValidHotkey
        {
            get { return this.validHotkey; }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.isEnabled = value; }
        }

        public GlobalHotkey EvaluationHotkey
        {
            get { return this.evaulationHotkey; }
            set 
            {
                using (DdMonitor.Lock(this))
                {
                    this.evaulationHotkey = value;
                }
            }
        }

        public void EvaluateHotkey()
        {
            bool failed = false;
            this.EvaluateHotkey(ref failed);
        }

        private void EvaluateHotkey(object sender, HotkeyChangedEventArgs e)
        {
            bool failed = e.Failed;
            this.EvaluateHotkey(ref failed);
            e.Failed = failed;
        }

        private void EvaluateHotkey(object sender, EventArgs e)
        {
            this.EvaluateHotkey();
        }

        private void EvaluateHotkey(ref bool failed)
        {
            if (!this.isEnabled)
            {
                failed = false;
                return;
            }

            using (DdMonitor.Lock(this))
            {
                GlobalHotkey hotkeyToUse = this.evaulationHotkey;
                bool isTempHotkey = false;

                if (hotkeyToUse == null)
                {
                    isTempHotkey = true;
                    hotkeyToUse = new GlobalHotkey(this.hotkeyPresenter.HotkeyModifierKeys, this.hotkeyPresenter.HotkeyKey.AssociatedKey, this.hotkeyPresenter.OverideHotkey);
                }
                else
                {
                    this.hotkeyPresenter.ImpartTo(hotkeyToUse);
                }

                try
                {
                    hotkeyToUse.Register();
                }
                catch (HotkeyException)
                {
                    if (!this.hotkeyPresenter.RollingBack)
                    {
                        failed = true;
                    }
                }
                finally
                {
                    if (isTempHotkey)
                    {
                        hotkeyToUse.Unregister();
                    }
                }

                this.hotkeyPresenter.NativeInterface.HotkeyStateText = failed
                    ? Localization.UIResources.HotkeyStateUnavailable
                    : Localization.UIResources.HotkeyStateAvailable;
                this.hotkeyPresenter.NativeInterface.HotkeyState = failed ? UIModel.HotkeyState.Taken : HotkeyState.Available;
                this.validHotkey = !failed;

                this.OnEvaluationFinished(EventArgs.Empty);
            }
            //this.UpdateCommandButtons();
        }

        protected virtual void OnEvaluationFinished(EventArgs e)
        {
            EventHandler handler = this.EvaluationFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
