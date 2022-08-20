using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class ImageSourceWithPadding
    {
        private ImageSource imageSource;
        private Thickness padding;

        public ImageSourceWithPadding(ImageSource imageSource, Thickness padding)
        {
#if !NO_WPF
            if (imageSource == null)
            {
                throw new ArgumentNullException("imageSource");
            }
#endif

            this.imageSource = imageSource;
            this.padding = padding;
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
        }

        public Thickness Padding
        {
            get { return this.padding; }
        }
    }
}
