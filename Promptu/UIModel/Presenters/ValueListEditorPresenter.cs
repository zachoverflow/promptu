using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ValueListEditorPresenter
    {
        private readonly IValueListEditor editor;
        private readonly ValueListContentsPanelPresenter valueListContentsPanel;

        public ValueListEditorPresenter(ValueList valueList)
            : this(
            InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructValueListEditor(),
            valueList)
        {
        }

        public ValueListEditorPresenter(IValueListEditor editor, ValueList valueList)
        {
            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }

            this.editor = editor;

            this.valueListContentsPanel = new ValueListContentsPanelPresenter(this.editor.ValueListContentsPanel);

            this.editor.OkButton.Text = Localization.UIResources.OkButtonText;
            this.editor.CancelButton.Text = Localization.UIResources.CancelButtonText;
            this.editor.NameLabelText = Localization.UIResources.ValueListEditorNameLabelText;
            this.editor.NamespaceInterpretation.Text = Localization.UIResources.ValueListEditorNamespaceText;
            this.editor.TranslateValues.Text = Localization.UIResources.ValueListEditorTranslationText;
            this.editor.MainInstructions = Localization.UIResources.ValueListEditorMainInstructions;
            this.editor.Text = Localization.UIResources.ValueListEditorText;

            this.editor.Name.Cue = Localization.UIResources.ValueListEditorNameCue;
            this.editor.Name.TextChanged += this.HandleNameTextChanged;
            this.editor.TranslateValues.CheckedChanged += this.HandleTranslateCheckBoxCheckedChanged;
            this.editor.TranslateValues.ToolTipText = Localization.UIResources.ValueListTranslationHelp;

            this.editor.NamespaceInterpretation.ToolTipText = Localization.UIResources.ValueListNamespaceHelp;

            this.editor.Name.Text = valueList.Name;
            this.editor.TranslateValues.Checked = valueList.UseItemTranslations;
            this.editor.NamespaceInterpretation.Checked = valueList.UseNamespaceInterpretation;

            this.valueListContentsPanel.AssignItems(valueList);

            this.editor.OkButton.Click += this.HandleOkButtonClick;

            this.UpdateEnableChangeableUIElements();

            this.editor.Name.Select();
        }

        public IValueListEditor NativeInterface
        {
            get { return this.editor; }
        }

        public UIDialogResult ShowDialog()
        {
            return this.editor.ShowModal();
        }

        private void HandleNameTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnableChangeableUIElements();
        }

        private void HandleTranslateCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            this.valueListContentsPanel.ShowTranslationColumn = this.editor.TranslateValues.Checked;
        }

        public ValueList AssembleValueList()
        {
            ValueList list = this.valueListContentsPanel.AssembleItems();
            list.Name = this.editor.Name.Text;
            list.UseNamespaceInterpretation = this.editor.NamespaceInterpretation.Checked;
            list.UseItemTranslations = this.editor.TranslateValues.Checked;
            return list;
        }

        private void HandleOkButtonClick(object sender, EventArgs e)
        {
            string proposedName = this.editor.Name.Text;
            if (proposedName.Trim().Length <= 0)
            {
                UIMessageBox.Show(
                    Localization.MessageFormats.ValueListNameCannotBeEmptyWhitespace,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);

                return;
            }

            this.editor.CloseWithOk();
        }

        private void UpdateEnableChangeableUIElements()
        {
            this.editor.OkButton.Enabled = !this.editor.Name.CueDisplayed && this.editor.Name.Text.Length > 0;
        }
    }
}
