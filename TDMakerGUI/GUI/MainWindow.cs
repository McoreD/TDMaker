using BDInfo;
using HelpersLib;
using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TDMakerGUI.Properties;
using TDMakerLib;
using UploadersLib;

namespace TDMaker
{
    public partial class MainWindow : Form
    {
        private TrackerManager mTrackerManager = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            LoadMedia(paths);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            ConfigureDirs();

            // ConfigureGUIForUnix();
            SettingsRead();

            // Logo
            string logo1 = Path.Combine(Application.StartupPath, "logo1.png");
            if (!File.Exists(logo1))
            {
                logo1 = Path.Combine(Program.SettingsDir, "logo1.png");
            }

            string logo2 = Path.Combine(Application.StartupPath, "logo.png");
            if (!File.Exists(logo2))
            {
                logo2 = Path.Combine(Program.SettingsDir, "logo.png");
            }

            if (File.Exists(logo1))
            {
                gbLocation.BackgroundImage = Image.FromFile(logo1);
                gbLocation.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else if (File.Exists(logo2))
            {
                //this.BackgroundImage = Image.FromFile(logo);
                //this.BackgroundImageLayout = ImageLayout.Tile;
                //tpMedia.BackgroundImage = Image.FromFile(logo);
                //tpMedia.BackgroundImageLayout = ImageLayout.None;
                pbLogo.BackgroundImage = Image.FromFile(logo2);
                pbLogo.BackgroundImageLayout = ImageLayout.Stretch;
                pbLogo.BackColor = System.Drawing.SystemColors.ControlDark;
                this.BackColor = System.Drawing.SystemColors.ControlDark;
            }

            sBar.Text = string.Format("Ready.");

            this.Text = Program.GetProductName();

            UpdateGuiControls();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            rtbDebugLog.Text = FileSystem.DebugLog.ToString();
            FileSystem.DebugLogChanged += new FileSystem.DebugLogEventHandler(FileSystem_DebugLogChanged);

            string mtnExe = (Program.IsUNIX ? "mtn" : "mtn.exe");

            if (Program.Settings.ThumbnailerType == ThumbnailerType.MovieThumbnailer)
            {
                if (!File.Exists(Program.Settings.MTNPath))
                {
                    Program.Settings.MTNPath = Path.Combine(Application.StartupPath, mtnExe);
                }

                if (!File.Exists(Program.Settings.MTNPath))
                {
                    Program.Settings.MTNPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), Application.ProductName), mtnExe);
                }

