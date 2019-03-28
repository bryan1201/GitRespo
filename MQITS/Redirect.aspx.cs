using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Redirect : System.Web.UI.Page
{
    const string sp_ARForSQMP = "sp_ARForSQMP";
    protected void Page_Load(object sender, EventArgs e)
    {
        string IssueID = Request.QueryString["IssueID"];
        string ActionID = Request.QueryString["ActionID"];
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(IssueID, "IssueID"));
        vchSet.Append(Method.BuildXML(ActionID, "ActionID"));
        string sqlCmd = Method.GetSqlCmd(sp_ARForSQMP, "REDIRECT", "WEBSITE", vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        Response.Redirect(ds.Tables[0].Rows[0]["WebSite"].ToString());
    }
}
