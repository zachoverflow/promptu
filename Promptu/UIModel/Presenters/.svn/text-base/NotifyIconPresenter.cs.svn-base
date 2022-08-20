using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Interfaces;
using ZachJohnson.Promptu.Skins;
using System.Globalization;

namespace ZachJohnson.Promptu.UIModel.Presenters
{
    internal class NotifyIconPresenter : IDisposable
    {
        private const string newCommandImageKey = "NewCommand";
        private readonly INotifyIcon notifyIcon;
        //private UIc contextMenuItems;
        private UIMenuItemInternal contextNewCommand;

        public NotifyIconPresenter()
            : this(InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructNotifyIcon())
        {
        }

        public NotifyIconPresenter(INotifyIcon notifyIcon)
        {
            if (notifyIcon == null)
            {
                throw new ArgumentNullException("notifyIcon");
            }

            this.notifyIcon = notifyIcon;
            this.notifyIcon.Icon = InternalGlobals.GuiManager.ToolkitHost.AppIcon;
            //this.contextMenuItems = new UIMenuItemCollection(this.notifyIcon.ContextMenuItems);

            this.InitializeMenu();

            this.notifyIcon.TimeToOpenPrompt += this.HandleTimeToOpenPrompt;

            this.UpdateToolTipText();
        }

        ~NotifyIconPresenter()
        {
            this.Dispose(false);
        }

        public INotifyIcon NotifyIcon
        {
            get { return this.notifyIcon; }
        }

        public void IntializePostProfileLoad()
        {
            PromptHandler.GetInstance().ListsChanged += this.HandleListsChanged;
            PromptHandler.GetInstance().HotkeyChanged += this.HandleHotkeyChanged;

            this.UpdateNewCommandItems();
        }

        public UIContextMenu ContextMenu
        {
            get { return this.notifyIcon.ContextMenu; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.notifyIcon.Hide();

                if (!PromptHandler.IsInitializing)
                {
                    PromptHandler.GetInstance().Dispose();
                }
            }
        }

        private void InitializeMenu()
        {
            UIMenuItemInternal openPrompt = new UIMenuItemInternal(
                "Promptu.Icon.OpenPrompt",
                Localization.UIResources.IconOpenPromptText,
                new EventHandler(this.OpenPrompt));

            openPrompt.TextStyle = TextStyle.Bold;

            this.contextNewCommand = new UIMenuItemInternal(
                "Promptu.Icon.NewCommand");

            this.contextNewCommand.Image = InternalGlobals.GuiManager.ToolkitHost.Images.NewCommand;
            this.contextNewCommand.Click += this.HandleCreateNewCommand;

            this.UpdateNewCommandItems();

            this.ContextMenu.Items.Add(openPrompt);
            //this.ContextMenuItems.AddInternal(new UIMenuItemInternal(
            //    "Promptu.Icon.OpenNotes",
            //    Localization.UIResources.IconOpenNotesText,
            //    new EventHandler(this.OpenNotes)));

            this.ContextMenu.Items.Add(new UIMenuItemInternal(
                "Promptu.Icon.Setup",
                Localization.UIResources.IconSetupText,
                new EventHandler(this.ShowSetup)));

            this.ContextMenu.Items.Add(this.contextNewCommand);
            this.ContextMenu.Items.Add(new UIMenuSeparatorInternal("Promptu.Icon.OpenPromptSeparator"));
            this.ContextMenu.Items.Add(new UIMenuItemInternal(
                "Promptu.Icon.Help",
                Localization.UIResources.IconHelpText,
                new EventHandler(this.ShowHelp)));

            this.ContextMenu.Items.Add(new UIMenuItemInternal(
                "Promptu.Icon.About",
                Localization.UIResources.IconAboutText,
                new EventHandler(this.ShowAbout)));

            this.ContextMenu.Items.Add(new UIMenuSeparatorInternal("Promptu.Icon.HelpSeparator"));
            this.ContextMenu.Items.Add(new UIMenuItemInternal(
                "Promptu.Icon.Quit",
                Localization.UIResources.IconQuitText,
                new EventHandler(this.QuitClick)));

            //ToolStripMenuItem openPrompt = new ToolStripMenuItem("", null, new EventHandler(this.OpenPrompt));
            //openPrompt.Font = new System.Drawing.Font(openPrompt.Font, System.Drawing.FontStyle.Bold);

            //this.contextNewCommand = new ToolStripMenuItem();
            //this.contextNewCommand.Image = Images.NewCommand;
            //this.contextNewCommand.Click += this.HandleCreateNewCommand;

            

            //return new ToolStripItem[] 
            //{ 
            //    //openPrompt,
            //    //new ToolStripMenuItem("Open &Notes", null, new EventHandler(this.OpenNotes)),
            //    //new ToolStripMenuItem("&Setup", null, new EventHandler(this.ShowSetup)),
            //   // this.contextNewCommand,
            //    //new ToolStripSeparator(),
            //    //new ToolStripMenuItem("&Help", null, new EventHandler(this.ShowHelp)),
            //    //new ToolStripMenuItem("&About", null, new EventHandler(this.ShowAbout)),
            //    //new ToolStripSeparator(),
            //    new ToolStripMenuItem("&Quit", null, new EventHandler(this.QuitClick))
            //};
        }

