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

public partial class RMAQuery : System.Web.UI.Page
{
    const string sp_NPIIssue = "sp_NPIIssue";
    const string sp_ListRMAIssue = "sp_ListRMAIssue";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserID.Text = Method.GetCurrentUserId(Page);
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

    public static bool InputCalCheck(string str)
    {
        bool check = false;
        try
        {
            System.Text.RegularExpressions.Regex reg1 =
                new System.Text.RegularExpressions.Regex("^(\\d{4})/(0\\d{1}|1[0-2])/(0\\d{1}|[12]\\d{1}|3[01])");
            if (reg1.IsMatch(str))
            {
                DateTime dt = DateTime.Parse(str);
                check = true;
            }
        }
        catch
        {
            check = false;
        }
        return check;
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string MType = "";
        bool Scheck = false;
        bool Echeck = false;
        if (txtStart.Text.Trim() != "")
        {
            Scheck = InputCalCheck(txtStart.Text);
            if (Scheck == false)
            {
                Method.MessageOut(Page, "its format is yyyy/MM/dd");
                return;
            }
        }
        if (txtEnd.Text.Trim() != "")
        {
            Echeck = InputCalCheck(txtEnd.Text);
            if (Echeck == false)
            {
                Method.MessageOut(Page, "its format is yyyy/MM/dd");
                return;
            }
        }

        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(ddlModel.SelectedValue, "Module"));
        vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
        vchSet.Append(Method.BuildXML(ddlMaterial.SelectedValue, "MaterialType"));
        vchSet.Append(Method.BuildXML(ddlStation.SelectedValue, "Station"));
        vchSet.Append(Method.BuildXML(ddlIssueOwner.SelectedValue, "IssueOwner"));
        vchSet.Append(Method.BuildXML(ddlLiability.SelectedValue, "Liability"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(ddlPriority.SelectedValue, "Priority"));
        vchSet.Append(Method.BuildXML(ddlRepeat.SelectedValue, "RepeatDefect"));
        vchSet.Append(Method.BuildXML(ddlReturnSite.SelectedValue, "ReturnSite"));
        vchSet.Append(Method.BuildXML(ddlReturnType.SelectedValue, "ReturnType"));
        vchSet.Append(Method.BuildXML(txtStart.Text.Trim(), "StartTime"));
        vchSet.Append(Method.BuildXML(txtEnd.Text.Trim(), "EndTime"));
        vchSet.Append(Method.BuildXML(txtLocation.Text.Trim(), "Location"));
        vchSet.Append(Method.BuildXML(txtDefectSymptom.Text.Trim(), "DefectSymptom"));
        vchSet.Append(Method.BuildXML(txtOriginalDefect.Text.Trim(), "OriginalDefect"));
        vchSet.Append(Method.BuildXML(txtFailureSymptom.Text.Trim(), "FailureSymptom"));
        
        vchSet = vchSet.Replace(@"'", @"`");


        if (ddlProject.SelectedValue != "" && ddlMaterial.SelectedValue != "")
            MType = fn_GetMType(ddlProject.SelectedValue, ddlMaterial.SelectedValue);


        if (ddlMaterial.SelectedValue == "")
        {
            string sqlCmdPCA = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTPCARMAISSUE", vchSet.ToString());
            DataSet dsPCA = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdPCA.ToString());
            gvPCADetail.DataSource = dsPCA.Tables[0];
            gvPCADetail.DataBind();
            gvPCADetail.Visible = true;

            string sqlCmdCPU = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTCPURMAISSUE", vchSet.ToString());
            DataSet dsCPU = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdCPU.ToString());
            gvCPUDetil.DataSource = dsCPU.Tables[0];
            gvCPUDetil.DataBind();
            gvCPUDetil.Visible = true;
        }
        else if (MType != "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTRMAISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            gvPCADetail.DataSource = ds.Tables[0];
            gvPCADetail.DataBind();
            gvPCADetail.Visible = true;
            gvCPUDetil.Visible = false;
        }
        else if (MType == "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTRMAISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd.ToString());
            gvCPUDetil.DataSource = ds.Tables[0];
            gvCPUDetil.DataBind();
            gvPCADetail.Visible = false;
            gvCPUDetil.Visible = true;
        }

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
        if (!Directory.Exists(Constant.S_FileRoot + txtUserID.Text))
        {
            Directory.CreateDirectory(Constant.S_FileRoot + txtUserID.Text);
        }
        //string ExportID = System.Guid.NewGuid().ToString();
        string ExportID = Proacessid();
        
        string MType = "";
        bool Scheck = false;
        bool Echeck = false;
        if (txtStart.Text.Trim() != "")
        {
            Scheck = InputCalCheck(txtStart.Text);
            if (Scheck == false)
            {
                Method.MessageOut(Page, "its format is yyyy/MM/dd");
                return;
            }
        }
        if (txtEnd.Text.Trim() != "")
        {
            Echeck = InputCalCheck(txtEnd.Text);
            if (Echeck == false)
            {
                Method.MessageOut(Page, "its format is yyyy/MM/dd");
                return;
            }
        }

        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(ddlModel.SelectedValue, "Module"));
        vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
        vchSet.Append(Method.BuildXML(ddlMaterial.SelectedValue, "MaterialType"));
        vchSet.Append(Method.BuildXML(ddlStation.SelectedValue, "Station"));
        vchSet.Append(Method.BuildXML(ddlIssueOwner.SelectedValue, "IssueOwner"));
        vchSet.Append(Method.BuildXML(ddlLiability.SelectedValue, "Liability"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(ddlPriority.SelectedValue, "Priority"));
        vchSet.Append(Method.BuildXML(ddlRepeat.SelectedValue, "RepeatDefect"));
        vchSet.Append(Method.BuildXML(ddlReturnSite.SelectedValue, "ReturnSite"));
        vchSet.Append(Method.BuildXML(ddlReturnType.SelectedValue, "ReturnType"));
        vchSet.Append(Method.BuildXML(txtStart.Text.Trim(), "StartTime"));
        vchSet.Append(Method.BuildXML(txtEnd.Text.Trim(), "EndTime"));
        vchSet.Append(Method.BuildXML(txtLocation.Text.Trim(), "Location"));
        vchSet.Append(Method.BuildXML(txtDefectSymptom.Text.Trim(), "DefectSymptom"));
        vchSet.Append(Method.BuildXML(txtOriginalDefect.Text.Trim(), "OriginalDefect"));
        vchSet.Append(Method.BuildXML(txtFailureSymptom.Text.Trim(), "FailureSymptom"));
        vchSet = vchSet.Replace(@"'", @"`");


        if (ddlProject.SelectedValue != "" && ddlMaterial.SelectedValue != "")
            MType = fn_GetMType(ddlProject.SelectedValue, ddlMaterial.SelectedValue);

        OleDbConnection ocn = new OleDbConnection(Constant.OLEDBMQITSConnectionString);

        if (ddlMaterial.SelectedValue == "")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTPCARMAISSUE", vchSet.ToString());
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
            sbrHTML.Append("<TH bgcolor=#cccc66>PCA P/N</TH><TH bgcolor=#cccc66>Station</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Issue date</TH><TH bgcolor=#cccc66>S/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Defect Symptom</TH><TH bgcolor=#cccc66>Location</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Defective Component P/N</TH><TH bgcolor=#cccc66>Root cause</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Corrective Action</TH><TH bgcolor=#cccc66>PIC</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Liability</TH><TH bgcolor=#cccc66>Status</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>TAT</TH><TH bgcolor=#cccc66>Priority</TH>");
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
                    "</TD></TR>");
            }


            sqlCmd = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTCPURMAISSUE", vchSet.ToString());
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
                sbrHTML.Append("<TH bgcolor=#cccc66>CPU</TH><TH bgcolor=#cccc66>Station</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Issue date</TH><TH bgcolor=#cccc66>S/N</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Defect Symptom</TH><TH bgcolor=#cccc66>Faulty Commodity</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Faulty Commodity P/N</TH><TH bgcolor=#cccc66>Faulty Commodity S/N</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Root cause</TH><TH bgcolor=#cccc66>Corrective Action</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>PIC</TH><TH bgcolor=#cccc66>Liability</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Status</TH><TH bgcolor=#cccc66>TAT</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Priority</TH><TH bgcolor=#cccc66>Failure Rate</TH>");
                sbrHTML.Append("<TH bgcolor=#cccc66>Repeat Defect?</TH>");


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
                        "</TD><TD>" + odr1.GetValue(19).ToString() + 
                        "</TD></TR>");
                }

            }
            sbrHTML.Append("</TABLE>");


            StreamWriter swXLS = new StreamWriter(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCACPUIssue.xls", false, Encoding.Unicode);


            swXLS.Write(sbrHTML.ToString());
            swXLS.Close();

            if (File.Exists(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCACPUIssue.xls"))
            {
                Method.OpenWindows(Page, Constant.S_WebSite + "UploadFile/" + txtUserID.Text + "/" + ExportID + "PCACPUIssue.xls", "List");
            }
        }
        else if (MType != "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTRMAISSUE", vchSet.ToString());
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
            sbrHTML.Append("<TH bgcolor=#cccc66>PCA P/N</TH><TH bgcolor=#cccc66>Station</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Issue date</TH><TH bgcolor=#cccc66>S/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Defect Symptom</TH><TH bgcolor=#cccc66>Location</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Defective Component P/N</TH><TH bgcolor=#cccc66>Root cause</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Corrective Action</TH><TH bgcolor=#cccc66>PIC</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Liability</TH><TH bgcolor=#cccc66>Status</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>TAT</TH><TH bgcolor=#cccc66>Priority</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Failure Rate</TH><TH bgcolor=#cccc66>Repeat Defect?</TH>");


            while (odr.Read())
            {
                sbrHTML.Append("<TR><TD>" + odr.GetValue(1).ToString() + "</TD><TD>" + odr.GetValue(2).ToString() +
                    "</TD><TD>" + odr.GetValue(3).ToString() + "</TD><TD>" + odr.GetValue(4).ToString() +
                    "</TD><TD>" + odr.GetValue(5).ToString() + "</TD><TD>" + odr.GetValue(6).ToString() +
                    "</TD><TD>" + odr.GetValue(7).ToString() + "</TD><TD>" + odr.GetValue(8).ToString() +
                    "</TD><TD>" + odr.GetValue(9).ToString() + "</TD><TD>" + odr.GetValue(13).ToString() +
                    "</TD><TD>" + odr.GetValue(14).ToString() + "</TD><TD>" + odr.GetValue(15).ToString() +
                    "</TD><TD>" + odr.GetValue(16).ToString() + "</TD><TD>" + odr.GetValue(17).ToString() +
                    "</TD><TD>" + odr.GetValue(18).ToString() + "</TD><TD>" + odr.GetValue(19).ToString() +
                    "</TD><TD>" + odr.GetValue(20).ToString() + "</TD><TD>" + odr.GetValue(21).ToString() +
                    "</TD></TR>");
            }

            sbrHTML.Append("</TABLE>");

            StreamWriter swXLS = new StreamWriter(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCAIssue.xls", false, Encoding.Unicode);


            swXLS.Write(sbrHTML.ToString());
            swXLS.Close();

            if (File.Exists(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "PCAIssue.xls"))
            {
                Method.OpenWindows(Page, Constant.S_WebSite + "UploadFile/" + txtUserID.Text + "/" + ExportID+ "PCAIssue.xls", "List");
            }
        }
        else if (MType == "CPU")
        {
            sqlCmd = Method.GetSqlCmd(sp_ListRMAIssue, "QUERY", "LISTRMAISSUE", vchSet.ToString());
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
            sbrHTML.Append("<TH bgcolor=#cccc66>CPU</TH><TH bgcolor=#cccc66>Station</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Issue date</TH><TH bgcolor=#cccc66>S/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Defect Symptom</TH><TH bgcolor=#cccc66>Faulty Commodity</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Faulty Commodity P/N</TH><TH bgcolor=#cccc66>Faulty Commodity S/N</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Root cause</TH><TH bgcolor=#cccc66>Corrective Action</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>PIC</TH><TH bgcolor=#cccc66>Liability</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Status</TH><TH bgcolor=#cccc66>TAT</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Priority</TH><TH bgcolor=#cccc66>Failure Rate</TH>");
            sbrHTML.Append("<TH bgcolor=#cccc66>Repeat Defect?</TH>");


            while (odr.Read())
            {
                sbrHTML.Append("<TR><TD>" + odr.GetValue(1).ToString() + "</TD><TD>" + odr.GetValue(2).ToString() +
                        "</TD><TD>" + odr.GetValue(3).ToString() + "</TD><TD>" + odr.GetValue(4).ToString() +
                        "</TD><TD>" + odr.GetValue(5).ToString() + "</TD><TD>" + odr.GetValue(6).ToString() +
                        "</TD><TD>" + odr.GetValue(7).ToString() + "</TD><TD>" + odr.GetValue(10).ToString() +
                        "</TD><TD>" + odr.GetValue(11).ToString() + "</TD><TD>" + odr.GetValue(12).ToString() +
                        "</TD><TD>" + odr.GetValue(13).ToString() + "</TD><TD>" + odr.GetValue(14).ToString() +
                        "</TD><TD>" + odr.GetValue(15).ToString() + "</TD><TD>" + odr.GetValue(16).ToString() +
                        "</TD><TD>" + odr.GetValue(17).ToString() + "</TD><TD>" + odr.GetValue(18).ToString() +
                        "</TD><TD>" + odr.GetValue(19).ToString() + "</TD><TD>" + odr.GetValue(20).ToString() +
                        "</TD><TD>" + odr.GetValue(21).ToString() + 
                        "</TD></TR>");
            }

            sbrHTML.Append("</TABLE>");

            StreamWriter swXLS = new StreamWriter(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "CPUIssue.xls", false, Encoding.Unicode);


            swXLS.Write(sbrHTML.ToString());
            swXLS.Close();



            if (File.Exists(Constant.S_FileRoot + txtUserID.Text + "/" + ExportID + "CPUIssue.xls"))
            {
                Method.OpenWindows(Page, Constant.S_WebSite + "UploadFile/" + txtUserID.Text + "/" + ExportID+"CPUIssue.xls", "List");
            }

        }
    }
}
