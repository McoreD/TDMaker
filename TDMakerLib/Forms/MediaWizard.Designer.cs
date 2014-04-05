namespace TDMakerLib
{
    partial class MediaWizard
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
            this.gbQuestion = new System.Windows.Forms.GroupBox();
            this.flpFileOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.rbFilesAsIndiv = new System.Windows.Forms.RadioButton();
            this.rbFilesAsColl = new System.Windows.Forms.RadioButton();
            this.flpTasks = new System.Windows.Forms.FlowLayoutPanel();
            this.chkScreenshotsCreate = new System.Windows.Forms.CheckBox();
            this.chkCreateTorrent = new System.Windows.Forms.CheckBox();
            this.lblUserActionMsg = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbTasks = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbQuestion.SuspendLayout();
            this.flpFileOptions.SuspendLayout();
            this.flpTasks.SuspendLayout();
            this.gbTasks.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbQuestion
            // 
            this.gbQuestion.Controls.Add(this.flpFileOptions);
            this.gbQuestion.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbQuestion.Location = new System.Drawing.Point(8, 48);
            this.gbQuestion.Name = "gbQuestion";
            this.gbQuestion.Size = new System.Drawing.Size(463, 96);
            this.gbQuestion.TabIndex = 1;
            this.gbQuestion.TabStop = false;
            this.gbQuestion.Text = "What would you like to do?";
            // 
            // flpFileOptions
            // 
            this.flpFileOptions.Controls.Add(this.rbFilesAsIndiv);
            this.flpFileOptions.Controls.Add(this.rbFilesAsColl);
            this.flpFileOptions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpFileOptions.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpFileOptions.Location = new System.Drawing.Point(8, 24);
            this.flpFileOptions.Name = "flpFileOptions";
            this.flpFileOptions.Size = new System.Drawing.Size(440, 64);
            this.flpFileOptions.TabIndex = 0;
            // 
            // rbFilesAsIndiv
            // 
            this.rbFilesAsIndiv.AutoSize = true;
            this.rbFilesAsIndiv.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFilesAsIndiv.Location = new System.Drawing.Point(3, 3);
            this.rbFilesAsIndiv.Name = "rbFilesAsIndiv";
            this.rbFilesAsIndiv.Size = new System.Drawing.Size(179, 22);
            this.rbFilesAsIndiv.TabIndex = 0;
            this.rbFilesAsIndiv.TabStop = true;
            this.rbFilesAsIndiv.Text = "Process files individually";
            this.rbFilesAsIndiv.UseVisualStyleBackColor = true;
            this.rbFilesAsIndiv.CheckedChanged += new System.EventHandler(this.rbFilesAsIndiv_CheckedChanged);
            // 
            // rbFilesAsColl
            // 
            this.rbFilesAsColl.AutoSize = true;
            this.rbFilesAsColl.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFilesAsColl.Location = new System.Drawing.Point(3, 31);
            this.rbFilesAsColl.Name = "rbFilesAsColl";
            this.rbFilesAsColl.Size = new System.Drawing.Size(237, 22);
            this.rbFilesAsColl.TabIndex = 1;
            this.rbFilesAsColl.TabStop = true;
            this.rbFilesAsColl.Text = "Process files as a Media Collection";
            this.rbFilesAsColl.UseVisualStyleBackColor = true;
            this.rbFilesAsColl.CheckedChanged += new System.EventHandler(this.rbFilesAsColl_CheckedChanged);
            // 
            // flpTasks
            // 
            this.flpTasks.Controls.Add(this.chkScreenshotsCreate);
            this.flpTasks.Controls.Add(this.chkCreateTorrent);
            this.flpTasks.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTasks.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpTasks.Location = new System.Drawing.Point(8, 24);
            this.flpTasks.Name = "flpTasks";
            this.flpTasks.Size = new System.Drawing.Size(440, 64);
            this.flpTasks.TabIndex = 2;
            // 
            // chkScreenshotsCreate
            // 
            this.chkScreenshotsCreate.AutoSize = true;
            this.chkScreenshotsCreate.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkScreenshotsCreate.Location = new System.Drawing.Point(3, 3);
            this.chkScreenshotsCreate.Name = "chkScreenshotsCreate";
            this.chkScreenshotsCreate.Size = new System.Drawing.Size(150, 22);
            this.chkScreenshotsCreate.TabIndex = 1;
            this.chkScreenshotsCreate.Text = "Include screenshots";
            this.chkScreenshotsCreate.UseVisualStyleBackColor = true;
            this.chkScreenshotsCreate.CheckedChanged += new System.EventHandler(this.chkScreenshotsInclude_CheckedChanged);
            // 
            // chkCreateTorrent
            // 
            this.chkCreateTorrent.AutoSize = true;
            this.chkCreateTorrent.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCreateTorrent.Location = new System.Drawing.Point(3, 31);
            this.chkCreateTorrent.Name = "chkCreateTorrent";
            this.chkCreateTorrent.Size = new System.Drawing.Size(125, 22);
            this.chkCreateTorrent.TabIndex = 0;
            this.chkCreateTorrent.Text = "Create a torrent";
            this.chkCreateTorrent.UseVisualStyleBackColor = true;
            this.chkCreateTorrent.CheckedChanged += new System.EventHandler(this.chkCreateTorrent_CheckedChanged);
            // 
            // lblUserActionMsg
            // 
            this.lblUserActionMsg.AutoSize = true;
            this.lblUserActionMsg.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserActionMsg.Location = new System.Drawing.Point(16, 16);
            this.lblUserActionMsg.Name = "lblUserActionMsg";
            this.lblUserActionMsg.Size = new System.Drawing.Size(131, 18);
            this.lblUserActionMsg.TabIndex = 3;
            this.lblUserActionMsg.Text = "You about to import";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(312, 272);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbTasks
            // 
            this.gbTasks.Controls.Add(this.flpTasks);
            this.gbTasks.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTasks.Location = new System.Drawing.Point(8, 152);
            this.gbTasks.Name = "gbTasks";
            this.gbTasks.Size = new System.Drawing.Size(464, 104);
            this.gbTasks.TabIndex = 5;
            this.gbTasks.TabStop = false;
            this.gbTasks.Text = "Tasks";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(392, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MediaWizard
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 306);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbTasks);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblUserActionMsg);
            this.Controls.Add(this.gbQuestion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MediaWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TDMaker - Media Wizard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MediaWizard_FormClosed);
            this.gbQuestion.ResumeLayout(false);
            this.flpFileOptions.ResumeLayout(false);
            this.flpFileOptions.PerformLayout();
            this.flpTasks.ResumeLayout(false);
            this.flpTasks.PerformLayout();
            this.gbTasks.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbQuestion;
        private System.Windows.Forms.FlowLayoutPanel flpFileOptions;
        private System.Windows.Forms.FlowLayoutPanel flpTasks;
        private System.Windows.Forms.CheckBox chkCreateTorrent;
        private System.Windows.Forms.Label lblUserActionMsg;
        private System.Windows.Forms.CheckBox chkScreenshotsCreate;
        private System.Windows.Forms.RadioButton rbFilesAsIndiv;
        private System.Windows.Forms.RadioButton rbFilesAsColl;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbTasks;
        private System.Windows.Forms.Button btnCancel;


    }
}