        public void UpdateNewCommandItems()
        {
            if (InternalGlobals.CurrentProfile == null || InternalGlobals.CurrentProfile.Lists.Count <= 0)
            {
                this.contextNewCommand.Available = false;
            }
            else
            {
                this.contextNewCommand.Available = true;
                this.contextNewCommand.SubItems.Clear();
                this.contextNewCommand.Tag = null;

                if (InternalGlobals.CurrentProfile.Lists.Count == 1)
                {
                    this.contextNewCommand.Text = Localization.UIResources.IconCreateNewCommandText;
                    this.contextNewCommand.Tag = InternalGlobals.CurrentProfile.Lists[0].FolderName;
                }
                else
                {
                    this.contextNewCommand.Text = Localization.UIResources.IconCreateNewCommandInText;

                    foreach (UserModel.List list in InternalGlobals.CurrentProfile.Lists)
                    {
                        UIMenuItem subMenuItem = new UIMenuItemInternal(
                            String.Format(CultureInfo.InvariantCulture, "Promptu.List={0}", list.FolderName),
                            list.Name);

                        subMenuItem.Tag = list.FolderName;
                        subMenuItem.Click += this.HandleCreateNewCommand;
                        this.contextNewCommand.SubItems.Add(subMenuItem);

                        //ToolStripMenuItem subMenuItem = new ToolStripMenuItem();
                        //subMenuItem.Text = list.Name;
                        //subMenuItem.Tag = list.FolderName;
                        //subMenuItem.Click += this.HandleCreateNewCommand;
                        //this.contextNewCommand.DropDownItems.Add(subMenuItem);
                    }
                }
            }

            //this.contextNewCommand.Text = "New &Command";
            //this.contextNewCommand.Image = Images.NewCommand;
        }

        private void HandleCreateNewCommand(object sender, EventArgs e)
        {
            UIMenuItem menuItem = sender as UIMenuItem;

            if (menuItem != null)
            {
                string folderName = menuItem.Tag as string;

                if (folderName != null)
                {
                    if (!PromptHandler.IsInitializing)
                    {
                        PromptHandler promptHandler = PromptHandler.GetInstance();
                        promptHandler.ShowSetupDialog();
                        promptHandler.SetupDialog.CreateNewCommand(folderName);
                    }
                }
            }
        }

        private void ShowHelp(object sender, EventArgs e)
        {
            if (PromptHandler.IsInitializing)
            {
                return;
            }

            PromptHandler.GetInstance().TakeUserToHelp();
        }

        private void ShowAbout(object sender, EventArgs e)
        {
            if (PromptHandler.IsInitializing)
            {
                return;
            }

            PromptHandler.GetInstance().SetupDialog.ShowUserToAbout();
        }

        private void ShowSetup(object sender, EventArgs e)
        {
            if (PromptHandler.IsInitializing)
            {
                return;
            }

            PromptHandler.GetInstance().ShowSetupDialog();
        }

        private void QuitClick(object sender, EventArgs e)
        {
            PromptuUtilities.ExitApplication();
        }

        private void OpenPrompt(object sender, EventArgs e)
        {
            this.HandleTimeToOpenPrompt();
        }

        private void HandleTimeToOpenPrompt()
        {
            if (PromptHandler.IsInitializing)
            {
                return;
            }

            PromptHandler.GetInstance().OpenPrompt();
        }

        private void HandleHotkeyChanged(object sender, EventArgs e)
        {
            this.UpdateToolTipText();
        }

        private void HandleListsChanged(object sender, EventArgs e)
        {
            this.UpdateNewCommandItems();
        }

        public void UpdateToolTipText()
        {
            if (InternalGlobals.CurrentProfile != null)
            {
                try
                {
                    string hotkeyDisplay;
                    if (InternalGlobals.CurrentProfile.Hotkey.Registered)
                    {
                        hotkeyDisplay = Utilities.ConvertHotkeyToString(InternalGlobals.CurrentProfile.Hotkey.ModifierKeys, InternalGlobals.CurrentProfile.Hotkey.Key);
                    }
                    else
                    {
                        hotkeyDisplay = Localization.UIResources.UnableToMapHotkeyText;
                    }

                    this.notifyIcon.ToolTipText = String.Format(
                        CultureInfo.CurrentCulture, 
                        Localization.UIResources.IconToolTipFormat,
                        hotkeyDisplay);
                }
                catch (ArgumentException)
                {
                    this.notifyIcon.ToolTipText = Localization.Promptu.AppName;
                }
            }
            else
            {
                this.notifyIcon.ToolTipText = Localization.Promptu.AppName;
            }
        }
    }
}
