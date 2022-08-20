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

namespace System.Xml.Extensions
{
    using System;

    internal static class XmlNodeExtensions
    {
        public static XmlNode FindChild(this XmlNode node, string name, bool caseSensitive)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            else if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            string nameToFind;

            if (caseSensitive)
            {
                nameToFind = name;
            }
            else
            {
                nameToFind = name.ToUpperInvariant();
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                string childName;

                if (caseSensitive)
                {
                    childName = childNode.Name;
                }
                else
                {
                    childName = childNode.Name.ToUpperInvariant();
                }

                if (nameToFind == childName)
                {
                    return childNode;
                }
            }

            return null;
        }
    }
}
