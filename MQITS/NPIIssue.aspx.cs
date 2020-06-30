using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.IO;

public partial class NPIIssue : System.Web.UI.Page
{
    const string sp_NPIIssue = "sp_NPIIssue";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitCondition();  
        }    
    }
    protected void PreInitCondition()
    {
        if (Session["NPIIssueID"] == null && Request.QueryString["IssueID"] != null)//第一次
        {
            Session["NPIIssueID"] = Request.QueryString["IssueID"];
        }

        if (Request.QueryString["IssueID"] != null && Session["NPIIssue_ActiveViewIndex"] == null)
        {
            Session["NPIIssueID"] = Request.QueryString["IssueID"];
        }

        if (Session["NPIIssue_ActiveViewIndex"] != null)
        {
            if (Session["NPIIssueID"] != null && Request.QueryString["IssueID"] != null)
            {
                string NPIIssueID = Session["NPIIssueID"].ToString().ToUpper();

                if (NPIIssueID != Request.QueryString["IssueID"].ToString().ToUpper())
                {
                    Session["NPIIssue_ActiveViewIndex"] = null;
                }
            }
        }
    }


    protected void InitCondition()
    {
        txtUserId.Text = Method.GetCurrentUserId(Page);
        txtDueDate.Text = System.DateTime.Today.ToString("yyyy/MM/dd");//AR的DueDate
        txtDate.Text = System.DateTime.Today.ToString("yyyy/MM/dd");//Handing History 的Date
        PreInitCondition();   

        if (Request.QueryString["IssueID"] == null)
        {
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
            string sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "CHECK", "PROJECTMEMBER", vchSet.ToString());
            string[] Role = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmd);
            if (Role[0] == "")
            {
                Response.Write("<script language='javascript'>alert('You are not belong to any ProjectOwner/Member')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
            else
            {
                btnAdd.Visible = true;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                hlSQMP.Visible = false;
            }
        }
        else if (Request.QueryString["IssueID"] != null)
        {
            lblIssueID.Text = Request.QueryString["IssueID"];
            Session["NPIIssueID"] = lblIssueID.Text;
            //mvIssueForm.ActiveViewIndex = 0;
            if (Session["NPIIssue_ActiveViewIndex"] != null)
            {
                int imen = 0;
                int.TryParse(Session["NPIIssue_ActiveViewIndex"].ToString(), out imen);
                MenuFunction(imen);
            }
            else
            {
                MenuFunction(0);
            }
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
            vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
            string sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "REPORTERANDOWNER", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
            
            //權限控制
            lblReporterBadgeCode.Text = ds.Tables[0].Rows[0]["Reporter"].ToString();//取得報Issue的人
            lblStatusCode.Text = ds.Tables[0].Rows[0]["Status"].ToString();//取得Issue的Status
            lblProjectID.Text = ds.Tables[0].Rows[0]["Project"].ToString();//取得Issue的Project
            hfProjectMaterial.Value = ds.Tables[0].Rows[0]["ProjectMaterial"].ToString();
            hfMType.Value = ds.Tables[0].Rows[0]["MType"].ToString();
            BindAssignedTo(lblProjectID.Text);//取得專案成員與SQE/CE
            lblIssueOwnerBadgeCode.Text = ds.Tables[1].Rows[0]["ProjectOwner"].ToString();//取得ProjectOwner，其權限與Reporter一樣
         
            /*bool ARMember = false;
            for (int i = 0; i <= ddlAssignedTo.Items.Count - 1; i++)
            {
                if (ddlAssignedTo.Items[i].Value == txtUserId.Text)
                {
                    ARMember = true;
                }
            }*/

            //權限控制

            //用於Query
            if (lblReporterBadgeCode.Text == txtUserId.Text || lblIssueOwnerBadgeCode.Text == "Y")//repoter/issueowner
            {
                CheckARObject(gvAR.SelectedIndex);
                ViewSQE();
                if (lblStatusCode.Text == "3")//結束後無法再做任何事
                {
                    ObjectInvalid();
                    ViewSQE();
                }
            }
            else
            {
                ObjectInvalid();
                ViewSQE();
            }
            //用於Query

            //用於MyIssueAction和mail的連結
            if (Request.QueryString["ActionID"] != null)
            {
                lblActionID.Text = Request.QueryString["ActionID"].ToString();
                MenuFunction(0);
                StringBuilder vchSetAR = new StringBuilder();
                vchSetAR.Append(Method.BuildXML(Request.QueryString["IssueID"], "IssueID"));
                vchSetAR.Append(Method.BuildXML(Request.QueryString["ActionID"], "ActionID"));
                string sqlCmdAR = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "NPIAR", vchSetAR.ToString());
                DataSet dsAR = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdAR);
                lblARBadgeCode.Text = dsAR.Tables[0].Rows[0]["AssignedTo"].ToString();//取得Issue的AR

                //SQE
                /*StringBuilder vchSetSQE = new StringBuilder();
                vchSetSQE.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSetSQE.Append(Method.BuildXML(lblARBadgeCode.Text, "editor"));
                string sqlCmdSQE = Method.GetSqlCmd(sp_NPIIssue, "CHECK", "SQEROLE", vchSetSQE.ToString());
                DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdSQE);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hlSQMP.Visible = true;
                    hlSQMP.NavigateUrl = "http://quality.inventec.com.cn/SRM_SERVER/op_Issue_Manage.aspx?user_id=" + txtUserId.Text + "&type=NEW&UserBU=EBG&UserRole=SQE";
                    //http://quality.inventec.com.cn/SRM_SERVER/op_Issue_Manage.aspx?user_id=IEC940480&type=NEW&UserBU=EBG&UserRole=SQE
                }
                */

                //if (dsAR.Tables[0].Rows[0]["Status"].ToString() != "3")
               // {}

                //將所有值填入AR的欄位
                ddlAssignedTo.SelectedValue = dsAR.Tables[0].Rows[0]["AssignedTo"].ToString();
                ddlStatus.SelectedValue = dsAR.Tables[0].Rows[0]["Status"].ToString();
                txtDueDate.Text = dsAR.Tables[0].Rows[0]["DueDate"].ToString();
                txtPendingDay.Text = dsAR.Tables[0].Rows[0]["PendingDate"].ToString();
                txtActionDescription.Text = dsAR.Tables[0].Rows[0]["ActionDescription"].ToString();
                txtCompletionComment.Text = dsAR.Tables[0].Rows[0]["CompletionComment"].ToString();
                
                for (int i = 0; i <= gvAR.Rows.Count - 1; i++)
                {
                    if (((Label)gvAR.Rows[i].FindControl("lblActionID")).Text.ToUpper() == lblActionID.Text.ToUpper())
                    {
                       gvAR.SelectedIndex = i;
                    }                    
                }

                CheckARObject(gvAR.SelectedIndex);
                if (lblReporterBadgeCode.Text == txtUserId.Text)//Owner自己Assign給自己
                {

                }
                else if (lblARBadgeCode.Text == txtUserId.Text)
                {
                    fvIssue.ChangeMode(FormViewMode.ReadOnly);
                    ddlAssignedTo.Enabled = false;
                    Img3.Visible = false;
                    txtActionDescription.ReadOnly = true;
                    txtDueDate.ReadOnly = true;
                    btnSendMail.Visible = false;
                    btnUpload.Visible = true;
                    btnUploadPhoto.Visible = true;
                    btnAddHH.Visible = false;
                    btnCancelHH.Visible = false;
                    btnRepeatDefect.Visible = false;
                    btnAddNote.Visible = true;

                    //SQE
                    StringBuilder vchSetSQE = new StringBuilder();
                    vchSetSQE.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSetSQE.Append(Method.BuildXML(lblARBadgeCode.Text, "editor"));
                    string sqlCmdSQE = Method.GetSqlCmd(sp_NPIIssue, "CHECK", "SQEROLE", vchSetSQE.ToString());//判斷此人是否為SQE/CE
                    DataSet dsSQE = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdSQE);

                    StringBuilder vchSetSQMP = new StringBuilder();
                    vchSetSQMP.Append(Method.BuildXML(Request.QueryString["IssueID"], "IssueID"));
                    vchSetSQMP.Append(Method.BuildXML(lblActionID.Text, "ActionID"));
                    vchSetSQMP.Append(Method.BuildXML(lblARBadgeCode.Text, "editor"));
                    string sqlCmdSQMP = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "SQMPIDFORSQE", vchSetSQMP.ToString());
                    string[] SQMPID = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmdSQMP);


                    if (dsSQE.Tables[0].Rows.Count > 0)//是SQE/CE的人才看的見
                    {
                        hlSQMP.Visible = true;

                        if (SQMPID[0] != "0")//edit
                        {
                            hlSQMP.NavigateUrl = Constant.S_SQMPWebSite + "user_id=" + txtUserId.Text + "&type=DETAIL&IssueNumber=" +
                                    SQMPID[0] + "&UserBU=EBG&UserRole=SQE";
                            //http://quality.inventec.com.cn/SRM/op_Issue_Manage.aspx?user_id=IEC940480&type=DETAIL&IssueNumber=987&UserBU=EBG&UserRole=SQE
                        }
                        else//Add
                        {
                            hlSQMP.NavigateUrl = Constant.S_SQMPWebSite + "user_id=" + txtUserId.Text +
                                    "&type=NEW&UserBU=EBG&UserRole=SQE&IssueID=" + lblIssueID.Text + "&ActionID=" + lblActionID.Text;
                            //http://quality.inventec.com.cn/SRM/op_Issue_Manage.aspx?user_id=IEC940480&type=NEW&UserBU=EBG&UserRole=SQE;                           
                        }
                    }

                    if (dsAR.Tables[0].Rows[0]["Status"].ToString() == "3")
                    {
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                    }
                }
                else//防一下轉寄給別人
                {
                    ObjectInvalid();
                    ViewSQE();
                }
               
                chkShowIssue.Checked = false;
                fvIssue.Visible = false;
            }//用於MyIssueAction和mail的連結

            //UpdCondition();//LoadData()
            IssueTitle();
            MaterialEnable(hfMType.Value);
            RepeatDefectCheck();
        }    
    }

    protected void IssueTitle()
    {
        StringBuilder vchSetTitle = new StringBuilder();
        vchSetTitle.Append(Method.BuildXML(Request.QueryString["IssueID"], "IssueID"));
        string sqlCmdTitle = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "ISSUETITLE", vchSetTitle.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdTitle);
        Page.Title = ds.Tables[0].Rows[0][0].ToString();
    }

    protected void ViewSQE()
    {
        StringBuilder vchSetSQMP = new StringBuilder();
        vchSetSQMP.Append(Method.BuildXML(Request.QueryString["IssueID"], "IssueID"));
        string sqlCmdSQMP = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "SQMPIDFORVIEWER", vchSetSQMP.ToString());
        string[] SQMPID = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmdSQMP);
        if (SQMPID[0] != "")
        {
            hlSQMP.Visible = true;
            hlSQMP.NavigateUrl = Constant.S_SQMPWebSite + "user_id=" + txtUserId.Text + "&type=DETAIL&IssueNumber=" +
                    SQMPID[0] + "&UserBU=EBG&UserRole=IQC";
            //http://quality.inventec.com.cn/SRM/op_Issue_Manage.aspx?user_id=IES070448&type=DETAIL&IssueNumber=986&UserBU=EBG&UserRole=IQC
        }
    }

    protected void ObjectInvalid()
    {
        fvIssue.ChangeMode(FormViewMode.ReadOnly);
        btnAdd.Visible = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
        hlSQMP.Visible = false;
        btnSendMail.Visible = false;
        btnUpload.Visible = false;
        btnUploadPhoto.Visible = false;
        btnAddHH.Visible = false;
        btnCancelHH.Visible = false;
        btnRepeatDefect.Visible = false;
        btnAddNote.Visible = false;
    }

    protected void UpdCondition()
    {
        try
        {
            StringBuilder vchSet = new StringBuilder();
            string sqlCmd = "";
            vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
            sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "LISTISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

            if (fvIssue.CurrentMode == FormViewMode.Insert)
            {
                ((DropDownList)fvIssue.FindControl("ddlCustomer")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlCustomer")).SelectedValue = ds.Tables[0].Rows[0]["Customer"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlSite")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue = ds.Tables[0].Rows[0]["Site"].ToString();
                //((DropDownList)fvIssue.FindControl("ddlProject")).DataBind();
                //((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue = ds.Tables[0].Rows[0]["Project"].ToString();
                ((Label)fvIssue.FindControl("lblReporterName")).Text = ds.Tables[0].Rows[0]["Reporter"].ToString();


                DropDownList ddlProject = (DropDownList)fvIssue.FindControl("ddlProject");
                DropDownList ddlProjectMaterial = (DropDownList)fvIssue.FindControl("ddlPCACPU");

                ddlProject.DataBind();
                ddlProject.SelectedValue = ds.Tables[0].Rows[0]["Project"].ToString();
                ddlProjectMaterial.DataBind();
                ddlProjectMaterial.SelectedValue = ds.Tables[0].Rows[0]["ProjectMaterial"].ToString();
                lblProjectID.Text = ddlProject.SelectedValue;

                string MType = fn_GetMType(ddlProject.SelectedValue, ddlProjectMaterial.SelectedValue);


                StringBuilder vchSetPhase = new StringBuilder();
                string sqlCmdPhase = "";
                vchSetPhase.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue, "Project"));
                vchSetPhase.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue, "Site"));
                sqlCmdPhase = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "PROJECTPHASE", vchSetPhase.ToString());
                DAO.selDropDownList(((DropDownList)fvIssue.FindControl("ddlPhase")), Constant.S_MQITSConnStr, sqlCmdPhase, false);
                ((DropDownList)fvIssue.FindControl("ddlPhase")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlPhase")).SelectedValue = ds.Tables[0].Rows[0]["Phase"].ToString();

                ((DropDownList)fvIssue.FindControl("ddlOwner")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlOwner")).SelectedValue = ds.Tables[0].Rows[0]["IssueOwner"].ToString();
                //((DropDownList)fvIssue.FindControl("ddlPCACPU")).DataBind();
                //((DropDownList)fvIssue.FindControl("ddlPCACPU")).SelectedValue = ds.Tables[0].Rows[0]["MaterialType"].ToString();

                if (MType == "CPU")
                    ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect";
                else
                    ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect Sympton";
                
                ((DropDownList)fvIssue.FindControl("ddlStatus")).SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlStation")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlStation")).SelectedValue = ds.Tables[0].Rows[0]["Station"].ToString();
                ((Label)fvIssue.FindControl("lblPendDay")).Text = ds.Tables[0].Rows[0]["PendingDate"].ToString();
                ((Label)fvIssue.FindControl("lblFailQty")).Text = ds.Tables[0].Rows[0]["FailureQty"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlPriority")).SelectedValue = ds.Tables[0].Rows[0]["Priority"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlLiability")).SelectedValue = ds.Tables[0].Rows[0]["Liability"].ToString();
                ((Label)fvIssue.FindControl("lblFailRate")).Text = ds.Tables[0].Rows[0]["FailureRate"].ToString();
                ((TextBox)fvIssue.FindControl("txtIssueDate")).Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                ((TextBox)fvIssue.FindControl("txtDueDate")).Text = ds.Tables[0].Rows[0]["DueDate"].ToString();
                ((Label)fvIssue.FindControl("lblRepeatDefectCheck")).Text = ds.Tables[0].Rows[0]["RelatedIssue"].ToString();
                ((TextBox)fvIssue.FindControl("txtSerialNo")).Text = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                ((TextBox)fvIssue.FindControl("txtDefectSympton")).Text = ds.Tables[0].Rows[0]["DefectSymptom"].ToString();
                //lblProjectID.Text = ((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue;
                BindAssignedTo(lblProjectID.Text);

                if (ds.Tables[0].Rows[0]["Location"].ToString() != "")
                {
                    ((Panel)fvIssue.FindControl("panelPCA")).Visible = true;
                    ((TextBox)fvIssue.FindControl("txtLocation")).Text = ds.Tables[0].Rows[0]["Location"].ToString();
                    ((TextBox)fvIssue.FindControl("txtPackageType")).Text = ds.Tables[0].Rows[0]["PackageType"].ToString();
                    ((TextBox)fvIssue.FindControl("txtDefectPartNo")).Text = ds.Tables[0].Rows[0]["DefectComponentPN"].ToString();
                }

                if (ds.Tables[0].Rows[0]["FaultCommodity"].ToString() != ""
                        && ds.Tables[0].Rows[0]["FaultCommoditySN"].ToString() != ""
                        && ds.Tables[0].Rows[0]["FaultCommodityPN"].ToString() != "")
                {
                    ((Panel)fvIssue.FindControl("panelCPU")).Visible = true;
                    ((TextBox)fvIssue.FindControl("txtCommdity")).Text = ds.Tables[0].Rows[0]["FaultCommodity"].ToString();
                    ((TextBox)fvIssue.FindControl("txtCommodity_SN")).Text = ds.Tables[0].Rows[0]["FaultCommoditySN"].ToString();
                    ((TextBox)fvIssue.FindControl("txtCommodity_PN")).Text = ds.Tables[0].Rows[0]["FaultCommodityPN"].ToString();
                }
                ((TextBox)fvIssue.FindControl("txtRootCause")).Text = ds.Tables[0].Rows[0]["RootCause"].ToString();

                ((TextBox)fvIssue.FindControl("txtCorrectiveAction")).Text = ds.Tables[0].Rows[0]["CorrectAction"].ToString();
                ((Label)fvIssue.FindControl("lblIssueNoLabel")).Visible = true;
                ((Label)fvIssue.FindControl("lblIssueNo")).Visible = true;
                ((Label)fvIssue.FindControl("lblIssueNo")).Text = ds.Tables[0].Rows[0]["IssueSN"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script language='javascript'>alert('" + ex.ToString().Substring(0, 35) + "')</script>");
            Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");           
        }
    }
    protected void RepeatDefectCheck()
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "REPEATDEFECTCHECK", vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRepeatDefectCheck.DataSource = ds.Tables[0];
            gvRepeatDefectCheck.DataBind();
        }
    }
    protected void menuAction_MenuItemClick(object sender, MenuEventArgs e)
    {
        int imen = int.Parse(e.Item.Value);
        Session["NPIIssue_ActiveViewIndex"] = imen;
        Session["NPIIssueID"] = Request.QueryString["IssueID"];
        MenuFunction(imen);
    }
    protected void MenuFunction(int index)
    {
        mvIssueForm.ActiveViewIndex = index;

        menuAction.Items[index].Selected = true;

    }
    protected void btnRepeatDefect_Click(object sender, EventArgs e)
    {
        /*if (Request.QueryString["IssueID"] == null)
        {
            this.Response.Write("<script>window.close();</script>");
            Response.Redirect("NPIIssue.aspx?IssueID=" + lblIssueID.Text);
            //Method.OpenWindows(Page, "NPIIssue.aspx?IssueID=" + lblIssueID.Text, "RepeatDefect");  
            Method.OpenWindows(Page, "RepeatDefect.aspx?IssueID=" + lblIssueID.Text, "RepeatDefect", 980);  
            //this.Response.Write("<script>window.opener.location.replace(window.opener.document.referrer);window.close();</script>");
        }*/
        
        
        if (lblIssueID.Text != "")
        {
            //Method.OpenWindows(Page, "RepeatDefect.aspx?IssueID=" + lblIssueID.Text + "&UserID=" + txtUserId.Text, "RepeatDefect", 980);
            Method.OpenWindows(Page, "RepeatDefect.aspx?IssueID=" + lblIssueID.Text, "RepeatDefect", 980);  
        }
        else
            Method.MessageOut(Page, "Please Report Issue First!!");
    }  
    protected void chkShowIssue_CheckedChanged(object sender, EventArgs e)
    {
        if (chkShowIssue.Checked)
            fvIssue.Visible = true;
        else
            fvIssue.Visible = false;
    } 
    protected void fvIssue_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            if (fvIssue.CurrentMode == FormViewMode.Insert)
            {
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
            }  
        }
    }
    protected void fvIssue_ModeChanging(object sender, FormViewModeEventArgs e)
    {
        if (fvIssue.CurrentMode == FormViewMode.Insert)
        {
            e.Cancel = true;
        }
    }
    protected void ddlPCACPU_SelectedIndexChanged(object sender, EventArgs e)
    {
        //MaterialEnable();
        DropDownList ddlProject = (DropDownList)fvIssue.FindControl("ddlProject");
        DropDownList ddlProjectMaterial = (DropDownList)fvIssue.FindControl("ddlPCACPU");

        string MType = fn_GetMType(ddlProject.SelectedValue, ddlProjectMaterial.SelectedValue);

        MaterialEnable(MType);
    }
    protected void MaterialEnable(string MType)
    {
        if (fvIssue.CurrentMode == FormViewMode.Insert)
        {
            if (MType == "CPU")
            {
                ((Panel)fvIssue.FindControl("panelCPU")).Visible = true;
                ((Panel)fvIssue.FindControl("panelPCA")).Visible = false;
                ((TextBox)fvIssue.FindControl("txtLocation")).Text = "";
                ((TextBox)fvIssue.FindControl("txtPackageType")).Text = "";
                ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect";
            }
            else
            {
                ((Panel)fvIssue.FindControl("panelPCA")).Visible = true;
                ((Panel)fvIssue.FindControl("panelCPU")).Visible = false;
                ((TextBox)fvIssue.FindControl("txtCommdity")).Text = "";
                ((TextBox)fvIssue.FindControl("txtCommodity_SN")).Text = "";
                ((TextBox)fvIssue.FindControl("txtCommodity_PN")).Text = "";
                ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect Sympton";
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

    protected void fvIssue_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        DropDownList ddlProject = (DropDownList)fvIssue.FindControl("ddlProject");
        DropDownList ddlProjectMaterial = (DropDownList)fvIssue.FindControl("ddlPCACPU");
        DropDownList ddlPhase = (DropDownList)fvIssue.FindControl("ddlPhase");

        if (ddlProject.SelectedValue == ""
                || ddlPhase.SelectedValue == ""
                || ddlProjectMaterial.SelectedValue == "")
        {

            Method.MessageOut(Page, "Please fill out * fields");
            e.Cancel = true;
            return;
        }

        string MType = fn_GetMType(ddlProject.SelectedValue, ddlProjectMaterial.SelectedValue);

        if (MType == "")
        {
            Method.MessageOut(Page, "Cannot find Material Type, please correct Project Material setting!");
            e.Cancel = true;
            return;
        }

        bool check = false;
        bool check1 = false;
        TextBox txtIssueDate = ((TextBox)fvIssue.FindControl("txtIssueDate"));
        DateTime IssueDate = DateTime.Parse(txtIssueDate.Text);
        check = InputCalCheck(txtIssueDate.Text);
        if (check == false)
        {
            Method.MessageOut(Page, "its format is yyyy/MM/dd");
            e.Cancel = true;
            return;
        }
        if (((DropDownList)fvIssue.FindControl("ddlPCACPU")).SelectedValue == ""
            || ((TextBox)fvIssue.FindControl("txtSerialNo")).Text == ""
            || ((TextBox)fvIssue.FindControl("txtDefectSympton")).Text == "")
        {
            Method.MessageOut(Page, "Please fill out * fields");
            e.Cancel = true;
            return;
        }
        if (MType == "CPU")
        {
            if (((TextBox)fvIssue.FindControl("txtCommdity")).Text == ""
                || ((TextBox)fvIssue.FindControl("txtCommodity_SN")).Text == ""
                || ((TextBox)fvIssue.FindControl("txtCommodity_PN")).Text == "")
            {
                Method.MessageOut(Page, "Please fill out * fields");
                e.Cancel = true;
                return;
            }
        }
        else
        {  
            if (((TextBox)fvIssue.FindControl("txtLocation")).Text == "" 
                || ((TextBox)fvIssue.FindControl("txtDefectPartNo")).Text == "")
            {
                Method.MessageOut(Page, "Please fill out * fields");
                e.Cancel = true;
                return;
            }
        }

        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        if (Request.QueryString["IssueID"] == null && lblIssueID.Text=="")
        {
            lblIssueID.Text = System.Guid.NewGuid().ToString();
        }
        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlCustomer")).SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue, "Project"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "Reporter"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlPhase")).SelectedValue, "Phase"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlOwner")).SelectedValue, "IssueOwner"));

        vchSet.Append(Method.BuildXML(ddlProjectMaterial.SelectedValue, "ProjectMaterial"));
        vchSet.Append(Method.BuildXML(MType, "MType"));
        vchSet.Append(Method.BuildXML(ddlProjectMaterial.SelectedValue, "MaterialType"));
        

        //vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlPCACPU")).SelectedValue, "MaterialType"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlStatus")).SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlStation")).SelectedValue, "Station"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlPriority")).SelectedValue, "Priority"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlLiability")).SelectedValue, "Liability"));
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtIssueDate")).Text, "IssueDate"));
        TextBox txtDueDate = ((TextBox)fvIssue.FindControl("txtDueDate"));
        if (txtDueDate.Text.Trim() != "")
        {
            DateTime DueDate = DateTime.Parse(txtDueDate.Text);
            check1 = InputCalCheck(txtDueDate.Text);
            if (check1 == false)
            {
                Method.MessageOut(Page, "its format is yyyy/MM/dd");
                return;
            }
            int checkcompare = DateTime.Compare(DueDate, IssueDate);

            if (checkcompare < 0)
            {
                Method.MessageOut(Page, "DueDate must greater than IssueDate!");
                e.Cancel = true;
                return;
            }
            else
            {
                vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtDueDate")).Text, "DueDate"));
            }
        }
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtSerialNo")).Text, "SerialNo"));
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtDefectSympton")).Text, "DefectSymptom"));

        if (((TextBox)fvIssue.FindControl("txtLocation")).Enabled)
        {
            vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtLocation")).Text, "Location"));
        }

        if (((TextBox)fvIssue.FindControl("txtPackageType")).Text.Trim() != "")
        {
            vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtPackageType")).Text, "PackageType"));
        }

        if (((TextBox)fvIssue.FindControl("txtDefectPartNo")).Text.Trim()!="")
            vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtDefectPartNo")).Text, "DefectPartNo"));

        if (((TextBox)fvIssue.FindControl("txtCommdity")).Enabled
            && ((TextBox)fvIssue.FindControl("txtCommodity_SN")).Enabled
            && ((TextBox)fvIssue.FindControl("txtCommodity_PN")).Enabled)
        {
            vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtCommdity")).Text, "FaultCommodity"));
            vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtCommodity_SN")).Text, "FaultCommoditySN"));
            vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtCommodity_PN")).Text, "FaultCommodityPN"));
        }
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtRootCause")).Text, "RootCause"));
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtCorrectiveAction")).Text, "CorrectAction"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        vchSet = vchSet.Replace(@"'", @"`");

        sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SAVE", "NPIISSUE", vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvIssue.DataBind();
        Method.MessageOut(Page, "Save Successfully!!");

        if (Request.QueryString["IssueID"]==null)
        {
           Response.Write("<script languge='javascript'>alert('Save Successfully!!'); window.location.href='NPIIssue.aspx?IssueID=" + lblIssueID.Text + "'</script>"); 
        }
        
        UpdCondition();
        InitCondition();
        e.Cancel = true;
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
    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        DataTable dtbcc = new DataTable();
        dtbcc.Columns.Add("BadgeCode");
        dtbcc.Columns.Add("email");
        string sqlCmdbcc = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "MAILBCC", "");
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdbcc.ToString());
        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        {
            dtbcc.Rows.Add(ds.Tables[0].Rows[i]["BadgeCode"].ToString(), ds.Tables[0].Rows[i]["email"].ToString());
        }

        int count = 0;
        foreach (GridViewRow gvr in gvAR.Rows)
        {
            CheckBox cb = ((CheckBox)gvr.FindControl("cbcheck"));

            if (cb.Checked)
            {
               string mail_body = "";
                SendMail.smtpMail(Method.GetUsereMailByUserId(txtUserId.Text),
                            Method.GetUsereMailByUserId(((Label)gvr.FindControl("lblAssignedToCode")).Text),
                            Method.GetUsereMailByUserId(txtUserId.Text), dtbcc, "MQITS:Action Request Notice--IssueID:" + ((Label)fvIssue.FindControl("lblIssueNo")).Text,
                   mail_body = GenerateBody(lblIssueID.Text, ((Label)gvr.FindControl("lblActionID")).Text,"Remind"));
               
       
                count++;
            }
        }
        if (count > 0)
        {
            Method.MessageOut(Page, "Mail send out");
            for (int i = 0; i <= gvAR.Rows.Count - 1; i++)
            {
                if (((CheckBox)gvAR.Rows[i].FindControl("cbcheck")).Checked)
                {
                    ((CheckBox)gvAR.Rows[i].FindControl("cbcheck")).Checked = false;
                }
            }
        }
        else
        {
            Method.MessageOut(Page, "Please check item");
        }      
    }
    protected string GenerateBody(string IssueID, string ActionID,string Receiver)
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(IssueID, "IssueID"));
        vchSet.Append(Method.BuildXML(ActionID, "ActionID"));
        sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SENDMAIL", "NPIAR", vchSet.ToString());

        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        string content = "";
        content += "<table width='90%' border='1' style='font-family:Tahoma'>";
        content += "<tr><td style='width: 20%; background-color: #b6cade;'><b>Project</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["ProjectName"].ToString() + "</td>";
        content += "<td style='WIDTH: 20%; background-color: #b6cade;'><b>Phase</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["PhaseName"].ToString() + "</td></tr>";
        content += "<tr><td style='WIDTH: 20%; background-color: #b6cade;'><b>AR Description</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["ActionDescription"].ToString() + "</td>";
        content += "<td style='WIDTH: 20%; background-color: #b6cade;'><b>Reporter</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["Reporter"].ToString() + "</td></tr>";
        content += "<tr><td style='WIDTH: 20%; background-color: #b6cade;'><b>AssignTo</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["AssignToer"].ToString() + "</td>";
        content += "<td style='WIDTH: 20%; background-color: #b6cade;'><b>Priority</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["PriorityName"].ToString() + "</td></tr>";
        content += "<tr><td style='WIDTH: 20%; background-color: #b6cade;'><b>Assigned Date</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["AssignedDate"].ToString() + "</td>";
        content += "<td style='width: 20%; background-color: #b6cade;'><b>Due Date</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["DueDate"].ToString() + "</td></tr>";
        if (Receiver != "Remind")
        {
            content += "<tr><td style='WIDTH: 20%; background-color: #b6cade;'><b>CompletionComment</b></td>";
            content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["CompletionComment"].ToString() + "</td>";
            content += "<td style='width: 20%; background-color: #b6cade;'></td>";
            content += "<td style='WIDTH: 25%; background-color: #eff3f8;'></td></tr>";
        }
     
        content += "<tr><td colspan='4'><p align='center'><b><a href ='" + Constant.S_WebSite + "NPIIssue.aspx?IssueID=" + ds.Tables[0].Rows[0]["IssueID"].ToString() + "";
        content += "&ActionID=" + ds.Tables[0].Rows[0]["ActionID"].ToString() + "";
        content += "'>Detail</b></a></td></tr></table>";
        
        return content;
    }

    protected void gvAR_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblStatusCode.Text == "3")
        {
            Method.MessageOut(Page, "It cannot Edit");
            return;
        }
        
        try
        {            
            if (((Label)gvAR.SelectedRow.FindControl("lblStatusCode")).Text == "3")
            {
                if (txtUserId.Text != lblReporterBadgeCode.Text &&  lblIssueOwnerBadgeCode.Text != "Y")
                {
                    Method.MessageOut(Page, "It cannot modify");
                    return;
                }
            }
            if (((Label)gvAR.SelectedRow.FindControl("lblAssignedToCode")).Text == txtUserId.Text
                   //|| (txtUserId.Text == lblReporterBadgeCode.Text || txtUserId.Text == lblIssueOwnerBadgeCode.Text))
                || (txtUserId.Text == lblReporterBadgeCode.Text || lblIssueOwnerBadgeCode.Text=="Y"))
            {

                lblActionID.Text = gvAR.DataKeys[gvAR.SelectedIndex].Value.ToString();
                ddlAssignedTo.SelectedValue = ((Label)gvAR.SelectedRow.FindControl("lblAssignedToCode")).Text.ToString();
                txtDueDate.Text = ((Label)gvAR.SelectedRow.FindControl("lblDueDate")).Text.ToString();
                txtPendingDay.Text = ((Label)gvAR.SelectedRow.FindControl("lblPD")).Text.ToString();
                txtFinishDate.Text = ((Label)gvAR.SelectedRow.FindControl("lblFinishDate")).Text.ToString();
                txtActionDescription.Text = ((Label)gvAR.SelectedRow.FindControl("lblAD")).Text.ToString();
                txtCompletionComment.Text = ((Label)gvAR.SelectedRow.FindControl("lblCC")).Text.ToString();
                ddlStatus.SelectedValue = ((Label)gvAR.SelectedRow.FindControl("lblStatusCode")).Text.ToString();
                CheckARObject(gvAR.SelectedIndex);
            }
            else
            {
                Method.MessageOut(Page, "You have no authority to modity");
            }        
        }
        catch
        {
            Method.MessageOut(Page,"This ProjectMember is not exist");
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            if (txtFileDescr.Text.Trim() != "" && fufile.PostedFile.FileName != "")
            {
                //string[] Ext = fufile.PostedFile.FileName.Split('.');
                string Ext = fufile.PostedFile.FileName.Substring(fufile.PostedFile.FileName.LastIndexOf(@".") + 1);

                bool check = true;

                //check = CheckFileName(Ext[1].ToUpper().ToString(), "IssueFU");
                check = CheckFileName(Ext.ToUpper().ToString(), "IssueFU");


                if (check == true)
                {
                    string FileName = FileUpload(fufile, lblIssueID.Text);
                    //string[] Split = FileName.Split('.');

                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSet.Append(Method.BuildXML(txtFileDescr.Text.Trim(), "FileDescr"));
                    vchSet.Append(Method.BuildXML(FileName, "FileName"));
                    vchSet.Append(Method.BuildXML(fufile.PostedFile.ContentLength.ToString(), "FileSize"));
                    //vchSet.Append(Method.BuildXML(Split[1], "FileExt"));
                    vchSet.Append(Method.BuildXML(Ext.ToUpper(), "FileExt"));
                    vchSet.Append(Method.BuildXML(Constant.S_WebSite + "UploadFile/" + lblIssueID.Text + @"/" + FileName, "FilePath"));
                    vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                    vchSet = vchSet.Replace(@"'", @"`");
                    sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "UPLOAD", "ATTACHFILE", vchSet.ToString());
                    DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                    BindData(lblIssueID.Text);
                    gvATF.DataBind();
                    txtFileDescr.Text = "";
                }
                else
                {
                    Method.MessageOut(Page, "Please Upload Extfile " + lblExtType.Text);
                }
            }
            else
            {
                Method.MessageOut(Page, "Please fill out * fields");
            }
        }
        else
        {
            Method.MessageOut(Page, "Please Report Issue First!!");
        }
    }

    public string FileUpload(FileUpload fu, string folder)
    {
        string backupFile = "";
        if (fu.PostedFile.FileName != "")
        {
            if (!Directory.Exists(Constant.S_FileRoot + folder))
            {
                Directory.CreateDirectory(Constant.S_FileRoot + folder);
            }

            backupFile = fu.PostedFile.FileName.Substring(fu.PostedFile.FileName.LastIndexOf(@"\") + 1);

            if (backupFile != "")
            {
                //fu.PostedFile.SaveAs(ConfigurationManager.AppSettings["FilePath"] + folder + @"\" + backupFile);
                fu.PostedFile.SaveAs(Constant.S_FileRoot + folder + @"\" + backupFile);
            }
        }
        else
        {
            backupFile = "";
        }

        return backupFile;
    }
    private bool CheckFileName(string FileName,string Type)
    {
        lblExtType.Text = "";
        string vchCmd = "CHECK";
        string vchObjectName = "EXTTYPE";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(Type, "TYPE"));
        string sqlCmd = Method.GetSqlCmd(sp_NPIIssue, vchCmd, vchObjectName, vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        string[] Ext = ds.Tables[0].Rows[0]["EXTTYPE"].ToString().Split(';');

        for (int i = 0; i < Ext.Length; i++)
        {
            lblExtType.Text += "; " + Ext[i];
            if (FileName == Ext[i])
                return true;
        }
        lblExtType.Text = lblExtType.Text.Substring(1);
        return false;
    }
    protected void btnUploadPhoto_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            if (txtPhotoDesc.Text.Trim() != "" && fuphoto.PostedFile.FileName != "")
            {
                //string[] Ext = fuphoto.PostedFile.FileName.Split('.');
                string Ext = fuphoto.PostedFile.FileName.Substring(fuphoto.PostedFile.FileName.LastIndexOf(@".") + 1);
                bool check = true;

                //check = CheckFileName(Ext[1].ToUpper().ToString(), "IssuePH");
                check = CheckFileName(Ext.ToUpper().ToString(), "IssuePH");


                if (check == true)
                {
                    string FileName = FileUpload(fuphoto, lblIssueID.Text);
                    //string[] Split = FileName.Split('.');

                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSet.Append(Method.BuildXML(txtPhotoDesc.Text.Trim(), "FileDescr"));
                    vchSet.Append(Method.BuildXML(FileName, "FileName"));
                    vchSet.Append(Method.BuildXML(fuphoto.PostedFile.ContentLength.ToString(), "FileSize"));
                    //vchSet.Append(Method.BuildXML(Split[1], "FileExt"));
                    vchSet.Append(Method.BuildXML(Ext.ToUpper(), "FileExt"));
                    vchSet.Append(Method.BuildXML(Constant.S_WebSite + "UploadFile/" + lblIssueID.Text + @"/" + FileName, "FilePath"));
                    vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                    vchSet = vchSet.Replace(@"'", @"`");
                    sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "UPLOAD", "PHOTO", vchSet.ToString());
                    DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                    BindData(lblIssueID.Text);
                    rpPhoto.DataBind();                  
                    txtPhotoDesc.Text = "";
                    
                }
                else
                {
                    Method.MessageOut(Page, "Please Upload Extfile " + lblExtType.Text);
                }
            }
            else
            {
                Method.MessageOut(Page, "Please fill out * fields");
            }
        }
        else
        {
            Method.MessageOut(Page, "Please Report Issue First!!");
        }
    }
    protected void rpPhoto_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)(e.Item.FindControl("imgq1"));
        img.Attributes.Add("onmouseover", "this.style.cursor='hand'");
        img.Attributes.Add("onclick", "window.open('" + img.ImageUrl.ToString() + "');'height=600,width=800,toolbar=no,location=no,status=yes,menubar=no,resizable=yes,left=100,top=0'");
    }
    protected void gvATF_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            if (((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblEditor")).Text == txtUserId.Text
                   || lblIssueOwnerBadgeCode.Text=="Y" || lblReporterBadgeCode.Text==txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfID")).Value.ToString(), "ID"));
                vchSet.Append(Method.BuildXML(((HyperLink)((GridView)sender).Rows[e.RowIndex].FindControl("hyFileDesc")).Text.ToString(), "FileDescr"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfFileName")).Value.ToString(), "FileName"));
                sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "DELETE", "ATTACHFILE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvATF.DataBind();
            }
            else
            {
                Method.MessageOut(Page, "You have no authority to del");
            }
        }
        else
        {
            Method.MessageOut(Page, "It cannot Del");
        }
        e.Cancel = true;
    }
    protected void rpPhoto_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            if (e.CommandName == "DelChart")
            {
                if (((Label)e.Item.FindControl("lblEditor")).Text == txtUserId.Text
                       || lblIssueOwnerBadgeCode.Text=="Y" || lblReporterBadgeCode.Text==txtUserId.Text)
                {
                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSet.Append(Method.BuildXML(((HiddenField)e.Item.FindControl("hfid")).Value, "ID"));
                    vchSet.Append(Method.BuildXML(((HiddenField)e.Item.FindControl("hfFileName")).Value, "FileName"));
                    vchSet.Append(Method.BuildXML(((Label)e.Item.FindControl("lblPhotoDescr")).Text, "FileDescr"));
                    sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "DELETE", "PHOTO", vchSet.ToString());
                    DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                    rpPhoto.DataBind();
                }
                else
                {
                    Method.MessageOut(Page, "You have no authority to del");
                }
            }
        }
        else
        {
            Method.MessageOut(Page, "It cannot Del");
        }
    }
    protected void btnAddHH_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            bool checkDate = false;
            if (txtDate.Text.Trim() != "" && txtInputQty.Text.Trim() != "" && txtFailQty.Text.Trim() != "" && txtHandlingNote.Text.Trim() != "")
            {
                checkDate = InputCalCheck(txtDate.Text.Trim());
                if (checkDate == false)
                {
                    Method.MessageOut(Page, "its format is yyyy/MM/dd");
                    return;
                }

                try
                {
                    int InputQty, FailQty;
                    InputQty = int.Parse(txtInputQty.Text.Trim());
                    FailQty = int.Parse(txtFailQty.Text.Trim());
                    if (InputQty >= FailQty && InputQty>0)
                    {
                        StringBuilder vchSet = new StringBuilder();
                        string sqlCmd = "";
                        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                        if (gvHH.SelectedIndex == -1)
                        {
                            lblHandingID.Text = "";
                        }
                        vchSet.Append(Method.BuildXML(lblHandingID.Text, "HandingID"));
                        vchSet.Append(Method.BuildXML(txtDate.Text.Trim(), "DueDate"));
                        vchSet.Append(Method.BuildXML(InputQty.ToString(), "InputQty"));
                        vchSet.Append(Method.BuildXML(FailQty.ToString(), "FailureQty"));
                        vchSet.Append(Method.BuildXML(txtHandlingNote.Text.Trim(), "HandingNote"));
                        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                        vchSet = vchSet.Replace(@"'", @"`");
                        sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SAVE", "HANDLINGNOTE", vchSet.ToString());
                        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                        BindData(lblIssueID.Text);
                        gvHH.DataBind();
                        txtDate.Text = "";
                        txtInputQty.Text = "";
                        txtFailQty.Text = "";
                        txtHandlingNote.Text = "";
                        gvHH.SelectedIndex = -1;
                        UpdCondition();
                    }
                    else
                    {
                        Method.MessageOut(Page, "InputQty must greater or equal than FailQty!!");
                    }
                }
                catch
                {
                    Method.MessageOut(Page, "InputQty and FailQty must digital!!");
                    txtInputQty.Text = "";
                    txtFailQty.Text = "";
                }
            }
            else
            {
                Method.MessageOut(Page, "Please fill out * fields");
            }
        }
        else
        {
            Method.MessageOut(Page, "Please Report Issue First!!");
        }
    }
    protected void gvHH_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            lblHandingID.Text = gvHH.DataKeys[gvHH.SelectedIndex].Value.ToString();
            txtDate.Text = ((Label)gvHH.SelectedRow.FindControl("lblHandingDate")).Text.ToString();
            txtInputQty.Text = ((Label)gvHH.SelectedRow.FindControl("lblInputQty")).Text.ToString();
            txtFailQty.Text = ((Label)gvHH.SelectedRow.FindControl("lblFailureQty")).Text.ToString();
            txtHandlingNote.Text = ((Label)gvHH.SelectedRow.FindControl("lblHandlingNote")).Text.ToString();
        }
        else
        {
            gvHH.SelectedIndex = -1;
        }
    }
    protected void gvHH_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            if (((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblEditor")).Text == txtUserId.Text
                || lblIssueOwnerBadgeCode.Text=="Y" || lblReporterBadgeCode.Text==txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfHandingID")).Value.ToString(), "HandingID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblHandingDate")).Text.ToString(), "DueDate"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblInputQty")).Text.ToString(), "InputQty"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblFailureQty")).Text.ToString(), "FailureQty"));
                sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "DELETE", "HANDLINGNOTE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvHH.DataBind();
            }
            else
            {
                Method.MessageOut(Page, "You have no authority to del");
            }
        }
        else
        {
            Method.MessageOut(Page, "It cannot Del");
        }
        e.Cancel = true;
    }

    protected void btnAddNote_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            if (txtNote.Text.Trim() != "")
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(txtNote.Text.Trim().Replace(@"'", @"`"), "Note"));
                vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SAVE", "NOTE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                BindData(lblIssueID.Text);
                gvNote.DataBind();
                txtNote.Text = "";
            }
            else
            {
                Method.MessageOut(Page, "Please fill out * fields");
            }
        }
        else
        {
            Method.MessageOut(Page, "Please Report Issue First!!");
        }
    }
    protected void gvNote_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtNote")).Text.Trim() != "")
        {
            if (((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblEditor")).Text == txtUserId.Text
                  || lblIssueOwnerBadgeCode.Text=="Y" || lblReporterBadgeCode.Text==txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblNoteID")).Text.ToString(), "NoteID"));
                vchSet.Append(Method.BuildXML(((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtNote")).Text.ToString(), "Note"));
                vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "UPDATE", "NOTE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvNote.DataBind();
                gvNote.EditIndex = -1;
            }
            else
            {
                Method.MessageOut(Page, "You have no authority to edit");
            }
        }
        e.Cancel = true;
    }
    protected void gvNote_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            if (((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblEditor")).Text == txtUserId.Text
               || lblIssueOwnerBadgeCode.Text=="Y" || lblReporterBadgeCode.Text==txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblNoteID")).Text.ToString(), "NoteID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblNote")).Text.ToString(), "Note"));
                vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "DELETE", "NOTE", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvNote.DataBind();
            }
            else
            {
                Method.MessageOut(Page, "You have no authority to del");
            }
        }
        else
        {
            Method.MessageOut(Page, "It cannot Del");
        }
        e.Cancel = true;
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue != "")
        {
            ((DropDownList)fvIssue.FindControl("ddlOwner")).SelectedValue = txtUserId.Text;
            
            StringBuilder vchSet = new StringBuilder();
            string sqlCmd = "";
            vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue, "Project"));
            vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue, "Site"));
            sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "PROJECTPHASE", vchSet.ToString());
            DAO.selDropDownList(((DropDownList)fvIssue.FindControl("ddlPhase")), Constant.S_MQITSConnStr, sqlCmd, false);
            StringBuilder vchSetPhase = new StringBuilder();
            string sqlCmdPhase = "";
            vchSetPhase.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue, "Project"));
            sqlCmdPhase = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "CURRENTPHASE", vchSetPhase.ToString());
            string[] CurrentPhase = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmdPhase);
            if (CurrentPhase[0] != "")
                ((DropDownList)fvIssue.FindControl("ddlPhase")).SelectedValue = CurrentPhase[0];
            else
                ((DropDownList)fvIssue.FindControl("ddlPhase")).SelectedValue = "";
        }
        else
        {
            ((DropDownList)fvIssue.FindControl("ddlPhase")).Items.Clear();
        }

        lblProjectID.Text = ((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue;
        BindAssignedTo(lblProjectID.Text);
    }
    protected void BindAssignedTo(string ProjectID)
    {
        StringBuilder vchSetAssignedTo = new StringBuilder();
        vchSetAssignedTo.Append(Method.BuildXML(ProjectID, "Project"));
        string sqlCmdAss = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "ASSIGNEDTO", vchSetAssignedTo.ToString());
        DAO.selDropDownList(ddlAssignedTo, Constant.S_MQITSConnStr, sqlCmdAss, false);
    }
    protected void gvRepeatDefectCheck_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            //if (lblReporterBadgeCode.Text == txtUserId.Text || lblIssueOwnerBadgeCode.Text == txtUserId.Text)
            if (lblReporterBadgeCode.Text == txtUserId.Text || lblIssueOwnerBadgeCode.Text == "Y")
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfIssueID")).Value.ToString(), "RelatedIssueID"));
                sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "DELETE", "REPEATDEFECTCHECK", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvRepeatDefectCheck.DataBind();
                RepeatDefectCheck();
                UpdCondition();
            }
            else
            {
                Method.MessageOut(Page, "You have no authority to del");
            }
        }
        else
        {
            Method.MessageOut(Page, "It cannot Del");
        }
        e.Cancel = true;
    }

    protected void fvIssue_DataBound(object sender, EventArgs e)
    {
        if (fvIssue.CurrentMode == FormViewMode.Insert)
        {
            if (Request.QueryString["IssueID"] == null)
            {
                ((DropDownList)fvIssue.FindControl("ddlStatus")).SelectedValue = "1";
                ((DropDownList)fvIssue.FindControl("ddlPriority")).SelectedValue = "3";
                ((DropDownList)fvIssue.FindControl("ddlLiability")).SelectedValue = "6";
                ((TextBox)fvIssue.FindControl("txtIssueDate")).Text = System.DateTime.Today.ToString("yyyy/MM/dd");
                ((Label)fvIssue.FindControl("lblReporterName")).Text = Method.GetUserName(txtUserId.Text);
                if (((DropDownList)fvIssue.FindControl("ddlOwner")).Items.Count > 0)
                    ((DropDownList)fvIssue.FindControl("ddlOwner")).SelectedValue = txtUserId.Text;
                ((Label)fvIssue.FindControl("lblRepeatDefectCheck")).Text = "No";
            }
            else
            {
                UpdCondition();
            }
        }
        if (fvIssue.CurrentMode == FormViewMode.ReadOnly)
        {

            Label lblProjectMaterialType = ((Label)fvIssue.FindControl("lblProjectMaterialType"));
            Label lblProjectMaterial = ((Label)fvIssue.FindControl("lblProjectMaterial"));
            
            if (((Label)fvIssue.FindControl("lblLocationName")).Text != "")
            {
                ((Panel)fvIssue.FindControl("panelPCA")).Visible = true;
            }
            if (((Label)fvIssue.FindControl("lblFaultyCommodityName")).Text != "")
            {
                ((Panel)fvIssue.FindControl("panelCPU")).Visible = true;
            }
            if (lblProjectMaterialType.Text == "CPU")
            {
                ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect";
            }
            else
            {
                ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect Sympton";
            }
        }
    }
    public bool ChangeBool(string inBool)
    {//將畫面上bool資料改變true<->false
        if (inBool == "True")
            return false;
        else
            return true;
    }

    protected void btnAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i <= gvAR.Rows.Count - 1; i++)
        {
            if (((CheckBox)gvAR.Rows[i].FindControl("cbcheck")).Enabled)
            {
                if (((CheckBox)gvAR.Rows[i].FindControl("cbcheck")).Checked)
                {
                    ((CheckBox)gvAR.Rows[i].FindControl("cbcheck")).Checked = false;
                }
                else
                {
                    ((CheckBox)gvAR.Rows[i].FindControl("cbcheck")).Checked = true;
                }
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlAssignedTo.SelectedIndex = 0;
        txtDueDate.Text = "";
        txtPendingDay.Text = "";
        //txtFailQty.Text = "";
        txtFinishDate.Text = "";
        txtActionDescription.Text = "";
        txtCompletionComment.Text = "";
        CheckARObject(gvAR.SelectedIndex = -1);

    }
    protected void btnCancelHH_Click(object sender, EventArgs e)
    {
        txtDate.Text = "";
        txtInputQty.Text = "";
        txtFailQty.Text = "";
        txtHandlingNote.Text = "";
        gvHH.SelectedIndex = -1;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (gvAR.SelectedIndex != -1 || lblActionID.Text != "")
        {
            if (lblReporterBadgeCode.Text != txtUserId.Text
                && lblIssueOwnerBadgeCode.Text != "Y"
                && txtActionDescription.ReadOnly == true
                && txtActionDescription.Text == "")
            {
                Method.MessageOut(Page, "You have no authority to Add");
                return;
            }
            SendMailOwner(lblActionID.Text, "SAVE");
        }
        else
        {
            Method.MessageOut(Page, "Please check item");

        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            SendMailOwner(System.Guid.NewGuid().ToString(),"ADD");
        }
        else
        {
            Method.MessageOut(Page, "Please Report Issue First!!");
        }
    }
    protected void SendMailOwner(string ARID,string cmd)
    {
        bool checkDate = false;
        if (txtDueDate.Text.Trim() != "")
        {
            checkDate = InputCalCheck(txtDueDate.Text.Trim());
            if (checkDate == false)
            {
                Method.MessageOut(Page, "its format is yyyy/MM/dd");
                return;
            }
        }
        else
        {
            Method.MessageOut(Page, "Please Fill out Due Date");
            return;
        }

        if (ddlStatus.SelectedValue == "1")
        {
            if (txtActionDescription.Text.Trim() == "")
            {
                Method.MessageOut(Page, "Please Fill out ActionDescription ");
                return;
            }
        }
        if (ddlStatus.SelectedValue == "3")
        {
            if (txtCompletionComment.Text.Trim() == "")
            {
                Method.MessageOut(Page, "Please Fill out CompletionComment");
                return;
            }
        }
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        vchSet.Append(Method.BuildXML(ARID, "ActionID"));
        vchSet.Append(Method.BuildXML(ddlAssignedTo.SelectedValue, "AssignedTo"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(txtDueDate.Text.Trim(), "DueDate"));
        vchSet.Append(Method.BuildXML(txtActionDescription.Text.Trim(), "ActionDescription"));
        vchSet.Append(Method.BuildXML(txtCompletionComment.Text.Trim(), "CompletionComment"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        vchSet = vchSet.Replace(@"'", @"`");
        sqlCmd = Method.GetSqlCmd(sp_NPIIssue, cmd, "NPIAR", vchSet.ToString());

        string[] ActionRID = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmd);
        BindData(lblIssueID.Text);
        gvAR.DataBind();

       
        DataTable dtbcc = new DataTable();
        dtbcc.Columns.Add("BadgeCode");
        dtbcc.Columns.Add("email");
        string sqlCmdbcc = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "MAILBCC", "");
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdbcc.ToString());
        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        {
            dtbcc.Rows.Add(ds.Tables[0].Rows[i]["BadgeCode"].ToString(), ds.Tables[0].Rows[i]["email"].ToString());
        }
        string mail_body = "";
        string mail_Subject = "";

        if (lblIssueOwnerBadgeCode.Text == "Y" || lblIssueOwnerBadgeCode.Text == "")//IssueOwner 送信給AR
        {
            mail_Subject = "MQITS:Action Request Notice, IssueID:" + ((Label)fvIssue.FindControl("lblIssueNo")).Text;
            SendMail.smtpMail(Method.GetUsereMailByUserId(txtUserId.Text),
                       Method.GetUsereMailByUserId(ddlAssignedTo.SelectedValue),
                       Method.GetUsereMailByUserId(txtUserId.Text), dtbcc, mail_Subject,
              mail_body = GenerateBody(lblIssueID.Text, ARID, "Remind"));

            Method.MessageOut(Page, "Save Successfully!! Mail to " + Method.GetUserNameByeMail(Method.GetUsereMailByUserId(ddlAssignedTo.SelectedValue)) + mail_Subject);
        }
        else//AR回信
        {
            if (ddlStatus.SelectedValue == "3")
            {
                mail_Subject = "MQITS:AR Close Notice, IssueID:" + ((Label)fvIssue.FindControl("lblIssueNo")).Text;
                SendMail.smtpMail(Method.GetUsereMailByUserId(txtUserId.Text),
                            Method.GetUsereMailByUserId(lblReporterBadgeCode.Text),
                            Method.GetUsereMailByUserId(txtUserId.Text), dtbcc, mail_Subject,
                   mail_body = GenerateBody(lblIssueID.Text, lblActionID.Text, lblReporterBadgeCode.Text));

                Method.MessageOut(Page, "Save Successfully!! Mail to " + Method.GetUserNameByeMail(Method.GetUsereMailByUserId(lblReporterBadgeCode.Text)) + mail_Subject);
            }
        }

        txtPendingDay.Text = "";
        txtFinishDate.Text = "";
        txtDueDate.Text = "";
        txtActionDescription.Text = "";
        txtCompletionComment.Text = "";
        gvAR.SelectedIndex = -1;
        CheckARObject(-1);
    }
    protected void CheckARObject(int index)
    {
        if (index == -1)
        {
            btnSave.Visible = false;
            btnCancel.Visible = false;
            if (lblReporterBadgeCode.Text == txtUserId.Text || lblIssueOwnerBadgeCode.Text=="Y")
            {
                btnAdd.Visible = true;
            }         
        }
        else
        {
            btnAdd.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }
    }
    protected void BindData(string IssueID)
    {
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(IssueID, "IssueID"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_NPIIssue, "SELECT", "REPORTERANDOWNER", vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        lblReporterBadgeCode.Text = ds.Tables[0].Rows[0]["Reporter"].ToString();//取得報Issue的人
        lblIssueOwnerBadgeCode.Text = ds.Tables[1].Rows[0]["ProjectOwner"].ToString();
    }
}
