using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ZachJohnson.Promptu.AssemblyCaching
{
    internal class CachedAssembly
    {
        private FileSystemFile file;

        public CachedAssembly(FileSystemFile file)
        {
            this.file = file;
        }

        public FileSystemFile File
        {
            get { return this.file; }
        }
    }
}
