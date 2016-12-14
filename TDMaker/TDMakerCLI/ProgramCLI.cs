using Mono.Options;
using ShareX.HelpersLib;
using ShareX.UploadersLib;
using System;
using System.Collections.Generic;
using System.IO;
using TDMakerLib;
using UploadersLib;

namespace TDMakerCLI
{
    public class ProgramCLI
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
            string mSettingsFilePath = string.Empty;
            string dirTorrents = string.Empty;

            bool mFileCollection = false;
            bool mShowHelp = false;

            var p = new OptionSet()
            {
                { "c", "Treat multiple files as a collection", v => mFileCollection = v != null },
                { "m|media=", "Location of the media file/folder", v => mMediaLoc = v },
                { "o|options=", "Location of the settings file", v => mSettingsFilePath = v },
                { "rd=", "Root directory for screenshots, torrent and all other output files. Overrides all other custom folders.", v => dirRoot = v },
                { "s", "Create screenshots", v => mScreenshotsCreate = v != null },
                { "sd=", "Create screenshots in a custom folder and upload", v => dirImages = v },
                { "t", "Create torrent file in the parent folder of the media file", v => mTorrentCreate = v != null },
                { "td=", "Create torrent file in a custom folder", v => dirTorrents = v },
                { "u", "Upload screenshots", v => mScreenshotsUpload = v != null },
                { "x|xml", "Folder path of the XML torrent description file", v => mXmlCreate = v != null },
                { "h|help", "Show this message and exit", v => mShowHelp = v != null }
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

            if (!File.Exists(mSettingsFilePath))
            {
                mSettingsFilePath = App.SettingsFilePath;
            }

            if (File.Exists(mSettingsFilePath))
            {
                App.Settings = Settings.Load(mSettingsFilePath);
                App.UploadersConfig = UploadersConfig.Load(App.UploadersConfigPath);
            }

            if (App.Settings != null)
            {
                App.InitializeDefaultFolderPaths();

                List<string> listFileOrDir = new List<string>() { mMediaLoc };
                MediaWizardOptions mwo = Adapter.GetMediaType(listFileOrDir, true);
                if (mwo.MediaTypeChoice == MediaType.MediaIndiv && mFileCollection)
                {
                    mwo.MediaTypeChoice = MediaType.MediaCollection;
                }

                TaskSettings ts = new TaskSettings();
                ts.MediaOptions.CreateScreenshots = mScreenshotsCreate;
                ts.MediaOptions.UploadScreenshots = mScreenshotsUpload;

                WorkerTask task = new WorkerTask(ts);
                MediaInfo2 mi = new MediaInfo2(mwo, mMediaLoc);
                mi.ReadMedia();

                if (mScreenshotsUpload)
                {
                    TakeScreenshots(task);
                    task.UploadScreenshots();
                }
                else if (mScreenshotsCreate)
                {
                    TakeScreenshots(task);
                }

                //  CreatePublish(ti);
                //  CreateTorrent(ti);
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
            Console.WriteLine(@"tdmakercli -m ""P:\Hercules.2014.576p.BDRip.x264-HANDJOB.mkv"" -s -u -t --rd ""P:\Screenshots""");
            Console.WriteLine();
        }

        private static void TakeScreenshots(WorkerTask task)
        {
            if (Directory.Exists(mScreenshotDir))
            {
                task.TakeScreenshots(mScreenshotDir);
            }
            else
            {
                task.TakeScreenshots();
            }
        }

        private static void CreatePublish(WorkerTask ti)
        {
            PublishOptions pop = new PublishOptions()
            {
                AlignCenter = App.Settings.ProfileActive.AlignCenter,
                FullPicture = App.Settings.ProfileActive.UseFullPictureURL,
                PreformattedText = App.Settings.ProfileActive.PreText,
                PublishInfoTypeChoice = App.Settings.ProfileActive.Publisher,
                TemplateLocation = Path.Combine(App.TemplatesDir, "BTN")
            };

            Console.WriteLine(Adapter.ToPublishString(ti.Info.TaskSettings, pop));
        }

        private static void CreateTorrent(WorkerTask task)
        {
            if (mTorrentCreate)
            {
                Helpers.CreateDirectoryFromDirectoryPath(mTorrentsDir);
                if (Directory.Exists(mTorrentsDir))
                {
                    task.Info.TaskSettings.TorrentFolder = mTorrentsDir;
                }

                task.CreateTorrent();

                // create xml file
                if (mXmlCreate)
                {
                    string fp = Path.Combine(task.Info.TaskSettings.TorrentFolder,
                        MediaHelper.GetMediaName(task.Info.TaskSettings.Media.Location)) + ".xml";
                    FileSystem.GetXMLTorrentUpload(task.Info.TaskSettings).Write2(fp);
                }
            }
        }
    }
}