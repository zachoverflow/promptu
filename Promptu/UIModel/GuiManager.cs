using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UIModel.Presenters;
using ZachJohnson.Promptu.UserModel;
using System.Xml;

namespace ZachJohnson.Promptu.UIModel
{
    internal class GuiManager
    {
        private ToolkitHost toolkitHost;
        //private NotifyIconPresenter notifyIconPresenter;
        //private GuiFactory factory;
        private bool loadingSettings;

        public GuiManager(ToolkitHost toolkitHost)
        {
            if (toolkitHost == null)
            {
                throw new ArgumentNullException("toolkitHost");
            }

            this.toolkitHost = toolkitHost;
            this.toolkitHost.Settings.SettingChanged += this.HandleUISettingChanged;
            //this.factory = new GuiFactory(this);
        }

        public ToolkitHost ToolkitHost
        {
            get { return this.toolkitHost; }
        }

        public void SaveUISettings()
        {
            Profile currentProfile = InternalGlobals.CurrentProfile;
            if (currentProfile != null)
            {
                XmlDocument document = new XmlDocument();
                XmlNode rootNode = document.CreateElement("Promptu");
                document.AppendChild(rootNode);

                this.toolkitHost.Settings.AppendAsXmlOn(rootNode, "UISettings");
                document.Save(currentProfile.Directory + "\\UI.xml");
            }
        }

        private void HandleUISettingChanged(object sender, EventArgs e)
        {
            if (this.loadingSettings)
            {
                return;
            }

            this.SaveUISettings();
        }

        public void LoadUISettingsFromCurrentProfile()
        {
            Profile currentProfile = InternalGlobals.CurrentProfile;
            if (currentProfile != null)
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(currentProfile.Directory + "\\UI.xml");

                    foreach (XmlNode root in document.ChildNodes)
                    {
                        if (root.Name.ToUpperInvariant() == "PROMPTU")
                        {
                            foreach (XmlNode node in root.ChildNodes)
                            {
                                if (node.Name.ToUpperInvariant() == "UISETTINGS")
                                {
                                    this.loadingSettings = true;
                                    this.toolkitHost.Settings.UpdateFrom(node);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (System.IO.IOException)
                {
                }
                catch (XmlException)
                {
                }
                finally
                {
                    this.loadingSettings = false;
                }
            }

        }

        //public NotifyIconPresenter NotifyIconPresenter
        //{
        //    get { return this.notifyIconPresenter; }
        //}

        //public GuiFactory Factory
        //{
        //    get { return this.factory; }
        //}

        //public void RunApplication()
        //{
        //    this.notifyIconPresenter = new NotifyIconPresenter(toolkitFactory.ConstructNotifyIcon());
        //}

        // necessary?
        //private void HandleUnhandledException(object sender, ThreadExceptionEventArgs e)
        //{
        //    HandleUnhandledException(e.Exception);
        //}

        
    }
}
