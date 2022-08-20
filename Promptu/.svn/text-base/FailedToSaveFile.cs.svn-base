using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal delegate void ResaveHandler(FailedToSaveFile file);

    internal class FailedToSaveFile
    {
        private string id;
        private string fileId;
        private FileSystemFile filepath;
        private ResaveHandler resaveHandler;

        public FailedToSaveFile(string id, string fileId, FileSystemFile filepath, ResaveHandler resaveHandler)
        {
            this.id = id;
            this.fileId = fileId;
            this.filepath = filepath;
            this.resaveHandler = resaveHandler;
        }

        public string Id
        {
            get { return this.id; }
        }

        public string FileId
        {
            get { return this.fileId; }
        }

        public FileSystemFile Filepath
        {
            get { return this.filepath; }
        }

        public ResaveHandler ResaveHandler
        {
            get { return this.resaveHandler; }
        }
    }
}
