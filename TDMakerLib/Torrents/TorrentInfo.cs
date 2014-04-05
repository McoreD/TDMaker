using HelpersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UploadersLib;
using UploadersLib.HelperClasses;
using UploadersLib.ImageUploaders;
using UploadersLib.URLShorteners;

namespace TDMakerLib
{
    public class TorrentInfo
    {
        /// <summary>
        /// MediaInfo2 Object
        /// </summary>
        public MediaInfo2 Media { get; set; }

        /// <summary>
        /// String Representation of Publish tab
        /// ToString() should be called at least once
        /// </summary>
        public string PublishString { get; set; }

        /// <summary>
        /// Options for Publishing
        /// </summary>
        public PublishOptionsPacket PublishOptions { get; set; }

        private BackgroundWorker BwAppMy = null;

        public TorrentInfo(MediaInfo2 mi)
        {
            this.Media = mi;
        }

        public TorrentInfo(BackgroundWorker bwApp, MediaInfo2 mi)
            : this(mi)
        {
            this.BwAppMy = bwApp;
        }

        /// <summary>
        /// Createa and upload screenshots
        /// </summary>
        public void CreateUploadScreenshots()
        {
            TakeScreenshots();
            UploadScreenshots();
        }

        public void CreateUploadScreenshots(string ssDir)
        {
            TakeScreenshots(ssDir);
            UploadScreenshots();
        }

        private bool TakeScreenshot(MediaFile mf, string ssDir)
        {
            bool success = true;
            String mediaFilePath = mf.FilePath;
            Debug.WriteLine("Taking Screenshot for " + Path.GetFileName(mediaFilePath));
            ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, "Taking Screenshot for " + Path.GetFileName(mediaFilePath));

            switch (Program.Settings.ThumbnailerType)
            {
                case ThumbnailerType.MovieThumbnailer:
                    mf.Thumbnailer = new MovieThumbNailer(mf, ssDir);
                    break;

                case ThumbnailerType.MPlayer:
                    mf.Thumbnailer = new MPlayerThumbnailer(mf, ssDir, Program.Settings.MPlayerOptions);
                    break;
            }

            try
            {
                mf.Thumbnailer.TakeScreenshot();
            }
            catch (Exception ex)
            {
                success = false;
                Debug.WriteLine(ex.ToString());
                ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, ex.Message + " for " + Path.GetFileName(mediaFilePath));
            }

            if (Program.IsUNIX)
            {
                // Save _s.txt to MediaInfo2.Overall object
                if (string.IsNullOrEmpty(Media.Overall.Summary))
                {
                    Media.Overall.Summary = mf.Thumbnailer.MediaSummary;
                }
            }

