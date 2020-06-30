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

public partial class Location : System.Web.UI.Page
{
    const string sp_RMAIssue = "sp_RMAIssue";
    protected void Page_Load(object sender, EventArgs e)
    {
        txtIssueID.Text = Request.QueryString["IssueID"];
        txtFailureDataID.Text = Request.QueryString["FailureDataID"];
        if (!IsPostBack)
        {
            txtUserID.Text = Method.GetCurrentUserId(Page);
        }
    }
    protected void gvLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(e.Keys[0].ToString(), "LocationID"));
        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "DELETE", "LOCATIONQTY", vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvLocation.DataBind();
        e.Cancel = true;
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        this.Response.Write("<script>window.opener.location.href=window.opener.location.href;window.close();</script>");
        this.Response.Write("<script>window.opener.location.replace(window.opener.document.referrer);window.close();</script>");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtLocation.Text.Trim() != "" && txtQty.Text.Trim() != "")
        {
            try
            {
                int FailQty;
                FailQty = int.Parse(txtQty.Text.Trim());
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(txtIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(txtFailureDataID.Text, "FailureDataID"));
                vchSet.Append(Method.BuildXML(txtLocation.Text.ToUpper().Trim(), "Location"));
                vchSet.Append(Method.BuildXML(FailQty.ToString(), "LocationQty"));
                vchSet.Append(Method.BuildXML(txtUserID.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "ADD", "LOCATIONQTY", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvLocation.DataBind();
                txtLocation.Text = "";
                txtQty.Text = "";

                /*this.Response.Write("<script>window.opener.location.href=window.opener.location.href;</script>");
                this.Response.Write("<script>window.opener.location.href='RMAIssue.aspx?IssueID="+txtIssueID.Text+"';</script>");*/
            }
            catch
            {
                Method.MessageOut(Page, "Qty must digital!!");
                txtQty.Text = "";
            }

        }
    }
    protected void gvLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLocation.Text = ((Label)gvLocation.SelectedRow.FindControl("lblLocation")).Text.ToString();
        txtQty.Text = ((Label)gvLocation.SelectedRow.FindControl("lblQty")).Text.ToString();
    }
    protected void gvLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtLocation")).Text.Trim() != ""
                && ((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtQty")).Text.Trim()!="")
        {
            try
            {
                int FailQty;
                FailQty = int.Parse(((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtQty")).Text.ToString());
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblLocationID")).Text.ToString(), "LocationID"));
                vchSet.Append(Method.BuildXML(((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtLocation")).Text.ToString(), "Location"));
                vchSet.Append(Method.BuildXML(FailQty.ToString(), "LocationQty"));
                vchSet.Append(Method.BuildXML(txtUserID.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "UPDATE", "LOCATIONQTY", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvLocation.DataBind();
                gvLocation.EditIndex = -1;
            }
            catch
            {
                Method.MessageOut(Page, "Qty must digital!!");
            }
            
        }
        e.Cancel = true;
    }
}
