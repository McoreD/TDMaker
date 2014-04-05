using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TDMakerLib;
using TDMakerLib.MediaInfo;

namespace TDMakerLib
{
    /// <summary>
    /// Class that holds all Media Info: Genernal, Audio 1 to n, Video
    /// </summary>
    public class MediaInfo2
    {
        /// <summary>
        /// Disc property is set to true if the media is found to be DVD, BD, HDDVD source
        /// </summary>
        public MediaType MediaTypeChoice { get; private set; }

        public SourceType DiscType { get; set; }

        /// <summary>
        /// Packet that contains Tracker Information
        /// </summary>
        public TorrentCreateInfo TorrentCreateInfoMy { get; set; }

        /// <summary>
        /// FilePath or DirectoryPath of the Media
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Check here Screenshots were taken
        /// </summary>
        public bool UploadScreenshots { get; set; }

        /// <summary>
        /// Title is same as Overall.FileName
        /// </summary>
        public string Title { get; private set; }

        public string Source { get; set; }

        public string Authoring { get; set; }

        public string Menu { get; set; }

        public string Extras { get; set; }

        public string WebLink { get; set; }

        public string ReleaseDescription { get; set; }

        public MediaFile Overall { get; set; }

        public List<MediaFile> MediaFiles { get; private set; }

        public List<string> FileCollection { get; set; }

        /// <summary>
        /// Folder Path of the Template
        /// </summary>
        public string TemplateLocation { get; set; }

        public MediaInfo2(MediaType mediaType, string loc)
        {
            this.MediaTypeChoice = mediaType;
            FileCollection = new List<string>();
            MediaFiles = new List<MediaFile>();
            Overall = new MediaFile(loc, this.Source);

            // this could be a file path or a directory
            this.Location = loc;
        }

        /// <summary>
        /// Function to override Title
        /// </summary>
        /// <param name="title"></param>
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Add Media to the Media List if the file has Audio or Video
        /// </summary>
        /// <param name="mf"></param>
        private void AddMedia(MediaFile mf)
        {
            if (mf.HasVideo || mf.HasAudio)
            {
                mf.Index = MediaFiles.Count + 1;
                this.MediaFiles.Add(mf);
            }
        }

        private void ReadMediaFile()
        {
            if (File.Exists(Location))
            {
                this.Overall = new MediaFile(this.Location, this.Source);
                if (string.IsNullOrEmpty(Title))
                    this.Title = Path.GetFileNameWithoutExtension(this.Overall.FilePath); // this.Overall.FileName;
                AddMedia(this.Overall);
            }
        }

        /// <summary>
        /// Function to populate MediaInfo class
        /// </summary>
        public void ReadMedia()
        {
            switch (MediaTypeChoice)
            {
                case MediaType.MediaIndiv:
                    if (File.Exists(Location))
                    {
                        ReadMediaFile();
                    }
                    else
                    {
                        ReadDirectory();
                    }
                    break;

                case MediaType.MediaCollection:
                    ReadMediaFileCollection();
                    break;

                case MediaType.MediaDisc:
                case MediaType.MusicAudioAlbum:
                    ReadDirectory();
                    break;
            }
        }

        public List<string> GetFilesToProcess(string dir)
        {
            List<string> filePaths = new List<string>();
            foreach (string ext in Program.Settings.SupportedFileExtVideo)
            {
                filePaths.AddRange(Directory.GetFiles(Location, "*" + ext, SearchOption.AllDirectories));
                Debug.WriteLine("Processing " + ext);
            }

            filePaths.RemoveAll(x => Path.GetFileName(x).ToLower().Contains("sample"));
            return filePaths;
        }

