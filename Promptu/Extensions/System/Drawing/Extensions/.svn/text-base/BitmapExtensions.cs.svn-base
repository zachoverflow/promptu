//-----------------------------------------------------------------------
// <copyright file="BitmapExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Drawing.Extensions
{
    using System;

    internal static class BitmapExtensions
    {
        public static Icon ToIcon(this Bitmap bitmap)
        {
            IntPtr handle = bitmap.GetHicon();
            return Icon.FromHandle(handle);
        }

        public static void FillWithTransparency(this Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, Color.Transparent);
                }
            }
        }

        public static Bitmap OverlayWithAlphaBlended(this Bitmap backgroundImage, Bitmap topImage, Point startingAt)
        {
            if (topImage == null)
            {
                throw new ArgumentNullException("topImage");
            }
            else if (backgroundImage == null)
            {
                throw new ArgumentNullException("backgroundImage", "An AlphaBlendedOverlay was attempted on a null bitmap.");
            }

            Bitmap composite = new Bitmap(backgroundImage.Size.Width, backgroundImage.Size.Height);
            for (int x = startingAt.X; x < composite.Width; x++)
            {
                for (int y = startingAt.Y; y < composite.Height; y++)
                {
                    Color backgroundPixel = backgroundImage.GetPixel(x, y);
                    Color sourcePixel = topImage.GetPixel(x, y);
                    composite.SetPixel(x, y, sourcePixel.BlendByAlphaOnto(backgroundPixel));
                }
            }

            return composite;
        }

        public static Bitmap OverlayWith(this Bitmap baseBitmap, Bitmap bitmap, Point startingAt)
        {
            if (baseBitmap == null)
            {
                throw new ArgumentNullException("baseBitmap");
            }
            else if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            Bitmap overlayed = new Bitmap(baseBitmap);

            for (int x = startingAt.X; x < overlayed.Width; x++)
            {
                for (int y = startingAt.Y; y < overlayed.Height; y++)
                {
                    int bitmapX = x - startingAt.X;
                    int bitmapY = y - startingAt.Y;
                    if (bitmapX < bitmap.Width && bitmapY < bitmap.Height)
                    {
                        overlayed.SetPixel(x, y, bitmap.GetPixel(bitmapX, bitmapY));
                    }
                }
            }

            return overlayed;
        }

        public static Bitmap TrimRight(this Bitmap bitmap, Color backgroundColor)
        {
            Size croppedSize = bitmap.Size;
            int backgroundColorToArgb = backgroundColor.ToArgb();

            for (int x = bitmap.Width - 1; x >= 0; x--)
            {
                bool allPixelsAreBackgroundColor = true;
                for (int y = bitmap.Height - 1; y >= 0; y--)
                {
                    if (bitmap.GetPixel(x, y).ToArgb() != backgroundColorToArgb)
                    {
                        allPixelsAreBackgroundColor = false;
                        break;
                    }
                }

                if (allPixelsAreBackgroundColor)
                {
                    croppedSize.Width--;
                }
                else
                {
                    break;
                }
            }

            return bitmap.GetSection(new Rectangle(new Point(), croppedSize));
        }

        public static Bitmap AutoCrop(this Bitmap bitmap, Color backgroundColor)
        {
            Size croppedSize = bitmap.Size;
            int backgroundColorToArgb = backgroundColor.ToArgb();

            for (int x = bitmap.Width - 1; x >= 0; x--)
            {
                bool allPixelsAreBackgroundColor = true;
                for (int y = bitmap.Height - 1; y >= 0; y--)
                {
                    if (bitmap.GetPixel(x, y).ToArgb() != backgroundColorToArgb)
                    {
                        allPixelsAreBackgroundColor = false;
                        break;
                    }
                }

                if (allPixelsAreBackgroundColor)
                {
                    croppedSize.Width--;
                }
                else
                {
                    break;
                }
            }

            for (int y = bitmap.Height - 1; y >= 0; y--)
            {
                bool allPixelsAreBackgroundColor = true;
                for (int x = bitmap.Width - 1; x >= 0; x--)
                {
                    if (bitmap.GetPixel(x, y).ToArgb() != backgroundColorToArgb)
                    {
                        allPixelsAreBackgroundColor = false;
                        break;
                    }
                }

                if (allPixelsAreBackgroundColor)
                {
                    croppedSize.Height--;
                }
                else
                {
                    break;
                }
            }

            return bitmap.GetSection(new Rectangle(new Point(), croppedSize));
        }

        public static Bitmap GetSection(this Bitmap bitmap, Rectangle section)
        {
            if (section.Width == 0 || section.Height == 0)
            {
                return new Bitmap(1, 1);
            }

            Bitmap bitmapSection = new Bitmap(section.Width, section.Height);
            for (int x = 0; x < section.Width; x++)
            {
                for (int y = 0; y < section.Height; y++)
                {
                    int xWhole = x + section.X;
                    int yWhole = y + section.Y;
                    if (yWhole < bitmap.Height && yWhole >= 0 && xWhole < bitmap.Width && xWhole >= 0)
                    {
                        bitmapSection.SetPixel(x, y, bitmap.GetPixel(xWhole, yWhole));
                    }
                    else
                    {
                        bitmapSection.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            return bitmapSection;
        }
    }
}
