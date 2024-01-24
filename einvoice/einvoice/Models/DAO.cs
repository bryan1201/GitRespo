using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace einvoice.Models
{
    public class DAO
    {
        private static string _connString = Constant.QASDBContext;
        private static string _devConnectionString = Constant.DEVDBContext;
        private static string _qasConnectionString = Constant.QASDBContext;
        private static string _prdConnectionString = Constant.PRDDBContext;

        public static void sqlCmd(string sql)
        {
            SqlConnection cn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public static DataTable sqlCmdDataTable(string sql)
        {
            SqlConnection cn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds, "datasetresult");
            DataTable dt = ds.Tables[0];

            return dt;
        }

        public static void sqlCmd(string sql, string option)
        {
            string connstring = GetConnectionEnvironment(option);

            SqlConnection cn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public static DataTable sqlCmdDataTable(string _connectionString, string sql)
        {
            string connstring = _connectionString;

            SqlConnection cn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds, "datasetresult");
            DataTable dt = ds.Tables[0];

            return dt;
        }

        private static string GetConnectionEnvironment(string option) {
            string connstring = string.Empty;
            switch (option)
            {
                case "DEV":
                    connstring = _devConnectionString;
                    break;
                case "QAS":
                    connstring = _qasConnectionString;
                    break;
                case "PRD":
                    connstring = _prdConnectionString;
                    break;
            }
            return connstring;
        }
    }
}