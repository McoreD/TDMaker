namespace TDMakerLib
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    internal class HttpWebMultipartRequest
    {
        private MemoryStream body;
        private string boundary = "-----------------------------7d430e355036c";
        private ICredentials credentials;
        private Encoding encoding;
        private HttpWebRequest request;

        public HttpWebMultipartRequest(string uri, CookieContainer cookie)
        {
            this.request = (HttpWebRequest) WebRequest.Create(uri);
            this.request.Method = "POST";
            this.request.KeepAlive = false;
            this.request.ContentType = "multipart/form-data; boundary=" + this.boundary;
            this.encoding = Encoding.Default;
            this.encoding = Encoding.GetEncoding("iso-8859-1");
            this.body = new MemoryStream();
            this.WriteBoundaryToMemoryStream();
            this.request.CookieContainer = cookie;
        }

        public void AddField(string name, string data)
        {
            this.WriteToMemoryStream(string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n", name, data));
            this.WriteBoundaryToMemoryStream();
        }

        public void AddFile(string name, string filename, string contentType, Stream data)
        {
            this.WriteToMemoryStream(string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", name, filename));
            this.WriteToMemoryStream(string.Format("\r\nContent-Type: {0}\r\n\r\n", contentType));
            BinaryReader reader = new BinaryReader(data);
            this.WriteToMemoryStream(reader.ReadBytes((int) data.Length));
            this.WriteToMemoryStream("\r\n");
            this.WriteBoundaryToMemoryStream();
            data.Close();
        }

        public HttpWebResponse GetResponse()
        {
            this.WriteToMemoryStream("--\r\n");
            this.request.ContentLength = this.body.Length;
            Stream requestStream = this.request.GetRequestStream();
            this.body.WriteTo(requestStream);
            requestStream.Close();
            try
            {
                return (HttpWebResponse) this.request.GetResponse();
            }
            catch (WebException exception)
            {
                if (exception.Status != WebExceptionStatus.KeepAliveFailure)
                {
                    throw exception;
                }
                return null;
            }
        }

        private void WriteBoundaryToMemoryStream()
        {
            this.WriteToMemoryStream("--" + this.boundary);
        }

        private void WriteToMemoryStream(byte[] bytes)
        {
            this.body.Write(bytes, 0, bytes.Length);
        }

        private void WriteToMemoryStream(string s)
        {
            this.body.Write(this.encoding.GetBytes(s), 0, this.encoding.GetByteCount(s));
        }

        public ICredentials Credentials
        {
            set
            {
                this.credentials = value;
            }
        }
    }
}

