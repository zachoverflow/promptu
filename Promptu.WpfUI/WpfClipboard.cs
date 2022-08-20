using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZachJohnson.Promptu.UIModel;
using System.Collections;
using System.Collections.Specialized;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class WpfClipboard : ToolkitClipboard
    {
        public WpfClipboard()
        {
        }

        protected override bool GetContainsTextCore()
        {
            return Clipboard.ContainsText();
        }

        protected override bool GetContainsFileDropCore()
        {
            return Clipboard.ContainsFileDropList();
        }

        protected override StringCollection GetFileDropCore()
        {
            return Clipboard.GetFileDropList();
        }

        protected override void SetDataCore(ClipboardDataFormat dataFormat, object data)
        {
            Clipboard.SetData(GetCorrespondingFormat(dataFormat), data);
        }

        private static string GetCorrespondingFormat(ClipboardDataFormat dataFormat)
        {
            switch (dataFormat)
            {
                case ClipboardDataFormat.FileDrop:
                    return DataFormats.FileDrop;
                case ClipboardDataFormat.Text:
                default:
                    return DataFormats.UnicodeText;
            }
        }

        protected override object GetDataCore(string format)
        {
            return Clipboard.GetData(format);
        }

        protected override object GetDataCore(ClipboardDataFormat format)
        {
            return Clipboard.GetData(GetCorrespondingFormat(format));
        }

        protected override void ClearCore()
        {
            Clipboard.Clear();
        }

        protected override string GetTextCore()
        {
            return Clipboard.GetText();
        }

        protected override void SetDataCore(string format, object data)
        {
            Clipboard.SetData(format, data);
        }

        protected override void SetTextCore(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
