using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ValueListSelectorWindowPresenter : DialogPresenterBase<IValueListSelectorWindow>
    {
        private ValueListSetupPanelPresenter valueListSetupPanel;

        public ValueListSelectorWindowPresenter(ParameterlessVoid updateAllCallback)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructValueListSelectorWindow(),
            updateAllCallback)
        {
        }

        public ValueListSelectorWindowPresenter(
            IValueListSelectorWindow nativeInterface, 
            ParameterlessVoid updateAllCallback)
            : base(nativeInterface)
        {
            this.NativeInterface.Text = Localization.UIResources.ValueListSelectorText;
            this.NativeInterface.OkButton.Text = Localization.UIResources.OkButtonText;
            this.NativeInterface.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.NativeInterface.MainInstructions = Localization.UIResources.ValueListSelectorMessage;

            this.valueListSetupPanel = new ValueListSetupPanelPresenter(
                this.NativeInterface.ValueListSetupPanel,
                updateAllCallback);

            this.valueListSetupPanel.SelectedItemChanged += this.UpdateOkButtonEnabled;

            this.valueListSetupPanel.MultiSelect = false;
            //this.valueListSetupPanel.ClearSelectedIndices();
            this.valueListSetupPanel.CollectionPresenter.Select();
            this.valueListSetupPanel.RaiseItemDoubleClick = true;
            this.valueListSetupPanel.EditItemOnDoubleClick = false;
            this.valueListSetupPanel.ItemDoubleClick += this.HandleItemDoubleClick;

            this.UpdateOkButtonEnabled();
        }

        private void HandleItemDoubleClick(object sender, EventArgs e)
        {
            this.NativeInterface.CloseWithOk();
        }

        public string SelectedValueListName
        {
            get
            {
                string name = null;

                if (this.valueListSetupPanel.CollectionPresenter.SelectedIndexes.Count > 0)
                {
                    name = this.valueListSetupPanel.GetSelectedItem().Name;
                }

                return name;
            }

            set
            {
                if (value == null || InternalGlobals.CurrentProfile.SelectedList == null)
                {
                    this.valueListSetupPanel.ClearSelectedIndices();
                }
                else
                {
                    this.valueListSetupPanel.SelectIndex(this.valueListSetupPanel.IndexOf(value));
                }
            }
        }

        private void UpdateOkButtonEnabled(object sender, EventArgs e)
        {
            this.UpdateOkButtonEnabled();
        }

        private void UpdateOkButtonEnabled()
        {
            this.NativeInterface.OkButton.Enabled = this.valueListSetupPanel.CollectionPresenter.SelectedIndexes.Count > 0;
        }
    }
}
