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

public partial class MGroup : System.Web.UI.Page
{
    const string sp_GroupMaintain = "sp_GroupMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTree("0");
            hfUserId.Value = Method.GetCurrentUserId(Page);
            mvGroupMaintain.ActiveViewIndex = 0;
        }
    }

    private void BindTree(string RoleCode)
    {
        TreeNode tr = new TreeNode();
        tvGroup.Nodes.Clear();
        GenerateTreeGroup(0, tr);

        /*
        string vchCmd = "SELECT";
        string vchObjectName = "a_Owner";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        tvGroup.Nodes.Clear();

        ArrayList nodeID = DAO.SelCmdArr(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < nodeID.Count; i++)
        {
            for (int j = 0; j < tvGroup.Nodes.Count; j++)
            {
                CheckNode((string)nodeID[i], tvGroup.Nodes[j]);
            }
        }
        */
        
        tvGroup.Visible = true;
    }

    private void GenerateTreeGroup(Int64 rootId, TreeNode rootNode)
    {
        string vchCmd = "Select";
        string vchObjectName = "TREEGROUP";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(rootId.ToString(), "GroupID"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
            
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TreeNode tr = new TreeNode();
            object[] rootData = ds.Tables[0].Rows[i].ItemArray;

            tr.Value = rootData[0].ToString();
            tr.Text = (string)rootData[1];
            bool blChk = false;
            bool.TryParse(rootData[5].ToString(), out blChk);
            tr.Checked = blChk;
            if (rootId == 0)
            {
                tvGroup.Nodes.Add(tr);
                tr.Expanded = true;
            }
            else
            {
                //tr.NavigateUrl = (string)rootData[0];
                tr.Target = (string)rootData[8];
                rootNode.ChildNodes.Add(tr);
                tr.Expanded = true;
            }

            long rootId64 = 0;
            rootId64 = Int64.Parse(tr.Value);
            GenerateTreeGroup(rootId64, tr);
        }
    }

    private void CheckNode(string str, TreeNode tn)
    {
        if (tn.Value == str)
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

    protected void BindRoleUser(long GroupID)
    { 
        string vchCmd = "Select";
        string vchObjectName = "v_GroupRoleUser";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(GroupID.ToString(), "GroupID"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
    }
    /*
    protected void BindOwner(long GroupID)
    {
        string vchCmd = "Select";
        string vchObjectName = "v_GroupOwner";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(GroupID.ToString(), "GroupID"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        SqlDSGroupOwner.SelectCommand = sqlCmd;
        SqlDSGroupOwner.ConnectionString = Constant.S_MQITSConnStr;
        gvGroupOwner.DataSourceID = SqlDSGroupOwner.UniqueID;
        SqlDSGroupOwner.DataBind();
    }

    protected void tvGroup_SelectedNodeChanged(object sender, EventArgs e)
    {
        string GroupID = ((TreeView)sender).SelectedValue;
        long Int64GroupID = 0;
        Int64.TryParse(GroupID, out Int64GroupID);
        BindOwner(Int64GroupID);
    }
     */
    protected void btnFindRole_Click(object sender, EventArgs e)
    {
        txtFindRole.Text = txtFindRole.Text.Trim();
        if (txtFindRole.Text.Trim() == "")
            txtFindRole.Text = "%";
    }

    private void BindOwnerGroupTree(string RoleCode)
    {
        TreeNode tr = new TreeNode();
        tvOwnerGroup.Nodes.Clear();

        GenerateOwnerGroupTree(RoleCode, 0, tr);

        /*
        string vchCmd = "SELECT";
        string vchObjectName = "a_Owner";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        tvGroup.Nodes.Clear();

        ArrayList nodeID = DAO.SelCmdArr(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < nodeID.Count; i++)
        {
            for (int j = 0; j < tvGroup.Nodes.Count; j++)
            {
                CheckNode((string)nodeID[i], tvGroup.Nodes[j]);
            }
        }
        */

        this.tvOwnerGroup.Visible = true;
    }

    private void GenerateOwnerGroupTree(string RoleCode, Int64 rootId, TreeNode rootNode)
    {
        string vchCmd = "Select";
        string vchObjectName = "V_OWNERGROUPTREE";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(rootId.ToString(), "GroupID"));
        vchSet.Append(Method.BuildXML(RoleCode, "RoleCode"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());

        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TreeNode tr = new TreeNode();
            object[] rootData = ds.Tables[0].Rows[i].ItemArray;

            tr.Value = rootData[0].ToString();
            tr.Text = (string)rootData[2];
            bool blChk = false;
            bool.TryParse(rootData[7].ToString(),out blChk);
            tr.Checked = blChk;
            if (rootId == 0)
            {
                this.tvOwnerGroup.Nodes.Add(tr);
                tr.Expanded = true;
            }
            else
            {
                //tr.NavigateUrl = (string)rootData[0];
                //tr.Target = "";
                rootNode.ChildNodes.Add(tr);
                tr.Expanded = true;
            }

            long rootId64 = 0;
            rootId64 = Int64.Parse(tr.Value);
            GenerateOwnerGroupTree(RoleCode, rootId64, tr);
        }
    }
    protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string RoleCode = "";
        if (e.CommandName == "Select" && gvRole.SelectedIndex != -1)
        {
            RoleCode = ((GridView)sender).DataKeys[this.gvRole.SelectedIndex].Value.ToString();
            BindOwnerGroupTree(RoleCode);
            //gvGroupOwner.DataBind();
        }
    }
    protected void menuGroupMaintain_MenuItemClick(object sender, MenuEventArgs e)
    {
        int imen = int.Parse(e.Item.Value);
        this.mvGroupMaintain.ActiveViewIndex = imen;
    }
    protected void gvRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        string RoleCode = gvRole.DataKeys[this.gvRole.SelectedIndex].Value.ToString();
        BindOwnerGroupTree(RoleCode);
    }
    protected void fvOwnerGroup_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        // Add Role or User into Group
        // Assign Roles or Users to be Owner or Member of the Group
        // Keys: GroupID, OwnerType: U/R, IssueType:1 IssueOwner/2 Issue Member
        string vchCmd = "Update";
        string vchObjectName = "a_Owner";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "GroupID"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "OwnerCode"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "OwnerType"));
        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "IssueType"));
        vchSet.Append(Method.BuildXML(e.NewValues[4].ToString(), "IsEnable"));
        vchSet.Append(Method.BuildXML(e.NewValues[5].ToString(), "IsReadOnly"));
        vchSet.Append(Method.BuildXML(e.NewValues[6].ToString(), "OwnerID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        e.Cancel = true;
    }
    protected void tvOwnerGroup_SelectedNodeChanged(object sender, EventArgs e)
    {
        //fvOwnerGroup.DataBind();
    }
    protected void tvGroup_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        // Enable or Disable Group: Customer, Phase, Project, Site
        TreeNode tn = e.Node;
        bool check = tn.Checked;

        string vchCmd = "UPDATE";
        string vchObjectName = "m_Group";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(tn.Value, "GroupID"));
        vchSet.Append(Method.BuildXML(check.ToString(), "IsEnable"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_GroupMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
    }
    protected void btnRenewGroup_Click(object sender, EventArgs e)
    {
        BindTree("0");
    }
    protected void tvGroup_SelectedNodeChanged(object sender, EventArgs e)
    {
        gvGroupOwner.Caption = tvGroup.SelectedNode.Text;
    }

    protected void lbtnAdd_Command(object sender, CommandEventArgs e)
    {
        // Add Role or User into Group
        // Assign Roles or Users to be Owner or Member of the Group
        // Keys: GroupID, OwnerType: U/R, IssueType:1 IssueOwner/2 Issue Member
    }
}
