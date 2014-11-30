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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sbarIcon = new System.Windows.Forms.ToolStripStatusLabel();
            this.sBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssPerc = new System.Windows.Forms.ToolStripStatusLabel();
            this.pBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.cmsApp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.foldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmScreenshots = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTorrentsDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmLogsDir = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSettingsDir = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTemplates = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmsVersionHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAppAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.ttApp = new System.Windows.Forms.ToolTip(this.components);
            this.btnRefreshTrackers = new System.Windows.Forms.Button();
            this.txtWebLink = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
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
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPreferKnownFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpVersionHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPublish = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnCreateTorrent = new System.Windows.Forms.Button();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMedia = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.lbStatus = new System.Windows.Forms.ListBox();
            this.gbSourceProp = new System.Windows.Forms.GroupBox();
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
            this.gbMediaInfoQuickOptions = new System.Windows.Forms.GroupBox();
            this.chkMediaInfoComplete = new System.Windows.Forms.CheckBox();
            this.tpScreenshots = new System.Windows.Forms.TabPage();
            this.tlpScreenshots = new System.Windows.Forms.TableLayoutPanel();
            this.lbScreenshots = new System.Windows.Forms.ListBox();
            this.tlpScreenshotProps = new System.Windows.Forms.TableLayoutPanel();
            this.pbScreenshot = new System.Windows.Forms.PictureBox();
            this.pgScreenshot = new System.Windows.Forms.PropertyGrid();
            this.tpPublish = new System.Windows.Forms.TabPage();
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
            this.tpScreenshotOptions = new System.Windows.Forms.TabPage();
            this.tcThumbnailers = new System.Windows.Forms.TabControl();
            this.tpThumbnailersGeneral = new System.Windows.Forms.TabPage();
            this.gbUploadScreenshots = new System.Windows.Forms.GroupBox();
            this.flpScreenshots = new System.Windows.Forms.FlowLayoutPanel();
            this.chkScreenshotUpload = new System.Windows.Forms.CheckBox();
            this.cboScreenshotDest = new System.Windows.Forms.ComboBox();
            this.btnUploadersConfig = new System.Windows.Forms.Button();
            this.gbThumbnailer = new System.Windows.Forms.GroupBox();
            this.cboThumbnailer = new System.Windows.Forms.ComboBox();
            this.gbScreenshotsLoc = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboScreenshotsLoc = new System.Windows.Forms.ComboBox();
            this.txtScreenshotsLoc = new System.Windows.Forms.TextBox();
            this.btnScreenshotsLocBrowse = new System.Windows.Forms.Button();
            this.tpMtn = new System.Windows.Forms.TabPage();
            this.tlpMTN = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMtnUsage = new System.Windows.Forms.TableLayoutPanel();
            this.pgMtn = new System.Windows.Forms.PropertyGrid();
            this.tlpMtnProfiles = new System.Windows.Forms.TableLayoutPanel();
            this.flpMtn = new System.Windows.Forms.FlowLayoutPanel();
            this.tbnAddMtnProfile = new System.Windows.Forms.Button();
            this.btnRemoveMtnProfile = new System.Windows.Forms.Button();
            this.lbMtnProfiles = new System.Windows.Forms.ListBox();
            this.txtMtnArgs = new System.Windows.Forms.TextBox();
            this.tpMPlayer = new System.Windows.Forms.TabPage();
            this.pgMPlayerOptions = new System.Windows.Forms.PropertyGrid();
            this.tpPublishOptions = new System.Windows.Forms.TabPage();
            this.cboPublishType = new System.Windows.Forms.ComboBox();
            this.btnTemplatesRewrite = new System.Windows.Forms.Button();
            this.cboTemplate = new System.Windows.Forms.ComboBox();
            this.gbTemplatesInternal = new System.Windows.Forms.GroupBox();
            this.nudFontSizeIncr = new System.Windows.Forms.NumericUpDown();
            this.chkPre = new System.Windows.Forms.CheckBox();
            this.chkPreIncreaseFontSize = new System.Windows.Forms.CheckBox();
            this.chkAlignCenter = new System.Windows.Forms.CheckBox();
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
            this.chkTemplatesMode = new System.Windows.Forms.CheckBox();
            this.tpTorrentCreator = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTorrentLoc = new System.Windows.Forms.ComboBox();
            this.chkWritePublish = new System.Windows.Forms.CheckBox();
            this.chkTorrentOrganize = new System.Windows.Forms.CheckBox();
            this.btnBrowseTorrentCustomFolder = new System.Windows.Forms.Button();
            this.txtTorrentCustomFolder = new System.Windows.Forms.TextBox();
            this.gbTrackerMgr = new System.Windows.Forms.GroupBox();
            this.tlpTrackers = new System.Windows.Forms.TableLayoutPanel();
            this.flpTrackers = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddTracker = new System.Windows.Forms.Button();
            this.btnRemoveTracker = new System.Windows.Forms.Button();
            this.pgTracker = new System.Windows.Forms.PropertyGrid();
            this.flpTrackerGroups = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddTrackerGroup = new System.Windows.Forms.Button();
            this.btnRemoveTrackerGroup = new System.Windows.Forms.Button();
            this.gbTrackerGroups = new System.Windows.Forms.GroupBox();
            this.lbTrackerGroups = new System.Windows.Forms.ListBox();
            this.gbTrackers = new System.Windows.Forms.GroupBox();
            this.lbTrackers = new System.Windows.Forms.ListBox();
            this.cboTrackerGroupActive = new System.Windows.Forms.ComboBox();
            this.chkCreateTorrent = new System.Windows.Forms.CheckBox();
            this.tpProxy = new System.Windows.Forms.TabPage();
            this.chkProxyEnable = new System.Windows.Forms.CheckBox();
            this.pgProxy = new System.Windows.Forms.PropertyGrid();
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.rtbDebugLog = new System.Windows.Forms.RichTextBox();
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.pgApp = new System.Windows.Forms.PropertyGrid();
            this.statusStrip1.SuspendLayout();
            this.cmsApp.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpMedia.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.gbSourceProp.SuspendLayout();
            this.gbDVD.SuspendLayout();
            this.gbLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.tpMediaInfo.SuspendLayout();
            this.tlpMediaInfo.SuspendLayout();
            this.gbMediaInfoQuickOptions.SuspendLayout();
            this.tpScreenshots.SuspendLayout();
            this.tlpScreenshots.SuspendLayout();
            this.tlpScreenshotProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbScreenshot)).BeginInit();
            this.tpPublish.SuspendLayout();
            this.tlpPublish.SuspendLayout();
            this.gbQuickPublish.SuspendLayout();
            this.flpPublishConfig.SuspendLayout();
            this.tpScreenshotOptions.SuspendLayout();
            this.tcThumbnailers.SuspendLayout();
            this.tpThumbnailersGeneral.SuspendLayout();
            this.gbUploadScreenshots.SuspendLayout();
            this.flpScreenshots.SuspendLayout();
            this.gbThumbnailer.SuspendLayout();
            this.gbScreenshotsLoc.SuspendLayout();
            this.tpMtn.SuspendLayout();
            this.tlpMTN.SuspendLayout();
            this.tlpMtnUsage.SuspendLayout();
            this.tlpMtnProfiles.SuspendLayout();
            this.flpMtn.SuspendLayout();
            this.tpMPlayer.SuspendLayout();
            this.tpPublishOptions.SuspendLayout();
            this.gbTemplatesInternal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSizeIncr)).BeginInit();
            this.gbFonts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading1Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading2Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading3Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBodySize)).BeginInit();
            this.tpTorrentCreator.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.gbTrackerMgr.SuspendLayout();
            this.tlpTrackers.SuspendLayout();
            this.flpTrackers.SuspendLayout();
            this.flpTrackerGroups.SuspendLayout();
            this.gbTrackerGroups.SuspendLayout();
            this.gbTrackers.SuspendLayout();
            this.tpProxy.SuspendLayout();
            this.tpDebug.SuspendLayout();
            this.tpAdvanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // bwApp
            // 
            this.bwApp.WorkerReportsProgress = true;
            this.bwApp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwApp_DoWork);
            this.bwApp.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwApp_ProgressChanged);
            this.bwApp.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwApp_RunWorkerCompleted);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbarIcon,
            this.sBar,
            this.tssPerc,
            this.pBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 579);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(892, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sbarIcon
            // 
            this.sbarIcon.Image = ((System.Drawing.Image)(resources.GetObject("sbarIcon.Image")));
            this.sbarIcon.Name = "sbarIcon";
            this.sbarIcon.Size = new System.Drawing.Size(16, 17);
            // 
            // sBar
            // 
            this.sBar.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.sBar.Name = "sBar";
            this.sBar.Size = new System.Drawing.Size(719, 17);
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
            // cmsApp
            // 
            this.cmsApp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.foldersToolStripMenuItem,
            this.toolStripSeparator1,
            this.tmsVersionHistory,
            this.cmsAppAbout});
            this.cmsApp.Name = "cmsApp";
            this.cmsApp.Size = new System.Drawing.Size(164, 76);
            // 
            // foldersToolStripMenuItem
            // 
            this.foldersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmScreenshots,
            this.tsmTorrentsDir,
            this.toolStripSeparator2,
            this.tsmLogsDir,
            this.tsmSettingsDir,
            this.tsmTemplates});
            this.foldersToolStripMenuItem.Name = "foldersToolStripMenuItem";
            this.foldersToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.foldersToolStripMenuItem.Text = "&Folders";
            // 
            // tsmScreenshots
            // 
            this.tsmScreenshots.Name = "tsmScreenshots";
            this.tsmScreenshots.Size = new System.Drawing.Size(146, 22);
            this.tsmScreenshots.Text = "&Screenshots...";
            this.tsmScreenshots.Click += new System.EventHandler(this.tsmScreenshots_Click);
            // 
            // tsmTorrentsDir
            // 
            this.tsmTorrentsDir.Name = "tsmTorrentsDir";
            this.tsmTorrentsDir.Size = new System.Drawing.Size(146, 22);
            this.tsmTorrentsDir.Text = "&Torrents...";
            this.tsmTorrentsDir.Click += new System.EventHandler(this.tsmTorrentsDir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // tsmLogsDir
            // 
            this.tsmLogsDir.Name = "tsmLogsDir";
            this.tsmLogsDir.Size = new System.Drawing.Size(146, 22);
            this.tsmLogsDir.Text = "&Logs...";
            this.tsmLogsDir.Click += new System.EventHandler(this.tsmLogsDir_Click);
            // 
            // tsmSettingsDir
            // 
            this.tsmSettingsDir.Name = "tsmSettingsDir";
            this.tsmSettingsDir.Size = new System.Drawing.Size(146, 22);
            this.tsmSettingsDir.Text = "&Settings...";
            this.tsmSettingsDir.Click += new System.EventHandler(this.tsmSettingsDir_Click);
            // 
            // tsmTemplates
            // 
            this.tsmTemplates.Name = "tsmTemplates";
            this.tsmTemplates.Size = new System.Drawing.Size(146, 22);
            this.tsmTemplates.Text = "Templates...";
            this.tsmTemplates.Click += new System.EventHandler(this.tsmTemplates_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // tmsVersionHistory
            // 
            this.tmsVersionHistory.Name = "tmsVersionHistory";
            this.tmsVersionHistory.Size = new System.Drawing.Size(163, 22);
            this.tmsVersionHistory.Text = "&Version History...";
            this.tmsVersionHistory.Click += new System.EventHandler(this.tmsVersionHistory_Click);
            // 
            // cmsAppAbout
            // 
            this.cmsAppAbout.Name = "cmsAppAbout";
            this.cmsAppAbout.Size = new System.Drawing.Size(163, 22);
            this.cmsAppAbout.Text = "&About...";
            this.cmsAppAbout.Click += new System.EventHandler(this.cmsAppAbout_Click);
            // 
            // btnRefreshTrackers
            // 
            this.btnRefreshTrackers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshTrackers.Location = new System.Drawing.Point(763, 10);
            this.btnRefreshTrackers.Name = "btnRefreshTrackers";
            this.btnRefreshTrackers.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshTrackers.TabIndex = 5;
            this.btnRefreshTrackers.Text = "&Refresh";
            this.ttApp.SetToolTip(this.btnRefreshTrackers, "Refresh Trackers List from Tracker Manager");
            this.btnRefreshTrackers.UseVisualStyleBackColor = true;
            this.btnRefreshTrackers.Click += new System.EventHandler(this.btnRefreshTrackers_Click);
            // 
            // txtWebLink
            // 
            this.txtWebLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebLink.Location = new System.Drawing.Point(112, 72);
            this.txtWebLink.Name = "txtWebLink";
            this.txtWebLink.Size = new System.Drawing.Size(464, 20);
            this.txtWebLink.TabIndex = 10;
            this.ttApp.SetToolTip(this.txtWebLink, "IMDB URL");
            this.txtWebLink.TextChanged += new System.EventHandler(this.txtWebLink_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tsmiEditTools,
            this.foldersToolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(892, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
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
            this.tsmiFileOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFileOpenFile.Image")));
            this.tsmiFileOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiFileOpenFile.Name = "tsmiFileOpenFile";
            this.tsmiFileOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiFileOpenFile.Size = new System.Drawing.Size(223, 22);
            this.tsmiFileOpenFile.Text = "&Open File...";
            this.tsmiFileOpenFile.Click += new System.EventHandler(this.miFileOpenFile_Click);
            // 
            // miFileOpenFolder
            // 
            this.miFileOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("miFileOpenFolder.Image")));
            this.miFileOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miFileOpenFolder.Name = "miFileOpenFolder";
            this.miFileOpenFolder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.miFileOpenFolder.Size = new System.Drawing.Size(223, 22);
            this.miFileOpenFolder.Text = "&Open Folder...";
            this.miFileOpenFolder.Click += new System.EventHandler(this.miFileOpenFolder_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(220, 6);
            // 
            // miFileSaveTorrent
            // 
            this.miFileSaveTorrent.Image = ((System.Drawing.Image)(resources.GetObject("miFileSaveTorrent.Image")));
            this.miFileSaveTorrent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miFileSaveTorrent.Name = "miFileSaveTorrent";
            this.miFileSaveTorrent.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.miFileSaveTorrent.Size = new System.Drawing.Size(223, 22);
            this.miFileSaveTorrent.Text = "&Save Torrent";
            this.miFileSaveTorrent.Click += new System.EventHandler(this.miFileSaveTorrent_Click);
            // 
            // miFileSaveInfoAs
            // 
            this.miFileSaveInfoAs.Name = "miFileSaveInfoAs";
            this.miFileSaveInfoAs.Size = new System.Drawing.Size(223, 22);
            this.miFileSaveInfoAs.Text = "&Save Publish Info As...";
            this.miFileSaveInfoAs.Click += new System.EventHandler(this.miFileSaveInfoAs_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(220, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(223, 22);
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
            this.miEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("miEditCopy.Image")));
            this.miEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miEditCopy.Name = "miEditCopy";
            this.miEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miEditCopy.Size = new System.Drawing.Size(144, 22);
            this.miEditCopy.Text = "&Copy";
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
            this.miFoldersTemplates,
            this.toolStripSeparator5,
            this.tsmiPreferKnownFolders});
            this.foldersToolStripMenuItem1.Name = "foldersToolStripMenuItem1";
            this.foldersToolStripMenuItem1.Size = new System.Drawing.Size(57, 20);
            this.foldersToolStripMenuItem1.Text = "&Folders";
            // 
            // miFoldersScreenshots
            // 
            this.miFoldersScreenshots.Name = "miFoldersScreenshots";
            this.miFoldersScreenshots.Size = new System.Drawing.Size(195, 22);
            this.miFoldersScreenshots.Text = "&Screenshots...";
            this.miFoldersScreenshots.Click += new System.EventHandler(this.miFoldersScreenshots_Click);
            // 
            // miFoldersTorrents
            // 
            this.miFoldersTorrents.Name = "miFoldersTorrents";
            this.miFoldersTorrents.Size = new System.Drawing.Size(195, 22);
            this.miFoldersTorrents.Text = "&Torrents...";
            this.miFoldersTorrents.Click += new System.EventHandler(this.miFoldersTorrents_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
            // 
            // miFoldersLogs
            // 
            this.miFoldersLogs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFoldersLogsDebug});
            this.miFoldersLogs.Name = "miFoldersLogs";
            this.miFoldersLogs.Size = new System.Drawing.Size(195, 22);
            this.miFoldersLogs.Text = "&Logs...";
            this.miFoldersLogs.Click += new System.EventHandler(this.miFoldersLogs_Click);
            // 
            // miFoldersLogsDebug
            // 
            this.miFoldersLogsDebug.Name = "miFoldersLogsDebug";
            this.miFoldersLogsDebug.Size = new System.Drawing.Size(118, 22);
            this.miFoldersLogsDebug.Text = "&Debug...";
            this.miFoldersLogsDebug.Click += new System.EventHandler(this.miFoldersLogsDebug_Click);
            // 
            // miFoldersSettings
            // 
            this.miFoldersSettings.Name = "miFoldersSettings";
            this.miFoldersSettings.Size = new System.Drawing.Size(195, 22);
            this.miFoldersSettings.Text = "&Settings...";
            this.miFoldersSettings.Click += new System.EventHandler(this.miFoldersSettings_Click);
            // 
            // miFoldersTemplates
            // 
            this.miFoldersTemplates.Name = "miFoldersTemplates";
            this.miFoldersTemplates.Size = new System.Drawing.Size(195, 22);
            this.miFoldersTemplates.Text = "&Templates...";
            this.miFoldersTemplates.Click += new System.EventHandler(this.miFoldersTemplates_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
            // 
            // tsmiPreferKnownFolders
            // 
            this.tsmiPreferKnownFolders.Name = "tsmiPreferKnownFolders";
            this.tsmiPreferKnownFolders.Size = new System.Drawing.Size(195, 22);
            this.tsmiPreferKnownFolders.Text = "Prefer &Known Folders...";
            this.tsmiPreferKnownFolders.Click += new System.EventHandler(this.tsmiPreferKnownFolders_Click);
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
            this.miHelpVersionHistory.Name = "miHelpVersionHistory";
            this.miHelpVersionHistory.Size = new System.Drawing.Size(163, 22);
            this.miHelpVersionHistory.Text = "&Version History...";
            this.miHelpVersionHistory.Click += new System.EventHandler(this.miHelpVersionHistory_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(163, 22);
            this.tsmiAbout.Text = "&About...";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // btnPublish
            // 
            this.btnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPublish.AutoSize = true;
            this.btnPublish.Enabled = false;
            this.btnPublish.Location = new System.Drawing.Point(768, 552);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(106, 23);
            this.btnPublish.TabIndex = 5;
            this.btnPublish.Text = "&Copy to Clipboard";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAnalyze.AutoSize = true;
            this.btnAnalyze.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAnalyze.Enabled = false;
            this.btnAnalyze.Location = new System.Drawing.Point(16, 551);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(104, 23);
            this.btnAnalyze.TabIndex = 9;
            this.btnAnalyze.Text = "&Create Description";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnCreateTorrent
            // 
            this.btnCreateTorrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateTorrent.AutoSize = true;
            this.btnCreateTorrent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCreateTorrent.Location = new System.Drawing.Point(128, 551);
            this.btnCreateTorrent.Name = "btnCreateTorrent";
            this.btnCreateTorrent.Size = new System.Drawing.Size(85, 23);
            this.btnCreateTorrent.TabIndex = 10;
            this.btnCreateTorrent.Text = "Create &Torrent";
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
            this.tcMain.Controls.Add(this.tpScreenshotOptions);
            this.tcMain.Controls.Add(this.tpPublishOptions);
            this.tcMain.Controls.Add(this.tpTorrentCreator);
            this.tcMain.Controls.Add(this.tpProxy);
            this.tcMain.Controls.Add(this.tpDebug);
            this.tcMain.Controls.Add(this.tpAdvanced);
            this.tcMain.Location = new System.Drawing.Point(8, 32);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(882, 512);
            this.tcMain.TabIndex = 4;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpMedia
            // 
            this.tpMedia.Controls.Add(this.groupBox10);
            this.tpMedia.Controls.Add(this.gbSourceProp);
            this.tpMedia.Controls.Add(this.gbDVD);
            this.tpMedia.Controls.Add(this.gbLocation);
            this.tpMedia.Location = new System.Drawing.Point(4, 22);
            this.tpMedia.Name = "tpMedia";
            this.tpMedia.Size = new System.Drawing.Size(874, 486);
            this.tpMedia.TabIndex = 4;
            this.tpMedia.Text = "Input";
            this.tpMedia.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.lbStatus);
            this.groupBox10.Location = new System.Drawing.Point(8, 288);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupBox10.Size = new System.Drawing.Size(856, 158);
            this.groupBox10.TabIndex = 13;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Progress";
            // 
            // lbStatus
            // 
            this.lbStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStatus.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.FormattingEnabled = true;
            this.lbStatus.IntegralHeight = false;
            this.lbStatus.ItemHeight = 15;
            this.lbStatus.Location = new System.Drawing.Point(5, 18);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.ScrollAlwaysVisible = true;
            this.lbStatus.Size = new System.Drawing.Size(846, 135);
            this.lbStatus.TabIndex = 11;
            // 
            // gbSourceProp
            // 
            this.gbSourceProp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSourceProp.Controls.Add(this.chkTitle);
            this.gbSourceProp.Controls.Add(this.txtTitle);
            this.gbSourceProp.Controls.Add(this.chkWebLink);
            this.gbSourceProp.Controls.Add(this.chkSource);
            this.gbSourceProp.Controls.Add(this.txtWebLink);
            this.gbSourceProp.Controls.Add(this.cboSource);
            this.gbSourceProp.Location = new System.Drawing.Point(272, 176);
            this.gbSourceProp.Name = "gbSourceProp";
            this.gbSourceProp.Size = new System.Drawing.Size(592, 104);
            this.gbSourceProp.TabIndex = 12;
            this.gbSourceProp.TabStop = false;
            this.gbSourceProp.Text = "Source Properties";
            // 
            // chkTitle
            // 
            this.chkTitle.AutoSize = true;
            this.chkTitle.Checked = true;
            this.chkTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTitle.Location = new System.Drawing.Point(16, 50);
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.Size = new System.Drawing.Size(49, 17);
            this.chkTitle.TabIndex = 14;
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
            this.txtTitle.Size = new System.Drawing.Size(464, 20);
            this.txtTitle.TabIndex = 15;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // chkWebLink
            // 
            this.chkWebLink.AutoSize = true;
            this.chkWebLink.Location = new System.Drawing.Point(16, 74);
            this.chkWebLink.Name = "chkWebLink";
            this.chkWebLink.Size = new System.Drawing.Size(75, 17);
            this.chkWebLink.TabIndex = 9;
            this.chkWebLink.Text = "&Web Link:";
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
            this.chkSource.TabIndex = 13;
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
            this.cboSource.TabIndex = 0;
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
            this.gbDVD.Location = new System.Drawing.Point(6, 176);
            this.gbDVD.Name = "gbDVD";
            this.gbDVD.Size = new System.Drawing.Size(256, 104);
            this.gbDVD.TabIndex = 11;
            this.gbDVD.TabStop = false;
            this.gbDVD.Text = "DVD Properties";
            // 
            // cboDiscMenu
            // 
            this.cboDiscMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDiscMenu.FormattingEnabled = true;
            this.cboDiscMenu.Location = new System.Drawing.Point(120, 48);
            this.cboDiscMenu.Name = "cboDiscMenu";
            this.cboDiscMenu.Size = new System.Drawing.Size(121, 21);
            this.cboDiscMenu.TabIndex = 18;
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
            this.chkDiscMenu.TabIndex = 17;
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
            this.cboExtras.TabIndex = 16;
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
            this.chkExtras.TabIndex = 15;
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
            this.cboAuthoring.TabIndex = 14;
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
            this.chkAuthoring.TabIndex = 13;
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
            this.gbLocation.Size = new System.Drawing.Size(856, 160);
            this.gbLocation.TabIndex = 7;
            this.gbLocation.TabStop = false;
            this.gbLocation.Text = "Locations - Browse or Drag and Drop a Movie file or folder...";
            // 
            // btnBrowseDir
            // 
            this.btnBrowseDir.AutoSize = true;
            this.btnBrowseDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowseDir.Location = new System.Drawing.Point(144, 26);
            this.btnBrowseDir.Name = "btnBrowseDir";
            this.btnBrowseDir.Size = new System.Drawing.Size(128, 23);
            this.btnBrowseDir.TabIndex = 14;
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
            this.lbFiles.Location = new System.Drawing.Point(6, 58);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFiles.Size = new System.Drawing.Size(840, 96);
            this.lbFiles.Sorted = true;
            this.lbFiles.TabIndex = 13;
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);
            // 
            // btnBrowse
            // 
            this.btnBrowse.AutoSize = true;
            this.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrowse.Location = new System.Drawing.Point(6, 26);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(134, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "&Browse for a file or files...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbLogo.Location = new System.Drawing.Point(642, 20);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(206, 29);
            this.pbLogo.TabIndex = 11;
            this.pbLogo.TabStop = false;
            // 
            // tpMediaInfo
            // 
            this.tpMediaInfo.Controls.Add(this.tlpMediaInfo);
            this.tpMediaInfo.Location = new System.Drawing.Point(4, 22);
            this.tpMediaInfo.Name = "tpMediaInfo";
            this.tpMediaInfo.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpMediaInfo.Size = new System.Drawing.Size(874, 486);
            this.tpMediaInfo.TabIndex = 0;
            this.tpMediaInfo.Text = "Media Info";
            this.tpMediaInfo.UseVisualStyleBackColor = true;
            // 
            // tlpMediaInfo
            // 
            this.tlpMediaInfo.ColumnCount = 3;
            this.tlpMediaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMediaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpMediaInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tlpMediaInfo.Controls.Add(this.lbMediaInfo, 0, 0);
            this.tlpMediaInfo.Controls.Add(this.txtMediaInfo, 1, 0);
            this.tlpMediaInfo.Controls.Add(this.gbMediaInfoQuickOptions, 2, 0);
            this.tlpMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMediaInfo.Location = new System.Drawing.Point(3, 3);
            this.tlpMediaInfo.Name = "tlpMediaInfo";
            this.tlpMediaInfo.RowCount = 1;
            this.tlpMediaInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMediaInfo.Size = new System.Drawing.Size(868, 480);
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
            this.lbMediaInfo.Size = new System.Drawing.Size(147, 474);
            this.lbMediaInfo.TabIndex = 0;
            this.lbMediaInfo.SelectedIndexChanged += new System.EventHandler(this.LbMediaInfoSelectedIndexChanged);
            // 
            // txtMediaInfo
            // 
            this.txtMediaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMediaInfo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMediaInfo.Location = new System.Drawing.Point(156, 3);
            this.txtMediaInfo.Multiline = true;
            this.txtMediaInfo.Name = "txtMediaInfo";
            this.txtMediaInfo.ReadOnly = true;
            this.txtMediaInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMediaInfo.Size = new System.Drawing.Size(607, 474);
            this.txtMediaInfo.TabIndex = 1;
            // 
            // gbMediaInfoQuickOptions
            // 
            this.gbMediaInfoQuickOptions.Controls.Add(this.chkMediaInfoComplete);
            this.gbMediaInfoQuickOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMediaInfoQuickOptions.Location = new System.Drawing.Point(768, 2);
            this.gbMediaInfoQuickOptions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbMediaInfoQuickOptions.Name = "gbMediaInfoQuickOptions";
            this.gbMediaInfoQuickOptions.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbMediaInfoQuickOptions.Size = new System.Drawing.Size(98, 476);
            this.gbMediaInfoQuickOptions.TabIndex = 2;
            this.gbMediaInfoQuickOptions.TabStop = false;
            this.gbMediaInfoQuickOptions.Text = "Quick Options";
            // 
            // chkMediaInfoComplete
            // 
            this.chkMediaInfoComplete.AutoSize = true;
            this.chkMediaInfoComplete.Location = new System.Drawing.Point(4, 17);
            this.chkMediaInfoComplete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkMediaInfoComplete.Name = "chkMediaInfoComplete";
            this.chkMediaInfoComplete.Size = new System.Drawing.Size(70, 17);
            this.chkMediaInfoComplete.TabIndex = 0;
            this.chkMediaInfoComplete.Text = "Complete";
            this.chkMediaInfoComplete.UseVisualStyleBackColor = true;
            this.chkMediaInfoComplete.CheckedChanged += new System.EventHandler(this.chkMediaInfoComplete_CheckedChanged);
            // 
            // tpScreenshots
            // 
            this.tpScreenshots.Controls.Add(this.tlpScreenshots);
            this.tpScreenshots.Location = new System.Drawing.Point(4, 22);
            this.tpScreenshots.Name = "tpScreenshots";
            this.tpScreenshots.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpScreenshots.Size = new System.Drawing.Size(874, 486);
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
            this.tlpScreenshots.Size = new System.Drawing.Size(868, 480);
            this.tlpScreenshots.TabIndex = 1;
            // 
            // lbScreenshots
            // 
            this.lbScreenshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbScreenshots.FormattingEnabled = true;
            this.lbScreenshots.HorizontalScrollbar = true;
            this.lbScreenshots.IntegralHeight = false;
            this.lbScreenshots.Location = new System.Drawing.Point(3, 3);
            this.lbScreenshots.Name = "lbScreenshots";
            this.lbScreenshots.Size = new System.Drawing.Size(254, 474);
            this.lbScreenshots.TabIndex = 2;
            this.lbScreenshots.SelectedIndexChanged += new System.EventHandler(this.lbScreenshots_SelectedIndexChanged);
            // 
            // tlpScreenshotProps
            // 
            this.tlpScreenshotProps.ColumnCount = 1;
            this.tlpScreenshotProps.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpScreenshotProps.Controls.Add(this.pbScreenshot, 0, 0);
            this.tlpScreenshotProps.Controls.Add(this.pgScreenshot, 0, 1);
            this.tlpScreenshotProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpScreenshotProps.Location = new System.Drawing.Point(260, 0);
            this.tlpScreenshotProps.Margin = new System.Windows.Forms.Padding(0);
            this.tlpScreenshotProps.Name = "tlpScreenshotProps";
            this.tlpScreenshotProps.RowCount = 2;
            this.tlpScreenshotProps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpScreenshotProps.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpScreenshotProps.Size = new System.Drawing.Size(608, 480);
            this.tlpScreenshotProps.TabIndex = 3;
            // 
            // pbScreenshot
            // 
            this.pbScreenshot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbScreenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbScreenshot.Location = new System.Drawing.Point(3, 3);
            this.pbScreenshot.Name = "pbScreenshot";
            this.pbScreenshot.Size = new System.Drawing.Size(602, 378);
            this.pbScreenshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbScreenshot.TabIndex = 1;
            this.pbScreenshot.TabStop = false;
            this.pbScreenshot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbScreenshot_MouseDown);
            // 
            // pgScreenshot
            // 
            this.pgScreenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgScreenshot.HelpVisible = false;
            this.pgScreenshot.Location = new System.Drawing.Point(3, 387);
            this.pgScreenshot.Name = "pgScreenshot";
            this.pgScreenshot.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgScreenshot.Size = new System.Drawing.Size(602, 90);
            this.pgScreenshot.TabIndex = 2;
            this.pgScreenshot.ToolbarVisible = false;
            // 
            // tpPublish
            // 
            this.tpPublish.Controls.Add(this.tlpPublish);
            this.tpPublish.Location = new System.Drawing.Point(4, 22);
            this.tpPublish.Name = "tpPublish";
            this.tpPublish.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpPublish.Size = new System.Drawing.Size(874, 486);
            this.tpPublish.TabIndex = 2;
            this.tpPublish.Text = "Publish";
            this.tpPublish.UseVisualStyleBackColor = true;
            // 
            // tlpPublish
            // 
            this.tlpPublish.ColumnCount = 3;
            this.tlpPublish.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00001F));
            this.tlpPublish.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.99999F));
            this.tlpPublish.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tlpPublish.Controls.Add(this.gbQuickPublish, 2, 0);
            this.tlpPublish.Controls.Add(this.txtPublish, 1, 0);
            this.tlpPublish.Controls.Add(this.lbPublish, 0, 0);
            this.tlpPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPublish.Location = new System.Drawing.Point(3, 3);
            this.tlpPublish.Name = "tlpPublish";
            this.tlpPublish.RowCount = 1;
            this.tlpPublish.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPublish.Size = new System.Drawing.Size(868, 480);
            this.tlpPublish.TabIndex = 2;
            // 
            // gbQuickPublish
            // 
            this.gbQuickPublish.Controls.Add(this.flpPublishConfig);
            this.gbQuickPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQuickPublish.Location = new System.Drawing.Point(724, 3);
            this.gbQuickPublish.Name = "gbQuickPublish";
            this.gbQuickPublish.Size = new System.Drawing.Size(141, 474);
            this.gbQuickPublish.TabIndex = 1;
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
            this.flpPublishConfig.Size = new System.Drawing.Size(135, 455);
            this.flpPublishConfig.TabIndex = 7;
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
            this.cboQuickPublishType.TabIndex = 7;
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
            this.cboQuickTemplate.TabIndex = 3;
            this.cboQuickTemplate.SelectedIndexChanged += new System.EventHandler(this.cboQuickTemplate_SelectedIndexChanged);
            // 
            // txtPublish
            // 
            this.txtPublish.AcceptsReturn = true;
            this.txtPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPublish.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPublish.Location = new System.Drawing.Point(147, 3);
            this.txtPublish.Multiline = true;
            this.txtPublish.Name = "txtPublish";
            this.txtPublish.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPublish.Size = new System.Drawing.Size(571, 474);
            this.txtPublish.TabIndex = 0;
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
            this.lbPublish.Size = new System.Drawing.Size(138, 474);
            this.lbPublish.TabIndex = 2;
            this.lbPublish.SelectedIndexChanged += new System.EventHandler(this.LbPublishSelectedIndexChanged);
            // 
            // tpScreenshotOptions
            // 
            this.tpScreenshotOptions.Controls.Add(this.tcThumbnailers);
            this.tpScreenshotOptions.Location = new System.Drawing.Point(4, 22);
            this.tpScreenshotOptions.Name = "tpScreenshotOptions";
            this.tpScreenshotOptions.Size = new System.Drawing.Size(874, 486);
            this.tpScreenshotOptions.TabIndex = 6;
            this.tpScreenshotOptions.Text = "Thumbnailers";
            this.tpScreenshotOptions.UseVisualStyleBackColor = true;
            // 
            // tcThumbnailers
            // 
            this.tcThumbnailers.Controls.Add(this.tpThumbnailersGeneral);
            this.tcThumbnailers.Controls.Add(this.tpMtn);
            this.tcThumbnailers.Controls.Add(this.tpMPlayer);
            this.tcThumbnailers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcThumbnailers.Location = new System.Drawing.Point(0, 0);
            this.tcThumbnailers.Name = "tcThumbnailers";
            this.tcThumbnailers.SelectedIndex = 0;
            this.tcThumbnailers.Size = new System.Drawing.Size(874, 486);
            this.tcThumbnailers.TabIndex = 0;
            // 
            // tpThumbnailersGeneral
            // 
            this.tpThumbnailersGeneral.Controls.Add(this.gbUploadScreenshots);
            this.tpThumbnailersGeneral.Controls.Add(this.gbThumbnailer);
            this.tpThumbnailersGeneral.Controls.Add(this.gbScreenshotsLoc);
            this.tpThumbnailersGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpThumbnailersGeneral.Name = "tpThumbnailersGeneral";
            this.tpThumbnailersGeneral.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpThumbnailersGeneral.Size = new System.Drawing.Size(866, 460);
            this.tpThumbnailersGeneral.TabIndex = 5;
            this.tpThumbnailersGeneral.Text = "General";
            this.tpThumbnailersGeneral.UseVisualStyleBackColor = true;
            // 
            // gbUploadScreenshots
            // 
            this.gbUploadScreenshots.Controls.Add(this.flpScreenshots);
            this.gbUploadScreenshots.Location = new System.Drawing.Point(16, 208);
            this.gbUploadScreenshots.Name = "gbUploadScreenshots";
            this.gbUploadScreenshots.Size = new System.Drawing.Size(784, 72);
            this.gbUploadScreenshots.TabIndex = 10;
            this.gbUploadScreenshots.TabStop = false;
            this.gbUploadScreenshots.Text = "Step 3 - Upload screenshots";
            // 
            // flpScreenshots
            // 
            this.flpScreenshots.AutoSize = true;
            this.flpScreenshots.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpScreenshots.Controls.Add(this.chkScreenshotUpload);
            this.flpScreenshots.Controls.Add(this.cboScreenshotDest);
            this.flpScreenshots.Controls.Add(this.btnUploadersConfig);
            this.flpScreenshots.Location = new System.Drawing.Point(8, 24);
            this.flpScreenshots.Name = "flpScreenshots";
            this.flpScreenshots.Size = new System.Drawing.Size(529, 30);
            this.flpScreenshots.TabIndex = 0;
            // 
            // chkScreenshotUpload
            // 
            this.chkScreenshotUpload.AutoSize = true;
            this.chkScreenshotUpload.Checked = true;
            this.chkScreenshotUpload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScreenshotUpload.Location = new System.Drawing.Point(3, 3);
            this.chkScreenshotUpload.Name = "chkScreenshotUpload";
            this.chkScreenshotUpload.Size = new System.Drawing.Size(129, 17);
            this.chkScreenshotUpload.TabIndex = 0;
            this.chkScreenshotUpload.Text = "Upload Screenshot to";
            this.chkScreenshotUpload.UseVisualStyleBackColor = true;
            this.chkScreenshotUpload.CheckedChanged += new System.EventHandler(this.chkScreenshotUpload_CheckedChanged);
            // 
            // cboScreenshotDest
            // 
            this.cboScreenshotDest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScreenshotDest.FormattingEnabled = true;
            this.cboScreenshotDest.Location = new System.Drawing.Point(138, 3);
            this.cboScreenshotDest.Name = "cboScreenshotDest";
            this.cboScreenshotDest.Size = new System.Drawing.Size(222, 21);
            this.cboScreenshotDest.TabIndex = 2;
            this.cboScreenshotDest.SelectedIndexChanged += new System.EventHandler(this.cboScreenshotDest_SelectedIndexChanged);
            // 
            // btnUploadersConfig
            // 
            this.btnUploadersConfig.Location = new System.Drawing.Point(366, 3);
            this.btnUploadersConfig.Name = "btnUploadersConfig";
            this.btnUploadersConfig.Size = new System.Drawing.Size(160, 24);
            this.btnUploadersConfig.TabIndex = 10;
            this.btnUploadersConfig.Text = "Uploaders Configuration...";
            this.btnUploadersConfig.UseVisualStyleBackColor = true;
            this.btnUploadersConfig.Click += new System.EventHandler(this.btnUploadersConfig_Click);
            // 
            // gbThumbnailer
            // 
            this.gbThumbnailer.Controls.Add(this.cboThumbnailer);
            this.gbThumbnailer.Location = new System.Drawing.Point(16, 16);
            this.gbThumbnailer.Name = "gbThumbnailer";
            this.gbThumbnailer.Size = new System.Drawing.Size(784, 45);
            this.gbThumbnailer.TabIndex = 0;
            this.gbThumbnailer.TabStop = false;
            this.gbThumbnailer.Text = "Step 1- Preferred Thumbnailer";
            // 
            // cboThumbnailer
            // 
            this.cboThumbnailer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThumbnailer.FormattingEnabled = true;
            this.cboThumbnailer.Items.AddRange(new object[] {
            "MTN",
            "MPlayer"});
            this.cboThumbnailer.Location = new System.Drawing.Point(8, 16);
            this.cboThumbnailer.Name = "cboThumbnailer";
            this.cboThumbnailer.Size = new System.Drawing.Size(200, 21);
            this.cboThumbnailer.TabIndex = 0;
            this.cboThumbnailer.SelectedIndexChanged += new System.EventHandler(this.cboThumbnailer_SelectedIndexChanged);
            // 
            // gbScreenshotsLoc
            // 
            this.gbScreenshotsLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbScreenshotsLoc.Controls.Add(this.label2);
            this.gbScreenshotsLoc.Controls.Add(this.cboScreenshotsLoc);
            this.gbScreenshotsLoc.Controls.Add(this.txtScreenshotsLoc);
            this.gbScreenshotsLoc.Controls.Add(this.btnScreenshotsLocBrowse);
            this.gbScreenshotsLoc.Location = new System.Drawing.Point(16, 88);
            this.gbScreenshotsLoc.Name = "gbScreenshotsLoc";
            this.gbScreenshotsLoc.Size = new System.Drawing.Size(784, 96);
            this.gbScreenshotsLoc.TabIndex = 9;
            this.gbScreenshotsLoc.TabStop = false;
            this.gbScreenshotsLoc.Text = "Step 2 - Save thumbnais to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Custom Directory";
            // 
            // cboScreenshotsLoc
            // 
            this.cboScreenshotsLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScreenshotsLoc.FormattingEnabled = true;
            this.cboScreenshotsLoc.Location = new System.Drawing.Point(16, 24);
            this.cboScreenshotsLoc.Name = "cboScreenshotsLoc";
            this.cboScreenshotsLoc.Size = new System.Drawing.Size(288, 21);
            this.cboScreenshotsLoc.TabIndex = 10;
            this.cboScreenshotsLoc.SelectedIndexChanged += new System.EventHandler(this.cboScreenshotsLoc_SelectedIndexChanged);
            // 
            // txtScreenshotsLoc
            // 
            this.txtScreenshotsLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScreenshotsLoc.Location = new System.Drawing.Point(112, 52);
            this.txtScreenshotsLoc.Name = "txtScreenshotsLoc";
            this.txtScreenshotsLoc.Size = new System.Drawing.Size(544, 20);
            this.txtScreenshotsLoc.TabIndex = 8;
            // 
            // btnScreenshotsLocBrowse
            // 
            this.btnScreenshotsLocBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScreenshotsLocBrowse.Location = new System.Drawing.Point(664, 51);
            this.btnScreenshotsLocBrowse.Name = "btnScreenshotsLocBrowse";
            this.btnScreenshotsLocBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnScreenshotsLocBrowse.TabIndex = 9;
            this.btnScreenshotsLocBrowse.Text = "&Browse";
            this.btnScreenshotsLocBrowse.UseVisualStyleBackColor = true;
            this.btnScreenshotsLocBrowse.Click += new System.EventHandler(this.btnScreenshotsLocBrowse_Click);
            // 
            // tpMtn
            // 
            this.tpMtn.Controls.Add(this.tlpMTN);
            this.tpMtn.Location = new System.Drawing.Point(4, 22);
            this.tpMtn.Name = "tpMtn";
            this.tpMtn.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpMtn.Size = new System.Drawing.Size(868, 462);
            this.tpMtn.TabIndex = 0;
            this.tpMtn.Text = "MTN";
            this.tpMtn.UseVisualStyleBackColor = true;
            // 
            // tlpMTN
            // 
            this.tlpMTN.ColumnCount = 1;
            this.tlpMTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMTN.Controls.Add(this.tlpMtnUsage, 0, 0);
            this.tlpMTN.Controls.Add(this.txtMtnArgs, 0, 1);
            this.tlpMTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMTN.Location = new System.Drawing.Point(3, 3);
            this.tlpMTN.Name = "tlpMTN";
            this.tlpMTN.RowCount = 2;
            this.tlpMTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tlpMTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpMTN.Size = new System.Drawing.Size(862, 456);
            this.tlpMTN.TabIndex = 7;
            // 
            // tlpMtnUsage
            // 
            this.tlpMtnUsage.ColumnCount = 2;
            this.tlpMtnUsage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpMtnUsage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlpMtnUsage.Controls.Add(this.pgMtn, 1, 0);
            this.tlpMtnUsage.Controls.Add(this.tlpMtnProfiles, 0, 0);
            this.tlpMtnUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMtnUsage.Location = new System.Drawing.Point(3, 3);
            this.tlpMtnUsage.Name = "tlpMtnUsage";
            this.tlpMtnUsage.RowCount = 1;
            this.tlpMtnUsage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMtnUsage.Size = new System.Drawing.Size(856, 404);
            this.tlpMtnUsage.TabIndex = 1;
            // 
            // pgMtn
            // 
            this.pgMtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMtn.Location = new System.Drawing.Point(217, 3);
            this.pgMtn.Name = "pgMtn";
            this.pgMtn.Size = new System.Drawing.Size(636, 398);
            this.pgMtn.TabIndex = 0;
            this.pgMtn.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PgMtnPropertyValueChanged);
            // 
            // tlpMtnProfiles
            // 
            this.tlpMtnProfiles.ColumnCount = 1;
            this.tlpMtnProfiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMtnProfiles.Controls.Add(this.flpMtn, 0, 1);
            this.tlpMtnProfiles.Controls.Add(this.lbMtnProfiles, 0, 0);
            this.tlpMtnProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMtnProfiles.Location = new System.Drawing.Point(3, 3);
            this.tlpMtnProfiles.Name = "tlpMtnProfiles";
            this.tlpMtnProfiles.RowCount = 2;
            this.tlpMtnProfiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMtnProfiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpMtnProfiles.Size = new System.Drawing.Size(208, 398);
            this.tlpMtnProfiles.TabIndex = 1;
            // 
            // flpMtn
            // 
            this.flpMtn.Controls.Add(this.tbnAddMtnProfile);
            this.flpMtn.Controls.Add(this.btnRemoveMtnProfile);
            this.flpMtn.Location = new System.Drawing.Point(3, 365);
            this.flpMtn.Name = "flpMtn";
            this.flpMtn.Size = new System.Drawing.Size(162, 29);
            this.flpMtn.TabIndex = 7;
            // 
            // tbnAddMtnProfile
            // 
            this.tbnAddMtnProfile.Location = new System.Drawing.Point(3, 3);
            this.tbnAddMtnProfile.Name = "tbnAddMtnProfile";
            this.tbnAddMtnProfile.Size = new System.Drawing.Size(75, 23);
            this.tbnAddMtnProfile.TabIndex = 0;
            this.tbnAddMtnProfile.Text = "Add...";
            this.tbnAddMtnProfile.UseVisualStyleBackColor = true;
            this.tbnAddMtnProfile.Click += new System.EventHandler(this.TbnAddMtnProfileClick);
            // 
            // btnRemoveMtnProfile
            // 
            this.btnRemoveMtnProfile.Location = new System.Drawing.Point(84, 3);
            this.btnRemoveMtnProfile.Name = "btnRemoveMtnProfile";
            this.btnRemoveMtnProfile.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveMtnProfile.TabIndex = 1;
            this.btnRemoveMtnProfile.Text = "Remove";
            this.btnRemoveMtnProfile.UseVisualStyleBackColor = true;
            this.btnRemoveMtnProfile.Click += new System.EventHandler(this.BtnRemoveMtnProfileClick);
            // 
            // lbMtnProfiles
            // 
            this.lbMtnProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMtnProfiles.FormattingEnabled = true;
            this.lbMtnProfiles.Location = new System.Drawing.Point(3, 3);
            this.lbMtnProfiles.Name = "lbMtnProfiles";
            this.lbMtnProfiles.Size = new System.Drawing.Size(202, 356);
            this.lbMtnProfiles.TabIndex = 8;
            this.lbMtnProfiles.SelectedIndexChanged += new System.EventHandler(this.LbMtnProfilesSelectedIndexChanged);
            // 
            // txtMtnArgs
            // 
            this.txtMtnArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMtnArgs.Location = new System.Drawing.Point(3, 413);
            this.txtMtnArgs.Multiline = true;
            this.txtMtnArgs.Name = "txtMtnArgs";
            this.txtMtnArgs.ReadOnly = true;
            this.txtMtnArgs.Size = new System.Drawing.Size(856, 40);
            this.txtMtnArgs.TabIndex = 6;
            // 
            // tpMPlayer
            // 
            this.tpMPlayer.Controls.Add(this.pgMPlayerOptions);
            this.tpMPlayer.Location = new System.Drawing.Point(4, 22);
            this.tpMPlayer.Name = "tpMPlayer";
            this.tpMPlayer.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpMPlayer.Size = new System.Drawing.Size(868, 462);
            this.tpMPlayer.TabIndex = 1;
            this.tpMPlayer.Text = "MPlayer";
            this.tpMPlayer.UseVisualStyleBackColor = true;
            // 
            // pgMPlayerOptions
            // 
            this.pgMPlayerOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMPlayerOptions.Location = new System.Drawing.Point(3, 3);
            this.pgMPlayerOptions.Name = "pgMPlayerOptions";
            this.pgMPlayerOptions.Size = new System.Drawing.Size(862, 456);
            this.pgMPlayerOptions.TabIndex = 1;
            // 
            // tpPublishOptions
            // 
            this.tpPublishOptions.Controls.Add(this.cboPublishType);
            this.tpPublishOptions.Controls.Add(this.btnTemplatesRewrite);
            this.tpPublishOptions.Controls.Add(this.cboTemplate);
            this.tpPublishOptions.Controls.Add(this.gbTemplatesInternal);
            this.tpPublishOptions.Controls.Add(this.gbFonts);
            this.tpPublishOptions.Controls.Add(this.chkUploadFullScreenshot);
            this.tpPublishOptions.Controls.Add(this.chkTemplatesMode);
            this.tpPublishOptions.Location = new System.Drawing.Point(4, 22);
            this.tpPublishOptions.Name = "tpPublishOptions";
            this.tpPublishOptions.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpPublishOptions.Size = new System.Drawing.Size(874, 486);
            this.tpPublishOptions.TabIndex = 2;
            this.tpPublishOptions.Text = "Publish Templates";
            this.tpPublishOptions.UseVisualStyleBackColor = true;
            // 
            // cboPublishType
            // 
            this.cboPublishType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPublishType.FormattingEnabled = true;
            this.cboPublishType.Location = new System.Drawing.Point(160, 13);
            this.cboPublishType.Name = "cboPublishType";
            this.cboPublishType.Size = new System.Drawing.Size(195, 21);
            this.cboPublishType.TabIndex = 14;
            this.cboPublishType.SelectedIndexChanged += new System.EventHandler(this.cboPublishType_SelectedIndexChanged_1);
            // 
            // btnTemplatesRewrite
            // 
            this.btnTemplatesRewrite.AutoSize = true;
            this.btnTemplatesRewrite.Location = new System.Drawing.Point(16, 233);
            this.btnTemplatesRewrite.Name = "btnTemplatesRewrite";
            this.btnTemplatesRewrite.Size = new System.Drawing.Size(196, 27);
            this.btnTemplatesRewrite.TabIndex = 13;
            this.btnTemplatesRewrite.Text = "&Rewrite Default Templates...";
            this.btnTemplatesRewrite.UseVisualStyleBackColor = true;
            this.btnTemplatesRewrite.Click += new System.EventHandler(this.btnTemplatesRewrite_Click);
            // 
            // cboTemplate
            // 
            this.cboTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTemplate.FormattingEnabled = true;
            this.cboTemplate.Location = new System.Drawing.Point(360, 13);
            this.cboTemplate.Name = "cboTemplate";
            this.cboTemplate.Size = new System.Drawing.Size(195, 21);
            this.cboTemplate.TabIndex = 9;
            this.cboTemplate.SelectedIndexChanged += new System.EventHandler(this.cboTemplate_SelectedIndexChanged);
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
            this.gbTemplatesInternal.TabIndex = 3;
            this.gbTemplatesInternal.TabStop = false;
            this.gbTemplatesInternal.Text = "Internal Template Settings";
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
            this.nudFontSizeIncr.TabIndex = 9;
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
            this.chkPre.Size = new System.Drawing.Size(132, 17);
            this.chkPre.TabIndex = 1;
            this.chkPre.Text = "Use Preformatted Text";
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
            this.chkPreIncreaseFontSize.Size = new System.Drawing.Size(148, 17);
            this.chkPreIncreaseFontSize.TabIndex = 8;
            this.chkPreIncreaseFontSize.Text = "and increase Font Size by";
            this.chkPreIncreaseFontSize.UseVisualStyleBackColor = true;
            this.chkPreIncreaseFontSize.CheckedChanged += new System.EventHandler(this.chkPreIncreaseFontSize_CheckedChanged);
            // 
            // chkAlignCenter
            // 
            this.chkAlignCenter.AutoSize = true;
            this.chkAlignCenter.Location = new System.Drawing.Point(17, 33);
            this.chkAlignCenter.Name = "chkAlignCenter";
            this.chkAlignCenter.Size = new System.Drawing.Size(83, 17);
            this.chkAlignCenter.TabIndex = 0;
            this.chkAlignCenter.Text = "Align &Center";
            this.chkAlignCenter.UseVisualStyleBackColor = true;
            this.chkAlignCenter.CheckedChanged += new System.EventHandler(this.chkAlignCenter_CheckedChanged);
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
            this.gbFonts.TabIndex = 7;
            this.gbFonts.TabStop = false;
            this.gbFonts.Text = "Font Size";
            // 
            // nudHeading1Size
            // 
            this.nudHeading1Size.Location = new System.Drawing.Point(106, 30);
            this.nudHeading1Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading1Size.Name = "nudHeading1Size";
            this.nudHeading1Size.Size = new System.Drawing.Size(55, 20);
            this.nudHeading1Size.TabIndex = 10;
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
            this.label9.Location = new System.Drawing.Point(44, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Heading 1";
            // 
            // nudHeading2Size
            // 
            this.nudHeading2Size.Location = new System.Drawing.Point(106, 53);
            this.nudHeading2Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading2Size.Name = "nudHeading2Size";
            this.nudHeading2Size.Size = new System.Drawing.Size(55, 20);
            this.nudHeading2Size.TabIndex = 8;
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
            this.label8.Location = new System.Drawing.Point(44, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Heading 2";
            // 
            // nudHeading3Size
            // 
            this.nudHeading3Size.Location = new System.Drawing.Point(106, 79);
            this.nudHeading3Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeading3Size.Name = "nudHeading3Size";
            this.nudHeading3Size.Size = new System.Drawing.Size(55, 20);
            this.nudHeading3Size.TabIndex = 4;
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
            this.label7.Location = new System.Drawing.Point(69, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Body";
            // 
            // nudBodySize
            // 
            this.nudBodySize.Location = new System.Drawing.Point(106, 105);
            this.nudBodySize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBodySize.Name = "nudBodySize";
            this.nudBodySize.Size = new System.Drawing.Size(55, 20);
            this.nudBodySize.TabIndex = 5;
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
            this.label3.Location = new System.Drawing.Point(44, 82);
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
            this.chkUploadFullScreenshot.Size = new System.Drawing.Size(200, 17);
            this.chkUploadFullScreenshot.TabIndex = 1;
            this.chkUploadFullScreenshot.Text = "Use &Full Image instead of Thumbnail ";
            this.chkUploadFullScreenshot.UseVisualStyleBackColor = true;
            this.chkUploadFullScreenshot.CheckedChanged += new System.EventHandler(this.chkUploadFullScreenshot_CheckedChanged);
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
            // tpTorrentCreator
            // 
            this.tpTorrentCreator.Controls.Add(this.btnRefreshTrackers);
            this.tpTorrentCreator.Controls.Add(this.groupBox8);
            this.tpTorrentCreator.Controls.Add(this.gbTrackerMgr);
            this.tpTorrentCreator.Controls.Add(this.cboTrackerGroupActive);
            this.tpTorrentCreator.Controls.Add(this.chkCreateTorrent);
            this.tpTorrentCreator.Location = new System.Drawing.Point(4, 22);
            this.tpTorrentCreator.Name = "tpTorrentCreator";
            this.tpTorrentCreator.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpTorrentCreator.Size = new System.Drawing.Size(874, 486);
            this.tpTorrentCreator.TabIndex = 1;
            this.tpTorrentCreator.Text = "Torrent Creator";
            this.tpTorrentCreator.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.cboTorrentLoc);
            this.groupBox8.Controls.Add(this.chkWritePublish);
            this.groupBox8.Controls.Add(this.chkTorrentOrganize);
            this.groupBox8.Controls.Add(this.btnBrowseTorrentCustomFolder);
            this.groupBox8.Controls.Add(this.txtTorrentCustomFolder);
            this.groupBox8.Location = new System.Drawing.Point(17, 288);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(827, 122);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Save Location";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Custom Directory";
            // 
            // cboTorrentLoc
            // 
            this.cboTorrentLoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTorrentLoc.FormattingEnabled = true;
            this.cboTorrentLoc.Location = new System.Drawing.Point(16, 16);
            this.cboTorrentLoc.Name = "cboTorrentLoc";
            this.cboTorrentLoc.Size = new System.Drawing.Size(288, 21);
            this.cboTorrentLoc.TabIndex = 6;
            this.cboTorrentLoc.SelectedIndexChanged += new System.EventHandler(this.cboTorrentLoc_SelectedIndexChanged);
            // 
            // chkWritePublish
            // 
            this.chkWritePublish.AutoSize = true;
            this.chkWritePublish.Location = new System.Drawing.Point(16, 96);
            this.chkWritePublish.Name = "chkWritePublish";
            this.chkWritePublish.Size = new System.Drawing.Size(241, 17);
            this.chkWritePublish.TabIndex = 5;
            this.chkWritePublish.Text = "Write Publish Information of the Torrent to File";
            this.chkWritePublish.UseVisualStyleBackColor = true;
            this.chkWritePublish.CheckedChanged += new System.EventHandler(this.chkWritePublish_CheckedChanged);
            // 
            // chkTorrentOrganize
            // 
            this.chkTorrentOrganize.AutoSize = true;
            this.chkTorrentOrganize.Checked = true;
            this.chkTorrentOrganize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTorrentOrganize.Location = new System.Drawing.Point(114, 71);
            this.chkTorrentOrganize.Name = "chkTorrentOrganize";
            this.chkTorrentOrganize.Size = new System.Drawing.Size(293, 17);
            this.chkTorrentOrganize.TabIndex = 4;
            this.chkTorrentOrganize.Text = "Create torrents in sub-folders according to Tracker Name";
            this.chkTorrentOrganize.UseVisualStyleBackColor = true;
            this.chkTorrentOrganize.CheckedChanged += new System.EventHandler(this.chkTorrentOrganize_CheckedChanged);
            // 
            // btnBrowseTorrentCustomFolder
            // 
            this.btnBrowseTorrentCustomFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTorrentCustomFolder.Location = new System.Drawing.Point(722, 43);
            this.btnBrowseTorrentCustomFolder.Name = "btnBrowseTorrentCustomFolder";
            this.btnBrowseTorrentCustomFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTorrentCustomFolder.TabIndex = 3;
            this.btnBrowseTorrentCustomFolder.Text = "&Browse";
            this.btnBrowseTorrentCustomFolder.UseVisualStyleBackColor = true;
            this.btnBrowseTorrentCustomFolder.Click += new System.EventHandler(this.btnBrowseTorrentCustomFolder_Click);
            // 
            // txtTorrentCustomFolder
            // 
            this.txtTorrentCustomFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTorrentCustomFolder.Location = new System.Drawing.Point(112, 44);
            this.txtTorrentCustomFolder.Name = "txtTorrentCustomFolder";
            this.txtTorrentCustomFolder.Size = new System.Drawing.Size(602, 20);
            this.txtTorrentCustomFolder.TabIndex = 2;
            this.txtTorrentCustomFolder.TextChanged += new System.EventHandler(this.txtTorrentCustomFolder_TextChanged);
            // 
            // gbTrackerMgr
            // 
            this.gbTrackerMgr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTrackerMgr.Controls.Add(this.tlpTrackers);
            this.gbTrackerMgr.Location = new System.Drawing.Point(17, 40);
            this.gbTrackerMgr.Name = "gbTrackerMgr";
            this.gbTrackerMgr.Size = new System.Drawing.Size(827, 240);
            this.gbTrackerMgr.TabIndex = 3;
            this.gbTrackerMgr.TabStop = false;
            this.gbTrackerMgr.Text = "Tracker Manager";
            // 
            // tlpTrackers
            // 
            this.tlpTrackers.ColumnCount = 3;
            this.tlpTrackers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpTrackers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpTrackers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTrackers.Controls.Add(this.flpTrackers, 1, 1);
            this.tlpTrackers.Controls.Add(this.pgTracker, 2, 0);
            this.tlpTrackers.Controls.Add(this.flpTrackerGroups, 0, 1);
            this.tlpTrackers.Controls.Add(this.gbTrackerGroups, 0, 0);
            this.tlpTrackers.Controls.Add(this.gbTrackers, 1, 0);
            this.tlpTrackers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTrackers.Location = new System.Drawing.Point(3, 16);
            this.tlpTrackers.Name = "tlpTrackers";
            this.tlpTrackers.RowCount = 2;
            this.tlpTrackers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTrackers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpTrackers.Size = new System.Drawing.Size(821, 221);
            this.tlpTrackers.TabIndex = 0;
            // 
            // flpTrackers
            // 
            this.flpTrackers.Controls.Add(this.btnAddTracker);
            this.flpTrackers.Controls.Add(this.btnRemoveTracker);
            this.flpTrackers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTrackers.Location = new System.Drawing.Point(208, 188);
            this.flpTrackers.Name = "flpTrackers";
            this.flpTrackers.Size = new System.Drawing.Size(199, 30);
            this.flpTrackers.TabIndex = 4;
            // 
            // btnAddTracker
            // 
            this.btnAddTracker.Location = new System.Drawing.Point(3, 3);
            this.btnAddTracker.Name = "btnAddTracker";
            this.btnAddTracker.Size = new System.Drawing.Size(75, 23);
            this.btnAddTracker.TabIndex = 0;
            this.btnAddTracker.Text = "Add";
            this.btnAddTracker.UseVisualStyleBackColor = true;
            this.btnAddTracker.Click += new System.EventHandler(this.BtnAddTrackerClick);
            // 
            // btnRemoveTracker
            // 
            this.btnRemoveTracker.Location = new System.Drawing.Point(84, 3);
            this.btnRemoveTracker.Name = "btnRemoveTracker";
            this.btnRemoveTracker.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTracker.TabIndex = 1;
            this.btnRemoveTracker.Text = "Remove";
            this.btnRemoveTracker.UseVisualStyleBackColor = true;
            this.btnRemoveTracker.Click += new System.EventHandler(this.btnRemoveTracker_Click);
            // 
            // pgTracker
            // 
            this.pgTracker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgTracker.Location = new System.Drawing.Point(413, 3);
            this.pgTracker.Name = "pgTracker";
            this.pgTracker.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgTracker.Size = new System.Drawing.Size(405, 179);
            this.pgTracker.TabIndex = 1;
            this.pgTracker.ToolbarVisible = false;
            this.pgTracker.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PgTrackerPropertyValueChanged);
            // 
            // flpTrackerGroups
            // 
            this.flpTrackerGroups.Controls.Add(this.btnAddTrackerGroup);
            this.flpTrackerGroups.Controls.Add(this.btnRemoveTrackerGroup);
            this.flpTrackerGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTrackerGroups.Location = new System.Drawing.Point(3, 188);
            this.flpTrackerGroups.Name = "flpTrackerGroups";
            this.flpTrackerGroups.Size = new System.Drawing.Size(199, 30);
            this.flpTrackerGroups.TabIndex = 3;
            // 
            // btnAddTrackerGroup
            // 
            this.btnAddTrackerGroup.Location = new System.Drawing.Point(3, 3);
            this.btnAddTrackerGroup.Name = "btnAddTrackerGroup";
            this.btnAddTrackerGroup.Size = new System.Drawing.Size(75, 23);
            this.btnAddTrackerGroup.TabIndex = 0;
            this.btnAddTrackerGroup.Text = "Add";
            this.btnAddTrackerGroup.UseVisualStyleBackColor = true;
            this.btnAddTrackerGroup.Click += new System.EventHandler(this.btnAddTrackerGroup_Click);
            // 
            // btnRemoveTrackerGroup
            // 
            this.btnRemoveTrackerGroup.Location = new System.Drawing.Point(84, 3);
            this.btnRemoveTrackerGroup.Name = "btnRemoveTrackerGroup";
            this.btnRemoveTrackerGroup.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTrackerGroup.TabIndex = 1;
            this.btnRemoveTrackerGroup.Text = "Remove";
            this.btnRemoveTrackerGroup.UseVisualStyleBackColor = true;
            this.btnRemoveTrackerGroup.Click += new System.EventHandler(this.btnRemoveTrackerGroup_Click);
            // 
            // gbTrackerGroups
            // 
            this.gbTrackerGroups.Controls.Add(this.lbTrackerGroups);
            this.gbTrackerGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTrackerGroups.Location = new System.Drawing.Point(3, 3);
            this.gbTrackerGroups.Name = "gbTrackerGroups";
            this.gbTrackerGroups.Size = new System.Drawing.Size(199, 179);
            this.gbTrackerGroups.TabIndex = 5;
            this.gbTrackerGroups.TabStop = false;
            this.gbTrackerGroups.Text = "Groups";
            // 
            // lbTrackerGroups
            // 
            this.lbTrackerGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTrackerGroups.FormattingEnabled = true;
            this.lbTrackerGroups.Location = new System.Drawing.Point(3, 16);
            this.lbTrackerGroups.Name = "lbTrackerGroups";
            this.lbTrackerGroups.Size = new System.Drawing.Size(193, 160);
            this.lbTrackerGroups.TabIndex = 2;
            this.lbTrackerGroups.SelectedIndexChanged += new System.EventHandler(this.lbTrackerGroups_SelectedIndexChanged);
            this.lbTrackerGroups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbTrackerGroups_MouseDoubleClick);
            // 
            // gbTrackers
            // 
            this.gbTrackers.Controls.Add(this.lbTrackers);
            this.gbTrackers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTrackers.Location = new System.Drawing.Point(208, 3);
            this.gbTrackers.Name = "gbTrackers";
            this.gbTrackers.Size = new System.Drawing.Size(199, 179);
            this.gbTrackers.TabIndex = 6;
            this.gbTrackers.TabStop = false;
            this.gbTrackers.Text = "Trackers";
            // 
            // lbTrackers
            // 
            this.lbTrackers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTrackers.FormattingEnabled = true;
            this.lbTrackers.Location = new System.Drawing.Point(3, 16);
            this.lbTrackers.Name = "lbTrackers";
            this.lbTrackers.Size = new System.Drawing.Size(193, 160);
            this.lbTrackers.TabIndex = 0;
            this.lbTrackers.SelectedIndexChanged += new System.EventHandler(this.lbTrackers_SelectedIndexChanged);
            // 
            // cboTrackerGroupActive
            // 
            this.cboTrackerGroupActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTrackerGroupActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrackerGroupActive.FormattingEnabled = true;
            this.cboTrackerGroupActive.Location = new System.Drawing.Point(209, 12);
            this.cboTrackerGroupActive.Name = "cboTrackerGroupActive";
            this.cboTrackerGroupActive.Size = new System.Drawing.Size(548, 21);
            this.cboTrackerGroupActive.TabIndex = 2;
            this.cboTrackerGroupActive.SelectedIndexChanged += new System.EventHandler(this.cboAnnounceURL_SelectedIndexChanged);
            // 
            // chkCreateTorrent
            // 
            this.chkCreateTorrent.AutoSize = true;
            this.chkCreateTorrent.Location = new System.Drawing.Point(17, 14);
            this.chkCreateTorrent.Name = "chkCreateTorrent";
            this.chkCreateTorrent.Size = new System.Drawing.Size(182, 17);
            this.chkCreateTorrent.TabIndex = 0;
            this.chkCreateTorrent.Text = "Automatically create &torrent using";
            this.chkCreateTorrent.UseVisualStyleBackColor = true;
            this.chkCreateTorrent.CheckedChanged += new System.EventHandler(this.chkCreateTorrent_CheckedChanged);
            // 
            // tpProxy
            // 
            this.tpProxy.Controls.Add(this.chkProxyEnable);
            this.tpProxy.Controls.Add(this.pgProxy);
            this.tpProxy.Location = new System.Drawing.Point(4, 22);
            this.tpProxy.Name = "tpProxy";
            this.tpProxy.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpProxy.Size = new System.Drawing.Size(874, 486);
            this.tpProxy.TabIndex = 4;
            this.tpProxy.Text = "Proxy";
            this.tpProxy.UseVisualStyleBackColor = true;
            // 
            // chkProxyEnable
            // 
            this.chkProxyEnable.Location = new System.Drawing.Point(16, 16);
            this.chkProxyEnable.Name = "chkProxyEnable";
            this.chkProxyEnable.Size = new System.Drawing.Size(104, 24);
            this.chkProxyEnable.TabIndex = 1;
            this.chkProxyEnable.Text = "Enable &Proxy";
            this.chkProxyEnable.UseVisualStyleBackColor = true;
            this.chkProxyEnable.CheckedChanged += new System.EventHandler(this.ChkProxyEnableCheckedChanged);
            // 
            // pgProxy
            // 
            this.pgProxy.Location = new System.Drawing.Point(16, 48);
            this.pgProxy.Name = "pgProxy";
            this.pgProxy.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgProxy.Size = new System.Drawing.Size(448, 152);
            this.pgProxy.TabIndex = 0;
            this.pgProxy.ToolbarVisible = false;
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.rtbDebugLog);
            this.tpDebug.Location = new System.Drawing.Point(4, 22);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpDebug.Size = new System.Drawing.Size(874, 486);
            this.tpDebug.TabIndex = 5;
            this.tpDebug.Text = "Debug";
            this.tpDebug.UseVisualStyleBackColor = true;
            // 
            // rtbDebugLog
            // 
            this.rtbDebugLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtbDebugLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebugLog.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtbDebugLog.Location = new System.Drawing.Point(3, 3);
            this.rtbDebugLog.Name = "rtbDebugLog";
            this.rtbDebugLog.ReadOnly = true;
            this.rtbDebugLog.Size = new System.Drawing.Size(868, 480);
            this.rtbDebugLog.TabIndex = 1;
            this.rtbDebugLog.Text = "";
            this.rtbDebugLog.WordWrap = false;
            // 
            // tpAdvanced
            // 
            this.tpAdvanced.Controls.Add(this.pgApp);
            this.tpAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tpAdvanced.Name = "tpAdvanced";
            this.tpAdvanced.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpAdvanced.Size = new System.Drawing.Size(874, 486);
            this.tpAdvanced.TabIndex = 3;
            this.tpAdvanced.Text = "Advanced";
            this.tpAdvanced.UseVisualStyleBackColor = true;
            // 
            // pgApp
            // 
            this.pgApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgApp.Location = new System.Drawing.Point(3, 3);
            this.pgApp.Name = "pgApp";
            this.pgApp.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgApp.Size = new System.Drawing.Size(868, 480);
            this.pgApp.TabIndex = 0;
            this.pgApp.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgApp_PropertyValueChanged);
            // 
            // MainWindow
            // 
            this.AcceptButton = this.btnBrowse;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 601);
            this.ContextMenuStrip = this.cmsApp;
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCreateTorrent);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.tcMain);
            this.MainMenuStrip = this.menuStrip1;
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
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsApp.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpMedia.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.gbSourceProp.ResumeLayout(false);
            this.gbSourceProp.PerformLayout();
            this.gbDVD.ResumeLayout(false);
            this.gbDVD.PerformLayout();
            this.gbLocation.ResumeLayout(false);
            this.gbLocation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.tpMediaInfo.ResumeLayout(false);
            this.tlpMediaInfo.ResumeLayout(false);
            this.tlpMediaInfo.PerformLayout();
            this.gbMediaInfoQuickOptions.ResumeLayout(false);
            this.gbMediaInfoQuickOptions.PerformLayout();
            this.tpScreenshots.ResumeLayout(false);
            this.tlpScreenshots.ResumeLayout(false);
            this.tlpScreenshotProps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbScreenshot)).EndInit();
            this.tpPublish.ResumeLayout(false);
            this.tlpPublish.ResumeLayout(false);
            this.tlpPublish.PerformLayout();
            this.gbQuickPublish.ResumeLayout(false);
            this.flpPublishConfig.ResumeLayout(false);
            this.flpPublishConfig.PerformLayout();
            this.tpScreenshotOptions.ResumeLayout(false);
            this.tcThumbnailers.ResumeLayout(false);
            this.tpThumbnailersGeneral.ResumeLayout(false);
            this.gbUploadScreenshots.ResumeLayout(false);
            this.gbUploadScreenshots.PerformLayout();
            this.flpScreenshots.ResumeLayout(false);
            this.flpScreenshots.PerformLayout();
            this.gbThumbnailer.ResumeLayout(false);
            this.gbScreenshotsLoc.ResumeLayout(false);
            this.gbScreenshotsLoc.PerformLayout();
            this.tpMtn.ResumeLayout(false);
            this.tlpMTN.ResumeLayout(false);
            this.tlpMTN.PerformLayout();
            this.tlpMtnUsage.ResumeLayout(false);
            this.tlpMtnProfiles.ResumeLayout(false);
            this.flpMtn.ResumeLayout(false);
            this.tpMPlayer.ResumeLayout(false);
            this.tpPublishOptions.ResumeLayout(false);
            this.tpPublishOptions.PerformLayout();
            this.gbTemplatesInternal.ResumeLayout(false);
            this.gbTemplatesInternal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSizeIncr)).EndInit();
            this.gbFonts.ResumeLayout(false);
            this.gbFonts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading1Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading2Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading3Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBodySize)).EndInit();
            this.tpTorrentCreator.ResumeLayout(false);
            this.tpTorrentCreator.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.gbTrackerMgr.ResumeLayout(false);
            this.tlpTrackers.ResumeLayout(false);
            this.flpTrackers.ResumeLayout(false);
            this.flpTrackerGroups.ResumeLayout(false);
            this.gbTrackerGroups.ResumeLayout(false);
            this.gbTrackers.ResumeLayout(false);
            this.tpProxy.ResumeLayout(false);
            this.tpDebug.ResumeLayout(false);
            this.tpAdvanced.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bwApp;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sbarIcon;
        private System.Windows.Forms.ToolStripStatusLabel sBar;
        private System.Windows.Forms.ToolStripProgressBar pBar;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.ContextMenuStrip cmsApp;
        private System.Windows.Forms.ToolStripMenuItem cmsAppAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem foldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmTorrentsDir;
        private System.Windows.Forms.ToolStripMenuItem tsmScreenshots;
        private System.Windows.Forms.ToolStripMenuItem tsmTemplates;
        private System.Windows.Forms.ToolStripMenuItem tsmLogsDir;
        private System.Windows.Forms.ToolStripStatusLabel tssPerc;
        private System.Windows.Forms.ToolStripMenuItem tmsVersionHistory;
        private System.Windows.Forms.ToolTip ttApp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmSettingsDir;
        private System.Windows.Forms.MenuStrip menuStrip1;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreferKnownFolders;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpMedia;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ListBox lbStatus;
        private System.Windows.Forms.GroupBox gbSourceProp;
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
        private System.Windows.Forms.PictureBox pbScreenshot;
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
        private System.Windows.Forms.TabPage tpAdvanced;
        private System.Windows.Forms.PropertyGrid pgApp;
        private System.Windows.Forms.TabPage tpMtn;
        private System.Windows.Forms.TableLayoutPanel tlpMTN;
        private System.Windows.Forms.TableLayoutPanel tlpMtnUsage;
        private System.Windows.Forms.PropertyGrid pgMtn;
        private System.Windows.Forms.TableLayoutPanel tlpMtnProfiles;
        private System.Windows.Forms.FlowLayoutPanel flpMtn;
        private System.Windows.Forms.Button tbnAddMtnProfile;
        private System.Windows.Forms.Button btnRemoveMtnProfile;
        private System.Windows.Forms.ListBox lbMtnProfiles;
        private System.Windows.Forms.TextBox txtMtnArgs;
        private System.Windows.Forms.TabPage tpThumbnailersGeneral;
        private System.Windows.Forms.GroupBox gbScreenshotsLoc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboScreenshotsLoc;
        private System.Windows.Forms.TextBox txtScreenshotsLoc;
        private System.Windows.Forms.Button btnScreenshotsLocBrowse;
        private System.Windows.Forms.FlowLayoutPanel flpScreenshots;
        private System.Windows.Forms.CheckBox chkScreenshotUpload;
        private System.Windows.Forms.ComboBox cboScreenshotDest;
        private System.Windows.Forms.TabPage tpPublishOptions;
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
        private System.Windows.Forms.TabPage tpTorrentCreator;
        private System.Windows.Forms.Button btnRefreshTrackers;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTorrentLoc;
        private System.Windows.Forms.CheckBox chkWritePublish;
        private System.Windows.Forms.CheckBox chkTorrentOrganize;
        private System.Windows.Forms.Button btnBrowseTorrentCustomFolder;
        private System.Windows.Forms.TextBox txtTorrentCustomFolder;
        private System.Windows.Forms.GroupBox gbTrackerMgr;
        private System.Windows.Forms.TableLayoutPanel tlpTrackers;
        private System.Windows.Forms.FlowLayoutPanel flpTrackers;
        private System.Windows.Forms.Button btnAddTracker;
        private System.Windows.Forms.Button btnRemoveTracker;
        private System.Windows.Forms.PropertyGrid pgTracker;
        private System.Windows.Forms.FlowLayoutPanel flpTrackerGroups;
        private System.Windows.Forms.Button btnAddTrackerGroup;
        private System.Windows.Forms.Button btnRemoveTrackerGroup;
        private System.Windows.Forms.GroupBox gbTrackerGroups;
        private System.Windows.Forms.ListBox lbTrackerGroups;
        private System.Windows.Forms.GroupBox gbTrackers;
        private System.Windows.Forms.ListBox lbTrackers;
        private System.Windows.Forms.ComboBox cboTrackerGroupActive;
        private System.Windows.Forms.CheckBox chkCreateTorrent;
        private System.Windows.Forms.TabPage tpProxy;
        private System.Windows.Forms.CheckBox chkProxyEnable;
        private System.Windows.Forms.PropertyGrid pgProxy;
        private System.Windows.Forms.TabPage tpDebug;
        private System.Windows.Forms.RichTextBox rtbDebugLog;
        private System.Windows.Forms.Button btnCreateTorrent;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Button btnUploadersConfig;
        private System.Windows.Forms.TabPage tpScreenshotOptions;
        private System.Windows.Forms.TabControl tcThumbnailers;
        private System.Windows.Forms.GroupBox gbThumbnailer;
        private System.Windows.Forms.ComboBox cboThumbnailer;
        private System.Windows.Forms.TabPage tpMPlayer;
        private System.Windows.Forms.GroupBox gbUploadScreenshots;
        private System.Windows.Forms.PropertyGrid pgMPlayerOptions;
        private System.Windows.Forms.GroupBox gbMediaInfoQuickOptions;
        private System.Windows.Forms.CheckBox chkMediaInfoComplete;
    }
}