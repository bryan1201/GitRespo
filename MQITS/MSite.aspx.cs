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

public partial class MSite : System.Web.UI.Page
{
    const string sp_Site = "sp_Site";

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
        fvSite.Visible = false;
        fvSiteLine.Visible = false;
        fvSiteShift.Visible = false;
    }

    protected void gvSite_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string SiteID = e.Keys[0].ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_site";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "SiteName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");

        sqlCmd = Method.GetSqlCmd(sp_Site, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvSite.EditIndex = -1;
        gvSite.DataBind();

        e.Cancel = true;
    }

    protected void lbtnAddSite_Click(object sender, EventArgs e)
    {
        fvSite.ChangeMode(FormViewMode.Insert);
        fvSite.Visible = true;
    }


    protected void fvSite_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
        {
            ((FormView)sender).Visible = false;
        }
        e.Cancel = true;
    }
    protected void fvSite_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        bool IsInUse = true;
        string SiteID = "99999999";
        string vchCmd = "Add";
        string vchObjectName = "m_site";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";

        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "SiteName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Site, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvSite.Visible = false;
        gvSite.DataBind();

        e.Cancel = true;
    }

    protected void lbtnAddSiteLine_Click(object sender, EventArgs e)
    {
        fvSiteLine.ChangeMode(FormViewMode.Insert);
        fvSiteLine.Visible = true;
    }

    protected void fvSiteLine_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
        {
            ((FormView)sender).Visible = false;
        }
        e.Cancel = true;
    }
    protected void fvSiteLine_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        bool IsInUse = true;
        string LineID = "99999999";
        string vchCmd = "Add";
        string vchObjectName = "m_siteLine";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        string SiteID = gvSite.SelectedDataKey[0].ToString();

        vchSet.Append(Method.BuildXML(LineID, "LineID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "LineName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Site, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvSiteLine.Visible = false;
        gvSiteLine.DataBind();

        e.Cancel = true;
    }

    protected void gvSiteLine_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string LineID = e.Keys[0].ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_siteline";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        string SiteID = gvSite.SelectedDataKey[0].ToString();
        vchSet.Append(Method.BuildXML(LineID, "LineID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "LineName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");

        sqlCmd = Method.GetSqlCmd(sp_Site, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvSiteLine.EditIndex = -1;
        gvSiteLine.DataBind();

        e.Cancel = true;
    }

    protected void lbtnAddSiteShift_Click(object sender, EventArgs e)
    {
        fvSiteShift.ChangeMode(FormViewMode.Insert);
        fvSiteShift.Visible = true;
    }

    protected void fvSiteShift_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
        {
            ((FormView)sender).Visible = false;
        }
            e.Cancel = true;
    }
    protected void fvSiteShift_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        bool IsInUse = true;
        string ShiftID = "99999999";
        string vchCmd = "Add";
        string vchObjectName = "m_siteShift";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        string SiteID = gvSite.SelectedDataKey[0].ToString();

        vchSet.Append(Method.BuildXML(ShiftID, "ShiftID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(e.Values[0].ToString(), "ShiftName"));
        vchSet.Append(Method.BuildXML(e.Values[1].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");
        sqlCmd = Method.GetSqlCmd(sp_Site, vchCmd, vchObjectName, vchSet.ToString());

        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvSiteShift.Visible = false;
        gvSiteShift.DataBind();

        e.Cancel = true;
    }

    protected void gvSiteShift_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string ShiftID = e.Keys[0].ToString();
        string vchCmd = "UPDATE";
        string vchObjectName = "m_siteShift";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        string SiteID = gvSite.SelectedDataKey[0].ToString();
        vchSet.Append(Method.BuildXML(ShiftID, "ShiftID"));
        vchSet.Append(Method.BuildXML(SiteID, "SiteID"));
        vchSet.Append(Method.BuildXML(e.NewValues[0].ToString(), "ShiftName"));
        vchSet.Append(Method.BuildXML(e.NewValues[1].ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(e.NewValues[2].ToString(), "Rank"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        vchSet = vchSet.Replace("'", "''");

        sqlCmd = Method.GetSqlCmd(sp_Site, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvSiteShift.EditIndex = -1;
        gvSiteShift.DataBind();

        e.Cancel = true;
    }
    protected void gvSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvSiteLine.DataBind();
        gvSiteShift.DataBind();
    }
}
