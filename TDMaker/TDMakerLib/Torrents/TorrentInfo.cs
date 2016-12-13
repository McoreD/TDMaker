using ShareX.HelpersLib;
using ShareX.UploadersLib;
using ShareX.UploadersLib.FileUploaders;
using ShareX.UploadersLib.ImageUploaders;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UploadersLib.ImageUploaders;

namespace TDMakerLib
{
    public class TorrentInfo
    {
        public MediaInfo2 Media { get; set; }
        public string PublishString { get; set; }
        public PublishOptions PublishOptions { get; set; }

        private BackgroundWorker worker;

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

        /// <summary>
        /// Create Publish based on a Template
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public string CreatePublishExternal(PublishOptions options, TemplateReader tr)
        {
            tr.CreateInfo(options);

            return BbFormat(tr.PublishInfo, options);
        }

        public string CreatePublishMediaInfo(PublishOptions pop)
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

        public string CreatePublishInternal(PublishOptions pop)
        {
            StringBuilder sbPublish = new StringBuilder();
            string info = Media.Options.MediaTypeChoice == MediaType.MusicAudioAlbum ? Media.ToStringAudio() : Media.ToStringMedia(pop);
            sbPublish.Append(BbFormat(info, pop));

            return sbPublish.ToString().Trim();
        }

        private string BbFormat(string p, PublishOptions options)
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