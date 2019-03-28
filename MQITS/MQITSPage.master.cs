using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text;

public partial class MQITSPage : System.Web.UI.MasterPage
{
    const string CONSTPROC = "sp_TreeMenuRole";
    const string defaultURL = "MyIssueAction.aspx";
    public string UserId;
    TreeView tvPCB = new TreeView();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.InitCondition();
            InitFunctionKey();
        }
    }

    protected void InitLanguage()
    {

        switch (Constant.S_UILang)
        { 
            case "ENG":
                lblWebSiteName.Text = Constant.S_Test + Resources.ResourceENG.WebSiteName;
                lblWebSiteVer.Text = Constant.S_WebSiteVersion;
                break;
            case "CHT":
                lblWebSiteName.Text = Constant.S_Test + Resources.ResourceCHT.WebSiteName;
                lblWebSiteVer.Text = Constant.S_WebSiteVersion;
                break;
            default:
                break;
        }
    }

    protected void InitCondition()
    {
        if (Session["UILang"] != null)
        {
            if (Session["UILang"].ToString().Trim() != "")
            {
                ddlUILang.SelectedItem.Value = (string)Session["UILang"];
            }
            else
            {
                ddlUILang.SelectedIndex = 1;
            }
        }
        else
        {
            ddlUILang.SelectedIndex = 1;
        }
        //Constant.S_UILang = ddlUILang.Text;
        ddlUILang.Visible = false;
        InitLanguage();

        if (Session["UserId"] != null)
        {
            txtUserIdSim.Text = Session["UserId"].ToString();
        }

        if (txtUserIdSim.Text.Trim() == "")
        {
            txtUserId.Text = Method.GetCurrentUserId(Page);
        }
        else
        {
            int iDomain = txtUserIdSim.Text.Trim().IndexOf(@"\", 0);
            if (iDomain > 0 && txtUserIdSim.Text.Trim().Length > 9)
                txtUserIdSim.Text = txtUserIdSim.Text.Trim().Substring(iDomain + 1);

            txtUserId.Text = txtUserIdSim.Text.Trim().ToUpper();
        }

        if (Session["EnableSendMail"] != null)
        {
            bool blSendMail = true;
            bool.TryParse(Session["EnableSendMail"].ToString(), out blSendMail);
            chkSendMail.Checked = blSendMail;
        }

        Session["UserId"] = txtUserId.Text;
        Session["EnableSendMail"] = chkSendMail.Checked.ToString();
        SendMail.EnableSendMail = Method.EnableSendmail(Page);

        InitMenuItem();
        lblUserName.Text = Method.GetUserName(txtUserId.Text);
        if (lblUserName.Text.Trim() != "")
        {
            StringBuilder myValue = new StringBuilder();
            myValue.Append("<span style='color:Olive'> ~</span>");
            myValue.Append(" Welcome ");
            myValue.Append("<span style='color:darkblue;'>");
            myValue.Append(lblUserName.Text);
            myValue.Append("</span>");
            myValue.Append("<span style='color:Olive'> ~</span>");
            lblWelcome.Text = myValue.ToString();
        }
        
    }

    private void InitMenuItem()
    {
        try
        {
            UserId = txtUserId.Text;
            menuMQITS.Items.Clear();
            MenuItem mit = new MenuItem();

            GenerateMenuItem(0, mit);
            tvPCB.ShowExpandCollapse = true;
            tvPCB.Visible = true;
        }
        catch (Exception ex)
        {
            this.Response.Write(ex.Message);
        }
    }

    private void GenerateMenuItem(int rootId, MenuItem rootMenu)
    {
        string vchType = "selectMenuItem";
        StringBuilder vchParameters = new StringBuilder();
        vchParameters.Append(Method.BuildXML(rootId.ToString(), "parent_node_id"));
        vchParameters.Append(Method.BuildXML(txtUserId.Text, "user_id"));

        string sqlCmd = Method.GetSqlCmd(CONSTPROC, vchType, vchParameters.ToString()); // "EXEC sp_TreeMenu 'selectTreeNode', '<parent_node_id>" + rootId + "</parent_node_id><user_id>" + txtUserId.Text + "</user_id>'";
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            MenuItem mit = new MenuItem();
            object[] rootData = ds.Tables[0].Rows[i].ItemArray;

            mit.Value = rootData[0].ToString();
            mit.Text = (string)rootData[3];
            if (rootId == 0)
            {
                menuMQITS.Items.Add(mit);
            }
            else
            {
                mit.NavigateUrl = (string)rootData[6];
                mit.Target = (string)rootData[7];
                rootMenu.ChildItems.Add(mit);
            }

            GenerateMenuItem((int)rootData[0], mit);
        }
    }

    private void InitTreeMenu()
    {
        try
        {
            UserId = txtUserId.Text;
            tvPCB.Nodes.Clear();
            TreeNode tr = new TreeNode();
            GenerateTreeMenu(0, tr);
            tvPCB.ShowExpandCollapse = true;
            tvPCB.Visible = true;
        }
        catch (Exception ex)
        {
            this.Response.Write(ex.Message);
        }
    }

    private void InitFunctionKey()
    {
        string userid = Method.GetLogonUserId(Page);
        string vchType = "SelectFunctionKey";
        StringBuilder parameters = new StringBuilder();
        parameters.Append(Method.BuildXML("MQITSPage.master", "navigate_url"));
        parameters.Append(Method.BuildXML(userid, "user_id"));

        string sqlCmd = Method.GetSqlCmd(CONSTPROC, vchType, parameters.ToString());

        try
        {
            //sqlCmd = "EXEC sp_TreeMenu @type='SelectFunctionKey', @parameters='<navigate_url>PCBMasterPage.master</navigate_url><user_id>" + UserId + "</user_id>'";
            string[] arrRslt = DAO.sqlCmdArrSingleCol(Constant.S_MQITSConnStr, sqlCmd);
            this.txtUserIdSim.Visible = bool.Parse(arrRslt[0]);
            this.bthSubmit.Visible = bool.Parse(arrRslt[0]);
            if (bthSubmit.Visible == false)
            {
                spanAdmin.Attributes.Add("style", "display: none;");
            }
            else
            {
                spanAdmin.Attributes.Add("style", "display: block;");
            }
        }
        catch (Exception ex)
        {
            this.Response.Write(ex.Message);
        }
    }

    private void GenerateTreeMenu(int rootId, TreeNode rootNode)
    {
        string vchType = "selectTreeNode";
        StringBuilder vchParameters = new StringBuilder();
        vchParameters.Append(Method.BuildXML(rootId.ToString(), "parent_node_id"));
        vchParameters.Append(Method.BuildXML(txtUserId.Text, "user_id"));

        string sqlCmd = Method.GetSqlCmd(CONSTPROC, vchType, vchParameters.ToString()); // "EXEC sp_TreeMenu 'selectTreeNode', '<parent_node_id>" + rootId + "</parent_node_id><user_id>" + txtUserId.Text + "</user_id>'";
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TreeNode tr = new TreeNode();
            object[] rootData = ds.Tables[0].Rows[i].ItemArray;

            tr.Value = rootData[0].ToString();
            tr.Text = (string)rootData[3];
            if (rootId == 0)
            {
                tvPCB.Nodes.Add(tr);
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

    protected void bthSubmit_Click(object sender, EventArgs e)
    {
        Session["UserId"] = null;
        Session["UserRoleCode"] = null;
        Session["EnableSendMail"] = null;
        InitCondition();
        Response.Redirect(defaultURL);
    }
    protected void ddlUILang_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["UILang"] = ddlUILang.Text;
        Constant.S_UILang = ddlUILang.Text;
        InitLanguage();
    }
}
