using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Net.Mime;
using einvoice.Models.eInvoiceMessage;
using System.IO;
using System.Xml;
using System.Configuration;

namespace einvoice.Models
{
    
    public class PRDServerRAWDATACollection : IRawDataCollection
    {
        private string _url { get; set; }

        public PRDServerRAWDATACollection()
        {
            this._url = Constant.PRDServerFTP;
        }

        public string GetContent(string filepathname, string contenttype)
        {
            RawData ir = new RawData(this._url);
            return ir.GetContent(filepathname, contenttype);
        }

        public void SaveRawData(A0101 Invoice, string filename)
        {
            //Do nothing!
        }

        public string GetFtpUrl()
        {
            return this._url;
        }
    }

    public class QASServerRAWDATACollection : IRawDataCollection
    {
        private string _url { get; set; }

        public QASServerRAWDATACollection()
        {
            this._url = Constant.QASServerFTP;
        }

        public string GetContent(string filepathname, string contenttype)
        {
            RawData ir = new RawData(this._url);
            return ir.GetContent(filepathname, contenttype);
        }

        public void SaveRawData(A0101 Invoice, string filename)
        {
            //Do nothing!
        }

        public string GetFtpUrl()
        {
            return this._url;
        }
    }

    public class DEVServerRAWDATACollection : IRawDataCollection
    {
        private string _url { get; set; }

        public DEVServerRAWDATACollection()
        {
            this._url = Constant.DEVServerFTP;
        }

        public string GetContent(string filepathname, string contenttype)
        {
            RawData ir = new RawData(this._url);
            return ir.GetContent(filepathname, contenttype);
        }

        public void SaveRawData(A0101 Invoice, string filename)
        {
            //Do nothing!
        }

        string IRawDataCollection.GetFtpUrl()
        {
            return this._url;
        }
    }

    public class RawData
    {
        public string FilepathName { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        private const string _contentType = @"application/json; charset=utf-8";
        private string charSet { get; set; }

        public RawData(string _url)
        {
            this.Url = _url;
        }

        public RawData(string filepathname, string contenttype)
        {
            Init();
            this.FilepathName = (string.IsNullOrEmpty(filepathname) == true) ? FilepathName : filepathname;
            this.ContentType = (string.IsNullOrEmpty(contenttype) == true) ? ContentType : contenttype;
            string weburl = filepathname.Replace(Constant.Turnkeyfileroot, string.Empty).Replace("\\", "/");

            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(weburl);
            this.Content = FTPdownload(fullurl.ToString());
        }

        private void Init()
        {
            //http://10.1.251.202:50000/b2bmt/rest/rawdata/6e33c3f4-0c40-11e6-a6ca-0000004d0a76
            //http://iec1-piq:50000/b2bmt/rest/rawdata/6e33c3f4-0c40-11e6-a6ca-0000004d0a76
            this.Url = (string.IsNullOrEmpty(Url) == true) ? Constant.S_eInvoiceFTPServer : string.Empty;
            this.ContentType = (string.IsNullOrEmpty(ContentType) == true) ? _contentType : string.Empty;
            this.charSet = "UTF-8";
        }

        public void SaveRawData(A0101 Invoice, string filename)
        {
            throw new NotImplementedException();
        }

        public string GetContent(string filename, string contenttype)
        {
            contenttype = (string.IsNullOrEmpty(contenttype) == true) ? "UTF-8" : contenttype;
            this.FilepathName = (string.IsNullOrEmpty(filename) == true) ? FilepathName : filename;
            this.ContentType = (string.IsNullOrEmpty(contenttype) == true) ? ContentType : contenttype;
            string weburl = filename.Replace(Constant.Turnkeyfileroot, string.Empty).Replace("\\", "/");

            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(weburl);
            this.Content = FTPdownload(fullurl.ToString());
            return this.Content;
        }

        private string FTPdownload(string url)
        {
            string Rslt = string.Empty;
            try
            {
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(url);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(Constant.S_eInvoiceFTPUser, Constant.S_eInvoiceFTPPWD);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                Stream ftpStream = ftpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(ftpStream);
                Rslt = reader.ReadToEnd();
                if (url.EndsWith(".xml"))
                    Rslt = Constant.PrettyXml(Rslt);
                //XmlDocument xml = new XmlDocument();
                //xml.LoadXml(Rslt);
                //Rslt = xml.OuterXml;
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) {
                StringBuilder sb = new StringBuilder();
                sb.Append("Error:");
                sb.Append(" ");
                sb.Append(ex.Message);
                sb.Append("\r\n\r\n");
                sb.Append(new string('-',40));
                sb.Append("\r\n\r\n");
                sb.Append(Rslt);
                Rslt = sb.ToString();
            }
            return Rslt;
        }

        private string GetWebResponse(string url)
        {
            string response = string.Empty;
            try
            {
                var request = HttpWebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    var reqheader = request.GetResponse() as HttpWebResponse;

                    if (reqheader != null)
                    {
                        this.ContentType = reqheader.ContentType;
                        var contentType = new ContentType(this.ContentType);
                        this.charSet = (contentType.CharSet != "UTF-8") ? contentType.CharSet : "UTF-8";
                    }
                }
                /*
                var contentType = new ContentType(client.ResponseHeaders["Content-Type"]);
                Console.WriteLine("{0} ({1})", contentType.MediaType, contentType.CharSet);
                var charset = (contentType.CharSet ?? "UTF-8");
                */
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = this.ContentType;
                    client.UseDefaultCredentials = true;
                    client.Encoding = System.Text.Encoding.GetEncoding(this.charSet);//.GetEncoding(54936);
                    response = client.DownloadString(url);
                }
            }
            catch (WebException ex)
            {
                HttpContext con = HttpContext.Current;
                Constant.webRequestException(ex, con, url, out response);
            }
            return response;
        }
    }
}