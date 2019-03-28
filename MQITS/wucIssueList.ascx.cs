using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Security;

public partial class wucIssueList : System.Web.UI.UserControl
{
    private static string ugroupissueId;
    [Personalizable]
    public string GroupIssueID
    {
        get
        {
            return ugroupissueId;
        }
        set
        {
            ugroupissueId = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GroupIssueID != null)
        {
            hfIssueGroupID.Value = GroupIssueID;
            gvIssueList.DataBind();
        }
    }
}
