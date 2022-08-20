using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using ZachJohnson.Promptu.FileFileSystem;
using System.IO.Extensions;
using ZachJohnson.Promptu.Skins;
using ZachJohnson.Promptu.UIModel;
using ZachJohnson.Promptu.UIModel.Presenters;
using System.Security.Extensions;
using ZachJohnson.Promptu.PluginModel;
using ZachJohnson.Promptu.PluginModel.Internals;
using System.ComponentModel;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    internal static class Updater
    {
        private static bool checkingForUpdateAlready;
        private static bool firstCheckHasRun;
        private static DateTime lastUpdateCheckTime; 

        public static DateTime LastUpdateCheckTime
        {
            get { return lastUpdateCheckTime; }
        }

        public static bool FirstCheckHasRun
        {
            get { return firstCheckHasRun; }
        }

        public static void CheckForUpdateAsyncAutoUpdate()
        {
            System.Threading.Thread updateCheckThread = new System.Threading.Thread(CheckForUpdateAutoUpdate);
            updateCheckThread.IsBackground = true;
            updateCheckThread.Start();
        }

        private static void CheckForUpdateAutoUpdate()
        {
            CheckForUpdate(true);
        }

        public static void CheckForUpdate(bool isAutoUpdate)
        {
            if (checkingForUpdateAlready)
            {
                return;
            }

            checkingForUpdateAlready = true;
            string packagePath;
            try
            {
                if (UpdateIsAvailable(out packagePath, !isAutoUpdate) && packagePath != null)
                {
                    DateTime? lastUpdatePrompt = null;

                    FileSystemDirectory settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    FileSystemDirectory promptuDirectory = settingsDirectory + "Promptu";
                    FileSystemFile lastUpdatePromptFile = promptuDirectory + "LastUpdatePrompt";
                    if (lastUpdatePromptFile.Exists)
                    {
                        try
                        {
                            lastUpdatePrompt = DateTime.Parse(lastUpdatePromptFile.ReadAllText(), CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                        }
                    }

                    if (!isAutoUpdate || (lastUpdatePrompt == null || lastUpdatePrompt.Value < DateTime.Now.AddDays(-7)))
                    {
                        ParameterlessVoid askUserAction = new ParameterlessVoid(delegate
                            {
                                UpdateAvailableDialogPresenter updateAvailableDialog = new UpdateAvailableDialogPresenter();
                                if (updateAvailableDialog.ShowDialog() == UIDialogResult.OK)
                                {
                                    PerformUpdate(packagePath);
                                    //Action<string> performUpdate = new Action<string>(PerformUpdate);
                                    //PromptHandler handler = PromptHandler.GetInstance();
                                    //if (handler.InvokeOnMainThreadRequired)
                                    //{
                                    //    handler.InvokeOnMainThread(performUpdate, packagePath);
                                    //}
                                    //else
                                    //{
                                    //    performUpdate(packagePath);
                                    //}
                                }
                                else
                                {
                                    lastUpdatePromptFile.WriteAllText(DateTime.Now.ToString());
                                }
                            });

                        PromptHandler handler = PromptHandler.GetInstance();
                        if (handler.InvokeOnMainThreadRequired)
                        {
                            handler.InvokeOnMainThread(askUserAction);
                        }
                        else
                        {
                            askUserAction();
                        }
                    }
                    else
                    {
                        CheckForPluginUpdatesIfAllowed(true);
                    }
                }
                else
                {
                    if (!isAutoUpdate)
                    {
                        UIMessageBox.Show(
                            Localization.Promptu.AppUpToDate,
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Information,
                            UIMessageBoxResult.OK);
                    }
                    else
                    {
                        CheckForPluginUpdatesIfAllowed(true);
                    }
                }

            }
            catch (WebException)
            {
                UIMessageBox.Show(
                        Localization.Promptu.UpdateErrorContactingWebServer,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.OK);
            }

            lastUpdateCheckTime = DateTime.Now;
            firstCheckHasRun = true;
            checkingForUpdateAlready = false;
        }

        public static string DefaultUserAgent
        {
            get
            {
                StringBuilder userAgent = new StringBuilder();

                userAgent.AppendFormat("Promptu/{0}", Assembly.GetExecutingAssembly().GetName().Version);

                if (InternalGlobals.CurrentReleaseType == ReleaseType.Beta)
                {
                    userAgent.AppendFormat("b");
                }
                else if (InternalGlobals.CurrentReleaseType == ReleaseType.Alpha)
                {
                    userAgent.AppendFormat("a");
                }

#if DEBUG
                userAgent.Append(" (dev)");
#endif
                return userAgent.ToString();
            }
        }

        public static string FaviconUserAgent
        {
            get
            {
                StringBuilder userAgent = new StringBuilder();

                userAgent.AppendFormat("Promptu/{0}", Assembly.GetExecutingAssembly().GetName().Version);

                if (InternalGlobals.CurrentReleaseType == ReleaseType.Beta)
                {
                    userAgent.AppendFormat("b");
                }
                else if (InternalGlobals.CurrentReleaseType == ReleaseType.Alpha)
                {
                    userAgent.AppendFormat("a");
                }

                userAgent.Append(" (+http://www.promptulauncher.com/webmasters.php)");
                return userAgent.ToString();
            }
        }

        public static PromptuWebClient GetFaviconWebClient()
        {
            return GetDefaultWebClient(FaviconUserAgent);
        }

        public static PromptuWebClient GetDefaultWebClient()
        {
            return GetDefaultWebClient(DefaultUserAgent);
        }

        public static PromptuWebClient GetDefaultWebClient(string userAgent)
        {
            PromptuWebClient client = new PromptuWebClient(userAgent);
            //if (userAgent != null)
            //{
            //    client.Headers.Add("User-Agent", userAgent);
            //}

            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Proxy = WebRequest.DefaultWebProxy;
            client.Proxy.Credentials = client.Credentials;

            if (InternalGlobals.Settings.Proxy.Mode != ProxyMode.NoProxy)
            {
                if (InternalGlobals.Settings.Proxy.Mode == ProxyMode.FromIE)
                {
                    client.Proxy = WebRequest.GetSystemWebProxy();
                }
                else
                {
                    string proxyAddress = InternalGlobals.Settings.Proxy.Address;
                    if (!String.IsNullOrEmpty(proxyAddress))
                    {
                        try
                        {
                            WebProxy proxy = new WebProxy(proxyAddress, true);
                            if (!String.IsNullOrEmpty(InternalGlobals.Settings.Proxy.Username) && InternalGlobals.Settings.Proxy.Password != null)
                            {
                                proxy.Credentials = new NetworkCredential(InternalGlobals.Settings.Proxy.Username, InternalGlobals.Settings.Proxy.Password.ConvertToUnsecureString());
                            }
                            else
                            {
                                proxy.Credentials = CredentialCache.DefaultCredentials;
                            }

                            client.Proxy = proxy;
                        }
                        catch (UriFormatException)
                        {
                        }
                    }
                }
            }

            return client;
        }

        public static MemoryStream Download(string url, IIndicatesProgress progressIndicator)
        {
            WebClient client = GetDefaultWebClient();

            using (Stream webStream = client.OpenRead(url))
            {
                string progressFormat;

                long lengthOfDownload = GetWebFileLength(url);
                if (lengthOfDownload >= 0)
                {
                    progressFormat = Localization.Promptu.UpdateDownloadProgressFormatWithTotalLength.Replace("[1]", (lengthOfDownload / 1000).ToString(CultureInfo.CurrentCulture));
                }
                else
                {
                    progressFormat = Localization.Promptu.UpdateDownloadProgressFormat;
                }

                MemoryStream memoryStream = new MemoryStream();

                byte[] buffer = new byte[4096];

                int bytesRead = 0;
                int totalBytesRead = 0;

                progressIndicator.StatusMessage = String.Format(CultureInfo.CurrentCulture, progressFormat, totalBytesRead / 1024);
                progressIndicator.ProgressPercentage = 0;

                do
                {
                    bytesRead = webStream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        totalBytesRead += bytesRead;
                        progressIndicator.ProgressPercentage = (int)(((double)totalBytesRead / (double)lengthOfDownload) * 100);
                        progressIndicator.StatusMessage = String.Format(CultureInfo.CurrentCulture, progressFormat, totalBytesRead / 1024);
                        memoryStream.Write(buffer, 0, bytesRead);
                    }
                }
                while (bytesRead > 0);

                return memoryStream;
            }
        }

        private static void PerformUpdate(string packagePath)
        {
            try
            {
                WebClient client = GetDefaultWebClient();

                DownloadProgressDialogPresenter dialog = new DownloadProgressDialogPresenter();
                ProgressIndicator indicator = new ProgressIndicator();
                dialog.NativeInterface.ProgressIndicator = indicator;
                dialog.Show();

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += delegate
                {
                    try
                    {
                        using (MemoryStream packageStream = Download(packagePath, indicator))
                        {
                            dialog.NativeInterface.Invoke(new ParameterlessVoid(delegate
                            {
                                dialog.Close();
                                //string progressFormat;

                                //long lengthOfDownload = GetWebFileLength(packagePath);
                                //if (lengthOfDownload >= 0)
                                //{
                                //    progressFormat = Localization.Promptu.UpdateDownloadProgressFormatWithTotalLength.Replace("[1]", (lengthOfDownload / 1000).ToString());
                                //}
                                //else
                                //{
                                //    progressFormat = Localization.Promptu.UpdateDownloadProgressFormat;
                                //}

                                //MemoryStream packageStream = webStream.ToMemoryStreamVisible(progressFormat, new DownloadProgressDialogPresenter());

                                FileFileDirectory packageRoot = FileFileDirectory.FromContainer(packageStream);

                                string executablePath = Application.ExecutablePath;

                                if (packageRoot.Files.Contains("Promptu.exe"))
                                {
                                    FileSystemFile exeFile = executablePath;
                                    FileSystemFile oldExeFile = executablePath + ".old";
                                    oldExeFile.DeleteIfExists();
                                    exeFile.Rename(exeFile.Name + ".old");
                                }

                                Unpack(packageRoot, Application.StartupPath, true);

                                MessageBox.Show(
                                    Localization.Promptu.UpdateSuccessful,
                                    Localization.Promptu.AppName,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);

                                System.Diagnostics.Process.Start(executablePath, String.Format(CultureInfo.InvariantCulture, "wait-for {0}", System.Diagnostics.Process.GetCurrentProcess().Id));
                                PromptuUtilities.ExitApplication();
                            }), null);
                        }

                        dialog.NativeInterface.Invoke(new ParameterlessVoid(dialog.Close), null);
                    }
                    catch (WebException)
                    {
                        MessageBox.Show(
                            Localization.Promptu.UpdateDownloadError,
                            Localization.Promptu.AppName,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);

                        dialog.NativeInterface.Invoke(new ParameterlessVoid(dialog.Close), null);
                    }
                    catch (FileFileSystemException)
                    {
                        MessageBox.Show(
                            Localization.Promptu.UpdateError,
                            Localization.Promptu.AppName,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);

                        dialog.NativeInterface.Invoke(new ParameterlessVoid(dialog.Close), null);
                    }
                    finally
                    {
                        dialog.NativeInterface.Invoke(new ParameterlessVoid(dialog.Close), null);
                    }
                };

                worker.RunWorkerAsync();
            }
            catch (WebException)
            {
                MessageBox.Show(
                    Localization.Promptu.UpdateDownloadError,
                    Localization.Promptu.AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
            catch (FileFileSystemException)
            {
                MessageBox.Show(
                    Localization.Promptu.UpdateError,
                    Localization.Promptu.AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }
        }

        public static void Unpack(FileFileDirectory directory, FileSystemDirectory physicalDirectory, bool deletePrevious)
        {
            Dictionary<string, string> meta = new Dictionary<string, string>();
            if (directory.Files.Contains(".meta"))
            {
                using (StreamReader reader = new StreamReader(directory.Files[".meta"].Contents))
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        string[] split = line.Split(' ');
                        if (split.Length > 1)
                        {
                            string split0ToUpper = split[0].ToUpperInvariant();
                            switch (split0ToUpper)
                            {
                                case "DELETE":
                                    FileSystemFile fileToDelete = physicalDirectory + split[1];
                                    fileToDelete.DeleteIfExists();
                                    break;
                                case "NO-OVERWRITE":
                                    meta.Add(split[1], split0ToUpper);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }


            foreach (FileFileDirectory child in directory.Directories)
            {
                FileSystemDirectory fileSystemDirectory = physicalDirectory.CreateDirectory(child.Name);
                Unpack(child, fileSystemDirectory, deletePrevious);
            }

            foreach (FileFile file in directory.Files)
            {
                if (file.Name.ToUpperInvariant() == ".META")
                {
                    continue;
                }

                string instructions;
                bool found = meta.TryGetValue(file.Name, out instructions);

                if (found && instructions != null)
                {
                    switch (instructions)
                    {
                        case "NO-OVERWRITE":
                            if (((FileSystemFile)(physicalDirectory + file.Name)).Exists)
                            {
                                continue;
                            }

                            break;
                        default:
                            break;
                    }
                }

                if (deletePrevious)
                {
                    FileSystemFile oldFile = physicalDirectory + file.Name;
                    if (oldFile.Exists)
                    {
                        try
                        {
                            oldFile.Delete();
                        }
                        catch (UnauthorizedAccessException)
                        {
                            string newName = physicalDirectory.GetAvailableFileName(file.Name + "{+}.deleteme", "({number})", InsertBase.Two);
                            oldFile.Rename(newName);
                        }
                    }
                }

                file.SaveIn(physicalDirectory);
            }
        }

        public static long GetWebFileLength(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "HEAD";
            WebResponse response = request.GetResponse();
            return response.ContentLength;
        }

        private static bool UpdateIsAvailable(out string packagePath, bool throwWebException)
        {
            packagePath = null;

            //try
            // {
            WebClient client = GetDefaultWebClient();
            //Stream releaseFile = client.OpenRead("http://localhost/Promptu.auinfo.txt");

            ReleaseVersion latestVersion = null;

            // TODO test this
            if (InternalGlobals.CurrentReleaseType != ReleaseType.Release || InternalGlobals.Settings.CheckForPreReleaseUpdates)
            {
                string prereleasePackagePath;
                ReleaseVersion prereleaseVersion = ReadReleaseVersion(
                    client,
#if LOCALUPDATE
                    "http://localhost/promptu/Promptu_prerelease.auinfo",
#else
                    "http://www.PromptuLauncher.com/meta/Promptu_prerelease.auinfo",
#endif
                    false,
                    out prereleasePackagePath);

                string regularPackagePath;
                ReleaseVersion regularReleaseVersion = ReadReleaseVersion(
                    client,
#if LOCALUPDATE
                    "http://localhost/promptu/Promptu.auinfo",
#else
                    "http://www.PromptuLauncher.com/meta/Promptu.auinfo",
#endif
                    throwWebException,
                    out regularPackagePath);

                if (prereleaseVersion != null)
                {
                    if (regularReleaseVersion != null)
                    {
                        if (prereleaseVersion.IsAfter(regularReleaseVersion))
                        {
                            latestVersion = prereleaseVersion;
                            packagePath = prereleasePackagePath;
                        }
                        else
                        {
                            latestVersion = regularReleaseVersion;
                            packagePath = regularPackagePath;
                        }
                    }
                    else
                    {
                        latestVersion = prereleaseVersion;
                        packagePath = prereleasePackagePath;
                    }
                }
                else
                {
                    latestVersion = regularReleaseVersion;
                    packagePath = regularPackagePath;
                }
            }
            else
            {
                latestVersion = ReadReleaseVersion(
                    client,
#if LOCALUPDATE
                    "http://localhost/promptu/Promptu.auinfo",
#else
                    "http://www.PromptuLauncher.com/meta/Promptu.auinfo",
#endif
                    throwWebException,
                    out packagePath);
            }
            //#if BETA
            //                Stream releaseFile;

            //                try
            //                {
            //                    releaseFile = client.OpenRead("http://www.PromptuLauncher.com/meta/Promptu_0.8_beta.auinfo");
            //                }
            //                catch (WebException)
            //                {
            //                    releaseFile = client.OpenRead("http://www.PromptuLauncher.com/meta/Promptu.auinfo");
            //                }
            //#else
            //                Stream releaseFile = client.OpenRead("http://www.PromptuLauncher.com/meta/Promptu.auinfo");
            //#endif
            //                StreamReader reader = new StreamReader(releaseFile);
            //                string file = reader.ReadToEnd();
            //                //XmlDocument document = new XmlDocument();
            //                //document.LoadXml(file);
            //                ReleaseVersion latestVersion = null;
            ReleaseVersion runningVersion = InternalGlobals.CurrentPromptuVersion;

            //string[] split = file.Split('|');

            //if (split.Length > 1)
            //{
            //    latestVersion = new ReleaseVersion(split[0]);
            //    packagePath = split[1];
            //}


            if (latestVersion == null)
            {
                return false;
            }

            //MessageBox.Show("Successful connect.");

            if (latestVersion.IsAfter(runningVersion))
            {
                return true;
            }
            //}
            //catch (WebException)
            //{
            //    if (throwWebException)
            //    {
            //        throw;
            //    }
            //}
            //catch (FormatException)
            //{
            //}

            return false;
        }

        public static void CheckForPluginUpdatesIfAllowed(bool shouldRestart)
        {
            FileSystemDirectory settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FileSystemDirectory promptuDirectory = settingsDirectory + "Promptu";
            FileSystemFile lastPluginUpdateInteractionFile = promptuDirectory + "LastPluginUpdateInteraction";

            DateTime? lastPluginUpdateInteraction = null;

            if (lastPluginUpdateInteractionFile.Exists)
            {
                try
                {
                    lastPluginUpdateInteraction = DateTime.Parse(lastPluginUpdateInteractionFile.ReadAllText(), CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                }
            }

            if (lastPluginUpdateInteraction == null || lastPluginUpdateInteraction.Value < DateTime.Now.AddDays(-10))
            {
                CheckForPluginUpdates(shouldRestart, true);
            }
        }

        public static void CheckForPluginUpdates(bool shouldRestart, bool isAutoUpdate)
        {
            CheckForPluginUpdates(shouldRestart, isAutoUpdate, null);
        }

        public static void CheckForPluginUpdates(bool shouldRestart, bool isAutoUpdate, ParameterlessVoid finishedPrelimiaryCheck)
        {
            WebClient client = GetDefaultWebClient();
            List<PromptuPlugin> pluginsToCheck = new List<PromptuPlugin>();

            using (DdMonitor.Lock(InternalGlobals.AvailablePluginsLock))
            {
                foreach (string id in InternalGlobals.CurrentProfile.PluginMeta)
                {
                    PromptuPlugin plugin = InternalGlobals.AvailablePlugins.TryGet(id);
                    if (plugin != null)
                    {
                        pluginsToCheck.Add(plugin);
                    }
                }
            }

            List<PluginUpdate> availableUpdates = new List<PluginUpdate>();

            foreach (PromptuPlugin plugin in pluginsToCheck)
            {
                string packagePath;
                ReleaseVersion latestUpdateVersion;

                if (plugin.UpdateUrl == null)
                {
                    continue;
                }

                try
                {
                    latestUpdateVersion = ReadReleaseVersionPlugin(
                       client,
                       plugin.UpdateUrl,
                       true,
                       out packagePath);
                }
                catch (WebException ex)
                {
                    ErrorConsole.WriteLine(plugin.Id, String.Format(CultureInfo.CurrentCulture, "Exception while checking for update.  Message: {0}", ex.Message));
                    continue;
                }

                if (latestUpdateVersion == null)
                {
                    continue;
                }

                if (latestUpdateVersion.IsAfter(new ReleaseVersion(plugin.Version)))
                {
                    availableUpdates.Add(new PluginUpdate(plugin, packagePath, latestUpdateVersion));
                }
            }

            if (finishedPrelimiaryCheck != null)
            {
                finishedPrelimiaryCheck();
            }

            if (availableUpdates.Count > 0)
            {
                FileSystemDirectory settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                FileSystemDirectory promptuDirectory = settingsDirectory + "Promptu";
                FileSystemFile lastPluginUpdateInteractionFile = promptuDirectory + "LastPluginUpdateInteraction";

                ParameterlessVoid askUserAction = new ParameterlessVoid(delegate
                    {
                        PluginUpdateDialogPresenter updateDialog = new PluginUpdateDialogPresenter(availableUpdates);
                        updateDialog.ShowDialog();
                        lastPluginUpdateInteractionFile.WriteAllText(DateTime.Now.ToString());

                        if (shouldRestart)
                        {
                            UIMessageBox.Show(
                                Localization.Promptu.PluginUpdateSuccessful,
                                Localization.Promptu.AppName,
                                UIMessageBoxButtons.OK,
                                UIMessageBoxIcon.Information,
                                UIMessageBoxResult.OK);

                            string executablePath = Application.ExecutablePath;
                            System.Diagnostics.Process.Start(executablePath, String.Format(CultureInfo.InvariantCulture, "wait-for {0}", System.Diagnostics.Process.GetCurrentProcess().Id));
                            PromptuUtilities.ExitApplication();
                        }
                    });

                PromptHandler handler = PromptHandler.GetInstance();
                if (handler.InvokeOnMainThreadRequired)
                {
                    handler.InvokeOnMainThread(askUserAction);
                }
                else
                {
                    askUserAction();
                }
            }
            else if (!isAutoUpdate)
            {
                UIMessageBox.Show(
                    Localization.UIResources.NoPluginUpdatesMessage,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Information,
                    UIMessageBoxResult.OK);
            }
        }

        private static ReleaseVersion ReadReleaseVersionPlugin(WebClient client, string url, bool throwWebExceptions, out string packagePath)
        {
            packagePath = null;

            try
            {
                using (Stream releaseFile = client.OpenRead(url))
                using (StreamReader reader = new StreamReader(releaseFile))
                {
                    string file = reader.ReadToEnd();
                    ReleaseVersion latestVersion = null;

                    string[] split = file.Split('|');

                    if (split.Length > 1)
                    {
                        latestVersion = new ReleaseVersion(split[0]);
                        packagePath = split[1];

                        if (split.Length > 2)
                        {
                            string minVersion = split[2];

                            if (minVersion.StartsWith("minVersion=", StringComparison.InvariantCultureIgnoreCase))
                            {
                                minVersion = minVersion.Substring(11);

                                try
                                {
                                    ReleaseVersion minPromptuVersion = new ReleaseVersion(minVersion);

                                    if (minPromptuVersion.IsAfter(InternalGlobals.CurrentPromptuVersion))
                                    {
                                        return null;
                                    }
                                }
                                catch (FormatException)
                                {
                                    return null;
                                }
                            }
                        }
                    }

                    return latestVersion;
                }
            }
            catch (WebException)
            {
                if (throwWebExceptions)
                {
                    throw;
                }
            }
            catch (FormatException)
            {
            }

            return null;
        }

        private static ReleaseVersion ReadReleaseVersion(WebClient client, string url, bool throwWebExceptions, out string packagePath)
        {
            packagePath = null;

            try
            {
                using (Stream releaseFile = client.OpenRead(url))
                using (StreamReader reader = new StreamReader(releaseFile))
                {
                    string file = reader.ReadToEnd();
                    ReleaseVersion latestVersion = null;

                    string[] split = file.Split('|');

                    if (split.Length > 1)
                    {
                        latestVersion = new ReleaseVersion(split[0]);
                        packagePath = split[1];
                    }

                    return latestVersion;
                }
            }
            catch (WebException)
            {
                if (throwWebExceptions)
                {
                    throw;
                }
            }
            catch (FormatException)
            {
            }

            return null;
        }
    }
}
