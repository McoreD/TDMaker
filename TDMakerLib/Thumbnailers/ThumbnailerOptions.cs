using HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UploadersLib;

namespace TDMakerLib
{
    public class ProfileOptions : SettingsBase<ProfileOptions>
    {
        public ImageDestination ImageUploaderType = ImageDestination.Imgur;
        public FileDestination ImageFileUploaderType = FileDestination.Pomf;

        [Category(ComponentModelStrings.General), DefaultValue("Default"), Description("Profile name")]
        public string Name { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(3), Description("Maximum number of screenshots to take")]
        public int ScreenshotCount { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Choose random frame each time a media file is processed.")]
        public bool RandomFrame { get; set; }

        [Category(ComponentModelStrings.Screenshots), DefaultValue(true), Description("Upload screenshots")]
        public bool UploadScreenshots { get; set; }

        #region Screenshots / Combine

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(false), Description("Combine all screenshots to one large screenshot")]
        public bool CombineScreenshots { get; set; }

        [Category(ComponentModelStrings.ScreenshotsCombine), DefaultValue(0), Description("Maximum thumbnail width size, 0 means don't resize")]
        public int MaxThumbnailWidth { get; set; }

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

        [Category(ComponentModelStrings.Publish), DefaultValue(PublishInfoType.MediaInfo), Description("Use internal template, external templates or information in MediaInfo in the torrent description in Publish tab")]
        public PublishInfoType PublishInfoTypeChoice { get; set; }

        [Category(ComponentModelStrings.Publish), DefaultValue(0), Description("External template index")]
        public int ExternalTemplateIndex { get; set; }

        #endregion Publish

        #region Torrent creator

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(0), Description("Tracker group index")]
        public int TrackerGroupActive { get; set; }

        [Category(ComponentModelStrings.TorrentCreator), DefaultValue(false), Description("Create Torrent")]
        public bool CreateTorrent { get; set; }

        #endregion Torrent creator

        public ProfileOptions()
        {
            this.ApplyDefaultPropertyValues();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}