using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class MProjectMaterial : System.Web.UI.Page
{
    const string sp_GroupMaintain = "sp_GroupMaintain";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            hfUserId.Value = Method.GetCurrentUserId(Page);
    }

    protected void lbtnAddProjectMaterial_Click(object sender, EventArgs e)
    {
        fvProjectMaterial.Visible = true;
    }

    protected void fvProjectMaterial_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string vchCmd = "Add";
        string vchObjectName = "m_ProjectMaterial";
        string MType = e.Values[1].ToString();
        bool IsInUse = true;
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
        if (ddlProject.SelectedIndex == -1)
            return;
        hfProjectID.Value = ddlProject.SelectedValue;
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
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        hfProjectID.Value = ddlProject.SelectedValue;
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