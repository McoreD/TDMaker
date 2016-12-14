using MonoTorrent.Common;
using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TDMakerLib
{
    public class TorrentCreateInfo
    {
        private ProfileOptions Profile { get; set; }
        public string MediaLocation { get; private set; }
        public string TorrentFolder { get; set; }
        public string TorrentFilePath { get; private set; }

        public TorrentCreateInfo(ProfileOptions profile, string mediaLoc)
        {
            Profile = profile;
            MediaLocation = mediaLoc;
            TorrentFolder = DefaultTorrentFolder;
        }

        private string DefaultTorrentFolder
        {
            get
            {
                string dir = App.TorrentsDir;

                if (this.Profile.TorrentsFolder == LocationType.ParentFolder)
                {
                    dir = Path.GetDirectoryName(MediaLocation);
                }

                return dir;
            }
        }

        /// <summary>
        /// Create torrent with progress
        /// </summary>
        /// <param name="workerMy"></param>
        public void CreateTorrent()
        {
            string p = MediaLocation;
            if (this.Profile != null && this.Profile.Trackers != null && (File.Exists(p) || Directory.Exists(p)))
            {
                foreach (string tracker in this.Profile.Trackers)
                {
                    TorrentCreator tc = new TorrentCreator();
                    tc.CreatedBy = Application.ProductName;
                    tc.Private = true;
                    tc.Comment = MediaHelper.GetMediaName(p);
                    tc.Path = p;
                    tc.PublisherUrl = "https://github.com/McoreD/TDMaker";
                    tc.Publisher = Application.ProductName;
                    tc.StoreMD5 = false; // delays torrent creation
                    List<string> temp = new List<string>();
                    temp.Add(tracker);
                    tc.Announces.Add(temp);

                    var uri = new Uri(tracker);
                    string torrentFileName = string.Format("{0}.torrent", (File.Exists(p) ? Path.GetFileNameWithoutExtension(p) : MediaHelper.GetMediaName(p)));
                    TorrentFilePath = Path.Combine(Path.Combine(TorrentFolder, uri.Host), torrentFileName);

                    //  ReportProgress(workerMy, ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Creating {0}", this.TorrentFilePath));

                    tc.Hashed += delegate (object o, TorrentCreatorEventArgs e)
                    {
                        //  ReportProgress(workerMy, ProgressType.UPDATE_PROGRESSBAR_Cumulative, e.OverallCompletion);
                    };

                    Helpers.CreateDirectoryFromFilePath(this.TorrentFilePath);
                    tc.Create(this.TorrentFilePath);
                    // ReportProgress(workerMy, ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Created {0}", this.TorrentFilePath));
                }
            }
            else
            {
                DebugHelper.WriteLine("There were no active trackers configured to create a torrent.");
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