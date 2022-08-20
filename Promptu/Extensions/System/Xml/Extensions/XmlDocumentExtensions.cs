//-----------------------------------------------------------------------
// <copyright file="XmlDocumentExtensions.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace System.Xml.Extensions
{
    using System;
    using System.Xml;

    internal static class XmlDocumentExtensions
    {
        public static void AppendHeader(this XmlDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            document.AppendChild(document.CreateProcessingInstruction("xml", "version=\"1.0\""));
        }

        public static XmlNode CreateNewNode(this XmlDocument document, string name, string value)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            else if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            else if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            XmlNode node = document.CreateElement(name);
            node.InnerText = value;
            return node;
        }
    }
}
