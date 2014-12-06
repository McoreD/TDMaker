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
        public FFmpegThumbnailer(MediaFile mf, string ssDir, ThumbnailerOptions options)
            : base(mf, ssDir, options)
        {
            ThumbnailerPath = App.Settings.FFmpegPath;
        }

        public override void TakeScreenshot()
        {
            for (int i = 0; i < Options.ScreenshotCount; i++)
            {
                int timeSliceElapsed = TimeSlice * (i + 1);
                string tempScreenshotPath = Path.Combine(ScreenshotDir, string.Format("{0}-{1}.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath), timeSliceElapsed));

                ProcessStartInfo psi = new ProcessStartInfo(ThumbnailerPath);
                psi.Arguments = string.Format("-ss {0} -i \"{1}\" -f image2 -vframes 1 -y \"{2}\"", timeSliceElapsed, MediaFile.FilePath, tempScreenshotPath);
                psi.WindowStyle = ProcessWindowStyle.Minimized;

                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit(1000 * 30);
                p.Close();

                if (File.Exists(tempScreenshotPath))
                {
                    ScreenshotInfo screenshotInfo = new ScreenshotInfo(tempScreenshotPath)
                    {
                        Args = psi.Arguments,
                        Timestamp = TimeSpan.FromSeconds(timeSliceElapsed)
                    };

                    TempScreenshots.Add(screenshotInfo);
                }
            }

            Finish();
        }

        public void TakeScreenshotAlternative()
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