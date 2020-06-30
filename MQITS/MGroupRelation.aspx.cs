using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;

public partial class MGroupRelation : System.Web.UI.Page
{
    const string sp_GroupMaintain = "sp_GroupMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            hfUserId.Value = Method.GetCurrentUserId(Page);
    }
    protected void btnGenRelation_Click(object sender, EventArgs e)
    {
        string vchCmd = "ADD";
        string vchObjectName = "m_GroupRelation";
        StringBuilder vchSet = new StringBuilder();
        if (ddlModule.SelectedIndex != -1)
            vchSet.Append(Method.BuildXML(ddlModule.SelectedValue, "ModuleID"));
        if (ddlCustomer.SelectedIndex != -1)
            vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "CustomerID"));
        if (ddlSite.SelectedIndex != -1)
            vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "SiteID"));
        if (ddlProjectPhase.SelectedIndex != -1)
            vchSet.Append(Method.BuildXML(ddlProjectPhase.SelectedValue, "PhaseID"));
        if (ddlProject.SelectedIndex !=-1 )
            vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "ProjectID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
    }

    protected void gvGroupRelation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string keyValue = e.Keys[0].ToString();
        string vchCmd = "Update";
        string vchObjectName = "m_GroupRelation";
        string Module = "";
        string Customer = "";
        string Site = "";
        string Project = "";
        string Phase = "";
        
        StringBuilder vchSet = new StringBuilder();
        if (e.NewValues[0] == null)
            Module = "";
        else
            Module = e.NewValues[0].ToString();

        if (e.NewValues[1] == null)
            Customer = "";
        else
            Customer = e.NewValues[1].ToString();

        if (e.NewValues[2] == null)
            Project = "";
        else
            Project = e.NewValues[3].ToString();

        if (e.NewValues[3] == null)
            Site = "";
        else
            Site = e.NewValues[2].ToString();

        if (e.NewValues[4] == null)
            Phase = "";
        else
            Phase = e.NewValues[4].ToString();

        vchSet.Append(Method.BuildXML(Module, "ModuleID"));
        vchSet.Append(Method.BuildXML(Customer, "CustomerID"));
        vchSet.Append(Method.BuildXML(Site, "SiteID"));
        vchSet.Append(Method.BuildXML(Project, "ProjectID"));
        vchSet.Append(Method.BuildXML(Phase, "PhaseID"));

        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        ((GridView)sender).EditIndex = -1;
        e.Cancel = true;
    }
}
