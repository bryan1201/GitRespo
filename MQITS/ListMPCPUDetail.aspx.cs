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

public partial class ListMPCPUDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        const string sp_MPSummary = "sp_MPSummary";
        string Customer = Request.QueryString["Customer"].ToString();
        string Site = Request.QueryString["Site"].ToString();
        string Project = Request.QueryString["Project"].ToString();
        string Status = Request.QueryString["Status"].ToString();
        string MType = Request.QueryString["MType"].ToString();
        string StartTime = Request.QueryString["StartTime"].ToString();
        string EndTime = Request.QueryString["EndTime"].ToString();

        if (!IsPostBack)
        {
            StringBuilder vchSet = new StringBuilder();
            string sqlCmd = "";
            vchSet.Append(Method.BuildXML(Customer, "Customer"));
            vchSet.Append(Method.BuildXML(Site, "Site"));
            vchSet.Append(Method.BuildXML(Project, "Project"));
            vchSet.Append(Method.BuildXML(MType, "MType"));
            vchSet.Append(Method.BuildXML(StartTime, "StartTime"));
            vchSet.Append(Method.BuildXML(EndTime, "EndTime"));

            if (Request.QueryString["Material"] != null)
            {
                string Material = Request.QueryString["Material"].ToString();
                vchSet.Append(Method.BuildXML(Material, "Material"));
            }

            if (Request.QueryString["Status"]!="")
            {
                vchSet.Append(Method.BuildXML(Status, "Status"));
            }

            if (Request.QueryString["Station"] != null)
            {
                vchSet.Append(Method.BuildXML(Request.QueryString["Station"], "Station"));
            }

            sqlCmd = Method.GetSqlCmd(sp_MPSummary, "QUERY", "LIABILITY", vchSet.ToString());
            SqlDSLiability.SelectCommand = sqlCmd;
            SqlDSLiability.DataBind();
            rptDetail.LocalReport.Refresh();

            sqlCmd = Method.GetSqlCmd(sp_MPSummary, "QUERY", "LISTCPUDETAIL", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
            if (ds.Tables[0].Rows.Count.ToString() =="0")
            {
                Response.Write("<script language='javascript'>alert('No data List')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
            SqlDSCPUListDetail.SelectCommand = sqlCmd;
            SqlDSCPUListDetail.DataBind();
            rptDetail.LocalReport.Refresh();

        }
    }
}
