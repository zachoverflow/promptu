using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ZachJohnson.Promptu.FileFileSystem;

namespace ZachJohnson.Promptu
{
    internal class GlobalSettings
    {
        private FileSystemFile associatedFile;
        private ProxySettings proxySettings;
        private bool checkForPreReleaseUpdates;

        public GlobalSettings(FileSystemFile associatedFile)
        {
            this.associatedFile = associatedFile;
            this.proxySettings = new ProxySettings();
        }

        public bool CheckForPreReleaseUpdates
        {
            get { return this.checkForPreReleaseUpdates; }
            set { this.checkForPreReleaseUpdates = value; }
        }

        public ProxySettings Proxy
        {
            get { return this.proxySettings; }
        }

        public void Save()
        {
            this.associatedFile.GetParentDirectory().CreateIfDoesNotExist();

            XmlDocument miscDocument = new XmlDocument();

            XmlDocument document = new XmlDocument();
            XmlNode mainNode = document.CreateElement("Misc");
            document.AppendChild(mainNode);
            mainNode.Attributes.Append(XmlUtilities.CreateAttribute("checkForPreReleaseUpdates", this.checkForPreReleaseUpdates, document));

            using (MemoryStream proxyStream = new MemoryStream())
            using (MemoryStream miscStream = new MemoryStream())
            {
                this.proxySettings.ToStream(proxyStream);
                document.Save(miscStream);
                FileFileDirectory root = new FileFileDirectory(String.Empty);
                root.Files.Add(new FileFile("ProxySettings", proxyStream));
                root.Files.Add(new FileFile("Misc", miscStream));
                root.SaveInContainer(this.associatedFile);
            }
        }

        public static GlobalSettings FromFile(FileSystemFile file)
        {
            GlobalSettings settings = new GlobalSettings(file);

            try
            {
                FileFileDirectory root = FileFileDirectory.FromContainer(file);
                settings.proxySettings = ProxySettings.FromStream(root.Files["ProxySettings"].Contents);

                FileFile miscFile = root.Files.TryGet("Misc");

                if (miscFile != null)
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(miscFile.Contents);

                    foreach (XmlNode node in document.ChildNodes)
                    {
                        if (node.Name.ToUpperInvariant() == "MISC")
                        {
                            foreach (XmlAttribute attribute in node.Attributes)
                            {
                                switch (attribute.Name.ToUpperInvariant())
                                {
                                    case "CHECKFORPRERELEASEUPDATES":
                                        settings.checkForPreReleaseUpdates = Utilities.TryParseBoolean(attribute.Value, settings.checkForPreReleaseUpdates);
                                        //try
                                        //{
                                        //    settings.checkForPreReleaseUpdates = Convert.ToBoolean(attribute.Value);
                                        //}
                                        //catch (FormatException)
                                        //{
                                        //}

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
            }
            catch (IOException)
            {
            }
            catch (FileFileSystemException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (InvalidDataException)
            {
            }

            return settings;
        }
    }
}
