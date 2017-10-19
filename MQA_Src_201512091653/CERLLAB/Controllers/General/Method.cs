using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Collections.Generic;
using CERLLAB.Models;

namespace CERLLAB.Controllers.General
{
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

        public static bool SendMail(List<string> to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(Constant.DefaultMailFrom);

            foreach(var item in to)
            {
                if (item != null)
                    if (item.Trim().Length > 0)
                        message.To.Add(new MailAddress(item));
            }

            char[] delimiterChars = { ';' };
            string[] bccArray = Constant.DefaultMailBcc.Split(delimiterChars);
            foreach(string bcc in bccArray)
            {
                message.Bcc.Add(new MailAddress(bcc));
            }

            message.IsBodyHtml = true;
            message.Subject = subject;
            message.BodyEncoding = Encoding.UTF8;
            message.Body = body;
            SmtpClient client = new SmtpClient(Constant.DefaultMailServer);
            client.Send(message);

            return true;
        }

        public static bool SendMail(List<string> to, List<string> cc, string subject, string body)
        {
            string SendMail = "SendMail";
            sysErrorMessageDBSet syserrdb = new sysErrorMessageDBSet();
            MailMessage message = new MailMessage();
            message.From = new MailAddress(Constant.DefaultMailFrom);
            try
            {
                foreach (var item in to)
                {
                    if (item != null)
                        if (item.Trim().Length > 0)
                            message.To.Add(new MailAddress(item));
                }

                if (cc != null)
                {
                    foreach (var item in cc)
                    {
                        if (item != null)
                            if (item.Trim().Length > 0)
                                message.CC.Add(new MailAddress(item));
                    }
                }
                char[] delimiterChars = { ';' };
                string[] bccArray = Constant.DefaultMailBcc.Split(delimiterChars);
                foreach (string bcc in bccArray)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }

            }
            catch(Exception ex)
            {
                message.To.Add(new MailAddress(Constant.DefaultMailBcc));
                body += ex.Message;
                syserrdb.InitErrorData(Src: SendMail, content: body, editor: "IEC891652");
            }
            try
            {
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.BodyEncoding = Encoding.UTF8;
                message.Body = body;
                SmtpClient client = new SmtpClient(Constant.DefaultMailServer);
                client.Send(message);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                syserrdb.InitErrorData(Src: SendMail, content: msg, editor: "IEC891652");
            }

