using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2BService.Models
{
    public class Constant
    {
        public static string PIQServer = ConfigurationManager.AppSettings["PIQServer"];
        public static string PIDServer = ConfigurationManager.AppSettings["PIDServer"];
        public static string PIPServer = ConfigurationManager.AppSettings["PIPServer"];

        public static string PIDUrl = ConfigurationManager.AppSettings["PIDUrl"];
        public static string PIQUrl = ConfigurationManager.AppSettings["PIQUrl"];
        public static string PIPUrl = ConfigurationManager.AppSettings["PIPUrl"];

        public static string ContentTypeUTF8 = @"application/json;";

        public static string PIPConnStr = ConfigurationManager.AppSettings["PIPConnStr"];
        public static string PIQConnStr = ConfigurationManager.AppSettings["PIQConnStr"];
        public static string PIDConnStr = ConfigurationManager.AppSettings["PIDConnStr"];

        public static string MDN = "MDN";
        public static string AuditLog = "AuditLog";
        public static string RawData = "RawData";

        public static IEnumerable<LOOKUP_DB> LookupMTProcessStep;
        public static IEnumerable<LOOKUP_DB> LookupMTPROCStatus;
        public static IEnumerable<LOOKUP_DB> LookupMTStatus;
        public static IEnumerable<LOOKUP_DB> LookupMTMsgDirection;//MT_MSG_DIRECTION
        public static IEnumerable<LOOKUP_DB> LookupMTAuditCheck;//MT_AUDIT_CHECK

        public static string MailFrom = ConfigurationManager.AppSettings["MailFrom"];
        public static string MailBcc = ConfigurationManager.AppSettings["MailBcc"];
        public static string MailTo = ConfigurationManager.AppSettings["MailTo"];
        public static string MailServer = ConfigurationManager.AppSettings["MailServer"];
        public static string MailTest = ConfigurationManager.AppSettings["MailTest"];
        public static string S_SPACE = " ";
        public static bool IsMailEnabled = bool.Parse(ConfigurationManager.AppSettings["IsMailEnabled"]);
        public static bool IsMailTest = bool.Parse(ConfigurationManager.AppSettings["IsMailTest"]);
        public static bool IsAlertTest = bool.Parse(ConfigurationManager.AppSettings["IsAlertTest"]);
        public static bool IsWebPerfLogEnabled = bool.Parse(ConfigurationManager.AppSettings["IsWebPerfLogEnabled"]);
        public static string B2BDBPerfURL = ConfigurationManager.AppSettings["B2BDBPerfURL"];

        public Constant()
        {

        }

        public static void Init()
        {
            ILOOKUPDBCollection LookupdbCollection;
            LOOKUP_DB db = new LOOKUP_DB();
            LookupdbCollection = DataAccess.CreateLOOKUPDBCollection(Constant.PIPServer);
            IEnumerable<LOOKUP_DB> LookupdbList = LookupdbCollection.Get(db);

            LookupMTProcessStep = LookupdbList.Where(x => x.TYPE == "MT_PROCESS_STEP").ToList();
            LookupMTStatus = LookupdbList.Where(x => x.TYPE == "MT_STATUS").ToList();
            LookupMTPROCStatus = LookupdbList.Where(x => x.TYPE == "MT_PROC_STATUS").ToList();
            LookupMTMsgDirection = LookupdbList.Where(x => x.TYPE == "MT_MSG_DIRECTION").ToList();
            LookupMTAuditCheck = LookupdbList.Where(x => x.TYPE == "MT_AUDIT_CHECK").ToList();
        }
    }
}