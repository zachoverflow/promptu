using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using ZachJohnson.Promptu.Collections;
//using ZachJohnson.Promptu.DynamicEntryModel;

namespace ZachJohnson.Promptu.UserModel.Collections
{
    internal class ProfilePlacemarkCollection : ChangeNotifiedList<ProfilePlacemark>
    {
        public ProfilePlacemarkCollection()
        {
        }

        public ProfilePlacemark this[string folderName]
        {
            get
            {
                foreach (ProfilePlacemark profilePlacemark in this)
                {
                    if (profilePlacemark.FolderName == folderName)
                    {
                        return profilePlacemark;
                    }
                }

                throw new ArgumentOutOfRangeException("No profile placemark has the provided name.");
            }
        }

        public static ProfilePlacemarkCollection FromFolder(FileSystemDirectory folder, bool silent)
        {
            ProfilePlacemarkCollection profilePlacemarks = new ProfilePlacemarkCollection();
            if (folder.Exists)
            {
                foreach (FileSystemDirectory childFolder in folder.GetDirectories())
                {
                    try
                    {
                        if (childFolder.Name.ToUpperInvariant() != "BASIC")
                        {
                            ProfilePlacemark profilePlacemark = ProfilePlacemark.FromFolder(childFolder);
                            profilePlacemarks.Add(profilePlacemark);
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (LoadException ex)
                    {
                        if (!silent)
                        {
                            Utilities.ShowPromptuErrorMessageBox(ex);
                        }
                    }
                    catch (IOException ex)
                    {
                        if (!silent)
                        {
                            Utilities.ShowPromptuErrorMessageBox(ex);
                        }
                    }
                    catch (XmlException ex)
                    {
                        if (!silent)
                        {
                            Utilities.ShowPromptuErrorMessageBox(ex);
                        }
                    }
                }
            }

            return profilePlacemarks;
        }

        public int IndexOf(string folderName)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].FolderName == folderName)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(string folderName)
        {
            foreach (ProfilePlacemark profilePlacemark in this)
            {
                if (profilePlacemark.FolderName == folderName)
                {
                    return true;
                }
            }

            return false;
        }

        public ProfilePlacemark TryGet(string folderName)
        {
            foreach (ProfilePlacemark profilePlacemark in this)
            {
                if (profilePlacemark.FolderName == folderName)
                {
                    return profilePlacemark;
                }
            }

            return null;
        }

        public bool Remove(string folderName)
        {
            ProfilePlacemark itemToRemove = null;
            foreach (ProfilePlacemark profilePlacemark in this)
            {
                if (profilePlacemark.FolderName == folderName)
                {
                    itemToRemove = profilePlacemark;
                    break;
                }
            }

            if (itemToRemove != null)
            {
                return this.Remove(itemToRemove);
            }

            return false;
        }

        public Dictionary<string, int> GetAllCachedAssemblyNamesReferences()
        {
            Dictionary<string, int> names = new Dictionary<string, int>();
            foreach (ProfilePlacemark profilePlacemark in this)
            {
                foreach (AssemblyReference reference in profilePlacemark.GetAllAssemblyReferences())
                {
                    if (reference.CachedName != null)
                    {
                        string referenceCachedNameInUppercase = reference.CachedName.ToUpperInvariant();
                        if (!names.ContainsKey(referenceCachedNameInUppercase))
                        {
                            names.Add(referenceCachedNameInUppercase, 1);
                        }
                        else
                        {
                            names[referenceCachedNameInUppercase]++;
                        }
                    }
                }
            }

            return names;
        }
    }
}
