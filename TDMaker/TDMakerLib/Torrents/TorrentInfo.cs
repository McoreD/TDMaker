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
        public string PublishString { get; set; }

        public bool Success { get; set; }

        /// <summary>
        /// Create Publish based on a Template
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public static string ToStringPublishExternal(PublishOptions options, TemplateReader tr)
        {
            tr.CreateInfo(options);

            return BbFormat(tr.PublishInfo, options);
        }

        public static string ToStringPublishMediaInfo(TaskSettings ts)
        {
            StringBuilder sbPublish = new StringBuilder();

            switch (ts.Media.Options.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    StringBuilder sbMediaInfo = new StringBuilder();
                    if (ts.Media.MediaFiles.Count > 0)
                    {
                        foreach (MediaFile mf in ts.Media.MediaFiles)
                        {
                            sbMediaInfo.AppendLine(BbCode.Bold(mf.FileName));
                            sbMediaInfo.AppendLine(mf.Summary.Trim());
                            sbMediaInfo.AppendLine();
                        }
                    }
                    else
                    {
                        sbMediaInfo.AppendLine(ts.Media.Overall.Summary.Trim());
                        sbMediaInfo.AppendLine();
                    }

                    sbPublish.AppendLine(BbFormat(sbMediaInfo.ToString(), ts.PublishOptions));

                    if (ts.Media.Options.UploadScreenshots)
                        sbPublish.AppendLine(ts.Media.Overall.GetScreenshotString(ts.PublishOptions));

                    break;
                default:
                    foreach (MediaFile mf in ts.Media.MediaFiles)
                    {
                        sbMediaInfo = new StringBuilder();
                        sbMediaInfo.AppendLine(mf.Summary.Trim());
                        sbMediaInfo.AppendLine();

                        sbPublish.AppendLine(BbFormat(sbMediaInfo.ToString(), ts.PublishOptions));

                        if (ts.Media.Options.UploadScreenshots)
                        {
                            sbPublish.AppendLine();
                            sbPublish.AppendLine(mf.GetScreenshotString(ts.PublishOptions));
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

        public static string ToStringPublishInternal(TaskSettings ts)
        {
            StringBuilder sbPublish = new StringBuilder();
            string info = ts.Media.Options.MediaTypeChoice == MediaType.MusicAudioAlbum ? ts.Media.ToStringAudio() : ts.ToStringMedia();
            sbPublish.Append(BbFormat(info, ts.PublishOptions));

            return sbPublish.ToString().Trim();
        }

        private static string BbFormat(string p, PublishOptions options)
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
    }
}