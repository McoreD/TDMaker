using System;
using System.Diagnostics;
using System.IO;

namespace TDMakerLib
{
    public class MovieThumbnailer : Thumbnailer
    {
        private MovieThumbnailer()
        {
        }

        public MovieThumbnailer(MediaFile mf, string ssDir)
        {
            this.MediaFile = mf;
            this.ScreenshotDir = ssDir;
        }

        public override void TakeScreenshot()
        {
            try
            {
                Debug.WriteLine("Creating a MTN process...");
                string assemblyMTN = (Program.IsUNIX ? Program.Settings.MTNPath.Replace(".exe", "") : Program.Settings.MTNPath);
                if (string.IsNullOrEmpty(Path.GetDirectoryName(assemblyMTN)))
                {
                    assemblyMTN = Path.Combine(System.Windows.Forms.Application.StartupPath, assemblyMTN);
                    Program.Settings.MTNPath = assemblyMTN;
                }

                string args = string.Format("{0} \"{1}\"", Adapter.GetMtnArg(ScreenshotDir, Program.mtnProfileMgr.GetMtnProfileActive()), MediaFile.FilePath);

                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo(assemblyMTN);

                if (Program.IsUNIX)
                {
                    psi.UseShellExecute = false;
                }

                Debug.WriteLine("MTN Path: " + assemblyMTN);
                Debug.WriteLine("MTN Args: " + args);

                psi.WindowStyle = (Program.Settings.ShowMTNWindow ? ProcessWindowStyle.Normal : ProcessWindowStyle.Minimized);
                Debug.WriteLine("MTN Window: " + psi.WindowStyle.ToString());
                psi.Arguments = args;

                p.StartInfo = psi;
                p.Start();
                p.WaitForExit(1000 * 30);

                string ssPath = Path.Combine(ScreenshotDir, Path.GetFileNameWithoutExtension(MediaFile.FilePath) + Program.mtnProfileMgr.GetMtnProfileActive().o_OutputSuffix);
                Screenshots.Add(new Screenshot(ssPath)
                {
                    Args = args
                });

                if (Program.IsUNIX)
                {
                    string info = Path.Combine(FileSystem.GetScreenShotsDir(MediaFile.FilePath), Path.GetFileNameWithoutExtension(MediaFile.FilePath) + Program.mtnProfileMgr.GetMtnProfileActive().N_InfoSuffix);

                    using (StreamReader sr = new StreamReader(info))
                    {
                        MediaSummary = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}