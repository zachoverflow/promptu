//-----------------------------------------------------------------------
// <copyright file="PluginUpdate.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using ZachJohnson.Promptu.FileFileSystem;

    internal class PluginUpdate : INotifyPropertyChanged, IIndicatesProgress
    {
        private PromptuPlugin plugin;
        private string downloadUrl;
        private ReleaseVersion latestVersion;
        private bool isUpdating;
        private int progress;
        private string updateStatusMessage;
        private bool showStatusMessage = true;
        private bool updateError;

        public PluginUpdate(PromptuPlugin plugin, string downloadUrl, ReleaseVersion latestVersion)
        {
            this.downloadUrl = downloadUrl;
            this.plugin = plugin;
            this.latestVersion = latestVersion;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PromptuPlugin Plugin
        {
            get { return this.plugin; }
        }

        public string DownloadUrl
        {
            get { return this.downloadUrl; }
        }

        public bool UpdateError
        {
            get
            {
                return this.updateError;
            }

            set
            {
                this.updateError = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("UpdateError"));
            }
        }

        public ReleaseVersion LatestVersion
        {
            get { return this.latestVersion; }
        }

        public bool IsUpdating
        {
            get
            {
                return this.isUpdating;
            }

            set
            {
                this.isUpdating = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("IsUpdating"));
            }
        }

        public bool ShowStatusMessage
        {
            get
            {
                return this.showStatusMessage;
            }

            set
            {
                this.showStatusMessage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ShowStatusMessage"));
            }
        }

        public int ProgressPercentage
        {
            get
            {
                return this.progress;
            }

            set
            {
                this.progress = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ProgressPercentage"));
            }
        }

        public string UpdateStatusMessage
        {
            get
            {
                return this.updateStatusMessage;
            }

            set
            {
                this.updateStatusMessage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("UpdateStatusMessage"));
            }
        }

        string IIndicatesProgress.StatusMessage
        {
            set { this.UpdateStatusMessage = value; }
        }

        public void RunUpdate()
        {
            this.IsUpdating = true;
            try
            {
                using (MemoryStream stream = Updater.Download(this.downloadUrl, this))
                {
                    this.UpdateStatusMessage = Localization.UIResources.PluginInstallingStatus;
                    FileFileDirectory package = FileFileDirectory.FromContainer(stream);
                    Updater.Unpack(package, this.Plugin.Folder, true);
                    this.UpdateStatusMessage = Localization.UIResources.PluginInstalledStatus;
                }
            }
            catch (WebException)
            {
                this.UpdateError = true;
                this.UpdateStatusMessage = Localization.UIResources.PluginUpdateWebError;
                this.ProgressPercentage = 100;
            }
            catch (FileFileSystemException)
            {
                this.UpdateError = true;
                this.UpdateStatusMessage = Localization.UIResources.PluginUpdateFileError;
                this.ProgressPercentage = 100;
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
