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

public partial class MProject : System.Web.UI.Page
{
    const string sp_GroupMaintain = "sp_GroupMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            hfUserId.Value = Method.GetCurrentUserId(Page);
    }
    protected void gvProject_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string keyValue = e.Keys[0].ToString();
        string vchCmd = "UpdateProjectCustomer";
        string vchObjectName = "m_Group";

        string ModuleID = ddlModule.SelectedValue;
        string CustomerID = "";
        string OldCustomerID = "";
        string SiteID = "401";

        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ModuleID, "ModuleID"));
        vchSet.Append(Method.BuildXML(keyValue, "GroupID"));
        vchSet.Append(Method.BuildXML(keyValue, "ProjectID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "IsEOSL"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "Rank"));

        if (e.NewValues[2] == null)
        {
            CustomerID = "";
            Method.MessageOut(Page, "Please assign Project to a Customer!");
        }
        else
        {
            CustomerID = e.NewValues[2].ToString();
            OldCustomerID = e.OldValues[2].ToString();
            if (CustomerID == "0")
                CustomerID = "";
        }

        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "IsMP"));
        /*
        if (e.NewValues[3] == null)
        {
            SiteID = "";
            Method.MessageOut(Page, "Please assign Project to a Site!");
        }
        else
        {
            SiteID = e.NewValues[3].ToString();
            if (SiteID == "0")
                SiteID = "";
        }
         */

        SiteID = "401";

        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(OldCustomerID, "OldCustomerID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        ((GridView)sender).EditIndex = -1;
        e.Cancel = true;
    }
    protected void btnFindOwner_Click(object sender, EventArgs e)
    {

    }
    protected void gvRoleUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        string OwnerType = ((GridView)sender).SelectedDataKey[0].ToString();
        string OwnerCode = ((GridView)sender).SelectedDataKey[1].ToString();
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(OwnerType, "OwnerType"));
        vchSet.Append(Method.BuildXML(OwnerCode, "OwnerCode"));
        string ModuleID = ddlModule.SelectedValue;
        string ProjectID = this.gvProject.SelectedDataKey[0].ToString();
        string CustomerID = gvProject.SelectedDataKey[1].ToString();
        string SiteID = ddlSite.SelectedItem.Value;//gvProject.SelectedDataKey[2].ToString();

        if (CustomerID == "")
        {
            Method.MessageOut(Page, "Please set Customer first for the Project!");
            return;
        }

        vchSet.Append(Method.BuildXML(ModuleID, "ModuleID"));
        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML("1", "IssueType"));

        string vchCmd = "AddOwner";
        string vchObjectName = "m_GroupRelation;a_Owner";
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

        // Bind Owner
        vchCmd = "SELECTBY_MODULE_PROJECT_SITE";
        vchObjectName = "V_GROUPRELATIONOWNER";
        sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        SqlDSOwner.SelectCommand = sqlCmd;
        SqlDSOwner.DataBind();
    }

    protected void gvProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        string ModuleID = ddlModule.SelectedValue;
        string ProjectID = gv.SelectedDataKey[0].ToString();
        string CustomerID = gv.SelectedDataKey[1].ToString();
        string SiteID = gv.SelectedDataKey[2].ToString();
        string vchCmd = "GenGroupRelation";
        string vchObjectName = "m_GroupRelation";
        StringBuilder vchSet = new StringBuilder();

        if (CustomerID == "")
        {
            Method.MessageOut(Page, "Please set Customer first for the Project!");
            return;
        }

        if (SiteID != ddlSite.SelectedItem.Value)
            SiteID = ddlSite.SelectedItem.Value;
        
        vchSet.Append(Method.BuildXML(ModuleID, "ModuleID"));
        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));

        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        vchCmd = "SELECTBY_MODULE_PROJECT_SITE";
        vchObjectName = "V_GROUPRELATIONOWNER";
        sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        Session["MProject_gvProject_sqlCmd"] = sqlCmd;
        BindOwnerData();
        //gvGroupRelationOwner.DataBind();
    }

    protected void BindOwnerData()
    {
        if (Session["MProject_gvProject_sqlCmd"] == null)
            return;
        string sqlCmd = Session["MProject_gvProject_sqlCmd"].ToString();
        SqlDSOwner.SelectCommand = sqlCmd;
        SqlDSOwner.DataBind();
    }

    protected void gvGroupRelationOwner_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string OwnerID = e.Keys[0].ToString();
        string GroupRelationID = e.Keys[1].ToString();
        string vchCmd = "Delete";
        string vchObjectName = "m_GroupRelation;a_Owner";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(OwnerID, "OwnerID"));
        vchSet.Append(Method.BuildXML(GroupRelationID, "GroupRelationID"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

        BindOwnerData();
        e.Cancel = true;
    }
    protected void gvGroupRelationOwner_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindOwnerData();
    }
    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        fvProjectMaterial.Visible = true;
    }
    protected void fvProjectMaterial_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        GridView gv = gvProject;
        bool IsInUse = true;
        string vchCmd = "Add";
        string vchObjectName = "m_ProjectMaterial";
        StringBuilder vchSet = new StringBuilder();
        string ProjectID = gv.SelectedDataKey[0].ToString();
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        if (e.Values[0] == null)
        {
            Method.MessageOut(Page, "Please input Project Material!");
            return;
        }
        else
            vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "Material"));

        if (e.Values[1] == null)
        {
            Method.MessageOut(Page, "Please input Material Rank!");
            return;
        }
        else
            vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));

        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "MType"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProjectMaterial.DataBind();
        e.Cancel = true;
    }
    protected void gvProjectMaterial_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ProjectID = gvProject.SelectedDataKey[0].ToString();
        string ID = e.Keys[0].ToString();
        string vchCmd = "Update";
        string vchObjectName = "m_ProjectMaterial";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(ID, "ID"));
        if(e.NewValues[0] == null)
        {
            Method.MessageOut(Page, "Please input Project Material!");
            return;
        }
        else
            vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "Material"));

        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "MType"));

        if (e.NewValues[2] == null)
        {
            Method.MessageOut(Page, "Please input Material Rank!");
            return;
        }
        else
            vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));

        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProjectMaterial.EditIndex = -1;
        gvProjectMaterial.DataBind();
        e.Cancel = true;
    }
    protected void fvProjectMaterial_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
            ((FormView)sender).Visible = false;
    }
    protected void btnAddProject_Click(object sender, EventArgs e)
    {
        bool IsEOSL = false;
        bool IsMP = false;
        string ProjectName = txtProject.Text.Trim().Replace("　", "").Replace("'","").Replace("%","");
        string vchCmd = "AddProject";
        string vchObjectName = "m_Group.Project";
        string SiteID = "401";
        string ModuleID = ddlModule.SelectedValue;
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ModuleID, "ModuleID"));
        vchSet.Append(Method.BuildXML(ProjectName, "ProjectName"));
        vchSet.Append(Method.BuildXML(IsEOSL.ToString(), "IsEOSL"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(IsMP.ToString(), "IsMP"));
        vchSet.Append(Method.BuildXML("999999", "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));

        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProject.EditIndex = -1;
        gvProject.SelectedIndex = -1;
        gvProject.DataBind();
    }
}
