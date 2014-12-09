using HelpersLib;
using TDMakerLib;

namespace TDMaker
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.bwApp = new System.ComponentModel.BackgroundWorker();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.sbarIcon = new System.Windows.Forms.ToolStripStatusLabel();
            this.sBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssPerc = new System.Windows.Forms.ToolStripStatusLabel();
            this.pBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.ttApp = new System.Windows.Forms.ToolTip(this.components);
            this.txtWebLink = new System.Windows.Forms.TextBox();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFileOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.miFileSaveTorrent = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveInfoAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.foldersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.miFoldersScreenshots = new System.Windows.Forms.ToolStripMenuItem();
            this.miFoldersTorrents = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.miFoldersLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.miFoldersLogsDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.miFoldersSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miFoldersTemplates = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpVersionHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPublish = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnCreateTorrent = new System.Windows.Forms.Button();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMedia = new System.Windows.Forms.TabPage();
            this.gbSource = new System.Windows.Forms.GroupBox();
            this.chkTitle = new System.Windows.Forms.CheckBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.chkWebLink = new System.Windows.Forms.CheckBox();
            this.chkSource = new System.Windows.Forms.CheckBox();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.gbDVD = new System.Windows.Forms.GroupBox();
            this.cboDiscMenu = new System.Windows.Forms.ComboBox();
            this.chkDiscMenu = new System.Windows.Forms.CheckBox();
            this.cboExtras = new System.Windows.Forms.ComboBox();
            this.chkExtras = new System.Windows.Forms.CheckBox();
            this.cboAuthoring = new System.Windows.Forms.ComboBox();
            this.chkAuthoring = new System.Windows.Forms.CheckBox();
            this.gbLocation = new System.Windows.Forms.GroupBox();
            this.btnBrowseDir = new System.Windows.Forms.Button();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.tpMediaInfo = new System.Windows.Forms.TabPage();
            this.tlpMediaInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lbMediaInfo = new System.Windows.Forms.ListBox();
            this.txtMediaInfo = new System.Windows.Forms.TextBox();
            this.chkMediaInfoComplete = new System.Windows.Forms.CheckBox();
            this.tpScreenshots = new System.Windows.Forms.TabPage();
            this.tlpScreenshots = new System.Windows.Forms.TableLayoutPanel();
            this.lbScreenshots = new System.Windows.Forms.ListBox();
            this.tlpScreenshotProps = new System.Windows.Forms.TableLayoutPanel();
            this.pbScreenshot = new HelpersLib.MyPictureBox();
            this.pgScreenshot = new System.Windows.Forms.PropertyGrid();
            this.tpPublish = new System.Windows.Forms.TabPage();
            this.tcPublish = new System.Windows.Forms.TabControl();
            this.tpPublishGeneral = new System.Windows.Forms.TabPage();
            this.tlpPublish = new System.Windows.Forms.TableLayoutPanel();
            this.gbQuickPublish = new System.Windows.Forms.GroupBox();
            this.flpPublishConfig = new System.Windows.Forms.FlowLayoutPanel();
            this.chkQuickPre = new System.Windows.Forms.CheckBox();
            this.chkQuickAlignCenter = new System.Windows.Forms.CheckBox();
            this.chkQuickFullPicture = new System.Windows.Forms.CheckBox();
            this.cboQuickPublishType = new System.Windows.Forms.ComboBox();
            this.cboQuickTemplate = new System.Windows.Forms.ComboBox();
            this.txtPublish = new System.Windows.Forms.TextBox();
            this.lbPublish = new System.Windows.Forms.ListBox();
            this.tpPublishTemplates = new System.Windows.Forms.TabPage();
            this.cboPublishType = new System.Windows.Forms.ComboBox();
            this.chkTemplatesMode = new System.Windows.Forms.CheckBox();
            this.gbTemplatesInternal = new System.Windows.Forms.GroupBox();
            this.nudFontSizeIncr = new System.Windows.Forms.NumericUpDown();
            this.chkPre = new System.Windows.Forms.CheckBox();
            this.chkPreIncreaseFontSize = new System.Windows.Forms.CheckBox();
            this.chkAlignCenter = new System.Windows.Forms.CheckBox();
            this.btnTemplatesRewrite = new System.Windows.Forms.Button();
            this.gbFonts = new System.Windows.Forms.GroupBox();
            this.nudHeading1Size = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudHeading2Size = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudHeading3Size = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudBodySize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUploadFullScreenshot = new System.Windows.Forms.CheckBox();
            this.cboTemplate = new System.Windows.Forms.ComboBox();
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.rtbDebugLog = new System.Windows.Forms.RichTextBox();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tpSettingsGeneral = new System.Windows.Forms.TabPage();
            this.pgApp = new System.Windows.Forms.PropertyGrid();
            this.tpSettingsProfiles = new System.Windows.Forms.TabPage();
            this.btnRemoveScreenshotProfile = new System.Windows.Forms.Button();
            this.btnAddScreenshotProfile = new System.Windows.Forms.Button();
            this.listBoxProfiles = new System.Windows.Forms.ListBox();
            this.gbUploadScreenshots = new System.Windows.Forms.GroupBox();
            this.cboFileUploader = new System.Windows.Forms.ComboBox();
            this.btnDownloadFFmpeg = new System.Windows.Forms.Button();
            this.btnUploadersConfig = new System.Windows.Forms.Button();
            this.cboImageUploader = new System.Windows.Forms.ComboBox();
            this.chkUploadScreenshots = new System.Windows.Forms.CheckBox();
            this.pgProfileOptions = new System.Windows.Forms.PropertyGrid();
            this.tpSettingsProxy = new System.Windows.Forms.TabPage();
            this.lblProxyHost = new System.Windows.Forms.Label();
            this.cbProxyMethod = new System.Windows.Forms.ComboBox();
            this.lblProxyMethod = new System.Windows.Forms.Label();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.nudProxyPort = new System.Windows.Forms.NumericUpDown();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.lblProxyUsername = new System.Windows.Forms.Label();
            this.txtProxyUsername = new System.Windows.Forms.TextBox();
            this.tscMain = new System.Windows.Forms.ToolStripContainer();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.tttvMain = new HelpersLib.TabToTreeView();
            this.flpMediaInfoOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.ssMain.SuspendLayout();
            this.msMain.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpMedia.SuspendLayout();
            this.gbSource.SuspendLayout();
            this.gbDVD.SuspendLayout();
            this.gbLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.tpMediaInfo.SuspendLayout();
            this.tlpMediaInfo.SuspendLayout();
            this.tpScreenshots.SuspendLayout();
            this.tlpScreenshots.SuspendLayout();
            this.tlpScreenshotProps.SuspendLayout();
            this.tpPublish.SuspendLayout();
            this.tcPublish.SuspendLayout();
            this.tpPublishGeneral.SuspendLayout();
            this.tlpPublish.SuspendLayout();
            this.gbQuickPublish.SuspendLayout();
            this.flpPublishConfig.SuspendLayout();
            this.tpPublishTemplates.SuspendLayout();
            this.gbTemplatesInternal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSizeIncr)).BeginInit();
            this.gbFonts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading1Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading2Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading3Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBodySize)).BeginInit();
            this.tpDebug.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.tcSettings.SuspendLayout();
            this.tpSettingsGeneral.SuspendLayout();
            this.tpSettingsProfiles.SuspendLayout();
            this.gbUploadScreenshots.SuspendLayout();
            this.tpSettingsProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).BeginInit();
            this.tscMain.BottomToolStripPanel.SuspendLayout();
            this.tscMain.ContentPanel.SuspendLayout();
            this.tscMain.TopToolStripPanel.SuspendLayout();
            this.tscMain.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.flpMediaInfoOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // bwApp
            // 
            this.bwApp.WorkerReportsProgress = true;
            this.bwApp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwApp_DoWork);
            this.bwApp.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwApp_ProgressChanged);
            this.bwApp.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwApp_RunWorkerCompleted);
            // 
            // ssMain
            // 
            this.ssMain.Dock = System.Windows.Forms.DockStyle.None;
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbarIcon,
            this.sBar,
            this.tssPerc,
            this.pBar});
            this.ssMain.Location = new System.Drawing.Point(0, 0);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(892, 22);
            this.ssMain.TabIndex = 0;
            this.ssMain.Text = "statusStrip1";
            // 
            // sbarIcon
            // 
            this.sbarIcon.Image = ((System.Drawing.Image)(resources.GetObject("sbarIcon.Image")));
            this.sbarIcon.Margin = new System.Windows.Forms.Padding(3, 3, 0, 2);
            this.sbarIcon.Name = "sbarIcon";
            this.sbarIcon.Size = new System.Drawing.Size(16, 17);
            // 
            // sBar
            // 
            this.sBar.Margin = new System.Windows.Forms.Padding(3, 3, 0, 2);
            this.sBar.Name = "sBar";
            this.sBar.Size = new System.Drawing.Size(749, 17);
            this.sBar.Spring = true;
            this.sBar.Text = "Ready";
            this.sBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssPerc
            // 
            this.tssPerc.Name = "tssPerc";
            this.tssPerc.Size = new System.Drawing.Size(0, 17);
            // 
            // pBar
            // 
            this.pBar.Margin = new System.Windows.Forms.Padding(1, 3, 5, 3);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(100, 16);
            // 
            // tmrStatus
            // 
            this.tmrStatus.Enabled = true;
            this.tmrStatus.Interval = 1000;
            this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
            // 
            // txtWebLink
            // 
            this.txtWebLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebLink.Location = new System.Drawing.Point(112, 72);
            this.txtWebLink.Name = "txtWebLink";
            this.txtWebLink.Size = new System.Drawing.Size(468, 20);
            this.txtWebLink.TabIndex = 5;
            this.ttApp.SetToolTip(this.txtWebLink, "IMDB URL");
            this.txtWebLink.TextChanged += new System.EventHandler(this.txtWebLink_TextChanged);
            // 
            // msMain
            // 
            this.msMain.Dock = System.Windows.Forms.DockStyle.None;
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tsmiEditTools,
            this.foldersToolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.msMain.Size = new System.Drawing.Size(892, 24);
            this.msMain.TabIndex = 0;
            this.msMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFileOpenFile,
            this.miFileOpenFolder,
            this.toolStripSeparator,
            this.miFileSaveTorrent,
            this.miFileSaveInfoAs,
            this.toolStripSeparator3,
            this.miFileExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // tsmiFileOpenFile
            // 
            this.tsmiFileOpenFile.Image = global::TDMakerGUI.Properties.Resources.folder_open_film;
            this.tsmiFileOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiFileOpenFile.Name = "tsmiFileOpenFile";
            this.tsmiFileOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiFileOpenFile.Size = new System.Drawing.Size(221, 22);
            this.tsmiFileOpenFile.Text = "&Open file...";
            this.tsmiFileOpenFile.Click += new System.EventHandler(this.miFileOpenFile_Click);
            // 
            // miFileOpenFolder
            // 
            this.miFileOpenFolder.Image = global::TDMakerGUI.Properties.Resources.folder_open;
            this.miFileOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miFileOpenFolder.Name = "miFileOpenFolder";
            this.miFileOpenFolder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.miFileOpenFolder.Size = new System.Drawing.Size(221, 22);
            this.miFileOpenFolder.Text = "&Open folder...";
            this.miFileOpenFolder.Click += new System.EventHandler(this.miFileOpenFolder_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(218, 6);
            // 
            // miFileSaveTorrent
            // 
            this.miFileSaveTorrent.Image = global::TDMakerGUI.Properties.Resources.disk_return;
            this.miFileSaveTorrent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miFileSaveTorrent.Name = "miFileSaveTorrent";
            this.miFileSaveTorrent.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSaveTorrent.Size = new System.Drawing.Size(221, 22);
            this.miFileSaveTorrent.Text = "&Save torrent";
            this.miFileSaveTorrent.Click += new System.EventHandler(this.miFileSaveTorrent_Click);
            // 
            // miFileSaveInfoAs
            // 
            this.miFileSaveInfoAs.Image = global::TDMakerGUI.Properties.Resources.disk__pencil;
            this.miFileSaveInfoAs.Name = "miFileSaveInfoAs";
            this.miFileSaveInfoAs.Size = new System.Drawing.Size(221, 22);
            this.miFileSaveInfoAs.Text = "&Save publish info as...";
            this.miFileSaveInfoAs.Click += new System.EventHandler(this.miFileSaveInfoAs_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(218, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Image = global::TDMakerGUI.Properties.Resources.cross;
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(221, 22);
            this.miFileExit.Text = "E&xit";
            this.miFileExit.Click += new System.EventHandler(this.miFileExit_Click);
            // 
            // tsmiEditTools
            // 
            this.tsmiEditTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditCopy});
            this.tsmiEditTools.Name = "tsmiEditTools";
            this.tsmiEditTools.Size = new System.Drawing.Size(39, 20);
            this.tsmiEditTools.Text = "&Edit";
            // 
            // miEditCopy
            // 
            this.miEditCopy.Image = global::TDMakerGUI.Properties.Resources.document_copy;
            this.miEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miEditCopy.Size = new System.Drawing.Size(210, 22);
            this.miEditCopy.Text = "&Copy publish info";
            this.miEditCopy.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // foldersToolStripMenuItem1
            // 
            this.foldersToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFoldersScreenshots,
            this.miFoldersTorrents,
            this.toolStripSeparator4,
            this.miFoldersLogs,
            this.miFoldersSettings,
            this.miFoldersTemplates});
            this.foldersToolStripMenuItem1.Name = "foldersToolStripMenuItem1";
            this.foldersToolStripMenuItem1.Size = new System.Drawing.Size(57, 20);
            this.foldersToolStripMenuItem1.Text = "&Folders";
            // 
            // miFoldersScreenshots
            // 
            this.miFoldersScreenshots.Image = global::TDMakerGUI.Properties.Resources.images;
            this.miFoldersScreenshots.Name = "miFoldersScreenshots";
            this.miFoldersScreenshots.Size = new System.Drawing.Size(146, 22);
            this.miFoldersScreenshots.Text = "&Screenshots...";
            this.miFoldersScreenshots.Click += new System.EventHandler(this.miFoldersScreenshots_Click);
            // 
            // miFoldersTorrents
            // 
            this.miFoldersTorrents.Image = global::TDMakerGUI.Properties.Resources.application_cloud;
            this.miFoldersTorrents.Name = "miFoldersTorrents";
            this.miFoldersTorrents.Size = new System.Drawing.Size(146, 22);
            this.miFoldersTorrents.Text = "&Torrents...";
            this.miFoldersTorrents.Click += new System.EventHandler(this.miFoldersTorrents_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(143, 6);
            // 
            // miFoldersLogs
            // 
            this.miFoldersLogs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFoldersLogsDebug});
            this.miFoldersLogs.Image = global::TDMakerGUI.Properties.Resources.folder_open_document_text;
            this.miFoldersLogs.Name = "miFoldersLogs";
            this.miFoldersLogs.Size = new System.Drawing.Size(146, 22);
            this.miFoldersLogs.Text = "&Logs...";
            this.miFoldersLogs.Click += new System.EventHandler(this.miFoldersLogs_Click);
            // 
            // miFoldersLogsDebug
            // 
            this.miFoldersLogsDebug.Image = global::TDMakerGUI.Properties.Resources.application_monitor;
            this.miFoldersLogsDebug.Name = "miFoldersLogsDebug";
            this.miFoldersLogsDebug.Size = new System.Drawing.Size(118, 22);
            this.miFoldersLogsDebug.Text = "&Debug...";
            this.miFoldersLogsDebug.Click += new System.EventHandler(this.miFoldersLogsDebug_Click);
            // 
            // miFoldersSettings
            // 
            this.miFoldersSettings.Image = global::TDMakerGUI.Properties.Resources.wrench_screwdriver;
            this.miFoldersSettings.Name = "miFoldersSettings";
            this.miFoldersSettings.Size = new System.Drawing.Size(146, 22);
            this.miFoldersSettings.Text = "&Settings...";
            this.miFoldersSettings.Click += new System.EventHandler(this.miFoldersSettings_Click);
            // 
            // miFoldersTemplates
            // 
            this.miFoldersTemplates.Image = global::TDMakerGUI.Properties.Resources.layout_design;
            this.miFoldersTemplates.Name = "miFoldersTemplates";
            this.miFoldersTemplates.Size = new System.Drawing.Size(146, 22);
            this.miFoldersTemplates.Text = "&Templates...";
            this.miFoldersTemplates.Click += new System.EventHandler(this.miFoldersTemplates_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpVersionHistory,
            this.tsmiAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // miHelpVersionHistory
            // 
            this.miHelpVersionHistory.Image = global::TDMakerGUI.Properties.Resources.application_task;
            this.miHelpVersionHistory.Name = "miHelpVersionHistory";
            this.miHelpVersionHistory.Size = new System.Drawing.Size(161, 22);
            this.miHelpVersionHistory.Text = "&Version history...";
            this.miHelpVersionHistory.Click += new System.EventHandler(this.miHelpVersionHistory_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Image = global::TDMakerGUI.Properties.Resources.crown;
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(161, 22);
            this.tsmiAbout.Text = "&About...";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // btnPublish
            // 
            this.btnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPublish.Enabled = false;
            this.btnPublish.Location = new System.Drawing.Point(252, 4);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(120, 23);
            this.btnPublish.TabIndex = 2;
            this.btnPublish.Text = "&Copy publish info";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAnalyze.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAnalyze.Enabled = false;
            this.btnAnalyze.Location = new System.Drawing.Point(0, 3);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(120, 24);
            this.btnAnalyze.TabIndex = 0;
            this.btnAnalyze.Text = "&Create description";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnCreateTorrent
            // 
            this.btnCreateTorrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateTorrent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCreateTorrent.Location = new System.Drawing.Point(126, 4);
            this.btnCreateTorrent.Name = "btnCreateTorrent";
            this.btnCreateTorrent.Size = new System.Drawing.Size(120, 23);
            this.btnCreateTorrent.TabIndex = 1;
            this.btnCreateTorrent.Text = "Create &torrent";
            this.btnCreateTorrent.UseVisualStyleBackColor = true;
            this.btnCreateTorrent.Click += new System.EventHandler(this.btnCreateTorrent_Click);
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpMedia);
            this.tcMain.Controls.Add(this.tpMediaInfo);
            this.tcMain.Controls.Add(this.tpScreenshots);
            this.tcMain.Controls.Add(this.tpPublish);
            this.tcMain.Controls.Add(this.tpDebug);
            this.tcMain.Controls.Add(this.tpSettings);
            this.tcMain.Location = new System.Drawing.Point(3, 3);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(886, 549);
            this.tcMain.TabIndex = 0;
            this.tcMain.Visible = false;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpMedia
            // 
            this.tpMedia.Controls.Add(this.gbSource);
            this.tpMedia.Controls.Add(this.gbDVD);
            this.tpMedia.Controls.Add(this.gbLocation);
            this.tpMedia.Location = new System.Drawing.Point(4, 22);
            this.tpMedia.Name = "tpMedia";
            this.tpMedia.Size = new System.Drawing.Size(878, 523);
            this.tpMedia.TabIndex = 4;
            this.tpMedia.Text = "Input";
            this.tpMedia.UseVisualStyleBackColor = true;
            // 
            // gbSource
            // 
            this.gbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSource.Controls.Add(this.chkTitle);
            this.gbSource.Controls.Add(this.txtTitle);
            this.gbSource.Controls.Add(this.chkWebLink);
            this.gbSource.Controls.Add(this.chkSource);
            this.gbSource.Controls.Add(this.txtWebLink);
            this.gbSource.Controls.Add(this.cboSource);
            this.gbSource.Location = new System.Drawing.Point(272, 413);
            this.gbSource.Name = "gbSource";
            this.gbSource.Size = new System.Drawing.Size(596, 104);
            this.gbSource.TabIndex = 2;
            this.gbSource.TabStop = false;
            this.gbSource.Text = "Source properties";
            // 
            // chkTitle
            // 
            this.chkTitle.AutoSize = true;
            this.chkTitle.Checked = true;
            this.chkTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTitle.Location = new System.Drawing.Point(16, 50);
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.Size = new System.Drawing.Size(49, 17);
            this.chkTitle.TabIndex = 2;
            this.chkTitle.Text = "&Title:";
            this.chkTitle.UseVisualStyleBackColor = true;
            this.chkTitle.CheckedChanged += new System.EventHandler(this.chkTitle_CheckedChanged);
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Location = new System.Drawing.Point(112, 48);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(468, 20);
            this.txtTitle.TabIndex = 3;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // chkWebLink
            // 
            this.chkWebLink.AutoSize = true;
            this.chkWebLink.Location = new System.Drawing.Point(16, 74);
            this.chkWebLink.Name = "chkWebLink";
            this.chkWebLink.Size = new System.Drawing.Size(71, 17);
            this.chkWebLink.TabIndex = 4;
            this.chkWebLink.Text = "&Web link:";
            this.chkWebLink.UseVisualStyleBackColor = true;
            this.chkWebLink.CheckedChanged += new System.EventHandler(this.chkWebLink_CheckedChanged);
            // 
            // chkSource
            // 
            this.chkSource.AutoSize = true;
            this.chkSource.Checked = true;
            this.chkSource.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkSource.Location = new System.Drawing.Point(16, 26);
            this.chkSource.Name = "chkSource";
            this.chkSource.Size = new System.Drawing.Size(60, 17);
            this.chkSource.TabIndex = 0;
            this.chkSource.Text = "&Source";
            this.chkSource.UseVisualStyleBackColor = true;
            this.chkSource.CheckedChanged += new System.EventHandler(this.chkSource_CheckedChanged);
            // 
            // cboSource
            // 
            this.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(112, 24);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(203, 21);
            this.cboSource.TabIndex = 1;
            this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboSource_SelectedIndexChanged);
            // 
            // gbDVD
            // 
            this.gbDVD.Controls.Add(this.cboDiscMenu);
            this.gbDVD.Controls.Add(this.chkDiscMenu);
            this.gbDVD.Controls.Add(this.cboExtras);
            this.gbDVD.Controls.Add(this.chkExtras);
            this.gbDVD.Controls.Add(this.cboAuthoring);
            this.gbDVD.Controls.Add(this.chkAuthoring);
            this.gbDVD.Location = new System.Drawing.Point(6, 413);
            this.gbDVD.Name = "gbDVD";
            this.gbDVD.Size = new System.Drawing.Size(256, 104);
            this.gbDVD.TabIndex = 1;
            this.gbDVD.TabStop = false;
            this.gbDVD.Text = "DVD properties";
            // 
            // cboDiscMenu
            // 
            this.cboDiscMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiscMenu.FormattingEnabled = true;
            this.cboDiscMenu.Location = new System.Drawing.Point(120, 48);
            this.cboDiscMenu.Name = "cboDiscMenu";
            this.cboDiscMenu.Size = new System.Drawing.Size(121, 21);
            this.cboDiscMenu.TabIndex = 3;
            this.cboDiscMenu.SelectedIndexChanged += new System.EventHandler(this.cboDiscMenu_SelectedIndexChanged);
            // 
            // chkDiscMenu
            // 
            this.chkDiscMenu.AutoSize = true;
            this.chkDiscMenu.Checked = true;
            this.chkDiscMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDiscMenu.Location = new System.Drawing.Point(16, 51);
            this.chkDiscMenu.Name = "chkDiscMenu";
            this.chkDiscMenu.Size = new System.Drawing.Size(56, 17);
            this.chkDiscMenu.TabIndex = 2;
            this.chkDiscMenu.Text = "Menu:";
            this.chkDiscMenu.UseVisualStyleBackColor = true;
            this.chkDiscMenu.CheckedChanged += new System.EventHandler(this.chkDiscMenu_CheckedChanged);
            // 
            // cboExtras
            // 
            this.cboExtras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExtras.FormattingEnabled = true;
            this.cboExtras.Location = new System.Drawing.Point(120, 72);
            this.cboExtras.Name = "cboExtras";
            this.cboExtras.Size = new System.Drawing.Size(121, 21);
            this.cboExtras.TabIndex = 5;
            this.cboExtras.SelectedIndexChanged += new System.EventHandler(this.cboExtras_SelectedIndexChanged);
            // 
            // chkExtras
            // 
            this.chkExtras.AutoSize = true;
            this.chkExtras.Checked = true;
            this.chkExtras.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExtras.Location = new System.Drawing.Point(16, 75);
            this.chkExtras.Name = "chkExtras";
            this.chkExtras.Size = new System.Drawing.Size(58, 17);
            this.chkExtras.TabIndex = 4;
            this.chkExtras.Text = "E&xtras:";
            this.chkExtras.UseVisualStyleBackColor = true;
            this.chkExtras.CheckedChanged += new System.EventHandler(this.chkExtras_CheckedChanged);
            // 
            // cboAuthoring
            // 
            this.cboAuthoring.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAuthoring.FormattingEnabled = true;
            this.cboAuthoring.Location = new System.Drawing.Point(120, 24);
            this.cboAuthoring.Name = "cboAuthoring";
            this.cboAuthoring.Size = new System.Drawing.Size(121, 21);
            this.cboAuthoring.TabIndex = 1;
            this.cboAuthoring.SelectedIndexChanged += new System.EventHandler(this.cboAuthoring_SelectedIndexChanged);
            // 
            // chkAuthoring
            // 
            this.chkAuthoring.AutoSize = true;
            this.chkAuthoring.Checked = true;
            this.chkAuthoring.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuthoring.Location = new System.Drawing.Point(16, 27);
            this.chkAuthoring.Name = "chkAuthoring";
            this.chkAuthoring.Size = new System.Drawing.Size(74, 17);
            this.chkAuthoring.TabIndex = 0;
            this.chkAuthoring.Text = "Authoring:";
            this.chkAuthoring.UseVisualStyleBackColor = true;
            this.chkAuthoring.CheckedChanged += new System.EventHandler(this.chkSourceEdit_CheckedChanged);
            // 
            // gbLocation
            // 
            this.gbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLocation.Controls.Add(this.btnBrowseDir);
            this.gbLocation.Controls.Add(this.lbFiles);
            this.gbLocation.Controls.Add(this.btnBrowse);
            this.gbLocation.Controls.Add(this.pbLogo);
            this.gbLocation.Location = new System.Drawing.Point(8, 8);
            this.gbLocation.Name = "gbLocation";
            this.gbLocation.Size = new System.Drawing.Size(860, 399);
            this.gbLocation.TabIndex = 0;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Locations - Browse or drag and drop a movie file or folder";
            // 
            // btnBrowseDir
            // 
            this.btnBrowseDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowseDir.Location = new System.Drawing.Point(176, 24);
            this.btnBrowseDir.Name = "btnBrowseDir";
            this.btnBrowseDir.Size = new System.Drawing.Size(160, 23);
            this.btnBrowseDir.TabIndex = 1;
            this.btnBrowseDir.Text = "&Browse for a directory...";
            this.btnBrowseDir.UseVisualStyleBackColor = true;
            this.btnBrowseDir.Click += new System.EventHandler(this.btnBrowseDir_Click);
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.HorizontalScrollbar = true;
            this.lbFiles.IntegralHeight = false;
            this.lbFiles.Location = new System.Drawing.Point(8, 56);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(840, 336);
            this.lbFiles.Sorted = true;
            this.lbFiles.TabIndex = 2;
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);
            // 
            // btnBrowse
            // 
            this.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowse.Location = new System.Drawing.Point(8, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(160, 23);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "&Browse for a file or files...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbLogo.Location = new System.Drawing.Point(648, 16);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(200, 29);
            this.pbLogo.TabIndex = 11;
            this.pbLogo.TabStop = false;
            // 
            // tpMediaInfo
            // 
            this.tpMediaInfo.Controls.Add(this.tlpMediaInfo);
            this.tpMediaInfo.Location = new System.Drawing.Point(4, 22);
            this.tpMediaInfo.Name = "tpMediaInfo";
            this.tpMediaInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpMediaInfo.Size = new System.Drawing.Size(878, 523);
            this.tpMediaInfo.TabIndex = 0;
            this.tpMediaInfo.Text = "Media info";
            this.tpMediaInfo.UseVisualStyleBackColor = true;
            // 
            // tlpMediaInfo
            // 
            this.tlpMediaInfo.ColumnCount = 2;
            this.tlpMediaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMediaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpMediaInfo.Controls.Add(this.lbMediaInfo, 0, 0);
            this.tlpMediaInfo.Controls.Add(this.txtMediaInfo, 1, 0);
            this.tlpMediaInfo.Controls.Add(this.flpMediaInfoOptions, 1, 1);
            this.tlpMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMediaInfo.Location = new System.Drawing.Point(3, 3);
            this.tlpMediaInfo.Name = "tlpMediaInfo";
            this.tlpMediaInfo.RowCount = 2;
            this.tlpMediaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMediaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpMediaInfo.Size = new System.Drawing.Size(872, 517);
            this.tlpMediaInfo.TabIndex = 0;
            // 
            // lbMediaInfo
            // 
            this.lbMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMediaInfo.FormattingEnabled = true;
            this.lbMediaInfo.HorizontalScrollbar = true;
            this.lbMediaInfo.IntegralHeight = false;
            this.lbMediaInfo.Location = new System.Drawing.Point(3, 3);
            this.lbMediaInfo.Name = "lbMediaInfo";
            this.lbMediaInfo.Size = new System.Drawing.Size(168, 479);
            this.lbMediaInfo.TabIndex = 0;
            this.lbMediaInfo.SelectedIndexChanged += new System.EventHandler(this.LbMediaInfoSelectedIndexChanged);
            // 
            // txtMediaInfo
            // 
            this.txtMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMediaInfo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMediaInfo.Location = new System.Drawing.Point(177, 3);
            this.txtMediaInfo.Multiline = true;
            this.txtMediaInfo.Name = "txtMediaInfo";
            this.txtMediaInfo.ReadOnly = true;
            this.txtMediaInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMediaInfo.Size = new System.Drawing.Size(692, 479);
            this.txtMediaInfo.TabIndex = 1;
            // 
            // chkMediaInfoComplete
            // 
            this.chkMediaInfoComplete.AutoSize = true;
            this.chkMediaInfoComplete.Location = new System.Drawing.Point(2, 2);
            this.chkMediaInfoComplete.Margin = new System.Windows.Forms.Padding(2);
            this.chkMediaInfoComplete.Name = "chkMediaInfoComplete";
            this.chkMediaInfoComplete.Size = new System.Drawing.Size(143, 17);
            this.chkMediaInfoComplete.TabIndex = 0;
            this.chkMediaInfoComplete.Text = "View detailed information";
            this.chkMediaInfoComplete.UseVisualStyleBackColor = true;
            this.chkMediaInfoComplete.CheckedChanged += new System.EventHandler(this.chkMediaInfoComplete_CheckedChanged);
            // 
            // tpScreenshots
            // 
            this.tpScreenshots.Controls.Add(this.tlpScreenshots);
            this.tpScreenshots.Location = new System.Drawing.Point(4, 22);
            this.tpScreenshots.Name = "tpScreenshots";
            this.tpScreenshots.Padding = new System.Windows.Forms.Padding(3);
            this.tpScreenshots.Size = new System.Drawing.Size(878, 523);
            this.tpScreenshots.TabIndex = 1;
            this.tpScreenshots.Text = "Screenshots";
            this.tpScreenshots.UseVisualStyleBackColor = true;
            // 
            // tlpScreenshots
            // 
            this.tlpScreenshots.ColumnCount = 2;
            this.tlpScreenshots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpScreenshots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpScreenshots.Controls.Add(this.lbScreenshots, 0, 0);
            this.tlpScreenshots.Controls.Add(this.tlpScreenshotProps, 1, 0);
            this.tlpScreenshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpScreenshots.Location = new System.Drawing.Point(3, 3);
            this.tlpScreenshots.Name = "tlpScreenshots";
            this.tlpScreenshots.RowCount = 1;
            this.tlpScreenshots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpScreenshots.Size = new System.Drawing.Size(872, 517);
            this.tlpScreenshots.TabIndex = 0;
            // 
            // lbScreenshots
            // 
            this.lbScreenshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbScreenshots.FormattingEnabled = true;
            this.lbScreenshots.HorizontalScrollbar = true;
            this.lbScreenshots.IntegralHeight = false;
            this.lbScreenshots.Location = new System.Drawing.Point(3, 3);
            this.lbScreenshots.Name = "lbScreenshots";
            this.lbScreenshots.Size = new System.Drawing.Size(255, 511);
            this.lbScreenshots.TabIndex = 0;
            this.lbScreenshots.SelectedIndexChanged += new System.EventHandler(this.lbScreenshots_SelectedIndexChanged);
            // 
            // tlpScreenshotProps
            // 
            this.tlpScreenshotProps.ColumnCount = 1;
            this.tlpScreenshotProps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpScreenshotProps.Controls.Add(this.pbScreenshot, 0, 0);
            this.tlpScreenshotProps.Controls.Add(this.pgScreenshot, 0, 1);
            this.tlpScreenshotProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpScreenshotProps.Location = new System.Drawing.Point(261, 0);
            this.tlpScreenshotProps.Margin = new System.Windows.Forms.Padding(0);
            this.tlpScreenshotProps.Name = "tlpScreenshotProps";
            this.tlpScreenshotProps.RowCount = 2;
            this.tlpScreenshotProps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpScreenshotProps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpScreenshotProps.Size = new System.Drawing.Size(611, 517);
            this.tlpScreenshotProps.TabIndex = 1;
            // 
            // pbScreenshot
            // 
            this.pbScreenshot.BackColor = System.Drawing.Color.White;
            this.pbScreenshot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbScreenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScreenshot.Location = new System.Drawing.Point(3, 3);
            this.pbScreenshot.Name = "pbScreenshot";
            this.pbScreenshot.Size = new System.Drawing.Size(605, 407);
            this.pbScreenshot.TabIndex = 0;
            this.pbScreenshot.TabStop = false;
            this.pbScreenshot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbScreenshot_MouseDown);
            // 
            // pgScreenshot
            // 
            this.pgScreenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgScreenshot.HelpVisible = false;
            this.pgScreenshot.Location = new System.Drawing.Point(3, 416);
            this.pgScreenshot.Name = "pgScreenshot";
            this.pgScreenshot.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgScreenshot.Size = new System.Drawing.Size(605, 98);
            this.pgScreenshot.TabIndex = 1;
            this.pgScreenshot.ToolbarVisible = false;
            // 
            // tpPublish
            // 
            this.tpPublish.Controls.Add(this.tcPublish);
            this.tpPublish.Location = new System.Drawing.Point(4, 22);
            this.tpPublish.Name = "tpPublish";
            this.tpPublish.Padding = new System.Windows.Forms.Padding(3);
            this.tpPublish.Size = new System.Drawing.Size(878, 523);
            this.tpPublish.TabIndex = 2;
            this.tpPublish.Text = "Publish";
            this.tpPublish.UseVisualStyleBackColor = true;
            // 
            // tcPublish
            // 
            this.tcPublish.Controls.Add(this.tpPublishGeneral);
            this.tcPublish.Controls.Add(this.tpPublishTemplates);
            this.tcPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPublish.Location = new System.Drawing.Point(3, 3);
            this.tcPublish.Name = "tcPublish";
            this.tcPublish.SelectedIndex = 0;
            this.tcPublish.Size = new System.Drawing.Size(872, 517);
            this.tcPublish.TabIndex = 0;
            // 
            // tpPublishGeneral
            // 
            this.tpPublishGeneral.Controls.Add(this.tlpPublish);
            this.tpPublishGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpPublishGeneral.Name = "tpPublishGeneral";
            this.tpPublishGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpPublishGeneral.Size = new System.Drawing.Size(864, 491);
            this.tpPublishGeneral.TabIndex = 0;
            this.tpPublishGeneral.UseVisualStyleBackColor = true;
            // 
            // tlpPublish
            // 
            this.tlpPublish.ColumnCount = 3;
            this.tlpPublish.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00001F));
            this.tlpPublish.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.99999F));
            this.tlpPublish.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 201F));
            this.tlpPublish.Controls.Add(this.gbQuickPublish, 2, 0);
            this.tlpPublish.Controls.Add(this.txtPublish, 1, 0);
            this.tlpPublish.Controls.Add(this.lbPublish, 0, 0);
            this.tlpPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPublish.Location = new System.Drawing.Point(3, 3);
            this.tlpPublish.Name = "tlpPublish";
            this.tlpPublish.RowCount = 1;
            this.tlpPublish.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPublish.Size = new System.Drawing.Size(858, 485);
            this.tlpPublish.TabIndex = 0;
            // 
            // gbQuickPublish
            // 
            this.gbQuickPublish.Controls.Add(this.flpPublishConfig);
            this.gbQuickPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQuickPublish.Location = new System.Drawing.Point(659, 3);
            this.gbQuickPublish.Name = "gbQuickPublish";
            this.gbQuickPublish.Size = new System.Drawing.Size(196, 479);
            this.gbQuickPublish.TabIndex = 2;
            this.gbQuickPublish.TabStop = false;
            this.gbQuickPublish.Text = "Quick Options";
            // 
            // flpPublishConfig
            // 
            this.flpPublishConfig.Controls.Add(this.chkQuickPre);
            this.flpPublishConfig.Controls.Add(this.chkQuickAlignCenter);
            this.flpPublishConfig.Controls.Add(this.chkQuickFullPicture);
            this.flpPublishConfig.Controls.Add(this.cboQuickPublishType);
            this.flpPublishConfig.Controls.Add(this.cboQuickTemplate);
            this.flpPublishConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPublishConfig.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpPublishConfig.Location = new System.Drawing.Point(3, 16);
            this.flpPublishConfig.Name = "flpPublishConfig";
            this.flpPublishConfig.Size = new System.Drawing.Size(190, 460);
            this.flpPublishConfig.TabIndex = 0;
            // 
            // chkQuickPre
            // 
            this.chkQuickPre.AutoSize = true;
            this.chkQuickPre.Location = new System.Drawing.Point(3, 3);
            this.chkQuickPre.Name = "chkQuickPre";
            this.chkQuickPre.Size = new System.Drawing.Size(110, 17);
            this.chkQuickPre.TabIndex = 0;
            this.chkQuickPre.Text = "&Preformatted Text";
            this.chkQuickPre.UseVisualStyleBackColor = true;
            this.chkQuickPre.CheckedChanged += new System.EventHandler(this.chkQuickPre_CheckedChanged);
            // 
            // chkQuickAlignCenter
            // 
            this.chkQuickAlignCenter.AutoSize = true;
            this.chkQuickAlignCenter.Location = new System.Drawing.Point(3, 26);
            this.chkQuickAlignCenter.Name = "chkQuickAlignCenter";
            this.chkQuickAlignCenter.Size = new System.Drawing.Size(83, 17);
            this.chkQuickAlignCenter.TabIndex = 1;
            this.chkQuickAlignCenter.Text = "Align &Center";
            this.chkQuickAlignCenter.UseVisualStyleBackColor = true;
            this.chkQuickAlignCenter.CheckedChanged += new System.EventHandler(this.chkQuickAlignCenter_CheckedChanged);
            // 
            // chkQuickFullPicture
            // 
            this.chkQuickFullPicture.AutoSize = true;
            this.chkQuickFullPicture.Location = new System.Drawing.Point(3, 49);
            this.chkQuickFullPicture.Name = "chkQuickFullPicture";
            this.chkQuickFullPicture.Size = new System.Drawing.Size(78, 17);
            this.chkQuickFullPicture.TabIndex = 2;
            this.chkQuickFullPicture.Text = "Full &Picture";
            this.chkQuickFullPicture.UseVisualStyleBackColor = true;
            this.chkQuickFullPicture.CheckedChanged += new System.EventHandler(this.chkQuickFullPicture_CheckedChanged);
            // 
            // cboQuickPublishType
            // 
            this.cboQuickPublishType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuickPublishType.FormattingEnabled = true;
            this.cboQuickPublishType.Location = new System.Drawing.Point(3, 72);
            this.cboQuickPublishType.Name = "cboQuickPublishType";
            this.cboQuickPublishType.Size = new System.Drawing.Size(121, 21);
            this.cboQuickPublishType.TabIndex = 3;
            this.cboQuickPublishType.SelectedIndexChanged += new System.EventHandler(this.cboPublishType_SelectedIndexChanged);
            // 
            // cboQuickTemplate
            // 
            this.cboQuickTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboQuickTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuickTemplate.FormattingEnabled = true;
            this.cboQuickTemplate.Location = new System.Drawing.Point(3, 99);
            this.cboQuickTemplate.Name = "cboQuickTemplate";
            this.cboQuickTemplate.Size = new System.Drawing.Size(121, 21);
            this.cboQuickTemplate.TabIndex = 4;
            this.cboQuickTemplate.SelectedIndexChanged += new System.EventHandler(this.cboQuickTemplate_SelectedIndexChanged);
            // 
            // txtPublish
            // 
            this.txtPublish.AcceptsReturn = true;
            this.txtPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPublish.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPublish.Location = new System.Drawing.Point(134, 3);
            this.txtPublish.Multiline = true;
            this.txtPublish.Name = "txtPublish";
            this.txtPublish.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPublish.Size = new System.Drawing.Size(519, 479);
            this.txtPublish.TabIndex = 1;
            this.txtPublish.WordWrap = false;
            this.txtPublish.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPublish_KeyPress);
            // 
            // lbPublish
            // 
            this.lbPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPublish.FormattingEnabled = true;
            this.lbPublish.IntegralHeight = false;
            this.lbPublish.Location = new System.Drawing.Point(3, 3);
            this.lbPublish.Name = "lbPublish";
            this.lbPublish.Size = new System.Drawing.Size(125, 479);
            this.lbPublish.TabIndex = 0;
            this.lbPublish.SelectedIndexChanged += new System.EventHandler(this.LbPublishSelectedIndexChanged);
            // 
            // tpPublishTemplates
            // 
            this.tpPublishTemplates.Controls.Add(this.cboPublishType);
            this.tpPublishTemplates.Controls.Add(this.chkTemplatesMode);
            this.tpPublishTemplates.Controls.Add(this.gbTemplatesInternal);
            this.tpPublishTemplates.Controls.Add(this.btnTemplatesRewrite);
            this.tpPublishTemplates.Controls.Add(this.gbFonts);
            this.tpPublishTemplates.Controls.Add(this.chkUploadFullScreenshot);
            this.tpPublishTemplates.Controls.Add(this.cboTemplate);
            this.tpPublishTemplates.Location = new System.Drawing.Point(4, 22);
            this.tpPublishTemplates.Name = "tpPublishTemplates";
            this.tpPublishTemplates.Padding = new System.Windows.Forms.Padding(3);
            this.tpPublishTemplates.Size = new System.Drawing.Size(864, 491);
            this.tpPublishTemplates.TabIndex = 3;
            this.tpPublishTemplates.Text = "Templates";
            this.tpPublishTemplates.UseVisualStyleBackColor = true;
            // 
            // cboPublishType
            // 
            this.cboPublishType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPublishType.FormattingEnabled = true;
            this.cboPublishType.Location = new System.Drawing.Point(160, 13);
            this.cboPublishType.Name = "cboPublishType";
            this.cboPublishType.Size = new System.Drawing.Size(195, 21);
            this.cboPublishType.TabIndex = 1;
            this.cboPublishType.SelectedIndexChanged += new System.EventHandler(this.cboPublishType_SelectedIndexChanged_1);
            // 
            // chkTemplatesMode
            // 
            this.chkTemplatesMode.AutoSize = true;
            this.chkTemplatesMode.Checked = true;
            this.chkTemplatesMode.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkTemplatesMode.Location = new System.Drawing.Point(16, 16);
            this.chkTemplatesMode.Name = "chkTemplatesMode";
            this.chkTemplatesMode.Size = new System.Drawing.Size(139, 17);
            this.chkTemplatesMode.TabIndex = 0;
            this.chkTemplatesMode.Text = "Create description using";
            this.chkTemplatesMode.UseVisualStyleBackColor = true;
            this.chkTemplatesMode.CheckedChanged += new System.EventHandler(this.chkTemplatesMode_CheckedChanged);
            // 
            // gbTemplatesInternal
            // 
            this.gbTemplatesInternal.Controls.Add(this.nudFontSizeIncr);
            this.gbTemplatesInternal.Controls.Add(this.chkPre);
            this.gbTemplatesInternal.Controls.Add(this.chkPreIncreaseFontSize);
            this.gbTemplatesInternal.Controls.Add(this.chkAlignCenter);
            this.gbTemplatesInternal.Location = new System.Drawing.Point(16, 72);
            this.gbTemplatesInternal.Name = "gbTemplatesInternal";
            this.gbTemplatesInternal.Size = new System.Drawing.Size(431, 148);
            this.gbTemplatesInternal.TabIndex = 4;
            this.gbTemplatesInternal.TabStop = false;
            this.gbTemplatesInternal.Text = "Internal template settings";
            // 
            // nudFontSizeIncr
            // 
            this.nudFontSizeIncr.Location = new System.Drawing.Point(319, 55);
            this.nudFontSizeIncr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFontSizeIncr.Name = "nudFontSizeIncr";
            this.nudFontSizeIncr.Size = new System.Drawing.Size(55, 20);
            this.nudFontSizeIncr.TabIndex = 3;
            this.nudFontSizeIncr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFontSizeIncr.ValueChanged += new System.EventHandler(this.nudFontSizeIncr_ValueChanged);
            // 
            // chkPre
            // 
            this.chkPre.AutoSize = true;
            this.chkPre.Location = new System.Drawing.Point(17, 56);
            this.chkPre.Name = "chkPre";
            this.chkPre.Size = new System.Drawing.Size(127, 17);
            this.chkPre.TabIndex = 1;
            this.chkPre.Text = "Use preformatted text";
            this.chkPre.UseVisualStyleBackColor = true;
            this.chkPre.CheckedChanged += new System.EventHandler(this.chkPre_CheckedChanged);
            // 
            // chkPreIncreaseFontSize
            // 
            this.chkPreIncreaseFontSize.AutoSize = true;
            this.chkPreIncreaseFontSize.Checked = true;
            this.chkPreIncreaseFontSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreIncreaseFontSize.Location = new System.Drawing.Point(165, 56);
            this.chkPreIncreaseFontSize.Name = "chkPreIncreaseFontSize";
            this.chkPreIncreaseFontSize.Size = new System.Drawing.Size(143, 17);
            this.chkPreIncreaseFontSize.TabIndex = 2;
            this.chkPreIncreaseFontSize.Text = "and increase font size by";
            this.chkPreIncreaseFontSize.UseVisualStyleBackColor = true;
            this.chkPreIncreaseFontSize.CheckedChanged += new System.EventHandler(this.chkPreIncreaseFontSize_CheckedChanged);
            // 
            // chkAlignCenter
            // 
            this.chkAlignCenter.AutoSize = true;
            this.chkAlignCenter.Location = new System.Drawing.Point(17, 33);
            this.chkAlignCenter.Name = "chkAlignCenter";
            this.chkAlignCenter.Size = new System.Drawing.Size(82, 17);
            this.chkAlignCenter.TabIndex = 0;
            this.chkAlignCenter.Text = "Align &center";
            this.chkAlignCenter.UseVisualStyleBackColor = true;
            this.chkAlignCenter.CheckedChanged += new System.EventHandler(this.chkAlignCenter_CheckedChanged);
            // 
            // btnTemplatesRewrite
            // 
            this.btnTemplatesRewrite.AutoSize = true;
            this.btnTemplatesRewrite.Location = new System.Drawing.Point(16, 233);
            this.btnTemplatesRewrite.Name = "btnTemplatesRewrite";
            this.btnTemplatesRewrite.Size = new System.Drawing.Size(196, 27);
            this.btnTemplatesRewrite.TabIndex = 6;
            this.btnTemplatesRewrite.Text = "&Rewrite default templates...";
            this.btnTemplatesRewrite.UseVisualStyleBackColor = true;
            this.btnTemplatesRewrite.Click += new System.EventHandler(this.btnTemplatesRewrite_Click);
            // 
            // gbFonts
            // 
            this.gbFonts.Controls.Add(this.nudHeading1Size);
            this.gbFonts.Controls.Add(this.label9);
            this.gbFonts.Controls.Add(this.nudHeading2Size);
            this.gbFonts.Controls.Add(this.label8);
            this.gbFonts.Controls.Add(this.nudHeading3Size);
            this.gbFonts.Controls.Add(this.label7);
            this.gbFonts.Controls.Add(this.nudBodySize);
            this.gbFonts.Controls.Add(this.label3);
            this.gbFonts.Location = new System.Drawing.Point(455, 72);
            this.gbFonts.Name = "gbFonts";
            this.gbFonts.Size = new System.Drawing.Size(190, 148);
            this.gbFonts.TabIndex = 5;
            this.gbFonts.TabStop = false;
            this.gbFonts.Text = "Font size";
            // 
            // nudHeading1Size
            // 
            this.nudHeading1Size.Location = new System.Drawing.Point(78, 22);
            this.nudHeading1Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading1Size.Name = "nudHeading1Size";
            this.nudHeading1Size.Size = new System.Drawing.Size(55, 20);
            this.nudHeading1Size.TabIndex = 1;
            this.nudHeading1Size.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading1Size.ValueChanged += new System.EventHandler(this.nudFontSizeHeading1_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Heading 1";
            // 
            // nudHeading2Size
            // 
            this.nudHeading2Size.Location = new System.Drawing.Point(78, 45);
            this.nudHeading2Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading2Size.Name = "nudHeading2Size";
            this.nudHeading2Size.Size = new System.Drawing.Size(55, 20);
            this.nudHeading2Size.TabIndex = 3;
            this.nudHeading2Size.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading2Size.ValueChanged += new System.EventHandler(this.nudHeading2Size_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Heading 2";
            // 
            // nudHeading3Size
            // 
            this.nudHeading3Size.Location = new System.Drawing.Point(78, 71);
            this.nudHeading3Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading3Size.Name = "nudHeading3Size";
            this.nudHeading3Size.Size = new System.Drawing.Size(55, 20);
            this.nudHeading3Size.TabIndex = 5;
            this.nudHeading3Size.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading3Size.ValueChanged += new System.EventHandler(this.nudHeading3Size_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Body";
            // 
            // nudBodySize
            // 
            this.nudBodySize.Location = new System.Drawing.Point(78, 97);
            this.nudBodySize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBodySize.Name = "nudBodySize";
            this.nudBodySize.Size = new System.Drawing.Size(55, 20);
            this.nudBodySize.TabIndex = 7;
            this.nudBodySize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBodySize.ValueChanged += new System.EventHandler(this.nudBodyText_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Heading 3";
            // 
            // chkUploadFullScreenshot
            // 
            this.chkUploadFullScreenshot.AutoSize = true;
            this.chkUploadFullScreenshot.Checked = true;
            this.chkUploadFullScreenshot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUploadFullScreenshot.Location = new System.Drawing.Point(16, 41);
            this.chkUploadFullScreenshot.Name = "chkUploadFullScreenshot";
            this.chkUploadFullScreenshot.Size = new System.Drawing.Size(190, 17);
            this.chkUploadFullScreenshot.TabIndex = 3;
            this.chkUploadFullScreenshot.Text = "Use &full Image instead of thumbnail";
            this.chkUploadFullScreenshot.UseVisualStyleBackColor = true;
            this.chkUploadFullScreenshot.CheckedChanged += new System.EventHandler(this.chkUploadFullScreenshot_CheckedChanged);
            // 
            // cboTemplate
            // 
            this.cboTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTemplate.FormattingEnabled = true;
            this.cboTemplate.Location = new System.Drawing.Point(360, 13);
            this.cboTemplate.Name = "cboTemplate";
            this.cboTemplate.Size = new System.Drawing.Size(195, 21);
            this.cboTemplate.TabIndex = 2;
            this.cboTemplate.SelectedIndexChanged += new System.EventHandler(this.cboTemplate_SelectedIndexChanged);
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.rtbDebugLog);
            this.tpDebug.Location = new System.Drawing.Point(4, 22);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tpDebug.Size = new System.Drawing.Size(878, 523);
            this.tpDebug.TabIndex = 5;
            this.tpDebug.Text = "Debug";
            this.tpDebug.UseVisualStyleBackColor = true;
            // 
            // rtbDebugLog
            // 
            this.rtbDebugLog.BackColor = System.Drawing.Color.White;
            this.rtbDebugLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDebugLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebugLog.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtbDebugLog.Location = new System.Drawing.Point(3, 3);
            this.rtbDebugLog.Name = "rtbDebugLog";
            this.rtbDebugLog.ReadOnly = true;
            this.rtbDebugLog.Size = new System.Drawing.Size(872, 517);
            this.rtbDebugLog.TabIndex = 0;
            this.rtbDebugLog.Text = "";
            this.rtbDebugLog.WordWrap = false;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.tcSettings);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(878, 523);
            this.tpSettings.TabIndex = 3;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // tcSettings
            // 
            this.tcSettings.Controls.Add(this.tpSettingsGeneral);
            this.tcSettings.Controls.Add(this.tpSettingsProfiles);
            this.tcSettings.Controls.Add(this.tpSettingsProxy);
            this.tcSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSettings.Location = new System.Drawing.Point(3, 3);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(872, 517);
            this.tcSettings.TabIndex = 0;
            // 
            // tpSettingsGeneral
            // 
            this.tpSettingsGeneral.Controls.Add(this.pgApp);
            this.tpSettingsGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpSettingsGeneral.Name = "tpSettingsGeneral";
            this.tpSettingsGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettingsGeneral.Size = new System.Drawing.Size(864, 491);
            this.tpSettingsGeneral.TabIndex = 0;
            this.tpSettingsGeneral.UseVisualStyleBackColor = true;
            // 
            // pgApp
            // 
            this.pgApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgApp.Location = new System.Drawing.Point(3, 3);
            this.pgApp.Name = "pgApp";
            this.pgApp.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgApp.Size = new System.Drawing.Size(858, 485);
            this.pgApp.TabIndex = 0;
            this.pgApp.ToolbarVisible = false;
            this.pgApp.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgApp_PropertyValueChanged);
            // 
            // tpSettingsProfiles
            // 
            this.tpSettingsProfiles.Controls.Add(this.btnRemoveScreenshotProfile);
            this.tpSettingsProfiles.Controls.Add(this.btnAddScreenshotProfile);
            this.tpSettingsProfiles.Controls.Add(this.listBoxProfiles);
            this.tpSettingsProfiles.Controls.Add(this.gbUploadScreenshots);
            this.tpSettingsProfiles.Controls.Add(this.pgProfileOptions);
            this.tpSettingsProfiles.Location = new System.Drawing.Point(4, 22);
            this.tpSettingsProfiles.Name = "tpSettingsProfiles";
            this.tpSettingsProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettingsProfiles.Size = new System.Drawing.Size(864, 491);
            this.tpSettingsProfiles.TabIndex = 5;
            this.tpSettingsProfiles.Text = "Profiles";
            this.tpSettingsProfiles.UseVisualStyleBackColor = true;
            // 
            // btnRemoveScreenshotProfile
            // 
            this.btnRemoveScreenshotProfile.Location = new System.Drawing.Point(67, 6);
            this.btnRemoveScreenshotProfile.Name = "btnRemoveScreenshotProfile";
            this.btnRemoveScreenshotProfile.Size = new System.Drawing.Size(64, 23);
            this.btnRemoveScreenshotProfile.TabIndex = 1;
            this.btnRemoveScreenshotProfile.Text = "Remove";
            this.btnRemoveScreenshotProfile.UseVisualStyleBackColor = true;
            this.btnRemoveScreenshotProfile.Click += new System.EventHandler(this.btnRemoveScreenshotProfile_Click);
            // 
            // btnAddScreenshotProfile
            // 
            this.btnAddScreenshotProfile.Location = new System.Drawing.Point(6, 6);
            this.btnAddScreenshotProfile.Name = "btnAddScreenshotProfile";
            this.btnAddScreenshotProfile.Size = new System.Drawing.Size(55, 23);
            this.btnAddScreenshotProfile.TabIndex = 0;
            this.btnAddScreenshotProfile.Text = "Add";
            this.btnAddScreenshotProfile.UseVisualStyleBackColor = true;
            this.btnAddScreenshotProfile.Click += new System.EventHandler(this.btnAddScreenshotProfile_Click);
            // 
            // listBoxProfiles
            // 
            this.listBoxProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxProfiles.FormattingEnabled = true;
            this.listBoxProfiles.Location = new System.Drawing.Point(6, 32);
            this.listBoxProfiles.Name = "listBoxProfiles";
            this.listBoxProfiles.Size = new System.Drawing.Size(125, 368);
            this.listBoxProfiles.TabIndex = 2;
            this.listBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.listBoxProfiles_SelectedIndexChanged);
            // 
            // gbUploadScreenshots
            // 
            this.gbUploadScreenshots.Controls.Add(this.cboFileUploader);
            this.gbUploadScreenshots.Controls.Add(this.btnDownloadFFmpeg);
            this.gbUploadScreenshots.Controls.Add(this.btnUploadersConfig);
            this.gbUploadScreenshots.Controls.Add(this.cboImageUploader);
            this.gbUploadScreenshots.Controls.Add(this.chkUploadScreenshots);
            this.gbUploadScreenshots.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbUploadScreenshots.Location = new System.Drawing.Point(3, 408);
            this.gbUploadScreenshots.Name = "gbUploadScreenshots";
            this.gbUploadScreenshots.Size = new System.Drawing.Size(858, 80);
            this.gbUploadScreenshots.TabIndex = 4;
            this.gbUploadScreenshots.TabStop = false;
            this.gbUploadScreenshots.Text = "Upload screenshots";
            // 
            // cboFileUploader
            // 
            this.cboFileUploader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileUploader.FormattingEnabled = true;
            this.cboFileUploader.Location = new System.Drawing.Point(152, 48);
            this.cboFileUploader.Name = "cboFileUploader";
            this.cboFileUploader.Size = new System.Drawing.Size(222, 21);
            this.cboFileUploader.TabIndex = 4;
            this.cboFileUploader.SelectedIndexChanged += new System.EventHandler(this.cboImageFileUploader_SelectedIndexChanged);
            // 
            // btnDownloadFFmpeg
            // 
            this.btnDownloadFFmpeg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDownloadFFmpeg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDownloadFFmpeg.Location = new System.Drawing.Point(550, 20);
            this.btnDownloadFFmpeg.Name = "btnDownloadFFmpeg";
            this.btnDownloadFFmpeg.Size = new System.Drawing.Size(138, 24);
            this.btnDownloadFFmpeg.TabIndex = 3;
            this.btnDownloadFFmpeg.Text = "Download FFmpeg";
            this.btnDownloadFFmpeg.UseVisualStyleBackColor = true;
            this.btnDownloadFFmpeg.Click += new System.EventHandler(this.btnDownloadFFmpeg_Click);
            // 
            // btnUploadersConfig
            // 
            this.btnUploadersConfig.Location = new System.Drawing.Point(384, 20);
            this.btnUploadersConfig.Name = "btnUploadersConfig";
            this.btnUploadersConfig.Size = new System.Drawing.Size(160, 24);
            this.btnUploadersConfig.TabIndex = 2;
            this.btnUploadersConfig.Text = "Uploaders configuration...";
            this.btnUploadersConfig.UseVisualStyleBackColor = true;
            this.btnUploadersConfig.Click += new System.EventHandler(this.btnUploadersConfig_Click);
            // 
            // cboImageUploader
            // 
            this.cboImageUploader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageUploader.FormattingEnabled = true;
            this.cboImageUploader.Location = new System.Drawing.Point(152, 22);
            this.cboImageUploader.Name = "cboImageUploader";
            this.cboImageUploader.Size = new System.Drawing.Size(222, 21);
            this.cboImageUploader.TabIndex = 1;
            this.cboImageUploader.SelectedIndexChanged += new System.EventHandler(this.cboScreenshotDest_SelectedIndexChanged);
            // 
            // chkUploadScreenshots
            // 
            this.chkUploadScreenshots.AutoSize = true;
            this.chkUploadScreenshots.Checked = true;
            this.chkUploadScreenshots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUploadScreenshots.Location = new System.Drawing.Point(16, 24);
            this.chkUploadScreenshots.Name = "chkUploadScreenshots";
            this.chkUploadScreenshots.Size = new System.Drawing.Size(130, 17);
            this.chkUploadScreenshots.TabIndex = 0;
            this.chkUploadScreenshots.Text = "Upload screenshot to:";
            this.chkUploadScreenshots.UseVisualStyleBackColor = true;
            this.chkUploadScreenshots.CheckedChanged += new System.EventHandler(this.chkScreenshotUpload_CheckedChanged);
            // 
            // pgProfileOptions
            // 
            this.pgProfileOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgProfileOptions.Location = new System.Drawing.Point(137, 6);
            this.pgProfileOptions.Name = "pgProfileOptions";
            this.pgProfileOptions.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgProfileOptions.Size = new System.Drawing.Size(724, 396);
            this.pgProfileOptions.TabIndex = 3;
            this.pgProfileOptions.ToolbarVisible = false;
            // 
            // tpSettingsProxy
            // 
            this.tpSettingsProxy.Controls.Add(this.lblProxyHost);
            this.tpSettingsProxy.Controls.Add(this.cbProxyMethod);
            this.tpSettingsProxy.Controls.Add(this.lblProxyMethod);
            this.tpSettingsProxy.Controls.Add(this.txtProxyHost);
            this.tpSettingsProxy.Controls.Add(this.nudProxyPort);
            this.tpSettingsProxy.Controls.Add(this.lblProxyPort);
            this.tpSettingsProxy.Controls.Add(this.lblProxyPassword);
            this.tpSettingsProxy.Controls.Add(this.txtProxyPassword);
            this.tpSettingsProxy.Controls.Add(this.lblProxyUsername);
            this.tpSettingsProxy.Controls.Add(this.txtProxyUsername);
            this.tpSettingsProxy.Location = new System.Drawing.Point(4, 22);
            this.tpSettingsProxy.Name = "tpSettingsProxy";
            this.tpSettingsProxy.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettingsProxy.Size = new System.Drawing.Size(864, 491);
            this.tpSettingsProxy.TabIndex = 1;
            this.tpSettingsProxy.Text = "Proxy";
            this.tpSettingsProxy.UseVisualStyleBackColor = true;
            // 
            // lblProxyHost
            // 
            this.lblProxyHost.AutoSize = true;
            this.lblProxyHost.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProxyHost.Location = new System.Drawing.Point(21, 64);
            this.lblProxyHost.Name = "lblProxyHost";
            this.lblProxyHost.Size = new System.Drawing.Size(32, 13);
            this.lblProxyHost.TabIndex = 2;
            this.lblProxyHost.Text = "Host:";
            // 
            // cbProxyMethod
            // 
            this.cbProxyMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProxyMethod.FormattingEnabled = true;
            this.cbProxyMethod.Location = new System.Drawing.Point(24, 33);
            this.cbProxyMethod.Name = "cbProxyMethod";
            this.cbProxyMethod.Size = new System.Drawing.Size(136, 21);
            this.cbProxyMethod.TabIndex = 1;
            this.cbProxyMethod.SelectedIndexChanged += new System.EventHandler(this.cbProxyMethod_SelectedIndexChanged);
            // 
            // lblProxyMethod
            // 
            this.lblProxyMethod.AutoSize = true;
            this.lblProxyMethod.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProxyMethod.Location = new System.Drawing.Point(21, 17);
            this.lblProxyMethod.Name = "lblProxyMethod";
            this.lblProxyMethod.Size = new System.Drawing.Size(100, 13);
            this.lblProxyMethod.TabIndex = 0;
            this.lblProxyMethod.Text = "Proxy configuration:";
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Location = new System.Drawing.Point(24, 81);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(232, 20);
            this.txtProxyHost.TabIndex = 4;
            this.txtProxyHost.TextChanged += new System.EventHandler(this.txtProxyHost_TextChanged);
            // 
            // nudProxyPort
            // 
            this.nudProxyPort.Location = new System.Drawing.Point(264, 81);
            this.nudProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudProxyPort.Name = "nudProxyPort";
            this.nudProxyPort.Size = new System.Drawing.Size(64, 20);
            this.nudProxyPort.TabIndex = 5;
            this.nudProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudProxyPort.ValueChanged += new System.EventHandler(this.nudProxyPort_ValueChanged);
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProxyPort.Location = new System.Drawing.Point(261, 65);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(29, 13);
            this.lblProxyPort.TabIndex = 3;
            this.lblProxyPort.Text = "Port:";
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProxyPassword.Location = new System.Drawing.Point(21, 161);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.lblProxyPassword.TabIndex = 8;
            this.lblProxyPassword.Text = "Password:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(24, 177);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(232, 20);
            this.txtProxyPassword.TabIndex = 9;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            this.txtProxyPassword.TextChanged += new System.EventHandler(this.txtProxyPassword_TextChanged);
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.AutoSize = true;
            this.lblProxyUsername.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProxyUsername.Location = new System.Drawing.Point(21, 113);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(58, 13);
            this.lblProxyUsername.TabIndex = 6;
            this.lblProxyUsername.Text = "Username:";
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.Location = new System.Drawing.Point(24, 129);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(232, 20);
            this.txtProxyUsername.TabIndex = 7;
            this.txtProxyUsername.TextChanged += new System.EventHandler(this.txtProxyUsername_TextChanged);
            // 
            // tscMain
            // 
            // 
            // tscMain.BottomToolStripPanel
            // 
            this.tscMain.BottomToolStripPanel.Controls.Add(this.ssMain);
            // 
            // tscMain.ContentPanel
            // 
            this.tscMain.ContentPanel.BackColor = System.Drawing.Color.White;
            this.tscMain.ContentPanel.Controls.Add(this.flpButtons);
            this.tscMain.ContentPanel.Controls.Add(this.tcMain);
            this.tscMain.ContentPanel.Controls.Add(this.tttvMain);
            this.tscMain.ContentPanel.Padding = new System.Windows.Forms.Padding(3);
            this.tscMain.ContentPanel.Size = new System.Drawing.Size(892, 591);
            this.tscMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscMain.Location = new System.Drawing.Point(0, 0);
            this.tscMain.Name = "tscMain";
            this.tscMain.Size = new System.Drawing.Size(892, 637);
            this.tscMain.TabIndex = 0;
            this.tscMain.Text = "toolStripContainer1";
            // 
            // tscMain.TopToolStripPanel
            // 
            this.tscMain.TopToolStripPanel.Controls.Add(this.msMain);
            // 
            // flpButtons
            // 
            this.flpButtons.Controls.Add(this.btnAnalyze);
            this.flpButtons.Controls.Add(this.btnCreateTorrent);
            this.flpButtons.Controls.Add(this.btnPublish);
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpButtons.Location = new System.Drawing.Point(3, 558);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(886, 30);
            this.flpButtons.TabIndex = 2;
            // 
            // tttvMain
            // 
            this.tttvMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tttvMain.ImageList = null;
            this.tttvMain.Location = new System.Drawing.Point(3, 3);
            this.tttvMain.MainTabControl = null;
            this.tttvMain.Margin = new System.Windows.Forms.Padding(0);
            this.tttvMain.Name = "tttvMain";
            this.tttvMain.Size = new System.Drawing.Size(885, 553);
            this.tttvMain.TabIndex = 1;
            this.tttvMain.TreeViewFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tttvMain.TreeViewSize = 150;
            // 
            // flpMediaInfoOptions
            // 
            this.flpMediaInfoOptions.Controls.Add(this.chkMediaInfoComplete);
            this.flpMediaInfoOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMediaInfoOptions.Location = new System.Drawing.Point(177, 488);
            this.flpMediaInfoOptions.Name = "flpMediaInfoOptions";
            this.flpMediaInfoOptions.Size = new System.Drawing.Size(692, 26);
            this.flpMediaInfoOptions.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AcceptButton = this.btnBrowse;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 637);
            this.Controls.Add(this.tscMain);
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(908, 639);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TDMaker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragEnter);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpMedia.ResumeLayout(false);
            this.gbSource.ResumeLayout(false);
            this.gbSource.PerformLayout();
            this.gbDVD.ResumeLayout(false);
            this.gbDVD.PerformLayout();
            this.gbLocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.tpMediaInfo.ResumeLayout(false);
            this.tlpMediaInfo.ResumeLayout(false);
            this.tlpMediaInfo.PerformLayout();
            this.tpScreenshots.ResumeLayout(false);
            this.tlpScreenshots.ResumeLayout(false);
            this.tlpScreenshotProps.ResumeLayout(false);
            this.tpPublish.ResumeLayout(false);
            this.tcPublish.ResumeLayout(false);
            this.tpPublishGeneral.ResumeLayout(false);
            this.tlpPublish.ResumeLayout(false);
            this.tlpPublish.PerformLayout();
            this.gbQuickPublish.ResumeLayout(false);
            this.flpPublishConfig.ResumeLayout(false);
            this.flpPublishConfig.PerformLayout();
            this.tpPublishTemplates.ResumeLayout(false);
            this.tpPublishTemplates.PerformLayout();
            this.gbTemplatesInternal.ResumeLayout(false);
            this.gbTemplatesInternal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSizeIncr)).EndInit();
            this.gbFonts.ResumeLayout(false);
            this.gbFonts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading1Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading2Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading3Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBodySize)).EndInit();
            this.tpDebug.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tcSettings.ResumeLayout(false);
            this.tpSettingsGeneral.ResumeLayout(false);
            this.tpSettingsProfiles.ResumeLayout(false);
            this.gbUploadScreenshots.ResumeLayout(false);
            this.gbUploadScreenshots.PerformLayout();
            this.tpSettingsProxy.ResumeLayout(false);
            this.tpSettingsProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).EndInit();
            this.tscMain.BottomToolStripPanel.ResumeLayout(false);
            this.tscMain.BottomToolStripPanel.PerformLayout();
            this.tscMain.ContentPanel.ResumeLayout(false);
            this.tscMain.TopToolStripPanel.ResumeLayout(false);
            this.tscMain.TopToolStripPanel.PerformLayout();
            this.tscMain.ResumeLayout(false);
            this.tscMain.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            this.flpMediaInfoOptions.ResumeLayout(false);
            this.flpMediaInfoOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bwApp;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel sbarIcon;
        private System.Windows.Forms.ToolStripStatusLabel sBar;
        private System.Windows.Forms.ToolStripProgressBar pBar;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.ToolStripStatusLabel tssPerc;
        private System.Windows.Forms.ToolTip ttApp;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miFileOpenFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveTorrent;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveInfoAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miFileExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditTools;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiFileOpenFile;
        private System.Windows.Forms.ToolStripMenuItem miEditCopy;
        private System.Windows.Forms.ToolStripMenuItem miHelpVersionHistory;
        private System.Windows.Forms.ToolStripMenuItem foldersToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miFoldersScreenshots;
        private System.Windows.Forms.ToolStripMenuItem miFoldersTorrents;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem miFoldersLogs;
        private System.Windows.Forms.ToolStripMenuItem miFoldersLogsDebug;
        private System.Windows.Forms.ToolStripMenuItem miFoldersSettings;
        private System.Windows.Forms.ToolStripMenuItem miFoldersTemplates;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpMedia;
        private System.Windows.Forms.GroupBox gbSource;
        private System.Windows.Forms.CheckBox chkTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.CheckBox chkWebLink;
        private System.Windows.Forms.CheckBox chkSource;
        private System.Windows.Forms.TextBox txtWebLink;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.GroupBox gbDVD;
        private System.Windows.Forms.ComboBox cboDiscMenu;
        private System.Windows.Forms.CheckBox chkDiscMenu;
        private System.Windows.Forms.ComboBox cboExtras;
        private System.Windows.Forms.CheckBox chkExtras;
        private System.Windows.Forms.ComboBox cboAuthoring;
        private System.Windows.Forms.CheckBox chkAuthoring;
        private System.Windows.Forms.GroupBox gbLocation;
        private System.Windows.Forms.Button btnBrowseDir;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.TabPage tpMediaInfo;
        private System.Windows.Forms.TableLayoutPanel tlpMediaInfo;
        private System.Windows.Forms.ListBox lbMediaInfo;
        private System.Windows.Forms.TextBox txtMediaInfo;
        private System.Windows.Forms.TabPage tpScreenshots;
        private System.Windows.Forms.TableLayoutPanel tlpScreenshots;
        private System.Windows.Forms.ListBox lbScreenshots;
        private System.Windows.Forms.TableLayoutPanel tlpScreenshotProps;
        private MyPictureBox pbScreenshot;
        private System.Windows.Forms.PropertyGrid pgScreenshot;
        private System.Windows.Forms.TabPage tpPublish;
        private System.Windows.Forms.TableLayoutPanel tlpPublish;
        private System.Windows.Forms.GroupBox gbQuickPublish;
        private System.Windows.Forms.FlowLayoutPanel flpPublishConfig;
        private System.Windows.Forms.CheckBox chkQuickPre;
        private System.Windows.Forms.CheckBox chkQuickAlignCenter;
        private System.Windows.Forms.CheckBox chkQuickFullPicture;
        private System.Windows.Forms.ComboBox cboQuickPublishType;
        private System.Windows.Forms.ComboBox cboQuickTemplate;
        private System.Windows.Forms.TextBox txtPublish;
        private System.Windows.Forms.ListBox lbPublish;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.PropertyGrid pgApp;
        private System.Windows.Forms.TabPage tpSettingsProfiles;
        private System.Windows.Forms.CheckBox chkUploadScreenshots;
        private System.Windows.Forms.ComboBox cboImageUploader;
        private System.Windows.Forms.ComboBox cboPublishType;
        private System.Windows.Forms.Button btnTemplatesRewrite;
        private System.Windows.Forms.ComboBox cboTemplate;
        private System.Windows.Forms.GroupBox gbTemplatesInternal;
        private System.Windows.Forms.NumericUpDown nudFontSizeIncr;
        private System.Windows.Forms.CheckBox chkPre;
        private System.Windows.Forms.CheckBox chkPreIncreaseFontSize;
        private System.Windows.Forms.CheckBox chkAlignCenter;
        private System.Windows.Forms.GroupBox gbFonts;
        private System.Windows.Forms.NumericUpDown nudHeading1Size;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudHeading2Size;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudHeading3Size;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudBodySize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkUploadFullScreenshot;
        private System.Windows.Forms.CheckBox chkTemplatesMode;
        private System.Windows.Forms.TabPage tpDebug;
        private System.Windows.Forms.RichTextBox rtbDebugLog;
        private System.Windows.Forms.Button btnCreateTorrent;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Button btnUploadersConfig;
        private System.Windows.Forms.GroupBox gbUploadScreenshots;
        private System.Windows.Forms.PropertyGrid pgProfileOptions;
        private System.Windows.Forms.CheckBox chkMediaInfoComplete;
        private System.Windows.Forms.ComboBox cboFileUploader;
        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpSettingsGeneral;
        private System.Windows.Forms.TabPage tpPublishTemplates;
        private System.Windows.Forms.TabPage tpSettingsProxy;
        private System.Windows.Forms.ToolStripContainer tscMain;
        private System.Windows.Forms.Button btnDownloadFFmpeg;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.TabControl tcPublish;
        private System.Windows.Forms.TabPage tpPublishGeneral;
        private TabToTreeView tttvMain;
        private System.Windows.Forms.ComboBox cbProxyMethod;
        private System.Windows.Forms.Label lblProxyMethod;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.NumericUpDown nudProxyPort;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label lblProxyUsername;
        private System.Windows.Forms.TextBox txtProxyUsername;
        private System.Windows.Forms.Label lblProxyHost;
        private System.Windows.Forms.Button btnRemoveScreenshotProfile;
        private System.Windows.Forms.Button btnAddScreenshotProfile;
        private System.Windows.Forms.ListBox listBoxProfiles;
        private System.Windows.Forms.FlowLayoutPanel flpMediaInfoOptions;
    }
}