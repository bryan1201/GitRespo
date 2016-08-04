using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.IO;

using System.Web;

namespace Meetings.Controllers.General
{
    public class DAO
    {
        public static void sqlCmd(string config, string sql)
        {
            string ConnStr = ConfigurationManager.ConnectionStrings[config].ToString();
            SqlConnection cn = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
    }
}