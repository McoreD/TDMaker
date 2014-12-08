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
using UploadersLib.HelperClasses;

namespace TDMaker
{
    public partial class MainWindow : Form
    {
        private bool IsGuiReady = false;

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
            ConfigureTemplates();

            LoadSettingsToControls();

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

            Icon = Resources.GenuineAdvIcon;

            UpdateGuiControls();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            rtbDebugLog.Text = DebugHelper.Logger.ToString();
            DebugHelper.Logger.MessageAdded += Logger_MessageAdded;

            if (App.Settings.ThumbnailerType == ThumbnailerType.FFmpeg)
            {
                if (!File.Exists(App.Settings.FFmpegPath))
                {
                    DialogResult result = MessageBox.Show("FFmpeg is not configured. \n\nWould you like to download the latest FFmpeg?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        btnDownloadFFmpeg_Click(sender, e);
                    }
                    else if (result == System.Windows.Forms.DialogResult.No)
                    {
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                        dlg.Title = "Browse for ffmpeg.exe";
                        dlg.Filter = "Applications (ffmpeg.exe)|ffmpeg.exe";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            App.Settings.FFmpegPath = dlg.FileName;
                        }
                    }
                }
            }
            else if (App.Settings.ThumbnailerType == ThumbnailerType.MPlayer)
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

            IsGuiReady = true;
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
                    TorrentCreateInfo tp = new TorrentCreateInfo(App.Settings.ProfileActive, p);
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
            else if (source.Contains("Blu") || source.Contains("BDRip"))
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

            wt.MediaOptions = mwo;
            if (mwo.PromptShown)
            {
                wt.MediaOptions = mwo;
                dlgResult = mwo.DialogResult;
            }
            else
            {
                // fill previous settings
                wt.MediaOptions.CreateTorrent = App.Settings.ProfileActive.CreateTorrent;
                wt.MediaOptions.CreateScreenshots = App.Settings.CreateScreenshots;
                wt.MediaOptions.UploadScreenshots = App.Settings.ProfileActive.UploadScreenshots;
            }

