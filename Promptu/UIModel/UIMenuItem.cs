using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.UIModel
{
    public class UIMenuItem : UIMenuItemBase
    {
        private object image;
        private string text;
        private string toolTipText;
        private bool enabled = true;
        private IMenuItem correspondingMenuItem;
        private UIMenuItemCollection subItems;
        private TextStyle textStyle;

        public UIMenuItem(string id)
            : this(id, null, null, null, null)
        {
        }

        public UIMenuItem(string id, string text)
            : this(id, text, null, null, null)
        {
        }

        public UIMenuItem(string id, string text, EventHandler clickEventHandler)
            : this(id, text, null, null, clickEventHandler)
        {
        }

        //public UIMenuItem(string id, string text, string toolTipText)
        //    : this(id, text, toolTipText, null)
        //{
        //}

        public UIMenuItem(string id, string text, string toolTipText, object image)
            : this(id, text, toolTipText, image, null)
        {
        }

        public UIMenuItem(string id, string text, object image, EventHandler clickEventHandler)
            : this(id, text, null, image, clickEventHandler)
        {
        }

        public UIMenuItem(string id, string text, string toolTipText, object image, EventHandler clickEventHandler)
            : base(id)
        {
            this.correspondingMenuItem = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructNewMenuItem();
            this.Text = text;
            this.ToolTipText = toolTipText;
            this.Image = image;
            this.Click = clickEventHandler;

            this.subItems = new UIMenuItemCollection(this.correspondingMenuItem.SubItems);

            this.correspondingMenuItem.Click += this.HandleClick;
        }

        public event EventHandler ImageChanged;

        public event EventHandler TextChanged;

        public event EventHandler ToolTipTextChanged;

        public event EventHandler EnabledChanged;

        public event EventHandler Click;

        internal override IGenericMenuItem NativeInterface
        {
            get { return this.correspondingMenuItem; }
        }

        public UIMenuItemCollection SubItems
        {
            get { return this.subItems; }
        }

        //public object Image
        //{
        //    get 
        //    { 
        //        return this.image; 
        //    }

        //    set
        //    {
        //        //this.ValidateNotInternal();
        //        this.ImageInternal = value;
        //    }
        //}

        //public TextStyle TextStyle
        //{
        //    get
        //    {
        //        return this.textStyle;
        //    }

        //    set
        //    {
        //        //this.ValidateNotInternal();
        //        this.TextStyleInternal = value;
        //    }
        //}

        //public string Text
        //{
        //    get
        //    {
        //        return this.text;
        //    }

        //    set
        //    {
        //        //this.ValidateNotInternal();
        //        this.TextInternal = value;
        //    }
        //}

        //public string ToolTipText
        //{
        //    get
        //    {
        //        return this.toolTipText;
        //    }

        //    set
        //    {
        //        //this.ValidateNotInternal();
        //        this.ToolTipTextInternal = value;
        //    }
        //}

        //public bool Enabled
        //{
        //    get
        //    {
        //        return this.enabled;
        //    }

        //    set
        //    {
        //        //this.ValidateNotInternal();
        //        this.EnabledInternal = value;
        //    }
        //}

        public object Image
        {
            get
            {
                return this.image;
            }

            set
            {
                if (this.image == value)
                {
                    return;
                }

                this.image = value;
                this.correspondingMenuItem.Image = value;
                this.OnImageChanged(EventArgs.Empty);
            }
        }

        public TextStyle TextStyle
        {
            get
            {
                return this.textStyle;
            }

            set
            {
                if (this.textStyle == value)
                {
                    return;
                }

                this.textStyle = value;
                this.correspondingMenuItem.TextStyle = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == value)
                {
                    return;
                }

                this.text = value;
                this.correspondingMenuItem.Text = value;
                this.OnTextChanged(EventArgs.Empty);
            }
        }

        public string ToolTipText
        {
            get
            {
                return this.toolTipText;
            }

            set
            {
                if (this.toolTipText == value)
                {
                    return;
                }

                this.toolTipText = value;
                this.correspondingMenuItem.ToolTipText = value;
                this.OnToolTipTextChanged(EventArgs.Empty);
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }

            set
            {
                if (this.enabled == value)
                {
                    return;
                }

                this.enabled = value;
                this.correspondingMenuItem.Enabled = value;
                this.OnEnabledChanged(EventArgs.Empty);
            }
        }

        protected override void SetAvailable(bool value)
        {
            this.correspondingMenuItem.Available = value;
        }

        private void OnImageChanged(EventArgs e)
        {
            EventHandler handler = this.ImageChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnTextChanged(EventArgs e)
        {
            EventHandler handler = this.TextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnToolTipTextChanged(EventArgs e)
        {
            EventHandler handler = this.ToolTipTextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnEnabledChanged(EventArgs e)
        {
            EventHandler handler = this.EnabledChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnClick(EventArgs e)
        {
            EventHandler handler = this.Click;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleClick(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
