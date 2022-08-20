//-----------------------------------------------------------------------
// <copyright file="PromptuPluginEntryPoint.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel
{
    using System;
    using System.Collections.Generic;
    using ZachJohnson.Promptu.Configuration;

    public abstract class PromptuPluginEntryPoint
    {
        private PromptuHooks hooks = new PromptuHooks();
        private IList<NamedOptionPage> options = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructBindingList<NamedOptionPage>();
        private ObjectPropertyCollection savingProperties = new ObjectPropertyCollection();
        private bool isEnabled;
        private SettingsBase savingSettings;
        private PromptuPluginFactory factory = new PromptuPluginFactory();

        public PromptuPluginEntryPoint()
        {
        }

        internal event EventHandler SavingSettingsChanged;

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            internal set { this.isEnabled = value; }
        }

        public IList<NamedOptionPage> Options
        {
            get { return this.options; }
        }

        public ObjectPropertyCollection SavingProperties
        {
            get { return this.savingProperties; }
        }

        public SettingsBase SavingSettings
        {
            get
            {
                return this.savingSettings;
            }

            set
            {
                SettingsBase previousSettings = this.savingSettings;
                if (previousSettings != null)
                {
                    previousSettings.SettingChanged -= this.HandleSavingSettingChanged;
                }

                this.savingSettings = value;

                if (value != null)
                {
                    value.SettingChanged += this.HandleSavingSettingChanged;
                }
            }
        }

        protected internal PromptuPluginFactory Factory
        {
            get { return this.factory; }
        }

        protected internal PromptuHooks Hooks
        {
            get { return this.hooks; }
        }

        protected internal virtual void OnLoad()
        {
        }

        protected internal virtual void OnUnload()
        {
        }

        protected void ShowOptionsDialog()
        {
            InternalGlobals.PluginConfigWindowManager.ShowConfigFor(this);
        }

        private void HandleSavingSettingChanged(object sender, EventArgs e)
        {
            this.OnSavingSettingsChanged(EventArgs.Empty);
        }

        private void OnSavingSettingsChanged(EventArgs e)
        {
            EventHandler handler = this.SavingSettingsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
