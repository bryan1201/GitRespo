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
using System.Data.OleDb;
using System.IO;

public partial class QueryIssue : System.Web.UI.UserControl
{
    const string sp_NPIIssue = "sp_NPIIssue";
    const string sp_ListNPIIssue = "sp_ListNPIIssue";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserID.Text = Method.GetCurrentUserId(Page);
            if (Request.QueryString["IssueID"] != null)
            {
                lblIssueID.Text = Request.QueryString["IssueID"].ToString();
            }
            else
            {
                btnConfirm.Visible = false;
                btnConfirm1.Visible = false;
                btnExport.Visible = true;
                if (Request.QueryString["Customer"] != null &&
                    Request.QueryString["Site"]!= "" &&
                    Request.QueryString["Phase"]!= null &&
                    Request.QueryString["Material"] != null &&
                    Request.QueryString["Liability"] != null &&
                    Request.QueryString["Status"] != null &&
                    Request.QueryString["Project"] != null)
                {
                    ddlCustomer.DataBind();
                    ddlCustomer.SelectedValue = Request.QueryString["Customer"].ToString();
                    ddlSite.DataBind();
                    ddlSite.SelectedValue = Request.QueryString["Site"].ToString();
                    ddlProject.DataBind();
                    ddlProject.SelectedValue = Request.QueryString["Project"].ToString();
                    ddlPhase.DataBind();
                    ddlPhase.SelectedValue = Request.QueryString["Phase"].ToString();
                    ddlMaterial.DataBind();
                    ddlMaterial.SelectedValue = Request.QueryString["Material"].ToString();
                    ddlLiability.DataBind();
                    ddlLiability.SelectedValue = Request.QueryString["Liability"].ToString();
                    ddlStatus.DataBind();
                    ddlStatus.SelectedValue = Request.QueryString["Status"].ToString();   
                    
                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
                    vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
                    vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
                    vchSet.Append(Method.BuildXML(ddlPhase.SelectedValue, "Phase"));
                    vchSet.Append(Method.BuildXML(ddlMaterial.SelectedValue, "MaterialType"));
                    vchSet.Append(Method.BuildXML(ddlLiability.SelectedValue, "Liability"));
                    vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
                    sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTSUMMARY", vchSet.ToString());
                    DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
                    gvPCADetail.DataSource = ds.Tables[0];
                    gvPCADetail.DataBind();
                    if (Request.QueryString["IssueID"] == null)
                    {
                        for (int i = 0; i <= gvPCADetail.Rows.Count - 1; i++)
                        {
                            ((CheckBox)gvPCADetail.Rows[i].FindControl("cbcheck")).Visible = false;
                        }
                    }
                }
            }
        }
    }

    protected string fn_GetMType(string Project, string ProjectMaterial)
    {
        string result = "";

        string vchCmd = "SELECT";
        string vchObjectName = "fn_GetProjectMaterialType";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(Project, "Project"));
        vchSet.Append(Method.BuildXML(ProjectMaterial, "ProjectMaterial"));
        string sqlCmd = Method.GetSqlCmd(sp_NPIIssue, vchCmd, vchObjectName, vchSet.ToString());
        DataSet dsRslt = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        DataRow drRslt;
        if (dsRslt.Tables.Count > 0)
            if (dsRslt.Tables[0].Rows.Count > 0)
            {
                drRslt = dsRslt.Tables[0].Rows[0];
                result = drRslt[0].ToString();
            }
        return result;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string MType = "";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(ddlModel.SelectedValue, "Module"));
        vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
        vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(ddlPhase.SelectedValue, "Phase"));
        vchSet.Append(Method.BuildXML(ddlMaterial.SelectedValue, "MaterialType"));
        vchSet.Append(Method.BuildXML(ddlStation.SelectedValue, "Station"));
        vchSet.Append(Method.BuildXML(ddlIssueOwner.SelectedValue, "IssueOwner"));
        vchSet.Append(Method.BuildXML(ddlLiability.SelectedValue, "Liability"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(ddlPriority.SelectedValue, "Priority"));
        vchSet.Append(Method.BuildXML(txtLocation.Text.Trim(), "Location"));
        vchSet.Append(Method.BuildXML(txtDefectComponent.Text.Trim(), "DefectPartNo"));
        vchSet.Append(Method.BuildXML(txtDefectSymptom.Text.Trim(), "DefectSymptom"));
        vchSet.Append(Method.BuildXML(txtFaultyCommodityPN.Text.Trim(), "FaultyCommodityPN"));
        vchSet.Append(Method.BuildXML(txtFaultyCommodity.Text.Trim(), "FaultyCommodity"));
        vchSet.Append(Method.BuildXML(ddlRepeat.SelectedValue, "RepeatDefect"));
        if (Request.QueryString["IssueID"] != null)
            vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        vchSet = vchSet.Replace(@"'", @"`");


        if (ddlProject.SelectedValue != "" && ddlMaterial.SelectedValue != "")
            MType = fn_GetMType(ddlProject.SelectedValue, ddlMaterial.SelectedValue);


        if (ddlMaterial.SelectedValue == "")//沒有選
        {
            string sqlCmdPCA = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTPCANPIISSUE", vchSet.ToString());
            DataSet dsPCA = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdPCA.ToString());
            gvPCADetail.DataSource = dsPCA.Tables[0];
            gvPCADetail.DataBind();
            gvPCADetail.Visible = true;

            string sqlCmdCPU = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTCPUNPIISSUE", vchSet.ToString());
            DataSet dsCPU = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdCPU.ToString());
            gvCPUDetil.DataSource = dsCPU.Tables[0];
            gvCPUDetil.DataBind();
            gvCPUDetil.Visible = true;
        }
        else if (MType != "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTNPIISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            gvPCADetail.DataSource = ds.Tables[0];
            gvPCADetail.DataBind();
            gvPCADetail.Visible = true;
            gvCPUDetil.Visible = false;
        }
        else if (MType == "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTNPIISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            gvCPUDetil.DataSource = ds.Tables[0];
            gvCPUDetil.DataBind();
            gvPCADetail.Visible = false;
            gvCPUDetil.Visible = true;
        }

         if (Request.QueryString["IssueID"] == null)
        {
            for (int i = 0; i <= gvPCADetail.Rows.Count - 1; i++)
            {
                ((CheckBox)gvPCADetail.Rows[i].FindControl("cbcheck")).Visible = false;
            }
            for (int i = 0; i <= gvCPUDetil.Rows.Count - 1; i++)
            {
                ((CheckBox)gvCPUDetil.Rows[i].FindControl("cbcheck")).Visible = false;
            }
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        CheckRelatedIssue();
    }
    protected void btnConfirm1_Click(object sender, EventArgs e)
    {
        CheckRelatedIssue();
    }
    protected void CheckRelatedIssue()
    {
        int count = 0;
        foreach (GridViewRow gvr in gvPCADetail.Rows)
        {
            string RelatedIssueID = "";
            CheckBox cb = ((CheckBox)gvr.FindControl("cbcheck"));

            if (cb.Checked)
            {
                RelatedIssueID = ((HiddenField)gvr.FindControl("hfIssueID")).Value;
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(RelatedIssueID, "RelatedIssueID"));
                vchSet.Append(Method.BuildXML(txtUserID.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "SAVE", "RELATEDISSUE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

                count++;
            }
        }

        foreach (GridViewRow gvr in gvCPUDetil.Rows)
        {
            string RelatedIssueID = "";
            CheckBox cb = ((CheckBox)gvr.FindControl("cbcheck"));

            if (cb.Checked)
            {
                RelatedIssueID = ((HiddenField)gvr.FindControl("hfIssueID")).Value;
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(RelatedIssueID, "RelatedIssueID"));
                vchSet.Append(Method.BuildXML(txtUserID.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "SAVE", "RELATEDISSUE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

                count++;
            }
        }


        if (count > 0)
        {
            //Method.MessageOut(Page, "Save Sucessfully");
            this.Response.Write("<script>window.opener.location.href=window.opener.location.href;window.close();</script>");
            this.Response.Write("<script>window.opener.location.replace(window.opener.document.referrer);window.close();</script>");
            //this.Response.Write("<script>window.opener.location.href='NPIIssue.aspx?IssueID="+lblIssueID.Text+"';</script>");

            
        }
        else
        {
            Method.MessageOut(Page, "Please check any Item!");
        }
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProject.SelectedValue == "")
            ddlPhase.Items.Clear();
    }

    private string Proacessid()
    {
        Random ran = new Random();
        string strRand = ran.Next(1, 9000).ToString();
        string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + strRand;	//取得年月日時分秒ex:060125093837
        return FileName;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        //匯出檔案
        if (!Directory.Exists(Constant.S_FileRoot + txtUserID.Text))
        {
            Directory.CreateDirectory(Constant.S_FileRoot + txtUserID.Text);
        }
        
        string ExportID = Proacessid();


        string MType = "";
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(ddlModel.SelectedValue, "Module"));
        vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
        vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(ddlPhase.SelectedValue, "Phase"));
        vchSet.Append(Method.BuildXML(ddlMaterial.SelectedValue, "MaterialType"));
        vchSet.Append(Method.BuildXML(ddlStation.SelectedValue, "Station"));
        vchSet.Append(Method.BuildXML(ddlIssueOwner.SelectedValue, "IssueOwner"));
        vchSet.Append(Method.BuildXML(ddlLiability.SelectedValue, "Liability"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(ddlPriority.SelectedValue, "Priority"));
        vchSet.Append(Method.BuildXML(txtLocation.Text.Trim(), "Location"));
        vchSet.Append(Method.BuildXML(txtDefectComponent.Text.Trim(), "DefectPartNo"));
        vchSet.Append(Method.BuildXML(txtDefectSymptom.Text.Trim(), "DefectSymptom"));
        vchSet.Append(Method.BuildXML(txtFaultyCommodityPN.Text.Trim(), "FaultyCommodityPN"));
        vchSet.Append(Method.BuildXML(txtFaultyCommodity.Text.Trim(), "FaultyCommodity"));
        vchSet.Append(Method.BuildXML(ddlRepeat.SelectedValue.ToString(), "RepeatDefect"));
        if (Request.QueryString["IssueID"] != null)
            vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        vchSet = vchSet.Replace(@"'", @"`");

        if (ddlProject.SelectedValue != "" && ddlMaterial.SelectedValue != "")
            MType = fn_GetMType(ddlProject.SelectedValue, ddlMaterial.SelectedValue);


        OleDbConnection ocn = new OleDbConnection(Constant.OLEDBMQITSConnectionString);
      
        if (ddlMaterial.SelectedValue == "")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTPCANPIISSUE", vchSet.ToString());
            DataSet dsPCA = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());

            OleDbCommand odc = new OleDbCommand(sqlCmd, ocn);
            OleDbDataReader odr;

            ocn.Open();
            odr = odc.ExecuteReader();

            gvPCADetail.DataSource = dsPCA.Tables[0];
            gvPCADetail.DataBind();
            gvPCADetail.Visible = true;
            
            StringBuilder sbrHTML = new StringBuilder();
            sbrHTML.Append("<TABLE Border=1 ID='Table1'>");
            sbrHTML.Append("<TR><TH bgcolor=#cccc66>IssueID</TH><TH bgcolor=#cccc66>Project</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Phase</TH><TH bgcolor=#cccc66>PCA P/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Station</TH><TH bgcolor=#cccc66>Issue date</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>S/N</TH><TH bgcolor=#cccc66>Defect Symptom</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Location</TH><TH bgcolor=#cccc66>Defective Component P/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Package Type</TH><TH bgcolor=#cccc66>Root cause</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Corrective Action</TH><TH bgcolor=#cccc66>PIC</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Liability</TH><TH bgcolor=#cccc66>Status</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Pending Day</TH><TH bgcolor=#cccc66>Priority</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Failure Rate</TH><TH bgcolor=#cccc66>Repeat Defect?</TH>");
            
            while (odr.Read())
            {
                sbrHTML.Append("<TR><TD>" + odr.GetValue(1).ToString() + "</TD><TD>" + odr.GetValue(2).ToString() +
                    "</TD><TD>" + odr.GetValue(3).ToString() + "</TD><TD>" + odr.GetValue(4).ToString() +
                    "</TD><TD>" + odr.GetValue(5).ToString() + "</TD><TD>" + odr.GetValue(6).ToString() +
                    "</TD><TD>" + odr.GetValue(7).ToString() + "</TD><TD>" + odr.GetValue(8).ToString() +
                    "</TD><TD>" + odr.GetValue(9).ToString() + "</TD><TD>" + odr.GetValue(10).ToString() +
                    "</TD><TD>" + odr.GetValue(11).ToString() + "</TD><TD>" + odr.GetValue(12).ToString() +
                    "</TD><TD>" + odr.GetValue(13).ToString() + "</TD><TD>" + odr.GetValue(14).ToString() +
                    "</TD><TD>" + odr.GetValue(15).ToString() + "</TD><TD>" + odr.GetValue(16).ToString() +
                    "</TD><TD>" + odr.GetValue(17).ToString() + "</TD><TD>" + odr.GetValue(18).ToString() +
                    "</TD><TD>" + odr.GetValue(19).ToString() + "</TD><TD>" + odr.GetValue(20).ToString() +
                    "</TD></TR>");
            }
            

            sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTCPUNPIISSUE", vchSet.ToString());
            DataSet dsCPU = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            OleDbCommand odc1 = new OleDbCommand(sqlCmd, ocn);
            OleDbDataReader odr1;

            odr1 = odc1.ExecuteReader();

            gvCPUDetil.DataSource = dsCPU.Tables[0];
            gvCPUDetil.DataBind();
            gvCPUDetil.Visible = true;

            if (dsCPU.Tables[0].Rows.Count > 0)
            {
                sbrHTML.Append("<TR><TH bgcolor=#cccc66>IssueID</TH><TH bgcolor=#cccc66>Project</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Phase</TH><TH bgcolor=#cccc66>CPU</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Station</TH><TH bgcolor=#cccc66>Issue date</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>S/N</TH><TH bgcolor=#cccc66>Defect Symptom</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Faulty Commodity</TH><TH bgcolor=#cccc66>Faulty Commodity P/N</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Faulty Commodity S/N</TH><TH bgcolor=#cccc66>Root cause</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Corrective Action</TH><TH bgcolor=#cccc66>PIC</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Liability</TH><TH bgcolor=#cccc66>Status</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Pending Day</TH><TH bgcolor=#cccc66>Priority</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Failure Rate</TH><TH bgcolor=#cccc66>Repeat Defect?</TH>");


                while (odr1.Read())
                {
                    sbrHTML.Append("<TR><TD>" + odr1.GetValue(1).ToString() + "</TD><TD>" + odr1.GetValue(2).ToString() +
                        "</TD><TD>" + odr1.GetValue(3).ToString() + "</TD><TD>" + odr1.GetValue(4).ToString() +
                        "</TD><TD>" + odr1.GetValue(5).ToString() + "</TD><TD>" + odr1.GetValue(6).ToString() +
                        "</TD><TD>" + odr1.GetValue(7).ToString() + "</TD><TD>" + odr1.GetValue(8).ToString() +
                        "</TD><TD>" + odr1.GetValue(9).ToString() + "</TD><TD>" + odr1.GetValue(10).ToString() +
                        "</TD><TD>" + odr1.GetValue(11).ToString() + "</TD><TD>" + odr1.GetValue(12).ToString() +
                        "</TD><TD>" + odr1.GetValue(13).ToString() + "</TD><TD>" + odr1.GetValue(14).ToString() +
                        "</TD><TD>" + odr1.GetValue(15).ToString() + "</TD><TD>" + odr1.GetValue(16).ToString() +
                        "</TD><TD>" + odr1.GetValue(17).ToString() + "</TD><TD>" + odr1.GetValue(18).ToString() +
                        "</TD><TD>" + odr1.GetValue(19).ToString() + "</TD><TD>" + odr1.GetValue(20).ToString() +
                        "</TD></TR>");
                }

            }
            sbrHTML.Append("</TABLE>");

            StreamWriter swXLS = new StreamWriter(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCACPUIssue.xls", false, Encoding.Unicode);


            swXLS.Write(sbrHTML.ToString());
            swXLS.Close();

            if (File.Exists(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCACPUIssue.xls"))
            {
                Method.OpenWindows(Page, Constant.S_WebSite + "UploadFile/" + txtUserID.Text + "/" + ExportID +"PCACPUIssue.xls", "List");
            }
        }
        else if (MType != "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTNPIISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            OleDbCommand odc = new OleDbCommand(sqlCmd, ocn);
            OleDbDataReader odr;

            ocn.Open();
            odr = odc.ExecuteReader();

            gvPCADetail.DataSource = ds.Tables[0];
            gvPCADetail.DataBind();
            gvPCADetail.Visible = true;
            gvCPUDetil.Visible = false;

            StringBuilder sbrHTML = new StringBuilder();
            sbrHTML.Append("<TABLE Border=1 ID='Table1'>");
            sbrHTML.Append("<TR><TH bgcolor=#cccc66>IssueID</TH><TH bgcolor=#cccc66>Project</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Phase</TH><TH bgcolor=#cccc66>PCA P/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Station</TH><TH bgcolor=#cccc66>Issue date</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>S/N</TH><TH bgcolor=#cccc66>Defect Symptom</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Location</TH><TH bgcolor=#cccc66>Defective Component P/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Package Type</TH><TH bgcolor=#cccc66>Root cause</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Corrective Action</TH><TH bgcolor=#cccc66>PIC</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Liability</TH><TH bgcolor=#cccc66>Status</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Pending Day</TH><TH bgcolor=#cccc66>Priority</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Failure Rate</TH><TH bgcolor=#cccc66>Repeat Defect?</TH>");


            while (odr.Read())
            {
                sbrHTML.Append("<TR><TD>" + odr.GetValue(1).ToString() + "</TD><TD>" + odr.GetValue(2).ToString() +
                    "</TD><TD>" + odr.GetValue(3).ToString() + "</TD><TD>" + odr.GetValue(4).ToString() +
                    "</TD><TD>" + odr.GetValue(5).ToString() + "</TD><TD>" + odr.GetValue(6).ToString() +
                    "</TD><TD>" + odr.GetValue(7).ToString() + "</TD><TD>" + odr.GetValue(8).ToString() +
                    "</TD><TD>" + odr.GetValue(9).ToString() + "</TD><TD>" + odr.GetValue(10).ToString() +
                    "</TD><TD>" + odr.GetValue(11).ToString() + "</TD><TD>" + odr.GetValue(15).ToString() +
                    "</TD><TD>" + odr.GetValue(16).ToString() + "</TD><TD>" + odr.GetValue(17).ToString() +
                    "</TD><TD>" + odr.GetValue(18).ToString() + "</TD><TD>" + odr.GetValue(19).ToString() +
                    "</TD><TD>" + odr.GetValue(20).ToString() + "</TD><TD>" + odr.GetValue(21).ToString() +
                    "</TD><TD>" + odr.GetValue(22).ToString() + "</TD><TD>" + odr.GetValue(23).ToString() +
                    "</TD></TR>");
            }

            sbrHTML.Append("</TABLE>");

            StreamWriter swXLS = new StreamWriter(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCAIssue.xls", false, Encoding.Unicode);


            swXLS.Write(sbrHTML.ToString());
            swXLS.Close();



            if (File.Exists(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCAIssue.xls"))
            {
                Method.OpenWindows(Page, Constant.S_WebSite + "UploadFile/" + txtUserID.Text + "/" + ExportID +"PCAIssue.xls", "List");
            }
        }
        else if (MType == "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListNPIIssue, "QUERY", "LISTNPIISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            OleDbCommand odc = new OleDbCommand(sqlCmd, ocn);
            OleDbDataReader odr;

            ocn.Open();
            odr = odc.ExecuteReader();
            
            gvCPUDetil.DataSource = ds.Tables[0];
            gvCPUDetil.DataBind();
            gvPCADetail.Visible = false;
            gvCPUDetil.Visible = true;

            StringBuilder sbrHTML = new StringBuilder();
            sbrHTML.Append("<TABLE Border=1 ID='Table1'>");
            sbrHTML.Append("<TR><TH bgcolor=#cccc66>IssueID</TH><TH bgcolor=#cccc66>Project</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Phase</TH><TH bgcolor=#cccc66>CPU</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Station</TH><TH bgcolor=#cccc66>Issue date</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>S/N</TH><TH bgcolor=#cccc66>Defect Symptom</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Faulty Commodity</TH><TH bgcolor=#cccc66>Faulty Commodity P/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Faulty Commodity S/N</TH><TH bgcolor=#cccc66>Root cause</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Corrective Action</TH><TH bgcolor=#cccc66>PIC</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Liability</TH><TH bgcolor=#cccc66>Status</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Pending Day</TH><TH bgcolor=#cccc66>Priority</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Failure Rate</TH><TH bgcolor=#cccc66>Repeat Defect?</TH>");


            while (odr.Read())
            {
                sbrHTML.Append("<TR><TD>" + odr.GetValue(1).ToString() + "</TD><TD>" + odr.GetValue(2).ToString() +
                    "</TD><TD>" + odr.GetValue(3).ToString() + "</TD><TD>" + odr.GetValue(4).ToString() +
                    "</TD><TD>" + odr.GetValue(5).ToString() + "</TD><TD>" + odr.GetValue(6).ToString() +
                    "</TD><TD>" + odr.GetValue(7).ToString() + "</TD><TD>" + odr.GetValue(8).ToString() +
                    "</TD><TD>" + odr.GetValue(12).ToString() + "</TD><TD>" + odr.GetValue(13).ToString() +
                    "</TD><TD>" + odr.GetValue(14).ToString() + "</TD><TD>" + odr.GetValue(15).ToString() +
                    "</TD><TD>" + odr.GetValue(16).ToString() + "</TD><TD>" + odr.GetValue(17).ToString() +
                    "</TD><TD>" + odr.GetValue(18).ToString() + "</TD><TD>" + odr.GetValue(19).ToString() +
                    "</TD><TD>" + odr.GetValue(20).ToString() + "</TD><TD>" + odr.GetValue(21).ToString() +
                    "</TD><TD>" + odr.GetValue(22).ToString() + "</TD><TD>" + odr.GetValue(23).ToString() +
                    "</TD></TR>");
            }

            sbrHTML.Append("</TABLE>");

            StreamWriter swXLS = new StreamWriter(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "CPUIssue.xls", false, Encoding.Unicode);


            swXLS.Write(sbrHTML.ToString());
            swXLS.Close();



            if (File.Exists(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "CPUIssue.xls"))
            {
                Method.OpenWindows(Page, Constant.S_WebSite + "UploadFile/" + txtUserID.Text + "/" + ExportID +"CPUIssue.xls", "List");
            }

        }

        if (Request.QueryString["IssueID"] == null)
        {
            for (int i = 0; i <= gvPCADetail.Rows.Count - 1; i++)
            {
                ((CheckBox)gvPCADetail.Rows[i].FindControl("cbcheck")).Visible = false;
            }
            for (int i = 0; i <= gvCPUDetil.Rows.Count - 1; i++)
            {
                ((CheckBox)gvCPUDetil.Rows[i].FindControl("cbcheck")).Visible = false;
            }
        }
       
    }
}
