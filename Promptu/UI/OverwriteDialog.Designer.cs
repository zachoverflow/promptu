namespace ZachJohnson.Promptu.UI
{
 //HACK disabled
    partial class OverwriteDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.renameButton = new ZachJohnson.Promptu.UI.AreaButton();
            this.skipButton = new ZachJohnson.Promptu.UI.AreaButton();
            this.mainLabel = new System.Windows.Forms.Label();
            this.replaceButton = new ZachJohnson.Promptu.UI.AreaButton();
            this.chooseLabel = new System.Windows.Forms.Label();
            this.doForRemaining = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.renameButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.skipButton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.mainLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.replaceButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chooseLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.doForRemaining, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(506, 342);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // renameButton
            // 
            this.renameButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renameButton.Location = new System.Drawing.Point(3, 134);
            this.renameButton.Name = "renameButton";
            this.renameButton.Padding = new System.Windows.Forms.Padding(7);
            this.renameButton.Size = new System.Drawing.Size(500, 89);
            this.renameButton.TabIndex = 3;
            this.renameButton.UseVisualStyleBackColor = true;
            // 
            // skipButton
            // 
            this.skipButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skipButton.Location = new System.Drawing.Point(3, 229);
            this.skipButton.Name = "skipButton";
            this.skipButton.Padding = new System.Windows.Forms.Padding(7);
            this.skipButton.Size = new System.Drawing.Size(500, 89);
            this.skipButton.TabIndex = 2;
            this.skipButton.UseVisualStyleBackColor = true;
            // 
            // mainLabel
            // 
            this.mainLabel.AutoSize = true;
            this.mainLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLabel.Location = new System.Drawing.Point(3, 0);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(500, 13);
            this.mainLabel.TabIndex = 0;
            this.mainLabel.Text = "X conficts with Y";
            // 
            // replaceButton
            // 
            this.replaceButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.replaceButton.Location = new System.Drawing.Point(3, 39);
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Padding = new System.Windows.Forms.Padding(7);
            this.replaceButton.Size = new System.Drawing.Size(500, 89);
            this.replaceButton.TabIndex = 1;
            this.replaceButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.replaceButton.UseVisualStyleBackColor = true;
            // 
            // chooseLabel
            // 
            this.chooseLabel.AutoSize = true;
            this.chooseLabel.Location = new System.Drawing.Point(3, 13);
            this.chooseLabel.Name = "chooseLabel";
            this.chooseLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.chooseLabel.Size = new System.Drawing.Size(93, 23);
            this.chooseLabel.TabIndex = 4;
            this.chooseLabel.Text = "Choose an option:";
            // 
            // doForRemaining
            // 
            this.doForRemaining.AutoSize = true;
            this.doForRemaining.Location = new System.Drawing.Point(3, 324);
            this.doForRemaining.Name = "doForRemaining";
            this.doForRemaining.Size = new System.Drawing.Size(129, 15);
            this.doForRemaining.TabIndex = 5;
            this.doForRemaining.Text = "Do this for all conflicts";
            this.doForRemaining.UseVisualStyleBackColor = true;
            // 
            // OverwriteDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 362);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OverwriteDialog";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Move Item";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label mainLabel;
        private AreaButton renameButton;
        private AreaButton skipButton;
        private AreaButton replaceButton;
        private System.Windows.Forms.Label chooseLabel;
        private System.Windows.Forms.CheckBox doForRemaining;
    }
}