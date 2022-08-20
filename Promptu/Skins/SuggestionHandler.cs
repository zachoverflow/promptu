using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.SkinApi;
using System.Windows.Forms;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.Skins
{
    internal abstract class SuggestionHandler
    {
        public void HandleKeyPress(KeyPressedEventArgs e)
        {
            this.HandleKeyPressCore(e);
        }

        public void DetachFromCurrentPrompt()
        {
            this.DetachFromCurrentPromptCore();
        }

        public void AttachToCurrentPrompt()
        {
            this.AttachToCurrentPromptCore();
        }

        //public void HandlePromptWM_ActivatedReceived(MessageEventArgs e)
        //{
        //    this.HandlePromptWM_ActivatedReceivedCore(e);
        //}

        //public void HandlePromptTextualInputMouseWheel(MouseEventArgs e)
        //{
        //    this.HandlePromptTextualInputMouseWheelCore(e);
        //}

        //public void HandlePromptTextualInputMouseDown(MouseEventArgs e)
        //{
        //    this.HandlePromptTextualInputMouseDownCore(e);
        //}

        public void HandleSuggestionAffectingChange()
        {
            this.HandleSuggestionAffectingChangeCore();
        }

        public void NotifyPromptOpened(bool fullReset)
        {
            this.NotifyPromptOpenedCore(fullReset);
        }

        protected abstract void HandleKeyPressCore(KeyPressedEventArgs e);

        //protected abstract void HandlePromptWM_ActivatedReceivedCore(MessageEventArgs e);

        protected virtual void NotifyPromptOpenedCore(bool fullReset)
        {
        }

        //protected virtual void HandlePromptTextualInputMouseWheelCore(MouseEventArgs e)
        //{
        //}

        //protected virtual void HandlePromptTextualInputMouseDownCore(MouseEventArgs e)
        //{
        //}

        protected virtual void DetachFromCurrentPromptCore()
        {
        }

        protected virtual void AttachToCurrentPromptCore()
        {
        }

        protected virtual void HandleSuggestionAffectingChangeCore()
        {
        }
    }
}
