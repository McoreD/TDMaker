using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class TaskSettings
    {
        public MediaInfo2 Media { get; set; }

        public ProfileOptions Profile { get; private set; }
        public PublishOptions PublishOptions { get; set; }
        public MediaWizardOptions MediaOptions { get; set; }

        public string TorrentFolder { get; set; }
        public string TorrentFilePath { get; set; }

        public TaskSettings()
        {
            Profile = App.Settings.ProfileActive;
            PublishOptions = new PublishOptions();
            MediaOptions = new MediaWizardOptions();

            TorrentFolder = App.TorrentsDir;

            if (Media != null && Profile.TorrentsFolder == LocationType.ParentFolder)
            {
                TorrentFolder = Path.GetDirectoryName(Media.Location);
            }
        }

        public string ToStringMedia()
        {
            return Media.ToStringMedia(PublishOptions);
        }
    }
}