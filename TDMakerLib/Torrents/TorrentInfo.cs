using HelpersLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using UploadersLib;
using UploadersLib.FileUploaders;
using UploadersLib.GUI;
using UploadersLib.HelperClasses;
using UploadersLib.ImageUploaders;

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
                    mf.Thumbnailer = new MovieThumbnailer(mf, ssDir);
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
                foreach (ScreenshotInfo ss in mf.Thumbnailer.Screenshots)
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
            UploadResult ur = null;

            if (File.Exists(ssPath))
            {
                ur = UploadImage(ssPath);

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

        private UploadResult UploadImage(string ssPath)
        {
            ImageUploader imageUploader = null;

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

                        string albumID = null;

                        if (Program.UploadersConfig.ImgurUploadSelectedAlbum && Program.UploadersConfig.ImgurSelectedAlbum != null)
                        {
                            albumID = Program.UploadersConfig.ImgurSelectedAlbum.id;
                        }

                        imageUploader = new Imgur_v3(Program.UploadersConfig.ImgurOAuth2Info)
                        {
                            UploadMethod = Program.UploadersConfig.ImgurAccountType,
                            DirectLink = Program.UploadersConfig.ImgurDirectLink,
                            ThumbnailType = Program.UploadersConfig.ImgurThumbnailType,
                            UploadAlbumID = albumID
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
                    case ImageDestination.Twitter:
                        OAuthInfo twitterOAuth = Program.UploadersConfig.TwitterOAuthInfoList.ReturnIfValidIndex(Program.UploadersConfig.TwitterSelectedAccount);
                        imageUploader = new Twitter(twitterOAuth);
                        break;
                    case ImageDestination.Chevereto:
                        imageUploader = new Chevereto(Program.UploadersConfig.CheveretoWebsite, Program.UploadersConfig.CheveretoAPIKey)
                        {
                            DirectURL = Program.UploadersConfig.CheveretoDirectURL
                        };
                        break;
                    case ImageDestination.HizliResim:
                        imageUploader = new HizliResim()
                        {
                            DirectURL = true
                        };
                        break;
                    case ImageDestination.CustomImageUploader:
                        if (Program.UploadersConfig.CustomUploadersList.IsValidIndex(Program.UploadersConfig.CustomImageUploaderSelected))
                        {
                            imageUploader = new CustomImageUploader(Program.UploadersConfig.CustomUploadersList[Program.UploadersConfig.CustomImageUploaderSelected]);
                        }
                        break;

                    case ImageDestination.FileUploader:
                        return UploadFile(ssPath);
                }
            }

            if (imageUploader != null)
            {
                ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Uploading {0}.", Path.GetFileName(ssPath)));
                return imageUploader.Upload(ssPath);
            }

            return null;
        }

        private UploadResult UploadFile(string ssPath)
        {
            FileUploader fileUploader = null;

            switch (Program.Settings.ImageFileUploaderType)
            {
                case FileDestination.Dropbox:
                    fileUploader = new Dropbox(Program.UploadersConfig.DropboxOAuth2Info, Program.UploadersConfig.DropboxAccountInfo)
                    {
                        UploadPath = NameParser.Parse(NameParserType.URL, Dropbox.TidyUploadPath(Program.UploadersConfig.DropboxUploadPath)),
                        AutoCreateShareableLink = Program.UploadersConfig.DropboxAutoCreateShareableLink,
                        ShareURLType = Program.UploadersConfig.DropboxURLType
                    };
                    break;
                case FileDestination.Copy:
                    fileUploader = new Copy(Program.UploadersConfig.CopyOAuthInfo, Program.UploadersConfig.CopyAccountInfo)
                    {
                        UploadPath = NameParser.Parse(NameParserType.URL, Copy.TidyUploadPath(Program.UploadersConfig.CopyUploadPath)),
                        URLType = Program.UploadersConfig.CopyURLType
                    };
                    break;
                case FileDestination.GoogleDrive:
                    fileUploader = new GoogleDrive(Program.UploadersConfig.GoogleDriveOAuth2Info)
                    {
                        IsPublic = Program.UploadersConfig.GoogleDriveIsPublic,
                        FolderID = Program.UploadersConfig.GoogleDriveUseFolder ? Program.UploadersConfig.GoogleDriveFolderID : null
                    };
                    break;
                case FileDestination.RapidShare:
                    fileUploader = new RapidShare(Program.UploadersConfig.RapidShareUsername, Program.UploadersConfig.RapidSharePassword, Program.UploadersConfig.RapidShareFolderID);
                    break;
                case FileDestination.SendSpace:
                    fileUploader = new SendSpace(APIKeys.SendSpaceKey);
                    switch (Program.UploadersConfig.SendSpaceAccountType)
                    {
                        case AccountType.Anonymous:
                            SendSpaceManager.PrepareUploadInfo(APIKeys.SendSpaceKey);
                            break;
                        case AccountType.User:
                            SendSpaceManager.PrepareUploadInfo(APIKeys.SendSpaceKey, Program.UploadersConfig.SendSpaceUsername, Program.UploadersConfig.SendSpacePassword);
                            break;
                    }
                    break;
                case FileDestination.Minus:
                    fileUploader = new Minus(Program.UploadersConfig.MinusConfig, Program.UploadersConfig.MinusOAuth2Info);
                    break;
                case FileDestination.Box:
                    fileUploader = new Box(Program.UploadersConfig.BoxOAuth2Info)
                    {
                        FolderID = Program.UploadersConfig.BoxSelectedFolder.id,
                        Share = Program.UploadersConfig.BoxShare
                    };
                    break;
                case FileDestination.Gfycat:
                    fileUploader = new GfycatUploader();
                    break;
                case FileDestination.Ge_tt:
                    fileUploader = new Ge_tt(APIKeys.Ge_ttKey)
                    {
                        AccessToken = Program.UploadersConfig.Ge_ttLogin.AccessToken
                    };
                    break;
                case FileDestination.Localhostr:
                    fileUploader = new Hostr(Program.UploadersConfig.LocalhostrEmail, Program.UploadersConfig.LocalhostrPassword)
                    {
                        DirectURL = Program.UploadersConfig.LocalhostrDirectURL
                    };
                    break;
                case FileDestination.CustomFileUploader:
                    if (Program.UploadersConfig.CustomUploadersList.IsValidIndex(Program.UploadersConfig.CustomFileUploaderSelected))
                    {
                        fileUploader = new CustomFileUploader(Program.UploadersConfig.CustomUploadersList[Program.UploadersConfig.CustomFileUploaderSelected]);
                    }
                    break;
                case FileDestination.FTP:
                    FTPAccount account = Program.UploadersConfig.FTPAccountList.ReturnIfValidIndex(Program.UploadersConfig.FTPSelectedImage);

                    if (account != null)
                    {
                        if (account.Protocol == FTPProtocol.FTP || account.Protocol == FTPProtocol.FTPS)
                        {
                            fileUploader = new FTP(account);
                        }
                        else if (account.Protocol == FTPProtocol.SFTP)
                        {
                            fileUploader = new SFTP(account);
                        }
                    }
                    break;
                case FileDestination.SharedFolder:
                    int idLocalhost = Program.UploadersConfig.LocalhostSelectedImages;
                    if (Program.UploadersConfig.LocalhostAccountList.IsValidIndex(idLocalhost))
                    {
                        fileUploader = new SharedFolderUploader(Program.UploadersConfig.LocalhostAccountList[idLocalhost]);
                    }
                    break;
                case FileDestination.Email:
                    using (EmailForm emailForm = new EmailForm(Program.UploadersConfig.EmailRememberLastTo ? Program.UploadersConfig.EmailLastTo : string.Empty,
                        Program.UploadersConfig.EmailDefaultSubject, Program.UploadersConfig.EmailDefaultBody))
                    {
                        emailForm.Icon = ShareXResources.Icon;

                        if (emailForm.ShowDialog() == DialogResult.OK)
                        {
                            if (Program.UploadersConfig.EmailRememberLastTo)
                            {
                                Program.UploadersConfig.EmailLastTo = emailForm.ToEmail;
                            }

                            fileUploader = new Email
                            {
                                SmtpServer = Program.UploadersConfig.EmailSmtpServer,
                                SmtpPort = Program.UploadersConfig.EmailSmtpPort,
                                FromEmail = Program.UploadersConfig.EmailFrom,
                                Password = Program.UploadersConfig.EmailPassword,
                                ToEmail = emailForm.ToEmail,
                                Subject = emailForm.Subject,
                                Body = emailForm.Body
                            };
                        }
                    }
                    break;
                case FileDestination.Jira:
                    fileUploader = new Jira(Program.UploadersConfig.JiraHost, Program.UploadersConfig.JiraOAuthInfo, Program.UploadersConfig.JiraIssuePrefix);
                    break;
                case FileDestination.Mega:
                    fileUploader = new Mega(Program.UploadersConfig.MegaAuthInfos, Program.UploadersConfig.MegaParentNodeId);
                    break;
                case FileDestination.AmazonS3:
                    fileUploader = new AmazonS3(Program.UploadersConfig.AmazonS3Settings);
                    break;
                case FileDestination.OwnCloud:
                    fileUploader = new OwnCloud(Program.UploadersConfig.OwnCloudHost, Program.UploadersConfig.OwnCloudUsername, Program.UploadersConfig.OwnCloudPassword)
                    {
                        Path = Program.UploadersConfig.OwnCloudPath,
                        CreateShare = Program.UploadersConfig.OwnCloudCreateShare,
                        DirectLink = Program.UploadersConfig.OwnCloudDirectLink,
                        IgnoreInvalidCert = Program.UploadersConfig.OwnCloudIgnoreInvalidCert
                    };
                    break;
                case FileDestination.Pushbullet:
                    fileUploader = new Pushbullet(Program.UploadersConfig.PushbulletSettings);
                    break;
                case FileDestination.MediaCrush:
                    fileUploader = new MediaCrushUploader()
                    {
                        DirectLink = Program.UploadersConfig.MediaCrushDirectLink
                    };
                    break;
                case FileDestination.MediaFire:
                    fileUploader = new MediaFire(APIKeys.MediaFireAppId, APIKeys.MediaFireApiKey, Program.UploadersConfig.MediaFireUsername, Program.UploadersConfig.MediaFirePassword)
                    {
                        UploadPath = NameParser.Parse(NameParserType.URL, Program.UploadersConfig.MediaFirePath),
                        UseLongLink = Program.UploadersConfig.MediaFireUseLongLink
                    };
                    break;
                case FileDestination.Pomf:
                    fileUploader = new Pomf();
                    break;
            }

            if (fileUploader != null)
            {
                ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Uploading {0}.", Path.GetFileName(ssPath)));
                return fileUploader.Upload(ssPath);
            }

            return null;
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
                        ScreenshotInfo ss = userState as ScreenshotInfo;
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