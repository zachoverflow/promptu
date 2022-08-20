using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class ErrorPanelPresenter
    {
        private readonly IErrorPanel uiObject;
        private FeedbackCollection currentMessages;

        public ErrorPanelPresenter(IErrorPanel uiObject)
        {
            if (uiObject == null)
            {
                throw new ArgumentNullException("uiObject");
            }

            this.uiObject = uiObject;
            this.currentMessages = new FeedbackCollection();

            this.uiObject.Caption = Localization.UIResources.ErrorListCaption;

            this.uiObject.ErrorsButton.Text = String.Format(CultureInfo.CurrentCulture, Localization.UIResources.ErrorPluralFormat, 0);
            this.uiObject.ErrorsButton.Click += this.ShowErrorClick;

            this.uiObject.WarningsButton.Text = String.Format(CultureInfo.CurrentCulture, Localization.UIResources.WarningPluralFormat, 0);
            this.uiObject.WarningsButton.Click += this.ShowWarningClick;

            this.uiObject.MessagesButton.Text = String.Format(CultureInfo.CurrentCulture, Localization.UIResources.MessagePluralFormat, 0);
            this.uiObject.MessagesButton.Click += this.ShowMessageClick;

            this.uiObject.ItemActivated += this.HandleItemActivated;
        }

        internal event EventHandler<MessageSelectedEventArgs> MessageSelected;

        public void SetMessages(FeedbackCollection messages, bool combine)
        {
            if (combine)
            {
                this.currentMessages.AddRange(messages);
            }
            else
            {
                this.currentMessages = messages;
            }

            this.RouteCurrentMessagesToMessageList();
        }

        internal void RouteCurrentMessagesToMessageList()
        {
            this.SortMessages();
            FeedbackCollection feedbackToDisplay = new FeedbackCollection();
            int errors = 0;
            int warnings = 0;
            int messages = 0;
            foreach (FeedbackMessage message in this.currentMessages)
            {
                switch (message.MessageType)
                {
                    case FeedbackType.Error:
                        errors++;
                        if (this.uiObject.ErrorsButton.Checked)
                        {
                            feedbackToDisplay.Add(message);
                        }

                        break;
                    case FeedbackType.Warning:
                        warnings++;
                        if (this.uiObject.WarningsButton.Checked)
                        {
                            feedbackToDisplay.Add(message);
                        }

                        break;
                    case FeedbackType.Message:
                        messages++;
                        if (this.uiObject.MessagesButton.Checked)
                        {
                            feedbackToDisplay.Add(message);
                        }

                        break;
                    default:
                        break;
                }
            }

            this.uiObject.SetMessages(feedbackToDisplay);
            string errorForm;
            if (errors == 1)
            {
                errorForm = Localization.UIResources.ErrorSingularFormat;
            }
            else
            {
                errorForm = Localization.UIResources.ErrorPluralFormat;
            }

            string warningForm;
            if (warnings == 1)
            {
                warningForm = Localization.UIResources.WarningSingularFormat;
            }
            else
            {
                warningForm = Localization.UIResources.WarningPluralFormat;
            }

            string messagesForm;
            if (messages == 1)
            {
                messagesForm = Localization.UIResources.MessageSingularFormat;
            }
            else
            {
                messagesForm = Localization.UIResources.MessagePluralFormat;
            }

            this.uiObject.ErrorsButton.Text = String.Format(CultureInfo.CurrentCulture, errorForm, errors.ToString(CultureInfo.CurrentCulture));
            this.uiObject.WarningsButton.Text = String.Format(CultureInfo.CurrentCulture, warningForm, warnings.ToString(CultureInfo.CurrentCulture));
            this.uiObject.MessagesButton.Text = String.Format(CultureInfo.CurrentCulture, messagesForm, messages.ToString(CultureInfo.CurrentCulture));
        }

        private void SortMessages()
        {
            FeedbackCollection final = new FeedbackCollection();
            FeedbackCollection messages = new FeedbackCollection();
            FeedbackCollection warnings = new FeedbackCollection();
            FeedbackCollection errors = new FeedbackCollection();

            foreach (FeedbackMessage message in this.currentMessages)
            {
                switch (message.MessageType)
                {
                    case FeedbackType.Message:
                        messages.Add(message);
                        break;
                    case FeedbackType.Warning:
                        warnings.Add(message);
                        break;
                    case FeedbackType.Error:
                        errors.Add(message);
                        break;
                    default:
                        break;
                }
            }

            final.AddRange(messages);
            final.AddRange(warnings);
            final.AddRange(errors);
            this.currentMessages = final;
        }

        private void HandleItemActivated(object sender, EventArgs e)
        {
            if (this.uiObject.SomethingIsSelected)
            {
                this.OnMessageSelected(new MessageSelectedEventArgs(this.currentMessages[this.uiObject.PrimarySelectedIndex]));
            }
        }

        private void ShowWarningClick(object sender, EventArgs e)
        {
            this.uiObject.WarningsButton.Checked = !this.uiObject.WarningsButton.Checked;
            this.RouteCurrentMessagesToMessageList();
        }

        private void ShowMessageClick(object sender, EventArgs e)
        {
            this.uiObject.MessagesButton.Checked = !this.uiObject.MessagesButton.Checked;
            this.RouteCurrentMessagesToMessageList();
        }

        private void ShowErrorClick(object sender, EventArgs e)
        {
            this.uiObject.ErrorsButton.Checked = !this.uiObject.ErrorsButton.Checked;
            this.RouteCurrentMessagesToMessageList();
        }

        protected virtual void OnMessageSelected(MessageSelectedEventArgs e)
        {
            EventHandler<MessageSelectedEventArgs> handler = this.MessageSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
