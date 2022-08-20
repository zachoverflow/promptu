// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
