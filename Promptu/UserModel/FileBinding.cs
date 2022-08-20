using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal struct FileBinding
    {
        private DateTime lastTimestamp;
        private FileSystemFile boundFile;

        public FileBinding(FileSystemFile boundFile)
            : this(boundFile.LastWriteTime, boundFile)
        {
        }

        public FileBinding(DateTime lastTimestamp, FileSystemFile boundFile)
        {
            this.lastTimestamp = lastTimestamp;
            this.boundFile = boundFile;
        }

        public DateTime LastTimestamp
        {
            get { return this.lastTimestamp; }
            set { this.lastTimestamp = value; }
        }

        public FileSystemFile BoundFile
        {
            get { return this.boundFile; }
        }

        public void UpdateLastTimestamp()
        {
            this.lastTimestamp = this.boundFile.LastWriteTime;
        }

        public bool FileHasChanged
        {
            get { return this.lastTimestamp != this.boundFile.LastWriteTime; }
        }

        public static implicit operator FileBinding(string path)
        {
            return new FileBinding(path);
        }
    }
}
