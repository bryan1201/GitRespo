using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPage : System.Web.UI.Page
{
    static string CurrentPage = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["CurrentPage"] != null)
        {
            CurrentPage = Request["CurrentPage"].ToString();
        }

        if (!IsPostBack)
        {
            hlErrorPageMsg.InnerText=hlErrorPageMsg.InnerText.Replace("CurrentPage", CurrentPage);
        }
    }
}