            if (!mwo.PromptShown && App.Settings.ShowMediaWizardAlways)
            {
                MediaWizard mw = new MediaWizard(wt);
                dlgResult = mw.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    wt.MediaOptions = mw.Options;
                }
            }

            if (dlgResult == DialogResult.OK)
            {
                if (wt.MediaOptions.MediaTypeChoice == MediaType.MediaCollection)
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
            if (string.IsNullOrEmpty(cboSource.Text) && App.Settings.ProfileActive.PublishInfoTypeChoice != PublishInfoType.MediaInfo)
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
            MediaType mt = wt.MediaOptions.MediaTypeChoice;
            MediaInfo2 mi = new MediaInfo2(wt.MediaOptions, p);
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
            mi.TorrentCreateInfo = new TorrentCreateInfo(App.Settings.ProfileActive, p);

            if (App.Settings.ProfileActive.PublishInfoTypeChoice == PublishInfoType.ExternalTemplate)
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
            App.Settings.Save(App.SettingsFilePath);
            App.UploadersConfig.Save(App.UploadersConfigPath);
        }

        private void ConfigureTemplates()
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
                cboTemplate.SelectedIndex = Math.Max(App.Settings.ProfileActive.ExternalTemplateIndex, 0);
                cboQuickTemplate.SelectedIndex = Math.Max(App.Settings.ProfileActive.ExternalTemplateIndex, 0);
            }
        }

        private void LoadSettingsToControls()
        {
            LoadSettingsInputControls();
            LoadSettingsInputMediaControls();

            LoadSettingsMediaInfoControls();

            LoadSettingsScreenshotControls();

            LoadSettingsPublishControls();
            LoadSettingsPublishTemplatesControls();

            pgApp.SelectedObject = App.Settings;
        }

        private static void LoadSettingsMediaInfoControls()
        {
            if (string.IsNullOrEmpty(App.Settings.CustomMediaInfoDllDir))
            {
                App.Settings.CustomMediaInfoDllDir = Application.StartupPath;
            }
            Kernel32Helper.SetDllDirectory(App.Settings.CustomMediaInfoDllDir);
        }

        private void LoadSettingsScreenshotControls()
        {
            chkUploadScreenshots.Checked = App.Settings.CreateScreenshots;
            btnUploadersConfig.Visible = cboFileUploader.Visible = cboImageUploader.Visible = string.IsNullOrEmpty(App.Settings.PtpImgCode);
            chkUploadScreenshots.Text = string.IsNullOrEmpty(App.Settings.PtpImgCode) ? "Upload screenshot to:" : "Upload screenshots to ptpimg.me";

            cboImageUploader.Items.Clear();
            cboImageUploader.Items.AddRange(Helpers.GetLocalizedEnumDescriptions<ImageDestination>());
            cboImageUploader.SelectedIndex = (int)App.Settings.ProfileActive.ImageUploaderType;

            cboFileUploader.Items.Clear();
            cboFileUploader.Items.AddRange(Helpers.GetLocalizedEnumDescriptions<FileDestination>());
            cboFileUploader.SelectedIndex = (int)App.Settings.ProfileActive.ImageFileUploaderType;

            App.Settings.Profiles.ForEach(x => listBoxProfiles.Items.Add(x));
            listBoxProfiles.SelectedIndex = App.Settings.ProfileIndex;
        }

        private void LoadSettingsInputControls()
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

        private void LoadSettingsInputMediaControls()
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

        private void LoadSettingsPublishControls()
        {
            if (cboQuickPublishType.Items.Count == 0)
            {
                cboQuickPublishType.Items.AddRange(Enum.GetNames(typeof(PublishInfoType)));
                cboPublishType.Items.AddRange(Enum.GetNames(typeof(PublishInfoType)));
            }
            cboPublishType.SelectedIndex = (int)App.Settings.ProfileActive.PublishInfoTypeChoice;
            cboQuickPublishType.SelectedIndex = (int)App.Settings.ProfileActive.PublishInfoTypeChoice;
        }

        private void LoadSettingsPublishTemplatesControls()
        {
            cboTemplate.SelectedIndex = App.Settings.ProfileActive.ExternalTemplateIndex;
            chkUploadFullScreenshot.Checked = App.Settings.UseFullPicture;

            chkAlignCenter.Checked = App.Settings.AlignCenter;
            chkPre.Checked = App.Settings.PreText;
            chkPreIncreaseFontSize.Checked = App.Settings.LargerPreText;

            nudFontSizeIncr.Value = (decimal)App.Settings.FontSizeIncr;
            nudHeading1Size.Value = (decimal)App.Settings.FontSizeHeading1;
            nudHeading2Size.Value = (decimal)App.Settings.FontSizeHeading2;
            nudHeading3Size.Value = (decimal)App.Settings.FontSizeHeading3;
            nudBodySize.Value = (decimal)App.Settings.FontSizeBody;

            // Proxy
            cbProxyMethod.Items.AddRange(Helpers.GetLocalizedEnumDescriptions<ProxyMethod>());
            cbProxyMethod.SelectedIndex = (int)App.Settings.ProxySettings.ProxyMethod;
            txtProxyUsername.Text = App.Settings.ProxySettings.Username;
            txtProxyPassword.Text = App.Settings.ProxySettings.Password;
            txtProxyHost.Text = App.Settings.ProxySettings.Host ?? string.Empty;
            nudProxyPort.Value = App.Settings.ProxySettings.Port;
            UpdateProxyControls();
        }

        private string CreatePublishInitial(TorrentInfo ti)
        {
            PublishOptionsPacket pop = new PublishOptionsPacket();
            pop.AlignCenter = App.Settings.AlignCenter;
            pop.FullPicture = ti.Media.Options.UploadScreenshots && App.Settings.UseFullPicture;
            pop.PreformattedText = App.Settings.PreText;
            pop.PublishInfoTypeChoice = App.Settings.ProfileActive.PublishInfoTypeChoice;
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
                if (wt.MediaOptions.UploadScreenshots)
                {
                    ti.CreateScreenshots();
                    ti.UploadScreenshots();
                }
                else if (wt.MediaOptions.CreateScreenshots)
                {
                    ti.CreateScreenshots();
                }

                ti.PublishString = CreatePublishInitial(ti);
                bwApp.ReportProgress((int)ProgressType.REPORT_TORRENTINFO, ti);

                if (App.Settings.WritePublish)
                {
                    // create textFiles of MediaInfo
                    string txtPath = Path.Combine(mi.TorrentCreateInfo.TorrentFolder, mi.Overall.FileName) + ".txt";

                    Helpers.CreateDirectoryIfNotExist(mi.TorrentCreateInfo.TorrentFolder);

                    using (StreamWriter sw = new StreamWriter(txtPath))
                    {
                        sw.WriteLine(ti.PublishString);
                    }
                }

                if (wt.MediaOptions.CreateTorrent)
                {
                    mi.TorrentCreateInfo.CreateTorrent(bwApp);
                }

                if (App.Settings.XMLTorrentUploadCreate)
                {
                    string fp = Path.Combine(mi.TorrentCreateInfo.TorrentFolder, MediaHelper.GetMediaName(mi.TorrentCreateInfo.MediaLocation)) + ".xml";
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
                    TorrentCreateInfo tci = ti.Media.TorrentCreateInfo;
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
            Helpers.OpenFile(pbScreenshot.ImageLocation);
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            tssPerc.Text = (bwApp.IsBusy ? string.Format("{0}%", (100.0 * (double)pBar.Value / (double)pBar.Maximum).ToString("0")) : "");
            btnAnalyze.Text = "Create &description" + (lbFiles.SelectedItems.Count > 1 ? "s" : "");
            btnCreateTorrent.Text = "Create &torrent" + (lbPublish.SelectedItems.Count > 1 ? "s" : "");
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

                    case ProgressType.UPDATE_PROGRESSBAR_ProgressManager:
                        ProgressManager progress = e.UserState as ProgressManager;
                        pBar.Style = ProgressBarStyle.Continuous;
                        pBar.Maximum = 100;
                        pBar.Value = (int)progress.Percentage;
                        Debug.WriteLine(progress.Percentage);
                        break;

                    case ProgressType.UPDATE_PROGRESSBAR_Cumulative:
                        pBar.Style = ProgressBarStyle.Continuous;
                        pBar.Maximum = 100;
                        pBar.Value = Convert.ToInt16(e.UserState);
                        break;

                    case ProgressType.REPORT_MEDIAINFO_SUMMARY:
                        MediaInfo2 mi = (MediaInfo2)e.UserState;
                        gbDVD.Enabled = (mi.Options.MediaTypeChoice == MediaType.MediaDisc);
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
            chkUploadFullScreenshot.Enabled = chkUploadScreenshots.Checked;
            App.Settings.ProfileActive.UploadScreenshots = chkUploadScreenshots.Checked;
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

        private void CreateTorrentButton()
        {
            if (!bwApp.IsBusy)
            {
                List<TorrentCreateInfo> tps = new List<TorrentCreateInfo>();
                List<TorrentInfo> tiList = new List<TorrentInfo>();
                foreach (TorrentInfo ti in lbPublish.SelectedItems)
                {
                    tps.Add(new TorrentCreateInfo(App.Settings.ProfileActive, ti.Media.Location));
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

                    if (ti.Media.Options.MediaTypeChoice == MediaType.MusicAudioAlbum)
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
            URLHelpers.OpenURL("http://moviethumbnail.sourceforge.net/usage.en.html");
        }

        private void tsmLogsDir_Click(object sender, EventArgs e)
        {
            FileSystem.OpenDirLogs();
        }

        private void cboTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.ProfileActive.ExternalTemplateIndex = cboTemplate.SelectedIndex;
        }

        private void cboScreenshotDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.ProfileActive.ImageUploaderType = (ImageDestination)cboImageUploader.SelectedIndex;
            cboFileUploader.Enabled = App.Settings.ProfileActive.ImageUploaderType == ImageDestination.FileUploader;
        }

        private void btnTemplatesRewrite_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will rewrite old copies of TDMaker created Templates. Your own templates will not be affected. \n\nAre you sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                App.WriteTemplates(true);
            }
        }

        private void OpenVersionHistory()
        {
            /*string h = App.GetText("VersionHistory.txt");

            if (h != string.Empty)
            {
                TextViewer v = new TextViewer(string.Format("{0} - {1}", Application.ProductName, "Version History"), h);
                v.Icon = this.Icon;
                v.ShowDialog();
            }*/

            URLHelpers.OpenURL("https://github.com/McoreD/TDMaker/wiki/Changelog");
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

        private void pgApp_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (IsGuiReady)
            {
                LoadSettingsToControls();

                if (File.Exists(App.Settings.CustomUploadersConfigPath))
                {
                    App.UploadersConfig = UploadersConfig.Load(App.Settings.CustomUploadersConfigPath);
                }
            }
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
            App.Settings.ProfileActive.PublishInfoTypeChoice = (PublishInfoType)cboPublishType.SelectedIndex;
            cboTemplate.Enabled = App.Settings.ProfileActive.PublishInfoTypeChoice == PublishInfoType.ExternalTemplate;
            gbTemplatesInternal.Enabled = App.Settings.ProfileActive.PublishInfoTypeChoice == PublishInfoType.InternalTemplate;
            gbFonts.Enabled = App.Settings.ProfileActive.PublishInfoTypeChoice == PublishInfoType.InternalTemplate;
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
            App.Settings.ProfileActive.ImageFileUploaderType = (FileDestination)cboFileUploader.SelectedIndex;
        }

        private void btnDownloadFFmpeg_Click(object sender, EventArgs e)
        {
            FFmpegHelper.DownloadFFmpeg(true, DownloaderForm_InstallRequested);
        }

        private void DownloaderForm_InstallRequested(string filePath)
        {
            string extractPath = Path.Combine(App.ToolsDir, "ffmpeg.exe");
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

        private void txtProxyHost_TextChanged(object sender, EventArgs e)
        {
            App.Settings.ProxySettings.Host = txtProxyHost.Text;
        }

        private void cbProxyMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.ProxySettings.ProxyMethod = (ProxyMethod)cbProxyMethod.SelectedIndex;

            if (App.Settings.ProxySettings.ProxyMethod == ProxyMethod.Automatic)
            {
                App.Settings.ProxySettings.IsValidProxy();
                txtProxyHost.Text = App.Settings.ProxySettings.Host ?? string.Empty;
                nudProxyPort.Value = App.Settings.ProxySettings.Port;
            }

            UpdateProxyControls();
        }

        private void UpdateProxyControls()
        {
            switch (App.Settings.ProxySettings.ProxyMethod)
            {
                case ProxyMethod.None:
                    txtProxyUsername.Enabled = txtProxyPassword.Enabled = txtProxyHost.Enabled = nudProxyPort.Enabled = false;
                    break;
                case ProxyMethod.Manual:
                    txtProxyUsername.Enabled = txtProxyPassword.Enabled = txtProxyHost.Enabled = nudProxyPort.Enabled = true;
                    break;
                case ProxyMethod.Automatic:
                    txtProxyUsername.Enabled = txtProxyPassword.Enabled = true;
                    txtProxyHost.Enabled = nudProxyPort.Enabled = false;
                    break;
            }
        }

        private void nudProxyPort_ValueChanged(object sender, EventArgs e)
        {
            App.Settings.ProxySettings.Port = (int)nudProxyPort.Value;
        }

        private void txtProxyPassword_TextChanged(object sender, EventArgs e)
        {
            App.Settings.ProxySettings.Password = txtProxyPassword.Text;
        }

        private void txtProxyUsername_TextChanged(object sender, EventArgs e)
        {
            App.Settings.ProxySettings.Username = txtProxyUsername.Text;
        }

        private void btnAddScreenshotProfile_Click(object sender, EventArgs e)
        {
            ProfileOptions profile = new ProfileOptions() { Name = string.Format("Profile {0}", App.Settings.Profiles.Count) };
            listBoxProfiles.Items.Add(profile);
            App.Settings.Profiles.Add(profile);
            listBoxProfiles.SelectedIndex = (listBoxProfiles.Items.Count - 1);
        }

        private void btnRemoveScreenshotProfile_Click(object sender, EventArgs e)
        {
            int sel = listBoxProfiles.SelectedIndex;
            if (listBoxProfiles.SelectedIndex > 0 && App.Settings.Profiles.Count > listBoxProfiles.SelectedIndex)
            {
                ProfileOptions profile = App.Settings.Profiles[listBoxProfiles.SelectedIndex];
                App.Settings.Profiles.Remove(profile);
                listBoxProfiles.Items.Remove(profile);
                listBoxProfiles.SelectedIndex = Math.Min(sel, listBoxProfiles.Items.Count - 1);
            }
        }

        private void listBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.Settings.ProfileIndex = listBoxProfiles.SelectedIndex;
            LoadProfileControls();
        }

        private void LoadProfileControls()
        {
            Text = App.GetProductName() + " - " + App.Settings.ProfileActive.Name;
            pgProfileOptions.SelectedObject = App.Settings.ProfileActive;

            if (IsGuiReady)
            {
                chkUploadScreenshots.Checked = App.Settings.ProfileActive.UploadScreenshots;
                cboImageUploader.SelectedIndex = (int)App.Settings.ProfileActive.ImageUploaderType;
                cboFileUploader.SelectedIndex = (int)App.Settings.ProfileActive.ImageFileUploaderType;
                cboPublishType.SelectedIndex = (int)App.Settings.ProfileActive.PublishInfoTypeChoice;
                cboTemplate.SelectedIndex = App.Settings.ProfileActive.ExternalTemplateIndex;
            }
        }
    }
}