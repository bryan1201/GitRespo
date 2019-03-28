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
using System.Collections;
using System.Text;

/// <summary>
/// Method 的摘要描述
/// </summary>
public class Method
{
    private const string sp_Method = "sp_Method";
    private const string sp_TreeMenuRole = "sp_TreeMenuRole";
    private const string ErrorPage = "ErrorPage.aspx";
    private const string DefaultPage = "MyIssueAction.aspx";
	public Method()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    public static Hashtable Hashtable_Parse(string s)
    {
        Hashtable h = new Hashtable();
        DataSet ds = new DataSet();

        ds = DataSet_Parse(s);

        int i = 0;
        while (i < ds.Tables["querytable"].Columns.Count)
        {

            h.Add(ds.Tables["querytable"].Columns[i].ToString(), ds.Tables["querytable"].Rows[0][i].ToString());
            i++;
        }

        return h;
    }
    public static String Hashtable_Parse(Hashtable h)
    {
        int i = 0;

        DataSet ds = new DataSet();
        ds.Tables.Add("querytable");
        string[] al = new string[h.Count];


        System.Collections.IDictionaryEnumerator ie = h.GetEnumerator();

        while (ie.MoveNext())
        {
            al.SetValue(ie.Value.ToString(), i);
            ds.Tables["querytable"].Columns.Add(ie.Key.ToString());
            i++;
        }

        ds.Tables[0].Rows.Add(al);

        return DataSet_Parse(ds);

    }

    public static String DataSet_Parse(DataSet ds)
    {
        return ds.GetXml();
    }
    //s must xml string
    public static DataSet DataSet_Parse(string s)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(new System.Xml.XmlTextReader(new System.IO.StringReader(s)));

