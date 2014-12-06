using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using TDMakerLib;
using UploadersLib;

namespace TDMakerCLI
{
    public class Program
    {
        private static string mMediaLoc = string.Empty;
        private static string mScreenshotDir = string.Empty;
        private static bool mScreenshotsCreate = false;
        private static bool mScreenshotsUpload = false;
        private static string mTorrentsDir = string.Empty;
        private static bool mTorrentCreate = false;
        private static bool mXmlCreate = false;

        private static void Main(string[] args)
        {
            string dirImages = string.Empty;
            string dirRoot = string.Empty;
            string mSettingsFile = string.Empty;
            string dirTorrents = string.Empty;

            bool mFileCollection = false;
            bool mShowHelp = false;

            var p = new OptionSet()
            {
                { "c", "Treat multiple files as a collection", v => mFileCollection = v != null},
                { "m|media=", "Location of the media file/folder", v => mMediaLoc = v },
                { "o|options=", "Location of the settings file", v => mSettingsFile = v },
                { "rd=", "Root directory for screenshots, torrent and all other output files. Overrides all other custom folders.", v => dirRoot = v },
                { "s", "Create screenshots", v => mScreenshotsCreate = v != null},
                { "sd=", "Create screenshots in a custom folder and upload", v => dirImages = v },
                { "t", "Create torrent file in the parent folder of the media file", v => mTorrentCreate = v != null},
                { "td=", "Create torrent file in a custom folder", v => dirTorrents = v},
                { "u", "Upload screenshots", v => mScreenshotsUpload = v != null},
                { "x|xml",  "Folder path of the XML torrent description file", v => mXmlCreate = v != null },
                { "h|help",  "Show this message and exit", v => mShowHelp = v != null }
            };

            if (args.Length == 0)
            {
                mShowHelp = true;
            }

            // give cli the ability to replace environment variables
            string[] args2 = new string[args.Length];
            int count = 0;
            foreach (string arg in args)
            {
                args2[count++] = arg.Replace("%appdata%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }

            try
            {
                p.Parse(args2);

                // set root folder for images or set images dir if set one
                mScreenshotDir = Directory.Exists(dirRoot) ? dirRoot : dirImages;

                // set root folder for torrents or set torrents dir if set one
                mTorrentsDir = Directory.Exists(dirRoot) ? dirRoot : dirTorrents;
            }
            catch (Exception ex)
            {
                Console.Write("tdmakercli: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try 'tdmakercli --help' for more information.");
                return;
            }

            if (mShowHelp)
            {
                ShowHelp(p);
                return;
            }

            if (!File.Exists(mSettingsFile))
            {
                App.Config = AppSettings.Load(AppSettings.AppConfigFilePath);
                mSettingsFile = App.Config.SettingsFilePath;
            }

            if (File.Exists(mSettingsFile))
            {
                App.Settings = Settings.Load(mSettingsFile);
                App.UploadersConfig = UploadersConfig.Load(App.UploadersConfigPath);
            }

            if (App.Config != null)
            {
                App.InitializeDefaultFolderPaths();

                List<string> listFileOrDir = new List<string>() { mMediaLoc };
                MediaWizardOptions mwo = Adapter.GetMediaType(listFileOrDir, true);
                if (mwo.MediaTypeChoice == MediaType.MediaIndiv && mFileCollection)
                {
                    mwo.MediaTypeChoice = MediaType.MediaCollection;
                }
                MediaInfo2 mi = new MediaInfo2(mwo.MediaTypeChoice, mMediaLoc);
                mi.ReadMedia();

                TorrentInfo ti = new TorrentInfo(mi);

                mi.UploadScreenshots = mScreenshotsUpload;

                if (mScreenshotsUpload)
                {
                    CreateScreenshots(ti);
                    ti.UploadScreenshots();
                }
                else if (mScreenshotsCreate)
                {
                    CreateScreenshots(ti);
                }

                CreatePublish(ti);
                CreateTorrent(ti);
            }
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: tdmakercli [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine(@"tdmakercli -m ""T:\Criminal Minds"" -s -u -t");
            Console.WriteLine(@"tdmakercli -m ""T:\Criminal Minds"" -s -u -t --rd ""T:\Criminal Minds""");
            Console.WriteLine();
        }

        private static void CreateScreenshots(TorrentInfo ti)
        {
            if (Directory.Exists(mScreenshotDir))
            {
                ti.CreateScreenshots(mScreenshotDir);
            }
            else
            {
                ti.CreateScreenshots();
            }
        }

        private static void CreatePublish(TorrentInfo ti)
        {
            PublishOptionsPacket pop = new PublishOptionsPacket()
            {
                AlignCenter = App.Settings.AlignCenter,
                FullPicture = App.Settings.UseFullPicture,
                PreformattedText = App.Settings.PreText,
                PublishInfoTypeChoice = App.Settings.PublishInfoTypeChoice,
                TemplateLocation = Path.Combine(App.TemplatesDir, "Default")
            };
            ti.PublishString = Adapter.CreatePublish(ti, pop);

            Console.WriteLine(ti.PublishString);
        }

        private static void CreateTorrent(TorrentInfo ti)
        {
            // create a torrent
            if (mTorrentCreate || Directory.Exists(mTorrentsDir))
            {
                ti.Media.TorrentCreateInfoMy = new TorrentCreateInfo(App.Settings.TrackerGroups[App.Settings.TrackerGroupActive], mMediaLoc);
                if (Directory.Exists(mTorrentsDir))
                {
                    ti.Media.TorrentCreateInfoMy.TorrentFolder = mTorrentsDir;
                }
                ti.Media.TorrentCreateInfoMy.CreateTorrent();

                // create xml file
                if (mXmlCreate)
                {
                    string fp = Path.Combine(ti.Media.TorrentCreateInfoMy.TorrentFolder, MediaHelper.GetMediaName(ti.Media.TorrentCreateInfoMy.MediaLocation)) + ".xml";
                    FileSystem.GetXMLTorrentUpload(ti.Media).Write2(fp);
                }
            }
        }
    }
}