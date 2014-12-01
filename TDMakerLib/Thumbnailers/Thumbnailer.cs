using System.Collections.Generic;

namespace TDMakerLib
{
    public abstract class Thumbnailer
    {
        protected MediaFile MediaFile { get; set; }
        protected string ScreenshotDir { get; set; }
        public List<ScreenshotInfo> Screenshots = new List<ScreenshotInfo>();

        public string MediaSummary { get; protected set; }

        public abstract void TakeScreenshot();
    }
}