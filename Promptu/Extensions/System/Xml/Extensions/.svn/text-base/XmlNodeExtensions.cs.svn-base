//-----------------------------------------------------------------------
// <copyright file="XmlNodeExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
