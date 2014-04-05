using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            int time_slice = (int)(MediaFile.SegmentDuration / ((Options.NumberOfScreenshots + 1) * 1000));

            if (File.Exists(tempFp))
            {
                File.Delete(tempFp);
            }

            List<string> fpPaths = new List<string>();
            List<Screenshot> tempSS = new List<Screenshot>();

            for (int i = 0; i < Options.NumberOfScreenshots; i++)
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
                    System.Drawing.Image img = Combine(fpPaths.ToArray(), 5, Options.CombineScreenshotsAddMovieInfo ? MediaFile.GetMTNString() : "");
                    string temp_fp = Path.Combine(ScreenshotDir, Path.GetFileNameWithoutExtension(MediaFile.FilePath) + "_s.png");
                    img.Save(temp_fp, ImageFormat.Png);
                    Screenshots.Add(new Screenshot(temp_fp) { Args = tempSS[0].Args });
                }
                else
                {
                    Screenshots.AddRange(tempSS);
                }
            }
        }
    }

    public class MPlayerThumbnailerOptions
    {
        [Category("Options"), DefaultValue(3), Description("# of screenshots to take")]
        public int NumberOfScreenshots { get; set; }

        [Category("Options"), DefaultValue(false), Description("Combine all screenshots to one large screenshot")]
        public bool CombineScreenshots { get; set; }

        [Category("Options"), DefaultValue(true), Description("Add movie information to the combined screenshot")]
        public bool CombineScreenshotsAddMovieInfo { get; set; }

        public MPlayerThumbnailerOptions()
        {
            ApplyDefaultValues(this);
        }

        static public void ApplyDefaultValues(object self)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(self))
            {
                DefaultValueAttribute attr = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
                if (attr == null) continue;
                prop.SetValue(self, attr.Value);
            }
        }
    }
}