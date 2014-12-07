using HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class ThumbnailerOptions
    {
        [Category("Options"), DefaultValue(3), Description("Number of screenshots to take")]
        public int ScreenshotCount { get; set; }

        [Category("Options"), DefaultValue(true), Description("Choose random frame each time a media file is processed.")]
        public bool RandomFrame { get; set; }

        [Category("Combine screenshots"), DefaultValue(false), Description("Combine all screenshots to one large screenshot")]
        public bool CombineScreenshots { get; set; }

        [Category("Combine screenshots"), DefaultValue(0), Description("Maximum thumbnail width size, 0 means don't resize")]
        public int MaxThumbnailWidth { get; set; }

        [Category("Combine screenshots"), DefaultValue(20), Description("Space between border and content as pixel")]
        public int Padding { get; set; }

        [Category("Combine screenshots"), DefaultValue(10), Description("Space between screenshots as pixel")]
        public int Spacing { get; set; }

        [Category("Combine screenshots"), DefaultValue(1), Description("Number of screenshots per row")]
        public int ColumnCount { get; set; }

        [Category("Combine screenshots"), DefaultValue(true), Description("Add movie information to the combined screenshot")]
        public bool AddMovieInfo { get; set; }

        [Category("Combine screenshots"), DefaultValue(true), Description("Add timestamp of screenshot at corner of image")]
        public bool AddTimestamp { get; set; }

        [Category("Combine screenshots"), DefaultValue(true), Description("Draw rectangle shadow behind thumbnails")]
        public bool DrawShadow { get; set; }

        public ThumbnailerOptions()
        {
            this.ApplyDefaultPropertyValues();
        }
    }
}