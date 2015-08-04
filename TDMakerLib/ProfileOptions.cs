using ShareX.HelpersLib;
using ShareX.UploadersLib;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using UploadersLib;

namespace TDMakerLib
{
    public class ProfileOptions : SettingsBase<ProfileOptions>
    {
        public ImageDestination ImageUploaderType = ImageDestination.Imgur;
        public FileDestination ImageFileUploaderType = FileDestination.MaxFile;

        [Category(ComponentModelStrings.General), DefaultValue("Default"), Description("Profile name")]
        public string Name { get; set; }

        [Category(ComponentModelStrings.General), DefaultValue("Default"), Description("Default initial directory for file/directory open dialog")]
        [Editor(typeof(DirectoryNameEditor), typeof(UITypeEditor))]
        public string DefaultMediaDirectory { get; set; }

        #region Screenshots

        [Category(ComponentModelStrings.Screenshots), DefaultValue(ThumbnailExtension.png), Description("FFmpeg thumbnail extension e.g. png or jpg")]
        public ThumbnailExtension FFmpegThumbnailExtension { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(3), Description("Maximum number of screenshots to take")]
        public int ScreenshotCount { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Choose random frame each time a media file is processed.")]
        public bool RandomFrame { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Upload screenshots")]
        public bool UploadScreenshots { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Keep or delete screenshots after processing files")]
        public bool KeepScreenshots { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Create screenshots using thumbnailer")]
        public bool CreateScreenshots { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(LocationType.KnownFolder), Description("Create screenshots in the same folders as the media file, default torrent folder or in a custom folder")]
        public LocationType ScreenshotsLocation { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(0), Description("Maximum thumbnail width size, 0 means don't resize")]
        public int MaxThumbnailWidth { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(false), Description("Combine all screenshots to one large screenshot")]
        public bool CombineScreenshots { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(20), Description("Space between border and content as pixel")]
        public int Padding { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(10), Description("Space between screenshots as pixel")]
        public int Spacing { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(1), Description("Number of screenshots per row")]
        public int ColumnCount { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(true), Description("Add movie information to the combined screenshot")]
        public bool AddMovieInfo { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(true), Description("Add timestamp of screenshot at corner of image")]
        public bool AddTimestamp { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(true), Description("Draw rectangle shadow behind thumbnails")]
        public bool DrawShadow { get; set; }

        #endregion Screenshots / Combine

        #region Publish

        [Category(ComponentModelStrings.Publish), DefaultValue(true), Description("Hide private information (file path in MediaInfo will be replaced by file name).")]
        public bool HidePrivateInfo { get; set; }

        [Category(ComponentModelStrings.Publish), DefaultValue(PublishInfoType.MediaInfo), Description("Use internal template, external templates or information in MediaInfo in the torrent description in Publish tab")]
        public PublishInfoType Publisher { get; set; }

        [Category(ComponentModelStrings.Publish), DefaultValue("MTN"), TypeConverter(typeof(StringListConverter))]
        public string PublisherExternalTemplateName { get; set; }

        [Category(ComponentModelStrings.Publish), DefaultValue(true), Description("Use full image URL (instead of thumbnail URL) in the torrent description.")]
        public bool UseFullPictureURL { get; set; }

        [Category(ComponentModelStrings.PublishTemplates), DefaultValue(false), Description("Setting true will center align the description")]
        public bool AlignCenter { get; set; }

        [Category(ComponentModelStrings.PublishTemplates), DefaultValue(false), Description("Setting true will retain the formatting on some message boards")]
        public bool PreText { get; set; }

        [Category(ComponentModelStrings.PublishTemplates), DefaultValue(false), Description("Write the torrent description to file")]
        public bool WritePublish { get; set; }

        [Category(ComponentModelStrings.PublishTemplates), DefaultValue(true), Description("Have larger text when [pre] tag is set")]
        public bool LargerPreText { get; set; }

        [Category(ComponentModelStrings.PublishTemplatesFontSizes), DefaultValue(5), Description("Font Size for Heading 1")]
        public int FontSizeHeading1 { get; set; }

        [Category(ComponentModelStrings.PublishTemplatesFontSizes), DefaultValue(4), Description("Font Size for Heading 2")]
        public int FontSizeHeading2 { get; set; }

        [Category(ComponentModelStrings.PublishTemplatesFontSizes), DefaultValue(3), Description("Font Size for Heading 3")]
        public int FontSizeHeading3 { get; set; }

        [Category(ComponentModelStrings.PublishTemplatesFontSizes), DefaultValue(2), Description("Font Size for Body")]
        public int FontSizeBody { get; set; }

        [Category(ComponentModelStrings.PublishTemplatesFontSizes), DefaultValue(1), Description("Font Size increment")]
        public int FontSizeIncr { get; set; }

        #endregion Publish

        #region Torrent creator

        [Category(ComponentModelStrings.TorrentCreator), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor)),
         Description("Your personal Announce URL is usually shown in the upload page e.g. http://torrent.ubuntu.com:6969")]
        public StringCollection Trackers { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(true), Description("Create torrent after analysing media.")]
        public bool CreateTorrent { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(LocationType.KnownFolder), Description("Create torrents in the same folders as the media file, default torrent folder or in a custom folder")]
        public LocationType TorrentsFolder { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(false), Description("Create XML Torrent Upload file")]
        public bool XMLTorrentUploadCreate { get; set; }

        #endregion Torrent creator

        public ProfileOptions()
        {
            this.ApplyDefaultPropertyValues();
            Trackers = new StringCollection();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}