        private void ReadDirectory()
        {
            // if a folder then read all media files
            // append all the durations and save in Duration
            if (Directory.Exists(Location))
            {
                // get largest file
                FileCollection = GetFilesToProcess(Location);
                List<FileInfo> listFileInfo = new List<FileInfo>();
                foreach (string fp in FileCollection)
                {
                    listFileInfo.Add(new FileInfo(fp));
                }

                // Subtitles, Format: DVD Video using VTS_01_0.IFO
                string[] ifo = Directory.GetFiles(Location, "VTS_01_0.IFO", SearchOption.AllDirectories);
                if (ifo.Length == 1) // CHECK IF A DVD
                {
                    this.MediaTypeChoice = MediaType.MediaDisc;
                    MediaInfoLib.MediaInfo mi = new MediaInfoLib.MediaInfo();
                    this.DiscType = TDMakerLib.SourceType.DVD;

                    mi.Open(ifo[0]);

                    // most prolly this will be: DVD Video
                    this.Overall.Format = mi.Get(StreamKind.General, 0, "Format");

                    int subCount = 0;
                    int.TryParse(mi.Get(StreamKind.Text, 0, "StreamCount"), out subCount);

                    if (subCount > 0)
                    {
                        List<string> langs = new List<string>();
                        for (int i = 0; i < subCount; i++)
                        {
                            string lang = mi.Get(StreamKind.Text, i, "Language/String");
                            if (!string.IsNullOrEmpty(lang))
                            {
                                // System.Windows.Forms.MessageBox.Show(lang);
                                if (!langs.Contains(lang))
                                    langs.Add(lang);
                            }
                        }
                        StringBuilder sbLangs = new StringBuilder();
                        for (int i = 0; i < langs.Count; i++)
                        {
                            sbLangs.Append(langs[i]);
                            if (i < langs.Count - 1)
                                sbLangs.Append(", ");
                        }

                        this.Overall.Subtitles = sbLangs.ToString();
                    }

                    // close ifo file
                    mi.Close();

                    // AFTER IDENTIFIED THE MEDIA TYPE IS A DVD
                    listFileInfo.RemoveAll(x => x.Length < 1000000000);
                }

                // Set Media Type
                bool allAudio = Adapter.MediaIsAudio(FileCollection);
                if (allAudio)
                {
                    this.MediaTypeChoice = MediaType.MusicAudioAlbum;
                }

                if (FileCollection.Count > 0)
                {
                    foreach (FileInfo fi in listFileInfo)
                    {
                        this.AddMedia(new MediaFile(fi.FullName, this.Source));
                    }

                    this.Overall = new MediaFile(FileSystemHelper.GetLargestFilePathFromDir(Location), this.Source);

                    // Set Overall File Name
                    this.Overall.FileName = Path.GetFileName(Location);
                    if (Overall.FileName.ToUpper().Equals("VIDEO_TS"))
                        Overall.FileName = Path.GetFileName(Path.GetDirectoryName(Location));
                    if (string.IsNullOrEmpty(Title))
                        this.Title = this.Overall.FileName;
                }

                // DVD Video
                // Combined Duration and File Size
                if (MediaTypeChoice == MediaType.MediaDisc)
                {
                    if (listFileInfo.Count > 0)
                    {
                        long dura = 0;
                        double size = 0;
                        foreach (FileInfo fiVob in listFileInfo)
                        {
                            MediaInfoLib.MediaInfo mi = new MediaInfoLib.MediaInfo();
                            mi.Open(fiVob.FullName);
                            string temp = mi.Get(0, 0, "Duration");
                            if (!string.IsNullOrEmpty(temp))
                            {
                                long d = 0;
                                long.TryParse(temp, out d);
                                dura += d;
                            }

                            // we are interested in combined file size only for VOB files
                            // thats why we dont calculate FileSize in FileInfo while determining largest file
                            double sz = 0;
                            double.TryParse(mi.Get(0, 0, "FileSize"), out sz);
                            size += sz;

                            // close vob file
                            mi.Close();
                        }

                        this.Overall.FileSize = size; // override any previous file size
                        this.Overall.FileSizeString = string.Format("{0} MiB", (this.Overall.FileSize / 1024.0 / 1024.0).ToString("0.00"));

                        this.Overall.Duration = dura;
                        this.Overall.DurationString2 = Program.GetDurationString(dura);
                    }
                }
            } // if Location is a directory
        }

