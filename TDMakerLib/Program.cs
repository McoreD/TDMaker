using ShareX.HelpersLib;
using ShareX.UploadersLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using UploadersLib;

namespace TDMakerLib
{
    public static class App
    {
        private static string mProductName = "TDMaker"; // NOT Application.ProductName because both CLI and GUI needs common access
        private static readonly string PortableRootFolder = mProductName; // using relative paths
        public static readonly string RootAppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), mProductName);

        public static AppInfo mAppInfo = new AppInfo(mProductName, Application.ProductVersion, AppInfo.SoftwareCycle.Beta, false);
        public static bool Portable = Directory.Exists(Path.Combine(Application.StartupPath, PortableRootFolder));

        public static readonly string LogsDir = Path.Combine(RootAppFolder, "Logs");

        public static string PicturesDir
        {
            get
            {
                return Settings != null && Directory.Exists(Settings.CustomScreenshotsDir) && Settings.ProfileActive.ScreenshotsLocation == LocationType.CustomFolder ? Settings.CustomScreenshotsDir : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), mProductName);
            }
        }

        public static readonly string SettingsDir = Path.Combine(RootAppFolder, "Settings");
        public static readonly string ToolsDir = Path.Combine(RootAppFolder, "Tools");
        public static string TemplatesDir
        {
            get
            {
                return Settings != null && Directory.Exists(Settings.CustomTemplatesDir) && Settings.UseCustomTemplatesDir ? Settings.CustomTemplatesDir : Path.Combine(RootAppFolder, "Templates");
            }
        }
        public static string TorrentsDir
        {
            get
            {
                return Settings != null && Directory.Exists(Settings.CustomTorrentsDir) && Settings.UseCustomTorrentsDir ? Settings.CustomTorrentsDir : Path.Combine(RootAppFolder, "Torrents");
            }
        }
        public static readonly string TempDir = Path.Combine(Path.GetTempPath(), mProductName);

        public static string SettingsFilePath = Path.Combine(SettingsDir, string.Format("{0}Settings.json", mProductName));
        public static string UploadersConfigPath
        {
            get
            {
                return Settings != null && File.Exists(Settings.CustomUploadersConfigPath) ? Settings.CustomUploadersConfigPath : Path.Combine(SettingsDir, "UploadersConfig.json");
            }
        }

        public static bool IsUNIX { get; private set; }

        private static string[] AppDirs;

        public static Settings Settings { get; set; }
        public static UploadersConfig UploadersConfig { get; set; }

        public static bool DetectUnix()
        {
            string os = System.Environment.OSVersion.ToString();
            bool b = os.Contains("Unix");
            IsUNIX = b;
            return IsUNIX;
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

            string duraString = string.Format("{0:00}:{1:00}:{2:00}", hours, mins, sec);

            return duraString;
        }

        public static void WriteTemplates(bool rewrite)
        {
            string[] tNames = new string[] { "Default", "BTN", "MTN", "Minimal" };
            foreach (string name in tNames)
            {
                // Copy Default Templates to Templates folder
                string dPrefix = string.Format("Templates.{0}.", name);
                string tDir = Path.Combine(TemplatesDir, name);

                Helpers.CreateDirectoryIfNotExist(tDir, false);

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
                Assembly oAsm = Assembly.GetExecutingAssembly();
                using (Stream oStrm = oAsm.GetManifestResourceStream(oAsm.GetName().Name + "." + name))
                {
                    if (oStrm != null)
                    {
                        using (StreamReader oRdr = new StreamReader(oStrm))
                        {
                            text = oRdr.ReadToEnd();
                        }
                    }
                }
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
            AppDirs = new[] { LogsDir, PicturesDir, SettingsDir, ToolsDir };
            AppDirs.ForEach(x => Helpers.CreateDirectoryIfNotExist(x));
        }

        public static string GetProductName()
        {
            return mAppInfo.GetApplicationTitle(AppInfo.VersionDepth.MajorMinorBuild);
        }

        public static void TurnOn()
        {
            DetectUnix();

            DebugHelper.WriteLine("Operating System: " + Environment.OSVersion.VersionString);
            DebugHelper.WriteLine("Product Version: " + mAppInfo.GetApplicationTitleFull());

            if (Directory.Exists(Path.Combine(Application.StartupPath, PortableRootFolder)))
            {
                mProductName += " Portable";
            }

            mAppInfo.AppName = mProductName;

            App.InitializeDefaultFolderPaths(); // happens before Settings is readed
            App.LoadSettings();
        }

        public static void TurnOff()
        {
            FileSystem.WriteDebugFile();
        }

        public static void LoadProxySettings()
        {
            ProxyInfo.Current = App.Settings.ProxySettings;
        }

        public static void LoadSettings()
        {
            App.Settings = Settings.Load(SettingsFilePath);
            LoadProxySettings();
            App.UploadersConfig = UploadersConfig.Load(UploadersConfigPath);
        }
    }
}