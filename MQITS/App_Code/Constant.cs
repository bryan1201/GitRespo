using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Constant 的摘要描述
/// </summary>
public class Constant
{
    public static String DefaultErrorMssagePage = "ErrorMsg.aspx";
    public static String S_SPACE = " ";
    public static String S_Test = ConfigurationManager.AppSettings["Test"];
    public static String S_WebSiteVersion = ConfigurationManager.AppSettings["WebSiteVersion"];
    public static String S_WebSite = ConfigurationManager.AppSettings["WebSite"];
    public static String S_SQMPWebSite = ConfigurationManager.AppSettings["SQMPWebSite"];
    public static string S_FileRoot = ConfigurationManager.AppSettings["uploadfileroot"];
    public static string S_PublicFileRoot = ConfigurationManager.AppSettings["publicfileroot"];
    public static String S_PCBMURConnStr = "PCBMURConnStr";
    public static String S_MQITSConnStr = "MQITSConnStr";
    public static string MQITSConnectionString = ConfigurationManager.ConnectionStrings["MQITSConnectionString"].ConnectionString;
    //OleDbConnection ocn = new OleDbConnection(Constant.OLEDBMQITSConnectionString);
    public static string OLEDBMQITSConnectionString = ConfigurationManager.ConnectionStrings["OLEDBMQITSConnectionString"].ConnectionString;
    public static String DefaultSelect = "-- select one --";
    public static String DefaultMailServer = ConfigurationManager.AppSettings["MailServer"]; //2005/5/18 update
    public static String DefaultMailFrom = ConfigurationManager.AppSettings["MailFrom"];
    public static String DefaultMailBcc = ConfigurationManager.AppSettings["MailBcc"];
    public static String DefaultSelectAddNew = "-- add new one --";
    public static String DefaultSelectone = "-- select one --";
    public static string S_ADConnStr = "ADConnStr";
    public static string SUBMIT = "Submit";
    public static string SAVE = "Save";
    public static String S_UILang = "ENG";
    public static string LogonUserId = "";
    public static string SimUserId = "";

	public Constant()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
}