        private void ReadMediaFileCollection()
        {
            if (FileCollection.Count > 0)
            {
                FileCollection.Sort();

                foreach (string p in this.FileCollection)
                {
                    AddMedia(new MediaFile(p, this.Source));
                }
                this.Title = Path.GetFileName(Path.GetDirectoryName(FileCollection[0]));
            }
            else
            {
                ReadDirectory();
            }
        }

        public override string ToString()
        {
            return Path.GetFileName(Location);
        }

        /// <summary>
        /// Tracklist of all the Audio files in the MediaInfo. Also accessible using %Tracklist%
        /// </summary>
        /// <returns>String representation of Tracklist</returns>
        public string ToStringAudio()
        {
            List<string> lstAudioFiles = new List<string>();
            foreach (MediaFile mf in MediaFiles)
            {
                if (mf.IsAudioFile())
                {
                    lstAudioFiles.Add(mf.FilePath);
                }
            }
            return new NfoReport(this.Location, null).ToString();
        }

        public string ToStringMedia(PublishOptionsPacket pop)
        {
            int fontSizeHeading1 = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
        Program.Settings.FontSizeHeading1 + Program.Settings.FontSizeIncr :
        Program.Settings.FontSizeHeading1);

            int fontSizeHeading2 = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeHeading2 + Program.Settings.FontSizeIncr :
                Program.Settings.FontSizeHeading2);

            int fontSizeBody = (int)(Program.Settings.PreText && Program.Settings.LargerPreText == true ?
                Program.Settings.FontSizeBody + Program.Settings.FontSizeIncr :
                Program.Settings.FontSizeBody);

            StringBuilder sbBody = new StringBuilder();

            // Show Title
            if (Program.Settings.bTitle)
            {
                sbBody.AppendLine(BbCode.Size(fontSizeHeading1, BbCode.Bold(this.Title)));
                sbBody.AppendLine();
            }

            StringBuilder sbTitleInfo = new StringBuilder();

            // Source
            if (!string.IsNullOrEmpty(this.Source))
            {
                sbTitleInfo.AppendLine(string.Format("[u]Source:[/u] {0}", this.Source));
            }

            if (MediaTypeChoice == MediaType.MediaDisc)
            {
                // Authoring
                if (Program.Settings.bAuthoring && !string.IsNullOrEmpty(this.Authoring))
                {
                    sbTitleInfo.AppendLine(string.Format("[u]Authoring:[/u] {0}", this.Authoring));
                }
                if (Program.Settings.bDiscMenu && !string.IsNullOrEmpty(this.Menu))
                {
                    sbTitleInfo.AppendLine(string.Format("[u]Menu:[/u] {0}", this.Menu));
                }

                // Extras
                if (Program.Settings.bExtras && !string.IsNullOrEmpty(this.Extras))
                {
                    sbTitleInfo.AppendLine(string.Format("[u]Extras:[/u] {0}", this.Extras));
                }

                // WebLink
                if (Program.Settings.bWebLink && !string.IsNullOrEmpty(this.WebLink))
                {
                    sbTitleInfo.AppendLine(string.Format("[u]Web Link:[/u] {0}", this.WebLink));
                }
            }

            if (!string.IsNullOrEmpty(sbTitleInfo.ToString()))
            {
                sbBody.AppendLine(BbCode.Size(fontSizeBody, sbTitleInfo.ToString()));
                sbBody.AppendLine();
            }

            if (this.MediaFiles.Count > 1 && this.MediaTypeChoice == MediaType.MediaDisc)

            // is a DVD so need Overall Info only
            {
                sbBody.AppendLine(this.Overall.ToStringPublish(pop));
            }
            else
            {
                // If the loaded folder is not a Disc but individual ripped files or a collection of files
                if (MediaTypeChoice == MediaType.MediaCollection)
                {
                    sbBody.AppendLine(ToStringMediaList());
                }
                else
                {
                    foreach (MediaFile mf in this.MediaFiles)
                    {
                        sbBody.AppendLine(BbCode.Size(fontSizeHeading2, BbCode.BoldItalic(mf.FileName)));
                        sbBody.AppendLine();
                        sbBody.AppendLine(mf.ToStringPublish(pop));
                    }
                }
            }

