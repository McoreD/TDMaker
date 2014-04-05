using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDMakerLib
{
    public static class FileSystem
    {
        public delegate void DebugLogEventHandler(string line);

        public static event DebugLogEventHandler DebugLogChanged;

        public static StringBuilder DebugLog = new StringBuilder();
        public static string DebugLogFilePath = Path.Combine(Program.LogsDir, string.Format("{0}-{1}-debug.txt", Application.ProductName, DateTime.Now.ToString("yyyyMMdd")));

        private static void OnDebugLogChanged(string line)
        {
            if (DebugLogChanged != null)
            {
                DebugLogChanged(line);
            }
        }

        public static void AppendDebug(Exception ex)
        {
            AppendDebug(ex.ToString());
        }

        public static void AppendDebug(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                // a modified http://iso.org/iso/en/prods-services/popstds/datesandtime.html - McoreD
                string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss - ") + msg;
                Debug.WriteLine(line);
                DebugLog.AppendLine(line);
                OnDebugLogChanged(line);
            }
        }

        public static void OpenDirTorrents()
        {
            if (Directory.Exists(Program.TorrentsDir))
            {
                Process.Start(Program.TorrentsDir);
            }
        }

        public static void OpenDirScreenshots()
        {
            if (Directory.Exists(GetScreenShotsDir()))
            {
                Process.Start(GetScreenShotsDir());
            }
        }

        public static void OpenDirTemplates()
        {
            if (Directory.Exists(Program.TemplatesDir))
            {
                Process.Start(Program.TemplatesDir);
            }
        }

        public static void OpenDirLogs()
        {
            if (Directory.Exists(Program.LogsDir))
            {
                Process.Start(Program.LogsDir);
            }
        }

        public static void OpenDirSettings()
        {
            if (Directory.Exists(Program.SettingsDir))
            {
                Process.Start(Program.SettingsDir);
            }
        }

        public static void OpenFileDebug()
        {
            if (File.Exists(FileSystem.DebugLogFilePath))
            {
                Process.Start(FileSystem.DebugLogFilePath);
            }
        }

        public static void WriteDebugFile()
        {
            if (!string.IsNullOrEmpty(Program.LogsDir))
            {
                string dir = Program.LogsDir;
                if (Program.Portable)
                {
                    dir = Path.Combine(Application.StartupPath, Program.LogsDir);
                }
                string fpDebug = Path.Combine(dir, string.Format("{0}-{1}-debug.txt", Application.ProductName, DateTime.Now.ToString("yyyyMMdd")));
                AppendDebug("Writing Debug file: " + fpDebug);
                if (Program.Settings.WriteDebugFile)
                {
                    if (DebugLog.Length > 0)
                    {
                        using (StreamWriter sw = new StreamWriter(fpDebug, true))
                        {
                            sw.WriteLine(DebugLog.ToString());
                            DebugLog = new StringBuilder();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Used to browse the defaule Screneshots folder only
        /// </summary>
        /// <returns></returns>
        public static string GetScreenShotsDir()
        {
            return GetScreenShotsDir("");
        }

        public static string GetScreenShotsDir(string mediaFilePath)
        {
            switch (Program.Settings.ScreenshotsLoc)
            {
                case LocationType.CustomFolder:
                    return Directory.Exists(Program.Settings.CustomScreenshotsDir) ? Program.Settings.CustomScreenshotsDir : Program.PicturesDir;
                case LocationType.ParentFolder:
                    if (string.IsNullOrEmpty(mediaFilePath))
                    {
                        if (Program.Settings.ScreenshotsLoc == LocationType.CustomFolder && Directory.Exists(Program.Settings.CustomScreenshotsDir))
                        {
                            return Program.Settings.CustomScreenshotsDir;
                        }
                        else
                        {
                            return Program.PicturesDir;
                        }
                    }
                    else
                    {
                        return Path.GetDirectoryName(mediaFilePath);
                    }
                case LocationType.KnownFolder:
                default:
                    return Program.Settings.KeepScreenshots ? Program.PicturesDir : Program.zTempDir;
            }
        }

        public static XMLTorrentUpload GetXMLTorrentUpload(MediaInfo2 mi)
        {
            string format = string.Empty;
            string res = string.Empty;
            string media = mi.Source;

            if (mi.MediaTypeChoice == MediaType.MediaDisc)
            {
                format = mi.Source;
                res = mi.Overall.Video.Standard;
                if (format == "DVD-5" || format == "DVD-9")
                {
                    media = "DVD";
                }
            }
            else if (!string.IsNullOrEmpty(mi.Overall.Video.Codec))
            {
                string codec = mi.Overall.Video.Codec.ToLower();
                if (codec.Contains("divx"))
                {
                    format = "DivX";
                }
                else if (codec.Contains("xvid"))
                {
                    format = "XviD";
                }
                else if (codec.Contains("avc") || codec.Contains("x264"))
                {
                    format = "H.264";
                }
            }

            if (string.IsNullOrEmpty(res) && !string.IsNullOrEmpty(mi.Overall.Video.Height) && !string.IsNullOrEmpty(mi.Overall.Video.Width))
            {
                string height = mi.Overall.Video.Height;
                double dblWidth = 0.0;
                double dblHeight = 0.0;
                double.TryParse(mi.Overall.Video.Width, out dblWidth);
                double.TryParse(height, out dblHeight);

                if (dblWidth > 1900)
                {
                    res = "1080p";
                }
                else if (dblWidth > 1200)
                {
                    res = "720p";
                }
                else if (dblHeight > 480)
                {
                    res = "576p";
                }
                else if (dblWidth > 700)
                {
                    res = "480p";
                }
                else
                {
                    res = mi.Overall.Video.Resolution;
                }
            }

            string fileType = string.Empty;
            string ext = mi.Overall.FileExtension.ToLower();
            string[] exts = new string[] { "avi", "mpg", "mpeg", "mkv", "mp4", "vob", "iso" };
            if (!string.IsNullOrEmpty(ext))
            {
                foreach (string exMy in exts)
                {
                    if (ext.Contains(exMy))
                    {
                        fileType = exMy.ToUpper();
                        fileType = fileType.Replace("MPEG", "MPG");
                        fileType = fileType.Replace("VOB", "VOB IFO");
                        break;
                    }
                }
            }

            XMLTorrentUpload xmlUpload = new XMLTorrentUpload()
            {
                TorrentFilePath = mi.TorrentCreateInfoMy.TorrentFilePath,
                ReleaseDescription = mi.ReleaseDescription,
                MediaInfoSummary = mi.Overall.Summary,
                Format = format,
                Resolution = res,
                Width = mi.Overall.Video.Width,
                Height = mi.Overall.Video.Height,
                Media = media,
                FileType = fileType
            };

            if (mi.UploadScreenshots)
            {
                switch (mi.MediaTypeChoice)
                {
                    case MediaType.MediaDisc:
                        foreach (Screenshot ss in mi.Overall.Thumbnailer.Screenshots)
                        {
                            xmlUpload.Screenshots.Add(ss.FullImageLink);
                        }
                        break;

                    default:
                        foreach (MediaFile mf in mi.MediaFiles)
                        {
                            foreach (Screenshot ss in mf.Thumbnailer.Screenshots)
                            {
                                xmlUpload.Screenshots.Add(ss.FullImageLink);
                            }
                        }
                        break;
                }
            }

            return xmlUpload;
        }

        /// <summary>
        /// Function to move a directory with overwriting existing files
        /// </summary>
        /// <param name="dirOld"></param>
        /// <param name="dirNew"></param>
        public static void MoveDirectory(string dirOld, string dirNew)
        {
            if (Directory.Exists(dirOld) && dirOld != dirNew)
            {
                if (MessageBox.Show("Would you like to move old Root folder content to the new location?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        CopyDirectory(dirOld, dirNew, true);
                        Directory.Delete(dirOld, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
    }
}