            return success;
        }

        private void TakeScreenshots(string ssDir)
        {
            switch (this.Media.MediaTypeChoice)
            {
                case MediaType.MediaCollection:
                case MediaType.MediaIndiv:
                    foreach (MediaFile mf in this.Media.MediaFiles)
                    {
                        TakeScreenshot(mf, ssDir);
                    }
                    break;

                case MediaType.MediaDisc:
                    TakeScreenshot(this.Media.Overall, ssDir);
                    break;
            }
        }

        public void TakeScreenshots()
        {
            switch (Media.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    TakeScreenshot(this.Media.Overall, FileSystem.GetScreenShotsDir(this.Media.Overall.FilePath));
                    break;

                default:
                    foreach (MediaFile mf in this.Media.MediaFiles)
                    {
                        TakeScreenshot(mf, FileSystem.GetScreenShotsDir(mf.FilePath));
                    }
                    break;
            }
        }

        public void UploadScreenshots()
        {
            if (Media.UploadScreenshots)
            {
                switch (Media.MediaTypeChoice)
                {
                    case MediaType.MediaDisc:
                        UploadScreenshots(Media.Overall);
                        break;

                    default:
                        foreach (MediaFile mf in this.Media.MediaFiles)
                        {
                            UploadScreenshots(mf);
                        }
                        break;
                }
            }
        }

        private void UploadScreenshots(MediaFile mf)
        {
            if (Media.UploadScreenshots)
            {
                foreach (Screenshot ss in mf.Thumbnailer.Screenshots)
                {
                    if (ss != null)
                    {
                        ReportProgress(ProgressType.UPDATE_SCREENSHOTS_LIST, ss);

                        UploadResult ur = UploadScreenshot(ss.LocalPath);

                        if (ur != null)
                        {
                            if (!string.IsNullOrEmpty(ur.URL))
                            {
                                ss.FullImageLink = ur.URL;
                                ss.LinkedThumbnail = ur.ThumbnailURL;
                            }
                        }
                    }
                }
            }
        }

        private UploadResult UploadScreenshot(string ssPath)
        {
            ImageUploader imageUploader = null;
            UploadResult ur = null;

            if (File.Exists(ssPath))
            {
                if (!string.IsNullOrEmpty(Program.Settings.PtpImgCode))
                {
                    imageUploader = new PtpImageUploader(Crypt.Decrypt(Program.Settings.PtpImgCode));
                }
                else
                {
                    switch ((ImageDestination)Program.Settings.ImageUploaderType)
                    {
                        case ImageDestination.TinyPic:
                            imageUploader = new TinyPicUploader(ZKeys.TinyPicID, ZKeys.TinyPicKey, Program.UploadersConfig.TinyPicAccountType,
                                Program.UploadersConfig.TinyPicRegistrationCode);
                            break;

                        case ImageDestination.Imgur:
                            if (Program.UploadersConfig.ImgurOAuth2Info == null)
                            {
                                Program.UploadersConfig.ImgurOAuth2Info = new OAuth2Info(APIKeys.ImgurClientID, APIKeys.ImgurClientSecret);
                            }

                            imageUploader = new Imgur_v3(Program.UploadersConfig.ImgurOAuth2Info)
                            {
                                UploadMethod = Program.UploadersConfig.ImgurAccountType,
                                ThumbnailType = Program.UploadersConfig.ImgurThumbnailType,
                                UploadAlbumID = Program.UploadersConfig.ImgurAlbumID
                            };
                            break;

                        case ImageDestination.Flickr:
                            imageUploader = new FlickrUploader(APIKeys.FlickrKey, APIKeys.FlickrSecret, Program.UploadersConfig.FlickrAuthInfo, Program.UploadersConfig.FlickrSettings);
                            break;

                        case ImageDestination.Photobucket:
                            imageUploader = new Photobucket(Program.UploadersConfig.PhotobucketOAuthInfo, Program.UploadersConfig.PhotobucketAccountInfo);
                            break;

                        case ImageDestination.Picasa:
                            imageUploader = new Picasa(Program.UploadersConfig.PicasaOAuth2Info)
                            {
                                AlbumID = Program.UploadersConfig.PicasaAlbumID
                            };
                            break;

                        case ImageDestination.Twitpic:
                            int indexTwitpic = Program.UploadersConfig.TwitterSelectedAccount;

                            if (Program.UploadersConfig.TwitterOAuthInfoList != null && Program.UploadersConfig.TwitterOAuthInfoList.IsValidIndex(indexTwitpic))
                            {
                                imageUploader = new TwitPicUploader(APIKeys.TwitPicKey, Program.UploadersConfig.TwitterOAuthInfoList[indexTwitpic])
                                {
                                    TwitPicThumbnailMode = Program.UploadersConfig.TwitPicThumbnailMode,
                                    ShowFull = Program.UploadersConfig.TwitPicShowFull
                                };
                            }
                            break;

                        case ImageDestination.Twitsnaps:
                            int indexTwitsnaps = Program.UploadersConfig.TwitterSelectedAccount;

                            if (Program.UploadersConfig.TwitterOAuthInfoList.IsValidIndex(indexTwitsnaps))
                            {
                                imageUploader = new TwitSnapsUploader(APIKeys.TwitsnapsKey, Program.UploadersConfig.TwitterOAuthInfoList[indexTwitsnaps]);
                            }
                            break;

                        case ImageDestination.yFrog:
                            YfrogOptions yFrogOptions = new YfrogOptions(APIKeys.ImageShackKey);
                            yFrogOptions.Username = Program.UploadersConfig.YFrogUsername;
                            yFrogOptions.Password = Program.UploadersConfig.YFrogPassword;
                            yFrogOptions.Source = Application.ProductName;
                            imageUploader = new YfrogUploader(yFrogOptions);
                            break;
                    }
                }

                if (imageUploader != null)
                {
                    ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Uploading {0}.", Path.GetFileName(ssPath)));
                    ur = imageUploader.Upload(ssPath);
                }

                if (ur != null)
                {
                    if (!string.IsNullOrEmpty(ur.URL))
                    {
                        ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Uploaded {0}.", Path.GetFileName(ssPath)));
                    }
                }
                else
                {
                    ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Failed uploading {0}. Try again later.", Path.GetFileName(ssPath)));
                }
            }
            return ur;
        }

        /// <summary>
        /// Create Publish based on a Template
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public string CreatePublish(PublishOptionsPacket options, TemplateReader2 tr)
        {
            tr.SetFullScreenshot(options.FullPicture);
            tr.CreateInfo();

            StringBuilder sbPublish = new StringBuilder();
            sbPublish.Append(GetPublishString(tr.PublishInfo, options));

            return sbPublish.ToString();
        }

        public string CreatePublishMediaInfo(PublishOptionsPacket pop)
        {
            StringBuilder sbPublish = new StringBuilder();

            switch (Media.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    StringBuilder sbMediaInfo = new StringBuilder();
                    if (Media.MediaFiles.Count > 0)
                    {
                        foreach (MediaFile mf in Media.MediaFiles)
                        {
                            sbMediaInfo.AppendLine(BbCode.Bold(mf.FileName));
                            sbMediaInfo.AppendLine(mf.Summary.Trim());
                            sbMediaInfo.AppendLine();
                        }
                    }
                    else
                    {
                        sbMediaInfo.AppendLine(Media.Overall.Summary.Trim());
                        sbMediaInfo.AppendLine();
                    }

                    sbPublish.AppendLine(GetPublishString(sbMediaInfo.ToString(), pop));

                    if (Media.UploadScreenshots)
                        sbPublish.AppendLine(Media.Overall.GetScreenshotString(pop));

                    break;

                default:
                    foreach (MediaFile mf in Media.MediaFiles)
                    {
                        sbMediaInfo = new StringBuilder();
                        sbMediaInfo.AppendLine(BbCode.Bold(mf.FileName));
                        sbMediaInfo.AppendLine();
                        sbMediaInfo.AppendLine(mf.Summary.Trim());
                        sbMediaInfo.AppendLine();

                        sbPublish.AppendLine(GetPublishString(sbMediaInfo.ToString(), pop));

                        if (Media.UploadScreenshots)
                            sbPublish.AppendLine(mf.GetScreenshotString(pop));
                    }

                    break;
            }

            return sbPublish.ToString();
        }

        /// <summary>
        /// Create Publish based on Default (built-in) Template.
        /// Uses ToString() method of MediaInfo2
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public string CreatePublishInternal(PublishOptionsPacket pop)
        {
            StringBuilder sbPublish = new StringBuilder();
            string info = Media.MediaTypeChoice == MediaType.MusicAudioAlbum ? Media.ToStringAudio() : Media.ToStringMedia(pop);
            sbPublish.Append(GetPublishString(info, pop));

            return sbPublish.ToString().Trim();
        }

        private void ReportProgress(ProgressType percentProgress, object userState)
        {
            if (BwAppMy != null)
            {
                BwAppMy.ReportProgress((int)percentProgress, userState);
            }
            else
            {
                switch (percentProgress)
                {
                    case ProgressType.UPDATE_STATUSBAR_DEBUG:
                        Console.WriteLine((string)userState);
                        break;

                    case ProgressType.UPDATE_SCREENSHOTS_LIST:
                        Screenshot ss = userState as Screenshot;
                        Console.WriteLine("Screenshot: " + ss.FullImageLink);
                        break;
                }
            }
        }

        public string GetPublishString(string p, PublishOptionsPacket options)
        {
            StringBuilder sbPublish = new StringBuilder();

            if (options.AlignCenter)
            {
                p = BbCode.AlignCenter(p);
            }

            if (options.PreformattedText)
            {
                sbPublish.AppendLine(BbCode.Pre(p));
            }
            else
            {
                sbPublish.AppendLine(p);
            }

            return sbPublish.ToString();
        }

        /// <summary>
        /// Default Publish String representation of a Torrent
        /// </summary>
        /// <returns>Publish String</returns>
        public string ToStringPublish()
        {
            return CreatePublishInternal(this.PublishOptions);
        }

        public override string ToString()
        {
            return Path.GetFileName(this.Media.Location);
        }
    }
}