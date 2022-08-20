using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal class PdcMeta
    {
        private int revision;

        public PdcMeta(int revision)
        {
            this.revision = revision;
        }

        public int Revision
        {
            get { return this.revision; }
        }

        public static PdcMeta FromStream(Stream s)
        {
            if (s.CanSeek)
            {
                s.Position = 0;
            }

            int revision = 0;

            XmlDocument document = new XmlDocument();
            document.Load(s);

            foreach (XmlNode root in document.ChildNodes)
            {
                if (root.Name.ToUpperInvariant() == "META")
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.Name.ToUpperInvariant())
                        {
                            case "DATA":
                                foreach (XmlAttribute attribute in node.Attributes)
                                {
                                    switch (attribute.Name.ToUpperInvariant())
                                    {
                                        case "REVISION":
                                            try
                                            {
                                                revision = Convert.ToInt32(attribute.Value, CultureInfo.InvariantCulture);
                                            }
                                            catch (FormatException)
                                            {
                                            }
                                            catch (OverflowException)
                                            {
                                            }

                                            break;
                                        default:
                                            break;
                                    }
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return new PdcMeta(revision);
        }
    }
}
