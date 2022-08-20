using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.SkinApi;
using System.Xml;

namespace ZachJohnson.Promptu.Skins
{
    internal class SkinCollection : List<PromptuSkin>
    {
        public SkinCollection()
        {
        }

        public static SkinCollection LoadFrom(FileSystemDirectory directory)
        {
            SkinCollection skins = new SkinCollection();
            foreach (FileSystemDirectory childDirectory in directory.GetDirectories())
            {
                try
                {
                    List<PromptuSkin> newlyLoadedSkins = LoadChildSkinsFrom(childDirectory);
                    if (newlyLoadedSkins != null)
                    {
                        skins.AddRange(newlyLoadedSkins);
                    }
                }
                catch (LoadException)
                {
                }
            }

            return skins;
        }

        public bool Contains(string id)
        {
            foreach (PromptuSkin skin in this)
            {
                if (id == skin.Id)
                {
                    return true;
                }
            }

            return false;
        }

        public PromptuSkin TryGet(string id)
        {
            foreach (PromptuSkin skin in this)
            {
                if (id == skin.Id)
                {
                    return skin;
                }
            }

            return null;
        }

        private static List<PromptuSkin> LoadChildSkinsFrom(FileSystemDirectory directory)
        {
            List<PromptuSkin> skins = new List<PromptuSkin>();

            FileSystemFile file = directory + "\\mainifest.xml";
            if (!file.Exists)
            {
                return null;
            }

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(file);
            }
            catch (XmlException ex)
            {
                throw new LoadException("Unable to load the manifest.", ex);
            }

            foreach (XmlNode root in document.ChildNodes)
            {
                if (root.Name.ToUpperInvariant() == "SKINS")
                {
                    foreach (XmlNode node in document.ChildNodes)
                    {
                        switch (node.Name.ToUpperInvariant())
                        {
                            case "SKIN":
                                try
                                {
                                    //HACK
                                    //PromptuSkin skin = Skin.LoadFrom(node);
                                    //if (skin != null)
                                    //{
                                    //    skins.Add(skin);
                                    //}
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

            return skins;
        }
    }
}
