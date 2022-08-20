using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    internal class UITabPage : UIComponent<UITabPage, ITabPage>
    {
        private string text;
        private object content;
        private ITabPage correspondingTabPage;

        public UITabPage(string id)
            : base(id)
        {
            this.correspondingTabPage = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructTabPage();
        }

        public string Text
        {
            get 
            {
                return this.text; 
            }

            set 
            {
                //this.ValidateNotInternal();
                this.TextInternal = value;
            }
        }

        internal string TextInternal
        {
            get 
            { 
                return this.text; 
            }

            set 
            {
                this.text = value;
                this.correspondingTabPage.Text = value;
            }
        }

        public object Content
        {
            get
            {
                return this.content;
            }

            set
            {
                //this.ValidateNotInternal();
                this.ContentInternal = value;
            }
        }

        internal object ContentInternal
        {
            get
            {
                return this.content;
            }

            set
            {
                this.content = value;
                this.correspondingTabPage.SetContent(value);
            }
        }

        internal override ITabPage NativeInterface
        {
            get { return this.correspondingTabPage; }
        }
    }
}
