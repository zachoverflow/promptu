using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using ZachJohnson.Promptu.UI;
using System.Extensions;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.Collections;
using System.Text;
using ZachJohnson.Promptu.Skins;
using System.Reflection;
using ZachJohnson.Promptu.UIModel;
using ZachJohnson.Promptu.UserModel.Collections;
using ZachJohnson.Promptu.UIModel.Interfaces;
using System.IO;
using ZachJohnson.Promptu.UIModel.Presenters;
using Microsoft.Win32;
using System.Xml;
using System.Diagnostics;
using System.Globalization;

namespace ZachJohnson.Promptu
{
    static class Program
    {
        private const int SplashScreenDisplayLength = 1000;
        private static ManualResetEvent splashScreenClosed = new ManualResetEvent(false);
#if CUSTOMTRACE
        private static CustomTraceListener traceListener;
#endif
        //private static IActivatable dialogToActivateAfterSplashScreen;

        [STAThread]
        static void Main(string[] args)
        {
            //double result = Calculator.RegexCalc.Evaluate("sin(pi)");
            //string test = SuggestionUtilities.ReplaceCommandParameter("this \"is\" \"a\" \"test\" ", 2, "\"test\"", "&?;");
            //string replaced = "test%appdata2%appdata".ExpandEnvironmentVariables();
            //StringExtensions.TestBreakApart();

            //foreach (Process process in Process.GetProcessesByName("Promptu"))
            //{
            //    process.Kill();
            //}
            

            int? processIDToWaitFor = null;
            string listOfCommandToEdit = null;
            Id idOfCommandToEdit = null;
            string commandContentsPath = null;

            bool validateRuntime = true;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpperInvariant())
                {
                    case "WAIT-FOR":
                        i++;
                        if (i < args.Length)
                        {
                            try
                            {
                                processIDToWaitFor = Convert.ToInt32(args[i], CultureInfo.InvariantCulture);
                            }
                            catch (OverflowException)
                            {
                            }
                            catch (FormatException)
                            {
                            }
                        }

                        break;
                    case "EDIT":
                        i++;
                        if (i + 1 < args.Length)
                        {
                            string[] split = args[i].Trim().Split(':');

                            if (split.Length == 2)
                            {
                                listOfCommandToEdit = split[0];

                                if (listOfCommandToEdit != null)
                                {
                                    if (split[1].ToUpperInvariant() != "NEW")
                                    {
                                        try
                                        {
                                            idOfCommandToEdit = new Id(Convert.ToInt32(split[1]));
                                        }
                                        catch (FormatException)
                                        {
                                        }
                                        catch (OverflowException)
                                        {
                                        }

                                        if (idOfCommandToEdit == null)
                                        {
                                            listOfCommandToEdit = null;
                                        }
                                    }
                                }
                            }

                            i++;
                            commandContentsPath = args[i];
                        }

                        break;
                    case "WITHOUT-RUNTIME-VALIDATION":
                        validateRuntime = false;
                        break;
                    default:
                        break;
                }
            }

            if (validateRuntime)
            {
                ValidateRuntime();
            }
            //FileFileSystem.FileFileDirectory dir = FileFileSystem.FileFileDirectory.FromContainer(@"F:\djfiles\Family\Computer\Promptu\Family_Shared.pdc");
            //Shortcut shortcut = Shortcut.FromFile("C:\\Users\\Admin\\Desktop\\Shortcuts\\a.lnk");
            //string oldPath = shortcut.Target;
            //shortcut.Target = "%SystemRoot%\\system32\\notepad.exe";
            //shortcut.Save();
            //Shortcut shortcut = new Shortcut("C:\\Users\\Admin\\Desktop\\Shortcuts\\a.lnk", "%SystemRoot%\\system32\\notepad.exe");
            //shortcut.Save();

            //string s = DateTime.Now.ToFourChars();

            //bool systemEventsOn = false;

            //try
            //{
            //    systemEventsOn = Convert.ToBoolean(ConfigurationManager.AppSettings["Debug"]);
            //}
            //catch (FormatException)
            //{
            //}

