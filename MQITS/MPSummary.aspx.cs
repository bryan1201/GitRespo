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

public partial class MPSummary : System.Web.UI.Page
{
    const string sp_MPSummary = "sp_MPSummary";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitCondition();
        }
    }
    protected void InitCondition()
    {
        //BindData("Init");
        txtStart.Text = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
        txtEnd.Text = DateTime.Today.ToString("yyyy/MM/dd");
    }
    protected void btnQry_Click(object sender, EventArgs e)
    {
        BindData("Query");
    }

    protected void BindData(string action)
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
      
        vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(txtStart.Text.Trim(), "StartTime"));
        vchSet.Append(Method.BuildXML(txtEnd.Text.Trim(), "EndTime"));
        string Project = "";
        if (lbtoright.Items.Count > 0)
        {
            for (int i = 0; i <= lbtoright.Items.Count - 1; i++)
            {
                Project += "," + lbtoright.Items[i].Value;
            }
            vchSet.Append(Method.BuildXML(Project.Substring(1), "Project"));
        }

        sqlCmd = Method.GetSqlCmd(sp_MPSummary, "QUERY", "MPPCASUMMARY", vchSet.ToString());
        DataSet dsPCA = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        gvPCA.DataSource = dsPCA.Tables[0];
        gvPCA.DataBind();
        
        sqlCmd = Method.GetSqlCmd(sp_MPSummary, "QUERY", "MPCPUSUMMARY", vchSet.ToString());
        DataSet dsCPU = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        gvCPU.DataSource = dsCPU.Tables[0];
        gvCPU.DataBind();
        /*SqlDSCPU.SelectCommand = sqlCmd;
        SqlDSCPU.DataBind();
        rptCPUSummary.LocalReport.Refresh();*/
    }
    protected void btnright_Click(object sender, EventArgs e)
    {
        right(lbtoright, lbtoleft);
    }
    protected void btnleft_Click(object sender, EventArgs e)
    {
        left(lbtoright, lbtoleft);
    }

    private void right(ListBox lbright, ListBox lbleft)
    {
        int i = 0;
        while (i <= lbleft.Items.Count - 1)
        {
            if (lbleft.Items[i].Selected == true)
            {
                lbright.Items.Add(new ListItem(lbleft.Items[i].Text, lbleft.Items[i].Value));
                lbleft.Items.Remove(lbleft.Items[i]);
            }
            else
            {
                i += 1;
            }
        }

    }
    private void left(ListBox lbright, ListBox lbleft)
    {
        int i = 0;
        while (i <= lbright.Items.Count - 1)
        {
            if (lbright.Items[i].Selected == true)
            {
                int count = lbleft.Items.Count;
                lbleft.Items.Add(new ListItem(lbright.Items[i].Text, lbright.Items[i].Value));
                lbright.Items.Remove(lbright.Items[i]);
            }
            else
            {
                i += 1;
            }
        }
    }

}
