using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;

namespace ZachJohnson.Promptu.WpfUI.Dummy
{
    internal class DummyPrompt : IPrompt
    {
        public DummyPrompt()
        {
            this.PromptContextMenu = new UIModel.UIContextMenu();
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event System.Windows.Forms.MouseEventHandler MouseWheel;

        public event System.Windows.Forms.MouseEventHandler MouseDown;

        public event System.Windows.Forms.MouseEventHandler MouseUp;

        public event EventHandler LocationChanged;

        public System.Drawing.Point Location
        {
            get;
            set;
        }

        public System.Drawing.Size Size
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public int SelectionStart
        {
            get;
            set;
        }

        public int SelectionLength
        {
            get;
            set;
        }

        public bool ContainsFocus
        {
            get { return false; }
        }

        public void Show()
        {
        }

        public void Hide()
        {
        }

        public void EnsureCreated()
        {
        }

        public void Activate()
        {
        }

        public void FocusOnTextInput()
        {
        }

        public bool IsCreated
        {
            get { return true; }
        }

        public object UIObject
        {
            get { return this; }
        }

        public PluginModel.OptionPage Options
        {
            get { return null; }
        }

        public PluginModel.ObjectPropertyCollection SavingProperties
        {
            get { return null; }
        }

        public UIModel.UIContextMenu PromptContextMenu
        {
            get;
            set;
        }
    }
}
