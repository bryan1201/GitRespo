using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ConsoleApplication1
{
    public class Constant
    {
        public static String S_ConnStr = ConfigurationManager.AppSettings["ConnStr"];
        public static string S_SqlConnStr = "SqlConnStr";
        public static string S_ID = ConfigurationManager.AppSettings["WebServiceHRLoginID"];
        public static string S_PWD = ConfigurationManager.AppSettings["WebServiceHRPWD"];
        public static string S_DestTableUsers = ConfigurationManager.AppSettings["DestTableUsers"];//"dbo.tmpUsers";
        public static string S_DestTableDepts = ConfigurationManager.AppSettings["DestTableDepts"];//"dbo.tmpDepts";
        public static string S_ProgramLog = ConfigurationManager.AppSettings["ProgramLog"]; // Log Path for each commands
        public static string S_DestFilePath = ConfigurationManager.AppSettings["DestFilePath"];
        public static string S_DestFileDept = ConfigurationManager.AppSettings["DestFileDept"];
        public static string S_DestFileUser = ConfigurationManager.AppSettings["DestFileUser"];
    }
}
