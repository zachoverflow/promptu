using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Markup;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    public class StockIcon : MarkupExtension
    {
        private StockIconIdentifier _identifier;
        private StockIconOptions _flags;
        private BitmapSource _bitmapSource = null;

        public StockIcon()
        {
        }

        public StockIcon(StockIconIdentifier identifier)
            : this(identifier, 0)
        {
        }

        public StockIcon(StockIconIdentifier identifier, StockIconOptions flags)
        {
            Identifier = identifier;
            Selected = (flags & StockIconOptions.Selected) == StockIconOptions.Selected;
            LinkOverlay = (flags & StockIconOptions.LinkOverlay) == StockIconOptions.LinkOverlay;
            ShellSize = (flags & StockIconOptions.ShellSize) == StockIconOptions.ShellSize;
            Small = (flags & StockIconOptions.Small) == StockIconOptions.Small;
        }

        protected void Check()
        {
            if (_bitmapSource != null)
            {
                throw new InvalidOperationException("The BitmapSource has already been created");
            }
        }

        public bool Selected
        {

            get
            {
                return (_flags & StockIconOptions.Selected) == StockIconOptions.Selected;
            }

            set
            {
                Check();

                if (value)
                {
                    _flags |= StockIconOptions.Selected;
                }
                else
                {
                    _flags &= ~StockIconOptions.Selected;
                }
            }
        }

        public bool LinkOverlay
        {
            get 
            { 
                return (_flags & StockIconOptions.LinkOverlay) == StockIconOptions.LinkOverlay; 
            }

            set
            {
                Check();

                if (value)
                {
                    _flags |= StockIconOptions.LinkOverlay;
                }
                else
                {
                    _flags &= ~StockIconOptions.LinkOverlay;
                }
            }
        }

        public bool ShellSize
        {
            get
            {
                return (_flags & StockIconOptions.ShellSize) == StockIconOptions.ShellSize;
            }

            set
            {
                Check();

                if (value)
                {
                    _flags |= StockIconOptions.ShellSize;
                }
                else
                {
                    _flags &= ~StockIconOptions.ShellSize;
                }
            }
        }

        public bool Small
        {
            get 
            { 
                return (_flags & StockIconOptions.Small) == StockIconOptions.Small; 
            }

            set
            {
                Check();

                if (value)
                {
                    _flags |= StockIconOptions.Small;
                }
                else
                {
                    _flags &= ~StockIconOptions.Small;
                }
            }
        }

        public StockIconIdentifier Identifier
        {
            get
            {
                return _identifier;
            }

            set
            {
                Check();
                _identifier = value;
            }
        }

        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            return Bitmap;
        }

        public BitmapSource Bitmap
        {
            get
            {
                if (_bitmapSource == null)
                {
                    _bitmapSource = GetBitmapSource(_identifier, _flags);
                }

                return _bitmapSource;
            }
        }

        protected internal static BitmapSource GetBitmapSource(StockIconIdentifier identifier, StockIconOptions flags)
        {
            BitmapSource bitmapSource = (BitmapSource)NativeMethods.MakeImage(identifier, StockIconOptions.Handle | flags);
            bitmapSource.Freeze();
            return bitmapSource;

        }
    }
}
