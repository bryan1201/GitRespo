using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Net.Mime;
namespace B2BService.Models
{
    /*
     <!--
    IEC1-WDQ 10.8.100.70 <-> 211.20.63.71
    IEC1-WDP 10.8.100.71 <-> 211.20.63.72
    -->
    
    Oracle DB Connection = b2bMT : Tpesappi7
    Query API的格式如下
    1. Get raw data
        URL : http://<host>:<port>/b2bmt/rest/rawdata/<message id>
        Response: raw data的內容(binary content)
        Example: http://iec1-pid.sapecc.inventec:50000/b2bmt/rest/rawdata/4eb70186-0a9e-11e6-cb25-0000001201f6

    2. Get MDN raw data
        URL : http://<host>:<port>/b2bmt/rest/rawdata/<message id>/mdn
        Response: raw data的內容(binary content)
        Example: http://iec1-pid.sapecc.inventec:50000/b2bmt/rest/rawdata/4eb70186-0a9e-11e6-cb25-0000001201f6/mdn

    3. Get audit log
        URL : http://<host>:<port>/b2bmt/rest/AuditLog/<message id>
        Response: JSON
        Example: http://iec1-pid.sapecc.inventec:50000/b2bmt/rest/AuditLog/d63f4bec-0aa8-11e6-94dc-0000001201f6

    附件為JSON的response content
        
    */
    public class RawData
    {
        public string MessageId { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set;}
        private const string _contentType = @"application/json; charset=utf-8";
        private string charSet { get; set; }

        public RawData()
        {
            Init();
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.RawData.ToLower());
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            this.Content = GetWebResponse(fullurl.ToString());
        }

        public RawData(string messageid, string url, string contenttype)
        {
            Init();
            this.MessageId = (string.IsNullOrEmpty(messageid) == true) ? MessageId : messageid;
            this.Url = (string.IsNullOrEmpty(url) == true) ? Url : url;
            this.ContentType = (string.IsNullOrEmpty(contenttype) == true) ? ContentType : contenttype;
            
            StringBuilder fullurl = new StringBuilder();
            string fullurl1 = string.Empty;
            string fullurl2 = string.Empty;
            try
            {
                fullurl.Append(this.Url);
                fullurl.Append(Constant.RawData.ToLower());
                fullurl.Append(@"/");
                fullurl.Append(this.MessageId);
                fullurl1 = fullurl.ToString();
                this.Content = GetWebResponse(fullurl.ToString());
                if (this.Content.Contains("WebException"))
                    throw new WebException();
            }
            catch
            {
                try
                {
                    fullurl.Clear();
                    fullurl.Append(this.Url);
                    fullurl.Append(Constant.Archiving_RawData.ToLower());
                    fullurl.Append(@"/");
                    fullurl.Append(this.MessageId);
                    fullurl2 = fullurl.ToString();
                    this.Content = GetWebResponse(fullurl.ToString());
                    if (this.Content.Contains("WebException"))
                        throw new WebException();
                }
                catch(Exception ex)
                {
                    fullurl.Clear();
                    fullurl.Append("Raw data file is not exist: \r\n ");
                    fullurl.Append(ex.Message);
                    fullurl.Append(" \r\n ");
                    fullurl.Append(fullurl1);
                    fullurl.Append(" AND \r\n ");
                    fullurl.Append(fullurl2);

                    this.Content = fullurl.ToString();
                }
            }
        }

        private void Init()
        {
            //http://10.1.251.202:50000/b2bmt/rest/rawdata/6e33c3f4-0c40-11e6-a6ca-0000004d0a76
            //http://iec1-piq:50000/b2bmt/rest/rawdata/6e33c3f4-0c40-11e6-a6ca-0000004d0a76
            this.MessageId = (string.IsNullOrEmpty(MessageId) == true) ? @"6e33c3f4-0c40-11e6-a6ca-0000004d0a76" : string.Empty;
            this.Url = (string.IsNullOrEmpty(Url) == true) ? Constant.PIQUrl: string.Empty;
            this.ContentType = (string.IsNullOrEmpty(ContentType) == true) ? _contentType : string.Empty;
        }

        public string Get()
        {
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.RawData.ToLower());
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            this.Content = GetWebResponse(fullurl.ToString());
            return Content;
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
            catch(WebException ex)
            {
                HttpContext con = HttpContext.Current;
                Constant.webRequestException(ex, con, url, out response);
            }
            return response;
        }

    }
}