using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;

namespace ZachJohnson.Promptu.WpfUI.Collections
{
    internal class WindowDataExtractions : Collection<WindowDataExtraction>
    {
        public WindowDataExtractions()
        {
        }

        public WindowDataExtractions GetExtractionsForProcess(string processName)
        {
            WindowDataExtractions found = new WindowDataExtractions();
            foreach (WindowDataExtraction extraction in this)
            {
                if (extraction.ProcessName == processName)
                {
                    found.Add(extraction);
                }
            }

            return found;
        }

        public static WindowDataExtractions FromFile(string path)
        {
            WindowDataExtractions extractions = new WindowDataExtractions();

            if (!File.Exists(path))
            {
                return extractions;
            }

            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(File.ReadAllText(path));
            }
            catch (XmlException)
            {
                return extractions;
            }

            foreach (XmlNode root in document.ChildNodes)
            {
                if (root.Name.ToUpperInvariant() == "WINDOWDATAEXTRACTIONS")
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.Name.ToUpperInvariant())
                        {
                            case WindowDataExtraction.XmlAlias:
                                try
                                {
                                    extractions.Add(WindowDataExtraction.FromXml(node));
                                }
                                catch (LoadException)
                                {
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return extractions;
        }
    }
}
