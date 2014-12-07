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
        public FFmpegThumbnailer(MediaFile mf, string ssDir, ProfileOptions options)
            : base(mf, ssDir, options)
        {
            ThumbnailerPath = App.Settings.FFmpegPath;
        }

        // This method is experimental.
        public override void TakeScreenshot()
        {
            ProcessStartInfo psi = new ProcessStartInfo(ThumbnailerPath);
            psi.WindowStyle = ProcessWindowStyle.Minimized;

            string tempScreenshotPathFormat = Path.Combine(ScreenshotDir, string.Format("{0}-%03d.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath)));

            psi.Arguments = string.Format("-i \"{0}\" -f image2 -vf fps=1/{1} -vframes {2} \"{3}\"", MediaFile.FilePath, TimeSlice, Options.ScreenshotCount, tempScreenshotPathFormat);

            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit(1000 * 30);

            for (int i = 1; i <= Options.ScreenshotCount; i++)
            {
                string tempScreenshotPath = Path.Combine(ScreenshotDir, string.Format("{0}-{1:000}.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath), i));

                if (File.Exists(tempScreenshotPath))
                {
                    ScreenshotInfo screenshotInfo = new ScreenshotInfo(tempScreenshotPath)
                    {
                        Args = psi.Arguments,
                        Timestamp = TimeSpan.FromSeconds(TimeSlice * i)
                    };

                    TempScreenshots.Add(screenshotInfo);
                }
            }

            Finish();
        }
    }
}