//-----------------------------------------------------------------------
// <copyright file="CustomStreamReader.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System.IO;

    internal class CustomStreamReader : StreamReader
    {
        private string peekedLine;

        public CustomStreamReader(Stream stream)
            : base(stream)
        {
        }

        public string PeekLine()
        {
            this.peekedLine = this.ReadLine();
            return this.peekedLine;
        }

        public override string ReadLine()
        {
            if (this.peekedLine != null)
            {
                string line = this.peekedLine;
                this.peekedLine = null;
                return line;
            }

            return base.ReadLine();
        }
    }
}
