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
using System.Text;


public partial class Summary : System.Web.UI.Page
{
    const string sp_NPISummary = "sp_NPISummary";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitCondition();
        }
    }
    protected void InitCondition()
    {
        BindData("Init"); 
    }
    protected void btnQry_Click(object sender, EventArgs e)
    {
       BindData("Query");
    }
    protected void BindData(string action)
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        if (action == "Init")
        {
            ddlCustomer.DataBind();
            ddlSite.DataBind();
            if(ddlCustomer.Items.Count>0)
                ddlCustomer.SelectedIndex = 0;
            if (ddlSite.Items.Count > 0)
                ddlSite.SelectedIndex = 0;
            
            rptPCASummary.Visible = false;
            rptCPUSummary.Visible = false;
        }
        else
        {

            rptPCASummary.Visible = true;
            vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
            vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
            sqlCmd = Method.GetSqlCmd(sp_NPISummary, "QUERY", "NPIPCASUMMARY", vchSet.ToString());
            SqlDSReport.SelectCommand = sqlCmd;
            SqlDSReport.DataBind();
            rptPCASummary.LocalReport.Refresh();

            rptCPUSummary.Visible = true;
            sqlCmd = Method.GetSqlCmd(sp_NPISummary, "QUERY", "NPICPUSUMMARY", vchSet.ToString());
            SqlDSCPU.SelectCommand = sqlCmd;
            SqlDSCPU.DataBind();
            rptCPUSummary.LocalReport.Refresh();
        }
        
        /*
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
    
        gvPCA.Visible = true;
        gvCPU.Visible = true;
        gvPCA.DataSource = ds.Tables[0];
        gvPCA.DataBind();
        gvCPU.DataSource = ds.Tables[1];
        gvCPU.DataBind(); */
    }


    protected void btnAddIssue_Click(object sender, EventArgs e)
    {
        Response.Redirect("NPIIssue.aspx");
    }
}
