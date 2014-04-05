using HelpersLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;
using UploadersLib;
using UploadersLib.HelperClasses;

namespace TDMakerLib
{
    [XmlRoot("Settings")]
    public class XMLSettingsCore : XMLSettings
    {
        public static string XMLFileName = string.Format("{0}-{1}-Settings.xml", Application.ProductName, Application.ProductVersion);

        public XMLSettingsCore()
        {
            ApplyDefaultValues(this);
            AuthoringModes = new StringCollection();
            DiscMenus = new StringCollection();
            Extras = new StringCollection();
            MediaSources = new StringCollection();
            SupportedFileExtAudio = new StringCollection();
            SupportedFileExtVideo = new StringCollection();
        }

        // Tab 1 - Input

        // Disc Properties
        [BrowsableAttribute(false)]
        public bool bAuthoring { get; set; }

        public string AuthoringMode = "Untouched";

        [Category("Input"), Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection AuthoringModes { get; set; }

        [BrowsableAttribute(false)]
        public bool bDiscMenu { get; set; }

        public string DiscMenu = "Intact";

        [Category("Input"), Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection DiscMenus { get; set; }

        [BrowsableAttribute(false)]
        public bool bExtras { get; set; }

        public string Extra = "Intact";

        [Category("Input"), Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection Extras { get; set; }

        // Source Properties
        [Category("Input"), Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection MediaSources { get; set; }

        [Category("Input"), Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)),
        Description("Supported file types by MediaInfo and MTN. Add more file types only if you are absolutely sure both MediaInfo and MTN can handle those.")]
        public StringCollection SupportedFileExtVideo { get; set; }

        [Category("Input"), Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)),
        Description("Supported file types by TDMaker to create a Music Album NFO file. Add more file types only if you are absolutely sure both MediaInfo and MTN can handle those.")]
        public StringCollection SupportedFileExtAudio { get; set; }

        [BrowsableAttribute(false), DefaultValue(true)]
        public bool bTitle { get; set; }

        [BrowsableAttribute(false)]
        public bool bWebLink { get; set; }

        // Tab 2 - Media Info
        /*
         * Nothing
         */

        // Tab 3 - Publish
        [Category("Screenshots"), DefaultValue(true), Description("Create screenshots using thumbnailer and upload")]
        public bool ScreenshotsUpload { get; set; }

        [Category("Screenshots"), DefaultValue(true), Description("Use full image URL in the torrent description.")]
        public bool UseFullPicture { get; set; }

        [Category("Screenshots"), DefaultValue(LocationType.KnownFolder), Description("Create screenshots in the same folders as the media file, default torrent folder or in a custom folder")]
        public LocationType ScreenshotsLoc { get; set; }

        [Category("Screenshots"), DefaultValue(true), Description("Keep or delete screenshots after processing files")]
        public bool KeepScreenshots { get; set; }

        // Tab 3 - Publish - Options
        [Category("Publish / Config"), DefaultValue(false), Description("Setting true will center align the description")]
        public bool AlignCenter { get; set; }

        [Category("Publish / Config"), DefaultValue(false), Description("Setting true will retain the formatting on some message boards")]
        public bool PreText { get; set; }

        // Tab 4.1 - Options - General
        [Category("Options / General"), DefaultValue(true), Description("Show Media Wizard always; otherwise it will only be shown when you import multiple files")]
        public bool ShowMediaWizardAlways { get; set; }

        [Category("Options / General"), DefaultValue(true), Description("Process media immediately after loading file or folder")]
        public bool AnalyzeAuto { get; set; }

        [Category("Options / General"), DefaultValue(true), Description("Write debug information into a log file.")]
        public bool WriteDebugFile { get; set; }

        // Tab 4.4 - Options - Publish Templates
        [Category("Options / Publish"), DefaultValue(false), Description("Write the torrent description to file")]
        public bool WritePublish { get; set; }

        [Category("Options / Publish"), DefaultValue(true), Description("Have larger text when [pre] tag is set")]
        public bool LargerPreText { get; set; }

        [Category("Options / Publish"), DefaultValue(PublishInfoType.MediaInfo), Description("Use internal template, external templates or information in MediaInfo in the torrent description in Publish tab")]
        public PublishInfoType PublishInfoTypeChoice { get; set; }

        [Category("Options / Publish / Font Sizes"), DefaultValue(5), Description("Font Size for Heading 1")]
        public int FontSizeHeading1 { get; set; }

        [Category("Options / Publish / Font Sizes"), DefaultValue(4), Description("Font Size for Heading 2")]
        public int FontSizeHeading2 { get; set; }

        [Category("Options / Publish / Font Sizes"), DefaultValue(3), Description("Font Size for Heading 3")]
        public int FontSizeHeading3 { get; set; }

