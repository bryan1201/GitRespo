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

public partial class RMAIssue : System.Web.UI.Page
{
    const string sp_RMAIssue = "sp_RMAIssue";
    const string sp_RoleMaintain = "sp_RoleMaintain";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.InitCondition();
        }
    }
    protected void PreInitCondition()
    {

        if (Session["RMAIssueID"] == null && Request.QueryString["IssueID"] != null)//第一次
        {
            Session["RMAIssueID"] = Request.QueryString["IssueID"];
        }

        if (Request.QueryString["IssueID"] != null && Session["RMAIssue_ActiveViewIndex"] == null)
        {
            Session["RMAIssueID"] = Request.QueryString["IssueID"];
        }

        if (Session["RMAIssue_ActiveViewIndex"] != null)
        {
            if (Session["RMAIssueID"] != null && Request.QueryString["IssueID"] != null)
            {
                string RMAIssueID = Session["RMAIssueID"].ToString().ToUpper();

                if (RMAIssueID != Request.QueryString["IssueID"].ToString().ToUpper())
                {
                    Session["RMAIssue_ActiveViewIndex"] = null;
                }
            }
        }
    }

    protected bool GetUserSite(string UserId, out DataSet dsSite)
    {
        bool blSucc = false;
        string vchCmd = "SELECT User Site";
        string vchObjectName = "v_userrole";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(UserId, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RoleMaintain, vchCmd, vchObjectName, vchSet.ToString());
        dsSite = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (dsSite.Tables.Count > 0)
            if (dsSite.Tables[0].Rows.Count <= 1 && dsSite.Tables[0].Rows.Count > 0)
            {
                blSucc = true;
            }

        return blSucc;
    }

    protected void InitCondition()
    {
        txtUserId.Text = Method.GetCurrentUserId(Page);
        txtDueDate.Text = System.DateTime.Today.ToString("yyyy/MM/dd");//AR的DueDate
        txtDate.Text = System.DateTime.Today.ToString("yyyy/MM/dd");//FailData History 的Date

        PreInitCondition();
        DataSet dsSite;

        if (Request.QueryString["IssueID"] == null)
        {
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
            string sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "CHECK", "PROJECTMEMBER", vchSet.ToString());
            string[] Role = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmd);
            if (Role[0] == "")
            {
                Response.Write("<script language='javascript'>alert('You are not belong to any ProjectOwner/Member')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
            else if (((DropDownList)fvIssue.FindControl("ddlSite")).Items.Count > 1)
            {
                bool blSucc = GetUserSite(txtUserId.Text, out dsSite);
                if (blSucc == false)
                {
                    string sMsg = "";
                    foreach (DataRow dr in dsSite.Tables[0].Rows)
                    {
                        sMsg = sMsg + dr["SiteName"].ToString() + " ";
                    }
                    sMsg = "You have one more Sites [" + sMsg + "] ,Please Contact Owens";
                    Response.Write("<script language='javascript'>alert('" + sMsg + "')</script>");
                    Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                }
                return;
            }
            else if (((DropDownList)fvIssue.FindControl("ddlProject")).Items.Count <= 1)
            {
                Response.Write("<script language='javascript'>alert('Project is in NPI Module')</script>");
                Response.Write("<script language='javascript'>window.opener=null;window.close();</script>");
                return;
            }
            else
            {
                lblSiteID.Text = ((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue;
                btnAdd.Visible = true;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                hlSQMP.Visible = true;
            }
        }
        else if (Request.QueryString["IssueID"] != null)
        {
            lblIssueID.Text = Request.QueryString["IssueID"];
            Session["RMAIssueID"] = lblIssueID.Text;
            //mvIssueForm.ActiveViewIndex = 0;
            if (Session["RMAIssue_ActiveViewIndex"] != null)
            {
                int imen = 0;
                int.TryParse(Session["RMAIssue_ActiveViewIndex"].ToString(), out imen);
                MenuFunction(imen);
            }
            else
            {
                MenuFunction(0);
            }

            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
            vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
            string sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "REPORTERANDOWNER", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

            //權限控制
            lblReporterBadgeCode.Text = ds.Tables[0].Rows[0]["Reporter"].ToString();//取得報Issue的人
            lblStatusCode.Text = ds.Tables[0].Rows[0]["Status"].ToString();//取得Issue的Status
            lblProjectID.Text = ds.Tables[0].Rows[0]["Project"].ToString();//取得Issue的Project
            hfProjectMaterial.Value = ds.Tables[0].Rows[0]["ProjectMaterial"].ToString();
            hfMType.Value = ds.Tables[0].Rows[0]["MType"].ToString();
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
                if (lblStatusCode.Text == "3")//結束後無法再做任何事
                {
                    ObjectInvalid();
                }
            }
            else
            {
                ObjectInvalid();
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
                string sqlCmdAR = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "RMAAR", vchSetAR.ToString());
                DataSet dsAR = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdAR);
                lblARBadgeCode.Text = dsAR.Tables[0].Rows[0]["AssignedTo"].ToString();//取得Issue的AR

                //SQE
                /*StringBuilder vchSetSQE = new StringBuilder();
                vchSetSQE.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSetSQE.Append(Method.BuildXML(lblARBadgeCode.Text, "editor"));
                string sqlCmdSQE = Method.GetSqlCmd(sp_RMAIssue, "CHECK", "SQEROLE", vchSetSQE.ToString());
                DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdSQE);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hlSQMP.Visible = true;
                    hlSQMP.NavigateUrl = "http://quality.inventec.com.cn/SRM_SERVER/op_Issue_Manage.aspx?user_id=" + txtUserId.Text + "&type=NEW&UserBU=EBG&UserRole=SQE";
                    //http://quality.inventec.com.cn/SRM_SERVER/op_Issue_Manage.aspx?user_id=IEC940480&type=NEW&UserBU=EBG&UserRole=SQE
                }
                */

                // if (dsAR.Tables[0].Rows[0]["Status"].ToString() != "3")

                //將所有值填入AR的欄位
                txtAssignedTo.Text = dsAR.Tables[0].Rows[0]["AssignedToName"].ToString();
                txtAssignedToBadgeCode.Text = dsAR.Tables[0].Rows[0]["AssignedTo"].ToString();
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
                    //ddlAssignedTo.Enabled = false;
                    Img3.Visible = false;
                    txtAssignedTo.ReadOnly = true;
                    btnAssignedTo.Visible = false;
                    txtActionDescription.ReadOnly = true;
                    txtDueDate.ReadOnly = true;
                    btnSendMail.Visible = false;
                    btnUpload.Visible = true;
                    btnUploadPhoto.Visible = true;
                    btnAddFD.Visible = false;
                    btnCancelFD.Visible = false;
                    btnRepeatDefect.Visible = false;
                    btnAddNote.Visible = true;

                    if (dsAR.Tables[0].Rows[0]["Status"].ToString() == "3")
                    {
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                    }
                }
                else//防一下轉寄給別人
                {
                    ObjectInvalid();
                }

                chkShowIssue.Checked = false;
                fvIssue.Visible = false;
            }//用於MyIssueAction和mail的連結

            IssueTitle();
            RepeatDefectCheck();
        }
    }

    protected void IssueTitle()
    {
        StringBuilder vchSetTitle = new StringBuilder();
        vchSetTitle.Append(Method.BuildXML(Request.QueryString["IssueID"], "IssueID"));
        string sqlCmdTitle = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "ISSUETITLE", vchSetTitle.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmdTitle);
        Page.Title = ds.Tables[0].Rows[0][0].ToString();
    }
    protected void ObjectInvalid()
    {
        fvIssue.ChangeMode(FormViewMode.ReadOnly);
        btnAssignedTo.Visible = false;
        btnAdd.Visible = false;
        btnSave.Visible = false;
        btnCancel.Visible = false;
        hlSQMP.Visible = false;
        btnSendMail.Visible = false;
        btnUpload.Visible = false;
        btnUploadPhoto.Visible = false;
        btnAddFD.Visible = false;
        btnCancelFD.Visible = false;
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
            sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "LISTISSUE", vchSet.ToString());
            DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

            if (fvIssue.CurrentMode == FormViewMode.Insert)
            {
                ((DropDownList)fvIssue.FindControl("ddlCustomer")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlCustomer")).SelectedValue = ds.Tables[0].Rows[0]["Customer"].ToString();
                ((Label)fvIssue.FindControl("lblReporterName")).Text = ds.Tables[0].Rows[0]["Reporter"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlSite")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue = ds.Tables[0].Rows[0]["Site"].ToString();

                lblSiteID.Text = ((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue;

                DropDownList ddlProject = (DropDownList)fvIssue.FindControl("ddlProject");
                DropDownList ddlProjectMaterial = (DropDownList)fvIssue.FindControl("ddlPCACPU");

                ddlProject.DataBind();
                ddlProject.SelectedValue = ds.Tables[0].Rows[0]["Project"].ToString();
                ddlProjectMaterial.DataBind();
                ddlProjectMaterial.SelectedValue = ds.Tables[0].Rows[0]["ProjectMaterial"].ToString();
                lblProjectID.Text = ddlProject.SelectedValue;

                string MType = fn_GetMType(ddlProject.SelectedValue, ddlProjectMaterial.SelectedValue);

                ((DropDownList)fvIssue.FindControl("ddlOwner")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlOwner")).SelectedValue = ds.Tables[0].Rows[0]["IssueOwner"].ToString();

                ((DropDownList)fvIssue.FindControl("ddlStatus")).SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlStation")).DataBind();
                ((DropDownList)fvIssue.FindControl("ddlStation")).SelectedValue = ds.Tables[0].Rows[0]["Station"].ToString();
                ((Label)fvIssue.FindControl("lblPendingDay")).Text = ds.Tables[0].Rows[0]["PendingDay"].ToString();
                ((Label)fvIssue.FindControl("lblFailedQty")).Text = ds.Tables[0].Rows[0]["FailureQty"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlReturnType")).SelectedValue = ds.Tables[0].Rows[0]["ReturnType"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlPriority")).SelectedValue = ds.Tables[0].Rows[0]["Priority"].ToString();
                ((DropDownList)fvIssue.FindControl("ddlLiability")).SelectedValue = ds.Tables[0].Rows[0]["Liability"].ToString();

                ((Label)fvIssue.FindControl("lblReturnSite")).Text = ds.Tables[0].Rows[0]["ReturnSite"].ToString();
                ((Label)fvIssue.FindControl("lblProduction")).Text = ds.Tables[0].Rows[0]["IDuration"].ToString();
                ((TextBox)fvIssue.FindControl("txtIssueDate")).Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                ((TextBox)fvIssue.FindControl("txtDueDate")).Text = ds.Tables[0].Rows[0]["DueDate"].ToString();
                ((Label)fvIssue.FindControl("lblRepeatDefectCheck")).Text = ds.Tables[0].Rows[0]["RelatedIssue"].ToString();
                ((TextBox)fvIssue.FindControl("txtDefectSymptom")).Text = ds.Tables[0].Rows[0]["DefectSymptom"].ToString();
                lblProjectID.Text = ((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue;

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
        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "REPEATDEFECTCHECK", vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRepeatDefectCheck.DataSource = ds.Tables[0];
            gvRepeatDefectCheck.DataBind();
            //MenuFunction(4);
        }
    }
    protected void menuAction_MenuItemClick(object sender, MenuEventArgs e)
    {
        int imen = int.Parse(e.Item.Value);
        Session["RMAIssue_ActiveViewIndex"] = imen;
        Session["RMAIssueID"] = Request.QueryString["IssueID"];
        MenuFunction(imen);
    }
    protected void MenuFunction(int index)
    {
        mvIssueForm.ActiveViewIndex = index;

        menuAction.Items[index].Selected = true;

    }
    protected void btnRepeatDefect_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
            Method.OpenWindows(Page, "RepeatDefect.aspx?IssueID=" + lblIssueID.Text, "RepeatDefect", 980);
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
    protected string fn_GetMType(string Project, string ProjectMaterial)
    {
        string result = "";

        string vchCmd = "SELECT";
        string vchObjectName = "fn_GetProjectMaterialType";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(Project, "Project"));
        vchSet.Append(Method.BuildXML(ProjectMaterial, "ProjectMaterial"));
        string sqlCmd = Method.GetSqlCmd(sp_RMAIssue, vchCmd, vchObjectName, vchSet.ToString());
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
        TextBox txtIssueDate = (TextBox)fvIssue.FindControl("txtIssueDate");
        
        if (ddlProject.SelectedValue == "" || ddlProjectMaterial.SelectedValue == ""
                ||((TextBox)fvIssue.FindControl("txtDefectSymptom")).Text == ""
                || ((TextBox)fvIssue.FindControl("txtIssueDate")).Text=="")
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
        DateTime IssueDate = DateTime.Parse(txtIssueDate.Text);
        check = InputCalCheck(txtIssueDate.Text);
        if (check == false)
        {
            Method.MessageOut(Page, "its format is yyyy/MM/dd");
            e.Cancel = true;
            return;
        }

        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        if (Request.QueryString["IssueID"] == null && lblIssueID.Text == "")
        {
            lblIssueID.Text = System.Guid.NewGuid().ToString();
        }
        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlCustomer")).SelectedValue, "Customer"));
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlSite")).SelectedValue, "Site"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "Reporter"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlOwner")).SelectedValue, "IssueOwner"));

        vchSet.Append(Method.BuildXML(ddlProjectMaterial.SelectedValue, "ProjectMaterial"));
        vchSet.Append(Method.BuildXML(MType, "MType"));
        vchSet.Append(Method.BuildXML(ddlProjectMaterial.SelectedValue, "MaterialType"));

        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlStatus")).SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlStation")).SelectedValue, "Station"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlPriority")).SelectedValue, "Priority"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlLiability")).SelectedValue, "Liability"));
        vchSet.Append(Method.BuildXML(((DropDownList)fvIssue.FindControl("ddlReturnType")).SelectedValue, "ReturnType"));
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtDefectSymptom")).Text.Trim(), "DefectSymptom"));
        vchSet.Append(Method.BuildXML(txtIssueDate.Text, "IssueDate"));
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
                vchSet.Append(Method.BuildXML(txtDueDate.Text, "DueDate"));
            }
        }
     
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtRootCause")).Text, "RootCause"));
        vchSet.Append(Method.BuildXML(((TextBox)fvIssue.FindControl("txtCorrectiveAction")).Text, "CorrectAction"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        vchSet = vchSet.Replace(@"'", @"`");

        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SAVE", "RMAISSUE", vchSet.ToString());
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
        fvIssue.DataBind();
        Method.MessageOut(Page, "Save Successfully!!");

        if (Request.QueryString["IssueID"] == null)
        {
            Response.Write("<script languge='javascript'>alert('Save Successfully!!'); window.location.href='RMAIssue.aspx?IssueID=" + lblIssueID.Text + "'</script>");
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
        string sqlCmdbcc = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "MAILBCC", "");
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
                   mail_body = GenerateBody(lblIssueID.Text, ((Label)gvr.FindControl("lblActionID")).Text, "Remind"));


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
    protected string GenerateBody(string IssueID, string ActionID, string Receiver)
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(IssueID, "IssueID"));
        vchSet.Append(Method.BuildXML(ActionID, "ActionID"));
        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SENDMAIL", "RMAAR", vchSet.ToString());

        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        string content = "";
        content += "<table width='90%' border='1' style='font-family:Tahoma'>";
        content += "<tr><td style='width: 20%; background-color: #b6cade;'><b>Project</b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'>" + ds.Tables[0].Rows[0]["ProjectName"].ToString() + "</td>";
        content += "<td style='WIDTH: 20%; background-color: #b6cade;'><b></b></td>";
        content += "<td style='WIDTH: 25%; background-color: #eff3f8;'></td></tr>";
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

        content += "<tr><td colspan='4'><p align='center'><b><a href ='" + Constant.S_WebSite + "RMAIssue.aspx?IssueID=" + ds.Tables[0].Rows[0]["IssueID"].ToString() + "";
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
                if (txtUserId.Text != lblReporterBadgeCode.Text && lblIssueOwnerBadgeCode.Text != "Y")
                {
                    Method.MessageOut(Page, "It cannot modify");
                    return;
                }
            }
            if (((Label)gvAR.SelectedRow.FindControl("lblAssignedToCode")).Text == txtUserId.Text
                //|| (txtUserId.Text == lblReporterBadgeCode.Text || txtUserId.Text == lblIssueOwnerBadgeCode.Text))
                || (txtUserId.Text == lblReporterBadgeCode.Text || lblIssueOwnerBadgeCode.Text == "Y"))
            {

                lblActionID.Text = gvAR.DataKeys[gvAR.SelectedIndex].Value.ToString();
                txtAssignedTo.Text = ((Label)gvAR.SelectedRow.FindControl("lblAssignedTo")).Text.ToString();
                txtAssignedToBadgeCode.Text = ((Label)gvAR.SelectedRow.FindControl("lblAssignedToCode")).Text.ToString();
                txtDueDate.Text = ((Label)gvAR.SelectedRow.FindControl("lblDueDate")).Text.ToString();
                txtPendingDay.Text = ((Label)gvAR.SelectedRow.FindControl("lblPD")).Text.ToString();
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
            Method.MessageOut(Page, "This ProjectMember is not exist");
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            if (txtFileDescr.Text.Trim() != "" && fufile.PostedFile.FileName != "")
            {
                string Ext = fufile.PostedFile.FileName.Substring(fufile.PostedFile.FileName.LastIndexOf(@".") + 1);

                bool check = true;

                check = CheckFileName(Ext.ToUpper().ToString(), "IssueFU");

                if (check == true)
                {
                    string FileName = FileUpload(fufile, lblIssueID.Text);
                    string[] Split = FileName.Split('.');

                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSet.Append(Method.BuildXML(txtFileDescr.Text.Trim(), "FileDescr"));
                    vchSet.Append(Method.BuildXML(FileName, "FileName"));
                    vchSet.Append(Method.BuildXML(fufile.PostedFile.ContentLength.ToString(), "FileSize"));
                    vchSet.Append(Method.BuildXML(Ext.ToUpper(), "FileExt"));
                    vchSet.Append(Method.BuildXML(Constant.S_WebSite + "UploadFile/" + lblIssueID.Text + @"/" + FileName, "FilePath"));
                    vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                    vchSet = vchSet.Replace(@"'", @"`");
                    sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "UPLOAD", "ATTACHFILE", vchSet.ToString());
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
                fu.PostedFile.SaveAs(Constant.S_FileRoot + folder + @"\" + backupFile);
            }
        }
        else
        {
            backupFile = "";
        }

        return backupFile;
    }
    private bool CheckFileName(string FileName, string Type)
    {
        lblExtType.Text = "";
        string vchCmd = "CHECK";
        string vchObjectName = "EXTTYPE";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(Type, "TYPE"));
        string sqlCmd = Method.GetSqlCmd(sp_RMAIssue, vchCmd, vchObjectName, vchSet.ToString());
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
                string Ext = fuphoto.PostedFile.FileName.Substring(fuphoto.PostedFile.FileName.LastIndexOf(@".") + 1);
                bool check = true;

                check = CheckFileName(Ext.ToUpper().ToString(), "IssuePH");

                if (check == true)
                {
                    string FileName = FileUpload(fuphoto, lblIssueID.Text);
                    string[] Split = FileName.Split('.');

                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSet.Append(Method.BuildXML(txtPhotoDesc.Text.Trim(), "FileDescr"));
                    vchSet.Append(Method.BuildXML(FileName, "FileName"));
                    vchSet.Append(Method.BuildXML(fuphoto.PostedFile.ContentLength.ToString(), "FileSize"));
                    vchSet.Append(Method.BuildXML(Ext.ToUpper(), "FileExt"));
                    vchSet.Append(Method.BuildXML(Constant.S_WebSite + "UploadFile/" + lblIssueID.Text + @"/" + FileName, "FilePath"));
                    vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                    vchSet = vchSet.Replace(@"'", @"`");
                    sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "UPLOAD", "PHOTO", vchSet.ToString());
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
                   || lblIssueOwnerBadgeCode.Text == "Y" || lblReporterBadgeCode.Text == txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfID")).Value.ToString(), "ID"));
                vchSet.Append(Method.BuildXML(((HyperLink)((GridView)sender).Rows[e.RowIndex].FindControl("hyFileDesc")).Text.ToString(), "FileDescr"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfFileName")).Value.ToString(), "FileName"));
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "DELETE", "ATTACHFILE", vchSet.ToString());
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
                       || lblIssueOwnerBadgeCode.Text == "Y" || lblReporterBadgeCode.Text == txtUserId.Text)
                {
                    StringBuilder vchSet = new StringBuilder();
                    string sqlCmd = "";
                    vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                    vchSet.Append(Method.BuildXML(((HiddenField)e.Item.FindControl("hfid")).Value, "ID"));
                    vchSet.Append(Method.BuildXML(((HiddenField)e.Item.FindControl("hfFileName")).Value, "FileName"));
                    vchSet.Append(Method.BuildXML(((Label)e.Item.FindControl("lblPhotoDescr")).Text, "FileDescr"));
                    sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "DELETE", "PHOTO", vchSet.ToString());
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
    
    protected void btnAddFD_Click(object sender, EventArgs e)
    {
        if (lblIssueID.Text != "")
        {
            bool checkDate = false;
            if (txtDate.Text.Trim() != "" && txtSerialNo.Text.Trim() != "" && txtOriginalDefect.Text.Trim() != ""
                        && txtFailQty.Text.Trim() != "" && txtFailureSymptom.Text.Trim() != "")
            {
                checkDate = InputCalCheck(txtDate.Text.Trim());
                if (checkDate == false)
                {
                    Method.MessageOut(Page, "its format is yyyy/MM/dd");
                    return;
                }

                try
                {
                    int FailQty;
                    FailQty = int.Parse(txtFailQty.Text.Trim());
                    if (FailQty >0)
                    {
                        StringBuilder vchSet = new StringBuilder();
                        string sqlCmd = "";
                        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                        if (gvFD.SelectedIndex == -1)
                        {
                            lblFailureDataID.Text = "";
                        }
                        vchSet.Append(Method.BuildXML(lblFailureDataID.Text, "FailureDataID"));
                        vchSet.Append(Method.BuildXML(txtDate.Text.Trim(), "FailureDate"));
                        vchSet.Append(Method.BuildXML(txtSerialNo.Text.Trim(), "SerialNo"));
                        vchSet.Append(Method.BuildXML(txtFailQty.Text.Trim(), "FailureQty"));
                        vchSet.Append(Method.BuildXML(ddlReturnSite.SelectedValue, "ReturnSite"));
                        vchSet.Append(Method.BuildXML(txtRmaNo.Text.Trim(), "RmaNo"));
                        vchSet.Append(Method.BuildXML(txtOriginalDefect.Text.Trim(), "OriginalDefect"));
                        vchSet.Append(Method.BuildXML(txtFailureSymptom.Text.Trim(), "FailureSymptom"));
                        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                        vchSet = vchSet.Replace(@"'", @"`");
                        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SAVE", "FAILUREDATA", vchSet.ToString());
                        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                        BindData(lblIssueID.Text);
                        gvFD.DataBind();
                        txtDate.Text = System.DateTime.Today.ToString("yyyy/MM/dd");//FailData History 的Date
                        txtFailQty.Text = "";
                        txtSerialNo.Text = "";
                        txtRmaNo.Text = "";
                        txtOriginalDefect.Text = "";
                        txtFailureSymptom.Text = "";
                        gvFD.SelectedIndex = -1;
                        UpdCondition();
                    }
                }
                catch
                {
                    Method.MessageOut(Page, "FailQty must digital!!");
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
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SAVE", "NOTE", vchSet.ToString());
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
                  || lblIssueOwnerBadgeCode.Text == "Y" || lblReporterBadgeCode.Text == txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblNoteID")).Text.ToString(), "NoteID"));
                vchSet.Append(Method.BuildXML(((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtNote")).Text.ToString(), "Note"));
                vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "UPDATE", "NOTE", vchSet.ToString());
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
               || lblIssueOwnerBadgeCode.Text == "Y" || lblReporterBadgeCode.Text == txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblNoteID")).Text.ToString(), "NoteID"));
                vchSet.Append(Method.BuildXML(((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblNote")).Text.ToString(), "Note"));
                vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "DELETE", "NOTE", vchSet.ToString());
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
        }

        lblProjectID.Text = ((DropDownList)fvIssue.FindControl("ddlProject")).SelectedValue;
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
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "DELETE", "REPEATDEFECTCHECK", vchSet.ToString());
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
                //((DropDownList)fvIssue.FindControl("ddlLiability")).SelectedValue = "8";
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
        //ddlAssignedTo.SelectedIndex = 0;
        txtDueDate.Text = "";
        txtPendingDay.Text = "";
        //txtFailQty.Text = "";
        txtActionDescription.Text = "";
        txtAssignedToBadgeCode.Text = "";
        txtCompletionComment.Text = "";
        txtAssignedTo.Text = "";
        CheckARObject(gvAR.SelectedIndex = -1);

    }
  
    protected void btnCancelFD_Click(object sender, EventArgs e)
    {
        txtDate.Text = "";
        txtFailQty.Text = "";
        txtSerialNo.Text = "";
        txtRmaNo.Text = "";
        txtOriginalDefect.Text = "";
        txtFailureSymptom.Text = "";
        gvFD.SelectedIndex = -1;        
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
            SendMailOwner(System.Guid.NewGuid().ToString(), "ADD");
        }
        else
        {
            Method.MessageOut(Page, "Please Report Issue First!!");
        }
    }
    protected void SendMailOwner(string ARID, string cmd)
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
            if (txtActionDescription.Text.Trim().Replace(" ", "") == "")
            {
                Method.MessageOut(Page, "Please Fill out ActionDescription ");
                return;
            }
        }
        if (ddlStatus.SelectedValue == "3")
        {
            if (txtCompletionComment.Text.Trim().Replace(" ", "") == "")
            {
                Method.MessageOut(Page, "Please Fill out CompletionComment");
                return;
            }
        }

        if (txtAssignedToBadgeCode.Text == "")
        {
            Method.MessageOut(Page, "Please Click Find User");
            return;
        }

        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
        vchSet.Append(Method.BuildXML(ARID, "ActionID"));
        vchSet.Append(Method.BuildXML(txtAssignedToBadgeCode.Text, "AssignedTo"));
        vchSet.Append(Method.BuildXML(ddlStatus.SelectedValue, "Status"));
        vchSet.Append(Method.BuildXML(txtDueDate.Text.Trim(), "DueDate"));
        vchSet.Append(Method.BuildXML(txtActionDescription.Text.Trim(), "ActionDescription"));
        vchSet.Append(Method.BuildXML(txtCompletionComment.Text.Trim(), "CompletionComment"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        vchSet = vchSet.Replace(@"'", @"`");
        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, cmd, "RMAAR", vchSet.ToString());

        string[] ActionRID = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmd);
        BindData(lblIssueID.Text);
        gvAR.DataBind();

        DataTable dtbcc = new DataTable();
        dtbcc.Columns.Add("BadgeCode");
        dtbcc.Columns.Add("email");
        string sqlCmdbcc = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "MAILBCC", "");
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
                       Method.GetUsereMailByUserId(txtAssignedToBadgeCode.Text),
                       Method.GetUsereMailByUserId(txtUserId.Text), dtbcc, mail_Subject,
              mail_body = GenerateBody(lblIssueID.Text, ARID, "Remind"));

            Method.MessageOut(Page, "Save Successfully!! Mail to " + Method.GetUserNameByeMail(Method.GetUsereMailByUserId(txtAssignedToBadgeCode.Text)) + mail_Subject);
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
        txtActionDescription.Text = "";
        txtCompletionComment.Text = "";
        txtAssignedTo.Text = "";
        txtAssignedToBadgeCode.Text = "";
        txtDueDate.Text = "";
        gvAR.SelectedIndex = -1;
        CheckARObject(-1);
    }
    protected void CheckARObject(int index)
    {
        if (index == -1)
        {
            btnSave.Visible = false;
            btnCancel.Visible = false;
            if (lblReporterBadgeCode.Text == txtUserId.Text || lblIssueOwnerBadgeCode.Text == "Y")
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
    protected void btnAssignedTo_Click(object sender, EventArgs e)
    {
        if (txtAssignedTo.Text.Trim() != "")
        {
            CheckName(txtAssignedTo.Text.Trim(), txtAssignedToBadgeCode, txtAssignedTo);
        }
        else
        {
            Method.MessageOut(Page, "Please Input Badge ID or User Name(full/partial) first");
        }

    }

    private void CheckName(string Search, TextBox BadgeCode, TextBox ChtName)
    {
        StringBuilder vchSet = new StringBuilder();
        string sqlCmd = "";
        vchSet.Append(Method.BuildXML(Search, "AssignedTo"));
        sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "CHECK", "GETUSER", vchSet.ToString());
        DataSet ds = new DataSet();
        ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);
        if (ds.Tables[0].Rows.Count == 0)
        {
            Method.MessageOut(Page, "No data list,Please try it again");

        }
        else if (ds.Tables[0].Rows.Count == 1)
        {
            BadgeCode.Text = ds.Tables[0].Rows[0]["BadgeCode"].ToString();
            ChtName.Text = ds.Tables[0].Rows[0]["ChtName"].ToString();
        }
        else
        {
            string linkUrl = @"ListMember.aspx?Query=";
            linkUrl += Search + "&BadgeCode=";
            linkUrl += BadgeCode.ClientID + "&ChtName=";
            linkUrl += ChtName.ClientID + "&postBack=";
            linkUrl += false;
            string sScript = @"<script language='javascript'>popUp=window.open('" + linkUrl + "','popupcal','height=400,width=600,resizable=yes,toolbar=no,location=no,status=yes,menubar=no,scrollbars=yes');</script>";
            Session["QueryUser"] = Search;
            Response.Write(sScript);
        }
    }
    public bool IsNumeric(string strVal)
    { //檢查是否數字
        bool check = true;
        try
        {
            if (strVal.IndexOf(',') > 0)
            {
                check = false;
            }
            else
            {
                double d = double.Parse(strVal);
            }
        }
        catch
        {
            check = false;
        }

        return check;
    }
    protected void BindData(string IssueID)
    {
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(IssueID, "IssueID"));
        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "SELECT", "REPORTERANDOWNER", vchSet.ToString());
        DataSet ds = DAO.sqlCmdDataSetSP(Constant.S_MQITSConnStr, sqlCmd);

        lblReporterBadgeCode.Text = ds.Tables[0].Rows[0]["Reporter"].ToString();//取得報Issue的人
        lblIssueOwnerBadgeCode.Text = ds.Tables[1].Rows[0]["ProjectOwner"].ToString();
    }
    protected void btnProjectMaterial_Click(object sender, EventArgs e)
    {
        DropDownList oDLL = (DropDownList)this.fvIssue.FindControl("ddlPCACPU");
        TextBox oTxt = (TextBox)this.fvIssue.FindControl("txtProjecMaterial");
        Button oBtn = (Button)this.fvIssue.FindControl("btnAddProjectMaterial");
        Button change = (Button)this.fvIssue.FindControl("btnProjectMaterial");

        if (oDLL.Visible == true)
        {
            oTxt.Visible = true;
            oBtn.Visible = true;
            oDLL.Visible = false;
            change.Text = "Cancel";
        }
        else
        {
            oTxt.Visible = false;
            oBtn.Visible = false;
            oDLL.Visible = true;
            change.Text = "＋";
        }

        oTxt.Text = oDLL.SelectedValue;

    }
    protected void btnAddProjectMaterial_Click(object sender, EventArgs e)
    {
        DropDownList ddlProject = (DropDownList)fvIssue.FindControl("ddlProject");
        TextBox txtProjectMaterial = (TextBox)this.fvIssue.FindControl("txtProjecMaterial");
        DropDownList ddlProjectMaterial = (DropDownList)fvIssue.FindControl("ddlPCACPU");
        Button btnAddProjectMaterial = (Button)fvIssue.FindControl("btnAddProjectMaterial");
        Button change = (Button)this.fvIssue.FindControl("btnProjectMaterial");
        change.Text = "＋";
        StringBuilder vchSet = new StringBuilder();
        vchSet.Append(Method.BuildXML(ddlProject.SelectedValue, "Project"));
        if (txtProjectMaterial.Text.Trim() == "" || txtProjectMaterial.Text.Length < 12)
        {
            Method.MessageOut(Page, "Please input Project Material and Material must 12 bits!");
            return;
        }
        else
        {
            vchSet.Append(Method.BuildXML(txtProjectMaterial.Text.ToUpper().Trim(), "ProjectMaterial"));
        }

        vchSet.Append(Method.BuildXML(txtUserId.Text, "editor"));
        string sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "ADD", "M_PROJECTMATERIAL", vchSet.ToString());
        string[] errmsg = DAO.sqlCmdArr(Constant.S_MQITSConnStr, sqlCmd);
        if (errmsg[0] == "")
        {
            ddlProjectMaterial.DataBind();

            ddlProjectMaterial.Visible = true;
            //ddlProjectMaterial.SelectedValue = txtProjectMaterial.Text;
            txtProjectMaterial.Visible = false;
            btnAddProjectMaterial.Visible = false;
        }
        else
        {
            Method.MessageOut(Page, errmsg[0]);
        }
    }

    protected void gvFD_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            if (((Label)((GridView)sender).Rows[e.RowIndex].FindControl("lblEditor")).Text == txtUserId.Text
                || lblIssueOwnerBadgeCode.Text == "Y" || lblReporterBadgeCode.Text == txtUserId.Text)
            {
                StringBuilder vchSet = new StringBuilder();
                string sqlCmd = "";
                vchSet.Append(Method.BuildXML(lblIssueID.Text, "IssueID"));
                vchSet.Append(Method.BuildXML(((HiddenField)((GridView)sender).Rows[e.RowIndex].FindControl("hfFailureID")).Value.ToString(), "FailureDataID"));
                sqlCmd = Method.GetSqlCmd(sp_RMAIssue, "DELETE", "FAILUREDATA", vchSet.ToString());
                DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);
                gvFD.DataBind();
                UpdCondition();
                ((GridView)fvIssue.FindControl("gvLocation")).DataBind();
                
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
    protected void gvFD_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            lblFailureDataID.Text = gvFD.DataKeys[gvFD.SelectedIndex].Value.ToString();
            txtDate.Text = ((Label)gvFD.SelectedRow.FindControl("lblFailureDate")).Text.ToString();
            txtFailQty.Text = ((Label)gvFD.SelectedRow.FindControl("lblFailureQty")).Text.ToString();
            txtSerialNo.Text = ((Label)gvFD.SelectedRow.FindControl("lblSN")).Text.ToString();
            ddlReturnSite.SelectedValue = ((Label)gvFD.SelectedRow.FindControl("lblReturnSite")).Text.ToString();
            txtRmaNo.Text = ((Label)gvFD.SelectedRow.FindControl("lblRMANo")).Text.ToString();
            txtOriginalDefect.Text=((Label)gvFD.SelectedRow.FindControl("lblOriginalDefect")).Text.ToString();
            txtFailureSymptom.Text = ((Label)gvFD.SelectedRow.FindControl("lblFailureSymptom")).Text.ToString();
        }
        else
        {
            gvFD.SelectedIndex = -1;
        }
    }
    protected void gvFD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (lblStatusCode.Text != "3")
        {
            if (e.CommandName == "AddLocation")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                string linkUrl = @"Location.aspx?IssueID=";
                linkUrl += lblIssueID.Text + "&FailureDataID=";
                linkUrl += ((HiddenField)((GridView)sender).Rows[rowIndex].FindControl("hfFailureID")).Value + "&LoginName=";
                linkUrl += txtUserId.Text;

                string sScript = @"<script language='javascript'>popUp=window.open('" + linkUrl + "','AddLocation','height=250,width=500,toolbar=no,location=no,resizable=yes,status=yes,menubar=no,scrollbars=yes,left=150,top=200');</script>";
                Response.Write(sScript);
            }
        }
        else
        {
            Method.MessageOut(Page, "This Issue is close,and it cannot add");
        }
    }
}
