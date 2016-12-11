using System;
using System.Collections.Generic;
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

        public TaskSettings()
        {
            Profile = App.Settings.ProfileActive;
            PublishOptions = new PublishOptions();
            MediaOptions = new MediaWizardOptions();
        }
    }
}