            return true;
        }

        public static string GetDisplay(string input)
        {
            string none = "none";
            string block = "";
            string rslt = "none";
            input = input.ToUpper();
            if (input == "TRUE")
                rslt = block;
            else
                rslt = none;

            return rslt;
        }

        public static string FileUpload(FileUpload fu, string id, string fileType, string logonID)
        {
            string backupFile = "";

            if (fu.PostedFile.FileName != "")
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["uploadfileroot"] + id))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings["uploadfileroot"] + id);
                }

                backupFile = fu.PostedFile.FileName.Substring(fu.PostedFile.FileName.LastIndexOf(@"\") + 1);

                if (backupFile != "")
                {
                    fu.PostedFile.SaveAs(ConfigurationManager.AppSettings["uploadfileroot"] + id + @"\" + backupFile);
                }
            }

            return backupFile;
        }

        public static bool InjectionCheck(string str)
        {
            bool check = false;
            char[] injection = { '\'', ';' };

            if (str.IndexOfAny(injection) != -1)
            {
                check = true;
            }

            return check;
        }

        public static string ReplaceInjection(string str)
        {
            return str.Replace('\'', '"');
        }


        public static string GetRole(MasterPage mp)
        {
            return ((DropDownList)mp.FindControl("ddlRole")).SelectedValue;
        }

        public static string ReplaceSpecialChar(string str)
        {
            return str.Replace("'", "’").Replace("<", "＜").Replace(">", "＞");
        }

        public static void CheckAuthority(Page page)
        {
            if (page.Session["RoleCode"].ToString() == null || page.Session["RoleCode"].ToString() == "")
            {
                page.Response.Redirect("MPersonalProfile.aspx");
            }
            else
            {
                string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
                string sRet = oInfo.Name;

                DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_ConnStr,
                    "EXEC sp_RoleMaintain 'CHECK', 'AUTHORITY', '<RoleCode>" + page.Session["RoleCode"] +
                    "</RoleCode><Page>" + sRet.ToUpper() + "</Page>'");

                if (ds.Tables[0].Rows.Count == 0)
                {
                    page.Response.Redirect("MPersonalProfile.aspx");
                }
            }
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
            string[] sRslt = { "true" };
            string vchType = "AUTHORITY_OF_CURRENTWEBPAGE";
            StringBuilder vchParameters = new StringBuilder();
            vchParameters.Append(Method.BuildXML(user_id, "user_id"));
            vchParameters.Append(Method.BuildXML(navigate_url, "navigate_url"));

            string sqlCmd = GetSqlCmd(sp_TreeMenuRole, vchType, vchParameters.ToString());
            sRslt = DAO.sqlCmdArrSingleCol(Constant.S_ConnStr, sqlCmd.ToString());
            bool IsEnable = true;
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
            sRslt = DAO.sqlCmdArrSingleCol(Constant.S_ConnStr, sqlCmd.ToString());
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
            sRslt = DAO.sqlCmdArrSingleCol(Constant.S_ConnStr, sqlCmd.ToString());
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
            sRslt = DAO.sqlCmdArrSingleCol(Constant.S_ConnStr, sqlCmd.ToString());
            eMailAddress = sRslt[0];
            return eMailAddress;
        }

        public static string GetUserBosseMailByUserId(string UserId)
        {
            string[] sRslt = { "eMailAddress" };
            string eMailAddress = "";
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(UserId, "editor"));

            string sqlCmd = GetSqlCmd(sp_Method, "GetUserBossMailByUserId", "m_userprofile", vchSet.ToString());
            sRslt = DAO.sqlCmdArrSingleCol(Constant.S_ConnStr, sqlCmd.ToString());
            eMailAddress = sRslt[0];
            return eMailAddress;
        }

        public static string GetUserBossBadgeCodeByUserId(string UserId)
        {
            string[] sRslt = { "BadgeCode" };
            string BossBadgeCode = "";
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(UserId, "editor"));

            string sqlCmd = GetSqlCmd(sp_Method, "GetUserBossByUserId", "m_userprofile", vchSet.ToString());
            sRslt = DAO.sqlCmdArrSingleCol(Constant.S_ConnStr, sqlCmd.ToString());
            BossBadgeCode = sRslt[0];
            return BossBadgeCode;
        }
        public static void MessageOut(Page pp, string sMsg)
        {
            //string sScript = @"<script language='javascript'>alert('" + sMsg + "');</script>";
            //string sScript = @"<script language='javascript'>showStickyNoticeToast('" + sMsg + "');</script>";
            string sScript = @"<script language='javascript'>apprise('" + sMsg + "');</script>";
            pp.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript);
        }

        public static void MessageOut(Page pp, string sMsg, bool IsCloseSrcWindow)
        {
            string sScript = @"<script language='javascript'>alert('" + sMsg + "');</script>";
            sScript += "<script language='javascript'>window.opener=null;window.close();</script>";
            pp.ClientScript.RegisterStartupScript(pp.GetType(), "", sScript);
        }

        public static void MessageOut(string msg, UpdatePanel up)
        {
            ScriptManager.RegisterClientScriptBlock(up, typeof(UpdatePanel), "", "alert('" + msg + "');", true);
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

        public static void SetLogonUser(HttpSessionStateBase pp, Controller conf, string UserId)
        {
            if (pp == null || conf == null)
            {
                pp["SimUserId"] = null;
                Constant.LogonUserId = GetLogonUserId(pp, conf, conf.User.Identity.Name.ToUpper());
                return;
            }

            if (UserId.Trim().Length == 0 || UserId == null)
            {
                pp["SimUserId"] = null;
                Constant.LogonUserId = GetLogonUserId(pp, conf, conf.User.Identity.Name.ToUpper());
            }
            else
            {
                UserId = UserId.Trim().ToUpper();
                pp["SimUserId"] = UserId;
                Constant.LogonUserId = UserId;
            }
        }

        public static string GetLogonUserId(HttpSessionStateBase http, string UserId)
        {
            try
            {
                if (http != null)
                    UserId = (http["SimUserId"] != null) ? http["SimUserId"].ToString() : UserId;

                int iDomain = UserId.IndexOf(@"\", 0);
                iDomain = UserId.IndexOf(@"\", 0);
                UserId = (iDomain > 0 && UserId.Length > 9) ? UserId.Substring(iDomain + 1) : UserId;
            }
            catch
            {
                // UserId = "IEC970504";
            }
            return UserId;
        }

        public static string GetLogonUserId(HttpSessionStateBase http, Controller conf, string UserId)
        {
            try
            {
                UserId = (UserId == null || UserId.Trim().Length == 0) ? conf.User.Identity.Name.ToUpper() : UserId.ToUpper();
                UserId = (http["SimUserId"] != null) ? http["SimUserId"].ToString() : conf.User.Identity.Name.ToUpper();
                int iDomain = UserId.IndexOf(@"\", 0);
                iDomain = UserId.IndexOf(@"\", 0);
                UserId = (iDomain > 0 && UserId.Length > 9) ? UserId.Substring(iDomain + 1) : UserId;
            }
            catch
            {
                // UserId = "IEC970504";
            }
            return UserId;
        }

        public static void SetLogonUser(Page pp, string UserId)
        {
            if (UserId == "")
                return;
            pp.Session["UserId"] = UserId;
            Constant.LogonUserId = UserId;
        }

        public static void CheckSession(Page pp)
        {
            if (pp.Session["AllowedUser"] == null || pp.Session["AllowedUser"].ToString() != "MCAD")
            {
                pp.Response.Write("<script language='javascript'>window.parent.document.location.href='Menu.aspx';</script>");
            }
        }

        public static void CheckAdmin(Page pp)
        {
            if (pp.Session["isAdmin"] == null || pp.Session["isAdmin"].ToString() != "TRUE")
            {
                pp.Response.Write("<script language='javascript'>window.parent.document.location.href='Menu.aspx';</script>");
            }
        }

        public static void AutoGenClsFunction(Page sender, HtmlGenericControl acc, String filePath, String pageName)
        {
            string[] dir = System.IO.Directory.GetDirectories(filePath);
            foreach (string foldepath in dir)
            {
                DirectoryInfo dirinfo = new DirectoryInfo(foldepath);
                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                HtmlGenericControl div = new HtmlGenericControl("div");
                Panel pn = new Panel();
                div.Controls.Add(pn);
                acc.Controls.Add(h3);
                acc.Controls.Add(div);
                pn.ID = dirinfo.Name;
                h3.InnerText = dirinfo.Name;
                AutoGenLinks(sender, pn, foldepath, dirinfo.Name, pageName);
            }
        }

        private static bool ExistsExtension(string ext)
        {
            bool rslt = false;
            string ExtTypes = Constant.ExtTypes;
            char[] delimiterChars = { ';' };
            string[] words = ExtTypes.Split(delimiterChars);

            foreach (string word in words)
            {
                if (ext == word)
                {
                    rslt = true;
                    return rslt;
                }
            }
            return rslt;
        }

        public static void AutoGenLinks(Page sender, Panel pnl, String filePath, String title, String pageName)
        {
            pnl.Visible = true;
            DirectoryInfo rootFolder = new DirectoryInfo(filePath);

            foreach (string rootfile in Directory.GetFiles(filePath))
            {
                if (ExistsExtension(Path.GetExtension(rootfile).ToUpper()))
                {
                    HyperLink hl = new HyperLink();
                    hl.NavigateUrl = "downloadFile.aspx?type=" + sender.Server.UrlEncode(pageName) + "&Title=" + sender.Server.UrlEncode(title) + "&Class=" + sender.Server.UrlEncode(rootFolder.Name) + "&filePath=" + sender.Server.UrlEncode(rootfile);
                    hl.Target = "_blank";
                    //hl.ID = title.Replace('.', '_') + j.ToString();
                    FileInfo fileInfo = new FileInfo(rootfile);
                    hl.Text = fileInfo.Name; //files[j].Split('\\')[files[j].Split('\\').Length - 1];
                    Label hyphen = new Label();
                    hyphen.Text = "- ";
                    pnl.Controls.Add(hyphen);
                    pnl.Controls.Add(hl);
                    pnl.Controls.Add(new LiteralControl("<br/>"));
                }
            }

            string[] dir = System.IO.Directory.GetDirectories(filePath);
            pnl.Controls.Add(new LiteralControl("<table width=\"100%\" style=\"font-family: Comic Sans MS; font-size:10pt\" cellpadding=\"10\" border=\"1\">"));
            for (int i = 0; i < dir.Length; i++)
            {
                if (i % 2 == 0)
                {
                    pnl.Controls.Add(new LiteralControl("<tr>"));
                }
                //pnl.Controls.Add(new LiteralControl("<td width=\"5%\">"));
                Label lbl = new Label();
                lbl.Text = title; // +(i + 1).ToString();
                //pnl.Controls.Add(lbl);
                //pnl.Controls.Add(new LiteralControl("</td>"));
                pnl.Controls.Add(new LiteralControl("<td width=\"45%\" align=\"left\" valign=\"top\">"));
                Label lblDir = new Label();
                DirectoryInfo dirInfo = new DirectoryInfo(dir[i]);
                lblDir.Text = dirInfo.Name; //dir[i].Split('\\')[dir[i].Split('\\').Length - 1];
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Style[HtmlTextWriterStyle.BackgroundColor] = "lightblue";
                div.Controls.Add(lblDir);
                pnl.Controls.Add(div);
                pnl.Controls.Add(new LiteralControl("<br/>"));
                string[] files = System.IO.Directory.GetFiles(dir[i]);
                if (files.Length != 0)
                {
                    for (int j = 0; j < files.Length; j++)
                    {
                        if (ExistsExtension(Path.GetExtension(files[j]).ToUpper()))
                        {
                            HyperLink hl = new HyperLink();
                            hl.NavigateUrl = "downloadFile.aspx?type=" + sender.Server.UrlEncode(pageName) + "&Title=" + sender.Server.UrlEncode(title) + "&Class=" + sender.Server.UrlEncode(lblDir.Text) + "&filePath=" + sender.Server.UrlEncode(files[j]);
                            hl.Target = "_blank";
                            hl.ID = title.Replace('.', '_') + j.ToString();
                            FileInfo fileInfo = new FileInfo(files[j]);
                            hl.Text = fileInfo.Name; //files[j].Split('\\')[files[j].Split('\\').Length - 1];
                            Label hyphen = new Label();
                            hyphen.Text = "- ";
                            pnl.Controls.Add(hyphen);
                            pnl.Controls.Add(hl);
                            pnl.Controls.Add(new LiteralControl("<br/>"));
                        }
                    }
                }

                pnl.Controls.Add(new LiteralControl("</td>"));
                if (i % 2 != 0)
                {
                    pnl.Controls.Add(new LiteralControl("</tr>"));
                }
            }
            pnl.Controls.Add(new LiteralControl("</table>"));
        }
    }
}