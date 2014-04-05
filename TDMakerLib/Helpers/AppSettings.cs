using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using UploadersLib;

namespace TDMakerLib
{
    [Serializable]
    public class AppSettings
    {
        public readonly static string AppSettingsFile = Path.Combine(Program.zLocalAppDataFolder, "AppSettings.xml");

        public string RootDir { get; set; }

        public string XMLSettingsFile { get; set; }

        public string UploadersConfigPath { get; set; }

        public ImageDestination ImageUploaderType { get; set; }

        public string PtpImgCode { get; set; }

        [Category("Options / General"), DefaultValue(false), Description("Prefer System Folders for all the data created by ZScreen")]
        public bool PreferSystemFolders { get; set; }

        public AppSettings()
        {
            ImageUploaderType = ImageDestination.ImageShack;
        }

        public static AppSettings Read()
        {
            return Read(AppSettingsFile);
        }

        public string GetSettingsFilePath()
        {
            return Path.Combine(Program.SettingsDir, XMLSettingsCore.XMLFileName);
        }

        public string GetSettingsFilePath(string fileName)
        {
            return Path.Combine(Program.SettingsDir, fileName);
        }

        public static AppSettings Read(string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath))
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(AppSettings));
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        return xs.Deserialize(fs) as AppSettings;
                    }
                }
                catch (Exception ex)
                {
                    FileSystem.AppendDebug(ex.ToString());
                }
            }

            return new AppSettings();
        }

        public void Write()
        {
            new Thread(SaveThread).Start(AppSettingsFile);
        }

        public void SaveThread(object filePath)
        {
            lock (this)
            {
                Write((string)filePath);
            }
        }

        public void Write(string filePath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                XmlSerializer xs = new XmlSerializer(typeof(AppSettings));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    xs.Serialize(fs, this);
                }
            }
            catch (Exception ex)
            {
                FileSystem.AppendDebug(ex.ToString());
            }
        }
    }
}