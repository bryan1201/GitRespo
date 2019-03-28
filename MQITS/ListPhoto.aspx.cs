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

public partial class ListPhoto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void rpPhoto_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)(e.Item.FindControl("imgq1"));
        img.Attributes.Add("onmouseover", "this.style.cursor='hand'");
        img.Attributes.Add("onclick", "window.open('" + img.ImageUrl.ToString() + "');'height=600,width=800,toolbar=no,location=no,status=yes,menubar=no,resizable=yes,left=100,top=0'");
    }
}
