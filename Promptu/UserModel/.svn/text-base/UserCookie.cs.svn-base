using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ZachJohnson.Promptu.UserModel
{
    internal class UserCookie
    {
        private string profileId = String.Empty;
        internal const string FileId = "cookie";

        public UserCookie(string profileId)
        {
            if (profileId != null)
            {
                this.profileId = profileId;
            }
        }

        public string ProfileId
        {
            get { return this.profileId; }
            set { this.profileId = value; }
        }

        public void Save()
        {
            FileSystemDirectory localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FileSystemDirectory promptuFolder = localAppDataFolder + "Promptu";
            promptuFolder.CreateIfDoesNotExist();

            XmlDocument document = new XmlDocument();
            XmlNode cookieNode = document.CreateElement("User");
            cookieNode.Attributes.Append(XmlUtilities.CreateAttribute("profile", this.profileId, document));
            document.AppendChild(cookieNode);

            InternalGlobals.FailedToSaveFiles.Remove(null, FileId);

            string path = promptuFolder + "Cookie.xml";

            try
            {
                document.Save(path);
            }
            catch (IOException)
            {
                InternalGlobals.FailedToSaveFiles.Add(
                    new FailedToSaveFile(null, FileId, path, new ResaveHandler(InternalGlobals.ResaveProfileItem)));
            }

            //document.Save(promptuFolder + "Cookie.xml");
        }

        public static UserCookie Load()
        {
            FileSystemDirectory localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FileSystemDirectory promptuFolder = localAppDataFolder + "Promptu";
            string cookieFilepath = promptuFolder + "Cookie.xml";
            string profileId = String.Empty;
            if (File.Exists(cookieFilepath))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(File.ReadAllText(cookieFilepath));

                    foreach (XmlNode root in document)
                    {
                        switch (root.Name.ToUpperInvariant())
                        {
                            case "USER":
                                foreach (XmlAttribute attribute in root.Attributes)
                                {
                                    switch (attribute.Name.ToUpperInvariant())
                                    {
                                        case "PROFILE":
                                            profileId = attribute.Value;
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
                catch (XmlException)
                {
                }
            }

            return new UserCookie(profileId);
        }
    }
}
