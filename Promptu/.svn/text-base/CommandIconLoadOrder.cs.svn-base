using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace ZachJohnson.Promptu
{
    internal class CommandIconLoadOrder : IconLoadOrder
    {
        private FileSystemFile file;
        private bool applyNamespaceOverlay;

        public CommandIconLoadOrder(FileSystemFile file, bool applyNamespaceOverlay, int indexTo)
            : base(indexTo)
        {
            this.file = file;
            this.applyNamespaceOverlay = applyNamespaceOverlay;
        }

        protected override void LoadCore(SkinApi.ISuggestionProvider suggestionProvider, IconSize iconSize)
        {
            if (!this.file.Exists)
            {
                return;
            }

            //bitmap(Bitmap)Bitmap.FromFile(iconFile);
            ////Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractFileIcon(this.FileFrom, iconSize);

            //if (icon != null)
            //{
            try
            {
                Bitmap bitmap;

                if (file.Extension.ToUpperInvariant() == ".ICO")
                {
                    try
                    {
                        Icon icon = new Icon(file, new Size(32, 32));
                        bitmap = icon.ToBitmap();
                    }
                    catch (ArgumentException)
                    {
                        bitmap = (Bitmap)Bitmap.FromFile(this.file);
                    }
                }
                else
                {
                    bitmap = (Bitmap)Bitmap.FromFile(this.file);//.ToBitmap();
                }

                object finalImage = this.applyNamespaceOverlay ?
                    InternalGlobals.GuiManager.ToolkitHost.Images.CreateCompositeWithNamespaceOverlay(bitmap) :
                    InternalGlobals.GuiManager.ToolkitHost.ConvertImage(bitmap);

                if (suggestionProvider.Images.Count > this.IndexTo)
                {
                    suggestionProvider.Images[this.IndexTo] = finalImage;
                }
                else
                {
                    for (int i = suggestionProvider.Images.Count; i < this.IndexTo; i++)
                    {
                        suggestionProvider.Images.Add(null);
                    }

                    suggestionProvider.Images.Add(finalImage);
                }

                suggestionProvider.RefreshThreadSafe();
            }
            catch (OutOfMemoryException)
            {
            }
            catch (IOException)
            {
            }
        }
    }
}