            if (processIDToWaitFor != null)
            {
                try
                {
                    System.Diagnostics.Process processToWaitFor = System.Diagnostics.Process.GetProcessById(processIDToWaitFor.Value);
                    processToWaitFor.WaitForExit();
                }
                catch (ArgumentException)
                {
                }
            }

            ReleaseVersion oldVersion = null;//new ReleaseVersion("0.7.5.5");

            try
            {
                FileSystemFile oldExeFile = Application.ExecutablePath + ".old";
                try
                {
                    AssemblyName oldExeName = AssemblyName.GetAssemblyName(oldExeFile);

                    oldVersion = new ReleaseVersion(oldExeName.Version);
                }
                catch (ArgumentException)
                {
                }
                catch (System.IO.FileNotFoundException)
                {
                }
                catch (System.Security.SecurityException)
                {
                }
                catch (BadImageFormatException)
                {
                }
                catch (System.IO.FileLoadException)
                {
                }

                oldExeFile.DeleteIfExists();
            }
            catch (UnauthorizedAccessException)
            {
            }

            FileSystemDirectory startupPath = (FileSystemDirectory)Application.StartupPath;

            foreach (FileSystemFile file in ((FileSystemDirectory)(startupPath + "\\Common\\")).GetFiles())
            {
                if (file.Extension.ToUpperInvariant() == ".DELETEME")
                {
                    try
                    {
                        file.DeleteIfExists();
                    }
                    catch (IOException)
                    {
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                }
            }

            //TODO remove after all updated
            try
            {
                FileSystemFile oldProxyFile = startupPath + "\\Config\\ProxySettings";
                oldProxyFile.DeleteIfExists();
            }
            catch (UnauthorizedAccessException)
            {
            }

            //Assembly wpfTest = Assembly.LoadFrom(@"C:\zjfiles\Projects\tmp\WpfSkinTest\WpfSkinTest\bin\Debug\WpfSkinTest.dll");
            //Type tkHostType = wpfTest.GetType("WpfSkinTest.WpfToolkitHost");

            //ToolkitHost tkHost = (ToolkitHost)Activator.CreateInstance(tkHostType);


            //Globals.GuiManager = new GuiManager(tkHost);
            //MessageBox.Show("About to load");
            InternalGlobals.GuiManager = new GuiManager(LoadToolkitHost());
            //MessageBox.Show("Loaded.");
            InternalGlobals.GuiManager.ToolkitHost.InitializeToolkit();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //SystemEvents.On = systemEventsOn;

            //Skins.Default.DefaultSuggester ds = new ZachJohnson.Promptu.Skins.Default.DefaultSuggester();
            //ds.ShowDialog();

            //new CollisionResolvingDialog(new List<ZachJohnson.Promptu.UserModel.Differencing.DiffDiffBase>()).ShowDialog();

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            
            //foreach (ManagementObject o in searcher.Get())
            //{
            //    Dictionary<string, object> properties = new Dictionary<string, object>();
            //    foreach (PropertyData data in o.Properties)
            //    {
            //        properties.Add(data.Name, data.Value);
            //    }
            //}

            //WindowsROverride.TEST();
#if CUSTOMTRACE
            traceListener = new CustomTraceListener(((FileSystemDirectory)Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + "Promptu\\trace.txt");
            System.Diagnostics.Trace.Listeners.Add(traceListener);
#endif
#if !DEBUG
            //Application.ThreadException += HandleUnhandledException;

            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            //throw new ArgumentException("blah");
#else
            //CustomTraceListener traceListener = new CustomTraceListener(((FileSystemDirectory)Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + "Promptu\\trace.txt");
            //System.Diagnostics.Trace.Listeners.Add(traceListener);
#endif
            bool isFirstInstance;
#if DEBUG
            using (Mutex mtx = new Mutex(true, "Promptu-debug", out isFirstInstance))
#else
            using (Mutex mtx = new Mutex(true, "Promptu", out isFirstInstance))
#endif
            {
                if (isFirstInstance)
                {
                    try
                    {
                        //new OverwriteDialog().Show();
                        using (NotifyIconPresenter notifyIconPresenter = BoostrapPromptu(oldVersion))
                        {
                            notifyIconPresenter.NotifyIcon.Show();

                            if (listOfCommandToEdit != null)
                            {
                                List list = InternalGlobals.CurrentProfile.Lists.TryGet(listOfCommandToEdit);

                                if (list != null && commandContentsPath != null)
                                {
                                    PromptHandler.GetInstance().InvokeOnMainThreadAsync(new ParameterlessVoid(delegate
                                        {
                                            XmlDocument document = new XmlDocument();
                                            Command commandContents = null;

                                            try
                                            {
                                                document.Load(commandContentsPath);
                                                if (document.ChildNodes.Count > 0)
                                                {
                                                    commandContents = Command.FromXml(document.ChildNodes[0]);
                                                }
                                            }
                                            catch (XmlException)
                                            {
                                            }
                                            //catch (IOException)
                                            //{
                                            //}
                                            //catch (Load

                                            File.Delete(commandContentsPath);

                                            if (commandContents == null)
                                            {
                                                return;
                                            }

                                            if (idOfCommandToEdit != null)
                                            {
                                                Command command = list.Commands.TryGet(idOfCommandToEdit);
                                                if (command != null)
                                                {
                                                    PromptHandler.GetInstance().SetupDialog.Show();
                                                    PromptHandler.GetInstance().SetupDialog.EditCommand(command.Name, list, commandContents);
                                                }
                                            }
                                            else
                                            {
                                                PromptHandler.GetInstance().SetupDialog.Show();
                                                PromptHandler.GetInstance().SetupDialog.EditNewCommand(list, commandContents);
                                            }
                                        }));
                                }
                            }

                            //Updater.CheckForPluginUpdates(false);
                            //InternalGlobals.GuiManager.ToolkitHost.MainThreadDispatcher.Invoke(new ParameterlessVoid(delegate { UIMessageBox.Show("I'm ok!", "Promptu", UIMessageBoxButtons.OK, UIMessageBoxIcon.None); }), null);
                            ////TrayIcon notificationIcon = new TrayIcon(oldVersion);
                            ////notificationIcon.Visible = true;
                            //new DownloadProgressDialog().Show();
                            //new OverwriteDialog().Show();
                            InternalGlobals.GuiManager.ToolkitHost.StartMessageLoop();
                        }
                        ///notificationIcon.Dispose();
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        UIMessageBox.Show(
                            String.Format(CultureInfo.CurrentCulture, Localization.MessageFormats.UnauthorizedAccessExceptionGlobalCatch, ex.Message),
                            Localization.Promptu.AppName,
                            UIMessageBoxButtons.OK,
                            UIMessageBoxIcon.Error,
                            UIMessageBoxResult.OK);
                    }
                }
                else
                {
                    UIMessageBox.Show(
                        Localization.Promptu.AppAlreadyStartedMessage,
                        Localization.Promptu.AppName,
                        UIMessageBoxButtons.OK,
                        UIMessageBoxIcon.Information,
                        UIMessageBoxResult.OK);
                }
            }

#if CUSTOMTRACE
            traceListener.Dispose();
#endif
        }

        private static void ValidateRuntime()
        {
            if (Environment.Version.Major < 4)
            {
                bool restartWithDotNet4 = true;
                
                RegistryKey clientKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP\v4\Client\");
                object clientProfileInstalled = clientKey == null ? null : clientKey.GetValue("Install");

                //MessageBox.Show("cprofile: " + clientProfileInstalled == null ? "null" : clientProfileInstalled.ToString());

                if (clientProfileInstalled == null || (int)clientProfileInstalled != 1)
                {
                    RegistryKey fullVersionKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP\v4\Full");
                    object fullVersionInstalled = fullVersionKey == null ? null : fullVersionKey.GetValue("Install");

                    //MessageBox.Show("full: " + fullVersionInstalled == null ? "null" : fullVersionInstalled.ToString());

                    if (fullVersionInstalled == null || (int)fullVersionInstalled != 1)
                    {
                        restartWithDotNet4 = false;
                        if (MessageBox.Show(
                            Localization.Promptu.PromptuNeedsDotNet4,
                            Localization.Promptu.AppName,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start("http://www.promptulauncher.com/get-dotnet");
                            }
                            catch (System.ComponentModel.Win32Exception)
                            {
                                MessageBox.Show(
                                    Localization.Promptu.CouldNotVisitDotNet4Download,
                                    Localization.Promptu.AppName,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }

                        Environment.Exit(0);
                    }
                }

                if (restartWithDotNet4)
                {
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    string exePath = new Uri(executingAssembly.CodeBase).LocalPath;
                    //FileSystemDirectory folder = Path.GetDirectoryName(executingAssembly.CodeBase);
                    FileSystemFile configFile = exePath + ".config";

                    bool createNew = true;

                    if (configFile.Exists)
                    {
                        try
                        {
                            XmlDocument config = new XmlDocument();
                            config.Load(configFile);

                            foreach (XmlNode root in config.ChildNodes)
                            {
                                if (root.Name.ToUpperInvariant() == "CONFIGURATION")
                                {
                                    createNew = false;

                                    bool addedKey = false;

                                    foreach (XmlNode innerNode in root.ChildNodes)
                                    {
                                        if (innerNode.Name.ToUpperInvariant() == "STARTUP")
                                        {
                                            XmlNode supportedRuntimeNode = config.CreateElement("supportedRuntime");
                                            supportedRuntimeNode.Attributes.Append(XmlUtilities.CreateAttribute("version", "v4.0", config));
                                            innerNode.AppendChild(supportedRuntimeNode);
                                            addedKey = true;
                                        }
                                    }

                                    if (!addedKey)
                                    {
                                        XmlNode startupNode = config.CreateElement("startup");
                                        XmlNode supportedRuntimeNode = config.CreateElement("supportedRuntime");
                                        supportedRuntimeNode.Attributes.Append(XmlUtilities.CreateAttribute("version", "v4.0", config));
                                        startupNode.AppendChild(supportedRuntimeNode);
                                        root.AppendChild(startupNode);
                                    }
                                }
                            }

                            config.Save(configFile);
                        }
                        catch (XmlException)
                        {
                        }
                        catch (IOException)
                        {
                        }
                    }

                    if (createNew)
                    {
                        File.WriteAllText(configFile, @"<?xml version=""1.0""?>
<configuration>
  <startup>
	<supportedRuntime version=""v4.0""/>
  </startup>
</configuration>");
                    }

                    ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;

                    //if (String.IsNullOrEmpty(startInfo.FileName))
                    //{
                    startInfo.FileName = exePath;
                    startInfo.Arguments = startInfo.Arguments + " without-runtime-validation";
                    //}

                    Process.Start(startInfo);
                    Environment.Exit(0);
                }
            }
        }

        private static ToolkitHost LoadToolkitHost()
        {
            //return (ToolkitHost)ZachJohnson.Promptu.WpfUI.Temp.CreateTKH();
            //// TODO add error reporting code
            Assembly uiAssembly = Assembly.LoadFrom(Application.StartupPath + "\\Common\\Promptu.WpfUI.dll");
            Type toolkitHostType = uiAssembly.GetType("ZachJohnson.Promptu.WpfUI.WpfToolkitHost");

            InternalGlobals.ToolkitAssembly = uiAssembly;

            return (ToolkitHost)Activator.CreateInstance(toolkitHostType);
        }

        private static UIDialogResult ShowDialogAfterSplashScreen<T>(DialogPresenterBase<T> dialogPresenter) where T : IDialog
        {
            //dialogToActivateAfterSplashScreen = dialogPresenter.NativeInterface as IActivatable;
            splashScreenClosed.WaitOne();
            return dialogPresenter.ShowDialog();
        }

        //private static void HandleSplashScreenAboutToClose(object sender, EventArgs e)
        //{
        //    ((ISplashScreen)sender).Closing -= HandleSplashScreenAboutToClose;
        //    IActivatable dialog = dialogToActivateAfterSplashScreen;
        //    if (dialog == null)
        //    {
        //        dialog.Activate();
        //    }
        //}

        private static NotifyIconPresenter BoostrapPromptu(ReleaseVersion oldVersion)
        {
            FileSystemDirectory startupPath = Application.StartupPath;

            NotifyIconPresenter notifyIconPresenter = new NotifyIconPresenter();
            InternalGlobals.NotifyIcon = notifyIconPresenter;

            InternalGlobals.LoadPlugins();
            InternalGlobals.AssemblyCache = new ZachJohnson.Promptu.AssemblyCaching.AssemblyCache(startupPath + "AssemblyCache");

            InternalGlobals.ProfilePlacemarks = ProfilePlacemarkCollection.FromFolder(startupPath + "Profiles", false);

            if (oldVersion != null)
            {
                foreach (ProfilePlacemark placemark in InternalGlobals.ProfilePlacemarks)
                {
                    FileSystemFile upgradedFromVersionFile = placemark.Directory + "upgradedFrom.version";
                    if (!upgradedFromVersionFile.Exists)
                    {
                        upgradedFromVersionFile.WriteAllText(oldVersion.ToString());
                    }
                }
            }

            UserCookie cookie = UserCookie.Load();
            bool ignoreLocked = false;

            if (String.IsNullOrEmpty(cookie.ProfileId) || !InternalGlobals.ProfilePlacemarks.Contains(cookie.ProfileId) || InternalGlobals.ProfilePlacemarks[cookie.ProfileId].ShowSplashScreen)
            {
                ISplashScreen splashScreen = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructSplashScreen();
                //splashScreen.Closing += HandleSplashScreenAboutToClose;
                splashScreen.Show(SplashScreenDisplayLength, splashScreenClosed);
            }
            else
            {
                splashScreenClosed.Set();
            }

            InternalGlobals.Skins = new SkinCollection();
            InternalGlobals.Skins.AddRange(InternalGlobals.GuiManager.ToolkitHost.GetDefaultSkins());

            if (String.IsNullOrEmpty(cookie.ProfileId) || !InternalGlobals.ProfilePlacemarks.Contains(cookie.ProfileId))
            {
                while (true)
                {
                    NewUserDialogPresenter newUserDialog = new NewUserDialogPresenter(NewUserDialogContext.Normal);//NewUserDialogPresenter.FromNormal();

                    if (ShowDialogAfterSplashScreen(newUserDialog) != UIDialogResult.OK)
                    {
                        Environment.Exit(0);
                    }

                    if (newUserDialog.NewUserAction == NewUserAction.UseExistingProfile)
                    {
                        cookie.ProfileId = newUserDialog.SelectedProfile.FolderName;
                        cookie.Save();

                        // 2010-08-02 removed prompt to autostart
                    }
                    else
                    {
                        ignoreLocked = true;
                        Profile newProfile = Profile.CreateNewFromUI();

                        //NewProfileDialogPresenter newProfileDialogPresenter = Globals.GuiManager.Factory.CreateNewProfileDialog(newProfile, true);
                        //if (newProfileDialogPresenter.ShowDialog() != UIDialogResult.OK)
                        //{
                        //    Globals.SyncSynchronizer.CancelSyncsAndWait();
                        //    Globals.ProfilePlacemarks.Remove(newProfile.FolderName);
                        //    newProfile.Directory.Delete();
                        //    continue;
                        //}

                        newProfile.Name = newUserDialog.ProfileName;
                        newUserDialog.HotkeyPresenter.ImpartTo(newProfile.Hotkey);
                        newProfile.PromptPositioningMode = newUserDialog.PromptPositioningMode;

                        InternalGlobals.AutoStartWithComputer = newUserDialog.AutostartPromptu;

                        //newProfile.Hotkey.SwitchHotkey(profileSetupDialog.HotkeySetupPanel.HotkeyModifierKeys, profileSetupDialog.HotkeySetupPanel.UnderlyingHotkeyKeyValue);
                        newProfile.SaveAll();

                        cookie.ProfileId = newProfile.FolderName;
                        cookie.Save();
                    }

                    break;
                }
            }

            SafeLoadToCurrentProfile(cookie.ProfileId);

            cookie.ProfileId = InternalGlobals.CurrentProfile.FolderName;
            cookie.Save();

            if (!ignoreLocked && InternalGlobals.CurrentProfile.IsExternallyLocked)
            {
                while (true)
                {
                    //bool createNewProfile = true;
                    //NewUserDialog dialog = new NewUserDialog(true, true, true);
                    NewUserDialogPresenter newUserDialog = new NewUserDialogPresenter(NewUserDialogContext.LockedProfile);//NewUserDialogPresenter.FromLocked();//new NewUserDialogPresenter(true, true, true);
                    //dialogPresenter.SetModeToLockedProfile();

                    if (ShowDialogAfterSplashScreen(newUserDialog) != UIDialogResult.OK)
                    {
                        Environment.Exit(0);
                    }

                    if (newUserDialog.NewUserAction == NewUserAction.UseExistingProfile)
                    {
                        //createNewProfile = false;
                        cookie.ProfileId = newUserDialog.SelectedProfile.FolderName;
                        cookie.Save();

                        //PromptUserToAutoStart();
                    }
                    else
                    {
                        ignoreLocked = true;
                        Profile newProfile = Profile.CreateNewFromUI();

                        //NewProfileDialogPresenter newProfileDialogPresenter = Globals.GuiManager.Factory.CreateNewProfileDialog(newProfile, true);
                        //if (newProfileDialogPresenter.ShowDialog() != UIDialogResult.OK)
                        //{
                        //    Globals.SyncSynchronizer.CancelSyncsAndWait();
                        //    Globals.ProfilePlacemarks.Remove(newProfile.FolderName);
                        //    newProfile.Directory.Delete();
                        //    continue;
                        //}

                        //newProfile.Name = newProfileDialogPresenter.ProfileName;
                        //newProfileDialogPresenter.HotkeyPresenter.ImpartTo(newProfile.Hotkey);
                        //newProfile.Hotkey.SwitchHotkey(profileSetupDialog.HotkeySetupPanel.HotkeyModifierKeys, profileSetupDialog.HotkeySetupPanel.UnderlyingHotkeyKeyValue);
                        newProfile.Name = newUserDialog.ProfileName;
                        newUserDialog.HotkeyPresenter.ImpartTo(newProfile.Hotkey);
                        newProfile.PromptPositioningMode = newUserDialog.PromptPositioningMode;

                        InternalGlobals.AutoStartWithComputer = newUserDialog.AutostartPromptu;

                        newProfile.SaveAll();

                        cookie.ProfileId = newProfile.FolderName;
                        cookie.Save();
                        //PromptUserToAutoStart();
                    }

                    SafeLoadToCurrentProfile(cookie.ProfileId);
                    cookie.ProfileId = InternalGlobals.CurrentProfile.FolderName;
                    cookie.Save();
                    break;
                }
            }

            InternalGlobals.CurrentProfile.UpdateLockFile();
            InternalGlobals.CurrentProfile.BackgroundWorkQueue.Pause();
            InternalGlobals.CurrentProfile.BackgroundWorkQueue.WaitUntilPaused();

            FileSystemFile oldVersionFile = InternalGlobals.CurrentProfile.Directory + "upgradedFrom.version";
            if (oldVersionFile.Exists)
            {
                try
                {
                    oldVersion = new ReleaseVersion(oldVersionFile.ReadAllText());
                }
                catch (FormatException)
                {
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }

                if (oldVersion != null)
                {
                    if (oldVersion.Major == 0 && oldVersion.Minor < 9)
                    {
                        FileSystemFile pluginsFile = InternalGlobals.CurrentProfile.Directory + "Plugins.xml";
                        //try
                        //{
                            pluginsFile.WriteAllText(Localization.Promptu.DefaultPluginXml);
                        //}
                        //catch (IOException)
                        //{
                        //}
                        //    if (InternalGlobals.RunningOS == OS.Windows)
                        //    {
                        //        //PromptuUpgradeWelcomeDialog welcomeDialog = new PromptuUpgradeWelcomeDialog();
                        //        //welcomeDialog.ShowDialog();

                        //    }
                    }

                    InternalGlobals.CurrentProfile.PluginMeta.LoadIds();
                    Updater.CheckForPluginUpdates(false, true);

                    oldVersionFile.Delete();
                }
            }

            // PromptuSettings.CurrentProfile.SyncLocalAssemblyReferences();

            InternalGlobals.CurrentProfile.SyncLocalAssemblyReferences();
            PromptHandler promptHandler = PromptHandler.GetInstance();

            InternalGlobals.CurrentProfile.BackgroundWorkQueue.Unpause();

            InternalGlobals.NotifyIcon.IntializePostProfileLoad();

            //this.notifyIcon = new NotifyIcon();
            //this.notifyIcon.Icon = Icons.ApplicationIcon;
            //this.notifyIcon.MouseClick += this.IconClick;
            //this.notifyIcon.MouseDoubleClick += this.IconDoubleClick;
            //this.notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            //this.notifyIcon.ContextMenuStrip.Items.AddRange(this.InitializeMenu());

            //this.promptHandler.ListsChanged += this.HandleListsChanged;
            // this.notifyIconToolTip = new ToolTip();

            //this.UpdateTrayIconText();

            InternalGlobals.SyncAllowed = true;

            //System.Threading.Thread syncThread = new System.Threading.Thread(PromptuSettings.CurrentProfile.AutoRetrySync);
            //syncThread.IsBackground = true;
            //syncThread.Start();
            InternalGlobals.AssemblyCache.ClearAllUnusedIfNotDoneAlready();
            InternalGlobals.CurrentProfile.AutoRetrySyncAsync();

            Updater.CheckForUpdateAsyncAutoUpdate();
            InternalGlobals.CurrentProfile.UpdateAllCachedIconsAsync(false);
            //Experimental.Test();

            //System.Threading.Thread updateCheckThread = new System.Threading.Thread(this.CheckForUpdate);
            //updateCheckThread.IsBackground = true;
            //updateCheckThread.Start();

            return notifyIconPresenter;

            //while (System.Environment.TickCount < ticksWanted)
            //{
            //}

            //splashScreenHandler.CloseSplashScreen();
            //this.notifyIconToolTip.SetToolTip(this.notifyIcon., "Promptu");

            // */
        }

        [Obsolete]
        private static void PromptUserToAutoStart()
        {
            if (!InternalGlobals.AutoStartWithComputer)
            {
                if (UIMessageBox.Show(
                    Localization.Promptu.StartWithComputer,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.YesNo,
                    UIMessageBoxIcon.Information,
                    UIMessageBoxResult.Yes) == UIMessageBoxResult.Yes)
                {
                    InternalGlobals.AutoStartWithComputer = true;
                }
            }
        }

        private static void SafeLoadToCurrentProfile(string profileFolderName)
        {
            if (profileFolderName == null)
            {
                throw new ArgumentNullException("profileFolderName");
            }

            while (true)
            {
                ProfilePlacemark profilePlacemark = InternalGlobals.ProfilePlacemarks[profileFolderName];

                try
                {
                    InternalGlobals.CurrentProfile = profilePlacemark.GetEntireProfile();
                    break;
                }
                catch (ProfileLoadException)
                {
                    InternalGlobals.ProfilePlacemarks.Remove(profilePlacemark);

                    while (true)
                    {
                        //bool createNewProfile = true;
                        //NewUserDialog dialog = new NewUserDialog(true, true, true);
                        NewUserDialogPresenter newUserDialog = new NewUserDialogPresenter(NewUserDialogContext.CorruptedProfile);//NewUserDialogPresenter.FromCorrupted(profilePlacemark); // new NewUserDialogPresenter(true, true, true);
                        //dialogPresenter.SetModeToLoadError(profilePlacemark);

                        if (ShowDialogAfterSplashScreen(newUserDialog) != UIDialogResult.OK)
                        {
                            Environment.Exit(0);
                        }

                        if (newUserDialog.NewUserAction == NewUserAction.UseExistingProfile)
                        {
                            profileFolderName = newUserDialog.SelectedProfile.FolderName;
                        }
                        else
                        {
                            Profile newProfile = Profile.CreateNewFromUI();

                            //NewProfileDialogPresenter newProfileDialogPresenter = Globals.GuiManager.Factory.CreateNewProfileDialog(newProfile, true);
                            //if (newProfileDialogPresenter.ShowDialog() != UIDialogResult.OK)
                            //{
                            //    Globals.SyncSynchronizer.CancelSyncsAndWait();
                            //    Globals.ProfilePlacemarks.Remove(newProfile.FolderName);
                            //    newProfile.Directory.Delete();
                            //    continue;
                            //}
                            newProfile.Name = newUserDialog.ProfileName;
                            newUserDialog.HotkeyPresenter.ImpartTo(newProfile.Hotkey);
                            newProfile.PromptPositioningMode = newUserDialog.PromptPositioningMode;

                            InternalGlobals.AutoStartWithComputer = newUserDialog.AutostartPromptu;
                            //newProfile.Name = newProfileDialogPresenter.ProfileName;
                            //newProfileDialogPresenter.HotkeyPresenter.ImpartTo(newProfile.Hotkey);
                            //newProfile.Hotkey.SwitchHotkey(profileSetupDialog.HotkeySetupPanel.HotkeyModifierKeys, profileSetupDialog.HotkeySetupPanel.UnderlyingHotkeyKeyValue);
                            newProfile.SaveAll();

                            profileFolderName = newProfile.FolderName;
                        }

                        break;
                    }
                }
            }
        }

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
#if CUSTOMTRACE
                traceListener.Dispose();
#endif
                Exception ex = e.ExceptionObject as Exception;

                if (InternalGlobals.CurrentProfile != null)
                {
                    if (InternalGlobals.CurrentProfile.Hotkey != null)
                    {
                        InternalGlobals.CurrentProfile.Hotkey.Unregister();
                    }
                    //else if (InternalGlobals.CurrentProfile.NotesHotkey != null)
                    //{
                    //    InternalGlobals.CurrentProfile.NotesHotkey.Unregister();
                    //}
                }

                bool unmanaged = ex == null;

                if (!unmanaged)
                {
                    ExceptionLogger.LogException(ex, "Promptu closed");
                }

                UnhandledExceptionDialogPresenter dialogPresenter = new UnhandledExceptionDialogPresenter(unmanaged);

                dialogPresenter.ShowDialog();

                if (!unmanaged)
                {
                    System.Environment.Exit(1);
                }
            }
        }

        //private static void HandleUnhandledException(Exception ex)
        //{
        //    if (PromptuSettings.CurrentProfile != null)
        //    {
        //        if (PromptuSettings.CurrentProfile.Hotkey != null)
        //        {
        //            PromptuSettings.CurrentProfile.Hotkey.Unregister();
        //        }
        //        else if (PromptuSettings.CurrentProfile.NotesHotkey != null)
        //        {
        //            PromptuSettings.CurrentProfile.NotesHotkey.Unregister();
        //        }
        //    }

        //    ExceptionLogger.LogException(ex, "Promptu closed");
        //    UnhandledExceptionDialog failureDialog = new UnhandledExceptionDialog(false);
        //    DialogResult result = failureDialog.ShowDialog();
        //    System.Environment.Exit(1);
        //}

        //private static void HandleUnmanagedUnhandledException()
        //{
        //    if (PromptuSettings.CurrentProfile != null)
        //    {
        //        if (PromptuSettings.CurrentProfile.Hotkey != null)
        //        {
        //            PromptuSettings.CurrentProfile.Hotkey.Unregister();
        //        }
        //        else if (PromptuSettings.CurrentProfile.NotesHotkey != null)
        //        {
        //            PromptuSettings.CurrentProfile.NotesHotkey.Unregister();
        //        }
        //    }

        //    UnhandledExceptionDialog failureDialog = new UnhandledExceptionDialog(true);
        //    DialogResult result = failureDialog.ShowDialog();
        //}
    }
}
