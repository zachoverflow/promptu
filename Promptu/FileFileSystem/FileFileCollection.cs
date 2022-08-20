//-----------------------------------------------------------------------
// <copyright file="FileFileCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.FileFileSystem
{
    using System;
    using System.Collections.Generic;

    internal class FileFileCollection : List<FileFile>
    {
        public FileFileCollection()
        {
        }

        public FileFile this[string name]
        {
            get
            {
                string nameToUpperInvariant = name.ToUpperInvariant();
                foreach (FileFile file in this)
                {
                    if (file.Name.ToUpperInvariant() == nameToUpperInvariant)
                    {
                        return file;
                    }
                }

                throw new ArgumentOutOfRangeException("No file was found with the supplied name.");
            }
        }

        public bool Contains(string name)
        {
            if (name == null)
            {
                return false;
            }

            string nameToUpperInvariant = name.ToUpperInvariant();
            foreach (FileFile file in this)
            {
                if (file.Name.ToUpperInvariant() == nameToUpperInvariant)
                {
                    return true;
                }
            }

            return false;
        }

        public FileFile TryGet(string name)
        {
            string nameToUpperInvariant = name.ToUpperInvariant();
            foreach (FileFile file in this)
            {
                if (file.Name.ToUpperInvariant() == nameToUpperInvariant)
                {
                    return file;
                }
            }

            return null;
        }
    }
}
