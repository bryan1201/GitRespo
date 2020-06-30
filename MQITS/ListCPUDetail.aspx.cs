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

public partial class ListCPUDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        const string sp_NPISummary = "sp_NPISummary";
        string Customer = Request.QueryString["Customer"].ToString();
        string Phase = Request.QueryString["Phase"].ToString();
        string Site = Request.QueryString["Site"].ToString();
        string Project = Request.QueryString["Project"].ToString();
        //string Liability = Request.QueryString["Liability"].ToString();
        //string Status = Request.QueryString["Status"].ToString();

        if (!IsPostBack)
        {
            StringBuilder vchSet = new StringBuilder();
            string sqlCmd = "";
            vchSet.Append(Method.BuildXML(Customer, "Customer"));
            vchSet.Append(Method.BuildXML(Site, "Site"));
            vchSet.Append(Method.BuildXML(Phase, "Phase"));
            vchSet.Append(Method.BuildXML(Project, "Project"));

            if (Request.QueryString["Material"] != null)
            {
                string Material = Request.QueryString["Material"].ToString();
                vchSet.Append(Method.BuildXML(Material, "Material"));
            }
            if (Request.QueryString["Check"] != null)
            {
                string Check = Request.QueryString["Check"].ToString();
                vchSet.Append(Method.BuildXML(Check, "Check"));
            }
            if (Request.QueryString["Status"] != null)
            {
                vchSet.Append(Method.BuildXML(Request.QueryString["Status"], "Status"));
            }
            else
            {
                vchSet.Append(Method.BuildXML("", "Status"));
            }

            sqlCmd = Method.GetSqlCmd(sp_NPISummary, "QUERY", "LIABILITY", vchSet.ToString());
            SqlDSLiability.SelectCommand = sqlCmd;
            SqlDSLiability.DataBind();
            rptDetail.LocalReport.Refresh();


            //vchSet.Append(Method.BuildXML(Liability, "Liability"));
            //vchSet.Append(Method.BuildXML(Status, "Status"));
            sqlCmd = Method.GetSqlCmd(sp_NPISummary, "QUERY", "LISTCPUDETAIL", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
            if (ds.Tables[0].Rows.Count.ToString() == "0")
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
