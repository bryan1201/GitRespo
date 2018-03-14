using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ConsoleApplication1
{
    public class DAO
    {
        public static void sqlCmd(string config, string sql)
        {
            try
            {
                string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings[config].ConnectionString;
                SqlConnection cn = new SqlConnection(ConnStr);
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandTimeout = 1800;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        public static void DatatableToSQL(string config, System.Data.DataTable srcTable, string destTable)
        {
            
            string ss = System.Configuration.ConfigurationManager.ConnectionStrings[config].ConnectionString;
            try
            {
                SqlConnection cn = new SqlConnection(ss);
                cn.Open();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(cn);
                bulkcopy.DestinationTableName = destTable;
                try
                {
                    bulkcopy.WriteToServer(srcTable);
                }
                catch (Exception ex)
                {
                    throw(ex);
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }
    }
}
