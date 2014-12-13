using ShareX.HelpersLib;
using ShareX.HelpersLib.UITypeEditors;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace TDMakerLib
{
    public class Settings : SettingsBase<Settings>
    {
        public Settings()
        {
            this.ApplyDefaultPropertyValues();
            AuthoringModes = new StringCollection();
            DiscMenus = new StringCollection();
            Extras = new StringCollection();
            MediaSources = new StringCollection();
            SupportedFileExtAudio = new StringCollection();
            SupportedFileExtVideo = new StringCollection();
        }

        [Browsable(false)]
        public ProfileOptions ProfileActive
        {
            get
            {
                if (Profiles.Count == 0)
                {
                    Profiles.Add(new ProfileOptions() { Name = "Movies Profile" });
                    Profiles.Add(new ProfileOptions()
                    {
                        Name = "Music Videos Profile",
                        CombineScreenshots = true,
                        ScreenshotCount = 16,
                        ColumnCount = 4,
                        MaxThumbnailWidth = 256,
                        KeepScreenshots = false,
                        FFmpegThumbnailExtension = ThumbnailExtension.jpg
                    });
                }

                ProfileOptions profile = Profiles[0];

                if (ProfileIndex >= 0 && Profiles.Count > ProfileIndex)
                {
                    profile = Profiles[ProfileIndex];
                }

                return profile;
            }
        }

        #region DVD Properties

        [BrowsableAttribute(false)]
        public bool bAuthoring { get; set; }
        public string AuthoringMode = "Untouched";

        [BrowsableAttribute(false)]
        public bool bDiscMenu { get; set; }
        public string DiscMenu = "Intact";

        [BrowsableAttribute(false)]
        public bool bExtras { get; set; }
        public string Extra = "Intact";

        [BrowsableAttribute(false), DefaultValue(true)]
        public bool bTitle { get; set; }

        [BrowsableAttribute(false)]
        public bool bWebLink { get; set; }

        #endregion DVD Properties

        #region Input

        [Category(ComponentModelStrings.Input), DefaultValue(true), Description("Show Media Wizard always; otherwise it will only be shown when you import multiple files")]
        public bool ShowMediaWizardAlways { get; set; }

        [Category(ComponentModelStrings.Input), DefaultValue(true), Description("Process media immediately after loading file or folder")]
        public bool AnalyzeAuto { get; set; }

        [Category(ComponentModelStrings.InputMedia), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection AuthoringModes { get; set; }

        [Category(ComponentModelStrings.InputMedia), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection DiscMenus { get; set; }

        [Category(ComponentModelStrings.InputMedia), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection Extras { get; set; }

        [Category(ComponentModelStrings.InputMedia), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection MediaSources { get; set; }

        [Category(ComponentModelStrings.InputMedia), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor)),
         Description("Supported file types by MediaInfo and MTN. Add more file types only if you are absolutely sure both MediaInfo and MTN can handle those.")]
        public StringCollection SupportedFileExtVideo { get; set; }

        [Category(ComponentModelStrings.InputMedia), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor)),
         Description("Supported file types by TDMaker to create a Music Album NFO file. Add more file types only if you are absolutely sure both MediaInfo and MTN can handle those.")]
        public StringCollection SupportedFileExtAudio { get; set; }

        [Category(ComponentModelStrings.Input), DefaultValue(true), Description("Write debug information into a log file.")]
        public bool WriteDebugFile { get; set; }

        #endregion Input

        #region Screenshots / Thumbnailers

        [Category(ComponentModelStrings.Thumbnailers), DefaultValue(ThumbnailerType.FFmpeg), Description("Chooser thumbnailer application to take screenshots.")]
        public ThumbnailerType ThumbnailerType { get; set; }

        [EditorAttribute(typeof(ExeFileNameEditor), typeof(UITypeEditor))]
        [Category(ComponentModelStrings.ThumbnailersFFmpeg), Description("FFmpeg path")]
        public string FFmpegPath { get; set; }

        [EditorAttribute(typeof(ExeFileNameEditor), typeof(UITypeEditor))]
        [Category(ComponentModelStrings.ThumbnailersMPlayer), Description("MPlayer path")]
        public string MPlayerPath { get; set; }

        public int ProfileIndex = 0;
        public List<ProfileOptions> Profiles = new List<ProfileOptions>();

        #endregion Screenshots / Thumbnailers

        #region Screenshots / Uploaders

        [Category(ComponentModelStrings.ScreenshotsImageUploaders), DefaultValue(""), Description("PtpImg registration code")]
        public string PtpImgCode { get; set; }

        [Category(ComponentModelStrings.ScreenshotsImageUploaders), DefaultValue(5), Description("Buffer size power")]
        public double BufferSizePower { get; set; }

        #endregion Screenshots / Uploaders

        #region Paths

        [Category(ComponentModelStrings.Paths), Description("Browse to reconfigure the MediaInfo.dll folder path")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomMediaInfoDllDir { get; set; }

        [Category(ComponentModelStrings.Paths), DefaultValue(false), Description("Use custom Templates directory")]
        public bool UseCustomTemplatesDir { get; set; }

        [Category(ComponentModelStrings.Paths), Description("Browse to reconfigure UploadersConfig file path")]
        [EditorAttribute(typeof(JsonFileNameEditor), typeof(UITypeEditor))]
        public string CustomUploadersConfigPath { get; set; }

        [Category(ComponentModelStrings.Paths), Description("Browse to reconfigure the Templates folder path")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomTemplatesDir { get; set; }

        [Category(ComponentModelStrings.Paths), DefaultValue(false), Description("Use custom Torrents directory")]
        public bool UseCustomTorrentsDir { get; set; }

        [Category(ComponentModelStrings.Paths), Description("Browse to change where torrent files are saved")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomTorrentsDir { get; set; }

        [Category(ComponentModelStrings.Paths), Description("Browse to change where screenshots are saved")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomScreenshotsDir { get; set; }

        #endregion Paths

        #region Proxy

        public ProxyInfo ProxySettings = new ProxyInfo();

        #endregion Proxy
    }
}