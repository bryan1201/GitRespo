using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PIDBMonitor
{
    public class Constant
    {
        public static string ConnStr = ConfigurationManager.AppSettings["ConnStr"];
        public static string LOGONID = ConfigurationManager.AppSettings["LogonID"].ToString();
        public static string PASSWORD = ConfigurationManager.AppSettings["Password"].ToString();
        public static string SPACE = " ";
        public static string invMailServer = ConfigurationManager.AppSettings["invMailServer"].ToString();
        public static string invIMP4Port = ConfigurationManager.AppSettings["invIMP4Port"].ToString();
        public static string invDefaultMailboxLogonID = ConfigurationManager.AppSettings["invDefaultMailboxLogonID"].ToString();
        public static string invDefaultMailboxPassword = ConfigurationManager.AppSettings["invDefaultMailboxPassword"].ToString();
        public static string GamilServer = ConfigurationManager.AppSettings["GamilServer"].ToString();
        public static string GmailIMP4Port = ConfigurationManager.AppSettings["GmailIMP4Port"];
        public static int TimeInterval = int.Parse(ConfigurationManager.AppSettings["TimeInterval"]);
    }
}
