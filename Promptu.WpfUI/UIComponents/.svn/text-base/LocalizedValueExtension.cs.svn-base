using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class LocalizedValueExtension : MarkupExtension
    {
        public LocalizedValueExtension(string key)
        {
            this.Key = key;
        }

        [ConstructorArgument("key")]
        public string Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Localization.UIResources.ResourceManager.GetString(this.Key);
        }
    }
}
