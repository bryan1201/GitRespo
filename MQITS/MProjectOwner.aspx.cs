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

public partial class MProjectOwner : System.Web.UI.Page
{
    const string sp_GroupMaintain = "sp_GroupMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            hfUserId.Value = Method.GetCurrentUserId(Page);
    }
    protected void btnFindOwner_Click(object sender, EventArgs e)
    {

    }
    protected void gvProjectPhase_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        string ID = gv.SelectedDataKey[0].ToString();
        hfCustomerID.Value = ddlCustomer.SelectedValue;
        hfProjectID.Value = gv.SelectedDataKey[1].ToString();
        hfPhaseID.Value = gv.SelectedDataKey[2].ToString();
        gvProjectPhase.DataBind();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        hfModuleID.Value = ddlModule.SelectedValue;
        hfCustomerID.Value = ddlCustomer.SelectedValue;
        hfProjectID.Value = ddlProject.SelectedValue;
        hfSiteID.Value = ddlSite.SelectedValue;
        string ModuleName = ddlModule.SelectedItem.Text;
        string CustomerName = ddlCustomer.SelectedItem.Text;
        string ProjectName = ddlProject.SelectedItem.Text;
        gvProjectPhase.Caption = CustomerName + " > " + ModuleName + " > " + ProjectName;
        CheckandAddProjectMaterialYield(hfProjectID.Value);
        gvProjectPhase.EditIndex = -1;
    }
    protected void gvProjectPhase_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ID = e.Keys[0].ToString();
        string vchCmd = "Update";
        string vchObjectName = "m_ProjectPhase";
        string PhaseName = "";
        string EDate = "";
        string IsInUse = "";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ID, "ID"));
        vchSet.Append(Method.BuildXML(hfCustomerID.Value, "CustomerID"));
        vchSet.Append(Method.BuildXML(ddlModule.SelectedValue, "ModuleID"));
        vchSet.Append(Method.BuildXML(hfProjectID.Value, "ProjectID"));
        vchSet.Append(Method.BuildXML(hfPhaseID.Value, "PhaseID"));
        if (e.NewValues[0] == null)
            Method.MessageOut(Page, "Please input PhaseName!");
        else
        {
            PhaseName = e.NewValues[0].ToString();
            PhaseName = PhaseName.Replace(" ", "").Trim();
            vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "PhaseName"));
        }

        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "SiteID"));
        if (e.NewValues[2] == null)
            Method.MessageOut(Page, "Please input Rank!");
        else
            vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));

        if (e.NewValues[3] != null)
            vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "SDate"));

        IsInUse = e.NewValues[4].ToString();
        vchSet.Append(Method.BuildXML(IsInUse, "IsInUse"));

        if (e.NewValues[5] != null)
            vchSet.Append(Method.BuildXML(e.NewValues[5].ToString(), "InputQtyEDate"));

        if (e.NewValues[6] != null)
        {
            EDate = e.NewValues[6].ToString();
            vchSet.Append(Method.BuildXML(EDate, "EDate"));
        }

        if (e.NewValues[7] != null)
            vchSet.Append(Method.BuildXML(e.NewValues[7].ToString(), "PhaseInputQty"));

        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        ((GridView)sender).EditIndex = -1;
        SqlDSProjectPhase.DataBind();
        e.Cancel = true;
    }
    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        fvProjectPhase.Visible = true;
    }

    protected void fvProjectPhase_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        bool IsInUse = true;
        string vchCmd = "Add";
        string vchObjectName = "m_ProjectPhase";
        string PhaseName = "";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(hfCustomerID.Value, "CustomerID"));
        vchSet.Append(Method.BuildXML(ddlModule.SelectedValue, "ModuleID"));
        vchSet.Append(Method.BuildXML(hfProjectID.Value, "ProjectID"));
        if (e.Values[0] == null)
            Method.MessageOut(Page, "Please input PhaseName!");
        else
        {
            PhaseName = e.Values[0].ToString();
            PhaseName = PhaseName.Replace(" ", "").Trim();
            vchSet.Append(Method.BuildXML(PhaseName, "PhaseName"));
        }

        if (e.Values[1] == null)
            Method.MessageOut(Page, "Please input Rank!");
        else
            vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));

        if (e.Values[1] == null)
            Method.MessageOut(Page, "Please select Site!");
        else
            vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "SiteID"));

        vchSet.Append(Method.BuildXML(IsInUse.ToString(),"IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProjectPhase.DataBind();
        e.Cancel = true;
    }
    protected void gvRoleUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ChkBySite = "true";
        string AddToSite = ddlSite.SelectedValue;//ddlOwnerSite.SelectedValue;
        string OwnerType = ((GridView)sender).SelectedDataKey[0].ToString();
        string OwnerCode = ((GridView)sender).SelectedDataKey[1].ToString();
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(OwnerType, "OwnerType"));
        vchSet.Append(Method.BuildXML(OwnerCode, "OwnerCode"));
        hfModuleID.Value = ddlModule.SelectedItem.Value;
        hfProjectID.Value = ddlProject.SelectedItem.Value;
        hfCustomerID.Value = ddlCustomer.SelectedItem.Value;
        string ModuleID = hfModuleID.Value;
        string ProjectID = hfProjectID.Value;
        string CustomerID = hfCustomerID.Value;
        string SiteID = ddlSite.SelectedItem.Value;

        vchSet.Append(Method.BuildXML(ModuleID, "ModuleID"));
        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(ddlIssueType.SelectedValue, "IssueType"));
        vchSet.Append(Method.BuildXML(ChkBySite, "ChkBySite"));
        vchSet.Append(Method.BuildXML(AddToSite, "AddToSite"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));

        string vchCmd = "AddOwnerOne";
        string vchObjectName = "m_GroupRelation;a_Owner";
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

        // Bind Owner
        //vchCmd = "SELECTBY_MODULE_PROJECT_SITE";
        //vchObjectName = "V_GROUPRELATIONOWNER";
        //sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        //DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        //SqlDSOwner.SelectCommand = sqlCmd;
        gvGroupRelationOwner.DataBind();
        gvGroupRelationMember.DataBind();
    }
    protected void fvProjectMaterial_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        bool IsInUse = true;
        string vchCmd = "Add";
        string vchObjectName = "m_ProjectMaterial";
        string MType = e.Values[1].ToString();
        string Material = "";
        StringBuilder vchSet = new StringBuilder();
        string ProjectID = hfProjectID.Value;
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        if (e.Values[0] == null)
        {
            Method.MessageOut(Page, "Please input Project Material!");
            return;
        }
        else
        {
            Material = e.Values[0].ToString();
            vchSet.Append(Method.BuildXML(Material, "Material"));
        }

        vchSet.Append(Method.BuildXML(MType, "MType"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProjectMaterial.DataBind();
        e.Cancel = true;
    }
    protected void gvProjectMaterial_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ProjectID = hfProjectID.Value;
        string ID = e.Keys[0].ToString();
        string vchCmd = "Update";
        string vchObjectName = "m_ProjectMaterial";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(ID, "ID"));
        if (e.NewValues[0] == null)
        {
            Method.MessageOut(Page, "Please input Project Material!");
            return;
        }
        else
            vchSet.Append(Method.BuildXML(e.OldValues[0].ToString(), "Material"));

        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "MType"));

        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProjectMaterial.EditIndex = -1;
        gvProjectMaterial.DataBind();
        e.Cancel = true;
    }

    protected void gvProjectMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string ProjectID = hfProjectID.Value;
        string ID = e.Keys[0].ToString();
        string vchCmd = "Delete";
        string vchObjectName = "m_ProjectMaterial";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(ID, "ID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvProjectMaterial.EditIndex = -1;
        gvProjectMaterial.DataBind();
        e.Cancel = true;
    }

    protected void gvProjectMaterialYield_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ID = e.Keys[0].ToString();
        string vchCmd = "Update by Project-Material-Phase";
        string vchObjectName = "m_ProjectMaterialYield";
        float ICTYield = 0f;
        float SAYield = 0f;
        float PreTestYield = 0f;
        float RunInYield = 0f;
        float FPY = 0f;
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ID, "ID"));

        if (e.NewValues[0] != null)
        {
            float.TryParse(e.NewValues[0].ToString(), out ICTYield);
            vchSet.Append(Method.BuildXML(ICTYield.ToString(), "ICTYield"));
        }

        if (e.NewValues[1] != null)
        {
            float.TryParse(e.NewValues[1].ToString(), out SAYield);
            vchSet.Append(Method.BuildXML(SAYield.ToString(), "SAYield"));
        }

        if (e.NewValues[2] != null)
        {
            float.TryParse(e.NewValues[2].ToString(), out PreTestYield);
            vchSet.Append(Method.BuildXML(PreTestYield.ToString(), "PreTestYield"));
        }

        if (e.NewValues[3] != null)
        {
            float.TryParse(e.NewValues[3].ToString(), out RunInYield);
            vchSet.Append(Method.BuildXML(RunInYield.ToString(), "RunInYield"));
        }

        if (e.NewValues[2] != null && e.NewValues[3] != null)
        {
            //float.TryParse(e.NewValues[4].ToString(), out FPY);
            FPY = PreTestYield * RunInYield;
            vchSet.Append(Method.BuildXML(FPY.ToString(), "FPY"));
        }

        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        ((GridView)sender).EditIndex = -1;
        SqlDSProjectMaterialYield.DataBind();
        e.Cancel = true;
    }

    protected void lbtnAddProjectMaterial_Click(object sender, EventArgs e)
    {
        fvProjectMaterial.Visible = true;
    }

    protected void gvGroupRelationOwner_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string ID = e.Keys[0].ToString();
        RemoveAOwner(ID);
        GridView gv = (GridView)sender;
        gv.EditIndex = -1;
        gv.SelectedIndex = -1;
        gv.DataBind();
        e.Cancel = true;
    }

    protected void gvGroupRelationMember_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string ID = e.Keys[0].ToString();
        RemoveAOwner(ID);
        GridView gv = (GridView)sender;
        gv.EditIndex = -1;
        gv.SelectedIndex = -1;
        gv.DataBind();
        e.Cancel = true;
    }

    protected void RemoveAOwner(string ID)
    {
        string vchCmd = "DELETE";
        string vchObjectName = "a_Owner";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ID, "ID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        hfModuleID.Value = ddlModule.SelectedValue;
        hfCustomerID.Value = ddlCustomer.SelectedValue;
        hfProjectID.Value = ddlProject.SelectedValue;
        hfSiteID.Value = ddlSite.SelectedValue;
        string ModuleName = ddlModule.SelectedItem.Text;
        string CustomerName = ddlCustomer.SelectedItem.Text;
        string ProjectName = ddlProject.SelectedItem.Text;
        gvProjectPhase.Caption = CustomerName + " > " + ModuleName + " > " + ProjectName;
        CheckandAddProjectMaterialYield(hfProjectID.Value);
        gvProjectPhase.EditIndex = -1;
        GetCurrentPhase(hfProjectID.Value);
        gvProjectMaterialYield.EditIndex = -1;
        gvProjectMaterialYield.DataBind();
    }

    protected void CheckandAddProjectMaterialYield(string ProjectID)
    {
        if (ProjectID == "")
            return;

        string vchCmd = "CheckandAdd";
        string vchObjectName = "m_ProjectMaterialYield";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        GetCurrentPhase(ProjectID);
        gvProjectMaterialYield.DataBind();
    }

    protected void GetCurrentPhase(string ProjectID)
    {
        string vchCmd = "GetCurrentPhase";
        string vchObjectName = "dbo.fn_GetCurrentPhaseID";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        DataRow dr;
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                dr = ds.Tables[0].Rows[0];
                hfCurrentPhaseID.Value = dr[0].ToString();
                gvProjectMaterialYield.Caption = dr[1].ToString();
            }
    }
    protected void gvProjectPhase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string ID = e.Keys[0].ToString();
        string vchCmd = "Delete";
        string vchObjectName = "m_ProjectPhase";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ID, "ID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        ((GridView)sender).EditIndex = -1;
        SqlDSProjectPhase.DataBind();
        e.Cancel = true;
    }
    protected void btnTransferToMP_Click(object sender, EventArgs e)
    {
        hfProjectID.Value = ddlProject.SelectedValue;
        string ProjectID = hfProjectID.Value;
        bool IsMP = true;
        string vchCmd = "Transfer Project to MP";
        string vchObjectName = "m_Group.Project";

        StringBuilder vchSet = new StringBuilder();
        
        vchSet.Append(Method.BuildXML(ProjectID, "ProjectID"));
        vchSet.Append(Method.BuildXML(IsMP.ToString(), "IsMP"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if(ds.Tables.Count>0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string sMsg = dr[0].ToString();
                Method.MessageOut(Page, sMsg);
                return;
            }

        ddlProject.DataBind();
        ddlProject.SelectedIndex = 0;

        gvProjectMaterial.EditIndex = -1;
        gvProjectPhase.EditIndex = -1;
        gvRoleUser.EditIndex = -1;
        gvRoleUser.SelectedIndex = -1;
        gvGroupRelationOwner.SelectedIndex = -1;
        gvGroupRelationOwner.EditIndex = -1;
        gvGroupRelationMember.EditIndex = -1;
        gvGroupRelationMember.SelectedIndex = -1;

        gvProjectPhase.DataBind();
        gvProjectMaterial.DataBind();
        gvGroupRelationOwner.DataBind();
        gvGroupRelationMember.DataBind();

    }
    protected void fvProjectPhase_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
        {
            ((FormView)sender).Visible = false;
        }
        e.Cancel = true;
    }
    protected void fvProjectMaterial_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
        {
            ((FormView)sender).Visible = false;
        }
        e.Cancel = true;
    }
}
