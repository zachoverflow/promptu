//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.Collections;
//using System.Xml;
//using System.IO;
//using ZachJohnson.Promptu.Skins;

//namespace ZachJohnson.Promptu.UserModel.Collections
//{
//    internal class SkinDataCollection : List<SkinData>
//    {
//        internal const string FileId = "skindata";
//        private FileSystemFile file;

//        public SkinDataCollection(FileSystemFile file, SkinCollection skins)
//        {
//            if (skins == null)
//            {
//                throw new ArgumentNullException("skins");
//            }

//            this.file = file;

//            foreach (Skin skin in skins)
//            {
//                this.Add(new SkinData(skin));
//            }
//        }

//        public SkinData this[string skinNameFor]
//        {
//            get
//            {
//                foreach (SkinData data in this)
//                {
//                    if (data.Skin.Name == skinNameFor)
//                    {
//                        return data;
//                    }
//                }

//                throw new ArgumentException("No SkinData has a skin of that name.");
//            }
//        }

//        public bool Contains(string skinNameFor)
//        {
//            foreach (SkinData data in this)
//            {
//                if (data.Skin.Name == skinNameFor)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        public void TakeAllSnapshots()
//        {
//            foreach (SkinData data in this)
//            {
//                data.TakeSnapshot();
//            }
//        }

//        public void RestoreAll()
//        {
//            foreach (SkinData data in this)
//            {
//                data.Restore();
//            }
//        }

//        public void TakeAllSnapshotsAndSave()
//        {
//            this.TakeAllSnapshots();
//            this.Save();
//        }

//        public static SkinDataCollection FromXml(FileSystemFile file, SkinCollection skins)
//        {
//            SkinDataCollection collection = new SkinDataCollection(file, skins);
//            XmlDocument document = new XmlDocument();
//            try
//            {
//                document.LoadXml(file.ReadAllText());
//            }
//            catch (XmlException ex)
//            {
//                ex.Data.Add(Globals.ExceptionPathToken, file.Path);
//                throw;
//            }
//            catch (IOException ex)
//            {
//                ex.Data.Add(Globals.ExceptionPathToken, file.Path);
//                throw;
//            }

//            foreach (XmlNode root in document.ChildNodes)
//            {
//                if (root.Name == "SkinData")
//                {
//                    foreach (XmlNode skinNode in root.ChildNodes)
//                    {
//                        if (skinNode.Name == "Skin")
//                        {
//                            string name = null;
//                            foreach (XmlAttribute attribute in skinNode.Attributes)
//                            {
//                                if (attribute.Name == "name")
//                                {
//                                    name = attribute.Value;
//                                }
//                            }

//                            if (name != null && collection.Contains(name))
//                            {
//                                collection[name].LoadDataFromXml(skinNode);
//                            }
//                        }
//                    }
//                }
//            }

//            return collection;
//        }

//        public void Save()
//        {
//            XmlDocument document = new XmlDocument();
//            XmlUtilities.AppendHeader(document);

//            XmlNode root = document.CreateElement("SkinData");

//            foreach (SkinData data in this)
//            {
//                XmlNode dataNode = data.ToXml(document);
//                dataNode.Attributes.Append(XmlUtilities.CreateAttribute("name", data.Skin.Name, document));
//                root.AppendChild(dataNode);
//            }

//            document.AppendChild(root);

//            //document.Save(this.file.Path);

//            Globals.FailedToSaveFiles.Remove(null, FileId);

//            try
//            {
//                document.Save(this.file.Path);
//            }
//            catch (IOException)
//            {
//                Globals.FailedToSaveFiles.Add(
//                    new FailedToSaveFile(null, FileId, this.file.Path, new ResaveHandler(Globals.ResaveProfileItem)));
//            }
//        }
//    }
//}