                if (!File.Exists(Program.Settings.MTNPath))
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = Application.StartupPath;
                    dlg.Title = "Browse for mtn.exe";
                    dlg.Filter = "Applications (*.exe)|*.exe";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Program.Settings.MTNPath = dlg.FileName;
                    }
                }
            }
            else if (Program.Settings.ThumbnailerType == ThumbnailerType.MPlayer)
            {
                if (!File.Exists(Program.Settings.MPlayerPath))
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    dlg.Title = "Browse for mplayer.exe or download from http://code.google.com/p/mplayer-for-windows/downloads/list";
                    dlg.Filter = "Applications (mplayer.exe)|mplayer.exe";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Program.Settings.MPlayerPath = dlg.FileName;
                    }
                    else
                    {
                        Helpers.LoadBrowserAsync("http://code.google.com/p/mplayer-for-windows/downloads/list");
                    }
                }
            }

            if (ProgramUI.ExplorerFilePaths.Count > 0)
            {
                LoadMedia(ProgramUI.ExplorerFilePaths.ToArray());
            }
        }

        private void OpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Title = "Browse for Media file...";
            StringBuilder sbExt = new StringBuilder();
            sbExt.Append("Media Files (");
            StringBuilder sbExtDesc = new StringBuilder();
            foreach (string ext in Program.Settings.SupportedFileExtVideo)
            {
                sbExtDesc.Append("*");
                sbExtDesc.Append(ext);
                sbExtDesc.Append("; ");
            }
            sbExt.Append(sbExtDesc.ToString().TrimEnd().TrimEnd(';'));
            sbExt.Append(")|");
            foreach (string ext in Program.Settings.SupportedFileExtVideo)
            {
                sbExt.Append("*");
                sbExt.Append(ext);
                sbExt.Append("; ");
            }
            dlg.Filter = sbExt.ToString().TrimEnd();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadMedia(dlg.FileNames);
            }
        }

        private void OpenFolder()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Browse for media disc folder...";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadMedia(new string[] { dlg.SelectedPath });
            }
        }

        private void LoadMedia(string[] ps)
        {
            if (1 == ps.Length)
            {
                txtTitle.Text = MediaHelper.GetMediaName(ps[0]);
            }

            if (!Program.Settings.WritePublish && ps.Length > 1)
            {
                if (MessageBox.Show("Writing Publish info to File is recommended when analysing multiple files or folders. \n\nWould you like to turn this feature on?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Program.Settings.WritePublish = true;
                }
            }

            List<TorrentCreateInfo> tps = new List<TorrentCreateInfo>();

            foreach (string p in ps)
            {
                if (File.Exists(p) || Directory.Exists(p))
                {
                    // txtMediaLocation.Text = p;
                    FileSystem.AppendDebug(string.Format("Queued {0} to create a torrent", p));
                    lbFiles.Items.Add(p);
                    TorrentCreateInfo tp = new TorrentCreateInfo(GetTracker(), p);
                    tps.Add(tp);

                    UpdateGuiControls();
                }
            }

            if (Program.Settings.AnalyzeAuto)
            {
                WorkerTask wt = new WorkerTask(bwApp, TaskType.ANALYZE_MEDIA);
                wt.FileOrDirPaths = new List<string>(ps);
                AnalyzeMedia(wt);
            }
        }

        private void AnalyzeMedia(WorkerTask wt)
        {
            if (!ValidateInput()) return;

            DialogResult dlgResult = DialogResult.OK;
            List<MediaInfo2> miList = new List<MediaInfo2>();

            MediaWizardOptions mwo = Adapter.GetMediaType(wt.FileOrDirPaths);

            wt.MediaTypeChoice = mwo.MediaTypeChoice;
            if (mwo.PromptShown)
            {
                wt.TorrentCreateAuto = mwo.CreateTorrent;
                wt.UploadScreenshot = mwo.ScreenshotsInclude;
                dlgResult = mwo.DialogResultMy;
            }
            else
            {
                // fill previous settings
                wt.TorrentCreateAuto = Program.Settings.TorrentCreateAuto;
                wt.UploadScreenshot = Program.Settings.ScreenshotsUpload;
            }

            if (!mwo.PromptShown && Program.Settings.ShowMediaWizardAlways)
            {
                MediaWizard mw = new MediaWizard(wt);
                dlgResult = mw.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    wt.TorrentCreateAuto = mw.Options.CreateTorrent;
                    wt.UploadScreenshot = mw.Options.ScreenshotsInclude;
                    wt.MediaTypeChoice = mw.Options.MediaTypeChoice;
                }
            }

            if (dlgResult == DialogResult.OK)
            {
                if (wt.MediaTypeChoice == MediaType.MediaCollection)
                {
                    wt.FileOrDirPaths.Sort();
                    string firstPath = wt.FileOrDirPaths[0];
                    MediaInfo2 mi = this.PrepareNewMedia(wt, File.Exists(firstPath) ? Path.GetDirectoryName(wt.FileOrDirPaths[0]) : firstPath);
                    foreach (string p in wt.FileOrDirPaths)
                    {
                        if (File.Exists(p))
                        {
                            mi.FileCollection.Add(p);
                        }
                    }
                    miList.Add(mi);
                }
                else
                {
                    foreach (string fd in wt.FileOrDirPaths)
                    {
                        if (File.Exists(fd) || Directory.Exists(fd))
                        {
                            MakeGUIReadyForAnalysis();

                            MediaInfo2 mi = this.PrepareNewMedia(wt, fd);

                            mi.DiscType = MediaHelper.GetSourceType(fd);

                            if (mi.DiscType == SourceType.Bluray)
                            {
                                mi.Overall = new MediaFile(FileSystemHelper.GetLargestFilePathFromDir(fd), cboSource.Text);
                                mi.Overall.Summary = BDInfo(fd);
                            }

                            if (wt.IsSingleTask() && !string.IsNullOrEmpty(txtTitle.Text))
                            {
                                mi.SetTitle(txtTitle.Text);

                                // if it is a DVD, set the title to be name of the folder.
                                this.Text = string.Format("{0} - {1}", Program.GetProductName(), MediaHelper.GetMediaName(mi.Location));
                            }
                            miList.Add(mi);
                        }
                    }
                }

                // Attach the MediaInfo2 object in to TorrentInfo
                List<TorrentInfo> tiList = new List<TorrentInfo>();
                foreach (MediaInfo2 mi in miList)
                {
                    TorrentInfo ti = new TorrentInfo(bwApp, mi);
                    tiList.Add(ti);
                }
                wt.MediaList = tiList;

                if (!bwApp.IsBusy)
                {
                    bwApp.RunWorkerAsync(wt);
                }

                UpdateGuiControls();
            }
        }

        private string BDInfo(string p)
        {
            BDInfoSettings.AutosaveReport = true;
            BDInfo.FormMain info = new BDInfo.FormMain(new string[] { p });

            info.ShowDialog(this);

            return info.Report;
        }

        private void MakeGUIReadyForAnalysis()
        {
            pBar.Value = 0;
        }

        private void bwApp_DoWork(object sender, DoWorkEventArgs e)
        {
            // start of the magic :)

            WorkerTask wt = (WorkerTask)e.Argument;

            ProgramUI.CurrentTask = wt.Task;

            switch (wt.Task)
            {
                case TaskType.ANALYZE_MEDIA:
                    e.Result = WorkerAnalyzeMedia(wt);
                    break;

                case TaskType.CREATE_TORRENT:
                    WorkerCreateTorrents(wt);
                    break;
            }
        }

        private bool ValidateInput()
        {
            StringBuilder sbMsg = new StringBuilder();

            // checks
            if (string.IsNullOrEmpty(cboSource.Text) && Program.Settings.PublishInfoTypeChoice != PublishInfoType.MediaInfo)
            {
                sbMsg.AppendLine("Source information is mandatory. Use the Source drop down menu to select the correct source type.");
            }

            if (sbMsg.Length > 0)
            {
                MessageBox.Show("The following errors were found:\n\n" + sbMsg.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p">File or Folder</param>
        /// <returns></returns>
        private List<string> CreateFileList(string p)
        {
            List<string> fl = new List<string>();
            if (File.Exists(p))
            {
                fl.Add(p);
            }
            else if (Directory.Exists(p))
            {
            }
            return fl;
        }

        /// <summary>
        /// Gets new MediaInfo2 object from settings based on GUI Controls
        /// </summary>
        /// <param name="p">File or Folder path of the Media</param>
        /// <returns>MediaInfo2 object</returns>
        private MediaInfo2 PrepareNewMedia(WorkerTask wt, string p)
        {
            MediaType mt = wt.MediaTypeChoice;
            MediaInfo2 mi = new MediaInfo2(mt, p);
            mi.Extras = cboExtras.Text;
            if (cboSource.Text == "DVD")
            {
                mi.Source = Adapter.GetDVDString(p);
            }
            else
            {
                mi.Source = cboSource.Text;
            }
            mi.Menu = cboDiscMenu.Text;
            mi.Authoring = cboAuthoring.Text;
            mi.WebLink = txtWebLink.Text;
            mi.TorrentCreateInfoMy = new TorrentCreateInfo(GetTracker(), p);

            if (Program.Settings.PublishInfoTypeChoice == PublishInfoType.ExternalTemplate)
            {
                mi.TemplateLocation = Path.Combine(Program.TemplatesDir, cboTemplate.Text);
            }

            return mi;
        }

        private void btnCreateTorrent_Click(object sender, EventArgs e)
        {
            CreateTorrentButton();
        }

        private void FileSystem_DebugLogChanged(string line)
        {
            if (!rtbDebugLog.IsDisposed)
            {
                MethodInvoker method = delegate
                {
                    rtbDebugLog.AppendText(line + Environment.NewLine);
                };

                if (this.InvokeRequired)
                {
                    this.Invoke(method);
                }
                else
                {
                    method.Invoke();
                }
            }
        }

        private void SettingsWrite()
        {
            Program.Settings.TrackerGroupActive = cboTrackerGroupActive.SelectedIndex;
            Program.Settings.TemplateIndex = cboTemplate.SelectedIndex;

            Program.Settings.TorrentLocationChoice = (LocationType)cboTorrentLoc.SelectedIndex;

            Program.Settings.ImageUploaderType = (ImageDestination)cboScreenshotDest.SelectedIndex;

            Program.Settings.Write();
            Program.UploadersConfig.Save(Program.UploadersConfigPath);
            Program.mtnProfileMgr.Write();
        }

        private void ConfigureDirs()
        {
            Program.WriteTemplates(false);

            // Read Templates to GUI
            if (Directory.Exists(Program.TemplatesDir))
            {
                string[] dirs = Directory.GetDirectories(Program.TemplatesDir);
                string[] templateNames = new string[dirs.Length];
                for (int i = 0; i < templateNames.Length; i++)
                {
                    templateNames[i] = Path.GetFileName(dirs[i]);
                }
                cboTemplate.Items.Clear();
                cboQuickTemplate.Items.Clear();
                cboTemplate.Items.AddRange(templateNames);
                cboQuickTemplate.Items.AddRange(templateNames);
            }
            if (cboTemplate.Items.Count > 0)
            {
                cboTemplate.SelectedIndex = Math.Max(Program.Settings.TemplateIndex, 0);
                cboQuickTemplate.SelectedIndex = Math.Max(Program.Settings.TemplateIndex, 0);
            }

            mTrackerManager = new TrackerManager();
        }

        private void SettingsRead()
        {
            tsmiPreferKnownFolders.Checked = Program.AppConf.PreferSystemFolders;

            SettingsReadInput();
            SettingsReadMedia();
            SettingsReadPublish();
            SettingsReadScreenshots();
            SettingsReadOptions();

            cboScreenshotDest.Items.Clear();
            foreach (ImageDestination sdt in Enum.GetValues(typeof(ImageDestination)))
            {
                cboScreenshotDest.Items.Add(sdt.ToDescriptionString());
            }
            cboScreenshotDest.SelectedIndex = (int)Program.Settings.ImageUploaderType;

            if (string.IsNullOrEmpty(Program.Settings.MTNPath))
            {
                string mtnPath = Path.Combine(Application.StartupPath, "mtn.exe");
                if (File.Exists(mtnPath))
                    Program.Settings.MTNPath = mtnPath;
            }

            if (string.IsNullOrEmpty(Program.Settings.CustomMediaInfoDllDir))
            {
                Program.Settings.CustomMediaInfoDllDir = Application.StartupPath;
            }
            Kernel32Helper.SetDllDirectory(Program.Settings.CustomMediaInfoDllDir);

            pgApp.SelectedObject = Program.Settings;
        }

        private void SettingsReadInput()
        {
            if (Program.Settings.MediaSources.Count == 0)
            {
                Program.Settings.MediaSources.AddRange(new string[] { "CAM", "TC", "TS", "R5", "DVD-Screener",
                                                            "DVD", "TV", "HDTV", "Blu-ray", "HD-DVD",
                                                            "Laser Disc", "VHS", "Unknown" });
            }
            if (Program.Settings.Extras.Count == 0)
            {
                Program.Settings.Extras.AddRange(new string[] { "Intact", "Shrunk", "Removed", "None on Source" });
            }
            if (Program.Settings.AuthoringModes.Count == 0)
            {
                Program.Settings.AuthoringModes.AddRange(new string[] { "Untouched", "Shrunk" });
            }
            if (Program.Settings.DiscMenus.Count == 0)
            {
                Program.Settings.DiscMenus.AddRange(new string[] { "Intact", "Removed", "Shrunk" });
            }
            if (Program.Settings.SupportedFileExtVideo.Count == 0)
            {
                Program.Settings.SupportedFileExtVideo.AddRange(new string[] { ".3g2", ".3gp", ".3gp2", ".3gpp", ".amr", ".asf", ".asx", ".avi", ".d2v", ".dat", ".divx", ".drc", ".dsa", ".dsm", ".dss", ".dsv", ".flc", ".fli", ".flic", ".flv", ".hdmov", ".ivf", ".m1v", ".m2ts", ".m2v", ".m4v", ".mkv", ".mov", ".mp2v", ".mp4", ".mpcpl", ".mpe", ".mpeg", ".mpg", ".mpv", ".mpv2", ".ogm", ".qt", ".ram", ".ratdvd", ".rm", ".rmvb", ".roq", ".rp", ".rpm", ".rt", ".swf", ".ts", ".vob", ".vp6", ".wm", ".wmp", ".wmv", ".wmx", ".wvx" });
            }
            if (Program.Settings.SupportedFileExtAudio.Count == 0)
            {
                Program.Settings.SupportedFileExtAudio.AddRange(new string[] { ".aac", ".aiff", ".ape", ".flac", ".m4a", ".mp3", ".mpc", ".ogg", ".mp4", ".wma" });
            }

            cboSource.Items.Clear();
            foreach (string src in Program.Settings.MediaSources)
            {
                cboSource.Items.Add(src);
            }

            cboAuthoring.Items.Clear();
            foreach (string ed in Program.Settings.AuthoringModes)
            {
                cboAuthoring.Items.Add(ed);
            }

            cboDiscMenu.Items.Clear();
            foreach (string ex in Program.Settings.DiscMenus)
            {
                cboDiscMenu.Items.Add(ex);
            }

            cboExtras.Items.Clear();
            foreach (string ex in Program.Settings.Extras)
            {
                cboExtras.Items.Add(ex);
            }
        }

        private void SettingsReadMedia()
        {
            chkAuthoring.Checked = Program.Settings.bAuthoring;
            cboAuthoring.Text = Program.Settings.AuthoringMode;

            chkDiscMenu.Checked = Program.Settings.bDiscMenu;
            cboDiscMenu.Text = Program.Settings.DiscMenu;

            chkExtras.Checked = Program.Settings.bExtras;
            cboExtras.Text = Program.Settings.Extra;

            chkTitle.Checked = Program.Settings.bTitle;
            chkWebLink.Checked = Program.Settings.bWebLink;
        }

        private void SettingsReadScreenshots()
        {
            chkScreenshotUpload.Checked = Program.Settings.ScreenshotsUpload;

            gbDVD.Visible = gbSourceProp.Visible = btnUploadersConfig.Visible = cboScreenshotDest.Visible =
                string.IsNullOrEmpty(Program.Settings.PtpImgCode);

            gbQuickPublish.Enabled = string.IsNullOrEmpty(Program.Settings.PtpImgCode);

            if (!string.IsNullOrEmpty(Program.Settings.PtpImgCode))
                chkScreenshotUpload.Text = "Upload screenshots to ptpimg.me";
        }

        private void SettingsReadPublish()
        {
            if (cboQuickPublishType.Items.Count == 0)
            {
                cboQuickPublishType.Items.AddRange(Enum.GetNames(typeof(PublishInfoType)));
                cboPublishType.Items.AddRange(Enum.GetNames(typeof(PublishInfoType)));
            }
            cboPublishType.SelectedIndex = (int)Program.Settings.PublishInfoTypeChoice;
            cboQuickPublishType.SelectedIndex = (int)Program.Settings.PublishInfoTypeChoice;
        }

        private void SettingsReadOptions()
        {
            cboTemplate.SelectedIndex = Program.Settings.TemplateIndex;
            chkUploadFullScreenshot.Checked = Program.Settings.UseFullPicture;

            chkAlignCenter.Checked = Program.Settings.AlignCenter;
            chkPre.Checked = Program.Settings.PreText;
            chkPreIncreaseFontSize.Checked = Program.Settings.LargerPreText;

            nudFontSizeIncr.Value = (decimal)Program.Settings.FontSizeIncr;
            nudHeading1Size.Value = (decimal)Program.Settings.FontSizeHeading1;
            nudHeading2Size.Value = (decimal)Program.Settings.FontSizeHeading2;
            nudHeading3Size.Value = (decimal)Program.Settings.FontSizeHeading3;
            nudBodySize.Value = (decimal)Program.Settings.FontSizeBody;

            chkProxyEnable.Checked = Program.Settings.ProxyEnabled;
            pgProxy.SelectedObject = Program.Settings.ProxySettings;

            if (cboScreenshotsLoc.Items.Count == 0)
            {
                cboScreenshotsLoc.Items.AddRange(Enum.GetNames(typeof(LocationType)));
            }
            cboScreenshotsLoc.SelectedIndex = (int)Program.Settings.ScreenshotsLoc;
            txtScreenshotsLoc.Text = Program.Settings.CustomScreenshotsDir;

            cboThumbnailer.SelectedIndex = (int)Program.Settings.ThumbnailerType;
            pgMPlayerOptions.SelectedObject = Program.Settings.MPlayerOptions;
            SettingsReadOptionsMTN();
            SettingsReadOptionsTorrents();
        }

        private void SettingsReadOptionsMTN()
        {
            if (Program.mtnProfileMgr.MtnProfiles.Count == 0)
            {
                XMLSettingsScreenshot mtnDefault1 = new XMLSettingsScreenshot("Movies (Auto Width)")
                {
                    k_ColorBackground = "eeeeee",
                    f_FontFile = "arial.ttf",
                    F_FontColor = "000000",
                    F_FontSize = 12,
                    g_GapBetweenShots = 8,
                    L_LocInfo = 4,
                    L_LocTimestamp = 2,
                    j_JpgQuality = 97,
                    N_InfoSuffix = ""
                };
                Program.mtnProfileMgr.MtnProfiles.Add(mtnDefault1);

                XMLSettingsScreenshot mtnDefault2 = new XMLSettingsScreenshot("Movies (Fixed Width)")
                {
                    k_ColorBackground = "eeeeee",
                    f_FontFile = "arial.ttf",
                    F_FontColor = "000000",
                    F_FontSize = 12,
                    g_GapBetweenShots = 8,
                    L_LocInfo = 4,
                    L_LocTimestamp = 2,
                    j_JpgQuality = 97,
                    w_Width = 800,
                    N_InfoSuffix = ""
                };
                Program.mtnProfileMgr.MtnProfiles.Add(mtnDefault2);

                XMLSettingsScreenshot mtnDefault3 = new XMLSettingsScreenshot("Protech (4x3)")
                {
                    r_Rows = 4,
                    c_Columns = 3,
                    k_ColorBackground = "000000",
                    D_EdgeDetection = 0,
                    f_FontFile = "tahomabd.ttf",
                    F_FontColor = "FFFFFF",
                    F_FontSize = 11,
                    g_GapBetweenShots = 8,
                    h_MinHeight = 225,
                    L_LocInfo = 4,
                    L_LocTimestamp = 2,
                    j_JpgQuality = 100,
                    w_Width = 1024,
                    N_InfoSuffix = ""
                };
                Program.mtnProfileMgr.MtnProfiles.Add(mtnDefault3);
            }

            if (lbMtnProfiles.Items.Count == 0)
            {
                foreach (XMLSettingsScreenshot mtnProfile in Program.mtnProfileMgr.MtnProfiles)
                {
                    lbMtnProfiles.Items.Add(mtnProfile);
                }
                lbMtnProfiles.SelectedIndex = Math.Min(Program.mtnProfileMgr.MtnProfiles.Count - 1, Program.mtnProfileMgr.MtnProfileActive);
            }

            this.chkCreateTorrent.Checked = Program.Settings.TorrentCreateAuto;
            this.chkTorrentOrganize.Checked = Program.Settings.TorrentsOrganize;
        }

        private void SettingsReadOptionsTorrents()
        {
            lbTrackerGroups.Items.Clear();
            foreach (TrackerGroup tg in Program.Settings.TrackerGroups)
            {
                lbTrackerGroups.Items.Add(tg);
                lbTrackers.Items.Clear();
                foreach (Tracker myTracker in tg.Trackers)
                {
                    lbTrackers.Items.Add(myTracker);
                }
                if (lbTrackers.Items.Count > 0)
                {
                    lbTrackers.SelectedIndex = 0;
                }
            }
            if (lbTrackerGroups.Items.Count > 0)
            {
                lbTrackerGroups.SelectedIndex = 0;
            }

            FillTrackersComboBox();
            if (cboTrackerGroupActive.Items.Count > 0 && Program.Settings.TrackerGroupActive < cboTrackerGroupActive.Items.Count)
            {
                cboTrackerGroupActive.SelectedIndex = Program.Settings.TrackerGroupActive;
            }

            if (cboTorrentLoc.Items.Count == 0)
            {
                cboTorrentLoc.Items.AddRange(Enum.GetNames(typeof(LocationType)));
            }
            cboTorrentLoc.SelectedIndex = (int)Program.Settings.TorrentLocationChoice;
            chkWritePublish.Checked = Program.Settings.WritePublish;
            chkTorrentOrganize.Checked = Program.Settings.TorrentsOrganize;

            txtTorrentCustomFolder.Text = Program.Settings.CustomTorrentsDir;
        }

        private string CreatePublishInitial(TorrentInfo ti)
        {
            PublishOptionsPacket pop = new PublishOptionsPacket();
            pop.AlignCenter = Program.Settings.AlignCenter;
            pop.FullPicture = ti.Media.UploadScreenshots && Program.Settings.UseFullPicture;
            pop.PreformattedText = Program.Settings.PreText;
            pop.PublishInfoTypeChoice = Program.Settings.PublishInfoTypeChoice;
            ti.PublishOptions = pop;

            return Adapter.CreatePublish(ti, pop);
        }

        private List<TorrentInfo> WorkerAnalyzeMedia(WorkerTask wt)
        {
            Program.LoadProxySettings();

            List<TorrentInfo> tiListTemp = wt.MediaList;

            bwApp.ReportProgress((int)ProgressType.UPDATE_PROGRESSBAR_MAX, tiListTemp.Count);

            foreach (TorrentInfo ti in tiListTemp)
            {
                MediaInfo2 mi = ti.Media;

                bwApp.ReportProgress((int)ProgressType.UPDATE_STATUSBAR_DEBUG, "Reading " + Path.GetFileName(mi.Location) + " using MediaInfo...");

                if (mi.DiscType != SourceType.Bluray)
                {
                    mi.ReadMedia();
                    bwApp.ReportProgress((int)ProgressType.REPORT_MEDIAINFO_SUMMARY, mi);
                }

                // creates screenshot
                mi.UploadScreenshots = wt.UploadScreenshot;
                if (wt.UploadScreenshot)
                {
                    ti.CreateUploadScreenshots();
                }

                ti.PublishString = CreatePublishInitial(ti);
                bwApp.ReportProgress((int)ProgressType.REPORT_TORRENTINFO, ti);

                if (Program.Settings.WritePublish)
                {
                    // create textFiles of MediaInfo
                    string txtPath = Path.Combine(mi.TorrentCreateInfoMy.TorrentFolder, mi.Overall.FileName) + ".txt";

                    if (!Directory.Exists(mi.TorrentCreateInfoMy.TorrentFolder))
                    {
                        Directory.CreateDirectory(mi.TorrentCreateInfoMy.TorrentFolder);
                    }

                    using (StreamWriter sw = new StreamWriter(txtPath))
                    {
                        sw.WriteLine(ti.PublishString);
                    }
                }

                if (wt.TorrentCreateAuto)
                {
                    mi.TorrentCreateInfoMy.CreateTorrent(bwApp);
                }

                if (Program.Settings.XMLTorrentUploadCreate)
                {
                    string fp = Path.Combine(mi.TorrentCreateInfoMy.TorrentFolder, MediaHelper.GetMediaName(mi.TorrentCreateInfoMy.MediaLocation)) + ".xml";
                    FileSystem.GetXMLTorrentUpload(mi).Write2(fp);
                }

                bwApp.ReportProgress((int)ProgressType.INCREMENT_PROGRESS_WITH_MSG, mi.Title);
            }

            return tiListTemp;
        }

        private object WorkerCreateTorrents(WorkerTask wt)
        {
            try
            {
                foreach (TorrentInfo ti in wt.MediaList)
                {
                    TorrentCreateInfo tci = ti.Media.TorrentCreateInfoMy;
                    tci.CreateTorrent(wt.MyWorker);
                    if (Program.Settings.XMLTorrentUploadCreate)
                    {
                        string fp = Path.Combine(tci.TorrentFolder, MediaHelper.GetMediaName(tci.MediaLocation)) + ".xml";
                        FileSystem.GetXMLTorrentUpload(ti.Media).Write(fp);
                    }
                }
            }
            catch (Exception ex)
            {
                bwApp.ReportProgress((int)ProgressType.UPDATE_STATUSBAR_DEBUG, ex.Message);
            }

            return null;
        }

        private void UpdateGuiControls()
        {
            btnCreateTorrent.Enabled = !bwApp.IsBusy && lbPublish.Items.Count > 0;
            btnAnalyze.Enabled = !bwApp.IsBusy && lbFiles.Items.Count > 0;

            btnPublish.Enabled = !bwApp.IsBusy && !string.IsNullOrEmpty(txtPublish.Text);

            txtTorrentCustomFolder.Enabled = Program.Settings.TorrentLocationChoice == LocationType.CustomFolder;
            btnBrowseTorrentCustomFolder.Enabled = Program.Settings.TorrentLocationChoice == LocationType.CustomFolder;
            chkTorrentOrganize.Enabled = Program.Settings.TorrentLocationChoice == LocationType.CustomFolder;

            txtScreenshotsLoc.Enabled = Program.Settings.ScreenshotsLoc == LocationType.CustomFolder;
            btnScreenshotsLocBrowse.Enabled = Program.Settings.ScreenshotsLoc == LocationType.CustomFolder;

            gbTemplatesInternal.Enabled = !chkTemplatesMode.Checked;
        }

        private void bwApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pBar.Style = ProgressBarStyle.Continuous;
            lbFiles.Items.Clear();
            sBar.Text = "Ready.";
            lbPublish.SelectedIndex = lbPublish.Items.Count - 1;
            UpdateGuiControls();
        }

        private void pbScreenshot_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pbScreenshot = sender as PictureBox;
            Process.Start(pbScreenshot.ImageLocation);
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            tssPerc.Text = (bwApp.IsBusy ? string.Format("{0}%", (100.0 * (double)pBar.Value / (double)pBar.Maximum).ToString("0.0")) : "");
            btnAnalyze.Text = "Create &Description" + (lbFiles.SelectedItems.Count > 1 ? "s" : "");
            btnCreateTorrent.Text = "Create &Torrent" + (lbPublish.SelectedItems.Count > 1 ? "s" : "");
            btnBrowse.Enabled = !bwApp.IsBusy;
            btnBrowseDir.Enabled = !bwApp.IsBusy;
            btnAnalyze.Enabled = !bwApp.IsBusy && lbFiles.Items.Count > 0;
            lbStatus.SelectedIndex = lbStatus.Items.Count - 1;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void CopyPublish()
        {
            if (!string.IsNullOrEmpty(txtPublish.Text))
            {
                Clipboard.SetText(txtPublish.Text);
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            CopyPublish();
        }

        private void bwApp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                string msg = "";
                if (e.UserState.GetType() == typeof(string))
                {
                    msg = e.UserState.ToString();
                }

                ProgressType perc = (ProgressType)e.ProgressPercentage;

                switch (perc)
                {
                    case ProgressType.INCREMENT_PROGRESS_WITH_MSG:
                        pBar.Style = ProgressBarStyle.Continuous;
                        pBar.Increment(1);
                        sBar.Text = msg;
                        break;

                    case ProgressType.REPORT_MEDIAINFO_SUMMARY:
                        MediaInfo2 mi = (MediaInfo2)e.UserState;
                        gbDVD.Enabled = (mi.MediaTypeChoice == MediaType.MediaDisc);
                        foreach (MediaFile mf in mi.MediaFiles)
                        {
                            lbMediaInfo.Items.Add(mf);
                            lbMediaInfo.SelectedIndex = lbMediaInfo.Items.Count - 1;
                        }
                        break;

                    case ProgressType.REPORT_TORRENTINFO:
                        TorrentInfo ti = e.UserState as TorrentInfo;
                        lbPublish.Items.Add(ti);

                        // initialize quick publish checkboxes
                        chkQuickFullPicture.Checked = Program.Settings.UseFullPicture;
                        chkQuickAlignCenter.Checked = Program.Settings.AlignCenter;
                        chkQuickPre.Checked = Program.Settings.PreText;
                        cboQuickPublishType.SelectedIndex = cboPublishType.SelectedIndex;
                        cboQuickTemplate.SelectedIndex = cboTemplate.SelectedIndex;
                        break;

                    case ProgressType.UPDATE_PROGRESSBAR_MAX:
                        pBar.Style = ProgressBarStyle.Continuous;
                        pBar.Maximum = (int)e.UserState;
                        break;

                    case ProgressType.UPDATE_SCREENSHOTS_LIST:
                        Screenshot sp = (Screenshot)e.UserState;
                        if (sp != null && !string.IsNullOrEmpty(sp.LocalPath))
                        {
                            lbScreenshots.Items.Add(sp);
                            lbScreenshots.SelectedIndex = lbScreenshots.Items.Count - 1;
                        }
                        break;

                    case ProgressType.UPDATE_STATUSBAR_DEBUG:
                        sBar.Text = msg;
                        lbStatus.Items.Add(msg);
                        FileSystem.AppendDebug(msg);
                        break;
                }
            }
        }

        private void chkScreenshotUpload_CheckedChanged(object sender, EventArgs e)
        {
            chkUploadFullScreenshot.Enabled = chkScreenshotUpload.Checked;
            Program.Settings.ScreenshotsUpload = chkScreenshotUpload.Checked;
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            string[] files = new string[lbFiles.Items.Count];
            for (int i = 0; i < lbFiles.Items.Count; i++)
            {
                files[i] = lbFiles.Items[i].ToString();
            }
            WorkerTask wt = new WorkerTask(bwApp, TaskType.ANALYZE_MEDIA);
            wt.FileOrDirPaths = new List<string>(files);
            this.AnalyzeMedia(wt);
        }

        private void ShowAboutWindow()
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void cmsAppAbout_Click(object sender, EventArgs e)
        {
            ShowAboutWindow();
        }

        private void FillTrackersComboBox()
        {
            try
            {
                cboTrackerGroupActive.Items.Clear();
                foreach (TrackerGroup tg in lbTrackerGroups.Items)
                {
                    cboTrackerGroupActive.Items.Add(tg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("fillTrackersComboBox() fails...");
                Console.WriteLine(ex.ToString());
            }
        }

        private TrackerGroup GetTracker()
        {
            TrackerGroup t = null;

            if (Program.Settings.TrackerGroupActive < 0)
                Program.Settings.TrackerGroupActive = 0;

            if (cboTrackerGroupActive.Items.Count > Program.Settings.TrackerGroupActive)
            {
                t = cboTrackerGroupActive.Items[Program.Settings.TrackerGroupActive] as TrackerGroup;
            }
            return t;
        }

        private void CreateTorrentButton()
        {
            if (!bwApp.IsBusy)
            {
                List<TorrentCreateInfo> tps = new List<TorrentCreateInfo>();
                List<TorrentInfo> tiList = new List<TorrentInfo>();
                foreach (TorrentInfo ti in lbPublish.SelectedItems)
                {
                    tps.Add(new TorrentCreateInfo(GetTracker(), ti.Media.Location));
                    tiList.Add(ti);
                }
                if (tps.Count > 0)
                {
                    WorkerTask wt = new WorkerTask(bwApp, TaskType.CREATE_TORRENT);
                    wt.MediaList = tiList;
                    wt.TorrentPackets = tps;
                    bwApp.RunWorkerAsync(wt);

                    btnCreateTorrent.Enabled = false;
                }
            }
        }

        private void btnBrowseTorrentCustomFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtTorrentCustomFolder.Text = dlg.SelectedPath;
                Program.Settings.CustomTorrentsDir = txtTorrentCustomFolder.Text;
            }
        }

        private void rbTorrentFolderCustom_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGuiControls();
            if (string.IsNullOrEmpty(txtTorrentCustomFolder.Text))
            {
                txtTorrentCustomFolder.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Torrent Uploads");
            }
        }

        private void cboAnnounceURL_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.TrackerGroupActive = cboTrackerGroupActive.SelectedIndex;
        }

        private TorrentInfo GetTorrentInfo()
        {
            TorrentInfo ti = null;
            if (lbPublish.SelectedIndex > -1)
            {
                ti = lbPublish.Items[lbPublish.SelectedIndex] as TorrentInfo;
            }
            return ti;
        }

        private void CreatePublishUser()
        {
            if (!bwApp.IsBusy)
            {
                TorrentInfo ti = GetTorrentInfo();
                if (ti != null)
                {
                    PublishOptionsPacket pop = new PublishOptionsPacket();
                    pop.AlignCenter = chkQuickAlignCenter.Checked;
                    pop.FullPicture = chkQuickFullPicture.Checked;
                    pop.PreformattedText = chkQuickPre.Checked;

                    pop.PublishInfoTypeChoice = (PublishInfoType)cboQuickPublishType.SelectedIndex;
                    pop.TemplateLocation = Path.Combine(Program.TemplatesDir, cboQuickTemplate.Text);

                    txtPublish.Text = Adapter.CreatePublish(ti, pop);

                    if (ti.Media.MediaTypeChoice == MediaType.MusicAudioAlbum)
                    {
                        txtPublish.BackColor = System.Drawing.Color.Black;
                        txtPublish.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        txtPublish.BackColor = System.Drawing.SystemColors.Window;
                        txtPublish.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                }
                Console.WriteLine("CreatePublish called from " + new StackFrame(1).GetMethod().Name);
            }
        }

        private void chkQuickPre_CheckedChanged(object sender, EventArgs e)
        {
            CreatePublishUser();
        }

        private void chkQuickAlignCenter_CheckedChanged(object sender, EventArgs e)
        {
            CreatePublishUser();
        }

        private void chkQuickFullPicture_CheckedChanged(object sender, EventArgs e)
        {
            CreatePublishUser();
        }

        private void txtPublish_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(1))
            {
                TextBox tb = (TextBox)sender;
                tb.SelectAll();
                e.Handled = true;
            }
        }

        private void chkTemplatesMode_CheckedChanged(object sender, EventArgs e)
        {
            chkTemplatesMode.CheckState = CheckState.Indeterminate;
        }

        private void btnMTNHelp_Click(object sender, EventArgs e)
        {
            Process.Start("http://moviethumbnail.sourceforge.net/usage.en.html");
        }

        private void tsmTorrentsDir_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirTorrents();
        }

        private void tsmScreenshots_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirScreenshots();
        }

        private void tsmTemplates_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirTemplates();
        }

        private void updateThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 1:
                    string info = e.UserState as string;
                    if (!string.IsNullOrEmpty(info))
                    {
                        string[] updates = Regex.Split(info, "\r\n");
                        lbStatus.Items.AddRange(updates);
                    }
                    break;
            }
        }

        private void cboTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.TemplateIndex = cboTemplate.SelectedIndex;
        }

        private void cboScreenshotDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.ImageUploaderType = (ImageDestination)cboScreenshotDest.SelectedIndex;
        }

        private void tsmLogsDir_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirLogs();
        }

        private void btnTemplatesRewrite_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will rewrite old copies of TDMaker created Templates. Your own templates will not be affected. \n\nAre you sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Program.WriteTemplates(true);
            }
        }

        private void btnImageShackRegCode_Click(object sender, EventArgs e)
        {
            Process.Start("http://my.imageshack.us/registration/");
        }

        private void btnImageShackImages_Click(object sender, EventArgs e)
        {
            Process.Start("http://my.imageshack.us/v_images.php");
        }

        private void OpenVersionHistory()
        {
            string h = Program.GetText("VersionHistory.txt");

            if (h != string.Empty)
            {
                ZSS.frmTextViewer v = new ZSS.frmTextViewer(string.Format("{0} - {1}",
                    Application.ProductName, "Version History"), h);
                v.Icon = this.Icon;
                v.ShowDialog();
            }
        }

        private void tmsVersionHistory_Click(object sender, EventArgs e)
        {
            OpenVersionHistory();
        }

        private void cboQuickTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreatePublishUser();
        }

        private string GetHexColor()
        {
            string hexColor = "";
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                hexColor = string.Format("0x{0:X8}", cd.Color.ToArgb());
                hexColor = hexColor.Substring(hexColor.Length - 6, 6);
            }
            return hexColor;
        }

        private void SetComboBoxTextColor(ref ComboBox cbo)
        {
            string hexColor = "";
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            cd.AnyColor = true;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                hexColor = string.Format("0x{0:X8}", cd.Color.ToArgb());
                hexColor = hexColor.Substring(hexColor.Length - 6, 6);
                cbo.Text = hexColor;
                cbo.BackColor = cd.Color;
            }
        }

        private void btnRefreshTrackers_Click(object sender, EventArgs e)
        {
            try
            {
                int old = cboTrackerGroupActive.SelectedIndex;
                FillTrackersComboBox();
                if (cboTrackerGroupActive.Items.Count > 0)
                {
                    if (old < 0) old = 0;
                    cboTrackerGroupActive.SelectedIndex = Math.Min(old, cboTrackerGroupActive.Items.Count - 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void tsmSettingsDir_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirSettings();
        }

        private void WriteMediaInfo(string info)
        {
            if (GetTorrentInfo() != null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Text Files (*.txt)|*.txt";
                dlg.FileName = GetTorrentInfo().Media.Title;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(dlg.FileName))
                    {
                        sw.WriteLine(info);
                    }
                }
            }
        }

        private void miFileSaveInfoAs_Click(object sender, EventArgs e)
        {
            string info = "";
            if (tcMain.SelectedTab == tpMediaInfo)
            {
                info = txtMediaInfo.Text;
            }
            else
            {
                info = txtPublish.Text;
            }
            WriteMediaInfo(info);
        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpMediaInfo)
            {
                miFileSaveInfoAs.Text = "&Save Media Info As...";
            }
            else
            {
                miFileSaveInfoAs.Text = "&Save Publish Info As...";
            }
        }

        private void miFileSaveTorrent_Click(object sender, EventArgs e)
        {
            CreateTorrentButton();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            ShowAboutWindow();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void miFileOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void miFileOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPublish();
        }

        private void miFoldersScreenshots_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirScreenshots();
        }

        private void miFoldersTorrents_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirTorrents();
        }

        private void miFoldersLogs_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirLogs();
        }

        private void miFoldersLogsDebug_Click(object sender, EventArgs e)
        {
            FileSystem.OpenFileDebug();
        }

        private void miFoldersSettings_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirSettings();
        }

        private void miFoldersTemplates_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirTemplates();
        }

        private void miHelpVersionHistory_Click(object sender, EventArgs e)
        {
            OpenVersionHistory();
        }

        private void chkCreateTorrent_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.TorrentCreateAuto = chkCreateTorrent.Checked;
        }

        private void pgApp_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SettingsRead();
        }

        private void cboAuthoring_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.AuthoringMode = cboAuthoring.Text;
        }

        private void chkSourceEdit_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.bAuthoring = chkAuthoring.Checked;
        }

        private void cboExtras_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.Extra = cboExtras.Text;
        }

        private void chkExtras_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.bExtras = chkExtras.Checked;
        }

        private void cboDiscMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.DiscMenu = cboDiscMenu.Text;
        }

        private void chkDiscMenu_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.bDiscMenu = chkDiscMenu.Checked;
        }

        private void chkSource_CheckedChanged(object sender, EventArgs e)
        {
            chkSource.CheckState = CheckState.Indeterminate;
        }

        private void chkTitle_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.bTitle = chkTitle.Checked;
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            // we dont save this
        }

        private void chkWebLink_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.bWebLink = chkWebLink.Checked;
        }

        private void txtWebLink_TextChanged(object sender, EventArgs e)
        {
            // we dont save this
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // this.WindowState = FormWindowState.Minimized;
            SettingsWrite();
            pbScreenshot.ImageLocation = null; // need this to successfully clear screenshots
            Program.ClearScreenshots();
        }

        private void chkUploadFullScreenshot_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.UseFullPicture = chkUploadFullScreenshot.Checked;
        }

        private void chkAlignCenter_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.AlignCenter = chkAlignCenter.Checked;
        }

        private void chkPre_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.PreText = chkPre.Checked;
        }

        private void chkPreIncreaseFontSize_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.LargerPreText = chkPreIncreaseFontSize.Checked;
        }

        private void nudFontSizeIncr_ValueChanged(object sender, EventArgs e)
        {
            Program.Settings.FontSizeIncr = (int)nudFontSizeIncr.Value;
        }

        private void nudFontSizeHeading1_ValueChanged(object sender, EventArgs e)
        {
            Program.Settings.FontSizeHeading1 = (int)nudHeading1Size.Value;
        }

        private void nudHeading2Size_ValueChanged(object sender, EventArgs e)
        {
            Program.Settings.FontSizeHeading2 = (int)nudHeading2Size.Value;
        }

        private void nudHeading3Size_ValueChanged(object sender, EventArgs e)
        {
            Program.Settings.FontSizeHeading3 = (int)nudHeading3Size.Value;
        }

        private void nudBodyText_ValueChanged(object sender, EventArgs e)
        {
            Program.Settings.FontSizeBody = (int)nudBodySize.Value;
        }

        private void lbTrackers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTrackers.SelectedIndex != -1)
            {
                Tracker t = lbTrackers.SelectedItem as Tracker;
                if (t != null)
                {
                    pgTracker.SelectedObject = t;
                }
                pgTracker.Enabled = t != null;
            }
        }

        private void lbTrackerGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbTrackers.Items.Clear();
            TrackerGroup tg = lbTrackerGroups.SelectedItem as TrackerGroup;
            if (tg != null)
            {
                foreach (Tracker myTracker in tg.Trackers)
                {
                    lbTrackers.Items.Add(myTracker);
                }
                lbTrackers.SelectedIndex = 0;
            }
        }

        private void btnAddTrackerGroup_Click(object sender, EventArgs e)
        {
            InputBox ib = new InputBox();
            ib.Text = "Enter group name";
            ib.InputText = "Linux ISOs";
            if (ib.ShowDialog() == DialogResult.OK)
            {
                TrackerGroup tg = new TrackerGroup(ib.InputText);
                Tracker t = new Tracker("Ubuntu", "http://torrent.ubuntu.com:6969");
                tg.Trackers.Add(t);

                Program.Settings.TrackerGroups.Add(tg);
                lbTrackerGroups.Items.Add(tg);
                lbTrackerGroups.SelectedIndex = lbTrackerGroups.Items.Count - 1;

                btnRefreshTrackers_Click(sender, e);
            }
        }

        private void btnRemoveTrackerGroup_Click(object sender, EventArgs e)
        {
            if (lbTrackerGroups.SelectedIndex > -1)
            {
                int sel = lbTrackerGroups.SelectedIndex;
                lbTrackerGroups.Items.RemoveAt(sel);
                Program.Settings.TrackerGroups.RemoveAt(sel);
                lbTrackers.Items.Clear();
                pgTracker.Enabled = false;
            }
        }

        private void btnRemoveTracker_Click(object sender, EventArgs e)
        {
            if (lbTrackers.SelectedIndex > -1 && lbTrackerGroups.SelectedIndex > -1)
            {
                int sel = lbTrackers.SelectedIndex;
                lbTrackers.Items.RemoveAt(sel);
                Program.Settings.TrackerGroups[lbTrackerGroups.SelectedIndex].Trackers.RemoveAt(sel);
            }
        }

        private void chkTorrentOrganize_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.TorrentsOrganize = chkTorrentOrganize.Checked;
        }

        private void chkWritePublish_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.WritePublish = chkWritePublish.Checked;
        }

        private void txtTorrentCustomFolder_TextChanged(object sender, EventArgs e)
        {
        }

        private void BtnAddTrackerClick(object sender, EventArgs e)
        {
            if (lbTrackerGroups.SelectedIndex > -1)
            {
                TrackerGroup tg = lbTrackerGroups.Items[lbTrackerGroups.SelectedIndex] as TrackerGroup;
                if (tg != null)
                {
                    Tracker t = new Tracker("Ubuntu", "http://torrent.ubuntu.com:6969");
                    tg.Trackers.Add(t);
                    lbTrackers.Items.Add(t);
                    lbTrackers.SelectedIndex = lbTrackers.Items.Count - 1;
                }
            }
        }

        private void PgTrackerPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (lbTrackers.SelectedIndex > -1 && lbTrackerGroups.SelectedIndex > -1)
            {
                int sel = lbTrackers.SelectedIndex;
                lbTrackers.Items[sel] = (Tracker)pgTracker.SelectedObject;
                Program.Settings.TrackerGroups[lbTrackerGroups.SelectedIndex].Trackers[lbTrackers.SelectedIndex] = (Tracker)pgTracker.SelectedObject;
            }
        }

        private void lbTrackerGroups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int sel = lbTrackerGroups.IndexFromPoint(e.X, e.Y);
            TrackerGroup tg = lbTrackerGroups.Items[sel] as TrackerGroup;
            if (tg != null)
            {
                InputBox ib = new InputBox();
                ib.InputText = tg.Name;
                ib.Text = "Enter new name...";
                if (ib.ShowDialog() == DialogResult.OK)
                {
                    tg.Name = ib.InputText;
                    lbTrackerGroups.Items[sel] = tg;
                    Program.Settings.TrackerGroups[sel] = tg;
                }
            }
        }

        private void PgMtnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            txtMtnArgs.Text = Adapter.GetMtnArg(Program.mtnProfileMgr.GetMtnProfileActive());
            if (lbMtnProfiles.SelectedIndex > -1)
            {
                lbMtnProfiles.Items[lbMtnProfiles.SelectedIndex] = Program.mtnProfileMgr.GetMtnProfileActive();
            }
        }

        private void TbnAddMtnProfileClick(object sender, EventArgs e)
        {
            InputBox ib = new InputBox("Enter profile name...", "Default");
            if (ib.ShowDialog() == DialogResult.OK)
            {
                XMLSettingsScreenshot mtnProfile = new XMLSettingsScreenshot(ib.InputText);
                Program.mtnProfileMgr.MtnProfiles.Add(mtnProfile);
                lbMtnProfiles.Items.Add(mtnProfile);
                lbMtnProfiles.SelectedIndex = lbMtnProfiles.Items.Count - 1;
            }
        }

        private void LbMtnProfilesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbMtnProfiles.SelectedIndex > -1)
            {
                XMLSettingsScreenshot mtnProfile = lbMtnProfiles.Items[lbMtnProfiles.SelectedIndex] as XMLSettingsScreenshot;
                pgMtn.SelectedObject = mtnProfile;
                Program.mtnProfileMgr.MtnProfileActive = lbMtnProfiles.SelectedIndex;
                txtMtnArgs.Text = Adapter.GetMtnArg(mtnProfile);
            }
        }

        private void BtnRemoveMtnProfileClick(object sender, EventArgs e)
        {
            int sel = lbMtnProfiles.SelectedIndex;
            if (sel >= 0)
            {
                lbMtnProfiles.Items.RemoveAt(sel);
                Program.mtnProfileMgr.MtnProfiles.RemoveAt(sel);
                sel = sel - 1;
                if (sel < 0)
                {
                    sel = 0;
                }
                if (lbMtnProfiles.Items.Count > 0)
                {
                    lbMtnProfiles.SelectedIndex = sel;
                }
            }
        }

        private void ChkProxyEnableCheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.ProxyEnabled = chkProxyEnable.Checked;
        }

        private void lbScreenshots_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = lbScreenshots.SelectedIndex;
            if (sel > -1)
            {
                Screenshot ss = lbScreenshots.Items[sel] as Screenshot;
                if (ss != null)
                {
                    pbScreenshot.WaitOnLoad = true;
                    pbScreenshot.ImageLocation = ss.LocalPath;
                    pgScreenshot.SelectedObject = ss;
                }
            }
        }

        private void pbScreenshot_MouseDown(object sender, MouseEventArgs e)
        {
            if (File.Exists(pbScreenshot.ImageLocation))
            {
                Process.Start(pbScreenshot.ImageLocation);
            }
        }

        private void LbMediaInfoSelectedIndexChanged(object sender, EventArgs e)
        {
            OnLbMediaInfoSelectedIndexChanged();
        }

        private void OnLbMediaInfoSelectedIndexChanged()
        {
            if (lbMediaInfo.SelectedIndex > -1)
            {
                if (!chkMediaInfoComplete.Checked)
                    txtMediaInfo.Text = (lbMediaInfo.Items[lbMediaInfo.SelectedIndex] as MediaFile).Summary;
                else
                    txtMediaInfo.Text = (lbMediaInfo.Items[lbMediaInfo.SelectedIndex] as MediaFile).SummaryComplete;
            }
        }

        private void LbPublishSelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPublish.SelectedIndex > -1)
            {
                CreatePublishUser();
            }
        }

        private void cboMediaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGuiControls();
        }

        private void cboTorrentLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.TorrentLocationChoice = (LocationType)cboTorrentLoc.SelectedIndex;
            UpdateGuiControls();
        }

        private void cboScreenshotsLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.ScreenshotsLoc = (LocationType)cboScreenshotsLoc.SelectedIndex;
            UpdateGuiControls();
        }

        private void btnScreenshotsLocBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtScreenshotsLoc.Text = dlg.SelectedPath;
                Program.Settings.CustomScreenshotsDir = txtScreenshotsLoc.Text;
            }
        }

        private void btnBrowseDir_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            // we do nothing
        }

        private void cboPublishType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreatePublishUser();
            cboQuickTemplate.Enabled = (PublishInfoType)cboQuickPublishType.SelectedIndex == PublishInfoType.ExternalTemplate;
        }

        private void cboPublishType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Program.Settings.PublishInfoTypeChoice = (PublishInfoType)cboPublishType.SelectedIndex;
            cboTemplate.Enabled = Program.Settings.PublishInfoTypeChoice == PublishInfoType.ExternalTemplate;
            gbTemplatesInternal.Enabled = Program.Settings.PublishInfoTypeChoice == PublishInfoType.InternalTemplate;
            gbFonts.Enabled = Program.Settings.PublishInfoTypeChoice == PublishInfoType.InternalTemplate;
        }

        private void tsmiPreferKnownFolders_Click(object sender, EventArgs e)
        {
            ConfigWizard cw = new ConfigWizard(Program.RootAppFolder);
            if (cw.ShowDialog() == DialogResult.OK)
            {
                tsmiPreferKnownFolders.Checked = cw.PreferSystemFolders;
                Program.InitializeDefaultFolderPaths();
            }
        }

        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                List<string> temp = new List<string>();
                foreach (string fd in lbFiles.SelectedItems)
                {
                    temp.Add(fd);
                }
                foreach (string fd in temp)
                {
                    lbFiles.Items.Remove(fd);
                }
            }
        }

        private void btnUploadersConfig_Click(object sender, EventArgs e)
        {
            UploadersConfigForm form = new UploadersConfigForm(Program.UploadersConfig);
            form.Show();
        }

        private void cboThumbnailer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Settings.ThumbnailerType = (ThumbnailerType)cboThumbnailer.SelectedIndex;
        }

        private void chkMediaInfoComplete_CheckedChanged(object sender, EventArgs e)
        {
            OnLbMediaInfoSelectedIndexChanged();
        }
    }
}