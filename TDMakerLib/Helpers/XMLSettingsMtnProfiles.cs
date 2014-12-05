using HelpersLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace TDMakerLib
{
    public class SettingsMtnProfiles : SettingsBase<SettingsMtnProfiles>
    {
        public static string MtnProfilesFileName = "MtnProfiles.json";

        public int MtnProfileActive = 0;
        public List<XMLSettingsScreenshot> MtnProfiles = new List<XMLSettingsScreenshot>();

        public SettingsMtnProfiles()
        {
            this.ApplyDefaultPropertyValues();
        }

        public XMLSettingsScreenshot GetMtnProfileActive()
        {
            if (MtnProfiles.Count > MtnProfileActive)
            {
                return MtnProfiles[MtnProfileActive];
            }
            return new XMLSettingsScreenshot();
        }
    }
}