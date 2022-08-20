using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZachJohnson.Promptu.UIModel
{
    internal class MenuItemViewModel
    {
        private readonly PropertyBinding<string> text = new PropertyBinding<string>();
        private readonly PropertyBinding<string> tooltipText = new PropertyBinding<string>();
        private readonly PropertyBinding<Image> image = new PropertyBinding<Image>();
        private readonly PropertyBinding<bool> enabled = new PropertyBinding<bool>();
        private readonly PropertyBinding<bool> visible = new PropertyBinding<bool>();

        public MenuItemViewModel()
        {
        }

        public MenuItemViewModel(string text, string tooltipText, Image image)
        {
            this.text.Value = text;
            this.tooltipText.Value = tooltipText;
            this.image.Value = image;
        }

        public PropertyBinding<string> Text
        {
            get { return this.Text; }
        }

        public PropertyBinding<string> ToolTipText
        {
            get { return this.tooltipText; }
        }

        public PropertyBinding<Image> Image
        {
            get { return this.image; }
        }

        public PropertyBinding<bool> Enabled
        {
            get { return this.enabled; }
        }

        public PropertyBinding<bool> Visible
        {
            get { return this.visible; }
        }
    }
}
