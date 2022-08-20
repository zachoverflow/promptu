using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.PTK
{
    internal class TextInputWidget : GenericWidget<ITextInput>
    {
        private ScaledQuantity? width;
        //private ScaledInt? height;

        public TextInputWidget(string id)
            : base(id, Globals.GuiManager.ToolkitHost.Factory.ConstructTextInput())
        {
        }

        public event EventHandler TextChanged;

        public event EventHandler EnabledChanged;

        public event EventHandler CueChanged;

        public string Text
        {
            get
            {
                return this.NativeInterface.Text;
            }

            set
            {
                this.NativeInterface.Text = value;
                this.OnTextChanged(EventArgs.Empty);
            }
        }

        public bool Enabled
        {
            get
            {
                return this.NativeInterface.Enabled;
            }

            set
            {
                if (this.Enabled != value)
                {
                    this.NativeInterface.Enabled = value;
                    this.OnEnabledChanged(EventArgs.Empty);
                }
            }
        }

        public string Cue
        {
            get
            {
                return this.NativeInterface.Cue;
            }

            set
            {
                this.NativeInterface.Cue = value;
                this.OnCueChanged(EventArgs.Empty);
            }
        }

        public bool Multiline
        {
            get 
            {
                return this.NativeInterface.Multiline; 
            }

            set
            {
                if (this.Multiline != value)
                {
                    this.NativeInterface.Multiline = value;
                }
            }
        }

        public ScaledQuantity? Width
        {
            get 
            { 
                return this.width; 
            }

            set
            {
                this.width = value;
                if (value != null)
                {
                    //this.NativeInterface.AutoSize = false;
                    this.NativeInterface.PhysicalWidth = (int)Globals.GuiManager.ToolkitHost.ConvertToPhysicalQuantity(value.Value);
                }
                else
                {
                    this.NativeInterface.PhysicalWidth = null;
                }
                //else
                //{
                //    //this.NativeInterface.AutoSize = true;
                //}
            }
        }

        public bool CueIsDisplayed
        {
            get { return this.NativeInterface.CueDisplayed; }
        }

        public int SelectionStart
        {
            get { return this.NativeInterface.SelectionStart; }
            set { this.NativeInterface.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return this.NativeInterface.SelectionLength; }
            set { this.NativeInterface.SelectionLength = value; }
        }

        public void Select(int start, int length)
        {
            this.NativeInterface.Select(start, length);
        }

        public void SelectAll()
        {
            this.NativeInterface.SelectAll();
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnEnabledChanged(EventArgs e)
        {
            EventHandler handler = this.EnabledChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCueChanged(EventArgs e)
        {
            EventHandler handler = this.CueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
