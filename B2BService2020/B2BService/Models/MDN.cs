using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace B2BService.Models
{
    public class MDN
    {
        /*
        2. Get MDN raw data
        URL : http://<host>:<port>/b2bmt/rest/rawdata/<message id>/mdn
        Response: raw data的內容(binary content)
        Example: http://iec1-pid.sapecc.inventec:50000/b2bmt/rest/rawdata/4eb70186-0a9e-11e6-cb25-0000001201f6/mdn
        */
        public string MessageId { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        private const string _contentType = @"application/json; charset=utf-8";

        public MDN()
        {
            Init();
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.RawData.ToLower());
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            fullurl.Append(@"/");
            fullurl.Append(Constant.MDN.ToLower());
            this.Content = GetWebResponse(fullurl.ToString());
        }

        public MDN(string messageid, string url, string contenttype)
        {
            Init();
            this.MessageId = (string.IsNullOrEmpty(messageid) == true) ? MessageId : messageid;
            this.Url = (string.IsNullOrEmpty(url) == true) ? Url : url;
            this.ContentType = (string.IsNullOrEmpty(contenttype) == true) ? ContentType : contenttype;
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.RawData.ToLower());
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            fullurl.Append(@"/");
            fullurl.Append(Constant.MDN.ToLower());
            this.Content = GetWebResponse(fullurl.ToString());
        }

        private void Init()
        {
            //http://10.1.251.202:50000/b2bmt/rest/rawdata/6e33c3f4-0c40-11e6-a6ca-0000004d0a76
            //http://iec1-piq:50000/b2bmt/rest/rawdata/6e33c3f4-0c40-11e6-a6ca-0000004d0a76
            this.MessageId = (string.IsNullOrEmpty(MessageId) == true) ? @"6e33c3f4-0c40-11e6-a6ca-0000004d0a76" : string.Empty;
            this.Url = (string.IsNullOrEmpty(Url) == true) ? Constant.PIQUrl : string.Empty;
            this.ContentType = (string.IsNullOrEmpty(ContentType) == true) ? _contentType : string.Empty;
        }

        public string Get()
        {
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.RawData.ToLower());
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            fullurl.Append(@"/");
            fullurl.Append(Constant.MDN.ToLower());
            this.Content = GetWebResponse(fullurl.ToString());
            return Content;
        }

        private string GetWebResponse(string url)
        {
            string response = string.Empty;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = this.ContentType;
                    client.UseDefaultCredentials = true;
                    client.Encoding = System.Text.Encoding.UTF8;
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