using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TDMakerLib
{
    public class TorrentCreateInfo
    {
        public TrackerGroup TrackerGroupActive { get; private set; }

        public string MediaLocation { get; private set; }

        public string TorrentFolder { get; set; }

        public string TorrentFilePath { get; private set; }

        public TorrentCreateInfo(TrackerGroup tracker, string mediaLoc)
        {
            this.TrackerGroupActive = tracker;
            this.MediaLocation = mediaLoc;
            this.TorrentFolder = GetTorrentFolderPath();
        }

        private string GetTorrentFolderPath()
        {
            string dir = "";

            switch (Program.Settings.TorrentLocationChoice)
            {
                case LocationType.CustomFolder:
                    if (Directory.Exists(Program.Settings.CustomTorrentsDir) && Program.Settings.TorrentsOrganize)
                    {
                        dir = Path.Combine(Program.Settings.CustomTorrentsDir, this.TrackerGroupActive.Name);
                    }
                    else
                    {
                        dir = Program.Settings.CustomTorrentsDir;
                    }
                    break;

                case LocationType.KnownFolder:
                    dir = Program.TorrentsDir;
                    break;

                case LocationType.ParentFolder:
                    dir = Path.GetDirectoryName(this.MediaLocation);
                    break;
            }

            return dir;
        }

        public void SetTorrentFilePath(string fileName)
        {
            TorrentFilePath = Path.Combine(TorrentFolder, fileName);
        }

        /// <summary>
        /// Create torrent without progress
        /// </summary>
        public void CreateTorrent()
        {
            CreateTorrent(null);
        }

        /// <summary>
        /// Create torrent with progress
        /// </summary>
        /// <param name="workerMy"></param>
        public void CreateTorrent(BackgroundWorker workerMy)
        {
            string p = this.MediaLocation;
            if (this.TrackerGroupActive != null)
            {
                if (File.Exists(p) || Directory.Exists(p))
                {
                    foreach (Tracker myTracker in this.TrackerGroupActive.Trackers)
                    {
                        MonoTorrent.Common.TorrentCreator tc = new MonoTorrent.Common.TorrentCreator();
                        tc.CreatedBy = Application.ProductName;
                        tc.Private = true;
                        tc.Comment = MediaHelper.GetMediaName(p);
                        tc.Path = p;
                        tc.PublisherUrl = "http://code.google.com/p/tdmaker";
                        tc.Publisher = Application.ProductName;
                        tc.StoreMD5 = true;
                        List<string> temp = new List<string>();
                        temp.Add(myTracker.AnnounceURL);
                        tc.Announces.Add(temp);

                        string torrentFileName = string.Format("{0} - {1}.torrent", (File.Exists(p) ? Path.GetFileName(p) : MediaHelper.GetMediaName(p)), myTracker.Name);
                        this.SetTorrentFilePath(torrentFileName);

                        ReportProgress(workerMy, ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Creating {0}", this.TorrentFilePath));
                        tc.Create(this.TorrentFilePath);
                        ReportProgress(workerMy, ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Created {0}", this.TorrentFilePath));
                    }
                }
            }
            else
            {
                Console.WriteLine("There were no active trackers configured to create a torrent.");
            }
        }

        public void ReportProgress(BackgroundWorker worker, ProgressType progress, object userState)
        {
            if (worker != null)
            {
                worker.ReportProgress((int)progress, userState);
            }
            else
            {
                switch (progress)
                {
                    case ProgressType.UPDATE_STATUSBAR_DEBUG:
                        Console.WriteLine(userState as string);
                        break;
                }
            }
        }
    }
}