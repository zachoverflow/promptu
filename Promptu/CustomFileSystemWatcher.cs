//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;

//namespace ZachJohnson.Promptu
//{
//    internal class CustomFileSystemWatcher : IDisposable
//    {
//        private FileSystemWatcher watcher;

//        public CustomFileSystemWatcher()
//        {
//            this.watcher = new FileSystemWatcher();
//        }

//        public CustomFileSystemWatcher(string path)
//        {
//            this.watcher = new FileSystemWatcher(path);
//        }

//        public CustomFileSystemWatcher(string path, string filter)
//        {
//            this.watcher = new FileSystemWatcher(path, filter);
//        }

//        ~CustomFileSystemWatcher()
//        {
//            this.Dispose(false);
//        }

//        public event FileSystemEventHandler Changed;

//        public event FileSystemEventHandler Created;

//        public event FileSystemEventHandler Deleted;

//        public event FileSystemEventHandler Renamed;

//        public event ErrorEventHandler Error;

//        public void Dispose()
//        {
//            this.Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (this.watcher != null)
//            {
//                this.watcher.Dispose();
//                this.watcher = null;
//            }
//        }

//        protected void ValidateNotDisposed()
//        {
//            if (this.watcher == null)
//            {
//                throw new ObjectDisposedException("CustomFileSystemWatcher");
//            }
//        }

//        protected virtual void OnChanged(FileSystemEventArgs e)
//        {
//            FileSystemEventHandler handler = this.Changed;
//            if (handler != null)
//            {
//                handler(this, e);
//            }
//        }

//        protected virtual void OnCreated(FileSystemEventArgs e)
//        {
//            FileSystemEventHandler handler = this.Created;
//            if (handler != null)
//            {
//                handler(this, e);
//            }
//        }

//        protected virtual void OnDeleted(FileSystemEventArgs e)
//        {
//            FileSystemEventHandler handler = this.Deleted;
//            if (handler != null)
//            {
//                handler(this, e);
//            }
//        }

//        protected virtual void OnRenamed(FileSystemEventArgs e)
//        {
//            FileSystemEventHandler handler = this.Renamed;
//            if (handler != null)
//            {
//                handler(this, e);
//            }
//        }

//        protected virtual void OnError(ErrorEventArgs e)
//        {
//            ErrorEventHandler handler = this.Error;
//            if (handler != null)
//            {
//                handler(this, e);
//            }
//        }

//        private void WatcherError(object sender, ErrorEventArgs e)
//        {
//        }
//    }
//}
