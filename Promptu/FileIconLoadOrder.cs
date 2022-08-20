using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ZachJohnson.Promptu
{
    internal class FileIconLoadOrder : IconLoadOrder
    {
        private string fileFrom;

        public FileIconLoadOrder(string fileFrom, int indexTo)
            : base(indexTo)
        {
            if (fileFrom == null)
            {
                throw new ArgumentNullException("fileFrom");
            }

            this.fileFrom = fileFrom;
        }

        public string FileFrom
        {
            get { return this.fileFrom; }
        }

        protected override void LoadCore(SkinApi.ISuggestionProvider suggestionProvider, IconSize iconSize)
        {
            Icon icon = InternalGlobals.GuiManager.ToolkitHost.ExtractFileIcon(this.FileFrom, iconSize);

            if (icon != null)
            {
                Bitmap bitmap = icon.ToBitmap();
                icon.Dispose();

                if (suggestionProvider.Images.Count > this.IndexTo)
                {
                    suggestionProvider.Images[this.IndexTo] = InternalGlobals.GuiManager.ToolkitHost.ConvertImage(bitmap);
                }
                else
                {
                    for (int i = suggestionProvider.Images.Count; i < this.IndexTo; i++)
                    {
                        suggestionProvider.Images.Add(null);
                    }

                    suggestionProvider.Images.Add(InternalGlobals.GuiManager.ToolkitHost.ConvertImage(bitmap));
                }

                suggestionProvider.RefreshThreadSafe();
            }
        }
    }
}
