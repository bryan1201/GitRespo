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

public partial class MCustomer : System.Web.UI.Page
{
    const string sp_Customer = "sp_Customer";

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
        fvCustomer.Visible = false;
    }

    protected void gvCustomer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string CustomerID = e.Keys[0].ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_customer";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "CustomerName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "IsEnable"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");

        sqlCmd = Method.GetSqlCmd(sp_Customer, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvCustomer.EditIndex = -1;
        gvCustomer.DataBind();

        e.Cancel = true;
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        fvCustomer.ChangeMode(FormViewMode.Insert);
        fvCustomer.Visible = true;
    }
    protected void fvCustomer_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
            e.Cancel = true;
    }
    protected void fvCustomer_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string CustomerID = "99999999";
        string vchCmd = "Add";
        string vchObjectName = "m_customer";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "CustomerName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "IsEnable"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Customer, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvCustomer.Visible = false;
        gvCustomer.DataBind();

        e.Cancel = true;
    }

    protected void lbtnAddPhase_Click(object sender, EventArgs e)
    {
        fvPhase.ChangeMode(FormViewMode.Insert);
        fvPhase.Visible = true;
    }
    protected void lbtnAddStation_Click(object sender, EventArgs e)
    {
        fvStation.ChangeMode(FormViewMode.Insert);
        fvStation.Visible = true;
    }

    protected void fvPhase_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        if (gvCustomer.SelectedIndex == -1)
            return;
        string CustomerID = gvCustomer.SelectedDataKey[0].ToString();
        string vchCmd = "AddPhase";
        string vchObjectName = "m_Group";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "PhaseName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "IsEnable"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_Customer, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvPhaseTemplate.DataBind();
        e.Cancel = true;
    }
    protected void gvPhaseTemplate_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string CustomerID = gvCustomer.SelectedDataKey[0].ToString();
        string PhaseID = e.Keys[0].ToString();
        string vchCmd = "UPDATEPHASE";
        string vchObjectName = "m_Group";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(PhaseID, "PhaseID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "PhaseName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "IsEnable"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");

        sqlCmd = Method.GetSqlCmd(sp_Customer, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        this.gvPhaseTemplate.EditIndex = -1;
        gvPhaseTemplate.DataBind();

        e.Cancel = true;
    }
    protected void gvStation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ID = e.Keys[0].ToString();
        string CustomerID = gvCustomer.SelectedDataKey[0].ToString();

        string vchCmd = "UPDATESTATION";
        string vchObjectName = "m_maintain";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(ID, "ID"));
        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "StationName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "MType"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(e.NewValues[3].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Customer, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvStation.EditIndex = -1;
        gvStation.DataBind();

        e.Cancel = true;
    }

    protected void fvStation_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string StationID = "99999999";
        string CustomerID = gvCustomer.SelectedDataKey[0].ToString();
        string vchCmd = "AddStation";
        string vchObjectName = "m_maintain";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(StationID, "StationID"));
        vchSet.Append(Method.BuildXML(CustomerID, "CustomerID"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "StationName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "MType"));
        vchSet.Append(Method.BuildXML(e.Values[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(e.Values[3].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Customer, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvCustomer.Visible = false;
        gvStation.DataBind();

        e.Cancel = true;
    }
    protected void FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        FormView fv = (FormView)sender;
        if (e.CommandName == "Cancel")
            fv.Visible = false;
    }
}
