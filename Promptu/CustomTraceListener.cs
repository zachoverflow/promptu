using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Globalization;

#if DEBUG
namespace ZachJohnson.Promptu
{
    internal class CustomTraceListener : TraceListener
    {
        private StreamWriter appendWriter;
        public CustomTraceListener(FileSystemFile file)
        {
            this.appendWriter = File.AppendText(file);
            this.appendWriter.WriteLine("*NEW INSTANCE*");
        }

        public override void WriteLine(string s)
        {
            this.appendWriter.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}: {1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), s));
        }

        public override void Write(string message)
        {
            this.appendWriter.Write(message);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.appendWriter.Dispose();
        }
    }
}
#endif
