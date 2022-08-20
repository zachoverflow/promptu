using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class ToolkitClipboard
    {
        public ToolkitClipboard()
        {
        }

        public bool ContainsText
        {
            get { return this.GetContainsTextCore(); }
        }

        public bool ContainsFileDrop
        {
            get { return this.GetContainsFileDropCore(); }
        }

        public void SetData(ClipboardDataFormat dataFormat, object data)
        {
            this.SetDataCore(dataFormat, data);
        }

        public void SetText(string text)
        {
            this.SetTextCore(text);
        }

        public void SetData(string format, object data)
        {
            this.SetDataCore(format, data);
        }

        public void Clear()
        {
            this.ClearCore();
        }

        public string GetText()
        {
            return this.GetTextCore();
        }

        public object GetData(string format)
        {
            return this.GetDataCore(format);
        }

        public object GetData(ClipboardDataFormat format)
        {
            return this.GetDataCore(format);
        }

        public StringCollection GetFileDrop()
        {
            return this.GetFileDropCore();
        }

        protected abstract object GetDataCore(string format);

        protected abstract object GetDataCore(ClipboardDataFormat format);

        protected abstract string GetTextCore();

        protected abstract void ClearCore();

        protected abstract void SetDataCore(string format, object data);

        protected abstract void SetDataCore(ClipboardDataFormat dataFormat, object data);

        protected abstract void SetTextCore(string text);

        protected abstract bool GetContainsTextCore();

        protected abstract bool GetContainsFileDropCore();

        protected abstract StringCollection GetFileDropCore();
    }
}
