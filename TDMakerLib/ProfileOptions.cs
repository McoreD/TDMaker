using ShareX.HelpersLib;
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
        public FileDestination ImageFileUploaderType = FileDestination.Pomf;

        [Category(ComponentModelStrings.General), DefaultValue("Default"), Description("Profile name")]
        public string Name { get; set; }

        [Category(ComponentModelStrings.General), DefaultValue("Default"), Description("Default initial directory for file/directory open dialog")]
        [Editor(typeof(DirectoryNameEditor), typeof(UITypeEditor))]
        public string DefaultMediaDirectory { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue("png"), Description("FFmpeg thumbnail extension e.g. png or jpg")]
        public string FFmpegThumbnailExtension { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(3), Description("Maximum number of screenshots to take")]
        public int ScreenshotCount { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Choose random frame each time a media file is processed.")]
        public bool RandomFrame { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Upload screenshots")]
        public bool UploadScreenshots { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(0), Description("Maximum thumbnail width size, 0 means don't resize")]
        public int MaxThumbnailWidth { get; set; }

        #region Screenshots / Combine

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
        public PublishInfoType PublishInfoTypeChoice { get; set; }

        [Category(ComponentModelStrings.Publish), DefaultValue(0), Description("External template index")]
        public int ExternalTemplateIndex { get; set; }

        #endregion Publish

        #region Torrent creator

        [Category(ComponentModelStrings.TorrentCreator), Editor(ComponentModelStrings.UITypeEditor, typeof(System.Drawing.Design.UITypeEditor)),
         Description("Your personal Announce URL is usually shown in the upload page e.g. http://torrent.ubuntu.com:6969")]
        public StringCollection Trackers { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(true), Description("Create torrent after analysing media.")]
        public bool CreateTorrent { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(LocationType.KnownFolder), Description("Create torrents in the same folders as the media file, default torrent folder or in a custom folder")]
        public LocationType TorrentsFolder { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(true), Description("Save torrent files in sub-folders organized by tracker name")]
        public bool OrganizeTorrentsByTracker { get; set; }

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