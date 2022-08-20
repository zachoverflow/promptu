using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace ZachJohnson.Promptu.UserModel
{
    internal class LoadedAssembly
    {
        private string name;
        private Assembly assembly;
        //private MemoryStream bytes;

        public LoadedAssembly(string name, Assembly assembly)
        {
            this.name = name;
            this.assembly = assembly;
            //this.bytes = bytes;
        }

        public string Name
        {
            get { return this.name; }
        }

        public Assembly Assembly
        {
            get { return this.assembly; }
        }

        //public MemoryStream Bytes
        //{
        //    get { return this.bytes; } 
        //}
    }
}
