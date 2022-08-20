using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Extensions;
using System.ComponentModel;

namespace ZachJohnson.Promptu
{
    [TypeConverter(typeof(FileSystemFileConverter))]
    public struct FileSystemFile
    {
        private string path;
        private FileInfo fileInfo;

        public FileSystemFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            this.path = path;

            this.fileInfo = null;
        }

        public string Path
        {
            get { return this.path; }
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(this.path);
            }
        }

        public string Extension
        {
            get
            {
                return System.IO.Path.GetExtension(this.path);
            }
        }

        public FileAttributes Attributes
        {
            get
            {
                if (this.fileInfo == null)
                {
                    this.fileInfo = new FileInfo(this.path);
                }

                return this.fileInfo.Attributes;
            }
        }

        public void Touch()
        {
            File.SetLastWriteTime(this.Path, DateTime.Now);
        }

        public bool IsHidden
        {
            get { return (this.Attributes & FileAttributes.Hidden) != 0; }
        }

        public bool IsSystem
        {
            get { return (this.Attributes & FileAttributes.System) != 0; }
        }

        public string NameWithoutExtension
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(this.path);
            }
        }

        public void Create(bool alsoCreateDirectoryIfNecessary)
        {
            if (alsoCreateDirectoryIfNecessary)
            {
                this.GetParentDirectory().CreateIfDoesNotExist();
            }

            File.Create(this.path);
        }

        public void Create()
        {
            this.Create(false);
        }

        public void MoveTo(FileSystemFile newLocation)
        {
            File.Move(this, newLocation);
            this.path = newLocation;
        }

        public void Rename(string newName)
        {
            this.MoveTo(this.GetParentDirectory() + newName);
        }

        public bool Exists
        {
            get { return File.Exists(this); }
        }

        public void SafeOverwrite(Stream streamFrom)
        {
            FileSystemDirectory parent = this.GetParentDirectory();
            string firstSaveAsPath = parent + parent.GetAvailableFileName(this.Name + "{+}", ".temp{number}", InsertBase.Two);
            using (FileStream stream = new FileStream(firstSaveAsPath, FileMode.Create))
            {
                streamFrom.TransferTo(stream);
                stream.Close();
            }

            this.DeleteIfExists();
            File.Move(firstSaveAsPath, this.path);
        }

        public FileSystemDirectory GetParentDirectory()
        {
            return System.IO.Path.GetDirectoryName(this);
        }

        public static implicit operator string(FileSystemFile file)
        {
            return file.Path;
        }

        public static implicit operator FileSystemFile(string path)
        {
            if (path != null)
            {
                return new FileSystemFile(path);
            }

            return null;
        }

        public string ReadAllText()
        {
            return File.ReadAllText(this.path);
        }

        public string[] ReadAllLines()
        {
            return File.ReadAllLines(this.path);
        }

        public byte[] ReadAllBytes()
        {
            return File.ReadAllBytes(this.path);
        }

        public void WriteAllLines(string[] contents)
        {
            File.WriteAllLines(this.path, contents);
        }

        public void WriteAllBytes(byte[] bytes)
        {
            File.WriteAllBytes(this.path, bytes);
        }

        public void WriteAllText(string contents)
        {
            File.WriteAllText(this.path, contents);
        }

        public DateTime GetLastEditedTimestampUtc()
        {
            return File.GetLastWriteTimeUtc(this);
        }

        public override string ToString()
        {
            return this.path;
        }

        public void Delete()
        {
            File.Delete(this);
        }

        public void DeleteIfExists()
        {
            if (this.Exists)
            {
                this.Delete();
            }
        }

        public static bool operator ==(FileSystemFile file1, FileSystemFile file2)
        {
            return file1.Path == file2.Path;
        }

        public static bool operator !=(FileSystemFile file1, FileSystemFile file2)
        {
            return file1.Path != file2.Path;
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

        public bool IsRooted
        {
            get { return System.IO.Path.IsPathRooted(this.Path); }
        }

        public FileSystemFile GetRelativePath()
        {
            FileSystemDirectory root = System.Windows.Forms.Application.StartupPath;
            if (this.Path.StartsWith(root, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Path.Substring(root.Path.Length);
            }

            return this;
        }

        public FileSystemFile GetAbsolutePath()
        {
            if (!this.IsRooted && this.path.Length > 0)
            {
                return ((FileSystemDirectory)System.Windows.Forms.Application.StartupPath) + this.Path;
            }

            return this;
        }

        public override int GetHashCode()
        {
            return this.Path.GetHashCode();
        }

        public bool IsExactCopyOf(FileSystemFile file)
        {
            if (this == file)
            {
                return true;
            }

            using (FileStream thisStream = new FileStream(this, FileMode.Open))
            using (FileStream otherStream = new FileStream(file, FileMode.Open))
            {
                return thisStream.IsExactCopyOf(otherStream);
            }
        }

        public bool IsExactCopyOf(Stream stream)
        {
            using (FileStream thisStream = new FileStream(this, FileMode.Open))
            {
                return thisStream.IsExactCopyOf(stream);
            }
        }
    }
}
