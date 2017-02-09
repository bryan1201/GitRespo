using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;
using System.Configuration;
using Oracle.DataAccess.Types;

namespace B2BService.Models
{
    public class DAO
    {
        public static void oracleCmdSP(string config, string sql)
        {
            OracleConnection conn = new OracleConnection(config);
            conn.Open();

            OracleTransaction dbTransaction;
            dbTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            OracleCommand cmd = new OracleCommand();
            cmd.Transaction = dbTransaction;
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1800;
                cmd.ExecuteNonQuery();
                dbTransaction.Commit();
            }
            catch(OracleException ex)
            {
                string Message = ex.Message;
                dbTransaction.Rollback();
                throw new Exception(ex.Message, ex);
            }

            conn.Close();
            conn.Dispose();
        }

        public static DataSet oracleCmdDataSetSP(string config, string sql)
        {
            OracleConnection conn = new OracleConnection(config);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 1800;
            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            conn.Close();
            conn.Dispose();

            return ds;
        }

        public static OracleDataReader oracleCmdDataReaderSP(string config, string sql)
        {
            OracleConnection conn = new OracleConnection(config);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            OracleDataReader reader = cmd.ExecuteReader();
            conn.Close();
            conn.Dispose();
            return reader;
        }

        public static OracleClob StringToOracleClob(string config, string key)
        {
            OracleConnection con = new OracleConnection(config);
            con.Open();
            OracleClob clob = new OracleClob(con);

            if (string.IsNullOrEmpty(key))
            {
                char[] writeBuffer = string.Empty.ToCharArray();
                clob.Write(writeBuffer, 0, writeBuffer.Length);
                clob.Close();
                clob.Dispose();
            }
            else
            {
                char[] writeBuffer = key.ToCharArray();
                clob.Write(writeBuffer, 0, writeBuffer.Length);
                clob.Close();
                clob.Dispose();
            }
            con.Close();
            con.Dispose();
            return clob;
        }
    }
}