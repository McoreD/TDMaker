using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class FFmpegThumbnailer : Thumbnailer
    {
        private FFmpegThumbnailer()
        {
        }

        public FFmpegThumbnailer(MediaFile mf, string ssDir, ThumbnailerOptions options)
            : base(mf, ssDir, options)
        {
            ThumbnailerPath = App.Settings.FFmpegPath;
        }

        public override void TakeScreenshot()
        {
            ProcessStartInfo psi = new ProcessStartInfo(ThumbnailerPath);
            psi.WindowStyle = ProcessWindowStyle.Minimized;

            double fps = 1 / (TimeSlice * 1.1);
            string tempScreenshotPathFormat = Path.Combine(ScreenshotDir, string.Format("{0}-%03d.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath)));

            // -i myvideo.avi -f image2 -vf fps=fps=1/60 img%03d.jpg
            psi.Arguments = string.Format("-i \"{0}\" -f image2 -vf fps=fps={1} \"{2}\"", MediaFile.FilePath, fps, tempScreenshotPathFormat);

            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit(1000 * 30);

            for (int i = 1; i <= Options.ScreenshotCount; i++)
            {
                string tempScreenshotPath = Path.Combine(ScreenshotDir, string.Format("{0}-{1}.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath), i.ToString("000")));

                if (File.Exists(tempScreenshotPath))
                {
                    ScreenshotInfo screenshotInfo = new ScreenshotInfo(tempScreenshotPath)
                    {
                        Args = psi.Arguments,
                        Timestamp = TimeSpan.FromSeconds((1/fps) * i)
                    };

                    TempScreenshots.Add(screenshotInfo);
                }
            }

            Finish();
        }
    }
}