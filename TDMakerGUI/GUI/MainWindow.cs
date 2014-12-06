using BDInfo;
using HelpersLib;
using ScreenCaptureLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
                logo1 = Path.Combine(App.SettingsDir, "logo1.png");
            }

            string logo2 = Path.Combine(Application.StartupPath, "logo.png");
            if (!File.Exists(logo2))
            {
                logo2 = Path.Combine(App.SettingsDir, "logo.png");
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
                pbLogo.BackColor = SystemColors.ControlDark;
                BackColor = SystemColors.ControlDark;
            }

            sBar.Text = string.Format("Ready.");

            tttvMain.MainTabControl = tcMain;

            Text = App.GetProductName();
            Icon = Resources.GenuineAdvIcon;

            UpdateGuiControls();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            rtbDebugLog.Text = DebugHelper.Logger.ToString();
            DebugHelper.Logger.MessageAdded += Logger_MessageAdded;

            string mtnExe = (App.IsUNIX ? "mtn" : "mtn.exe");

            if (App.Settings.ThumbnailerType == ThumbnailerType.MPlayer)
            {
                if (!File.Exists(App.Settings.MPlayerPath))
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    string mplayer = "http://mplayerwin.sourceforge.net/downloads.html";
                    dlg.Title = "Browse for mplayer.exe or download from " + mplayer;
                    dlg.Filter = "Applications (mplayer.exe)|mplayer.exe";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        App.Settings.MPlayerPath = dlg.FileName;
                    }
                    else
                    {
                        URLHelpers.OpenURL(mplayer);
                    }
                }
            }

            if (ProgramUI.ExplorerFilePaths.Count > 0)
            {
                LoadMedia(ProgramUI.ExplorerFilePaths.ToArray());
            }
        }

        private void Logger_MessageAdded(string message)
        {
            if (!rtbDebugLog.IsDisposed)
            {
                MethodInvoker method = delegate
                {
                    rtbDebugLog.AppendText(message + Environment.NewLine);
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

        private void OpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Title = "Browse for Media file...";
            StringBuilder sbExt = new StringBuilder();
            sbExt.Append("Media Files (");
            StringBuilder sbExtDesc = new StringBuilder();
            foreach (string ext in App.Settings.SupportedFileExtVideo)
            {
                sbExtDesc.Append("*");
                sbExtDesc.Append(ext);
                sbExtDesc.Append("; ");
            }
            sbExt.Append(sbExtDesc.ToString().TrimEnd().TrimEnd(';'));
            sbExt.Append(")|");
            foreach (string ext in App.Settings.SupportedFileExtVideo)
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
                GuessSource(txtTitle.Text);
            }

            if (!App.Settings.WritePublish && ps.Length > 1)
            {
                if (MessageBox.Show("Writing Publish info to File is recommended when analysing multiple files or folders. \n\nWould you like to turn this feature on?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    App.Settings.WritePublish = true;
                }
            }

            List<TorrentCreateInfo> tps = new List<TorrentCreateInfo>();

            foreach (string p in ps)
            {
                if (File.Exists(p) || Directory.Exists(p))
                {
                    // txtMediaLocation.Text = p;
                    DebugHelper.WriteLine(string.Format("Queued {0} to create a torrent", p));
                    lbFiles.Items.Add(p);
                    TorrentCreateInfo tp = new TorrentCreateInfo(GetTracker(), p);
                    tps.Add(tp);

                    UpdateGuiControls();
                }
            }

            if (App.Settings.AnalyzeAuto)
            {
                WorkerTask wt = new WorkerTask(bwApp, TaskType.ANALYZE_MEDIA);
                wt.FileOrDirPaths = new List<string>(ps);
                AnalyzeMedia(wt);
            }
        }

        private void GuessSource(string source)
        {
            if (source.Contains("DVD"))
                cboSource.Text = "DVD";
            else if (source.Contains("HDTV"))
                cboSource.Text = "HDTV";
            else if (source.Contains("Blu"))
                cboSource.Text = "Blu-ray";
            else if (source.Contains("TV"))
                cboSource.Text = "TV";
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
                wt.CreateScreenshots = mwo.CreateScreenshots;
                wt.UploadScreenshots = mwo.UploadScreenshots;
                dlgResult = mwo.DialogResultMy;
            }
            else
            {
                // fill previous settings
                wt.TorrentCreateAuto = App.Settings.TorrentCreateAuto;
                wt.CreateScreenshots = App.Settings.CreateScreenshots;
                wt.UploadScreenshots = App.Settings.UploadScreenshots;
            }

            if (!mwo.PromptShown && App.Settings.ShowMediaWizardAlways)
            {
                MediaWizard mw = new MediaWizard(wt);
                dlgResult = mw.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    wt.TorrentCreateAuto = mw.Options.CreateTorrent;
                    wt.CreateScreenshots = App.Settings.CreateScreenshots;
                    wt.UploadScreenshots = mw.Options.UploadScreenshots;
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
                                this.Text = string.Format("{0} - {1}", App.GetProductName(), MediaHelper.GetMediaName(mi.Location));
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
            if (string.IsNullOrEmpty(cboSource.Text) && App.Settings.PublishInfoTypeChoice != PublishInfoType.MediaInfo)
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

            if (App.Settings.PublishInfoTypeChoice == PublishInfoType.ExternalTemplate)
            {
                mi.TemplateLocation = Path.Combine(App.TemplatesDir, cboTemplate.Text);
            }

            return mi;
        }

        private void btnCreateTorrent_Click(object sender, EventArgs e)
        {
            CreateTorrentButton();
        }

        private void SettingsWrite()
        {
            App.Settings.TrackerGroupActive = cboTrackerGroupActive.SelectedIndex;
            App.Settings.TemplateIndex = cboTemplate.SelectedIndex;

            App.Settings.TorrentLocationChoice = (LocationType)cboTorrentLoc.SelectedIndex;

            App.Settings.ImageUploaderType = (ImageDestination)cboImageUploader.SelectedIndex;
            App.Settings.ImageFileUploaderType = (FileDestination)cboFileUploader.SelectedIndex;

            App.Settings.Save(App.Config.SettingsFilePath);
            App.UploadersConfig.Save(App.UploadersConfigPath);
        }

        private void ConfigureDirs()
        {
            App.WriteTemplates(false);

            // Read Templates to GUI
            if (Directory.Exists(App.TemplatesDir))
            {
                string[] dirs = Directory.GetDirectories(App.TemplatesDir);
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
                cboTemplate.SelectedIndex = Math.Max(App.Settings.TemplateIndex, 0);
                cboQuickTemplate.SelectedIndex = Math.Max(App.Settings.TemplateIndex, 0);
            }

            mTrackerManager = new TrackerManager();
        }

        private void SettingsRead()
        {
            tsmiPreferKnownFolders.Checked = App.Config.PreferSystemFolders;

            SettingsReadInput();
            SettingsReadMedia();
            SettingsReadPublish();
            SettingsReadScreenshots();
            SettingsReadOptions();

            cboImageUploader.Items.Clear();
            cboImageUploader.Items.AddRange(Helpers.GetLocalizedEnumDescriptions<ImageDestination>());
            cboImageUploader.SelectedIndex = (int)App.Settings.ImageUploaderType;

            cboFileUploader.Items.Clear();
            cboFileUploader.Items.AddRange(Helpers.GetLocalizedEnumDescriptions<FileDestination>());
            cboFileUploader.SelectedIndex = (int)App.Settings.ImageFileUploaderType;

            if (string.IsNullOrEmpty(App.Settings.CustomMediaInfoDllDir))
            {
                App.Settings.CustomMediaInfoDllDir = Application.StartupPath;
            }
            Kernel32Helper.SetDllDirectory(App.Settings.CustomMediaInfoDllDir);

            pgApp.SelectedObject = App.Settings;
        }

        private void SettingsReadInput()
        {
            if (App.Settings.MediaSources.Count == 0)
            {
                App.Settings.MediaSources.AddRange(new string[] { "CAM", "TC", "TS", "R5", "DVD-Screener",
                                                            "DVD", "TV", "HDTV", "Blu-ray", "HD-DVD",
                                                            "Laser Disc", "VHS", "Unknown" });
            }
            if (App.Settings.Extras.Count == 0)
            {
                App.Settings.Extras.AddRange(new string[] { "Intact", "Shrunk", "Removed", "None on Source" });
            }
            if (App.Settings.AuthoringModes.Count == 0)
            {
                App.Settings.AuthoringModes.AddRange(new string[] { "Untouched", "Shrunk" });
            }
            if (App.Settings.DiscMenus.Count == 0)
            {
                App.Settings.DiscMenus.AddRange(new string[] { "Intact", "Removed", "Shrunk" });
            }
            if (App.Settings.SupportedFileExtVideo.Count == 0)
            {
                App.Settings.SupportedFileExtVideo.AddRange(new string[] { ".3g2", ".3gp", ".3gp2", ".3gpp", ".amr", ".asf", ".asx", ".avi", ".d2v", ".dat", ".divx", ".drc", ".dsa", ".dsm", ".dss", ".dsv", ".flc", ".fli", ".flic", ".flv", ".hdmov", ".ivf", ".m1v", ".m2ts", ".m2v", ".m4v", ".mkv", ".mov", ".mp2v", ".mp4", ".mpcpl", ".mpe", ".mpeg", ".mpg", ".mpv", ".mpv2", ".ogm", ".qt", ".ram", ".ratdvd", ".rm", ".rmvb", ".roq", ".rp", ".rpm", ".rt", ".swf", ".ts", ".vob", ".vp6", ".wm", ".wmp", ".wmv", ".wmx", ".wvx" });
            }
            if (App.Settings.SupportedFileExtAudio.Count == 0)
            {
                App.Settings.SupportedFileExtAudio.AddRange(new string[] { ".aac", ".aiff", ".ape", ".flac", ".m4a", ".mp3", ".mpc", ".ogg", ".mp4", ".wma" });
            }

            cboSource.Items.Clear();
            foreach (string src in App.Settings.MediaSources)
            {
                cboSource.Items.Add(src);
            }

            cboAuthoring.Items.Clear();
            foreach (string ed in App.Settings.AuthoringModes)
            {
                cboAuthoring.Items.Add(ed);
            }

            cboDiscMenu.Items.Clear();
            foreach (string ex in App.Settings.DiscMenus)
            {
                cboDiscMenu.Items.Add(ex);
            }

            cboExtras.Items.Clear();
            foreach (string ex in App.Settings.Extras)
            {
                cboExtras.Items.Add(ex);
            }
        }

        private void SettingsReadMedia()
        {
            chkAuthoring.Checked = App.Settings.bAuthoring;
            cboAuthoring.Text = App.Settings.AuthoringMode;

            chkDiscMenu.Checked = App.Settings.bDiscMenu;
            cboDiscMenu.Text = App.Settings.DiscMenu;

            chkExtras.Checked = App.Settings.bExtras;
            cboExtras.Text = App.Settings.Extra;

            chkTitle.Checked = App.Settings.bTitle;
            chkWebLink.Checked = App.Settings.bWebLink;
        }

        private void SettingsReadScreenshots()
        {
            chkScreenshotUpload.Checked = App.Settings.CreateScreenshots;

            btnUploadersConfig.Visible = cboFileUploader.Visible = cboImageUploader.Visible = string.IsNullOrEmpty(App.Settings.PtpImgCode);

            if (!string.IsNullOrEmpty(App.Settings.PtpImgCode))
                chkScreenshotUpload.Text = "Upload screenshots to ptpimg.me";
        }

        private void SettingsReadPublish()
        {
            if (cboQuickPublishType.Items.Count == 0)
            {
                cboQuickPublishType.Items.AddRange(Enum.GetNames(typeof(PublishInfoType)));
                cboPublishType.Items.AddRange(Enum.GetNames(typeof(PublishInfoType)));
            }
            cboPublishType.SelectedIndex = (int)App.Settings.PublishInfoTypeChoice;
            cboQuickPublishType.SelectedIndex = (int)App.Settings.PublishInfoTypeChoice;
        }

        private void SettingsReadOptions()
        {
            cboTemplate.SelectedIndex = App.Settings.TemplateIndex;
            chkUploadFullScreenshot.Checked = App.Settings.UseFullPicture;

            chkAlignCenter.Checked = App.Settings.AlignCenter;
            chkPre.Checked = App.Settings.PreText;
            chkPreIncreaseFontSize.Checked = App.Settings.LargerPreText;

            nudFontSizeIncr.Value = (decimal)App.Settings.FontSizeIncr;
            nudHeading1Size.Value = (decimal)App.Settings.FontSizeHeading1;
            nudHeading2Size.Value = (decimal)App.Settings.FontSizeHeading2;
            nudHeading3Size.Value = (decimal)App.Settings.FontSizeHeading3;
            nudBodySize.Value = (decimal)App.Settings.FontSizeBody;

            pgProxy.SelectedObject = App.Settings.ProxySettings;

            pgThumbnailerOptions.SelectedObject = App.Settings.ThumbnailerOptions;

            SettingsReadOptionsTorrents();
        }

        private void SettingsReadOptionsTorrents()
        {
            lbTrackerGroups.Items.Clear();
            foreach (TrackerGroup tg in App.Settings.TrackerGroups)
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
            if (cboTrackerGroupActive.Items.Count > 0 && App.Settings.TrackerGroupActive < cboTrackerGroupActive.Items.Count)
            {
                cboTrackerGroupActive.SelectedIndex = App.Settings.TrackerGroupActive;
            }

            if (cboTorrentLoc.Items.Count == 0)
            {
                cboTorrentLoc.Items.AddRange(Enum.GetNames(typeof(LocationType)));
            }
            cboTorrentLoc.SelectedIndex = (int)App.Settings.TorrentLocationChoice;
            chkWritePublish.Checked = App.Settings.WritePublish;
            chkTorrentOrganize.Checked = App.Settings.TorrentsOrganize;

            txtTorrentCustomFolder.Text = App.Settings.CustomTorrentsDir;
        }

        private string CreatePublishInitial(TorrentInfo ti)
        {
            PublishOptionsPacket pop = new PublishOptionsPacket();
            pop.AlignCenter = App.Settings.AlignCenter;
            pop.FullPicture = ti.Media.UploadScreenshots && App.Settings.UseFullPicture;
            pop.PreformattedText = App.Settings.PreText;
            pop.PublishInfoTypeChoice = App.Settings.PublishInfoTypeChoice;
            ti.PublishOptions = pop;

            return Adapter.CreatePublish(ti, pop);
        }

        private List<TorrentInfo> WorkerAnalyzeMedia(WorkerTask wt)
        {
            App.LoadProxySettings();

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
                mi.UploadScreenshots = wt.UploadScreenshots;
                if (wt.UploadScreenshots)
                {
                    ti.CreateUploadScreenshots();
                }
                else if (wt.CreateScreenshots)
                {
                    ti.CreateScreenshots();
                }

                ti.PublishString = CreatePublishInitial(ti);
                bwApp.ReportProgress((int)ProgressType.REPORT_TORRENTINFO, ti);

                if (App.Settings.WritePublish)
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

                if (App.Settings.XMLTorrentUploadCreate)
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
                    if (App.Settings.XMLTorrentUploadCreate)
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

            txtTorrentCustomFolder.Enabled = App.Settings.TorrentLocationChoice == LocationType.CustomFolder;
            btnBrowseTorrentCustomFolder.Enabled = App.Settings.TorrentLocationChoice == LocationType.CustomFolder;
            chkTorrentOrganize.Enabled = App.Settings.TorrentLocationChoice == LocationType.CustomFolder;

            gbTemplatesInternal.Enabled = !chkTemplatesMode.Checked;
        }

        private void bwApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<TorrentInfo> tiList = e.Result as List<TorrentInfo>;

            pBar.Style = ProgressBarStyle.Continuous;
            pBar.Value = 0;

            bool success = true; tiList.ForEach(x => success &= x.Success);

            if (success)
            {
                lbFiles.Items.Clear();
                lbPublish.SelectedIndex = lbPublish.Items.Count - 1;
                sBar.Text = "Ready.";
            }
            else
            {
                sBar.Text = "Ready. One or more tasks failed.";
            }

            UpdateGuiControls();
        }

        private void pbScreenshot_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pbScreenshot = sender as PictureBox;
            Process.Start(pbScreenshot.ImageLocation);
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            tssPerc.Text = (bwApp.IsBusy ? string.Format("{0}%", (100.0 * (double)pBar.Value / (double)pBar.Maximum).ToString("0")) : "");
            btnAnalyze.Text = "Create &Description" + (lbFiles.SelectedItems.Count > 1 ? "s" : "");
            btnCreateTorrent.Text = "Create &Torrent" + (lbPublish.SelectedItems.Count > 1 ? "s" : "");
            btnBrowse.Enabled = !bwApp.IsBusy;
            btnBrowseDir.Enabled = !bwApp.IsBusy;
            btnAnalyze.Enabled = !bwApp.IsBusy && lbFiles.Items.Count > 0;
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

                    case ProgressType.UPDATE_PROGRESSBAR_CUMULATIVE:
                        pBar.Style = ProgressBarStyle.Continuous;
                        pBar.Maximum = 100;
                        pBar.Value = Convert.ToInt16(e.UserState);
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
                        chkQuickFullPicture.Checked = App.Settings.UseFullPicture;
                        chkQuickAlignCenter.Checked = App.Settings.AlignCenter;
                        chkQuickPre.Checked = App.Settings.PreText;
                        cboQuickPublishType.SelectedIndex = cboPublishType.SelectedIndex;
                        cboQuickTemplate.SelectedIndex = cboTemplate.SelectedIndex;
                        break;

                    case ProgressType.UPDATE_PROGRESSBAR_MAX:
                        pBar.Style = ProgressBarStyle.Continuous;
                        pBar.Maximum = (int)e.UserState;
                        break;

                    case ProgressType.UPDATE_SCREENSHOTS_LIST:
                        ScreenshotInfo sp = (ScreenshotInfo)e.UserState;
                        if (sp != null && !string.IsNullOrEmpty(sp.LocalPath))
                        {
                            lbScreenshots.Items.Add(sp);
                            lbScreenshots.SelectedIndex = lbScreenshots.Items.Count - 1;
                        }
                        break;

                    case ProgressType.UPDATE_STATUSBAR_DEBUG:
                        sBar.Text = msg;
                        DebugHelper.WriteLine(msg);
                        break;
                }
            }
        }

        private void chkScreenshotUpload_CheckedChanged(object sender, EventArgs e)
        {
            chkUploadFullScreenshot.Enabled = chkScreenshotUpload.Checked;
            App.Settings.CreateScreenshots = chkScreenshotUpload.Checked;
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

            if (App.Settings.TrackerGroupActive < 0)
                App.Settings.TrackerGroupActive = 0;

            if (cboTrackerGroupActive.Items.Count > App.Settings.TrackerGroupActive)
            {
                t = cboTrackerGroupActive.Items[App.Settings.TrackerGroupActive] as TrackerGroup;
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
                App.Settings.CustomTorrentsDir = txtTorrentCustomFolder.Text;
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
            App.Settings.TrackerGroupActive = cboTrackerGroupActive.SelectedIndex;
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
                    pop.TemplateLocation = Path.Combine(App.TemplatesDir, cboQuickTemplate.Text);

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

        private void cboTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.TemplateIndex = cboTemplate.SelectedIndex;
        }

        private void cboScreenshotDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.ImageUploaderType = (ImageDestination)cboImageUploader.SelectedIndex;
            cboFileUploader.Enabled = App.Settings.ImageUploaderType == ImageDestination.FileUploader;
        }

        private void tsmLogsDir_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirLogs();
        }

        private void btnTemplatesRewrite_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will rewrite old copies of TDMaker created Templates. Your own templates will not be affected. \n\nAre you sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                App.WriteTemplates(true);
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
            string h = App.GetText("VersionHistory.txt");

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
            App.Settings.TorrentCreateAuto = chkCreateTorrent.Checked;
        }

        private void pgApp_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            SettingsRead();
        }

        private void cboAuthoring_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.AuthoringMode = cboAuthoring.Text;
        }

        private void chkSourceEdit_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.bAuthoring = chkAuthoring.Checked;
        }

        private void cboExtras_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.Extra = cboExtras.Text;
        }

        private void chkExtras_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.bExtras = chkExtras.Checked;
        }

        private void cboDiscMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.DiscMenu = cboDiscMenu.Text;
        }

        private void chkDiscMenu_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.bDiscMenu = chkDiscMenu.Checked;
        }

        private void chkSource_CheckedChanged(object sender, EventArgs e)
        {
            chkSource.CheckState = CheckState.Indeterminate;
        }

        private void chkTitle_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.bTitle = chkTitle.Checked;
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            // we dont save this
        }

        private void chkWebLink_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.bWebLink = chkWebLink.Checked;
        }

        private void txtWebLink_TextChanged(object sender, EventArgs e)
        {
            // we dont save this
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // this.WindowState = FormWindowState.Minimized;
            SettingsWrite();
            App.ClearScreenshots();
        }

        private void chkUploadFullScreenshot_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.UseFullPicture = chkUploadFullScreenshot.Checked;
        }

        private void chkAlignCenter_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.AlignCenter = chkAlignCenter.Checked;
        }

        private void chkPre_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.PreText = chkPre.Checked;
        }

        private void chkPreIncreaseFontSize_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.LargerPreText = chkPreIncreaseFontSize.Checked;
        }

        private void nudFontSizeIncr_ValueChanged(object sender, EventArgs e)
        {
            App.Settings.FontSizeIncr = (int)nudFontSizeIncr.Value;
        }

        private void nudFontSizeHeading1_ValueChanged(object sender, EventArgs e)
        {
            App.Settings.FontSizeHeading1 = (int)nudHeading1Size.Value;
        }

        private void nudHeading2Size_ValueChanged(object sender, EventArgs e)
        {
            App.Settings.FontSizeHeading2 = (int)nudHeading2Size.Value;
        }

        private void nudHeading3Size_ValueChanged(object sender, EventArgs e)
        {
            App.Settings.FontSizeHeading3 = (int)nudHeading3Size.Value;
        }

        private void nudBodyText_ValueChanged(object sender, EventArgs e)
        {
            App.Settings.FontSizeBody = (int)nudBodySize.Value;
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

                App.Settings.TrackerGroups.Add(tg);
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
                App.Settings.TrackerGroups.RemoveAt(sel);
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
                App.Settings.TrackerGroups[lbTrackerGroups.SelectedIndex].Trackers.RemoveAt(sel);
            }
        }

        private void chkTorrentOrganize_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.TorrentsOrganize = chkTorrentOrganize.Checked;
        }

        private void chkWritePublish_CheckedChanged(object sender, EventArgs e)
        {
            App.Settings.WritePublish = chkWritePublish.Checked;
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
                App.Settings.TrackerGroups[lbTrackerGroups.SelectedIndex].Trackers[lbTrackers.SelectedIndex] = (Tracker)pgTracker.SelectedObject;
            }
        }

        private void lbTrackerGroups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int sel = lbTrackerGroups.IndexFromPoint(e.X, e.Y);
            UpdateTrackerGroup(sel);
        }

        private void UpdateTrackerGroup(int sel)
        {
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
                    App.Settings.TrackerGroups[sel] = tg;
                }
            }
        }

        private void lbScreenshots_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = lbScreenshots.SelectedIndex;
            if (sel > -1)
            {
                ScreenshotInfo ss = lbScreenshots.Items[sel] as ScreenshotInfo;
                pbScreenshot.Tag = ss;
                if (ss != null && File.Exists(ss.LocalPath))
                {
                    pbScreenshot.LoadImageFromFileAsync(ss.LocalPath);
                    pgScreenshot.SelectedObject = ss;
                }
                else if (!string.IsNullOrEmpty(ss.FullImageLink))
                {
                    pbScreenshot.LoadImageFromURLAsync(ss.FullImageLink);
                }
                else
                {
                    pbScreenshot.LoadImage(new Bitmap(300, 300));
                    pgScreenshot.SelectedObject = null;
                }
            }
        }

        private void pbScreenshot_MouseDown(object sender, MouseEventArgs e)
        {
            ScreenshotInfo ss = pbScreenshot.Tag as ScreenshotInfo;
            if (File.Exists(ss.LocalPath))
            {
                Helpers.OpenFile(ss.LocalPath);
            }
            else if (!string.IsNullOrEmpty(ss.FullImageLink))
            {
                URLHelpers.OpenURL(ss.FullImageLink);
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
            App.Settings.TorrentLocationChoice = (LocationType)cboTorrentLoc.SelectedIndex;
            UpdateGuiControls();
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
            App.Settings.PublishInfoTypeChoice = (PublishInfoType)cboPublishType.SelectedIndex;
            cboTemplate.Enabled = App.Settings.PublishInfoTypeChoice == PublishInfoType.ExternalTemplate;
            gbTemplatesInternal.Enabled = App.Settings.PublishInfoTypeChoice == PublishInfoType.InternalTemplate;
            gbFonts.Enabled = App.Settings.PublishInfoTypeChoice == PublishInfoType.InternalTemplate;
        }

        private void tsmiPreferKnownFolders_Click(object sender, EventArgs e)
        {
            ConfigWizard cw = new ConfigWizard(App.RootAppFolder);
            if (cw.ShowDialog() == DialogResult.OK)
            {
                tsmiPreferKnownFolders.Checked = cw.PreferSystemFolders;
                App.InitializeDefaultFolderPaths();
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
            UploadersConfigForm form = new UploadersConfigForm(App.UploadersConfig);
            form.Show();
        }

        private void chkMediaInfoComplete_CheckedChanged(object sender, EventArgs e)
        {
            OnLbMediaInfoSelectedIndexChanged();
        }

        private void cboImageFileUploader_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.ImageFileUploaderType = (FileDestination)cboFileUploader.SelectedIndex;
        }

        private void btnUpdateGroup_Click(object sender, EventArgs e)
        {
            int sel = lbTrackerGroups.SelectedIndex;
            if (sel > -1)
                UpdateTrackerGroup(sel);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            FFmpegHelper.DownloadFFmpeg(true, DownloaderForm_InstallRequested);
        }

        private void DownloaderForm_InstallRequested(string filePath)
        {
            string extractPath = Path.Combine(App.DefaultRootAppFolder, "ffmpeg.exe");
            bool result = FFmpegHelper.ExtractFFmpeg(filePath, extractPath);

            if (result)
            {
                this.InvokeSafe(() =>
                {
                    App.Settings.FFmpegPath = extractPath;
                });

                MessageBox.Show("Successfully downloaded FFmpeg.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to download FFmpeg.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}