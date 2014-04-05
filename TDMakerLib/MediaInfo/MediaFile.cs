using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TDMakerLib;

namespace TDMakerLib
{
    /// <summary>
    /// Class that holds all the MediaInfo of a media file
    /// This can be a single .vob, .avi etc.
    /// </summary>
    public class MediaFile
    {
        public int Index { get; set; }

        public bool HasAudio { get; set; }

        public bool HasVideo { get; set; }

        // General
        public string EncodedDate { get; set; }

        public string EncodedApplication { get; set; }

        public string BitrateOverall { get; set; }

        /// <summary>
        /// Duration in milli seconds
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Segment duration in milli seconds
        /// </summary>
        public double SegmentDuration { get; set; }

        /// <summary>
        /// Duration in hours:minutes:seconds
        /// </summary>
        public string DurationString2 { get; set; }

        public string DurationString3 { get; set; }

        public string FileExtension { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Always a File Path of Media
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// File Size in Bytes
        /// </summary>
        public double FileSize { get; set; }

        /// <summary>
        /// File Size in XX.X MiB
        /// </summary>
        public string FileSizeString { get; set; }

        public string Format { get; set; }

        public string FormatInfo { get; set; }

        public Thumbnailer Thumbnailer = null;

        public string Source { get; set; }

        private string mSubtitles = "None";

        public string Subtitles { get { return mSubtitles; } set { mSubtitles = value; } }

        /// <summary>
        /// This is what you get for mi.Option("Complete") using MediaInfo
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// This is what you get for mi.Option("Complete", "1") using MediaInfo
        /// </summary>
        public string SummaryComplete { get; set; }

        public TagLib.File TagLibFile { get; set; }

        public List<AudioInfo> Audio { get; set; }

        public VideoInfo Video { get; set; }

        /// <summary>
        /// Constructor for Media File
        /// </summary>
        /// <param name="fp">File Path of the Media File</param>
        /// <param name="src">Source of the Media File (HDTV, DVD, BD, etc.)</param>
        public MediaFile(string fp, string src)
        {
            this.FilePath = fp;
            this.FileExtension = Path.GetExtension(fp);
            this.FileName = Path.GetFileName(fp);
            this.Source = src;

            this.Audio = new List<AudioInfo>();
            this.Video = new VideoInfo();
            this.ReadFile();
        }

        /// <summary>
        /// Reads a media file and creates a MediaFile object
        /// </summary>
        /// <param name="fp">File Path of the Media File</param>
        /// <returns>MediaFile object</returns>
        public void ReadFile()
        {
            this.Source = Source;

            if (File.Exists(FilePath))
            {
                //*********************
                //* General
                //*********************

                //System.Debug.WriteLine("Current Dir1: " + System.Environment.CurrentDirectory);
                //System.Environment.CurrentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //System.Debug.WriteLine("Current Dir2: " + System.Environment.CurrentDirectory);

                MediaInfoLib.MediaInfo MI = null;
                try
                {
                    Debug.WriteLine("Loading MediaInfo.dll");
                    MI = new MediaInfoLib.MediaInfo();
                    Debug.WriteLine("Loaded MediaInfo.dll");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                if (MI != null)
                {
                    Debug.WriteLine(string.Format("MediaInfo Opening {0}", FilePath));
                    MI.Open(FilePath);
                    Debug.WriteLine(string.Format("MediaInfo Opened {0}", FilePath));
                    MI.Option("Complete");
                    this.Summary = MI.Inform();
                    MI.Option("Complete", "1");
                    this.SummaryComplete = MI.Inform();

                    if (Program.IsUNIX)
                    {
                        Debug.WriteLine(string.Format("MediaInfo Summary Length: {0}", this.Summary.Length.ToString()));
                        Debug.WriteLine(string.Format("MediaInfo Summary: {0}", this.Summary));
                    }

                    // Format Info
                    if (string.IsNullOrEmpty(this.Format))
                        this.Format = MI.Get(StreamKind.General, 0, "Format");
                    this.FormatInfo = MI.Get(StreamKind.General, 0, "Format/Info");

                    // this.FileName = mMI.Get(0, 0, "FileName");
                    if (0 == this.FileSize)
                    {
                        double sz;
                        double.TryParse(MI.Get(0, 0, "FileSize"), out sz);
                        this.FileSize = sz;
                    }
                    if (string.IsNullOrEmpty(this.FileSizeString))
                    {
                        this.FileSizeString = string.Format("{0} MiB", (this.FileSize / 1024.0 / 1024.0).ToString("0.00"));
                    }

                    // Duration
                    if (string.IsNullOrEmpty(this.DurationString2))
                        this.DurationString2 = MI.Get(0, 0, "Duration/String2");

                    if (this.Duration == 0.0)
                    {
                        double dura = 0.0;
                        double.TryParse(MI.Get(0, 0, "Duration"), out dura);
                        this.Duration = dura;
                        this.SegmentDuration = dura;
                    }

                    if (string.IsNullOrEmpty(this.DurationString3))
                        this.DurationString3 = MI.Get(0, 0, "Duration/String3");

                    this.BitrateOverall = MI.Get(StreamKind.General, 0, "OverallBitRate/String");
                    this.EncodedApplication = MI.Get(StreamKind.General, 0, "Encoded_Application");
                    this.EncodedDate = MI.Get(StreamKind.General, 0, "Encoded_Date");

                    if (string.IsNullOrEmpty(this.Subtitles))
                    {
                        StringBuilder sbSubs = new StringBuilder();

                        int subCount = 0;
                        int.TryParse(MI.Get(StreamKind.Text, 0, "StreamCount"), out subCount);

                        if (subCount > 0)
                        {
                            StringBuilder sbLang = new StringBuilder();
                            for (int i = 0; i < subCount; i++)
                            {
                                string lang = MI.Get(StreamKind.Text, i, "Language/String");
                                if (!string.IsNullOrEmpty(lang))
                                {
                                    // System.Windows.Forms.MessageBox.Show(lang);
                                    sbLang.Append(lang);
                                    if (i < subCount - 1)
                                        sbLang.Append(", ");
                                }
                            }
                            if (!string.IsNullOrEmpty(sbLang.ToString()))
                            {
                                sbSubs.Append(sbLang.ToString());
                            }
                            else
                            {
                                sbSubs.Append("N/A");
                            }
                        }
                        else
                        {
                            sbSubs.Append("None");
                        }

                        this.Subtitles = sbSubs.ToString();
                    }

                    //*********************
                    //* Video
                    //*********************

                    int videoCount;
                    int.TryParse(MI.Get(StreamKind.General, 0, "VideoCount"), out videoCount);
                    this.HasVideo = videoCount > 0;

                    this.Video.Format = MI.Get(StreamKind.Video, 0, "Format");
                    this.Video.FormatVersion = MI.Get(StreamKind.Video, 0, "Format_Version");

                    if (Path.GetExtension(this.FilePath).ToLower().Equals(".mkv"))
                    {
                        this.Video.Codec = MI.Get(StreamKind.Video, 0, "Encoded_Library");
                    }
                    this.Video.EncodedLibrarySettings = MI.Get(StreamKind.Video, 0, "Encoded_Library_Settings");
                    this.Video.DisplayAspectRatio = MI.Get(StreamKind.Video, 0, "DisplayAspectRatio/String");

                    if (string.IsNullOrEmpty(this.Video.Codec))
                        this.Video.Codec = MI.Get(StreamKind.Video, 0, "CodecID/Hint");
                    if (string.IsNullOrEmpty(this.Video.Codec))
                        this.Video.Codec = MI.Get(StreamKind.Video, 0, "CodecID");

                    this.Video.Bitrate = MI.Get(StreamKind.Video, 0, "BitRate/String");
                    this.Video.Standard = MI.Get(StreamKind.Video, 0, "Standard"); ;
                    this.Video.FrameRate = MI.Get(StreamKind.Video, 0, "FrameRate/String");
                    this.Video.ScanType = MI.Get(StreamKind.Video, 0, "ScanType/String");
                    this.Video.Height = MI.Get(StreamKind.Video, 0, "Height");
                    this.Video.Width = MI.Get(StreamKind.Video, 0, "Width");
                    this.Video.Resolution = string.Format("{0}x{1}", this.Video.Width, this.Video.Height);
                    this.Video.BitsPerPixelXFrame = MI.Get(StreamKind.Video, 0, "Bits-(Pixel*Frame)");

                    //*********************
                    //* Audio
                    //*********************
                    int audioCount;
                    int.TryParse(MI.Get(StreamKind.General, 0, "AudioCount"), out audioCount);
                    this.HasAudio = audioCount > 0;

                    for (int id = 0; id < audioCount; id++)
                    {
                        AudioInfo ai = new AudioInfo(id);
                        ai.Format = MI.Get(StreamKind.Audio, id, "Format");
                        ai.FormatVersion = MI.Get(StreamKind.Audio, 0, "Format_Version");
                        ai.FormatProfile = MI.Get(StreamKind.Audio, 0, "Format_Profile");

                        ai.Codec = MI.Get(StreamKind.Audio, 0, "CodecID/Hint");
                        if (string.IsNullOrEmpty(ai.Codec))
                            ai.Codec = MI.Get(StreamKind.Audio, 0, "CodecID/Info");
                        if (string.IsNullOrEmpty(ai.Codec))
                            ai.Codec = MI.Get(StreamKind.Audio, 0, "CodecID");

                        ai.Bitrate = MI.Get(StreamKind.Audio, id, "BitRate/String");
                        ai.BitrateMode = MI.Get(StreamKind.Audio, id, "BitRate_Mode/String");

                        ai.Channels = MI.Get(StreamKind.Audio, id, "Channel(s)/String");
                        ai.SamplingRate = MI.Get(StreamKind.Audio, id, "SamplingRate/String");
                        ai.Resolution = MI.Get(StreamKind.Audio, id, "Resolution/String");

                        this.Audio.Add(ai);
                    }

                    MI.Close();

                    //// Analyse Audio only files using TagLib

                    //if (this.HasAudio && !this.HasVideo)
                    //{
                    //    TagLib.File f = TagLib.File.Create(this.FilePath);
                    //    this.TagLibFile = f;
                    //}
                }
            }
        }

        /// <summary>
        /// Method to determine if the Media File is actually an audio file
        /// </summary>
        /// <returns></returns>
        public bool IsAudioFile()
        {
            return this.HasAudio && !this.HasVideo;
        }

        public override string ToString()
        {
            return this.FileName;
        }

        /// <summary>
        /// String representation of an Audio file
        /// </summary>
        /// <returns></returns>
        public string ToStringAudio()
        {
            return "";
        }

        /// <summary>
        /// Returns a Publish layout of Media Info that has Audio and Video
        /// </summary>
        /// <returns></returns>
        public string ToStringPublish(PublishOptionsPacket pop)
        {
            int fontSizeHeading3 = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
           Program.Settings.FontSizeHeading3 + Program.Settings.FontSizeIncr :
           Program.Settings.FontSizeHeading3);

            int fontSizeBody = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeBody + Program.Settings.FontSizeIncr :
                Program.Settings.FontSizeBody);

            StringBuilder sbBody = new StringBuilder();

            //*********************
            //* General
            //*********************
            StringBuilder sbGeneral = new StringBuilder();

            sbBody.AppendLine(BbCode.Size(fontSizeHeading3, BbCode.BoldItalic("General:")));
            sbBody.AppendLine();

            // Format
            sbGeneral.Append(string.Format("            [u]Format:[/u] {0}", this.Format));
            if (!string.IsNullOrEmpty(this.FormatInfo))
            {
                sbGeneral.Append(string.Format(" ({0})", this.FormatInfo));
            }
            sbGeneral.Append(Environment.NewLine);

            // File Size
            sbGeneral.AppendLine(string.Format("         [u]File Size:[/u] {0}", this.FileSizeString));

            // Duration
            sbGeneral.AppendLine(string.Format("          [u]Duration:[/u] {0}", this.DurationString2));

            // Bitrate
            sbGeneral.AppendLine(string.Format("           [u]Bitrate:[/u] {0}", this.BitrateOverall));

            // Subtitles
            if (!string.IsNullOrEmpty(this.Subtitles))
            {
                sbGeneral.AppendLine(string.Format("         [u]Subtitles:[/u] {0}", this.Subtitles));
            }

            sbBody.AppendLine(BbCode.Size(fontSizeBody, sbGeneral.ToString()));

            if (this.Thumbnailer != null)
            {
                string ss = this.GetScreenshotString(pop);
                if (ss.Length > 0)
                {
                    sbBody.AppendLine(this.GetScreenshotString(pop));
                }
            }

            //*********************
            //* Video
            //*********************
            VideoInfo vi = this.Video;

            sbBody.AppendLine();
            sbBody.AppendLine(BbCode.Size(fontSizeHeading3, BbCode.BoldItalic("Video:")));
            sbBody.AppendLine();

            StringBuilder sbVideo = new StringBuilder();

            // Format
            sbVideo.Append(string.Format("              [u]Format:[/u] {0}", this.Video.Format));
            if (!string.IsNullOrEmpty(this.Video.FormatVersion))
            {
                sbVideo.Append(string.Format(" {0}", this.Video.FormatVersion));
            }
            sbVideo.Append(Environment.NewLine);

            // Codec
            if (!string.IsNullOrEmpty(vi.Codec))
                sbVideo.AppendLine(string.Format("               [u]Codec:[/u] {0}", vi.Codec));

            // Bitrate
            sbVideo.AppendLine(string.Format("             [u]Bitrate:[/u] {0}", this.Video.Bitrate));

            // Standard
            if (!string.IsNullOrEmpty(vi.Standard))
                sbVideo.AppendLine(string.Format("            [u]Standard:[/u] {0}", this.Video.Standard));

            // Frame Rate
            sbVideo.AppendLine(string.Format("          [u]Frame Rate:[/u] {0}", vi.FrameRate));

            // Scan Type
            sbVideo.AppendLine(string.Format("           [u]Scan Type:[/u] {0}", vi.ScanType));
            sbVideo.AppendLine(string.Format("  [u]Bits/(Pixel*Frame):[/u] {0}", vi.BitsPerPixelXFrame));
            sbVideo.AppendLine(string.Format("[u]Display Aspect Ratio:[/u] {0}", vi.DisplayAspectRatio));

            // Resolution
            sbVideo.AppendLine(string.Format("          [u]Resolution:[/u] {0}x{1}",
                vi.Width,
                vi.Height));

            sbBody.Append(BbCode.Size(fontSizeBody, sbVideo.ToString()));

            //*********************
            //* Audio
            //*********************

            int audioCount = this.Audio.Count;

            for (int a = 0; a < audioCount; a++)
            {
                AudioInfo ai = this.Audio[a];

                sbBody.AppendLine();
                sbBody.AppendLine(string.Format(BbCode.Size(fontSizeHeading3, BbCode.BoldItalic("Audio #{0}:")), a + 1));
                sbBody.AppendLine();

                StringBuilder sbAudio = new StringBuilder();

                // Format
                sbAudio.Append(string.Format("            [u]Format:[/u] {0}", ai.Format));
                if (!string.IsNullOrEmpty(ai.FormatVersion))
                    sbAudio.Append(string.Format(" {0}", ai.FormatVersion));
                if (!string.IsNullOrEmpty(ai.FormatProfile))
                    sbAudio.Append(string.Format(" {0}", ai.FormatProfile));
                sbAudio.Append(Environment.NewLine);

                // Codec
                if (!string.IsNullOrEmpty(ai.Codec))
                    sbAudio.AppendLine(string.Format("             [u]Codec:[/u] {0}", ai.Codec));

                // Bitrate
                sbAudio.AppendLine(string.Format("           [u]Bitrate:[/u] {0} ({1})", ai.Bitrate, ai.BitrateMode));

                // Channels
                sbAudio.AppendLine(string.Format("          [u]Channels:[/u] {0}", ai.Channels));

                // Sampling Rate
                sbAudio.AppendLine(string.Format("     [u]Sampling Rate:[/u] {0}", ai.SamplingRate));

                // Resolution
                if (!string.IsNullOrEmpty(ai.Resolution))
                    sbAudio.AppendLine(string.Format(("        [u]Resolution:[/u] {0}"), ai.Resolution));

                sbBody.Append(BbCode.Size(fontSizeBody, sbAudio.ToString()));
                sbBody.AppendLine();
            }

            return sbBody.ToString();
        }

