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

public partial class user_logon : System.Web.UI.Page
{
    const string STPROC = "sp_UserLogon";
    const string FrontPage = "Summary.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            /*
            if (Request["nt_user"] != null)
            {
                Response.Write("nt_user" + Request["nt_user"].ToString());
                txtUserId.Text = Request["nt_user"].ToString().ToUpper();
                ButEnter_Click(null, null);
            }
             * */
            TextMsg.Text = "";
            this.divChangePWD.Visible = false;
            lblWebSiteName.Text = Resources.ResourceENG.WebSiteName;
        }
    }

    protected void ButEnter_Click(object sender, System.EventArgs e)
    {
        Response.Redirect(FrontPage);
        /*
        if (txtUserId.Text == "")
        {
            cuvUserPWD.IsValid = false;
            return;
        }
        else
        {
            txtUserId.Text = txtUserId.Text.Trim().ToUpper();
            Method.SetLogonUser(Page, txtUserId.Text);
            string vchCmd = "VALIDATE";
            string vchObjectName = "M_USER";
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(txtUserId.Text, "UserId"));
            vchSet.Append(Method.BuildXML(txtPWD.Text, "UserPWD"));
            string sqlCmd = Method.GetSqlCmd(STPROC, vchCmd, vchObjectName, vchSet.ToString());
            string[] IsPass = { "PASS" };
            IsPass = DAO.sqlCmdArrSingleCol(Constant.S_MQITSConnStr, sqlCmd);
            if (IsPass[0] == "PASS")
                Response.Redirect(FrontPage);
            else
            {
                cuvUserPWD.IsValid = false;

                //cuvUserPWD.Validate();
                return;
            }
        }
         */

    }

    protected bool UpdateUserPWD()
    {
        string user_id = txtUserId2.Text.Trim().ToUpper();
        this.txtUserId.Text = user_id;
        if (user_id == "")
        {
            this.cuvUserPWD.IsValid = false;
            this.cuvUserPWD.Text = "空白，請重新輸入！";
            return false;
        }

        string sqlCmd = "SELECT user_id, user_name, user_pwd FROM M_USER WHERE user_id = '" + user_id + "'";
        DataSet dsUser = DAO.sqlCmdDataSet(Constant.S_MQITSConnStr, sqlCmd);
        if (dsUser.Tables[0].Rows.Count == 0)
        {
            this.cuvOldPWD.IsValid = false;
            return false;
        }

        DataRow dr = dsUser.Tables[0].Rows[0];
        string user_name = dr["user_name"].ToString();
        string user_pwd = dr["user_pwd"].ToString();

        if (user_pwd != this.txtOldPWD.Text.Trim())
        {
            this.cuvOldPWD.IsValid = false;
            return false;
        }

        if (this.txtNewPWD1.Text.Trim() != this.txtNewPWD2.Text.Trim())
        {
            this.cpvNewPWD.IsValid = false;
            return false;
        }

        if (this.txtNewPWD1.Text.Trim() == "")
        {
            this.rfvNewPWD1.IsValid = false;
            return false;
        }

        if (this.txtNewPWD2.Text.Trim() == "")
        {
            this.rfvNewPWD2.IsValid = false;
            return false;
        }

        string vchCmd = "UPDATE_M_USER";
        string vchObjectName = "PASSWORD";

        string vchSet = "<BadgeCode>" + dr["user_id"].ToString() + "</BadgeCode><UserPWD>" + this.txtNewPWD1.Text.Trim() + "</UserPWD>";

        sqlCmd = "EXEC sp_UserProfile @vchCmd = '" + vchCmd + "', @vchObjectName = '" + vchObjectName + "', @vchSet = '" + vchSet + "'";
        DAO.sqlCmd(Constant.S_MQITSConnStr, sqlCmd);

        string sMessage = "使用者：[" + user_name + "]，帳號：[" + user_id + "]的密碼變更成功！請用新密碼重新登入。";

        Method.MessageOut(Page, sMessage);
        return true;
    }
    protected void ChangePWD_Click(object sender, EventArgs e)
    {
        this.divSubmit.Visible = false;
        this.divChangePWD.Visible = true;
        this.txtUserId2.Text = this.txtUserId.Text.Trim().ToUpper();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        bool blSuc = this.UpdateUserPWD();

        if (blSuc == true)
        {
            this.divChangePWD.Visible = false;
            this.divSubmit.Visible = true;
        }
    }
    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        this.divChangePWD.Visible = false;
        this.divSubmit.Visible = true;
        rfvNewPWD1.IsValid = true;
        rfvNewPWD2.IsValid = true;
    }
}
