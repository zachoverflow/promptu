using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ZachJohnson.Promptu
{
    public struct FileSystemDirectory
    {
        private string path;
        private DirectoryInfo directoryInfo;

        public FileSystemDirectory(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            this.path = path;
            if (!this.path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                this.path += System.IO.Path.DirectorySeparatorChar;
            }

            this.directoryInfo = null;
        }

        //public bool IsValid
        //{
        //    get
        //    {
        //        try
        //        {
        //            return System.IO.Path.GetFileName(this.Path).Length <= 0;
        //        }
        //        catch (ArgumentException)
        //        {
        //        }

        //        return false;
        //    }
        //}

        public string Path
        {
            get { return this.path; }
        }

        public string Name
        {
            get
            {
                string[] split = this.path.Split(System.IO.Path.DirectorySeparatorChar);

                return split[split.Length - 2];
            }
        }

        public bool Exists
        {
            get { return Directory.Exists(this); }
        }

        public bool LooksValid
        {
            get { return Utilities.LooksLikeValidPath(this); }
        }
        
        public void CreateIfDoesNotExist()
        {
            if (!this.Exists)
            {
                Directory.CreateDirectory(this);
            }
        }

        public FileAttributes Attributes
        {
            get
            {
                if (this.directoryInfo == null)
                {
                    this.directoryInfo = new DirectoryInfo(this.path);
                }

                return this.directoryInfo.Attributes;
            }
        }

        public bool IsHidden
        {
            get { return (this.Attributes & FileAttributes.Hidden) != 0; }
        }

        public bool IsSystem
        {
            get { return (this.Attributes & FileAttributes.System) != 0; }
        }

        public void Delete()
        {
            if (this.Exists)
            {
                Directory.Delete(this.path, true);
            }
        }

        public FileSystemDirectory GetParent()
        {
            return Directory.GetParent(this).FullName;
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(this);
        }

        public void CopyTo(FileSystemDirectory directory)
        {
            if (!directory.Exists)
            {
                directory.CreateIfDoesNotExist();
            }

            foreach (FileSystemDirectory childDirectory in this.GetDirectories())
            {
                childDirectory.CopyTo(directory + childDirectory.Name);
            }

            foreach (string file in this.GetFiles())
            {
                File.Copy(file, directory + System.IO.Path.GetFileName(file));
            }
        }

        internal FileSystemDirectory GetAvailableDirectoryName(string basicFormat, string insertFormat, InsertBase insertBase)
        {
            List<string> directories = new List<string>();
            if (this.Exists)
            {
                foreach (FileSystemDirectory directory in this.GetDirectories())
                {
                    directories.Add(directory.Name);
                }
            }

            return GeneralUtilities.GetAvailableIncrementingName(directories, basicFormat, insertFormat, false, insertBase);
        }

        internal FileSystemFile GetAvailableFileName(string basicFormat, string insertFormat, InsertBase insertBase)
        {
            List<string> files = new List<string>();
            if (this.Exists)
            {
                foreach (FileSystemFile file in this.GetFiles())
                {
                    files.Add(file.Name);
                }
            }

            return GeneralUtilities.GetAvailableIncrementingName(files, basicFormat, insertFormat, false, insertBase);
        }

        public FileSystemDirectory[] GetDirectories()
        {
            List<FileSystemDirectory> children = new List<FileSystemDirectory>();

            foreach (string child in Directory.GetDirectories(this.path))
            {
                children.Add(child);
            }

            return children.ToArray();
        }

        public FileSystemDirectory CreateDirectory(string nameOrRelativePath)
        {
            FileSystemDirectory directory = this + nameOrRelativePath;
            directory.CreateIfDoesNotExist();
            return directory;
        }

        public static implicit operator string(FileSystemDirectory directory)
        {
            return directory.Path;
        }

        public static implicit operator FileSystemDirectory(string path)
        {
            return new FileSystemDirectory(path);
        }

        public static bool operator ==(FileSystemDirectory directory1, FileSystemDirectory directory2)
        {
            return directory1.Path == directory2.Path;
        }

        public static bool operator !=(FileSystemDirectory directory1, FileSystemDirectory directory2)
        {
            return directory1.Path != directory2.Path;
        }

        public static string operator + (FileSystemDirectory directory, string relativePath)
        {
            if (relativePath == null)
            {
                throw new ArgumentNullException("relativePath");
            }

            string relativePathTrimmed = relativePath.TrimStart(System.IO.Path.DirectorySeparatorChar);
            return directory.Path + relativePathTrimmed;
        }

        public override bool Equals(object obj)
        {
            string s = obj as string;
            if (s != null)
            {
                return this.Path == s;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Path.GetHashCode();
        }

        public override string ToString()
        {
            return this.path;
        }
    }
}
