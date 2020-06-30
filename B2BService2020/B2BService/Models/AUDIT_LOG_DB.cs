using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Reflection;

namespace B2BService.Models
{
    public interface IAUDITLOGDBCollection
    {
        IEnumerable<AUDIT_LOG_DB> Get(AUDIT_LOG_DB db);
    }

    public class AUDIT_LOG_DB
    {
        [Key]
        public decimal NO { get; set; }
        public string MSGID { get; set; }
        public OracleTimeStamp? LOG_TIME { get; set; }
        public OracleClob ERR_LOG_CONTENT { get; set; }
        public string LOG_LEVEL { get; set; }
    }

    public class AUDITLOGDBCollection
    {
        public IEnumerable<AUDIT_LOG_DB> AUDITLOGDBList;
        private string config = string.Empty;
        public AUDITLOGDBCollection(string config)
        {
            this.config = config;
        }

        public IEnumerable<AUDIT_LOG_DB> Get(AUDIT_LOG_DB db)
        {
            string sqlString = GetSqlQuery(db);
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];

            AUDITLOGDBList = ConvertToTankReadings(dt);
            return AUDITLOGDBList;
        }

        private IEnumerable<AUDIT_LOG_DB> ConvertToTankReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                OracleTimeStamp? ocLOG_TIME = string.IsNullOrEmpty(row["LOG_TIME"].ToString()) ? OracleTimeStamp.Null : (OracleTimeStamp)(row["LOG_TIME"]);
                OracleClob ocERR_LOG_CONTENT = string.IsNullOrEmpty(row["ERR_LOG_CONTENT"].ToString()) ? OracleClob.Null : (OracleClob)(row["ERR_LOG_CONTENT"]);
                yield return new AUDIT_LOG_DB
                {
                    NO = Convert.ToDecimal(row["NO"].ToString()),
                    MSGID = Convert.ToString(row["MSGID"]),
                    LOG_TIME = ocLOG_TIME,
                    ERR_LOG_CONTENT = ocERR_LOG_CONTENT
                };
            }
        }

        private int CheckIsNullOrEmpty(AUDIT_LOG_DB db)
        {
            int rslt = 0;

            foreach (PropertyInfo prop in db.GetType().GetProperties())
            {
                var varprop = db.GetType().GetProperty(prop.Name);

                if (varprop.PropertyType.Name == "String")
                {
                    rslt = string.IsNullOrEmpty(varprop.GetValue(db, null) as string) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.Name == "OracleClob")
                {
                    rslt = string.IsNullOrEmpty((varprop.GetValue(db, null) as OracleClob).Value) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.IsGenericType &&
                        varprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var propertyType = varprop.PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
                    if (propertyType.Name == "OracleTimeStamp")
                    {
                        rslt = ((varprop.GetValue(db, null) as OracleTimeStamp?).HasValue) ? rslt : ++rslt;
                    }
                }
            }
            //rslt = (mtdb.CREATEDATE.HasValue) ? rslt : rslt++;
            return rslt;
        }

        private string GetSqlQuery(AUDIT_LOG_DB db)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;
            select = "SELECT * FROM AUDIT_LOG_DB";
            if (CheckIsNullOrEmpty(db) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                sql.Append(" ORDER BY NO");
            }
            else
            {
                sbwhere.Append("WHERE"); sbwhere.Append(space);
                int icond = 0;

                foreach (PropertyInfo prop in db.GetType().GetProperties())
                {
                    var varprop = db.GetType().GetProperty(prop.Name);

                    if (varprop.PropertyType.Name == "String")
                    {
                        string value = varprop.GetValue(db, null) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            where = varprop.Name + " = '@'"; where = where.Replace("@", value);
                            if (icond > 0)
                            {
                                sbwhere.Append(space);
                                sbwhere.Append("AND");
                                sbwhere.Append(space);
                            }
                            sbwhere.Append(where);
                            icond++;
                        }
                    }

                    if (varprop.PropertyType.Name == "OracleClob")
                    {
                        string value = (varprop.GetValue(db, null) as OracleClob).Value;
                        if (!string.IsNullOrEmpty(value))
                        {
                            where = varprop.Name + " = '@'"; where = where.Replace("@", value);
                            if (icond > 0)
                            {
                                sbwhere.Append(space);
                                sbwhere.Append("AND");
                                sbwhere.Append(space);
                            }
                            sbwhere.Append(where);
                            icond++;
                        }
                    }

                    if (varprop.PropertyType.IsGenericType &&
                        varprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var propertyType = varprop.PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
                        if (propertyType.Name == "OracleTimeStamp")
                        {
                            OracleTimeStamp? value = varprop.GetValue(db, null) as OracleTimeStamp?;
                            if (value.HasValue)
                            {
                                where = varprop.Name + " = '@'"; where = where.Replace("@", value.Value.ToString());
                                if (icond > 0)
                                {
                                    sbwhere.Append(space);
                                    sbwhere.Append("AND");
                                    sbwhere.Append(space);
                                }
                                sbwhere.Append(where);
                                icond++;
                            }
                        }
                        
                    } // end foreach
                }
                where = "ROWNUM <= @"; where = where.Replace("@", 100.ToString());
                if (icond > 0)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                }
                sbwhere.Append(where);

                orderby = "ORDER BY NO";
                sql.Append(select);
                sql.Append(space);
                sql.Append(sbwhere.ToString());
                sql.Append(space);
                sql.Append(orderby);
            } // end if

            return sql.ToString();
        }
    }
}