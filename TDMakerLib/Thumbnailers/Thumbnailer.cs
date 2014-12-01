using System.Collections.Generic;

namespace TDMakerLib
{
    public abstract class Thumbnailer
    {
        protected MediaFile MediaFile { get; set; }
        protected string ScreenshotDir { get; set; }
        public List<Screenshot> Screenshots = new List<Screenshot>();

        public string MediaSummary { get; protected set; }

        public abstract void TakeScreenshot();
    }
}