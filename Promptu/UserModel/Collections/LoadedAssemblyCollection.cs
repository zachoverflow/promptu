using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class LoadedAssemblyCollection : List<LoadedAssembly>
    {
        public LoadedAssemblyCollection()
        {
        }

        public LoadedAssembly TryGet(string name)
        {
            string uppercaseName = name.ToUpperInvariant();
            foreach (LoadedAssembly assembly in this)
            {
                if (uppercaseName == assembly.Name.ToUpperInvariant())
                {
                    return assembly;
                }
            }

            return null;
        }
    }
}
