using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TDMakerLib
{
    /// <summary>
    /// Class responsible for reading Template Directories.
    /// A Template Directory has 7 files:
    /// GeneralInfo.txt; VideoInfo.txt; AudioInfo.txt; Disc.txt; File.txt
    /// </summary>
    public class TemplateReader2
    {
        /// <summary>
        /// Location of the Template
        /// </summary>
        public string Location { get; private set; }

        public TorrentInfo TorrentInfo { get; private set; }

        public string PublishInfo { get; private set; }

        private MappingHelper MappingHelperMy = null;

        private string mDiscAudioInfo = "";
        private string mDiscVideoInfo = "";
        private string mFileAudioInfo = "";
        private string mFileVideoInfo = "";
        private string mGeneralInfo = "";
        private string mDiscInfo = "";
        private string mFileInfo = "";

        /// <summary>
        /// Constructor of TemplateReader
        /// </summary>
        /// <param name="loc">Directory Path of the Template</param>
        /// <param name="ti">TorrentInfo object that has MediaInfo</param>
        public TemplateReader2(string loc, TorrentInfo ti)
        {
            this.Location = loc;
            this.TorrentInfo = ti;
            this.MappingHelperMy = new MappingHelper(ti.Media.Overall.Summary);
            // this.MappingHelperMy.ListFieldsAll();

            // Read the files in Location
            string[] files = Directory.GetFiles(loc, "*.txt", SearchOption.AllDirectories);
            foreach (string f in files)
            {
                using (StreamReader sw = new StreamReader(f))
                {
                    switch (Path.GetFileNameWithoutExtension(f))
                    {
                        case "Disc":
                            mDiscInfo = sw.ReadToEnd().Trim();
                            break;
                        case "DiscAudioInfo":
                            mDiscAudioInfo = sw.ReadToEnd().Trim();
                            break;
                        case "File":
                            mFileInfo = sw.ReadToEnd().Trim();
                            break;
                        case "FileAudioInfo":
                            mFileAudioInfo = sw.ReadToEnd().Trim();
                            break;
                        case "GeneralInfo":
                            mGeneralInfo = sw.ReadToEnd().Trim();
                            break;
                        case "FileVideoInfo":
                            mFileVideoInfo = sw.ReadToEnd().Trim();
                            break;
                        case "DiscVideoInfo":
                            mDiscVideoInfo = sw.ReadToEnd().Trim();
                            break;
                    }
                }
            }
        }

        public void SetFullScreenshot(bool arg)
        {
            if (arg)
            {
                this.mFileInfo = Regex.Replace(this.mFileInfo, "%ScreenshotForums%", "%ScreenshotFull%", RegexOptions.IgnoreCase);
                this.mDiscInfo = Regex.Replace(this.mDiscInfo, "%ScreenshotForums%", "%ScreenshotFull%", RegexOptions.IgnoreCase);
            }
            else
            {
                this.mFileInfo = Regex.Replace(this.mFileInfo, "%ScreenshotFull%", "%ScreenshotForums%", RegexOptions.IgnoreCase);
                this.mDiscInfo = Regex.Replace(this.mDiscInfo, "%ScreenshotFull%", "%ScreenshotForums%", RegexOptions.IgnoreCase);
            }
        }

        public void CreateInfo()
        {
            string pattern = "";

            if (TorrentInfo.Media != null)
            {
                if (TorrentInfo.Media.MediaTypeChoice == MediaType.MediaDisc)
                {
                    pattern = CreateDiscInfo(TorrentInfo.Media);
                }
                else
                {
                    pattern = CreateFileInfo(TorrentInfo.Media);
                }

                pattern = GetSourceInfo(pattern, TorrentInfo.Media);
                pattern = Regex.Replace(pattern, "%NewLine%", Environment.NewLine);

                PublishInfo = pattern.Trim();
            }
        }

        private string GetScreenshotInfo(string pattern, MediaFile mf)
        {
            StringBuilder sbLinksFull = new StringBuilder();
            StringBuilder sbLinksThumbs = new StringBuilder();

            foreach (Screenshot ss in mf.Thumbnailer.Screenshots)
            {
                if (!string.IsNullOrEmpty(ss.FullImageLink))
                {
                    sbLinksFull.AppendLine(string.Format("[img]{0}[/img]", ss.FullImageLink));
                }
                if (!string.IsNullOrEmpty(ss.LinkedThumbnail))
                {
                    sbLinksThumbs.AppendLine(string.Format("[img]{0}[/img]", ss.LinkedThumbnail));
                }
            }
            pattern = Regex.Replace(pattern, "%ScreenshotFull%", sbLinksFull.ToString().Trim(), RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%ScreenshotForums%", sbLinksThumbs.ToString().Trim(), RegexOptions.IgnoreCase);
            return pattern;
        }

        private string GetGeneralInfo(string pattern, MediaFile mf)
        {
            pattern = Regex.Replace(pattern, "%Format%", mf.Format, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Bitrate%", mf.BitrateOverall, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%FileSize%", mf.FileSizeString, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Subtitles%", mf.Subtitles, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Duration%", mf.DurationString2, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%EncodedApplication%", mf.EncodedApplication, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%EncodedDate%", mf.EncodedDate, RegexOptions.IgnoreCase);

            pattern = this.MappingHelperMy.ReplacePatternGeneral(pattern);

            return pattern;
        }

        private string GetVideoInfo(string pattern, MediaFile mf)
        {
            pattern = Regex.Replace(pattern, "%Video_Codec%", mf.Video.Codec, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_Format%", mf.Video.Format, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_Bitrate%", mf.Video.Bitrate, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_Standard%", mf.Video.Standard, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_FrameRate%", mf.Video.FrameRate, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_ScanType%", mf.Video.ScanType, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_BitsPerPixelFrame%", mf.Video.BitsPerPixelXFrame, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_DisplayAspectRatio%", mf.Video.DisplayAspectRatio, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_Width%", mf.Video.Width, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_Height%", mf.Video.Height, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Video_EncodedLibrarySettings%", mf.Video.EncodedLibrarySettings, RegexOptions.IgnoreCase);

            pattern = this.MappingHelperMy.ReplacePatternVideo(pattern);

            return pattern;
        }

        private string GetAudioInfo(string pattern, MediaFile mf)
        {
            StringBuilder sbAudio = new StringBuilder();

            for (int i = 0; i < mf.Audio.Count; i++)
            {
                string info = pattern;
                AudioInfo ai = mf.Audio[i];
                info = Regex.Replace(info, "%AudioID%", (i + 1).ToString(), RegexOptions.IgnoreCase);
                info = GetStringFromAudio(info, ai);
                sbAudio.AppendLine(info);
            }

            return sbAudio.ToString();
        }

        private string GetStringFromAudio(string pattern, AudioInfo ai)
        {
            pattern = Regex.Replace(pattern, "%Audio_Format%", ai.Format, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Audio_%Format%", ai.Format, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Audio_Bitrate%", ai.Bitrate, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Audio_BitrateMode%", ai.BitrateMode, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Audio_Channels%", ai.Channels, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Audio_SamplingRate%", ai.SamplingRate, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Audio_Resolution%", ai.Resolution, RegexOptions.IgnoreCase);

            pattern = this.MappingHelperMy.ReplacePatternAudio(ai.Index, pattern);

            return pattern;
        }

        private string CreateFileInfo(MediaInfo2 mi)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < mi.MediaFiles.Count; i++)
            {
                MediaFile mf = mi.MediaFiles[i];

                string pattern = mFileInfo;

                string gi = GetGeneralInfo(mGeneralInfo, mf);
                string vi = GetVideoInfo(mFileVideoInfo, mf); // this is our %Video_Info%
                string ai = GetAudioInfo(mFileAudioInfo, mf); // this is our %Audio_Info%

                pattern = Regex.Replace(pattern, "%General_Info%", gi, RegexOptions.IgnoreCase);
                pattern = Regex.Replace(pattern, "%Video_Info%", vi, RegexOptions.IgnoreCase);
                pattern = Regex.Replace(pattern, "%Audio_Info%", ai, RegexOptions.IgnoreCase);

                pattern = GetStringFromAnyPattern(pattern, mf);
                pattern = GetStyles(pattern); // apply any formatting
                pattern = GetScreenshotInfo(pattern, mf);

                sb.AppendLine(pattern);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the string replacing all the syntax supported.
        /// Audio syntax defaults to first audio stream.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="mf"></param>
        /// <returns></returns>
        private string GetStringFromAnyPattern(string pattern, MediaFile mf)
        {
            pattern = GetGeneralInfo(pattern, mf);
            pattern = GetVideoInfo(pattern, mf);
            if (mf.Audio.Count > 0)
            {
                pattern = GetStringFromAudio(pattern, mf.Audio[0]);
            }
            pattern = Regex.Replace(pattern, "%FileName%", mf.FileName);

            return pattern;
        }

        private string GetSourceInfo(string pattern, MediaInfo2 mi)
        {
            if (!string.IsNullOrEmpty(mi.Title))
            {
                pattern = Regex.Replace(pattern, "%Title%", mi.Title, RegexOptions.IgnoreCase);
            }
            pattern = Regex.Replace(pattern, "%Source%", mi.Source, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Disc_Menu%", mi.Menu, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Disc_Extras%", mi.Extras, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%Disc_Authoring%", mi.Authoring, RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%WebLink%", mi.WebLink, RegexOptions.IgnoreCase);

            return pattern;
        }

        private string CreateDiscInfo(MediaInfo2 mi)
        {
            string pattern = mDiscInfo;

            if (mi.Overall != null)
            {
                string gi = GetGeneralInfo(mGeneralInfo, mi.Overall);
                string vi = GetVideoInfo(mDiscVideoInfo, mi.Overall);
                string ai = GetAudioInfo(mDiscAudioInfo, mi.Overall);

                pattern = Regex.Replace(pattern, "%General_Info%", gi, RegexOptions.IgnoreCase);
                pattern = Regex.Replace(pattern, "%Video_Info%", vi, RegexOptions.IgnoreCase);
                pattern = Regex.Replace(pattern, "%Audio_Info%", ai, RegexOptions.IgnoreCase);

                pattern = GetStringFromAnyPattern(pattern, mi.Overall);
                pattern = GetStyles(pattern); // apply any formatting
                foreach (MediaFile mf in mi.MediaFiles)
                {
                    if (mf.Thumbnailer != null)
                    {
                        pattern = GetScreenshotInfo(pattern, mf);
                        break;
                    }
                }
            }
            return pattern;
        }

        private string GetStyles(string pattern)
        {
            int fontSizeHeading1 = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeHeading1 + Program.Settings.FontSizeIncr : Program.Settings.FontSizeHeading1);

            int fontSizeHeading2 = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeHeading2 + Program.Settings.FontSizeIncr : Program.Settings.FontSizeHeading2);

            int fontSizeHeading3 = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeHeading3 + Program.Settings.FontSizeIncr : Program.Settings.FontSizeHeading3);

            int fontSizeBody = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeBody + Program.Settings.FontSizeIncr : Program.Settings.FontSizeBody);

            pattern = Regex.Replace(pattern, "%FontSize_Body%", fontSizeBody.ToString(), RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%FontSize_Heading1%", fontSizeHeading1.ToString(), RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%FontSize_Heading2%", fontSizeHeading2.ToString(), RegexOptions.IgnoreCase);
            pattern = Regex.Replace(pattern, "%FontSize_Heading3%", fontSizeHeading3.ToString(), RegexOptions.IgnoreCase);

            return pattern;
        }
    }
}