using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.UI
{
    public partial class PromptuUpgradeWelcomeDialog : Form
    {
        public PromptuUpgradeWelcomeDialog()
        {
            InitializeComponent();
            this.Icon = Icons.ApplicationIcon;
            this.Font = PromptuFonts.DefaultFont;
            this.mainLabel.Font = new Font(this.mainLabel.Font.FontFamily, 12F);
            this.giveDefaultItems.Font = new Font(this.giveDefaultItems.Font, FontStyle.Italic);

            this.giveDefaultItems.Checked = true;

            this.newFeaturesLinkLabel.LinkClicked += this.ShowUserListOfNewFeatures;

            this.StartPosition = FormStartPosition.CenterScreen;

            this.okButton.Click += this.HandleOkButtonClick;
        }

        private void ShowUserListOfNewFeatures(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.promptulauncher.com/changelogs/new-in-0.8.php");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                UIMessageBox.Show(
                    Localization.Promptu.ErrorVisitingWebsite,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
            catch (FileNotFoundException)
            {
                UIMessageBox.Show(
                    Localization.Promptu.ErrorVisitingWebsite,
                    Localization.Promptu.AppName,
                    UIMessageBoxButtons.OK,
                    UIMessageBoxIcon.Error,
                    UIMessageBoxResult.OK);
            }
        }

        private void HandleOkButtonClick(object sender, EventArgs e)
        {
            if (this.giveDefaultItems.Checked)
            {
                Globals.CurrentProfile.CreateNewList(null, false);
                Skins.PromptHandler.GetInstance().SetupDialog.ListSelector.UpdateLists();
            }
        }
    }
}
