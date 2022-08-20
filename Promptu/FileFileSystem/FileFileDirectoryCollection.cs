//-----------------------------------------------------------------------
// <copyright file="FileFileDirectoryCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;
    using System.Collections.Generic;

    internal class FileFileDirectoryCollection : List<FileFileDirectory>
    {
        public FileFileDirectoryCollection()
        {
        }

        public FileFileDirectory this[string name]
        {
            get
            {
                string nameToUpperInvariant = name.ToUpperInvariant();
                foreach (FileFileDirectory directory in this)
                {
                    if (directory.Name.ToUpperInvariant() == nameToUpperInvariant)
                    {
                        return directory;
                    }
                }

                throw new ArgumentOutOfRangeException("No directory was found with the supplied name.");
            }
        }

        public bool Contains(string name)
        {
            string nameToUpperInvariant = name.ToUpperInvariant();
            foreach (FileFileDirectory directory in this)
            {
                if (directory.Name.ToUpperInvariant() == nameToUpperInvariant)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
