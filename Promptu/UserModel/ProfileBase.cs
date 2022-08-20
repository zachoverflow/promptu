using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ZachJohnson.Promptu.UserModel
{
    public abstract class ProfileBase
    {
        private string name;
        private FileSystemFile lockFile;
        private FileSystemDirectory directory;
        private bool isLocked;
        private bool showSplashScreen;

        public ProfileBase(
            FileSystemDirectory directory,
            string name,
            bool showSplashScreen)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.directory = directory;
            this.name = name;
            this.showSplashScreen = showSplashScreen;

            this.lockFile = new FileSystemFile(this.directory + "profile.lock");
            this.UpdateIsLocked();
        }

        protected string NameInternal
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        protected bool ShowSplashScreenInternal
        {
            get
            {
                return this.showSplashScreen;
            }

            set
            {
                this.showSplashScreen = value;
            }
        }

        protected FileSystemFile LockFile
        {
            get { return this.lockFile; }
        }

        internal string FolderName
        {
            get { return this.directory.Name; }
        }

        public FileSystemDirectory Directory
        {
            get { return this.directory; }
        }

        internal bool IsExternallyLocked
        {
            get
            {
                if (this.isLocked)
                {
                    this.UpdateIsLocked();
                }

                return this.isLocked;
            }
        }

        internal string Locker
        {
            get
            {
                if (this.isLocked)
                {
                    if (this.lockFile.Exists)
                    {
                        string[] lockContents = this.lockFile.ReadAllLines();
                        if (lockContents.Length > 1)
                        {
                            return lockContents[1];
                        }
                    }

                    return Localization.Promptu.UnknownLocker;
                }

                return null;
            }
        }

        private void UpdateIsLocked()
        {
            if (this.lockFile.Exists)
            {
                string[] lockContents = this.lockFile.ReadAllLines();
                if (lockContents.Length > 0)
                {
                    try
                    {
                        DateTime lockTime = DateTime.FromBinary(Convert.ToInt64(lockContents[0], CultureInfo.InvariantCulture));
                        DateTime now = DateTime.Now;
                        if (lockTime < now && (now - lockTime).TotalSeconds < 70)
                        {
                            if (lockContents.Length > 2)
                            {
                                this.isLocked = lockContents[1] != Environment.UserName || lockContents[2] != Environment.MachineName;
                            }
                            else
                            {
                                this.isLocked = true;
                            }

                            return;
                        }
                    }
                    catch (FormatException)
                    {
                    }
                    catch (OverflowException)
                    {
                    }
                }
            }

            this.isLocked = false;
        }

        public override string ToString()
        {
            return this.NameInternal;
        }
    }
}
