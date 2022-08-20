using System;
using System.Collections.Generic;
using System.Text;
using System.Extensions;

namespace ZachJohnson.Promptu.AssemblyCaching
{
    internal class CachedAssemblyCollection : List<CachedAssembly>
    {
        public CachedAssemblyCollection()
        {
        }

        public CachedAssembly this[string fileName]
        {
            get
            {
                string fileNameToUpperInvariant = fileName.ToUpperInvariantNullSafe();

                foreach (CachedAssembly assembly in this)
                {
                    if (assembly.File.Name.ToUpperInvariant() == fileNameToUpperInvariant)
                    {
                        return assembly;
                    }
                }

                throw new ArgumentOutOfRangeException("No cached assembly with that name was found.");
            }
        }

        public bool Contains(string fileName)
        {
            string fileNameToUpperInvariant = fileName.ToUpperInvariantNullSafe();
            foreach (CachedAssembly assembly in this)
            {
                if (assembly.File.Name.ToUpperInvariant() == fileNameToUpperInvariant)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Remove(string fileName)
        {
            CachedAssembly assemblyToRemove = null;
            string fileNameToUpperInvariant = fileName.ToUpperInvariantNullSafe();
            foreach (CachedAssembly assembly in this)
            {
                if (assembly.File.Name.ToUpperInvariant() == fileNameToUpperInvariant)
                {
                    assemblyToRemove = assembly;
                    break;
                }
            }

            if (assemblyToRemove != null)
            {
                this.Remove(assemblyToRemove);
                return true;
            }

            return false;
        }

        public CachedAssembly TryGet(string fileName)
        {
            string fileNameToUpperInvariant = fileName.ToUpperInvariantNullSafe();

            foreach (CachedAssembly assembly in this)
            {
                if (assembly.File.Name.ToUpperInvariant() == fileNameToUpperInvariant)
                {
                    return assembly;
                }
            }

            return null;
        }
    }
}
