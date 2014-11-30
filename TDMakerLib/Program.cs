using HelpersLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using UploadersLib;
using UploadersLib.HelperClasses;

namespace TDMakerLib
{
    public static class Program
    {
        private static string mProductName = "TDMaker"; // NOT Application.ProductName because both CLI and GUI needs common access
        private static readonly string PortableRootFolder = mProductName; // using relative paths

        public static McoreSystem.AppInfo mAppInfo = new McoreSystem.AppInfo(mProductName, Application.ProductVersion, McoreSystem.AppInfo.SoftwareCycle.Final, false);
        public static bool Portable = Directory.Exists(Path.Combine(Application.StartupPath, PortableRootFolder));

        public static bool MultipleInstance { get; private set; }

        internal static readonly string zRoamingAppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), mProductName);
        internal static readonly string zLocalAppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), mProductName);

        internal static readonly string zLogsDir = Path.Combine(zLocalAppDataFolder, "Logs");
        internal static readonly string zPicturesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), mProductName);
        internal static readonly string zSettingsDir = Path.Combine(zRoamingAppDataFolder, "Settings");
        internal static readonly string zTemplatesDir = Path.Combine(zRoamingAppDataFolder, "Templates");
        internal static readonly string zTorrentsDir = Path.Combine(zLocalAppDataFolder, "Torrents");
        public static readonly string zTempDir = Path.Combine(zLocalAppDataFolder, "Temp");

        public static AppSettings AppConf = AppSettings.Read();

        public static string DefaultRootAppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), mProductName);

        private static readonly string UploadersConfigFileName = "UploadersConfig.xml";

        public static string RootAppFolder { get; set; }

        public static string LogsDir = zLogsDir;
        public static string PicturesDir = zPicturesDir;
        public static string SettingsDir = zSettingsDir;
        public static string TemplatesDir = Settings != null && Directory.Exists(Settings.CustomTemplatesDir) && Settings.UseCustomTemplatesDir ? Settings.CustomTemplatesDir : TemplatesDir;
        public static string TorrentsDir = Settings != null && Directory.Exists(Settings.CustomTorrentsDir) && Settings.UseCustomTorrentsDir ? Settings.CustomTorrentsDir : TorrentsDir;

        public static bool IsUNIX { get; private set; }

        private static bool RunConfig = false;

        private static string[] AppDirs;

        public static XMLSettingsCore Settings { get; set; }
        public static XMLSettingsMtnProfiles mtnProfileMgr { get; set; }
        public static UploadersConfig UploadersConfig { get; set; }

        public static bool DetectUnix()
        {
            string os = System.Environment.OSVersion.ToString();
            bool b = os.Contains("Unix");
            IsUNIX = b;
            return IsUNIX;
        }

        public static string XMLSettingsFile
        {
            get
            {
                if (!Directory.Exists(SettingsDir))
                {
                    Directory.CreateDirectory(SettingsDir);
                }

                return Program.AppConf.XMLSettingsFile;
            }
        }

        public static string UploadersConfigPath
        {
            get
            {
                return Path.Combine(SettingsDir, UploadersConfigFileName);
            }
        }

        public static void ClearScreenshots()
        {
            if (!Program.Settings.KeepScreenshots)
            {
                // delete if option set to temporary location
                string[] files = Directory.GetFiles(Program.zTempDir, "*.*", SearchOption.AllDirectories);
                foreach (string screenshot in files)
                {
                    try
                    {
                        File.Delete(screenshot);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Get DuratingString in HH:mm:ss
        /// </summary>
        /// <param name="dura">Duration in Milliseconds</param>
        /// <returns>DuratingString in HH:mm:ss</returns>
        public static string GetDurationString(double dura)
        {
            double duraSec = dura / 1000.0;

            long hours = (long)duraSec / 3600;
            long secLeft = (long)duraSec - hours * 3600;
            long mins = secLeft / 60;
            long sec = secLeft - mins * 60;

            string duraString = string.Format("{0}:{1}:{2}",
                hours.ToString("00"),
                mins.ToString("00"),
                sec.ToString("00"));

            return duraString;
        }

        public static void WriteTemplates(bool rewrite)
        {
            string[] tNames = new string[] { "Default", "MTN", "Minimal" };
            foreach (string name in tNames)
            {
                // Copy Default Templates to Templates folder
                string dPrefix = string.Format("Templates.{0}.", name);
                string tDir = Path.Combine(Program.TemplatesDir, name);
                if (!Directory.Exists(tDir))
                {
                    Directory.CreateDirectory(tDir);
                }
                string[] tFiles = new string[] { "Disc.txt", "File.txt", "DiscAudioInfo.txt", "FileAudioInfo.txt", "GeneralInfo.txt", "FileVideoInfo.txt", "DiscVideoInfo.txt" };

                foreach (string fn in tFiles)
                {
                    string dFile = Path.Combine(tDir, fn);
                    bool write = !File.Exists(dFile) || (File.Exists(dFile) && rewrite);
                    if (write)
                    {
                        using (StreamWriter sw = new StreamWriter(dFile))
                        {
                            sw.WriteLine(GetText(dPrefix + fn));
                        }
                    }
                }
            }
        }

        public static string GetText(string name)
        {
            string text = "";

            try
            {
                System.Reflection.Assembly oAsm = System.Reflection.Assembly.GetExecutingAssembly();
                Stream oStrm = oAsm.GetManifestResourceStream(oAsm.GetName().Name + "." + name);
                StreamReader oRdr = new StreamReader(oStrm);
                text = oRdr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return text;
        }

        /// <summary>
        /// Function to update Default Folder Paths based on Root folder
        /// </summary>
        public static void InitializeDefaultFolderPaths()
        {
            if (AppConf.PreferSystemFolders)
            {
                LogsDir = zLogsDir;
                PicturesDir = zPicturesDir;
                SettingsDir = zSettingsDir;
                TemplatesDir = zTemplatesDir;
                TorrentsDir = zTorrentsDir;
            }
            else
            {
                LogsDir = Path.Combine(RootAppFolder, "Logs");
                PicturesDir = Path.Combine(RootAppFolder, "Screenshots");
                SettingsDir = Path.Combine(RootAppFolder, "Settings");
                TemplatesDir = Path.Combine(RootAppFolder, "Templates");
                TorrentsDir = Path.Combine(RootAppFolder, "Torrents");
            }

            AppDirs = new[] { LogsDir, PicturesDir, SettingsDir, TorrentsDir };

            foreach (string dp in AppDirs)
            {
                if (!string.IsNullOrEmpty(dp) && !Directory.Exists(dp))
                {
                    Directory.CreateDirectory(dp);
                }
            }

            string latestSettingsFile = Path.Combine(SettingsDir, XMLSettingsCore.XMLFileName);
            if (File.Exists(latestSettingsFile))
            {
                Program.AppConf.XMLSettingsFile = latestSettingsFile;
            }
        }

        public static string GetProductName()
        {
            return mAppInfo.GetApplicationTitle(McoreSystem.AppInfo.VersionDepth.MajorMinorBuild);
        }

        public static bool TurnOn()
        {
            DetectUnix();
            FileSystem.AppendDebug("Operating System: " + Environment.OSVersion.VersionString);
            FileSystem.AppendDebug("Product Version: " + mAppInfo.GetApplicationTitleFull());
            DialogResult configResult = DialogResult.OK;

            if (Directory.Exists(Path.Combine(Application.StartupPath, PortableRootFolder)))
            {
                AppConf.PreferSystemFolders = false;
                RootAppFolder = PortableRootFolder;
                mProductName += " Portable";
                mAppInfo.AppName = mProductName;
            }
            else
            {
                if (string.IsNullOrEmpty(Program.AppConf.RootDir))
                {
                    RootAppFolder = DefaultRootAppFolder;
                    ConfigWizard cw = new ConfigWizard(DefaultRootAppFolder);
                    configResult = cw.ShowDialog();
                    RunConfig = true;
                }
                if (!string.IsNullOrEmpty(Program.AppConf.RootDir) && Directory.Exists(Program.AppConf.RootDir))
                {
                    RootAppFolder = Program.AppConf.RootDir;
                }
                else
                {
                    RootAppFolder = Program.AppConf.PreferSystemFolders ? zLocalAppDataFolder : DefaultRootAppFolder;
                }
            }
            if (configResult == DialogResult.OK)
            {
                FileSystem.AppendDebug("Config file: " + AppSettings.AppSettingsFile);
                FileSystem.AppendDebug(string.Format("Root Folder: {0}", AppConf.PreferSystemFolders ? zLocalAppDataFolder : RootAppFolder));
                FileSystem.AppendDebug("Initializing Default folder paths...");
                Program.InitializeDefaultFolderPaths(); // happens before XMLSettings is readed
            }
            mAppInfo.AppName = mProductName;
            return configResult == DialogResult.OK;
        }

        public static void TurnOff()
        {
            if (!Portable)
            {
                AppConf.Write();
            }

            FileSystem.WriteDebugFile();
        }

        public static void LoadProxySettings()
        {
            ProxyInfo.Current = Program.Settings.ProxySettings;
        }

        public static void LoadSettings()
        {
            LoadSettings(string.Empty);
            LoadProxySettings();
            Program.UploadersConfig = UploadersConfig.Load(UploadersConfigPath);
        }

        public static void LoadSettings(string fp)
        {
            if (string.IsNullOrEmpty(fp))
            {
                FileSystem.AppendDebug("Reading " + Program.XMLSettingsFile);
                Program.Settings = XMLSettingsCore.Read();
            }
            else
            {
                FileSystem.AppendDebug("Reading " + fp);
                Program.Settings = XMLSettingsCore.Read(fp);
            }

            // Use Configuration Wizard Settings if applied
            if (RunConfig)
            {
                Program.Settings.ImageUploaderType = Program.AppConf.ImageUploaderType;

                if (!string.IsNullOrEmpty(Program.AppConf.PtpImgCode))
                    Program.Settings.PtpImgCode = Program.AppConf.PtpImgCode;
            }
            mtnProfileMgr = XMLSettingsMtnProfiles.Read();
        }
    }
}