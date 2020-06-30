using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using B2BService.Models;
using System.Text;

namespace B2BService.Models
{
    public class Method
    {
        private static char[] delimiterChars = { ';' };
        private static string _TEST = Constant.MailTest;
        public static bool SendMail(string subject, string body)
        {
            subject = string.IsNullOrEmpty(_TEST) ? subject : _TEST + subject;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(Constant.MailFrom);
            string[] bccArray = Constant.MailBcc.Split(delimiterChars);
            string[] ToArray = Constant.MailTo.Split(delimiterChars);

            foreach (string to in ToArray)
            {
                message.To.Add(new MailAddress(to));
            }

            foreach (string bcc in bccArray)
            {
                message.Bcc.Add(new MailAddress(bcc));
            }

            message.IsBodyHtml = true;
            message.Subject = subject;
            message.BodyEncoding = Encoding.UTF8;
            message.Body = body;
            SmtpClient client = new SmtpClient(Constant.MailServer);
            client.Send(message);

            return true;
        }

        public static string BuildXML2(string obj_value, string obj_name, string style)
        {
            if (obj_value == null)
                obj_value = "";
            StringBuilder myValue = new StringBuilder();
            myValue.Append("<");
            myValue.Append(obj_name);
            myValue.Append(" style='" + style + "'");
            myValue.Append(">");
            myValue.Append(obj_value);
            myValue.Append("</");
            myValue.Append(obj_name);
            myValue.Append(">");
            return myValue.ToString();
        }

        public static string BuildXML2(string obj_value, string obj_name)
        {
            if (obj_value == null)
                obj_value = "";
            StringBuilder myValue = new StringBuilder();
            myValue.Append("<");
            myValue.Append(obj_name);
            myValue.Append(">");
            myValue.Append(obj_value);
            myValue.Append("</");
            myValue.Append(obj_name);
            myValue.Append(">");
            return myValue.ToString();
        }

        public static string BuildXML(string obj_value, string obj_name)
        {
            if (obj_value == null)
                obj_value = "";
            StringBuilder myValue = new StringBuilder();
            myValue.Append("<");
            myValue.Append(obj_name);
            myValue.Append(">");
            myValue.Append(obj_value.Replace("'", "’").Replace("<", "＜").Replace(">", "＞"));
            myValue.Append("</");
            myValue.Append(obj_name);
            myValue.Append(">");
            return myValue.ToString();
        }

        public static string GrabXml(string str, string skey)
        {
            if (str == null)
            {
                return "";
            }
            int startlen = str.IndexOf("<" + skey + ">") + ("<" + skey + ">").Length;
            int len = str.IndexOf("</" + skey + ">") - startlen;

            string s_value = "";
            if (len > 0) { s_value = str.Substring(startlen, len); }

            return s_value;

        }

        public static string ChgXml(string str, string skey, string s_newvalue)
        {
            StringBuilder getStr = new StringBuilder();
            StringBuilder oldValue = new StringBuilder();
            StringBuilder newValue = new StringBuilder();
            getStr.Append(str);
            int exists = getStr.ToString().IndexOf(skey);
            if (exists < 0)
            {
                str = getStr.Append(BuildXML(s_newvalue, skey)).ToString();
            }
            else
            {
                oldValue.Append(BuildXML(GrabXml(str, skey), skey));
                newValue.Append(BuildXML(s_newvalue, skey));

                str = getStr.Replace(oldValue.ToString(), newValue.ToString()).ToString();
            }
            return str;
        }

        public static string GetSqlCmd(string s_proc, string vchType, string vchParameter)
        {
            StringBuilder sqlCmd = new StringBuilder("EXEC ");
            sqlCmd.Append(s_proc);
            sqlCmd.Append(Constant.S_SPACE);
            sqlCmd.Append(" @type = N'");
            sqlCmd.Append(vchType);
            sqlCmd.Append("',");
            sqlCmd.Append(" @parameters = N'");
            sqlCmd.Append(vchParameter);
            sqlCmd.Append("'");

            return sqlCmd.ToString();
        }

        public static string GetSqlCmd(string s_proc, string vchCmd, string vchObjectName, string vchSet)
        {
            StringBuilder sqlCmd = new StringBuilder("EXEC ");
            sqlCmd.Append(s_proc);
            sqlCmd.Append(Constant.S_SPACE);
            sqlCmd.Append(" @vchCmd = N'");
            sqlCmd.Append(vchCmd);
            sqlCmd.Append("',");
            sqlCmd.Append(" @vchObjectName = N'");
            sqlCmd.Append(vchObjectName);
            sqlCmd.Append("',");
            sqlCmd.Append(" @vchSet = N'");
            sqlCmd.Append(vchSet);
            sqlCmd.Append("'");

            return sqlCmd.ToString();
        }

        public static string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }
    }
}