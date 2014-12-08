using HelpersLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TDMakerLib
{
    public class Thumbnailer
    {
        protected string ThumbnailerPath;

        protected ProfileOptions Options = new ProfileOptions();

        protected MediaFile MediaFile { get; set; }
        protected string ScreenshotDir { get; set; }

        protected List<ScreenshotInfo> TempScreenshots = new List<ScreenshotInfo>();
        public List<ScreenshotInfo> Screenshots = new List<ScreenshotInfo>();
        protected int TimeSlice;
        protected List<int> MediaSeekTimes = new List<int>();

        public string MediaSummary { get; protected set; }

        protected Thumbnailer()
        {
        }

        public Thumbnailer(MediaFile mf, string ssDir, ProfileOptions options)
        {
            MediaFile = mf;
            ScreenshotDir = ssDir;
            Options = options;

            TimeSlice = GetTimeSlice(Options.ScreenshotCount);
            for (int i = 1; i < Options.ScreenshotCount + 2; i++)
            {
                MediaSeekTimes.Add(GetTimeSlice(Options.ScreenshotCount, 2) * i);
            }
        }

        public virtual void TakeScreenshot()
        {
            string MPlayerTempFp = Path.Combine(ScreenshotDir, "00000001.png"); // MPlayer creates this file by default

            switch (App.Settings.ThumbnailerType)
            {
                case ThumbnailerType.MPlayer:
                    ThumbnailerPath = App.Settings.MPlayerPath;
                    if (File.Exists(MPlayerTempFp)) File.Delete(MPlayerTempFp);
                    break;
                case ThumbnailerType.FFmpeg:
                    ThumbnailerPath = App.Settings.FFmpegPath;
                    break;
            }

            for (int i = 0; i < Options.ScreenshotCount; i++)
            {
                int timeSliceElapsed = Options.RandomFrame ? GetRandomTimeSlice(i) : TimeSlice * (i + 1);
                string tempScreenshotPath = Path.Combine(ScreenshotDir, string.Format("{0}-{1}.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath), timeSliceElapsed));

                ProcessStartInfo psi = new ProcessStartInfo(ThumbnailerPath);
                psi.WindowStyle = ProcessWindowStyle.Minimized;

                switch (App.Settings.ThumbnailerType)
                {
                    case ThumbnailerType.MPlayer:
                        psi.Arguments = string.Format("-nosound -ss {0} -zoom -vf screenshot -frames 1 -vo png:z=9:outdir=\\\"{1}\\\" \"{2}\"",
                               timeSliceElapsed, ScreenshotDir, MediaFile.FilePath);
                        break;
                    case ThumbnailerType.FFmpeg:
                        psi.Arguments = string.Format("-ss {0} -i \"{1}\" -f image2 -vframes 1 -y \"{2}\"", timeSliceElapsed, MediaFile.FilePath, tempScreenshotPath);
                        break;
                }

                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit(1000 * 30);
                p.Close();

                switch (App.Settings.ThumbnailerType)
                {
                    case ThumbnailerType.MPlayer:
                        if (File.Exists(MPlayerTempFp))
                        {
                            if (File.Exists(tempScreenshotPath)) File.Delete(tempScreenshotPath);
                            File.Move(MPlayerTempFp, tempScreenshotPath);
                        }
                        break;
                }

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

        protected virtual void Finish()
        {
            if (TempScreenshots != null && TempScreenshots.Count > 0)
            {
                if (Options.CombineScreenshots)
                {
                    using (Image img = CombineScreenshots(TempScreenshots))
                    {
                        string temp_fp = Path.Combine(ScreenshotDir, Path.GetFileNameWithoutExtension(MediaFile.FilePath) + "_s.png");
                        img.Save(temp_fp, ImageFormat.Png);
                        Screenshots.Add(new ScreenshotInfo(temp_fp) { Args = TempScreenshots[0].Args });
                    }
                }
                else
                {
                    Screenshots.AddRange(TempScreenshots);
                }
            }
        }

        protected int GetRandomTimeSlice(int start)
        {
            Random random = new Random();
            return (int)(random.NextDouble() * (MediaSeekTimes[start + 1] - MediaSeekTimes[start]) + MediaSeekTimes[start]);
        }

        protected int GetTimeSlice(int numScreenshots, int extraSlices = 1)
        {
            return (int)(MediaFile.SegmentDuration / ((numScreenshots + extraSlices) * 1000));
        }

        protected Image CombineScreenshots(List<ScreenshotInfo> screenshots)
        {
            List<Image> images = new List<Image>();
            Image finalImage = null;

            try
            {
                string infoString = "";
                int infoStringHeight = 0;

                if (Options.AddMovieInfo)
                {
                    infoString = MediaFile.GetMTNString();
                    infoStringHeight = 80;
                }

                foreach (ScreenshotInfo screenshot in screenshots)
                {
                    Image img = Image.FromFile(screenshot.LocalPath);

                    if (Options.MaxThumbnailWidth > 0 && img.Width > Options.MaxThumbnailWidth)
                    {
                        int maxThumbnailHeight = (int)((float)Options.MaxThumbnailWidth / img.Width * img.Height);
                        img = ImageHelpers.ResizeImage(img, Options.MaxThumbnailWidth, maxThumbnailHeight);
                    }

                    images.Add(img);
                }

                int columnCount = Options.ColumnCount;

                int thumbWidth = images[0].Width;

                int width = Options.Padding * 2 +
                    thumbWidth * columnCount +
                    (columnCount - 1) * Options.Spacing;

                int rowCount = (int)Math.Ceiling(images.Count / (float)columnCount);

                int thumbHeight = images[0].Height;

                int height = Options.Padding * 3 +
                    infoStringHeight +
                    thumbHeight * rowCount +
                    (rowCount - 1) * Options.Spacing;

                finalImage = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.WhiteSmoke);

                    if (!string.IsNullOrEmpty(infoString))
                    {
                        using (Font font = new Font("Arial", 14))
                        {
                            g.DrawString(infoString, font, Brushes.Black, Options.Padding, Options.Padding);
                        }
                    }

                    int i = 0;
                    int offsetY = Options.Padding * 2 + infoStringHeight;

                    for (int y = 0; y < rowCount; y++)
                    {
                        int offsetX = Options.Padding;

                        for (int x = 0; x < columnCount; x++)
                        {
                            if (Options.DrawShadow)
                            {
                                int shadowOffset = 3;

                                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
                                {
                                    g.FillRectangle(shadowBrush, offsetX + shadowOffset, offsetY + shadowOffset, thumbWidth, thumbHeight);
                                }
                            }

                            g.DrawImage(images[i], offsetX, offsetY, thumbWidth, thumbHeight);

                            if (Options.AddTimestamp)
                            {
                                int timestampOffset = 10;

                                using (Font font = new Font("Arial", 12))
                                {
                                    ImageHelpers.DrawTextWithShadow(g, screenshots[i].Timestamp.ToString(),
                                        new Point(offsetX + timestampOffset, offsetY + timestampOffset), font, Color.White, Color.Black);
                                }
                            }

                            i++;

                            if (i >= images.Count)
                            {
                                return finalImage;
                            }

                            offsetX += thumbWidth + Options.Spacing;
                        }

                        offsetY += thumbHeight + Options.Spacing;
                    }
                }

                return finalImage;
            }
            catch
            {
                if (finalImage != null)
                {
                    finalImage.Dispose();
                }

                throw;
            }
            finally
            {
                foreach (Image image in images)
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }
            }
        }
    }
}