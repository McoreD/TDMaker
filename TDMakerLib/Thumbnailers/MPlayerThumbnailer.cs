using HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class MPlayerThumbnailer : Thumbnailer
    {
        private const string MplayerScreenshotFileName = "00000001.png";
        private MPlayerThumbnailerOptions Options = new MPlayerThumbnailerOptions();

        private MPlayerThumbnailer()
        {
        }

        public MPlayerThumbnailer(MediaFile mf, string ssDir, MPlayerThumbnailerOptions options)
        {
            this.MediaFile = mf;
            this.ScreenshotDir = ssDir;
            this.Options = options;
        }

        public override void TakeScreenshot()
        {
            string tempFp = Path.Combine(ScreenshotDir, MplayerScreenshotFileName);
            int time_slice = (int)(MediaFile.SegmentDuration / ((Options.ScreenshotCount + 1) * 1000));

            if (File.Exists(tempFp))
            {
                File.Delete(tempFp);
            }

            List<string> fpPaths = new List<string>();
            List<Screenshot> tempSS = new List<Screenshot>();

            for (int i = 0; i < Options.ScreenshotCount; i++)
            {
                int time_slice_elapsed = time_slice * (i + 1);
                string arg = string.Format("-nosound -ss {0} -zoom -vf screenshot -frames 1 -vo png:z=9:outdir=\\\"{1}\\\" \"{2}\"", time_slice_elapsed,
                                                                                                                                 ScreenshotDir,
                                                                                                                                 MediaFile.FilePath);
                ProcessStartInfo psi = new ProcessStartInfo(Program.Settings.MPlayerPath);
                psi.WindowStyle = ProcessWindowStyle.Minimized;
                psi.Arguments = arg;

                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit(1000 * 30);

                string temp_fp = Path.Combine(ScreenshotDir, string.Format("{0}-{1}.png", Path.GetFileNameWithoutExtension(MediaFile.FilePath), time_slice_elapsed));

                if (File.Exists(temp_fp)) File.Delete(temp_fp);

                if (File.Exists(tempFp))
                {
                    File.Move(tempFp, temp_fp);
                    fpPaths.Add(temp_fp);
                    tempSS.Add(new Screenshot(temp_fp) { Args = arg });
                }
            }

            if (fpPaths.Count > 0)
            {
                if (Options.CombineScreenshots)
                {
                    using (Image img = Render(fpPaths.ToArray()))
                    {
                        string temp_fp = Path.Combine(ScreenshotDir, Path.GetFileNameWithoutExtension(MediaFile.FilePath) + "_s.png");
                        img.Save(temp_fp, ImageFormat.Png);
                        Screenshots.Add(new Screenshot(temp_fp) { Args = tempSS[0].Args });
                    }
                }
                else
                {
                    Screenshots.AddRange(tempSS);
                }
            }
        }

        private Image Render(string[] files)
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
                    infoStringHeight = 100;
                }

                foreach (string file in files)
                {
                    Image img = Image.FromFile(file);
                    images.Add(img);
                }

                int rowCount = Options.ColumnCount;

                int thumbWidth = images[0].Width;

                int width = Options.Padding * 2 +
                    thumbWidth * rowCount +
                    (rowCount - 1) * Options.Spacing;

                int columnCount = (int)Math.Floor(images.Count / (float)rowCount);

                int thumbHeight = images[0].Height;

                int height = Options.Padding * 2 +
                    infoStringHeight +
                    thumbHeight * columnCount +
                    columnCount * Options.Spacing;

                finalImage = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.WhiteSmoke);

                    if (!string.IsNullOrEmpty(infoString))
                    {
                        using (Font font = new Font("Arial", 12, FontStyle.Bold))
                        {
                            g.DrawString(infoString, font, Brushes.Black, Options.Padding, Options.Padding);
                        }
                    }

                    int i = 0;
                    int offsetY = Options.Padding + infoStringHeight + Options.Spacing;

                    for (int y = 0; y < rowCount; y++)
                    {
                        int offsetX = Options.Padding;

                        for (int x = 0; x < columnCount; x++)
                        {
                            g.DrawImage(images[i++], new Rectangle(offsetX, offsetY, thumbWidth, thumbHeight));

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

    public class MPlayerThumbnailerOptions
    {
        [Category("Options"), DefaultValue(3), Description("Number of screenshots to take")]
        public int ScreenshotCount { get; set; }

        [Category("Combine screenshots"), DefaultValue(false), Description("Combine all screenshots to one large screenshot")]
        public bool CombineScreenshots { get; set; }

        [Category("Combine screenshots"), DefaultValue(20), Description("Space between border and content as pixel")]
        public int Padding { get; set; }

        [Category("Combine screenshots"), DefaultValue(true), Description("Add movie information to the combined screenshot")]
        public bool AddMovieInfo { get; set; }

        [Category("Combine screenshots"), DefaultValue(1), Description("Number of screenshots per row")]
        public int ColumnCount { get; set; }

        [Category("Combine screenshots"), DefaultValue(10), Description("Space between screenshots as pixel")]
        public int Spacing { get; set; }

        [Category("Combine screenshots"), DefaultValue(false), Description("Write timestamp of screenshot at corner of image")]
        public bool WriteTimeInfo { get; set; }

        public MPlayerThumbnailerOptions()
        {
            this.ApplyDefaultPropertyValues();
        }
    }
}