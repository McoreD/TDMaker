using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

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