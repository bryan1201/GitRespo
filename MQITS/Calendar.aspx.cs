using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Calendar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    protected void cal_SelectionChanged(object sender, EventArgs e)
    {
        txtCal.Value = cal.SelectedDate.ToString("yyyy/MM/dd");
        string linkUrl = "<script language='javascript'>opener.document.forms[0]." + Request.QueryString["ID"] + ".value = '" + txtCal.Value + "';window.close();</script>";
        Response.Write(linkUrl);
        /*DateTime NowDate = DateTime.Parse(System.DateTime.Now.ToString("yyyy/MM/dd"));
        DateTime yesDate = NowDate.AddDays(-1);
        DateTime CalDate = DateTime.Parse(txtCal.Value);

        //int check = DateTime.Compare(CalDate, NowDate);
        if (CalDate <= NowDate && CalDate == yesDate)
        {
           
        }
        else
        {
            DisplayMessage("選擇時間為昨天!");
            return;
        }*/
        
        
        
        /*if (CalDate>=NowDate || CalDate>=)
        {
            DisplayMessage("選擇時間為昨天!");
            return;
        }
        else
        {

            //string sScript = @"<script language='javascript'>FeedbackCal();</script>";
            string linkUrl = "<script language='javascript'>opener.document.aspnetForm." + Request.QueryString["ID"] + ".value = '" + txtCal.Value + "';window.close();</script>";
            Response.Write(linkUrl);
            //ClientScript.RegisterStartupScript(Page.GetType(), "", sScript);
        }*/
    }
    private void DisplayMessage(string strValue)
    {
        string sScript = @"<script language='javascript'>alert('" + strValue + "');</script>";
        ClientScript.RegisterStartupScript(Page.GetType(), "", sScript);
    }
}
