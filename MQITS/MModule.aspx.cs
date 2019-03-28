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

public partial class MModule : System.Web.UI.Page
{
    const string sp_Module = "sp_Module";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitCondition();
        }
    }

    protected void InitCondition()
    {
        hfUserId.Value = Method.GetCurrentUserId(Page);
        fvModule.Visible = false;
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        fvModule.ChangeMode(FormViewMode.Insert);
        fvModule.Visible = true;
    }
    protected void fvModule_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string ModuleCode = "99999999";
        string vchCmd = "Add";
        string vchObjectName = "m_module";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(ModuleCode, "ModuleCode"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "ModuleName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "IsEOSL"));
        vchSet.Append(Method.BuildXML(e.Values[3].ToString(), "IsVirtual"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Module, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvModule.Visible = false;
        gvModule.DataBind();

        e.Cancel = true;
    }
    protected void gvModule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ModuleCode = e.Keys[0].ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_module";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(ModuleCode, "ModuleCode"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "ModuleName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "IsEOSL"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "IsVirtual"));
        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");

        sqlCmd = Method.GetSqlCmd(sp_Module, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvModule.EditIndex = -1;
        gvModule.DataBind();

        e.Cancel = true;
    }
    protected void fvModule_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
            e.Cancel = true;
    }
}
