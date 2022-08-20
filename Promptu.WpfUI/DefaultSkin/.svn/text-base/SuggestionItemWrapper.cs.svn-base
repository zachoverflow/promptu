using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.SkinApi;
using System.Windows.Media;
using System.ComponentModel;

namespace ZachJohnson.Promptu.WpfUI.DefaultSkin
{
    internal class SuggestionItemWrapper : INotifyPropertyChanged
    {
        private SuggestionItem item;
        private IList<object> images;

        public SuggestionItemWrapper(SuggestionItem item, IList<object> images)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.item = item;
            this.images = images;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SuggestionItem Item
        {
            get { return this.item; }
        }

        public ImageSource ImageSource
        {
            get
            {
                int index = this.item.ImageIndex;
                if (this.images != null && index >= 0 && index < this.images.Count)
                {
                    ImageSource image = this.images[index] as ImageSource;
                    if (image != null)
                    {
                        return image;
                    }

                    //return this.images[index] as ImageSource;
                }

                int? fallbackIndex = this.item.FallbackImageIndex;
                if (fallbackIndex == null)
                {
                    return null;
                }

                index = fallbackIndex.Value;

                if (this.images != null && index >= 0 && index < this.images.Count)
                {
                    ImageSource image = this.images[index] as ImageSource;
                    if (image != null)
                    {
                        return image;
                    }

                    //return this.images[index] as ImageSource;
                }

                return null;
            }
        }

        public string Text
        {
            get { return this.item.Text; }
        }

        public override string ToString()
        {
            return this.Text;
        }

        public void Refresh()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs("ImageSource"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Text"));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
