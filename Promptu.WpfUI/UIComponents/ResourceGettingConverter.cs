using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ResourceGettingConverter : MarkupExtension
    {
        private string key;

        public ResourceGettingConverter(string key)
        {
            this.key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Localization.UIResources.ResourceManager.GetString(this.key);
        }
    }
}
