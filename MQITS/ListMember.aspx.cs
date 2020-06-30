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
using System.Text;

public partial class ListMember : System.Web.UI.Page
{
    string UserID, ChtName;
     
    protected void Page_Load(object sender, EventArgs e)
    {
        txtSearch.Text = Session["QueryUser"].ToString();//gv的查詢Control
        UserID = Request.QueryString["BadgeCode"];
        ChtName = Request.QueryString["ChtName"];

        if (!IsPostBack)
        {
            gvListMember.DataBind();            
        }
    }
    protected void gvListMember_SelectedIndexChanged(object sender, EventArgs e)
    {
        string linkUrl = "<script language='javascript'>opener.document.form1." + UserID + ".value = '" +
                ((Label)((GridView)sender).SelectedRow.Cells[1].FindControl("lblBadgeCode")).Text +
                "';opener.document.form1." + ChtName + ".value='" +
                 gvListMember.SelectedDataKey.Value.ToString() + "';window.close();</script>";
        Response.Write(linkUrl);

    }
}