            // CREATING XML TORRENT UPLOAD FILE DOES NOT REQUIRE SCREENSHOT IN RELEASE DESCRIPTION
            // THE SCREENSHOTS ARE IN THE XML INSTEAD
            if (this.HasScreenshots())
            {
                sbBody.AppendLine();
            }
            foreach (MediaFile mf in this.MediaFiles)
            {
                if (mf.Thumbnailer != null)
                {
                    sbBody.AppendLine(mf.GetScreenshotString(pop));
                }
            }

            return sbBody.ToString();
        }

        /*
        Track Number / Title                              | Len. | Rate |  FPS  | Resolution + SAR  | Size
        ------------------------------------------------------------------------------------------------------
        01 What Made You Say That                         | 3:02 | 2304 | 29.97 | 704x480 / 640x480 | 54.18 MiB
        02 Dance With The One That Brought You            | 2:29 | 1992 | 23.98 | 708x480 / 644x480 | 38.82 MiB
        03 You Lay A Whole Lot Of Love On Me              | 2:48 | 1399 | 29.97 | 704x348 / 640x348 | 31.88 MiB
        04 Whose Bed Have Your Boots Been Under           | 4:25 | 2004 | 23.98 | 708x480 / 644x480 | 69.42 MiB
        05 Any Man Of Mine                                | 4:10 | 2603 | 23.98 | 640x480 / 704x480 | 83.35 MiB
        06 The Woman In Me (Needs The man In You)         | 4:47 | 1702 | 23.98 | 704x480 / 640x480 | 64.85 MiB
        07 (If You're Not In It For Love) I'm Outta Here! | 4:40 | 1800 | 29.97 | 704x368 / 640x368 | 66.53 MiB
        08 You Win My Love                                | 4:33 | 2197 | 29.97 | 704x356 / 640x356 | 77.82 MiB
        09 No One Needs To Know                           | 3:46 | 1552 | 23.98 | 704x480 / 640x480 | 47.02 MiB
        10 Home Ain't Where His Heart Is (Anymore)        | 4:12 | 1599 | 29.97 | 704x360 / 640x360 | 53.82 MiB
        11 God Bless The Child                            | 3:49 | 2597 | 29.97 | 704x480 / 640x480 | 76.18 MiB
        12 Love Gets Me Every Time                        | 3:34 | 1906 | 23.98 | 704x480 / 640x480 | 53.56 MiB
        13 Don't Be Stupid                                | 3:47 | 3006 | 23.98 | 704x480 / 640x480 | 86.57 MiB
        14 You're Still The One                           | 3:36 | 1401 | 23.98 | 704x480 / 640x480 | 41.06 MiB
        15 Honey I'm Home                                 | 3:43 | 2706 | 29.97 | 708x480 / 644x480 | 77.10 MiB
        16 From This Moment On                            | 4:08 | 1603 | 23.98 | 704x480 / 640x480 | 53.09 MiB
        17 That Don't Impress Me Much                     | 3:45 | 1996 | 23.98 | 720x480 / 655x480 | 58.72 MiB
        18 Man! I Feel Like A Woman!                      | 3:55 | 1653 | 29.97 | 720x480 / 655x480 | 51.70 MiB
        19 You've Got A Way                               | 3:28 | 1003 | 23.98 | 716x320 / 651x320 | 29.67 MiB
        20 Come On Over                                   | 3:11 | 2301 | 29.97 | 716x480 / 651x480 | 56.81 MiB
        21 Rock This Country!                             | 4:41 | 2099 | 29.97 | 708x480 / 644x480 | 76.79 MiB
        -------------------------------------------------------------------------------------------------------
        Total:                                            |80:19 | 1976 |                           |  1.22 GiB
         **/

        public string ToStringMediaList()
        {
            string titleTracks = "Track Number / Title";
            StringBuilder sbBody = new StringBuilder();

            List<string> listFileNames = new List<string>();
            List<string> listDurations = new List<string>();
            List<string> listResolutions = new List<string>();
            List<string> listFileSizes = new List<string>();

            string totalSize = GetTotalSize().ToString("0.00") + " MiB";
            string totalDura = GetTotalDurationString();

            foreach (string p in FileCollection)
            {
                listFileNames.Add(Path.GetFileName(p));
            }
            int widthFileName = Math.Max(titleTracks.Length, listFileNames.Max(x => x.Length));

            foreach (MediaFile mf in MediaFiles)
            {
                listDurations.Add(Adapter.GetDurationString(mf.Duration));
                listResolutions.Add(mf.Video.Resolution);
                listFileSizes.Add(mf.FileSizeString);
            }

            int widthTracks = Math.Max(titleTracks.Length, this.MediaFiles.Count.ToString().Length + widthFileName + 1);
            int widthDura = Math.Max(totalDura.Length, listDurations.Max(x => x.Length));
            int widthRes = listResolutions.Max(x => x.Length);
            int widthFileSizes = Math.Max(totalSize.Length, listFileSizes.Max(x => x.Length));

            string sampleLine = GetMediaListLine(this.MediaFiles[0], widthFileName, widthDura, widthRes, widthFileSizes);

            sbBody.Append(titleTracks.PadRight(widthTracks));
            sbBody.Append(" | ");
            sbBody.Append("mm:ss".PadLeft(widthDura));
            sbBody.Append(" | ");
            sbBody.Append("Res".PadLeft(widthRes));
            sbBody.Append(" | ");
            sbBody.Append("Size".PadLeft(widthFileSizes));
            sbBody.AppendLine();

            sbBody.AppendLine("-".PadRight(sampleLine.Length, '-'));
            foreach (MediaFile mf in this.MediaFiles)
            {
                sbBody.AppendLine(GetMediaListLine(mf, widthFileName, widthDura, widthRes, widthFileSizes));
            }
            sbBody.AppendLine("-".PadRight(sampleLine.Length, '-'));
            sbBody.Append("Total:".PadRight(widthTracks));
            sbBody.Append(" | ");
            sbBody.Append(totalDura);
            sbBody.Append(" | ");
            sbBody.Append(" ".PadRight(widthRes));
            sbBody.Append(" | ");
            sbBody.Append(totalSize);

            return sbBody.ToString();
        }

        public string GetMediaListLine(MediaFile mf, int widthFileName, int widthDura, int widthRes, int widthFileSizes)
        {
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append(mf.Index.ToString().PadLeft(this.MediaFiles.Count.ToString().Length, '0'));
            sbBody.Append(" ");
            sbBody.Append(mf.FileName.PadRight(widthFileName, ' '));
            sbBody.Append(" | ");
            sbBody.Append(Adapter.GetDurationString(mf.Duration).PadLeft(widthDura, ' '));
            sbBody.Append(" | ");
            sbBody.Append(mf.Video.Resolution.PadLeft(widthRes, ' '));
            sbBody.Append(" | ");
            sbBody.Append(mf.FileSizeString.PadLeft(widthFileSizes, ' '));
            return sbBody.ToString();
        }

        public double GetTotalSize()
        {
            return MediaFiles.Sum(x => x.FileSize) / 1024 / 1024;
        }

        /// <summary>
        /// Return total duration in seconds
        /// </summary>
        /// <returns></returns>
        public double GetTotalDuration()
        {
            return MediaFiles.Sum(x => x.Duration);
        }

        public string GetTotalDurationString()
        {
            return Adapter.GetDurationString(GetTotalDuration());
        }

        public bool HasScreenshots()
        {
            bool hasSs = false;
            foreach (MediaFile mf in this.MediaFiles)
            {
                if (mf.Thumbnailer != null && mf.Thumbnailer.Screenshots.Count > 0 && !string.IsNullOrEmpty(mf.Thumbnailer.Screenshots[0].FullImageLink))
                {
                    hasSs = true;
                    return hasSs;
                }
            }
            return hasSs;
        }
    }
}