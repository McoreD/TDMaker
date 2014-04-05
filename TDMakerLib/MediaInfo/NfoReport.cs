using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TDMakerLib.MediaInfo
{
    class NfoReport
    {
        private StringBuilder msbAudio = new StringBuilder();
        private StringBuilder msbAlbumInfo = new StringBuilder();
        private StringBuilder msbExtraFiles = new StringBuilder();
        private StringBuilder mFileName = new StringBuilder();

        private string mFolderPath = string.Empty;
        private BackgroundWorker mBwApp = null;

        public string RippedBy { get; set; }
        public DateTime DateRipped { get; set; }

        public string Ripper { get; set; }
        public string Encoder { get; set; }
        public string Codec { get; set; }
        public string CodecProfile { get; set; }

        public double TotalBitrate { get; private set; }

        public NfoReport(string p, BackgroundWorker bwApp)
        {
            this.mFolderPath = p;
            this.mBwApp = bwApp;
            sMakeNfo();
        }

        public string FileName
        {

            get
            {
                return mFileName.ToString();
            }

        }

        public string TrackList(List<string> lstAudioFiles)
        {

            StringBuilder msbAudio = new StringBuilder();

            TagLib.File f = TagLib.File.Create(lstAudioFiles[0]);

            double totalBitrate = 0.0;

            string longestName = string.Format("{0} - {1}", f.Tag.Title, f.Tag.FirstPerformer);
            foreach (string af in lstAudioFiles)
            {
                TagLib.File xt = TagLib.File.Create(af);
                string cmp = string.Format("{0} - {1}", xt.Tag.Title, xt.Tag.FirstPerformer);
                if (longestName.Length < cmp.Length)
                {
                    longestName = cmp;
                }
            }

            foreach (string af in lstAudioFiles)
            {
                TagLib.File xt = TagLib.File.Create(af);
                FileInfo fi = new FileInfo(af);
                double bitRate = fGetBitrate(fi.Length, xt.Properties.Duration);

                string track = string.Format("{0} - {1}", xt.Tag.Title, xt.Tag.FirstPerformer);
                msbAudio.AppendLine(string.Format("{0}. {1} {2} {3} MiB [{4} Kibit/s] [{5}]", xt.Tag.Track.ToString("00"),
                                                                                                 track,
                                                                                                 fGetPadding(longestName, track),
                                                                                                 (fi.Length / 1024.0 / 1024.0).ToString("00.00"),
                                                                                                 bitRate.ToString("0.00"),
                                                                                                 fGetHMS(xt.Properties.Duration.TotalSeconds)));
                totalBitrate += bitRate;

                if (mBwApp != null)
                    mBwApp.ReportProgress(1);

            }

            this.TotalBitrate = totalBitrate;

            return msbAudio.ToString();

        }

        private string ExtraFilesList(List<string> lstExtraFiles)
        {
            StringBuilder sbExtraFiles = new StringBuilder();

            string longestName = Path.GetFileName(lstExtraFiles[0]);
            foreach (string ef in lstExtraFiles)
            {
                if (longestName.Length < Path.GetFileName(ef).Length)
                {
                    longestName = Path.GetFileName(ef);
                }
            }

            foreach (string ef in lstExtraFiles)
            {
                FileInfo fi = new FileInfo(ef);
                sbExtraFiles.AppendLine(string.Format("{0} {1} {2} KiB", Path.GetFileName(ef),
                                                                            fGetPadding(longestName, Path.GetFileName(ef)),
                                                                            (fi.Length / 1024.0).ToString("00.00")));
            }

            return sbExtraFiles.ToString();

        }

        private void sMakeNfo()
        {
            // browse the album folder
            List<string> lstExtraFiles = new List<string>();
            List<string> lstAudioFiles = new List<string>();
            List<string> lstTotalFiles = new List<string>();

            lstTotalFiles.AddRange(Directory.GetFiles(mFolderPath, "*.*", SearchOption.TopDirectoryOnly));
            lstExtraFiles.AddRange(lstTotalFiles);

            foreach (string ext in Program.Settings.SupportedFileExtAudio)
            {
                foreach (string f in lstTotalFiles)
                {
                    if (string.Equals(ext.ToLower(), Path.GetExtension(f)))
                    {
                        lstAudioFiles.Add(f);
                        lstExtraFiles.Remove(f);
                    }
                }
            }

            if (mBwApp != null)
                mBwApp.ReportProgress(0, lstAudioFiles.Count);

            if (lstAudioFiles.Count > 0)
            {

                TagLib.File f = TagLib.File.Create(lstAudioFiles[0]);

                mFileName.Append(f.Tag.FirstAlbumArtist);
                mFileName.Append(" - ");
                mFileName.Append(f.Tag.Album);
                mFileName.Append(string.Format(" [{0}] ", RippedBy));

                // read track list

                msbAudio.AppendLine("\t\t\t\t{:.. Track List ..:}");
                msbAudio.AppendLine(Environment.NewLine);
                msbAudio.Append(TrackList(lstAudioFiles));

                // fill album info
                msbAlbumInfo.AppendLine("\t\t\t\t{:.. Album Info ..:}      ");
                msbAlbumInfo.AppendLine(Environment.NewLine);

                msbAlbumInfo.AppendLine(string.Format("Album Artist:    {0}", f.Tag.FirstAlbumArtist));
                msbAlbumInfo.AppendLine(string.Format("Album Name:      {0}", f.Tag.Album));
                msbAlbumInfo.AppendLine(string.Format("Year:            {0}", f.Tag.Year));
                msbAlbumInfo.AppendLine(string.Format("Genre:           {0}", f.Tag.FirstGenre));
                msbAlbumInfo.AppendLine(string.Format("Bit Rate:        {0} Kibit/s", (this.TotalBitrate / (double)lstAudioFiles.Count).ToString("0.00")));
                msbAlbumInfo.AppendLine(string.Format("Ripped On:       {0}", DateRipped.ToString("yyyy-MM-dd")));
                msbAlbumInfo.AppendLine(string.Format("Tracks:          {0}", lstAudioFiles.Count));
                msbAlbumInfo.AppendLine(string.Format("Size:            {0} MiB", fGetFolderSize(lstTotalFiles).ToString("0.00")));


                if (lstExtraFiles.Count > 0)
                {
                    msbExtraFiles.AppendLine("\t\t\t\t{:.. Included Files ..:}     ");
                    msbExtraFiles.AppendLine(Environment.NewLine);
                    msbExtraFiles.Append(ExtraFilesList(lstExtraFiles));
                }

            }

        }

        private string fGetPadding(string longestName, string name)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = name.Length; i < longestName.Length; i++)
            {
                sb.Append(".");
            }
            return sb.ToString();
        }


        private string fGetText(string p)
        {
            using (StreamReader sr = new StreamReader(p))
            {
                return sr.ReadToEnd();
            }
        }

        private double fGetFolderSize(List<string> fz)
        {
            long ttl = 0;
            foreach (string f in fz)
            {
                FileInfo fi = new FileInfo(f);
                ttl += fi.Length;
            }
            return ttl / 1024.0 / 1024.0;
        }

        private double fGetBitrate(long sz, TimeSpan dur)
        {
            return (sz / 1024 * 8 / dur.TotalSeconds);
        }


        public string fGetHMS(double sec)
        {

            double[] hms = fGetDurationInHoursMS(sec);
            return string.Format("{0}:{1}:{2}", hms[0].ToString("00"), hms[1].ToString("00"), hms[2].ToString("00"));

        }


        public double[] fGetDurationInHoursMS(double seconds)
        {

            double[] arrayHoursMinutesSeconds = new double[4];
            double SecondsLeft = seconds;
            int hours = 0;
            int minutes = 0;

            while (SecondsLeft >= 3600)
            {
                SecondsLeft -= 3600;
                hours += 1;

            }

            arrayHoursMinutesSeconds[0] = hours;

            while (SecondsLeft >= 60)
            {

                SecondsLeft -= 60;
                minutes += 1;
            }

            arrayHoursMinutesSeconds[1] = minutes;
            arrayHoursMinutesSeconds[2] = SecondsLeft;

            return arrayHoursMinutesSeconds;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            string fp = Path.Combine(Application.StartupPath, "Header.txt");
            if (File.Exists(fp))
            {
                sb.AppendLine(fGetText(fp));
            }

            sb.AppendLine("\t\t\t\t{:... Sofware ...:}");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(string.Format("Ripper:          {0}", Ripper));
            sb.AppendLine(string.Format("Encoder:         {0}", Encoder));
            sb.AppendLine(string.Format("Codec:           {0}", Codec));
            sb.AppendLine(string.Format("Codec Profile:   {0}", CodecProfile));
            sb.AppendLine(Environment.NewLine);

            sb.AppendLine(msbAlbumInfo.ToString());

            sb.AppendLine(msbAudio.ToString());

            sb.AppendLine(msbExtraFiles.ToString());

            fp = Path.Combine(Application.StartupPath, "Footer.txt");
            if (File.Exists(fp))
            {
                sb.AppendLine(fGetText(fp));
            }

            return sb.ToString();

        }

    }
}
