using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Meetings.Models;

namespace Meetings.Controllers.General
{
    public class Constant
    {
        public static string GetAppSettings(string key)
        {
            string rslt = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(key))
                    rslt = ConfigurationManager.AppSettings[key];
            }
            catch
            {
                // do nothing
            }
            return rslt;
        }

        public static string LOGIN = "登入";
        public static string SUCCESSFUL = "成功";
        public static string FAIL = "失敗";
        public static string DOUBLELOGIN = "已登入過";
        public static string NOTMEETINGDATEMEMBER = "非此次報名成員";
        public static string ABNORMALMEETINGDATEMEMBER = "超過未到會次數";
    }
}