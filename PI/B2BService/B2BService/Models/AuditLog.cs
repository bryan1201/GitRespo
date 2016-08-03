using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;

namespace B2BService.Models
{
    /*
    Get audit log
    URL : http://<host>:<port>/b2bmt/rest/AuditLog/<message id>
    Response:
        JSON
    Example: http://iec1-pid.sapecc.inventec:50000/b2bmt/rest/AuditLog/d63f4bec-0aa8-11e6-94dc-0000001201f6
    */

    public class AuditLog
    {
        public string msgID { get; set; }
        public IList<string> auditLogStrList { get; set; }
        public IList<string> childMsgList { get; set; }
        private string MessageId { get; set; }
        private string Url { get; set; }
        private string Content { get; set; }
        private string ContentType { get; set; }
        private const string _contentType = @"application/json; charset=utf-8";

        public AuditLog()
        {
            Init();
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(@"/");
            fullurl.Append(Constant.AuditLog);
            fullurl.Append(this.MessageId);
            this.Content = GetWebResponse(fullurl.ToString());
        }

        public AuditLog(string messageid, string url, string contenttype)
        {
            Init();
            this.MessageId = (string.IsNullOrEmpty(messageid) == true) ? MessageId : messageid;
            this.Url = (string.IsNullOrEmpty(url) == true) ? Url : url;
            this.ContentType = (string.IsNullOrEmpty(contenttype) == true) ? ContentType : contenttype;
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.AuditLog);
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            this.Content = GetWebResponse(fullurl.ToString());
        }

        private void Init()
        {
            this.MessageId = (string.IsNullOrEmpty(MessageId) == true) ? @"6e33c3f4-0c40-11e6-a6ca-0000004d0a76" : string.Empty;
            this.Url = (string.IsNullOrEmpty(Url) == true) ? Constant.PIQUrl : string.Empty;
            this.ContentType = (string.IsNullOrEmpty(ContentType) == true) ? _contentType : string.Empty;
        }

        public string Get()
        {
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.AuditLog);
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
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = this.ContentType;
                    client.UseDefaultCredentials = true;
                    client.Encoding = System.Text.Encoding.UTF8;
                    response = client.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
    }

    public class AuditLogContentList
    {
        public string logText { get; set; }
        public DateTime logTime { get; set; }
        public string logStatus { get; set; }
    }

    public class AuditLog2
    {
        public string msgID { get; set; }
        public IList<AuditLogContentList> AuditLogContentList { get; set; }
        public IList<string> childMsgList { get; set; }
        private string MessageId { get; set; }
        private string Url { get; set; }
        private string Content { get; set; }
        private string ContentType { get; set; }

        private const string _contentType = @"application/json; charset=utf-8";

        public AuditLog2()
        {
            Init();
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(@"/");
            fullurl.Append(Constant.AuditLog);
            fullurl.Append(this.MessageId);
            this.Content = GetWebResponse(fullurl.ToString());
        }

        public AuditLog2(string messageid, string url, string contenttype)
        {
            Init();
            this.MessageId = (string.IsNullOrEmpty(messageid) == true) ? MessageId : messageid;
            this.Url = (string.IsNullOrEmpty(url) == true) ? Url : url;
            this.ContentType = (string.IsNullOrEmpty(contenttype) == true) ? ContentType : contenttype;
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.AuditLog);
            fullurl.Append(@"/");
            fullurl.Append(this.MessageId);
            this.Content = GetWebResponse(fullurl.ToString());
        }

        private void Init()
        {
            this.MessageId = (string.IsNullOrEmpty(MessageId) == true) ? @"6e33c3f4-0c40-11e6-a6ca-0000004d0a76" : string.Empty;
            this.Url = (string.IsNullOrEmpty(Url) == true) ? Constant.PIQUrl : string.Empty;
            this.ContentType = (string.IsNullOrEmpty(ContentType) == true) ? _contentType : string.Empty;
        }

        public string Get()
        {
            StringBuilder fullurl = new StringBuilder();
            fullurl.Append(this.Url);
            fullurl.Append(Constant.AuditLog);
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
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = this.ContentType;
                    client.UseDefaultCredentials = true;
                    client.Encoding = System.Text.Encoding.UTF8;
                    response = client.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
    }
}