        return ds;
    }
    public static object h_set_Value(Hashtable h, String _key, String _value)
    {

        h.Remove(_key);
        h.Add(_key, _value);
        return h;
    }

    public static object h_get_Value(Hashtable h, String Key)
    {

        IDictionaryEnumerator c = h.GetEnumerator();

        while (c.MoveNext())
        {
            while ((c.Key.ToString()) == Key) return c.Value;
        }
        return "nodata";
    }

    public static string BuildXML(string obj_value, string obj_name)
    {
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
            //str = str + BuildXML(s_value, skey);
            str = getStr.Append(BuildXML(s_newvalue, skey)).ToString();
        }
        else
        {
            //str = str.Replace("<" + skey + ">" + GrabXml(str, skey) + "</" + skey + ">", "<" + skey + ">" + s_value + "</" + skey + ">");
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

    public static string GetCurrentUserId(Page pp)
    {
        string UserId = (string)pp.Request["LOGON_USER"].ToUpper();
        int iDomain = UserId.IndexOf(@"\", 0);
        if (iDomain > 0 && UserId.Length > 9)
            UserId = UserId.Substring(iDomain + 1);

        if (pp.Session["UserId"] != null)
        {
            UserId = pp.Session["UserId"].ToString();
            iDomain = UserId.IndexOf(@"\", 0);
            if (iDomain > 0 && UserId.Length > 9)
                UserId = UserId.Substring(iDomain + 1);
        }

        UserId = UserId.Replace(@"IEC\", "");
        UserId = UserId.Replace(@"IES\", "");
        UserId = UserId.Replace(@"ICZ\", "");
        /*
        string CurrentPage = GetCurrentPageName();
        bool IsEnable = IsEnableCurrentPage(UserId, CurrentPage);

        if (CurrentPage != DefaultPage && CurrentPage != ErrorPage)
        {
            if (IsEnable == false)
            {
                pp.Response.Redirect(ErrorPage + "?CurrentPage=" + CurrentPage);
            }
        }
        */
        return UserId;
    }

    public static string GetCurrentPageName()
    {
        string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        string sRet = oInfo.Name;
        return sRet;
    }

    public static bool IsEnableCurrentPage(string user_id, string navigate_url)
    {
        string [] sRslt = { "true" };
        string vchType = "AUTHORITY_OF_CURRENTWEBPAGE";
        StringBuilder vchParameters = new StringBuilder();
        vchParameters.Append(Method.BuildXML(user_id, "user_id"));
        vchParameters.Append(Method.BuildXML(navigate_url, "navigate_url"));

        string sqlCmd = GetSqlCmd(sp_TreeMenuRole, vchType, vchParameters.ToString());
        sRslt = DAO.sqlCmdArrSingleCol(Constant.S_MQITSConnStr, sqlCmd.ToString());
        bool IsEnable =true;
        bool.TryParse(sRslt[0].ToString(), out IsEnable);
        return IsEnable;
    }

    public static string GetLogonUserId(Page pp)
    {
        string UserId = (string)pp.Request["LOGON_USER"].ToUpper();
        int iDomain = UserId.IndexOf(@"\", 0);

        iDomain = UserId.IndexOf(@"\", 0);
        if (iDomain > 0 && UserId.Length > 9)
            UserId = UserId.Substring(iDomain + 1);

        try
        {
            UserId = UserId.Replace(@"IEC\", "");
            UserId = UserId.Replace(@"IES\", "");
            UserId = UserId.Replace(@"ICZ\", "");
        }
        catch
        { 
        
        }

        return UserId;
    }

    public static bool EnableSendmail(Page pp)
    {
        bool enablesendmail = true;
        bool.TryParse(pp.Session["EnableSendMail"].ToString(), out enablesendmail);
        return enablesendmail;
    }

    public static string GetUserName(string UserId)
    {
        string[] sRslt = { "UserName" };
        string UserName = "";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(UserId, "editor"));

        string sqlCmd = GetSqlCmd(sp_Method, "Get", "UserName", vchSet.ToString());
        sRslt = DAO.sqlCmdArrSingleCol(Constant.S_MQITSConnStr, sqlCmd.ToString());
        UserName = sRslt[0];
        return UserName;
    }
   
    public static string GetUserNameByeMail(string eMailAddress)
    {
        string[] sRslt = { "UserName" };
        string UserName = "";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(eMailAddress, "email"));

        string sqlCmd = GetSqlCmd(sp_Method, "GetUserNameByeMailAddress", "m_userprofile", vchSet.ToString());
        sRslt = DAO.sqlCmdArrSingleCol(Constant.S_MQITSConnStr, sqlCmd.ToString());
        UserName = sRslt[0];
        return UserName;
    }

    public static string GetUsereMailByUserId(string UserId)
    {
        string[] sRslt = { "eMailAddress" };
        string eMailAddress = "";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(UserId, "editor"));

        string sqlCmd = GetSqlCmd(sp_Method, "GetUsereMailByUserId", "m_userprofile", vchSet.ToString());
        sRslt = DAO.sqlCmdArrSingleCol(Constant.S_MQITSConnStr, sqlCmd.ToString());
        eMailAddress = sRslt[0];
        return eMailAddress;
    }
    public static void MessageOut(Page pp, string sMsg)
    {
        string sScript = @"<script language='javascript'>alert('" + sMsg + "');</script>";
        pp.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript);
    }

    public static void MessageOut(Page pp, string sMsg, bool IsCloseSrcWindow)
    {
        string sScript = @"<script language='javascript'>alert('" + sMsg + "');</script>";
        sScript += "<script language='javascript'>window.opener=null;window.close();</script>";
        pp.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript);
    }

    public static void OpenWindows(Page pp, string url, string WindowName)
    {
        StringBuilder sScript = new StringBuilder();
        sScript.Append(@"<script language='javascript'>");
        sScript.Append("frmHeight=screen.availHeight-80;");
        sScript.Append("leftVal=(screen.availWidth-850)/2;");
        sScript.Append("topVal=(screen.height-frmHeight)/8;");
        sScript.Append("popUp=window.open('");
        sScript.Append(url);
        sScript.Append("', '" + WindowName + "','");
        sScript.Append("height=' + frmHeight + ',");
        sScript.Append("width=850,");
        sScript.Append("toolbar=no,locatiion=yes,resizable=yes,status=yes,menubar=no,scrollbars=yes,");
        sScript.Append("left=' + leftVal + ',");
        sScript.Append("top=' + topVal + '");
        sScript.Append("')");
        sScript.Append(@"</script>");
        pp.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript.ToString());
    }

    public static void OpenWindows(Page pp, string url, string WindowName, int width)
    {
        StringBuilder sScript = new StringBuilder();
        sScript.Append(@"<script language='javascript'>");
        sScript.Append("frmHeight=screen.availHeight-80;");
        sScript.Append("leftVal=(screen.availWidth-" + width.ToString() + ")/2;");
        sScript.Append("topVal=(screen.height-frmHeight)/8;");
        sScript.Append("popUp=window.open('");
        sScript.Append(url);
        sScript.Append("', '" + WindowName + "','");
        sScript.Append("height=' + frmHeight + ',");
        sScript.Append("width=" + width.ToString() + ",");
        sScript.Append("toolbar=no,locatiion=yes,resizable=yes,status=yes,menubar=no,scrollbars=yes,");
        sScript.Append("left=' + leftVal + ',");
        sScript.Append("top=' + topVal + '");
        sScript.Append("')");
        sScript.Append(@"</script>");
        pp.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript.ToString());
    }

    public static void SetLogonUser(Page pp, string UserId)
    {
        if (UserId == "")
            return;
        pp.Session["UserId"] = UserId;
        Constant.LogonUserId = UserId;
    }
}
