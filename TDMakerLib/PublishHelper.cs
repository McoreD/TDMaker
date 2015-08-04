using ShareX.HelpersLib;
using ShareX.UploadersLib;
using ShareX.UploadersLib.FileUploaders;
using ShareX.UploadersLib.GUI;
using ShareX.UploadersLib.HelperClasses;
using ShareX.UploadersLib.ImageUploaders;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UploadersLib;
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

        private BackgroundWorker worker;
        private Uploader uploader;

        public bool Success { get; set; }

        public TorrentInfo(MediaInfo2 mi)
        {
            Media = mi;
            Success = true; // set to false if error
        }

        public TorrentInfo(BackgroundWorker bwApp, MediaInfo2 mi)
            : this(mi)
        {
            worker = bwApp;
        }

        private bool TakeScreenshot(MediaFile mf, string ssDir)
        {
            String mediaFilePath = mf.FilePath;

            mf.Thumbnailer = new Thumbnailer(mf, ssDir, App.Settings.ProfileActive);

            try
            {
                mf.Thumbnailer.TakeScreenshots(worker);
                ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, "Done taking Screenshot for " + Path.GetFileName(mediaFilePath));
            }
            catch (Exception ex)
            {
                Success = false;
                Debug.WriteLine(ex.ToString());
                ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, ex.Message + " for " + Path.GetFileName(mediaFilePath));
            }

            return Success;
        }

        public void CreateScreenshots(string ssDir)
        {
            switch (this.Media.Options.MediaTypeChoice)
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

        public void CreateScreenshots()
        {
            switch (Media.Options.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    if (TakeScreenshot(this.Media.Overall, FileSystem.GetScreenShotsDir(this.Media.Overall.FilePath)))
                        AddScreenshot(this.Media.Overall);
                    break;

                default:
                    foreach (MediaFile mf in this.Media.MediaFiles)
                    {
                        if (TakeScreenshot(mf, FileSystem.GetScreenShotsDir(mf.FilePath)))
                            AddScreenshot(mf);
                    }
                    break;
            }
        }

        private void AddScreenshot(MediaFile mf)
        {
            foreach (ScreenshotInfo ss in mf.Thumbnailer.Screenshots)
            {
                if (ss != null)
                {
                    ReportProgress(ProgressType.UPDATE_SCREENSHOTS_LIST, ss);
                }
            }
        }

        public void UploadScreenshots()
        {
            if (Media.Options.UploadScreenshots)
            {
                switch (Media.Options.MediaTypeChoice)
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
            if (Media.Options.UploadScreenshots)
            {
                for (int i = 0; i < mf.Thumbnailer.Screenshots.Count; i++)
                {
                    ScreenshotInfo ss = mf.Thumbnailer.Screenshots[i];
                    if (ss == null) continue;
                    ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Uploading {0} ({1} of {2})", Path.GetFileName(ss.LocalPath), i + 1, mf.Thumbnailer.Screenshots.Count));
                    UploadResult ur = UploadScreenshot(ss.LocalPath);

                    if (ur == null) continue;
                    if (!string.IsNullOrEmpty(ur.URL))
                    {
                        ss.FullImageLink = ur.URL;
                        ss.LinkedThumbnail = ur.ThumbnailURL;
                    }
                    else
                    {
                        Success = false;
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
                        Adapter.ScheduleFileForDeletion(ssPath);
                    }
                }
                else
                {
                    ReportProgress(ProgressType.UPDATE_STATUSBAR_DEBUG, string.Format("Failed uploading {0}. Try again later.", Path.GetFileName(ssPath)));
                    Success = false;
                }
            }

            return ur;
        }

        private UploadResult UploadImage(string ssPath)
        {
            ImageUploader imageUploader = null;

            if (!string.IsNullOrEmpty(App.Settings.PtpImgCode))
            {
                imageUploader = new PtpImageUploader(Crypt.Decrypt(App.Settings.PtpImgCode));
            }
            else
            {
                switch (App.Settings.ProfileActive.ImageUploaderType)
                {
                    case ImageDestination.Imgur:
                        if (App.UploadersConfig.ImgurOAuth2Info == null)
                        {
                            App.UploadersConfig.ImgurOAuth2Info = new OAuth2Info(APIKeys.ImgurClientID, APIKeys.ImgurClientSecret);
                        }

                        string albumID = null;

                        if (App.UploadersConfig.ImgurUploadSelectedAlbum && App.UploadersConfig.ImgurSelectedAlbum != null)
                        {
                            albumID = App.UploadersConfig.ImgurSelectedAlbum.id;
                        }

                        imageUploader = new Imgur(App.UploadersConfig.ImgurOAuth2Info)
                        {
                            UploadMethod = App.UploadersConfig.ImgurAccountType,
                            DirectLink = App.UploadersConfig.ImgurDirectLink,
                            ThumbnailType = App.UploadersConfig.ImgurThumbnailType,
                            UploadAlbumID = albumID
                        };
                        break;
                    case ImageDestination.TinyPic:
                        imageUploader = new TinyPicUploader(APIKeys.TinyPicID, APIKeys.TinyPicKey, App.UploadersConfig.TinyPicAccountType, App.UploadersConfig.TinyPicRegistrationCode);
                        break;
                    case ImageDestination.Flickr:
                        imageUploader = new FlickrUploader(APIKeys.FlickrKey, APIKeys.FlickrSecret, App.UploadersConfig.FlickrAuthInfo, App.UploadersConfig.FlickrSettings);
                        break;
                    case ImageDestination.Photobucket:
                        imageUploader = new Photobucket(App.UploadersConfig.PhotobucketOAuthInfo, App.UploadersConfig.PhotobucketAccountInfo);
                        break;
                    case ImageDestination.Picasa:
                        imageUploader = new Picasa(App.UploadersConfig.PicasaOAuth2Info)
                        {
                            AlbumID = App.UploadersConfig.PicasaAlbumID
                        };
                        break;
                    case ImageDestination.Twitter:
                        OAuthInfo twitterOAuth = App.UploadersConfig.TwitterOAuthInfoList.ReturnIfValidIndex(App.UploadersConfig.TwitterSelectedAccount);
                        imageUploader = new Twitter(twitterOAuth);
                        break;
                    case ImageDestination.Chevereto:
                        imageUploader = new Chevereto(App.UploadersConfig.CheveretoWebsite, App.UploadersConfig.CheveretoAPIKey)
                        {
                            DirectURL = App.UploadersConfig.CheveretoDirectURL
                        };
                        break;
                    case ImageDestination.HizliResim:
                        imageUploader = new HizliResim()
                        {
                            DirectURL = true
                        };
                        break;
                    case ImageDestination.CustomImageUploader:
                        if (App.UploadersConfig.CustomUploadersList.IsValidIndex(App.UploadersConfig.CustomImageUploaderSelected))
                        {
                            imageUploader = new CustomImageUploader(App.UploadersConfig.CustomUploadersList[App.UploadersConfig.CustomImageUploaderSelected]);
                        }
                        break;
                    case ImageDestination.FileUploader:
                        return UploadFile(ssPath);
                }
            }

            if (imageUploader != null)
            {
                PrepareUploader(imageUploader);

                return imageUploader.Upload(ssPath);
            }

            return null;
        }

        private void PrepareUploader(Uploader currentUploader)
        {
            uploader = currentUploader;
            uploader.BufferSize = (int)Math.Pow(2, App.Settings.BufferSizePower) * 1024;
            uploader.ProgressChanged += uploader_ProgressChanged;
        }

        private void uploader_ProgressChanged(ProgressManager progress)
        {
            if (progress != null)
            {
                ReportProgress(ProgressType.UPDATE_PROGRESSBAR_ProgressManager, progress);
            }
        }

        private UploadResult UploadFile(string ssPath)
        {
            FileUploader fileUploader = null;

            switch (App.Settings.ProfileActive.ImageFileUploaderType)
            {
                case FileDestination.Dropbox:
                    fileUploader = new Dropbox(App.UploadersConfig.DropboxOAuth2Info, App.UploadersConfig.DropboxAccountInfo)
                    {
                        UploadPath = NameParser.Parse(NameParserType.URL, Dropbox.TidyUploadPath(App.UploadersConfig.DropboxUploadPath)),
                        AutoCreateShareableLink = App.UploadersConfig.DropboxAutoCreateShareableLink,
                        ShareURLType = App.UploadersConfig.DropboxURLType
                    };
                    break;
                case FileDestination.Copy:
                    fileUploader = new Copy(App.UploadersConfig.CopyOAuthInfo, App.UploadersConfig.CopyAccountInfo)
                    {
                        UploadPath = NameParser.Parse(NameParserType.URL, Copy.TidyUploadPath(App.UploadersConfig.CopyUploadPath)),
                        URLType = App.UploadersConfig.CopyURLType
                    };
                    break;
                case FileDestination.GoogleDrive:
                    fileUploader = new GoogleDrive(App.UploadersConfig.GoogleDriveOAuth2Info)
                    {
                        IsPublic = App.UploadersConfig.GoogleDriveIsPublic,
                        FolderID = App.UploadersConfig.GoogleDriveUseFolder ? App.UploadersConfig.GoogleDriveFolderID : null
                    };
                    break;
                case FileDestination.SendSpace:
                    fileUploader = new SendSpace(APIKeys.SendSpaceKey);
                    switch (App.UploadersConfig.SendSpaceAccountType)
                    {
                        case AccountType.Anonymous:
                            SendSpaceManager.PrepareUploadInfo(APIKeys.SendSpaceKey);
                            break;
                        case AccountType.User:
                            SendSpaceManager.PrepareUploadInfo(APIKeys.SendSpaceKey, App.UploadersConfig.SendSpaceUsername, App.UploadersConfig.SendSpacePassword);
                            break;
                    }
                    break;
                case FileDestination.Minus:
                    fileUploader = new Minus(App.UploadersConfig.MinusConfig, App.UploadersConfig.MinusOAuth2Info);
                    break;
                case FileDestination.Box:
                    fileUploader = new Box(App.UploadersConfig.BoxOAuth2Info)
                    {
                        FolderID = App.UploadersConfig.BoxSelectedFolder.id,
                        Share = App.UploadersConfig.BoxShare
                    };
                    break;
                case FileDestination.Gfycat:
                    fileUploader = new GfycatUploader();
                    break;
                case FileDestination.Ge_tt:
                    fileUploader = new Ge_tt(APIKeys.Ge_ttKey)
                    {
                        AccessToken = App.UploadersConfig.Ge_ttLogin.AccessToken
                    };
                    break;
                case FileDestination.Localhostr:
                    fileUploader = new Hostr(App.UploadersConfig.LocalhostrEmail, App.UploadersConfig.LocalhostrPassword)
                    {
                        DirectURL = App.UploadersConfig.LocalhostrDirectURL
                    };
                    break;
                case FileDestination.CustomFileUploader:
                    if (App.UploadersConfig.CustomUploadersList.IsValidIndex(App.UploadersConfig.CustomFileUploaderSelected))
                    {
                        fileUploader = new CustomFileUploader(App.UploadersConfig.CustomUploadersList[App.UploadersConfig.CustomFileUploaderSelected]);
                    }
                    break;
                case FileDestination.FTP:
                    FTPAccount account = App.UploadersConfig.FTPAccountList.ReturnIfValidIndex(App.UploadersConfig.FTPSelectedImage);

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
                    int idLocalhost = App.UploadersConfig.LocalhostSelectedImages;
                    if (App.UploadersConfig.LocalhostAccountList.IsValidIndex(idLocalhost))
                    {
                        fileUploader = new SharedFolderUploader(App.UploadersConfig.LocalhostAccountList[idLocalhost]);
                    }
                    break;
                case FileDestination.Email:
                    using (EmailForm emailForm = new EmailForm(App.UploadersConfig.EmailRememberLastTo ? App.UploadersConfig.EmailLastTo : string.Empty,
                        App.UploadersConfig.EmailDefaultSubject, App.UploadersConfig.EmailDefaultBody))
                    {
                        emailForm.Icon = ShareXResources.Icon;

                        if (emailForm.ShowDialog() == DialogResult.OK)
                        {
                            if (App.UploadersConfig.EmailRememberLastTo)
                            {
                                App.UploadersConfig.EmailLastTo = emailForm.ToEmail;
                            }

                            fileUploader = new Email
                            {
                                SmtpServer = App.UploadersConfig.EmailSmtpServer,
                                SmtpPort = App.UploadersConfig.EmailSmtpPort,
                                FromEmail = App.UploadersConfig.EmailFrom,
                                Password = App.UploadersConfig.EmailPassword,
                                ToEmail = emailForm.ToEmail,
                                Subject = emailForm.Subject,
                                Body = emailForm.Body
                            };
                        }
                    }
                    break;
                case FileDestination.Jira:
                    fileUploader = new Jira(App.UploadersConfig.JiraHost, App.UploadersConfig.JiraOAuthInfo, App.UploadersConfig.JiraIssuePrefix);
                    break;
                case FileDestination.Mega:
                    fileUploader = new Mega(App.UploadersConfig.MegaAuthInfos, App.UploadersConfig.MegaParentNodeId);
                    break;
                case FileDestination.AmazonS3:
                    fileUploader = new AmazonS3(App.UploadersConfig.AmazonS3Settings);
                    break;
                case FileDestination.OwnCloud:
                    fileUploader = new OwnCloud(App.UploadersConfig.OwnCloudHost, App.UploadersConfig.OwnCloudUsername, App.UploadersConfig.OwnCloudPassword)
                    {
                        Path = App.UploadersConfig.OwnCloudPath,
                        CreateShare = App.UploadersConfig.OwnCloudCreateShare,
                        DirectLink = App.UploadersConfig.OwnCloudDirectLink,
                        IgnoreInvalidCert = App.UploadersConfig.OwnCloudIgnoreInvalidCert
                    };
                    break;
                case FileDestination.Pushbullet:
                    fileUploader = new Pushbullet(App.UploadersConfig.PushbulletSettings);
                    break;
                case FileDestination.MediaFire:
                    fileUploader = new MediaFire(APIKeys.MediaFireAppId, APIKeys.MediaFireApiKey, App.UploadersConfig.MediaFireUsername, App.UploadersConfig.MediaFirePassword)
                    {
                        UploadPath = NameParser.Parse(NameParserType.URL, App.UploadersConfig.MediaFirePath),
                        UseLongLink = App.UploadersConfig.MediaFireUseLongLink
                    };
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
        public string CreatePublishExternal(PublishOptionsPacket options, TemplateReader tr)
        {
            tr.CreateInfo(options);

            return BbFormat(tr.PublishInfo, options);
        }

        public string CreatePublishMediaInfo(PublishOptionsPacket pop)
        {
            StringBuilder sbPublish = new StringBuilder();

            switch (Media.Options.MediaTypeChoice)
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

                    sbPublish.AppendLine(BbFormat(sbMediaInfo.ToString(), pop));

                    if (Media.Options.UploadScreenshots)
                        sbPublish.AppendLine(Media.Overall.GetScreenshotString(pop));

                    break;
                default:
                    foreach (MediaFile mf in Media.MediaFiles)
                    {
                        sbMediaInfo = new StringBuilder();
                        sbMediaInfo.AppendLine(mf.Summary.Trim());
                        sbMediaInfo.AppendLine();

                        sbPublish.AppendLine(BbFormat(sbMediaInfo.ToString(), pop));

                        if (Media.Options.UploadScreenshots)
                        {
                            sbPublish.AppendLine();
                            sbPublish.AppendLine(mf.GetScreenshotString(pop));
                        }
                    }

                    break;
            }

            string publishInfo = sbPublish.ToString().Trim();

            if (App.Settings.ProfileActive.HidePrivateInfo)
            {
                publishInfo = Regex.Replace(publishInfo, "(?<=Complete name *: ).+?(?=\\r)", match => Path.GetFileName(match.Value));
            }

            return publishInfo;
        }

        public string CreatePublishInternal(PublishOptionsPacket pop)
        {
            StringBuilder sbPublish = new StringBuilder();
            string info = Media.Options.MediaTypeChoice == MediaType.MusicAudioAlbum ? Media.ToStringAudio() : Media.ToStringMedia(pop);
            sbPublish.Append(BbFormat(info, pop));

            return sbPublish.ToString().Trim();
        }

        private void ReportProgress(ProgressType percentProgress, object userState)
        {
            if (worker != null)
            {
                worker.ReportProgress((int)percentProgress, userState);
            }
            else
            {
                switch (percentProgress)
                {
                    case ProgressType.UPDATE_STATUSBAR_DEBUG:
                        Debug.WriteLine((string)userState);
                        break;
                    case ProgressType.UPDATE_SCREENSHOTS_LIST:
                        ScreenshotInfo ss = userState as ScreenshotInfo;
                        Debug.WriteLine("Screenshot: " + ss.FullImageLink);
                        break;
                }
            }
        }

        private string BbFormat(string p, PublishOptionsPacket options)
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

            return sbPublish.ToString().Trim();
        }

        public override string ToString()
        {
            return Path.GetFileName(Media.Location);
        }
    }
}