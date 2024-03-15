using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace KMSharepointSync.Models
{
    public class Constant
    {

        public static string ReadContentUrl = ConfigurationManager.AppSettings["ReadContentUrl"];
        public static String S_SPACE = " ";
        public static String S_Title = ConfigurationManager.AppSettings["Title"];
        public static String NetworkCredentialUserId = ConfigurationManager.AppSettings["NetworkCredentialUserId"];
        public static String NetworkCredentialPWD = ConfigurationManager.AppSettings["NetworkCredentialPWD"];
        public static String WebRoot = ConfigurationManager.AppSettings["WebRoot"];

        public static String API_Key = ConfigurationManager.AppSettings["API_Key"];
        public static String TENANT = ConfigurationManager.AppSettings["TENANT"];
        public static String DataFormat = ConfigurationManager.AppSettings["DataFormat"];
        public static string KMUserId = ConfigurationManager.AppSettings["KMUserId"];
        public static string KMUserPassword = ConfigurationManager.AppSettings["KMUserPassword"];
        public static string KMUseremail = ConfigurationManager.AppSettings["KMUseremail"];
        public static string SharepointOnlineRoot = ConfigurationManager.AppSettings["SharepointOnlineRoot"];

        public static string GlobalDocClassId = "1"; //84
        public static string GlobalDocClassId84 = "84"; //84
        public static string GlobalCurrentCategoryId = "1";

        public static String S_ConnStr = "ConnStr";
        public static String DefaultSelect = "-- select one --";
        public static String DefaultSelectAddNew = "-- add new one --";
        public static String DefaultSelectone = "-- select one --";

        public static String PRDDBContext = ConfigurationManager.ConnectionStrings["PRDDBContext"].ToString();
        public static String QASDBContext = ConfigurationManager.ConnectionStrings["QASDBContext"].ToString();
        public static bool IsTracking = bool.Parse(ConfigurationManager.AppSettings["IsTracking"]);
        public static string TrackingSubject = ConfigurationManager.AppSettings["TrackingSubject"];
        public static string AICSharedDocument = @"https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/";

        public Constant()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
    }
}