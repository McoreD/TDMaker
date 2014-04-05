using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class TorrentInfoMgr
    {
        public List<TorrentInfo> TorrentInfos { get; set; }

        /// <summary>
        /// Options for Publishing
        /// </summary>
        public PublishOptionsPacket PublishOptions { get; set; }

        public TorrentInfoMgr()
        {
            this.TorrentInfos = new List<TorrentInfo>();
        }

        public TorrentInfoMgr(List<TorrentInfo> listTorentInfo)
        {
            this.TorrentInfos = listTorentInfo;
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
            sbPublish.Append(GetMediaInfo(tr.PublishInfo, options));

            return sbPublish.ToString();
        }

        public string GetMediaInfo(string p, PublishOptionsPacket options)
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
    }
}
