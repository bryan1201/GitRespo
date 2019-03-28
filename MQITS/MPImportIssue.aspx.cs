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
using System.Data.OleDb;
using System.IO;
using System.Text;

public partial class MPImportIssue : System.Web.UI.Page
{
    const string sp_ImportMPIssue = "sp_ImportMPIssue";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUserId.Text = Method.GetCurrentUserId(Page);
            ddlCustomer.DataBind();
            ddlProject.DataBind();
            ddlSite.DataBind();
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
            string sqlCmd = Method.GetSqlCmd("sp_MPIssue", "CHECK", "PROJECTMEMBER", vchSet.ToString());
            string[] Role = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmd);
            if (Role[0] == "")
            {
                Response.Write("<script language='javascript'>alert('You are not belong to any ProjectOwner/Member')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
            else if (ddlSite.Items.Count > 1)
            {
                Response.Write("<script language='javascript'>alert('You have two Site,Please Contact Owen')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
            else if (ddlProject.Items.Count <= 1)
            {
                Response.Write("<script language='javascript'>alert('Project is in NPI Module')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
        }

    }  
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (ddlCustomer.SelectedValue != "" &&  ddlProject.SelectedValue != ""
                 && fuImport.PostedFile.FileName!="")
        {
            ProcessUpdFile();
        }
        else
        {
            Method.MessageOut(Page, "Please fill out fields");
        }
    }

    protected void ProcessUpdFile()
    {
        string accessid = Proacessid();
        string filepath = Constant.S_FileRoot + "ImportFile\\";
        string filename = FileProcess(filepath,accessid);
        ImportExcel(filepath,filename, "PCA");
        ImportExcel(filepath, filename, "CPU");
        delFile(filepath + '\\' + filename);
    }
    private string Proacessid()
    {
        Random ran = new Random();                           //宣告
        string strRand = ran.Next(1, 9000).ToString();         //取1~8000之間任一亂數
        string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + strRand;	//取得年月日時分秒ex:060125093837
        return FileName;					                //傳回檔名
    }
    public string FileProcess(string filepath, string accessid)
    {
        string backupFile = "";
        string filename = "";
        if (fuImport.PostedFile.FileName != "")
        {
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            filename = accessid + "_" + fuImport.PostedFile.FileName.Substring(fuImport.PostedFile.FileName.LastIndexOf("\\") + 1);
            backupFile = filepath + filename;

            fuImport.PostedFile.SaveAs(backupFile);
        }
        return filename;
    }

    private void ImportExcel(string filepath, string filename, string sheetname)
    {
        OleDbConnection cn = null;
        OleDbDataAdapter da = null;
        try
        {
            DataSet ds = new DataSet();
            string strConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + filename +
                @";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1""";
            string strQuery = "";
            strQuery = @"SELECT * FROM [" + sheetname + "$A:K] ";
            cn = new OleDbConnection(strConnectionString);
            cn.Open();
            OleDbCommand cmdSelect = new OleDbCommand(strQuery, cn);
            da = new OleDbDataAdapter();
            da.SelectCommand = cmdSelect;
            da.Fill(ds, sheetname);//用fill方法將da的查詢結果填入ds,再傳回ds

           SaveImportData(ds,sheetname);//儲存資料進DB
           
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());          
        }
        finally
        {
            da.Dispose();
            cn.Close();
            cn.Dispose();
        }
    }
    private void SaveImportData(DataSet ds, string sheetname)
    {
        int PCACorrectCount = 0;
        int PCAIncorrectCount = 0;
        int CPUCorrectCount = 0;
        int CPUIncorrectCount = 0;

        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                object[] rowData = ds.Tables[0].Rows[i].ItemArray;

                bool flag = false;
                if (sheetname == "PCA")
                {
                    if (rowData[0].ToString().Trim() == "" || rowData[1].ToString().Trim() == "" || rowData[2].ToString().Trim() == ""
                       || rowData[3].ToString().Trim() == "" || rowData[4].ToString().Trim() == "" || rowData[5].ToString().Trim() == ""
                       || rowData[6].ToString().Trim() == "" || rowData[8].ToString().Trim() =="" || rowData[9].ToString().Trim()=="")
                    { }
                    else
                    {
                        flag = true;
                    }
                }
                if (sheetname == "CPU")
                {
                    if (rowData[0].ToString().Trim() == "" || rowData[1].ToString().Trim() == "" || rowData[2].ToString().Trim() == ""
                           || rowData[3].ToString().Trim() == "" || rowData[4].ToString().Trim() == "" || rowData[5].ToString().Trim() == ""
                           || rowData[6].ToString().Trim() == "" || rowData[7].ToString().Trim() == "" 
                           || rowData[9].ToString().Trim()=="" || rowData[10].ToString().Trim()=="")
                    { }                      
                    else
                    {
                        flag = true;
                    }
                }

                StringBuilder vchSet = new StringBuilder();

                vchSet.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
                vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "Site"));
                vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
                vchSet.Append(Method.BuildXML(rowData[0].ToString().Trim(), "MaterialType"));
                                
                //檢查輸入的Material是否為此Project
                StringBuilder vchSetProjectMaterialType = new StringBuilder();
                vchSetProjectMaterialType.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
                vchSetProjectMaterialType.Append(Method.BuildXML(sheetname, "Type"));
                vchSetProjectMaterialType.Append(Method.BuildXML(rowData[0].ToString().Trim(), "MaterialType"));
                string sqlProjetMaterial = Method.GetSqlCmd(sp_ImportMPIssue, "CHECK", "PROJECTMATERIAL", vchSetProjectMaterialType.ToString());
                string[] CheckMaterial=DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlProjetMaterial);
                //檢查輸入的Material是否為此Project

                vchSet.Append(Method.BuildXML(rowData[1].ToString().Trim(), "SerialNo"));
                vchSet.Append(Method.BuildXML(rowData[2].ToString().Trim(), "StationName"));

                //檢查Station是否為此Customer
                StringBuilder vchSetCustomerStation = new StringBuilder();
                vchSetCustomerStation.Append(Method.BuildXML(ddlCustomer.SelectedValue, "Customer"));
                vchSetCustomerStation.Append(Method.BuildXML(rowData[2].ToString().Trim(), "StationName"));
                vchSetCustomerStation.Append(Method.BuildXML(sheetname, "Type"));
                string sqlCustomerStation = Method.GetSqlCmd(sp_ImportMPIssue, "CHECK", "CUSTOMERSTATION", vchSetCustomerStation.ToString());
                string[] CheckStation = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCustomerStation);
                //檢查Station是否為此Customer

                vchSet.Append(Method.BuildXML(rowData[3].ToString().Trim(), "DefectSymptom"));
                bool checktime = false;
                int checkcompare = 0;
                int FailQty = 0;
                int InputQty = 0;

                if (sheetname == "PCA")
                {
                    vchSet.Append(Method.BuildXML(rowData[4].ToString().Trim(), "Location"));
                    vchSet.Append(Method.BuildXML(rowData[5].ToString().Trim(), "DefectComponentPN"));
                    vchSet.Append(Method.BuildXML(rowData[6].ToString().Trim(), "LiabilityName"));

                    if (rowData[7].ToString().Trim() != "")
                    {
                        DateTime DueDate = DateTime.Parse(rowData[7].ToString().Trim());
                        checktime = InputCalCheck(rowData[7].ToString().Trim());
                        DateTime NowDate = DateTime.Parse(System.DateTime.Now.ToString("yyyy/MM/dd"));
                        checkcompare = DateTime.Compare(DueDate, NowDate);
                        if (checktime == true && checkcompare > 0)
                        {
                            vchSet.Append(Method.BuildXML(rowData[7].ToString().Substring(0, 10).Trim(), "DueDate"));
                        }
                    }

                    if (rowData[8].ToString().Trim() != "" && rowData[9].ToString().Trim() !="" )
                    {
                        InputQty = int.Parse(rowData[8].ToString().Trim());
                        FailQty = int.Parse(rowData[9].ToString().Trim());
                        if (InputQty >= FailQty)
                        {
                            vchSet.Append(Method.BuildXML(rowData[8].ToString().Trim(), "InputQty"));
                            vchSet.Append(Method.BuildXML(rowData[9].ToString().Trim(), "FailureQty"));
                        }
                    }
                }
                else if (sheetname == "CPU")
                {
                    vchSet.Append(Method.BuildXML(rowData[4].ToString().Trim(), "FaultCommodity"));
                    vchSet.Append(Method.BuildXML(rowData[5].ToString().Trim(), "FaultCommodityPN"));
                    vchSet.Append(Method.BuildXML(rowData[6].ToString().Trim(), "FaultCommoditySN"));
                    vchSet.Append(Method.BuildXML(rowData[7].ToString().Trim(), "LiabilityName"));

                    if (rowData[8].ToString().Trim() != "")
                    {
                        DateTime DueDate = DateTime.Parse(rowData[8].ToString().Trim());
                        checktime = InputCalCheck(rowData[8].ToString().Trim());
                        DateTime NowDate = DateTime.Parse(System.DateTime.Now.ToString("yyyy/MM/dd"));
                        checkcompare = DateTime.Compare(DueDate, NowDate);
                        if (checktime == true && checkcompare > 0)
                        {
                            vchSet.Append(Method.BuildXML(rowData[8].ToString().Substring(0, 10).Trim(), "DueDate"));
                        }
                    }

                    if (rowData[9].ToString().Trim() != "" && rowData[10].ToString().Trim()!="")
                    {
                        InputQty = int.Parse(rowData[9].ToString().Trim());
                        FailQty = int.Parse(rowData[10].ToString().Trim());
                        if (InputQty >= FailQty && InputQty>0)
                        {
                            vchSet.Append(Method.BuildXML(rowData[9].ToString().Trim(), "InputQty"));
                            vchSet.Append(Method.BuildXML(rowData[10].ToString().Trim(), "FailureQty"));
                        }
    
                    }
                }

                vchSet.Append(Method.BuildXML(sheetname, "Type"));
                vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                vchSet = vchSet.Replace(@"'", @"`");
                if (flag)
                {
                    if (CheckMaterial[0] == "Y" && CheckStation[0] == "Y" && InputQty >= FailQty)
                    {
                        string sqlCmd = Method.GetSqlCmd(sp_ImportMPIssue, "IMPORT", "MPISSUE", vchSet.ToString());
                        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                        if (sheetname == "PCA")
                            PCACorrectCount += 1;
                        else
                            CPUCorrectCount += 1;
                    }
                    else
                    {
                        if (sheetname == "PCA")
                            PCAIncorrectCount += 1;
                        else
                            CPUIncorrectCount += 1;
                    }
                }
               
              gvDraft.DataBind();                     
            }
        }
        if (sheetname == "PCA")
        {
            lblPCAMessage.Text = "PCA sheet has " + PCACorrectCount + " is Successful and " + PCAIncorrectCount + " is Fail! <br/>";
        }
        lblMessage.Text = lblPCAMessage.Text;
        lblMessage.Text += "CPU sheet has " + CPUCorrectCount + " is Successful and " + CPUIncorrectCount + " is Fail!";
        
        //Method.MessageOut(Page, Message);         
    }

    private void delFile(string filepath)
    {      
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        CheckRelatedIssue("Del");
    }
    protected void btnOpen_Click(object sender, EventArgs e)
    {
        CheckRelatedIssue("Update");
    }

    protected void btnDel1_Click(object sender, EventArgs e)
    {
        CheckRelatedIssue("Del");
    }
    protected void btnOpen1_Click(object sender, EventArgs e)
    {
        CheckRelatedIssue("Update");
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
    protected void CheckRelatedIssue(string Action)
    {
        int count = 0;
        foreach (GridViewRow gvr in gvDraft.Rows)
        {
            string IssueID = "";
            CheckBox cb = ((CheckBox)gvr.FindControl("cbcheck"));

            if (cb.Checked)
            {
                IssueID = ((HiddenField)gvr.FindControl("hfIssueID")).Value;
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(IssueID, "IssueID"));
                vchSet.Append(Method.BuildXML(Action, "Action"));
                vchSet.Append(Method.BuildXML(ddlSite.SelectedValue, "SiteID"));
                sqlCmd = Method.GetSqlCmd(sp_ImportMPIssue, "HANDLE", "MPISSUE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvDraft.DataBind();
                count++;
            }
        }

        if (count > 0)
        {
            if (Action == "Del")
                Method.MessageOut(Page, "Del Sucessfully");
            else
                Method.MessageOut(Page, "Update Sucessfully");
            
        }
        else
        {
            Method.MessageOut(Page, "Please check any Item!");
        }
    }
    protected void btnAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i <= gvDraft.Rows.Count - 1; i++)
        {
            if (((CheckBox)gvDraft.Rows[i].FindControl("cbcheck")).Checked)
            {
                ((CheckBox)gvDraft.Rows[i].FindControl("cbcheck")).Checked = false;
            }
            else
            {
                ((CheckBox)gvDraft.Rows[i].FindControl("cbcheck")).Checked = true;
            }
        }
    }
}
