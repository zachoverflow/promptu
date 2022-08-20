namespace ZachJohnson.Promptu.UI
{
    partial class PromptuUpgradeWelcomeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromptuUpgradeWelcomeDialog));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.mainLabel = new System.Windows.Forms.Label();
            this.newFeaturesLinkLabel = new System.Windows.Forms.LinkLabel();
            this.newDefaultItemsLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.giveDefaultItems = new System.Windows.Forms.CheckBox();
            this.verticalAutoSizeLabel2 = new ZachJohnson.Promptu.UI.VerticalAutoSizeLabel();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.mainLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.newFeaturesLinkLabel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.newDefaultItemsLabel, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.groupBox1, 0, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(545, 307);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.AutoSize = true;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(467, 281);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // mainLabel
            // 
            this.mainLabel.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.mainLabel, 2);
            this.mainLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLabel.Location = new System.Drawing.Point(3, 0);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.mainLabel.Size = new System.Drawing.Size(539, 33);
            this.mainLabel.TabIndex = 1;
            this.mainLabel.Text = "You have been sucessfully updated to Promptu 0.8";
            this.mainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // newFeaturesLinkLabel
            // 
            this.newFeaturesLinkLabel.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.newFeaturesLinkLabel, 2);
            this.newFeaturesLinkLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newFeaturesLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(57, 11);
            this.newFeaturesLinkLabel.Location = new System.Drawing.Point(3, 33);
            this.newFeaturesLinkLabel.Name = "newFeaturesLinkLabel";
            this.newFeaturesLinkLabel.Padding = new System.Windows.Forms.Padding(3);
            this.newFeaturesLinkLabel.Size = new System.Drawing.Size(539, 23);
            this.newFeaturesLinkLabel.TabIndex = 3;
            this.newFeaturesLinkLabel.TabStop = true;
            this.newFeaturesLinkLabel.Text = "Promptu 0.8 includes a number of exciting new features.  See list...";
            this.newFeaturesLinkLabel.UseCompatibleTextRendering = true;
            // 
            // newDefaultItemsLabel
            // 
            this.newDefaultItemsLabel.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.newDefaultItemsLabel, 2);
            this.newDefaultItemsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDefaultItemsLabel.Location = new System.Drawing.Point(3, 56);
            this.newDefaultItemsLabel.Name = "newDefaultItemsLabel";
            this.newDefaultItemsLabel.Padding = new System.Windows.Forms.Padding(3);
            this.newDefaultItemsLabel.Size = new System.Drawing.Size(539, 19);
            this.newDefaultItemsLabel.TabIndex = 4;
            this.newDefaultItemsLabel.Text = "The default items have been updated in Promptu 0.8 to take advantage of the new f" +
                "eatures.";
            // 
            // groupBox1
            // 
            this.tableLayoutPanel.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.giveDefaultItems);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(10, 75);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 200);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.verticalAutoSizeLabel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 43);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 154);
            this.panel1.TabIndex = 7;
            // 
            // giveDefaultItems
            // 
            this.giveDefaultItems.AutoSize = true;
            this.giveDefaultItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.giveDefaultItems.Location = new System.Drawing.Point(3, 16);
            this.giveDefaultItems.Name = "giveDefaultItems";
            this.giveDefaultItems.Padding = new System.Windows.Forms.Padding(3, 0, 0, 10);
            this.giveDefaultItems.Size = new System.Drawing.Size(519, 27);
            this.giveDefaultItems.TabIndex = 5;
            this.giveDefaultItems.Text = "Add a new list with the new default items\r\n";
            this.giveDefaultItems.UseVisualStyleBackColor = true;
            // 
            // verticalAutoSizeLabel2
            // 
            this.verticalAutoSizeLabel2.AutoSize = true;
            this.verticalAutoSizeLabel2.Location = new System.Drawing.Point(3, 3);
            this.verticalAutoSizeLabel2.Name = "verticalAutoSizeLabel2";
            this.verticalAutoSizeLabel2.Size = new System.Drawing.Size(502, 130);
            this.verticalAutoSizeLabel2.TabIndex = 0;
            this.verticalAutoSizeLabel2.Text = resources.GetString("verticalAutoSizeLabel2.Text");
            // 
            // PromptuUpgradeWelcomeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 307);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "PromptuUpgradeWelcomeDialog";
            this.Text = "Welcome to Promptu 0.8!";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.LinkLabel newFeaturesLinkLabel;
        private System.Windows.Forms.Label newDefaultItemsLabel;
        private System.Windows.Forms.CheckBox giveDefaultItems;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private VerticalAutoSizeLabel verticalAutoSizeLabel2;
    }
}