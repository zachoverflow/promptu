using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZachJohnson.Promptu.UIModel
{
    internal class NotifyIconViewModel
    {
        private readonly PropertyBinding<string> tooltipText = new PropertyBinding<string>();
        private readonly PropertyBinding<Icon> icon = new PropertyBinding<Icon>();

        public NotifyIconViewModel()
        {
        }

        public PropertyBinding<string> ToolTipText
        {
            get { return this.tooltipText; }
        }

        public PropertyBinding<Icon> Icon
        {
            get { return this.icon; }
        }

        public void OpenPrompt()
        {
            //TODO open prompt
        }
    }
}
