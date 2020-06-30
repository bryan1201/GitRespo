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

public partial class MMaintain : System.Web.UI.UserControl
{
    private static string sp_RMAMaintain = "sp_RMAMaintain";
    private static string function = "";
    private static string viewTable = "";
    private static string viewTitle = "";
    private static string mObjectType = "";
    [Personalizable]
    public string UserObjectType
    {
        get
        {
            return mObjectType;
        }
        set
        {
            mObjectType = value;
        }
    }

    public string UserViewTitle
    {
        get
        {
            return viewTitle;
        }
        set
        {
            viewTitle = value;
        }
    }

    public string UserViewTable
    {
        get
        {
            return viewTable;
        }
        set
        {
            viewTable = value;
        }
    }

    public string UserFunction
    {
        get
        {
            return function;
        }
        set
        {
            function = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfUserId.Value = Method.GetCurrentUserId(Page);
            InitMMaintain(function);
        }
    }

    public void InitMMaintain(string mfun)
    {
        divPriority.Visible = false;
        divLiability.Visible = false;
        divIssueStatus.Visible = false;
        divRMAMaintain.Visible = false;
        switch (mfun)
        {
            case "Priority":
                divPriority.Visible = true;
                break;
            case "Liability":
                divLiability.Visible = true;
                break;
            case "IssueStatus":
                divIssueStatus.Visible = true;
                break;
            default:
                divRMAMaintain.Visible = true;
                spanTitle.InnerText = viewTitle;
                BindRMAMaintain();
                break;
        }
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        fvRMAMaintain.Visible = true;
    }
    protected void fvRMAMaintain_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string vchCmd = "AddRMADLL";
        string vchObjectName = "m_maintain";
        bool IsInUse = true;
        string vTitle = e.Values[0].ToString();
        string Rank = e.Values[1].ToString();
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(vTitle, "vTitle"));
        vchSet.Append(Method.BuildXML(Rank, "Rank"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(mObjectType, "ObjectType"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RMAMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvRMAMaintain.Visible = false;
        BindRMAMaintain();
        e.Cancel = true;
    }

    protected void BindRMAMaintain()
    {
        Session["RMAMaintainObjectName"] = "m_maintain";
        Session["RMAMaintainvchSet"] = Method.BuildXML(mObjectType, "ObjectType");
        SqlDSRMAMaintain.DataBind();
        gvRMAMaintain.DataBind();
    }

    protected void gvRMAMaintain_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string vchCmd = "UpdateRMADLL";
        string vchObjectName = "m_maintain";
        bool IsInUse = true;
        string ID = e.Keys[0].ToString();
        string vTitle = e.NewValues[0].ToString();
        bool.TryParse(e.NewValues[1].ToString(), out IsInUse);
        string Rank = e.NewValues[2].ToString();
        StringBuilder vchSet = new StringBuilder();

        vchSet.Append(Method.BuildXML(ID, "ID"));
        vchSet.Append(Method.BuildXML(vTitle, "vTitle"));
        vchSet.Append(Method.BuildXML(Rank, "Rank"));
        vchSet.Append(Method.BuildXML(IsInUse.ToString(), "IsInUse"));
        vchSet.Append(Method.BuildXML(mObjectType, "ObjectType"));
        vchSet.Append(Method.BuildXML(hfUserId.Value, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RMAMaintain, vchCmd, vchObjectName, vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        gvRMAMaintain.EditIndex = -1;
        BindRMAMaintain();
        e.Cancel = true;
    }

    protected void fvRMAMaintain_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (e.CancelingEdit == true)
        {
            ((FormView)sender).Visible = false;
        }
        e.Cancel = true;
    }
}
