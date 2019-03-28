using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Text;
 
public partial class MyIssueAction : System.Web.UI.Page
{
    const string sp_MyIssueAction = "sp_MyIssueAction";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserId.Text = Method.GetCurrentUserId(Page);
            lblQstring.Text = "";
            this.InitCondition(lblQstring.Text);
        }    
    }
    protected void InitCondition(string Qstring)
    {
        BindData(0, SqlDSReportMe, gvReportMe, Qstring);
        BindData(0, SqlDSMonitor, gvMonitor, Qstring);
        BindData(0, SqlDSOwnerMe, gvOwenMe, Qstring);
        BindData(0, SqlDSRM, gvRM, Qstring);
        BindData(0, SqlDSMyAction, gvMyAction, Qstring);
    }

    private void BindData(int PageExpression,SqlDataSource ds,GridView gv,string action)
    {
        try
        {

            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
            if (action != "")
                vchSet.Append(action.ToString());

            string sqlCmd = "";
            if (gv == gvReportMe)
            {
                sqlCmd = Method.GetSqlCmd(sp_MyIssueAction, "SELECT", "REPORTME", vchSet.ToString());
                ds.SelectCommand = sqlCmd;
                gv.PageIndex = PageExpression;
                gv.DataBind();
            }
            else if (gv == gvMonitor)
            {
                sqlCmd = Method.GetSqlCmd(sp_MyIssueAction, "SELECT", "MONITOR", vchSet.ToString());
                ds.SelectCommand = sqlCmd;
                gv.PageIndex = PageExpression;
                gv.DataBind();
            }
            else if (gv == gvOwenMe)
            {
                sqlCmd = Method.GetSqlCmd(sp_MyIssueAction, "SELECT", "OWNERME", vchSet.ToString());
                ds.SelectCommand = sqlCmd;
                gv.PageIndex = PageExpression;
                gv.DataBind();
            }
            else if (gv == gvRM)
            {
                sqlCmd = Method.GetSqlCmd(sp_MyIssueAction, "SELECT", "RECENTLY", vchSet.ToString());
                ds.SelectCommand = sqlCmd;
                gv.PageIndex = PageExpression;
                gv.DataBind();
            }
            else
            {
                sqlCmd = Method.GetSqlCmd(sp_MyIssueAction, "SELECT", "MYACTION", vchSet.ToString());
                ds.SelectCommand = sqlCmd;
                gv.PageIndex = PageExpression;
                gv.DataBind();
            }
        }
        catch
        {
            Response.Write("<script languge='javascript'>alert('No Data List!!'); window.location.href='MyIssueAction.aspx'</script>"); 
        }
    }
    protected void gvReportMe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReportMe.EditIndex = -1;
        BindData(e.NewPageIndex,SqlDSReportMe,gvReportMe,lblQstring.Text);
        InitCondition(lblQstring.Text);
    }
    protected void gvMonitor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMonitor.EditIndex = -1;
        BindData(e.NewPageIndex, SqlDSMonitor, gvMonitor, lblQstring.Text);
        InitCondition(lblQstring.Text);
    }
    protected void gvOwenMe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOwenMe.EditIndex = -1;
        BindData(e.NewPageIndex, SqlDSOwnerMe, gvOwenMe, lblQstring.Text);
        InitCondition( lblQstring.Text);
    }
    protected void gvRM_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRM.EditIndex = -1;
        BindData(e.NewPageIndex, SqlDSRM, gvRM, lblQstring.Text);
        InitCondition( lblQstring.Text);
    }
    protected void gvMyAction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMyAction.EditIndex = -1;
        BindData(e.NewPageIndex, SqlDSMyAction, gvMyAction, lblQstring.Text);
        InitCondition(lblQstring.Text);
    }

    protected void btnQry_Click(object sender, EventArgs e)
    {
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        vchSet.Append(Method.BuildXML(ddlModule.SelectedValue, "ModuleID"));
        vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "CustomerID"));
        vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "SiteID"));
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "ProjectID"));
        vchSet.Append(Method.BuildXML(ddlPhase.SelectedValue, "PhaseID"));

        lblQstring.Text = vchSet.ToString();

        BindData(0, SqlDSReportMe, gvReportMe, lblQstring.Text);
        BindData(0, SqlDSMonitor, gvMonitor, lblQstring.Text);
        BindData(0, SqlDSOwnerMe, gvOwenMe, lblQstring.Text);
        BindData(0, SqlDSRM, gvRM, lblQstring.Text);
        BindData(0, SqlDSMyAction, gvMyAction, lblQstring.Text);
    }

    protected void gvReportMe_RowDataBound(object sender, GridViewRowEventArgs e)
    { 
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string unitsInStock = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status"));
            switch (unitsInStock)
            {
                case "1":
                    e.Row.BackColor = Color.FromName("#ffccff");
                    break;
                case "2":
                    e.Row.BackColor = Color.FromName("#ccff99");
                    break;
                case "3":
                    e.Row.BackColor = Color.FromName("#cccccc");
                    break;
                case "4":
                    e.Row.BackColor = Color.FromName("#ffcc66");
                    break;
                default:
                    break;
            }
        }
    }
    protected void gvMonitor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string unitsInStock = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status"));
            switch (unitsInStock)
            {
                case "1":
                    e.Row.BackColor = Color.FromName("#ffccff");
                    break;
                case "2":
                    e.Row.BackColor = Color.FromName("#ccff99");
                    break;
                case "3":
                    e.Row.BackColor = Color.FromName("#cccccc");
                    break;
                case "4":
                    e.Row.BackColor = Color.FromName("#ffcc66");
                    break;
                default:
                    break;
            }
        }
    }

    protected void gvOwenMe_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string unitsInStock = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status"));
            switch (unitsInStock)
            {
                case "1":
                    e.Row.BackColor = Color.FromName("#ffccff");
                    break;
                case "2":
                    e.Row.BackColor = Color.FromName("#ccff99");
                    break;
                case "3":
                    e.Row.BackColor = Color.FromName("#cccccc");
                    break;
                case "4":
                    e.Row.BackColor = Color.FromName("#ffcc66");
                    break;
                default:
                    break;
            }
        }
    }
    protected void gvRM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string unitsInStock = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status"));
            switch (unitsInStock)
            {
                case "1":
                    e.Row.BackColor = Color.FromName("#ffccff");
                    break;
                case "2":
                    e.Row.BackColor = Color.FromName("#ccff99");
                    break;
                case "3":
                    e.Row.BackColor = Color.FromName("#cccccc");
                    break;
                case "4":
                    e.Row.BackColor = Color.FromName("#ffcc66");
                    break;
                default:
                    break;
            }
        }
    }
    protected void gvMyAction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string unitsInStock = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status"));
            switch (unitsInStock)
            {
                case "1":
                    e.Row.BackColor = Color.FromName("#ffccff");
                    break;
                case "2":
                    e.Row.BackColor = Color.FromName("#ccff99");
                    break;
                case "3":
                    e.Row.BackColor = Color.FromName("#cccccc");
                    break;
                case "4":
                    e.Row.BackColor = Color.FromName("#ffcc66");
                    break;
                default:
                    break;
            }
        }
    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.InitCondition(lblQstring.Text);
    }
    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.InitCondition(lblQstring.Text);
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.InitCondition(lblQstring.Text);
    }
}
