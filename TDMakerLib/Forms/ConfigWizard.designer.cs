namespace TDMakerLib
{
    partial class ConfigWizard
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
            this.components = new System.ComponentModel.Container();
            this.lblScreenshotDestination = new System.Windows.Forms.Label();
            this.cboScreenshotDest = new System.Windows.Forms.ComboBox();
            this.gbRoot = new System.Windows.Forms.GroupBox();
            this.btnViewRootDir = new System.Windows.Forms.Button();
            this.btnBrowseRootDir = new System.Windows.Forms.Button();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbPublishOptions = new System.Windows.Forms.GroupBox();
            this.chkPreferSystemFolders = new System.Windows.Forms.CheckBox();
            this.ttApp = new System.Windows.Forms.ToolTip(this.components);
            this.btnExit = new System.Windows.Forms.Button();
            this.txtPtpImgCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbRoot.SuspendLayout();
            this.gbPublishOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblScreenshotDestination
            // 
            this.lblScreenshotDestination.AutoSize = true;
            this.lblScreenshotDestination.Location = new System.Drawing.Point(21, 39);
            this.lblScreenshotDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreenshotDestination.Name = "lblScreenshotDestination";
            this.lblScreenshotDestination.Size = new System.Drawing.Size(125, 17);
            this.lblScreenshotDestination.TabIndex = 3;
            this.lblScreenshotDestination.Text = "Image Destination:";
            // 
            // cboScreenshotDest
            // 
            this.cboScreenshotDest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScreenshotDest.FormattingEnabled = true;
            this.cboScreenshotDest.Location = new System.Drawing.Point(160, 34);
            this.cboScreenshotDest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboScreenshotDest.Name = "cboScreenshotDest";
            this.cboScreenshotDest.Size = new System.Drawing.Size(361, 24);
            this.cboScreenshotDest.TabIndex = 2;
            this.cboScreenshotDest.SelectedIndexChanged += new System.EventHandler(this.cboScreenshotDest_SelectedIndexChanged);
            // 
            // gbRoot
            // 
            this.gbRoot.Controls.Add(this.btnViewRootDir);
            this.gbRoot.Controls.Add(this.btnBrowseRootDir);
            this.gbRoot.Controls.Add(this.txtRootFolder);
            this.gbRoot.Location = new System.Drawing.Point(11, 41);
            this.gbRoot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbRoot.Name = "gbRoot";
            this.gbRoot.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbRoot.Size = new System.Drawing.Size(811, 79);
            this.gbRoot.TabIndex = 118;
            this.gbRoot.TabStop = false;
            this.gbRoot.Text = "Root folder for Settings and Data";
            this.gbRoot.UseCompatibleTextRendering = true;
            // 
            // btnViewRootDir
            // 
            this.btnViewRootDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewRootDir.Location = new System.Drawing.Point(653, 30);
            this.btnViewRootDir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnViewRootDir.Name = "btnViewRootDir";
            this.btnViewRootDir.Size = new System.Drawing.Size(139, 30);
            this.btnViewRootDir.TabIndex = 116;
            this.btnViewRootDir.Text = "View Directory...";
            this.btnViewRootDir.UseCompatibleTextRendering = true;
            this.btnViewRootDir.UseVisualStyleBackColor = true;
            this.btnViewRootDir.Click += new System.EventHandler(this.btnViewRootDir_Click);
            // 
            // btnBrowseRootDir
            // 
            this.btnBrowseRootDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRootDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowseRootDir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowseRootDir.Location = new System.Drawing.Point(536, 30);
            this.btnBrowseRootDir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseRootDir.Name = "btnBrowseRootDir";
            this.btnBrowseRootDir.Size = new System.Drawing.Size(107, 30);
            this.btnBrowseRootDir.TabIndex = 115;
            this.btnBrowseRootDir.Text = "Relocate...";
            this.btnBrowseRootDir.UseCompatibleTextRendering = true;
            this.btnBrowseRootDir.UseVisualStyleBackColor = true;
            this.btnBrowseRootDir.Click += new System.EventHandler(this.btnBrowseRootDir_Click);
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootFolder.Location = new System.Drawing.Point(21, 32);
            this.txtRootFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.ReadOnly = true;
            this.txtRootFolder.Size = new System.Drawing.Size(500, 22);
            this.txtRootFolder.TabIndex = 114;
            this.txtRootFolder.Tag = "Path of the Root folder that holds Images, Text, Cache, Settings and Temp folders" +
    "";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(703, 254);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 34);
            this.btnOK.TabIndex = 117;
            this.btnOK.Text = "&OK";
            this.btnOK.UseCompatibleTextRendering = true;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbPublishOptions
            // 
            this.gbPublishOptions.Controls.Add(this.label1);
            this.gbPublishOptions.Controls.Add(this.txtPtpImgCode);
            this.gbPublishOptions.Controls.Add(this.lblScreenshotDestination);
            this.gbPublishOptions.Controls.Add(this.cboScreenshotDest);
            this.gbPublishOptions.Location = new System.Drawing.Point(11, 129);
            this.gbPublishOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPublishOptions.Name = "gbPublishOptions";
            this.gbPublishOptions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbPublishOptions.Size = new System.Drawing.Size(811, 115);
            this.gbPublishOptions.TabIndex = 120;
            this.gbPublishOptions.TabStop = false;
            this.gbPublishOptions.Text = "Publish Options";
            this.gbPublishOptions.UseCompatibleTextRendering = true;
            // 
            // chkPreferSystemFolders
            // 
            this.chkPreferSystemFolders.AutoSize = true;
            this.chkPreferSystemFolders.Location = new System.Drawing.Point(11, 10);
            this.chkPreferSystemFolders.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPreferSystemFolders.Name = "chkPreferSystemFolders";
            this.chkPreferSystemFolders.Size = new System.Drawing.Size(304, 21);
            this.chkPreferSystemFolders.TabIndex = 121;
            this.chkPreferSystemFolders.Text = "&Prefer Known Folders for Settings and Data";
            this.chkPreferSystemFolders.UseVisualStyleBackColor = true;
            this.chkPreferSystemFolders.CheckedChanged += new System.EventHandler(this.chkPreferSystemFolders_CheckedChanged);
            // 
            // ttApp
            // 
            this.ttApp.AutoPopDelay = 15000;
            this.ttApp.InitialDelay = 100;
            this.ttApp.ReshowDelay = 100;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.Location = new System.Drawing.Point(595, 254);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 34);
            this.btnExit.TabIndex = 122;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseCompatibleTextRendering = true;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtPtpImgCode
            // 
            this.txtPtpImgCode.Location = new System.Drawing.Point(160, 66);
            this.txtPtpImgCode.Name = "txtPtpImgCode";
            this.txtPtpImgCode.Size = new System.Drawing.Size(632, 22);
            this.txtPtpImgCode.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "ptpimg.me code:";
            // 
            // ConfigWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 301);
            this.Controls.Add(this.chkPreferSystemFolders);
            this.Controls.Add(this.gbRoot);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.gbPublishOptions);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "ConfigWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TDMaker - Configuration Wizard";
            this.Load += new System.EventHandler(this.ConfigWizard_Load);
            this.gbRoot.ResumeLayout(false);
            this.gbRoot.PerformLayout();
            this.gbPublishOptions.ResumeLayout(false);
            this.gbPublishOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScreenshotDestination;
        private System.Windows.Forms.ComboBox cboScreenshotDest;
        private System.Windows.Forms.GroupBox gbRoot;
        private System.Windows.Forms.Button btnViewRootDir;
        private System.Windows.Forms.Button btnBrowseRootDir;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbPublishOptions;
        private System.Windows.Forms.CheckBox chkPreferSystemFolders;
        private System.Windows.Forms.ToolTip ttApp;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPtpImgCode;
    }
}