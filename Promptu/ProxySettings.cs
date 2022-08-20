using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ZachJohnson.Promptu.FileFileSystem;
using System.IO;
using System.Security;
using System.Security.Extensions;

namespace ZachJohnson.Promptu
{
    internal class ProxySettings
    {
        private ProxyMode mode;
        private string address;
        private SecureString password;
        private string username;

        public ProxySettings()
            : this(ProxyMode.NoProxy, null, null, null)
        {
        }

        public ProxySettings(ProxyMode mode, string address, SecureString password, string username)
        {
            this.mode = mode;
            if (address == null)
            {
                this.address = String.Empty;
            }
            else
            {
                this.address = address;
            }

            if (password == null)
            {
                this.password = new SecureString();
            }
            else
            {
                this.password = password;
            }

            if (this.username == null)
            {
                this.username = String.Empty;
            }
            else
            {
                this.username = username;
            }
        }

        public ProxyMode Mode
        {
            get { return this.mode; }
            set { this.mode = value; }
        }

        public string Address
        {
            get 
            { 
                return this.address; 
            }

            set
            {
                if (value == null)
                {
                    value = String.Empty;
                }

                this.address = value; 
            }
        }

        public string Username
        {
            get 
            {
                return this.username; 
            }

            set 
            {
                if (value == null)
                {
                    value = String.Empty;
                }

                this.username = value; 
            }
        }

        public SecureString Password
        {
            get 
            {
                return this.password; 
            }

            set
            {
                if (value == null)
                {
                    value = new SecureString();
                }
 
                this.password = value; 
            }
        }

        public void ToStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            XmlDocument document = new XmlDocument();
            XmlNode mainNode = document.CreateElement("Proxy");
            document.AppendChild(mainNode);
            mainNode.Attributes.Append(XmlUtilities.CreateAttribute("mode", this.mode, document));
            if (this.address != null)
            {
                mainNode.Attributes.Append(XmlUtilities.CreateAttribute("address", this.address, document));
            }

            if (this.username != null)
            {
                mainNode.Attributes.Append(XmlUtilities.CreateAttribute("username", this.username, document));
            }

            if (this.password != null)
            {
                mainNode.Attributes.Append(XmlUtilities.CreateAttribute("password", this.password.ConvertToUnsecureString(), document));
            }

            document.Save(stream);
        }

        public static ProxySettings FromStream(Stream stream)
        {
            ProxySettings settings = new ProxySettings();

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(stream);

                foreach (XmlNode node in document.ChildNodes)
                {
                    if (node.Name.ToUpperInvariant() == "PROXY")
                    {
                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "MODE":
                                    try
                                    {
                                        settings.Mode = (ProxyMode)Enum.Parse(typeof(ProxyMode), attribute.Value);
                                    }
                                    catch (ArgumentException)
                                    {
                                    }

                                    break;
                                case "ADDRESS":
                                    settings.Address = attribute.Value;
                                    break;
                                case "USERNAME":
                                    settings.Username = attribute.Value;
                                    break;
                                case "PASSWORD":
                                    settings.Password = new SecureString();
                                    foreach (char c in attribute.Value)
                                    {
                                        settings.Password.AppendChar(c);
                                    }

                                    break;
                                default:
                                    break;
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
