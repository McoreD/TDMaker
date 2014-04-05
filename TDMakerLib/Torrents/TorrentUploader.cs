using System.Text;
using System.Net;
using System.IO;

namespace TDMakerLib.Torrents
{
    public class TorrentUploader
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string HomePageURL { get; set; }
        public string LoginURL { get; set; }
        public string UploadURL { get; set; }

        private CookieContainer userCookie = null;

        public string Status { get; set; }

        public struct RequestInfo
        {
            /// <summary>
            /// Torrent Description
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// File Path of the Torrent file with .torrent extension
            /// </summary>
            public string FilePath { get; set; }
            /// <summary>
            /// Name of the Release
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// File Path of the NFO file
            /// </summary>
            public string NFO { get; set; }            
            public string Type { get; set; }
            /// <summary>
            /// URL of the Release
            /// </summary>
            public string URL { get; set; }

        }

        private void UploadTorrent(RequestInfo ri)
        {

            this.Status = "Status: Retrieving user cookie";

            this.userCookie = this.RetrieveUserCookie();
            if (this.userCookie == null)
            {
                this.Status = "Login Failed! Incorrect username and password!";
            }
            else
            {
                this.Status = "Status: Uploading to tracker";

                HttpWebMultipartRequest request = new HttpWebMultipartRequest(this.UploadURL, this.userCookie);
                request.AddField("MAX_FILE_SIZE", "1048576");
                request.AddFile("file", ri.FilePath, "application/x-bittorrent", new FileInfo(ri.FilePath).OpenRead());
                request.AddField("name", ri.Name);
                request.AddFile("nfo", ri.NFO, "application/octet-stream", new FileInfo(ri.NFO).OpenRead());
                request.AddField("url", ri.URL);
                request.AddField("descr", ri.Description);
                request.AddField("strip", "1");
                request.AddField("type", ri.Type);
                //if (this.rbUserName.Checked)
                //{
                //    request.AddField("upas", "self");
                //}
                //else
                //{
                //    request.AddField("upas", "anon");
                //}
                new FileInfo(ri.FilePath).Delete();
                string htmlCode = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
                if (!htmlCode.Contains("Successfully uploaded!"))
                {
                    // new WebBrowser(htmlCode).Show();
                    this.Status = "Status: Upload unsuccessfull";
                }
                else
                {
                    string str2 = this.HomePageURL;
                    int index = htmlCode.IndexOf("href=\"/download.php");
                    int num2 = htmlCode.IndexOf('"', index + 7);
                    HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(str2 + htmlCode.Substring(index + 7, num2 - (index + 7)));
                    request2.CookieContainer = this.userCookie;
                    request2.Method = "GET";
                    request2.ContentType = "application/x-www-form-urlencoded";
                    HttpWebResponse response = (HttpWebResponse)request2.GetResponse();
                    FileStream writeStream = new FileStream(ri.FilePath, FileMode.Create);
                    this.ReadWriteStream(response.GetResponseStream(), writeStream);
                    writeStream.Close();
                    //Process.Start(Path[Path.Length - 1] + ".torrent");
                    this.Status = "Status: Upload complete";
                    // this.button2.Enabled = false;
                }
            }
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int count = 0x100;
            byte[] buffer = new byte[count];
            for (int i = readStream.Read(buffer, 0, count); i > 0; i = readStream.Read(buffer, 0, count))
            {
                writeStream.Write(buffer, 0, i);
            }
            readStream.Close();
            writeStream.Close();
        }


        public CookieContainer RetrieveUserCookie()
        {
            CookieContainer container = new CookieContainer();
            ServicePointManager.Expect100Continue = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.LoginURL);
            request.CookieContainer = container;
            request.Method = "POST";
            request.KeepAlive = false;
            request.Referer = this.LoginURL + "?returnto=%2F";
            request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5\r\n";
            request.ContentType = "application/x-www-form-urlencoded";
            ASCIIEncoding encoding = new ASCIIEncoding();
            string s = "username=" + this.UserName + "&password=" + this.Password + "&returnto=%2F";
            byte[] bytes = encoding.GetBytes(s);
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            request.Expect = null;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            if (reader.ReadToEnd().Contains("Login failed!"))
            {
                return null;
            }
            return container;
        }

    }
}
