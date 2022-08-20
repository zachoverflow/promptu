using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// HACK disabled
namespace ZachJohnson.Promptu.UI
{
    internal partial class OverwriteDialog : Form
    {
        private MoveConfictAction action;

        public OverwriteDialog(bool couldBeMore)
        {
            InitializeComponent();
            this.Font = PromptuFonts.DefaultFont;
            this.Icon = Icons.ApplicationIcon;

            this.mainLabel.Font = new Font(this.mainLabel.Font.FontFamily, 12F);

            this.replaceButton.Click += this.HandleReplaceClick;
            this.skipButton.Click += this.HandleSkipClick;
            this.renameButton.Click += this.HandleRenameClick;

            if (!couldBeMore)
            {
                this.doForRemaining.Parent = null;
            }

            //this.replaceButton.GraphicalEntries.Add(new GraphicalTextEntry("Move and Replace", this.mainLabel.Font, PromptuColors.InfoColor), 0, 0);
            //this.dontButton.GraphicalEntries.Add(new GraphicalTextEntry("Don't Move", this.mainLabel.Font, PromptuColors.InfoColor), 0, 0);
            //this.renameButton.GraphicalEntries.Add(new GraphicalTextEntry("Move and Rename", this.mainLabel.Font, PromptuColors.InfoColor), 0, 0);
        }

        public string MainText
        {
            get { return this.mainLabel.Text; }
            set { this.mainLabel.Text = value; }
        }

        public AreaButton RenameButton
        {
            get { return this.renameButton; }
        }

        public AreaButton ReplaceButton
        {
            get { return this.replaceButton; }
        }

        public AreaButton SkipButton
        {
            get { return this.skipButton; }
        }

        public bool DoActionForRemaining
        {
            get { return this.doForRemaining.Checked; }
        }

        public MoveConfictAction Action
        {
            get { return this.action; }
        }

        private void HandleRenameClick(object sender, EventArgs e)
        {
            this.action = MoveConfictAction.Rename;
            this.DialogResult = DialogResult.OK;
        }

        private void HandleSkipClick(object sender, EventArgs e)
        {
            this.action = MoveConfictAction.NoMove;
            this.DialogResult = DialogResult.OK;
        }

        private void HandleReplaceClick(object sender, EventArgs e)
        {
            this.action = MoveConfictAction.Overwrite;
            this.DialogResult = DialogResult.OK;
        }
    }
}
