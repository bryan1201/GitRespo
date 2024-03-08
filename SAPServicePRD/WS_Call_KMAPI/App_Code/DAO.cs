using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI;
using System.Collections;

	/// <summary>
	/// DAO 的摘要描述。
	/// </summary>
public class DAO
{
    public DAO()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }

    public static bool InjectionCheck(string str)
    {
        bool check = false;
        char[] injection = { '\'', ';' };

        if (str.IndexOfAny(injection) != -1)
        {
            check = true;
        }

        return check;
    }

    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql)
    { 
        
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();
    }


    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql, bool blCOne)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();
    }

    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql, string firstString)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();

        if (obj.Items.Count == 0)
        {
            obj.Items.Insert(0, new ListItem("-- no data found --", "-- select one --"));
        }
        else
        {
            obj.Items.Insert(0, firstString);
        }
    }

    public static void selListBox(System.Web.UI.WebControls.ListBox obj, string config, string sql)
    {

        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "listbox");

        obj.DataSource = ds.Tables["listbox"].DefaultView;
        obj.DataValueField = ds.Tables["listbox"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["listbox"].Columns[1].ToString();
        obj.DataBind();
        cn.Close();
    }

    public static void selCheckBoxList(System.Web.UI.WebControls.CheckBoxList obj, string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();

        if (obj.Items.Count == 0)
        {
            obj.Items.Insert(0, new ListItem("-- no data found --", "-- select one --"));
        }
    }

    public static void selDropDownList(System.Web.UI.WebControls.DropDownList obj, string config, string sql, string firstString, string addnew)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "dropdownlist");

        obj.DataSource = ds.Tables["dropdownlist"].DefaultView;
        obj.DataValueField = ds.Tables["dropdownlist"].Columns[0].ToString();
        obj.DataTextField = ds.Tables["dropdownlist"].Columns[1].ToString();
        obj.DataBind();

        if (obj.Items.Count == 0)
        {
            obj.Items.Insert(0, new ListItem("-- no data found --", "-- select one --"));
        }
        else
        {
            obj.Items.Insert(0, addnew);
            obj.Items.Insert(0, firstString);
        }
    }

    public static void fillDataGrid(System.Web.UI.WebControls.DataGrid dg, string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        dg.DataSource = ds.Tables["datasetresult"].DefaultView;
        dg.DataBind();
    }

    public static DataSet sqlCmdDataSet(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);

        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        da.FillSchema(ds, SchemaType.Source, "datasetresult");

        return ds;
    }

    public static DataSet sqlCmdDataSetSP(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand command = new SqlCommand(sql, cn);
        command.CommandTimeout = 300; //設為不會SQL timeout
        SqlDataAdapter da = new SqlDataAdapter(command);
        
        //SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        
        da.Fill(ds, "datasetresult");

        return ds;
    }

    public static DataSet sqlCmdDataSetSPNoTimeOut(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);

        //SqlDataAdapter da = new SqlDataAdapter(sql, cn);
        SqlCommand command = new SqlCommand(sql, cn);
        command.CommandTimeout = 300; //設為不會SQL timeout
        SqlDataAdapter da = new SqlDataAdapter(command);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        return ds;
    }

    public static void sqlCmd(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(sql, cn);
        cn.Open();
        cmd.CommandTimeout = 300;
        cmd.ExecuteNonQuery();
        cn.Close();
    }

    public static ArrayList SelCmdArr(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);

        SqlDataAdapter da = new SqlDataAdapter(sql, cn);

        DataSet ds = new DataSet();
        da.Fill(ds, "datasetresult");

        ArrayList alist = new ArrayList();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            alist.Add(dr[0].ToString());
        }
        return alist;
    }

    public static String[] sqlCmdArr(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cn.Open();

        SqlDataReader dr = cmd.ExecuteReader();

        String[] arrSQL = new string[40];

        if (dr.Read())
        {
            dr.GetValues(arrSQL);
        }
        else
        {
            for (int i = 0; i < 40; i++)
            {
                arrSQL[i] = "";
            }
        }

        cn.Close();
        return arrSQL;
    }

    public static String[] sqlCmdArrSingleCol(string config, string sql)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cn.Open();

        SqlDataReader dr = cmd.ExecuteReader();

        String[] arrSQL = new string[100];

        int x = 0;

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                arrSQL[x] = dr.GetString(0);
                x++;
            }
        }

        cn.Close();
        return arrSQL;
    }

    public static void addTableRowCell(System.Web.UI.WebControls.TableRow obj, string addStrings)
    {
        TableCell tc = new TableCell();

        tc.HorizontalAlign = HorizontalAlign.Center;
        tc.Text = addStrings;
        obj.Cells.Add(tc);
    }

    public static bool Checkid(string config, string user_id)
    {
        string ConnStr = ConfigurationSettings.AppSettings[config];
        string TSQL = "select * from m_user  where user_id = '" + user_id + "'";
        SqlConnection cn = new SqlConnection(ConnStr);
        SqlCommand cmd = new SqlCommand(TSQL, cn);

        if (cn.State.ToString() == "Open")
            cn.Close();
        else
            cn.Open();

        SqlDataReader dr = cmd.ExecuteReader();

        if (!dr.Read())
        { return false; }
        else
        { return true; }
    }
    public void addTableRowCell(System.Web.UI.WebControls.TableRow obj, string butStrings, int addrow)
    {
        TableCell tc = new TableCell();

        tc.HorizontalAlign = HorizontalAlign.Center;
        tc.Text = "<input align='center' runat='server' type='button' id='butAction' style='font-family:arial; font-size:9px' value='" + butStrings + "' onchange='butAction(" + addrow + ")' />";

        obj.Cells.Add(tc);
    }

    public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
    {
        DataTable dt = new DataTable(TableName);
        dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

        string accumulatedValue = "";
        foreach (DataRow dr in SourceTable.Select("", FieldName))
        {
            if (accumulatedValue.IndexOf(dr[FieldName].ToString()) == -1)
            {
                accumulatedValue += dr[FieldName].ToString();
                dt.Rows.Add(new object[] { dr[FieldName].ToString() });
            }
        }
        return dt;
    }
    public static string AddDataTableToDB(string config, DataTable dt, string table)
    {
        string ConnStr = ConfigurationManager.AppSettings[config];
        SqlTransaction tran = null;//宣告Transaction
        try
        {
            using (SqlConnection myCon = new SqlConnection(ConnStr))
            {
                myCon.Open();
                using (tran = myCon.BeginTransaction())
                {
                    using (SqlBulkCopy sqlBC = new SqlBulkCopy(myCon, SqlBulkCopyOptions.Default, tran))
                    {
                        sqlBC.DestinationTableName = table;  //資料表名稱
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlBC.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlBC.BulkCopyTimeout = 1000;
                        sqlBC.WriteToServer(dt);         //寫入DB
                        tran.Commit();                   //送出交易
                        myCon.Close();
                        return "TRUE";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (null != tran)
            {
                tran.Rollback(); //發生錯誤時 ; 取消交易
            }
            string Error = ex.ToString().Replace("'", "").Replace("\r\n", "\\r\\n");
            string Msg = "上傳失敗 !!\\n\\r\\n\\r" + Error.Substring(0, 500) + "...\\n\\r\\n\\r請洽系統管理員";
            return Msg;
        }
    }
}