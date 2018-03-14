using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace WWOMConverter
{
    class Constant
    {
        public static string S_SqlConnStr = "SqlConnStr";
        public static string S_DestTableUsers = ConfigurationManager.AppSettings["DestTableUsers"];//"dbo.srcHRPa";
        public static string S_DestTableDepts = ConfigurationManager.AppSettings["DestTableDepts"];//"dbo.srcHRDept";

        public static string S_SourceFilePath = ConfigurationManager.AppSettings["SourceFilePath"];
        public static string S_SourceFileDeptName = ConfigurationManager.AppSettings["SourceFileDept"];
        public static string S_SourceFilePaName = ConfigurationManager.AppSettings["SourceFilePa"];

        public static string S_SourceFileDept = S_SourceFilePath + S_SourceFileDeptName;
        public static string S_SourceFilePa = S_SourceFilePath + S_SourceFilePaName;

        public static string S_HRFileServer = ConfigurationManager.AppSettings["HRFileServer"];
        public static string S_HRFileServerDept = S_HRFileServer + S_SourceFileDeptName;
        public static string S_HRFileServerPa = S_HRFileServer + S_SourceFilePaName;
        public static string S_MappingExt = "txt";

        public static string S_ProgramLog = ConfigurationManager.AppSettings["ProgramLog"];
    }
}