        [Category("Options / Publish / Font Sizes"), DefaultValue(2), Description("Font Size for Body")]
        public int FontSizeBody { get; set; }

        [Category("Options / Publish / Font Sizes"), DefaultValue(1), Description("Font Size increment")]
        public int FontSizeIncr { get; set; }

        [Browsable(false)]
        public int TemplateIndex { get; set; }

        [Category("Options / Proxy"), Description("Proxy Settings")]
        public bool ProxyEnabled { get; set; }

        [Category("Options / Proxy"), Description("Proxy Settings")]
        public ProxyInfo ProxySettings = new ProxyInfo();

        // Tab 4.2 - Options - MTN
        [Category("Thumbnailers"), DefaultValue(ThumbnailerType.MPlayer), Description("Chooser thumbnailer application to take screenshots.")]
        public ThumbnailerType ThumbnailerType { get; set; }

        [Category("Thumbnailers / MTN"), DefaultValue(false), Description("Show MTN during file creation")]
        public bool ShowMTNWindow { get; set; }

        [EditorAttribute(typeof(ExeFileNameEditor), typeof(UITypeEditor))]
        [Category("Thumbnailers / MTN"), Description("MTN Argument")]
        public string MTNPath { get; set; }

        [EditorAttribute(typeof(ExeFileNameEditor), typeof(UITypeEditor))]
        [Category("Thumbnailers / MPlayer"), Description("MTN Argument")]
        public string MPlayerPath { get; set; }

        public MPlayerThumbnailerOptions MPlayerOptions = new MPlayerThumbnailerOptions();

        // Tab 4.3 - Options - Image Hosting
        public ImageDestination ImageUploaderType = ImageDestination.Imgur;

        [Category("Thumbnailers / Image Uploaders"), DefaultValue(""), Description("PtpImg registration code")]
        public string PtpImgCode { get; set; }

        [Category("Thumbnailers / Image Uploaders"), DefaultValue(""), Description("ImageShack registration code")]
        public string ImageShackRegCode { get; set; }

        [Category("Thumbnailers / Image Uploaders"), DefaultValue(false), Description("Use ImageShack registration code")]
        public bool UseImageShackRegCode { get; set; }

        // Tab 4.5 - Options - Torrent Creator
        [Browsable(false)]
        public int TrackerGroupActive { get; set; }

        [Category("Torrent Creator"), DefaultValue(false), Description("Create Torrent")]
        public bool TorrentCreateAuto { get; set; }

        [Category("Torrent Creator"), DefaultValue(LocationType.KnownFolder), Description("Create torrents in the same folders as the media file, default torrent folder or in a custom folder")]
        public LocationType TorrentLocationChoice { get; set; }

        [Category("Torrent Creator"), DefaultValue(false), Description("Save torrent files in sub-folders organized by tracker namer")]
        public bool TorrentsOrganize { get; set; }

        public List<TrackerGroup> TrackerGroups = new List<TrackerGroup>();

        [Category("Torrent Creator"), DefaultValue(false), Description("Create XML Torrent Upload file")]
        public bool XMLTorrentUploadCreate { get; set; }

        // Tab 4.0 - Options - Paths

        [Category("Options / Paths"), Description("Browse to reconfigure the MediaInfo.dll folder path")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomMediaInfoDllDir { get; set; }

        [Category("Options / Paths"), DefaultValue(false), Description("Use custom Templates directory")]
        public bool UseCustomTemplatesDir { get; set; }

        [Category("Options / Paths"), Description("Browse to reconfigure the Templates folder path")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomTemplatesDir { get; set; }

        [Category("Options / Paths"), DefaultValue(false), Description("Use custom Torrents directory")]
        public bool UseCustomTorrentsDir { get; set; }

        [Category("Options / Paths"), Description("Browse to change where torrent files are saved")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomTorrentsDir { get; set; }

        [Category("Options / Paths"), Description("Browse to change where screenshots are saved")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string CustomScreenshotsDir { get; set; }

        public string txtMTN_T_Title = string.Empty;

        #region I/O Methods

        public void Write(string filePath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                //Write XML file
                XmlSerializer serial = new XmlSerializer(typeof(XMLSettingsCore));
                FileStream fs = new FileStream(filePath, FileMode.Create);
                serial.Serialize(fs, this);
                fs.Close();

                serial = null;
                fs = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void Write()
        {
            Write(Program.AppConf.XMLSettingsFile);
        }

        public static XMLSettingsCore Read()
        {
            Program.AppConf.XMLSettingsFile = Program.AppConf.GetSettingsFilePath();
            return Read(Program.AppConf.XMLSettingsFile);
        }

        public static XMLSettingsCore Read(string filePath)
        {
            return SettingsHelper.Load<XMLSettingsCore>(filePath, SerializationType.Xml);
        }

        #endregion I/O Methods
    }
}