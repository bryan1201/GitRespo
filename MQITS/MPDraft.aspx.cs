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

public partial class MPDraft : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void fvIssue_DataBound(object sender, EventArgs e)
    {
        if (fvIssue.CurrentMode == FormViewMode.ReadOnly)
        {
            if (((Label)fvIssue.FindControl("lblLocationName")).Text != "")
            {
                ((Panel)fvIssue.FindControl("panelPCA")).Visible = true;
            }
            if (((Label)fvIssue.FindControl("lblFaultyCommodityName")).Text != "")
            {
                ((Panel)fvIssue.FindControl("panelCPU")).Visible = true;
            }
            if (((Label)fvIssue.FindControl("lblMaterialTypeName")).Text == "CPU")
            {
                ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect";
            }
            else
            {
                ((Label)fvIssue.FindControl("lblDefectSympton")).Text = "Defect Sympton";
            }

        }
    }
}
