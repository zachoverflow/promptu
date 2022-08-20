using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows.Media;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ZachJohnson.Promptu.PluginModel.Internals;
using ZachJohnson.Promptu.PluginModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class PluginImageConverter : MarkupExtension, IValueConverter
    {
        private object defaultImage;

        public PluginImageConverter()
        {
        }

        public PluginImageConverter(object defaultImage)
        {
            this.defaultImage = defaultImage;
        }

        public object DefaultImage
        {
            get { return this.defaultImage; }
            set { this.defaultImage = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PromptuPlugin plugin = (PromptuPlugin)value;

            object cachedImage = plugin.CachedImage;

            if (cachedImage != null)
            {
                if (cachedImage is ImageSource)
                {
                    return cachedImage;
                }
            }

            FileSystemDirectory folder = plugin.Folder;
            FileSystemFile xamlIconFile = folder + "icon.xaml";

            if (xamlIconFile.Exists)
            {
                try
                {
                    using (FileStream stream = new FileStream(xamlIconFile, FileMode.Open, FileAccess.Read))
                    {
                        object xamlObject = XamlReader.Load(stream);

                        ResourceDictionary resourceDictionary = xamlObject as ResourceDictionary;

                        object iconObject = null;

                        if (resourceDictionary != null)
                        {
                            if (resourceDictionary.Contains("icon"))
                            {
                                iconObject = resourceDictionary["icon"];
                            }
                        }
                        else
                        {
                            iconObject = xamlObject;
                        }

                        if (iconObject != null)
                        {
                            ImageSource imageSource = iconObject as ImageSource;

                            if (imageSource != null)
                            {
                                return imageSource;
                            }

                            DrawingBrush brush = iconObject as DrawingBrush;

                            if (brush != null)
                            {
                                DrawingImage drawingImage = new DrawingImage(brush.Drawing);
                                drawingImage.Freeze();

                                plugin.CachedImage = drawingImage;
                                return drawingImage;
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                }
                catch (XamlParseException ex)
                {
                    ErrorConsole.WriteLineFormat(plugin.Id, "Error loading icon.xaml.  Message: {0}", ex.Message);
                }
            }
            else
            {
                FileSystemFile pngIconFile = folder + "icon.png";
                if (pngIconFile.Exists)
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(pngIconFile));
                    bitmapImage.Freeze();
                    plugin.CachedImage = bitmapImage;
                    return bitmapImage;
                }
            }

            return this.defaultImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
