using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Data;

public partial class MPersonalProfile : System.Web.UI.Page
{
    const string sp_RoleMaintain = "sp_RoleMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            hfUserId.Value = Method.GetCurrentUserId(Page);
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        fvAgent.ChangeMode(FormViewMode.Insert);
    }

    protected void fvAgent_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        DataSet dsMsg;
        string vchCmd = "Add";
        string vchObjectName = "m_useragent";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "BadgeCode"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "ChtName"));
        vchSet.Append(Method.BuildXML(e.Values[3].ToString(), "email"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
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

        fvAgent.ChangeMode(FormViewMode.ReadOnly);
        gvAgent.DataBind();
        e.Cancel = true;
    }

    protected void gvAgent_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataSet dsMsg;
        string vchCmd = "UPDATE";
        string vchObjectName = "m_useragent";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(e.Keys[0].ToString(), "ID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));

        sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        this.gvAgent.EditIndex = -1;
        e.Cancel = true;
    }
    protected void gvAgent_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataSet dsMsg;
        int iIndex = (int)e.RowIndex;
        string RoleCode = gvAgent.SelectedDataKey.Value.ToString();
        string vchCmd = "Delete";
        string vchObjectName = "m_useragent";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(e.Keys[0].ToString(), "ID"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));

        sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        dsMsg = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        this.gvAgent.EditIndex = -1;
        e.Cancel = true;
    }
    protected void fvUserProfile_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        DataSet dsMsg;
        string vchCmd = "ENABLEAGENT";
        string vchObjectName = "m_userprofile";
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "EnableAgent"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
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

        fvUserProfile.ChangeMode(FormViewMode.ReadOnly);
        gvAgent.DataBind();
        e.Cancel = true;
    }
}
