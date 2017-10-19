using System;
using System.Configuration;
using System.Web.Mvc;
using System.Collections.Generic;

namespace CERLLAB.Controllers.General
{
    public class Constant
    {
        public static String DefaultErrorMssagePage = "GenericErrorPage.htm";
        public static String S_SPACE = " ";
        public static String S_Title = ConfigurationManager.AppSettings["Title"];
        public static String S_WebSite = ConfigurationManager.AppSettings["WebSite"];
        public static String RoutesWebSite = ConfigurationManager.AppSettings["RoutesWebSite"];
        public static String NetworkCredentialUserId = ConfigurationManager.AppSettings["NetworkCredentialUserId"];
        public static String NetworkCredentialPWD = ConfigurationManager.AppSettings["NetworkCredentialPWD"];
        public static String FcerlFlowCode = ConfigurationManager.AppSettings["FcerlFlowCode"];
        public static String AddAttachFcerlFlowCode = ConfigurationManager.AppSettings["AddAttachFcerlFlowCode"];
        public static String S_FileSite = ConfigurationManager.AppSettings["FileSite"];
        public static String S_FileSize = ConfigurationManager.AppSettings["FileSize"];
        public static string S_FileRoot = ConfigurationManager.AppSettings["FileRoot"];
        public static String S_Service = ConfigurationManager.AppSettings["Service"];
        public static String DefaultMailServer = ConfigurationManager.AppSettings["MailServer"];
        public static String DefaultMailFrom = ConfigurationManager.AppSettings["MailFrom"];
        public static String DefaultMailBcc = ConfigurationManager.AppSettings["MailBcc"];
        public static String MailTest = ConfigurationManager.AppSettings["MailTest"];
        public static String EnableSimulationMode = ConfigurationManager.AppSettings["EnableSimulationMode"];
        public static string OutResultLimit = ConfigurationManager.AppSettings["OutResultLimit"];
        public static string PageSize = ConfigurationManager.AppSettings["PageSize"];
        
        public static int iOutResultLimit = int.Parse(OutResultLimit);
        public static int iPageSize = int.Parse(PageSize);
        
        public static String WebRoot = ConfigurationManager.AppSettings["WebRoot"];
        public static String LabInformation = ConfigurationManager.AppSettings["LabInformation"];
        public static String UserFileDirectory = ConfigurationManager.AppSettings["UserFileDirectory"];
        public static String WebFileDirectory = ConfigurationManager.AppSettings["WebFileDirectory"];
        public static String UserWebPage = ConfigurationManager.AppSettings["UserWebPage"];
        public static String UserContactPage = ConfigurationManager.AppSettings["UserContactPage"];
        public static String ExtTypes = ConfigurationManager.AppSettings["ExtTypes"];

        public static String S_ConnStr = "ConnStr";
        public static String DefaultSelect = "-- select one --";
        public static String DefaultSelectAddNew = "-- add new one --";
        public static String DefaultSelectone = "-- select one --";
        public static string LogonUserId = "";
        public static String ConnStrTSB = "PRSConnStr";
        public static String ConnCERLDBContext = "CERLDBContext";
        public static List<SelectListItem> UserRoleList = new List<SelectListItem>();
        public static int UserRoleId = 2;

        public Constant()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

    }
}