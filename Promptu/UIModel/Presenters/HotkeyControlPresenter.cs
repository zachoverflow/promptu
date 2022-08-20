using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Windows.Forms;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class HotkeyControlPresenter : PresenterBase<IHotkeyControl>
    {
        private bool updating;
        //private bool settingUnderlyingHotkeyKeyValue;
        private bool rollingBack;
        //private bool settingOverride;
        private ValidHotkeyKey lastHotkeyKey;
        private HotkeyModifierKeys previousModifierKeys;
        private ValidHotkeyKey previousKey;

        //public HotkeyControlPresenter()
        //    : this(Globals.GuiManager.ToolkitHost.Factory.ConstructHotkeyControl())
        //{
        //}

        public HotkeyControlPresenter(IHotkeyControl nativeInterface)
            : base(nativeInterface)
        {
            if (nativeInterface == null)
            {
                throw new ArgumentNullException("nativeInterface");
            }

            this.NativeInterface.Alt.Text = Localization.UIResources.AltKeyText;
            this.NativeInterface.Ctrl.Text = Localization.UIResources.CtrlKeyText;
            this.NativeInterface.Shift.Text = Localization.UIResources.ShiftKeyText;
            this.NativeInterface.Win.Text = Localization.UIResources.WinKeyText;
            //this.NativeInterface.Title = Localization.UIResources.DefaultHotkeyTitle;
            this.NativeInterface.OverrideHotkey.Text = Localization.UIResources.OverrideHotkeyText;
            this.NativeInterface.HotkeyEnabled.Text = Localization.UIResources.HotkeyEnabledText;

            this.NativeInterface.Key.AddRange(ValidHotkeyKeys.ValidKeys);

            this.NativeInterface.HotkeyEnabled.CheckedChanged += this.HandleHotkeyEnabledChanged;
            this.NativeInterface.Alt.CheckedChanged += this.HandleModifierSelectionChanged;
            this.NativeInterface.Ctrl.CheckedChanged += this.HandleModifierSelectionChanged;
            this.NativeInterface.Shift.CheckedChanged += this.HandleModifierSelectionChanged;
            this.NativeInterface.Win.CheckedChanged += this.HandleModifierSelectionChanged;
            this.NativeInterface.OverrideHotkey.CheckedChanged += this.HandleOverrideHotkeyChanged;
            this.NativeInterface.Key.SelectedIndexChanged += this.HandleSelectedKeyChanged;
        }

        public event EventHandler<HotkeyChangedEventArgs> HotkeyChanged;

        public event EventHandler HotkeyEnabledChanged;

        public bool RollingBack
        {
            get { return this.rollingBack; }
        }

        public bool OverideHotkey
        {
            get 
            {
                return this.NativeInterface.OverrideHotkey.Checked; 
            }

            set 
            {
                if (this.OverideHotkey != value)
                {
                   // this.settingOverride = true;
                    this.NativeInterface.OverrideHotkey.Checked = value;
                   // this.settingOverride = false;
                }
            }
        }

        public bool HotkeyEnabledVisible
        {
            get { return this.NativeInterface.HotkeyEnabled.Visible; }
            set { this.NativeInterface.HotkeyEnabled.Visible = value; }
        }

        public bool HotkeyEnabled
        {
            get
            {
                return this.NativeInterface.HotkeyEnabled.Checked;
            }

            set
            {
                this.NativeInterface.HotkeyEnabled.Checked = value;
                this.UpdateEnabledStates();
            }
        }

        //public string TitleText
        //{
        //    get { return this.NativeInterface.Title; }
        //    set { this.NativeInterface.Title = value; }
        //}

        //public bool ShowTitle
        //{
        //    get { return this.NativeInterface.ShowTitle; }
        //    set { this.NativeInterface.ShowTitle = value; }
        //}

        public ValidHotkeyKey HotkeyKey
        {
            get { return (ValidHotkeyKey)this.NativeInterface.Key.SelectedValue; }
            set { this.NativeInterface.Key.SelectedValue = value; }
        }

        public Keys UnderlyingHotkeyKey
        {
            get
            {
                ValidHotkeyKey hotkeyKey = this.HotkeyKey;
                if (hotkeyKey != null)
                {
                    return hotkeyKey.AssociatedKey;
                }

                return Keys.None;
            }

            set
            {
                this.HotkeyKey = ValidHotkeyKeys.Map(value);
            }
        }

        public HotkeyModifierKeys HotkeyModifierKeys
        {
            get
            {
                HotkeyModifierKeys modifierKeys = (HotkeyModifierKeys)0;

                if (this.NativeInterface.Alt.Checked)
                {
                    modifierKeys |= HotkeyModifierKeys.Alt;
                }

                if (this.NativeInterface.Win.Checked)
                {
                    modifierKeys |= HotkeyModifierKeys.Win;
                }

                if (this.NativeInterface.Shift.Checked)
                {
                    modifierKeys |= HotkeyModifierKeys.Shift;
                }

                if (this.NativeInterface.Ctrl.Checked)
                {
                    modifierKeys |= HotkeyModifierKeys.Ctrl;
                }

                if (modifierKeys == (HotkeyModifierKeys)0)
                {
                    modifierKeys = HotkeyModifierKeys.Win;
                }

                return modifierKeys;
            }

            set
            {
                if (!this.NativeInterface.IsVisible)
                {
                    this.UpdatePreviousKeys();
                }

                this.updating = true;
                this.NativeInterface.Ctrl.Checked = (value & HotkeyModifierKeys.Ctrl) != 0;
                this.NativeInterface.Win.Checked = (value & HotkeyModifierKeys.Win) != 0;
                this.NativeInterface.Alt.Checked = (value & HotkeyModifierKeys.Alt) != 0;
                this.NativeInterface.Shift.Checked = (value & HotkeyModifierKeys.Shift) != 0;
                this.updating = false;
                this.NotifyHotkeyChanged();
            }
        }

        public void UpdateTo(GlobalHotkey hotkey)
        {
            this.OverideHotkey = hotkey.OverrideIfNecessary;
            this.NativeInterface.Key.SelectedValue = ValidHotkeyKeys.Map(hotkey.Key);
            this.HotkeyModifierKeys = hotkey.ModifierKeys;
            this.HotkeyEnabled = hotkey.Enabled;
        }

        public void ImpartTo(GlobalHotkey hotkey)
        {
            hotkey.SwitchTo(this.HotkeyModifierKeys, this.UnderlyingHotkeyKey, this.OverideHotkey);
        }

        public void RollbackToPevious()
        {
            if (!this.rollingBack)
            {
                this.rollingBack = true;
                this.HotkeyKey = this.previousKey;
                this.HotkeyModifierKeys = this.previousModifierKeys;
                this.rollingBack = false;
            }
        }

        protected virtual void OnHotkeyChanged(HotkeyChangedEventArgs e)
        {
            EventHandler<HotkeyChangedEventArgs> handler = this.HotkeyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnHotkeyEnabledChanged(EventArgs e)
        {
            EventHandler handler = this.HotkeyEnabledChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void UpdateEnabledStates()
        {
            this.NativeInterface.Win.Enabled = this.HotkeyEnabled;
            this.NativeInterface.Shift.Enabled = this.HotkeyEnabled;
            this.NativeInterface.Alt.Enabled = this.HotkeyEnabled;
            this.NativeInterface.Ctrl.Enabled = this.HotkeyEnabled;
            this.NativeInterface.Key.Enabled = this.HotkeyEnabled;
            this.NativeInterface.OverrideHotkey.Enabled = this.HotkeyEnabled;
        }

        private void HandleHotkeyEnabledChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledStates();
            this.OnHotkeyEnabledChanged(EventArgs.Empty);
        }

        private void HandleModifierSelectionChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            if (!this.NativeInterface.Alt.Checked &&
                !this.NativeInterface.Ctrl.Checked &&
                !this.NativeInterface.Win.Checked &&
                !this.NativeInterface.Shift.Checked)
            {
                ((ICheckBox)sender).Checked = true;
            }
            else
            {
                this.NotifyHotkeyChanged();
            }
        }

        private void NotifyHotkeyChanged()
        {
            try
            {
                if (this.HotkeyKey != null)
                {
                    HotkeyChangedEventArgs eventArgs = new HotkeyChangedEventArgs(this.HotkeyModifierKeys, this.HotkeyKey.AssociatedKey, this.OverideHotkey);
                    this.OnHotkeyChanged(eventArgs);

                    //if (!this.rollingBack && userInteractive && !eventArgs.Failed && this.HotkeyKey.AssociatedKey == Keys.R && this.HotkeyModifierKeys == HotkeyModifierKeys.Win && !this.OverideHotkey && !overrideChanged)
                    //{
                    //    UIMessageBox.Show(
                    //        Localization.Promptu.OverridingWinRWarning,
                    //        Localization.Promptu.AppName,
                    //        UIMessageBoxButtons.OK,
                    //        UIMessageBoxIcon.Warning,
                    //        UIMessageBoxResult.OK);
                    //}

                    if (!eventArgs.Failed)
                    {
                        this.UpdatePreviousKeys();
                    }
                }
            }
            catch (ArgumentException)
            {
                if (!this.rollingBack)
                {
                    this.RollbackToPevious();
                    UIMessageBox.Show(
                        Localization.Promptu.BadHotkeyMessage,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Error,
                        UIMessageBoxResult.OK);
                }
            }
        }

        

        private void UpdatePreviousKeys()
        {
            if (!this.rollingBack)
            {
                this.previousKey = this.HotkeyKey;
                this.previousModifierKeys = this.HotkeyModifierKeys;
            }
        }

        private void HandleSelectedKeyChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            if (this.lastHotkeyKey == null || this.HotkeyKey != this.lastHotkeyKey)
            {
                this.lastHotkeyKey = this.HotkeyKey;
                this.NotifyHotkeyChanged();
            }
        }

        private void HandleOverrideHotkeyChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            this.NotifyHotkeyChanged();
        }

        // ----------


        

        //public bool OverrideIfAlreadyInUse
        //{
        //    get 
        //    { 
        //        return this.overrideCheckBox.Checked; 
        //    }

        //    set 
        //    {
        //        if (this.OverrideIfAlreadyInUse != value)
        //        {
        //            this.settingOverride = true;
        //            this.overrideCheckBox.Checked = value;
        //            this.settingOverride = false;
        //        }
        //    }
        //}

        //public ValidHotkeyKey HotkeyKey
        //{
        //    get 
        //    { 
        //        return (ValidHotkeyKey)this.hotkeyKey.SelectedItem; 
        //    }

        //    set
        //    {
        //        if (value == null)
        //        {
        //            return;
        //        }

        //        this.UnderlyingHotkeyKeyValue = value.AssociatedKey;
        //    }
        //}

        //public Keys UnderlyingHotkeyKeyValue
        //{
        //    get 
        //    { 
        //        ValidHotkeyKey hotkeyKey = this.HotkeyKey;
        //        if (hotkeyKey != null)
        //        {
        //            return hotkeyKey.AssociatedKey;
        //        }

        //        return Keys.None;
        //    }

        //    set
        //    {
        //        if (value == Keys.None)
        //        {
        //            return;
        //        }

        //        this.UpdatePreviousKeys();

        //        foreach (object item in this.hotkeyKey.Items)
        //        {
        //            ValidHotkeyKey hotkeyKey = (ValidHotkeyKey)item;
        //            if (hotkeyKey.AssociatedKey == value)
        //            {
        //                try
        //                {
        //                    //this.lastHotkeyKey = hotkeyKey;
        //                    this.settingUnderlyingHotkeyKeyValue = true;
        //                    this.hotkeyKey.SelectedItem = item;
        //                }
        //                finally
        //                {
        //                    this.settingUnderlyingHotkeyKeyValue = false;
        //                }

        //                return;
        //            }
        //        }

        //        throw new ArgumentException("No hotkey matched the provided hotkey.");
        //    }
        //}

        

        //public override Size GetPreferredSize(Size proposedSize)
        //{
        //    //Size oldSize = this.overrideCheckBox.Size;
        //    //this.overrideCheckBox.MinimumSize = new Size(1,1);
        //    this.overrideCheckBox.Dock = DockStyle.None;
        //    Size preferredSize = base.GetPreferredSize(proposedSize);
        //    this.overrideCheckBox.Dock = DockStyle.Fill;
        //    return preferredSize;
        //}



        

        //private void HotkeyModifierKeyCheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.updating)
        //    {
        //        return;
        //    }

        //    CheckBox button = sender as CheckBox;

        //    if (button != null)
        //    {
        //        if (!button.Checked && !this.ctrlKey.Checked && !this.winKey.Checked && !this.shiftKey.Checked && !this.altKey.Checked)
        //        {
        //            button.Checked = true;
        //        }
        //        else
        //        {
        //            //this.NotifyHotkeyChanged(true, false);
        //            this.OnModifierSelectionChanged();
        //        }
        //    }
        //}

        private void HotkeyKeySelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            if (this.lastHotkeyKey == null || this.HotkeyKey != this.lastHotkeyKey)
            {
                this.lastHotkeyKey = this.HotkeyKey;
                this.NotifyHotkeyChanged();
            }
        }

        private void HandleOverrideIfAlreadyInUseCheckedChanged(object sender, EventArgs e)
        {
            if (this.updating)
            {
                return;
            }

            this.NotifyHotkeyChanged();
        }
    }
}
