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
            OracleConnection conn = null;
            OracleTransaction dbTransaction = null;
            OracleCommand cmd = new OracleCommand();
            
            try
            {
                conn = new OracleConnection(config);
                conn.Open();
                dbTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = dbTransaction;
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1800;
                cmd.ExecuteNonQuery();
                dbTransaction.Commit();
            }
            catch(OracleException ex)
            {
                string message = ex.Message;

                if (dbTransaction != null)
                    dbTransaction.Rollback();

                throw new Exception(message, ex);
            }
            finally
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    //conn.Dispose();
                }
            }
            
        }

        public static DataSet oracleCmdDataSetSP(string config, string sql)
        {
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter adapter = null;
            DataSet ds = new DataSet();
            try
            {
                conn = new OracleConnection(config);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1800;
                adapter = new OracleDataAdapter(cmd);
                adapter.Fill(ds);
            }
            catch(OracleException ex)
            {
                string message = ex.Message;
                throw new Exception(message);
            }
            finally
            {
                if(adapter != null)
                {
                    adapter.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if(conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    //conn.Dispose();
                }
            }

            return ds;
        }

        public static OracleDataReader oracleCmdDataReaderSP(string config, string sql)
        {
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleDataReader reader = null;
            try
            {
                conn = new OracleConnection(config);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
            }
            catch(OracleException ex)
            {
                string message = ex.Message;
                throw new Exception(message);
            }
            finally
            {
                if(reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    //reader.Dispose();
                }
                if(cmd != null)
                {
                    cmd.Dispose();
                }
                if(conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    //conn.Dispose();
                }
            }
            
            return reader;
        }

        public static OracleClob StringToOracleClob(string config, string key)
        {
            OracleConnection conn = null;
            OracleClob clob = null;

            try
            {
                conn = new OracleConnection(config);
                conn.Open();
                clob = new OracleClob(conn);

                if (string.IsNullOrEmpty(key))
                {
                    char[] writeBuffer = string.Empty.ToCharArray();
                    clob.Write(writeBuffer, 0, writeBuffer.Length);
                }
                else
                {
                    char[] writeBuffer = key.ToCharArray();
                    clob.Write(writeBuffer, 0, writeBuffer.Length);
                }
            }
            catch(OracleException ex)
            {
                string message = ex.Message;
                throw new Exception(message);
            }
            finally
            {
                if(clob != null)
                {
                    if (!clob.IsEmpty || !clob.IsNull)
                        clob.Close();
                    //clob.Dispose();
                }
                if(conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    //conn.Dispose();
                }
            }

            return clob;
        }
    }
}