        public string GetScreenshotString(PublishOptionsPacket pop)
        {
            StringBuilder sbPublish = new StringBuilder();

            foreach (Screenshot ss in this.Thumbnailer.Screenshots)
            {
                if (!string.IsNullOrEmpty(ss.FullImageLink) && pop.FullPicture)
                {
                    sbPublish.AppendLine(BbCode.Img(ss.FullImageLink));
                }
                else if (!string.IsNullOrEmpty(ss.LinkedThumbnail))
                {
                    sbPublish.AppendLine(ss.LinkedThumbnail);
                }
                sbPublish.AppendLine();
            }

            return sbPublish.ToString();
        }

        public string GetMTNString()
        {
            /*
               File: ITS Demo.wmv
               Size: 15.86 MiB, Duration: 3mn 18s, Average Bitrate: 669 Kbps
               Video: VC-1, 1024x576, 582 Kbps, 25.000 fps
               Audio 1: WMA, 44.1 KHz, 2 channels, 75.1 Kbps
            */

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("File: {0}", this.FileName));
            sb.AppendLine(string.Format("Size: {0}, Duration: {1}, Average bitrate: {2}", this.FileSizeString, this.DurationString2, this.BitrateOverall));
            sb.AppendLine(string.Format("Video: {0}, {1}, {2}, {3}", this.Video.Codec, this.Video.Resolution, this.Video.Bitrate, this.Video.FrameRate));
            int aiCount = 1;
            foreach (AudioInfo ai in this.Audio)
            {
                sb.AppendLine(string.Format("Audio {0}: {1}, {2}, {3}, {4}", aiCount++, ai.Codec, ai.SamplingRate, ai.Channels, ai.Bitrate));
            }

            return sb.ToString();
        }
    }

    public class Info
    {
        public string Bitrate { get; set; }

        public string Codec { get; set; }

        public string Standard { get; set; }

        public string Format { get; set; }

        public string FormatProfile { get; set; }

        public string FormatVersion { get; set; }

        public string Resolution { get; set; }

        public Info()
        {
            this.Resolution = "Unknown";
        }
    }

    public class AudioInfo : Info
    {
        //
        public int Index { get; set; }

        public string BitrateMode { get; set; }

        public string Channels { get; set; }

        public string SamplingRate { get; set; }

        public AudioInfo(int id)
        {
            this.Index = id;
        }
    }

    public class VideoInfo : Info
    {
        public string FrameRate { get; set; }

        public string Height { get; set; }

        public string ScanType { get; set; }

        public string Width { get; set; }

        public string BitsPerPixelXFrame { get; set; }

        public string DisplayAspectRatio { get; set; }

        public string EncodedLibrarySettings { get; set; }
    }
}