using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TDMakerLib
{
    /// <summary>
    /// Class responsible for reading Template Directories.
    /// A Template Directory has 7 files:
    /// GeneralInfo.txt; VideoInfo.txt; AudioInfo.txt; Disc.txt; File.txt
    /// </summary>
    public class TemplateReader
    {
        /// <summary>
        /// Location of the Template
        /// </summary>
        public string Location { get; private set; }

        public TaskSettings TorrentInfo { get; private set; }

        public string PublishInfo { get; private set; }

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
        public TemplateReader(string loc, TaskSettings ts)
        {
            Location = loc;
            this.TorrentInfo = ts;
            TorrentInfo.Media.Info = new MappingHelper(ts.Media.Overall.Summary);
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
                this.mFileInfo = Regex.Replace(this.mFileInfo, "%ScreenshotForums%", "%ScreenshotFull%");
                this.mDiscInfo = Regex.Replace(this.mDiscInfo, "%ScreenshotForums%", "%ScreenshotFull%");
            }
            else
            {
                this.mFileInfo = Regex.Replace(this.mFileInfo, "%ScreenshotFull%", "%ScreenshotForums%");
                this.mDiscInfo = Regex.Replace(this.mDiscInfo, "%ScreenshotFull%", "%ScreenshotForums%");
            }
        }

        public void CreateInfo(PublishOptions options)
        {
            SetFullScreenshot(options.FullPicture);

            string pattern = "";

            if (TorrentInfo.Media != null)
            {
                if (TorrentInfo.MediaOptions.MediaTypeChoice == MediaType.MediaDisc)
                {
                    pattern = CreateDiscInfo(TorrentInfo.Media);
                }
                else
                {
                    pattern = CreateFileInfo(TorrentInfo.Media);
                }

                pattern = GetSourceInfo(pattern, TorrentInfo.Media);
                pattern = pattern.ReplaceCode("%NewLine%", Environment.NewLine);

                if (string.IsNullOrEmpty(pattern)) return;

                pattern = Regex.Replace(pattern, ".*?%.+?%.*?\\r\\n", "");

                PublishInfo = pattern.Trim();
            }
        }

        private string GetScreenshotInfo(string pattern, MediaFile mf)
        {
            StringBuilder sbLinksFull = new StringBuilder();
            StringBuilder sbLinksThumbs = new StringBuilder();

            if (mf.Screenshots.Count > 0)
            {
                foreach (ScreenshotInfo ss in mf.Screenshots)
                {
                    if (!string.IsNullOrEmpty(ss.FullImageLink))
                    {
                        sbLinksFull.AppendLine(string.Format("[img]{0}[/img]", ss.FullImageLink));
                        sbLinksFull.AppendLine();
                    }
                    if (!string.IsNullOrEmpty(ss.LinkedThumbnail))
                    {
                        sbLinksThumbs.AppendLine(string.Format("[img]{0}[/img]", ss.LinkedThumbnail));
                        sbLinksFull.AppendLine();
                    }
                }
                pattern = pattern.ReplaceCode("%ScreenshotFull%", sbLinksFull.ToString().Trim());
                pattern = pattern.ReplaceCode("%ScreenshotForums%", sbLinksThumbs.ToString().Trim());
            }

            return pattern;
        }

        private string GetGeneralInfo(string pattern, MediaFile mf)
        {
            if (string.IsNullOrEmpty(pattern)) return null;

            pattern = pattern.ReplaceCode("%Format%", mf.Format);
            pattern = pattern.ReplaceCode("%Bitrate%", mf.BitrateOverall);
            pattern = pattern.ReplaceCode("%FileSize%", mf.FileSizeString);
            pattern = pattern.ReplaceCode("%Subtitles%", mf.Subtitles);
            pattern = pattern.ReplaceCode("%Duration%", mf.DurationString2);
            pattern = pattern.ReplaceCode("%EncodedApplication%", mf.EncodedApplication);
            pattern = pattern.ReplaceCode("%EncodedDate%", mf.EncodedDate);

            pattern = TorrentInfo.Media.Info.ReplacePatternGeneral(pattern);

            return pattern;
        }

        private string GetVideoInfo(string pattern, MediaFile mf)
        {
            if (string.IsNullOrEmpty(pattern)) return null;

            pattern = pattern.ReplaceCode("%Video_Codec%", mf.Video.Codec);
            pattern = pattern.ReplaceCode("%Video_Format%", mf.Video.Format);
            pattern = pattern.ReplaceCode("%Video_Bitrate%", mf.Video.Bitrate);
            pattern = pattern.ReplaceCode("%Video_Standard%", mf.Video.Standard);
            pattern = pattern.ReplaceCode("%Video_FrameRate%", mf.Video.FrameRate);
            pattern = pattern.ReplaceCode("%Video_ScanType%", mf.Video.ScanType);
            pattern = pattern.ReplaceCode("%Video_BitsPerPixelFrame%", mf.Video.BitsPerPixelXFrame);
            pattern = pattern.ReplaceCode("%Video_DisplayAspectRatio%", mf.Video.DisplayAspectRatio);
            pattern = pattern.ReplaceCode("%Video_Width%", mf.Video.Width);
            pattern = pattern.ReplaceCode("%Video_Height%", mf.Video.Height);
            pattern = pattern.ReplaceCode("%Video_Resolution%", mf.Video.Resolution);
            pattern = pattern.ReplaceCode("%Video_EncodedLibrarySettings%", mf.Video.EncodedLibrarySettings);

            pattern = TorrentInfo.Media.Info.ReplacePatternVideo(pattern);

            return pattern;
        }

        private string GetAudioInfo(string pattern, MediaFile mf)
        {
            if (string.IsNullOrEmpty(pattern)) return null;

            StringBuilder sbAudio = new StringBuilder();

            for (int i = 0; i < mf.Audio.Count; i++)
            {
                string info = pattern;
                AudioInfo ai = mf.Audio[i];
                info = info.ReplaceCode("%AudioID%", (i + 1).ToString());
                info = GetStringFromAudio(info, ai);
                sbAudio.AppendLine(info);
            }

            return sbAudio.ToString();
        }

        private string GetStringFromAudio(string pattern, AudioInfo ai)
        {
            if (string.IsNullOrEmpty(pattern)) return null;

            pattern = pattern.ReplaceCode("%Audio_Format%", ai.Format);
            pattern = pattern.ReplaceCode("%Audio_%Format%", ai.Format);
            pattern = pattern.ReplaceCode("%Audio_Bitrate%", ai.Bitrate);
            pattern = pattern.ReplaceCode("%Audio_BitrateMode%", ai.BitrateMode);
            pattern = pattern.ReplaceCode("%Audio_Channels%", ai.Channels);
            pattern = pattern.ReplaceCode("%Audio_SamplingRate%", ai.SamplingRate);
            pattern = pattern.ReplaceCode("%Audio_Resolution%", ai.Resolution);

            pattern = TorrentInfo.Media.Info.ReplacePatternAudio(ai.Index, pattern);

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

                pattern = pattern.ReplaceCode("%General_Info%", gi);
                pattern = pattern.ReplaceCode("%Video_Info%", vi);
                pattern = pattern.ReplaceCode("%Audio_Info%", ai);

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
            pattern = pattern.ReplaceCode("%FileName%", mf.FileName);

            return pattern;
        }

        private string GetSourceInfo(string pattern, MediaInfo2 mi)
        {
            try
            {
                pattern = pattern.ReplaceCode("%Title%", mi.Title);
                pattern = pattern.ReplaceCode("%Source%", mi.Source);
                pattern = pattern.ReplaceCode("%Disc_Menu%", mi.Menu);
                pattern = pattern.ReplaceCode("%Disc_Extras%", mi.Extras);
                pattern = pattern.ReplaceCode("%Disc_Authoring%", mi.Authoring);
                pattern = pattern.ReplaceCode("%WebLink%", mi.WebLink);
            }
            catch (Exception)
            {
            }

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

                pattern = pattern.ReplaceCode("%General_Info%", gi);
                pattern = pattern.ReplaceCode("%Video_Info%", vi);
                pattern = pattern.ReplaceCode("%Audio_Info%", ai);

                pattern = GetStringFromAnyPattern(pattern, mi.Overall);
                pattern = GetStyles(pattern); // apply any formatting
                foreach (MediaFile mf in mi.MediaFiles)
                {
                    if (mf.Screenshots.Count > 0)
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
            int fontSizeHeading1 = (int)(App.Settings.ProfileActive.PreText && App.Settings.ProfileActive.LargerPreText == true ?
                App.Settings.ProfileActive.FontSizeHeading1 + App.Settings.ProfileActive.FontSizeIncr : App.Settings.ProfileActive.FontSizeHeading1);

            int fontSizeHeading2 = (int)(App.Settings.ProfileActive.PreText && App.Settings.ProfileActive.LargerPreText == true ?
                App.Settings.ProfileActive.FontSizeHeading2 + App.Settings.ProfileActive.FontSizeIncr : App.Settings.ProfileActive.FontSizeHeading2);

            int fontSizeHeading3 = (int)(App.Settings.ProfileActive.PreText && App.Settings.ProfileActive.LargerPreText == true ?
                App.Settings.ProfileActive.FontSizeHeading3 + App.Settings.ProfileActive.FontSizeIncr : App.Settings.ProfileActive.FontSizeHeading3);

            int fontSizeBody = (int)(App.Settings.ProfileActive.PreText && App.Settings.ProfileActive.LargerPreText == true ?
                App.Settings.ProfileActive.FontSizeBody + App.Settings.ProfileActive.FontSizeIncr : App.Settings.ProfileActive.FontSizeBody);

            pattern = pattern.ReplaceCode("%FontSize_Body%", fontSizeBody.ToString());
            pattern = pattern.ReplaceCode("%FontSize_Heading1%", fontSizeHeading1.ToString());
            pattern = pattern.ReplaceCode("%FontSize_Heading2%", fontSizeHeading2.ToString());
            pattern = pattern.ReplaceCode("%FontSize_Heading3%", fontSizeHeading3.ToString());

            return pattern;
        }
    }
}