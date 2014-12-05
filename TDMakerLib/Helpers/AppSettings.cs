using HelpersLib;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using UploadersLib;

namespace TDMakerLib
{
    public class AppSettings : SettingsBase<AppSettings>
    {
        public readonly static string AppConfigFilePath = Path.Combine(App.zLocalAppDataFolder, "AppConfig.json");
        public string RootDir { get; set; }
        public string SettingsFilePath { get; set; }
        public string UploadersConfigPath { get; set; }

        public ImageDestination ImageUploaderType { get; set; }
        public string PtpImgCode { get; set; }

        [Category("Options / General"), DefaultValue(false), Description("Prefer System Folders for all the data created by ZScreen")]
        public bool PreferSystemFolders { get; set; }

        public AppSettings()
        {
            ImageUploaderType = ImageDestination.ImageShack;
        }
    }
}