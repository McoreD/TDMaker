using System.ComponentModel;
using System.IO;
using System.Text;
using TDMakerLib;

namespace TDMakerLib
{
    /// <summary>
    /// Class for Screnshot information. Belongs to MediaInfo2.cs
    /// </summary>
    public class Screenshot
    {
        /// <summary>
        /// Argument
        /// </summary>
        [Category("Screenshots"), Description("Thumbnailer command line")]
        public string Args { get; set; }

        /// <summary>
        /// URL of Full Image
        /// </summary>
        [Category("Screenshots"), Description("Full image URL")]
        public string FullImageLink { get; set; }

        /// <summary>
        /// URL of Linked Thumbnail
        /// </summary>
        [Category("Screenshots"), Description("Linked thumnail URL")]
        public string LinkedThumbnail { get; set; }

        /// <summary>
        /// Local file path of the Screenshot
        /// </summary>
        [Category("Screenshots"), Description("Local file path")]
        public string LocalPath { get; set; }

        public Screenshot(string fp)
        {
            LocalPath = fp;
        }

        public override string ToString()
        {
            return Path.GetFileName(LocalPath);
        }
    }
}