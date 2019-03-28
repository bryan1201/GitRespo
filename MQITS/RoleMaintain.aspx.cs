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
using System.Data.SqlClient;
using System.Text;

public partial class RoleMaintain : System.Web.UI.Page
{
    const string sp_RoleMaintain = "sp_RoleMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
       
       if(gvRole.SelectedIndex == -1)
            gvUserRole.Enabled = false;
        
        if (!IsPostBack)
        {
            txtUserId.Text = Method.GetCurrentUserId(Page);
            mvRolemaintain.ActiveViewIndex = 0;
        }
    }

    protected void BindRole()
    {
        string sqlCmd = "sp_RoleMaintain";
        sqlCmd = "sp_RoleMaintain";
        SqlConnection conn = new SqlConnection(Constant.S_MQITSConnStr);
        conn.Open();
        this.SqlDSRole.SelectCommand = sqlCmd;
        this.SqlDSRole.SelectParameters.Add("vchCmd", TypeCode.String, "");
        this.SqlDSRole.SelectParameters.Add("vchObjectName", TypeCode.String, "");
        this.SqlDSRole.SelectParameters.Add("vchSet", TypeCode.String, "");
        this.SqlDSRole.SelectParameters["vchCmd"].DefaultValue = "SELECT";
        this.SqlDSRole.SelectParameters["vchObjectName"].DefaultValue = "v_role";
        this.SqlDSRole.SelectParameters["vchSet"].DefaultValue = "";
        this.SqlDSRole.Select(DataSourceSelectArguments.Empty);
        SqlDSRole.DataBind();
        this.gvRole.DataSourceID = SqlDSRole.UniqueID;
        gvRole.DataBind();
    }

    private void BindTree(string RoleCode)
    {
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        string vchCmd = "SELECT";
        string vchObjectName = "m_rolefunction";

        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        //"EXEC sp_RoleMaintain @vchCmd='" + vchCmd + "', @vchObjectName='" + vchObjectName + "', @vchSet='" + vchSet + "'";
        tvRFunction.Nodes.Clear();
        ArrayList nodeID = DAO.SelCmdArr(Constant.S_MQITSConnStr, sqlCmd);
        TreeNode tr = new TreeNode();

        GenerateTreeMenu(0, tr);

        for (int i = 0; i < nodeID.Count; i++)
        {
            for (int j = 0; j < tvRFunction.Nodes.Count; j++)
            {
                CheckNode((string)nodeID[i], tvRFunction.Nodes[j]);
            }
        }
        tvRFunction.Visible = true;
    }

    protected void gvRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvUserRole.Enabled = true;
        string RoleCode = gvRole.DataKeys[this.gvRole.SelectedIndex].Value.ToString();
        BindTree(RoleCode);
        this.gvUserRole.DataBind();
        fvRole.Visible = true;
        fvRole.ChangeMode(FormViewMode.ReadOnly);
    }

    protected void gvUserRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataSet dsMsg;
        int iIndex = (int)e.RowIndex;
        string RoleCode = gvRole.SelectedDataKey.Value.ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "v_userrole";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(((CheckBox)((GridView)sender).Rows[iIndex].FindControl("chkEUserIsInUse")).Checked.ToString(), "UserIsInUse"));
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        vchSet.Append(Method.BuildXML(gvUserRole.DataKeys[iIndex].Value.ToString(), "BadgeCode"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));

        sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        this.gvUserRole.EditIndex = -1;
        fvRole.DataBind();
        e.Cancel = true;
    }
    protected void gvUserRole_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string RoleCode = this.gvRole.SelectedDataKey.Value.ToString();
        string vchCmd = "DELETE";
        string vchObjectName = "v_userrole";
        string vchSet = "";
        string sqlCmd = "";
        
        vchSet += "<UserIsInUse>" + ((CheckBox)((GridView)sender).Rows[e.RowIndex].FindControl("chkTUserIsInUse")).Checked.ToString() + "</UserIsInUse>";
        vchSet += "<RoleCode>" + RoleCode + "</RoleCode>";
        vchSet += "<BadgeCode>" + gvUserRole.DataKeys[e.RowIndex].Value.ToString() + "</BadgeCode>";
        vchSet += "<editor>" + txtUserId.Text + "</editor>";

        sqlCmd = "EXEC sp_RoleMaintain @vchCmd='" + vchCmd + "', @vchObjectName='" + vchObjectName + "',@vchSet='" + vchSet + "'";
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

        fvUserRole.ChangeMode(FormViewMode.ReadOnly);
        ((GridView)sender).DataBind();
        e.Cancel = true;
    }
    private void GenerateTreeMenu(int rootId, TreeNode rootNode)
    {
        string vchCmd = "Select";
        string vchObjectName = "TreeMenu";
        string vchSet = "";
        vchSet = "<rootId>" + rootId + "</rootId>";
        string sqlCmd = "EXEC sp_RoleMaintain @vchCmd = '" + vchCmd + "', @vchObjectName='" + vchObjectName + "', @vchSet='" + vchSet + "'";
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TreeNode tr = new TreeNode();
            object[] rootData = ds.Tables[0].Rows[i].ItemArray;

            tr.Value = rootData[0].ToString();
            tr.Text = (string)rootData[3];
            if (rootId == 0)
            {
                tvRFunction.Nodes.Add(tr);
                tr.Expanded = true;
            }
            else
            {
                tr.NavigateUrl = (string)rootData[6];
                tr.Target = (string)rootData[7];

                rootNode.ChildNodes.Add(tr);
                tr.Expanded = true;
            }

            GenerateTreeMenu((int)rootData[0], tr);
        }
    }

    private void CheckNode(string str, TreeNode tn)
    { 
        if(tn.Value == str)
        {
            tn.Checked = true;
        }

        foreach (TreeNode chkN in tn.ChildNodes)
        {
            if (chkN.Value == str)
            {
                chkN.Checked = true;
                CheckNode(str, chkN);
            }
            else
            {
                CheckNode(str, chkN);
            }
        }
    }

    protected void tvRFunction_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (gvRole.SelectedIndex == -1)
            return;
        string RoleCode = gvRole.DataKeys[this.gvRole.SelectedIndex].Value.ToString();

        TreeNode tn = e.Node;

        bool check = tn.Checked;

        StringBuilder vchSet = new StringBuilder();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_rolefunction";
        vchSet.Append(Method.BuildXML(tn.Value, "function_id"));
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        vchSet.Append(Method.BuildXML(check.ToString(), "ChkNode"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
    }

    protected void fvRole_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        switch (e.CommandName)
        { 
            case "Cancel":
                fvRole.ChangeMode(FormViewMode.ReadOnly);
                //fvRole.Visible = false;
                break;
            case "Edit":
                fvRole.ChangeMode(FormViewMode.Edit);
                break;
            case "Insert":
                fvRole.ChangeMode(FormViewMode.Insert);
                break;
            default:
                //fvRole.ChangeMode(FormViewMode.ReadOnly);
                break;
        }
    }

    protected void lbtnRoleCancel_Click(object sender, EventArgs e)
    {
        fvRole.ChangeMode(FormViewMode.ReadOnly);
        fvRole.Visible = false;
    }

    protected void lbtnRoleNew_Click(object sender, EventArgs e)
    {
        fvRole.ChangeMode(FormViewMode.Insert);
    }
    protected void fvRole_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        string RoleCode = e.Keys[0].ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_role";
        string Site = "";

        if (e.NewValues[1] != null)
            Site = e.NewValues[1].ToString();

        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "RoleName"));
        vchSet.Append(Method.BuildXML(Site, "Site"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "sortorder"));
        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "IsPrjRole"));
        vchSet.Append(Method.BuildXML(e.NewValues[4].ToString(), "IsSigner"));
        vchSet.Append(Method.BuildXML(e.NewValues[5].ToString(), "IsUserRole"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvRole.ChangeMode(FormViewMode.ReadOnly);
        gvRole.DataBind();
        e.Cancel = true;
    }
    protected void fvRole_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string RoleCode = "99999999";
        string vchCmd = "Add";
        string vchObjectName = "m_role";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        string ModuleCode = gvTreeModuleRole.DataKeys[gvTreeModuleRole.SelectedIndex].Value.ToString();

        string Site = "";

        if (e.Values[1] != null)
            Site = e.Values[1].ToString();

        //vchSet.Append(Method.BuildXML(gvForm.DataKeys[gvForm.SelectedIndex].Value.ToString(), "FormCode"));
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "RoleName"));
        vchSet.Append(Method.BuildXML(Site, "Site"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "sortorder"));
        vchSet.Append(Method.BuildXML(e.Values[3].ToString(), "IsPrjRole"));
        vchSet.Append(Method.BuildXML(e.Values[4].ToString(), "IsSigner"));
        vchSet.Append(Method.BuildXML(e.Values[5].ToString(), "IsUserRole"));
        vchSet.Append(Method.BuildXML(ModuleCode, "ModuleCode"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));

        sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvRole.ChangeMode(FormViewMode.ReadOnly);
        gvRole.DataBind();

        e.Cancel = true;
    }

    protected void lblUserRoleAddUser_Click(object sender, EventArgs e)
    {
        fvUserRole.ChangeMode(FormViewMode.Insert);
    }

    protected void fvUserRole_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        DataSet dsMsg;
        string RoleCode = gvRole.DataKeys[this.gvRole.SelectedIndex].Value.ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "v_userrole";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(((CheckBox)((FormView)sender).FindControl("chkUserIsInUse")).Checked.ToString(), "UserIsInUse"));
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "BadgeCode_Org"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString().ToUpper(), "BadgeCode"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "ChtName"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "email"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());

        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (dsMsg.Tables.Count > 0)
            if (dsMsg.Tables[0].Rows.Count > 0)
            {
                DataRow drMsg = dsMsg.Tables[0].Rows[0];
                string CmdArg = drMsg["CmdArg"].ToString();
                string sMessageOut = drMsg["MessageOut"].ToString();
                if (CmdArg == Constant.SAVE)
                    Method.MessageOut(Page, sMessageOut);
            }

        fvUserRole.ChangeMode(FormViewMode.ReadOnly);
        gvUserRole.DataBind();
        e.Cancel = true;
    }

    protected void fvUserRole_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        DataSet dsMsg;
        string RoleCode = gvRole.DataKeys[this.gvRole.SelectedIndex].Value.ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "v_userrole";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(((CheckBox)((FormView)sender).FindControl("chkUserIsInUse")).Checked.ToString(), "UserIsInUse"));
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        vchSet.Append(Method.BuildXML(e.OldValues[0].ToString(), "BadgeCode_Org"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString().ToUpper(), "BadgeCode"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "ChtName"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "email"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());

        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (dsMsg.Tables.Count > 0)
            if (dsMsg.Tables[0].Rows.Count > 0)
            {
                DataRow drMsg = dsMsg.Tables[0].Rows[0];
                string CmdArg = drMsg["CmdArg"].ToString();
                string sMessageOut = drMsg["MessageOut"].ToString();
                if (CmdArg == Constant.SAVE)
                    Method.MessageOut(Page, sMessageOut);
            }

        fvUserRole.ChangeMode(FormViewMode.ReadOnly);
        gvUserRole.DataBind();
        e.Cancel = true;
    }
    protected void fvUserRole_ItemDeleting(object sender, FormViewDeleteEventArgs e)
    {
        string RoleCode = gvRole.SelectedDataKey.Value.ToString();
        string vchCmd = "DELETE";
        string vchObjectName = "v_userrole";
        string vchSet = "";
        string sqlCmd = "";
        vchSet += "<UserIsInUse>" + ((CheckBox)((FormView)sender).FindControl("chkUserIsInUse")).Checked.ToString() + "</UserIsInUse>";
        vchSet += "<RoleCode>" + RoleCode + "</RoleCode>";
        vchSet += "<BadgeCode>" + e.Values[0].ToString() + "</BadgeCode>";
        vchSet += "<editor>" + txtUserId.Text + "</editor>";

        sqlCmd = "EXEC sp_RoleMaintain @vchCmd='" + vchCmd + "', @vchObjectName='" + vchObjectName + "',@vchSet='" + vchSet + "'";
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

        fvUserRole.ChangeMode(FormViewMode.ReadOnly);
        gvUserRole.DataBind();
        e.Cancel = true;
    }

    protected void lbtnAddRole_Click(object sender, EventArgs e)
    {
        fvRole.ChangeMode(FormViewMode.Insert);
    }
    protected void lbtnUserRoleAdd_Click(object sender, EventArgs e)
    {
        fvUserRole.ChangeMode(FormViewMode.Insert);
    }

    protected void btnFindRole_Click(object sender, EventArgs e)
    {
        txtFindRole.Text = txtFindRole.Text.Trim().ToUpper();

        if (txtFindRole.Text.Trim() == "")
            txtFindRole.Text = "%";
    }

    protected void btnFindMember_Click(object sender, EventArgs e)
    {
        txtFindMember.Text = txtFindMember.Text.Trim().ToUpper();
        if (txtFindMember.Text.Trim() == "")
            txtFindMember.Text = "%";
    }
    protected void gvRole_RowEditing(object sender, GridViewEditEventArgs e)
    {
        fvRole.ChangeMode(FormViewMode.Edit);
        fvRole.Visible = true;
    }
    protected void menuRolemaintain_MenuItemClick(object sender, MenuEventArgs e)
    {
        int imen = int.Parse(e.Item.Value);
        mvRolemaintain.ActiveViewIndex = imen;
    }
    protected void btnUserFind_Click(object sender, EventArgs e)
    {
        txtUserFind.Text = txtUserFind.Text.Trim().Replace("'", "''").ToUpper();
        txtUserFind.Text = txtUserFind.Text.Replace("*","%");
        txtUserFind.Text = txtUserFind.Text.Replace("?", "_");

        if (txtUserFind.Text == "")
            txtUserFind.Text = "%";

        SqlDSUser.DataBind();
    }
    protected void lbtnUserAdd_Click(object sender, EventArgs e)
    {
        fvUser.ChangeMode(FormViewMode.Insert);
    }

    protected void fvUser_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        DataSet dsMsg;
        string vchCmd = "Add";
        string vchObjectName = "v_user";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "UserIsInUse"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "BadgeCode"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "ChtName"));
        vchSet.Append(Method.BuildXML(e.Values[3].ToString(), "email"));
        vchSet.Append(Method.BuildXML(e.Values[4].ToString(), "Description"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());

        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (dsMsg.Tables.Count > 0)
            if (dsMsg.Tables[0].Rows.Count > 0)
            {
                DataRow drMsg = dsMsg.Tables[0].Rows[0];
                string CmdArg = drMsg["CmdArg"].ToString();
                string sMessageOut = drMsg["MessageOut"].ToString();
                if (CmdArg == Constant.SAVE)
                    Method.MessageOut(Page, sMessageOut);
            }

        fvUser.ChangeMode(FormViewMode.ReadOnly);
        gvUser.DataBind();
        e.Cancel = true;
    }
    protected void fvUser_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        DataSet dsMsg;
        string BadgeCode = gvUser.DataKeys[this.gvUser.SelectedIndex].Value.ToString();
        string vchCmd = "Update";
        string vchObjectName = "v_user";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "UserIsInUse"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "BadgeCode"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "ChtName"));
        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "email"));
        vchSet.Append(Method.BuildXML(e.NewValues[4].ToString(), "Description"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());

        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (dsMsg.Tables.Count > 0)
            if (dsMsg.Tables[0].Rows.Count > 0)
            {
                DataRow drMsg = dsMsg.Tables[0].Rows[0];
                string CmdArg = drMsg["CmdArg"].ToString();
                string sMessageOut = drMsg["MessageOut"].ToString();
                if (CmdArg == Constant.SAVE)
                    Method.MessageOut(Page, sMessageOut);
            }

        fvUser.ChangeMode(FormViewMode.ReadOnly);
        gvUser.DataBind();
        e.Cancel = true;
    }

    protected void lblUserAdd_Click(object sender, EventArgs e)
    {
        fvUser.ChangeMode(FormViewMode.Insert);
    }
    protected void gvUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet dsMsg;
        string BadgeCode = e.Keys[0].ToString();
        string vchCmd = "Delete";
        string vchObjectName = "v_user";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(BadgeCode, "BadgeCode"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());

        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (dsMsg.Tables.Count > 0)
            if (dsMsg.Tables[0].Rows.Count > 0)
            {
                DataRow drMsg = dsMsg.Tables[0].Rows[0];
                string CmdArg = drMsg["CmdArg"].ToString();
                string sMessageOut = drMsg["MessageOut"].ToString();
                if (CmdArg == Constant.SAVE)
                    Method.MessageOut(Page, sMessageOut);
            }

        fvUser.ChangeMode(FormViewMode.ReadOnly);
        gvUser.DataBind();
        e.Cancel = true;
    }
    
}
