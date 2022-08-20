using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;

namespace ZachJohnson.Promptu.UserModel
{
    internal class SyncLocation : IDisposable
    {
        private const double ChangeDelayTime = 500;
        private string path;
        private FileSystemWatcher watcher;
        private Timer changeDelayTimer;

        public SyncLocation(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (!System.IO.Path.IsPathRooted(path) && path.Length > 0)
            {
                this.path = ((FileSystemDirectory)System.Windows.Forms.Application.StartupPath) + path;
            }
            else
            {
                this.path = path;
            }

            this.changeDelayTimer = new Timer(ChangeDelayTime);
            this.changeDelayTimer.AutoReset = false;
            this.changeDelayTimer.Elapsed += this.HandleChangeDelayTimerElapsed;
        }

        ~SyncLocation()
        {
            this.Dispose(false);
        }

        public void HookUpFileChangedNotifications()
        {
            try
            {
                FileSystemWatcher watcher = this.watcher;

                if (watcher != null)
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.Created -= this.FileSystemWatcherFileChanged;
                    watcher.Renamed -= this.FileSystemWatcherFileChanged;
                    watcher.Changed -= this.FileSystemWatcherFileChanged;
                    watcher.Dispose();
                    watcher = null;
                }

                if (watcher == null)
                {
                    watcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(this.path), System.IO.Path.GetFileName(this.path));
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.Created += this.FileSystemWatcherFileChanged;
                    watcher.Renamed += this.FileSystemWatcherFileChanged;
                    watcher.Changed += this.FileSystemWatcherFileChanged;
                    this.watcher = watcher;
                }

                if (watcher != null)
                {
                    watcher.EnableRaisingEvents = true;
                }
            }
            catch (ArgumentException)
            {
            }
        }

        public event EventHandler FileChanged;

        public Stream GetWriteStream()
        {
            return new FileStream(this.path, FileMode.Create);
        }

        public Stream GetReadStream()
        {
            return new FileStream(this.path, FileMode.Open);
        }

        public bool Exists
        {
            get { return File.Exists(this.path); }
        }

        public bool CanCreate
        {
            get 
            { 
                string rootDirectory = System.IO.Path.GetPathRoot(this.path);
                if (rootDirectory == null)
                {
                    return true;
                }
                else
                {
                    rootDirectory = rootDirectory.ToUpperInvariant();
                }

                foreach (DriveInfo info in DriveInfo.GetDrives())
                {
                    if (info.RootDirectory.FullName.ToUpperInvariant() == rootDirectory)
                    {
                        return info.RootDirectory.Exists;
                    }
                }

                return false;
            }
        }

        //public void Create()
        //{
        //    if (this.CanCreate)
        //    {
        //        FileSystemFile file = this.path;
        //        file.Create(true);
        //    }
        //}

        public bool CanDelete
        {
            get { return true; }
        }

        public void Delete()
        {
            File.Delete(this.path);
        }

        public string Path
        {
            get { return this.path; }
        }

        public string GetRelativePath()
        {
            FileSystemDirectory root = System.Windows.Forms.Application.StartupPath;
            if (this.Path.StartsWith(root, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Path.Substring(root.Path.Length);
            }

            return this.Path;
        }

        public bool HasSameLocationAs(SyncLocation location)
        {
            if (location == null)
            {
                return false;
            }

            return location.Path.ToUpperInvariant() == this.Path.ToUpperInvariant();
        }

        public override string ToString()
        {
            return this.path;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.watcher != null)
            {
                this.watcher.Dispose();
                this.watcher = null;
            }
        }

        protected virtual void OnFileChanged(EventArgs e)
        {
            EventHandler handler = this.FileChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleChangeDelayTimerElapsed(object sender, EventArgs e)
        {
            this.OnFileChanged(EventArgs.Empty);
        }

        private void FileSystemWatcherFileChanged(object sender, EventArgs e)
        {
            this.changeDelayTimer.Stop();
            this.changeDelayTimer.Start();
        }
    }
}
