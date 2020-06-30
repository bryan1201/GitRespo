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

    public interface IPROCESSDBCollection
    {
        IEnumerable<vProcessDB> Get(PROCESS_DB db);
        IEnumerable<vProcessDB> Get(string msgid);
    }

    public class PROCESS_DB
    {
        [Key]
        public decimal NO { get; set; }
        public string MSGID { get; set; }
        public DateTime? DATETIME { get; set; }
        public decimal? STEP { get; set; }
        public decimal? STATUS { get; set; }
    }

    public class vProcessDB : PROCESS_DB
    {
        public string ProcessStep { get; set; }
        public string ProcessStepRemark { get; set; }
        public string ProcessStatus { get; set; }
        public string ProcessStatusRemark { get; set; }
    }

    public class PROCESSDBCollection
    {
        public IEnumerable<vProcessDB> PROCESSDBList;
        private string config = string.Empty;
        
        public PROCESSDBCollection(string config)
        {
            this.config = config;
        }

        public IEnumerable<vProcessDB> Get(PROCESS_DB db)
        {
            string sqlString = GetSqlQuery(db);
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];

            PROCESSDBList = ConvertToTankReadings(dt);
            return PROCESSDBList;
        }

        private IEnumerable<vProcessDB> ConvertToTankReadings(DataTable dataTable)
        {
            Constant.Init();
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime? dtDATETIME = string.IsNullOrEmpty(row["DATETIME"].ToString()) ? (DateTime?)null : DateTime.Parse(row["DATETIME"].ToString());
                string strStep = row["STEP"].ToString();
                string strStatus = row["STATUS"].ToString();
                LOOKUP_DB vpdb = Constant.LookupMTProcessStep.Where(x => x.CODE == strStep).FirstOrDefault();
                string strProcessStep = vpdb.DESCRIPTION;
                string strProcessStepRemark = vpdb.REMARK;
                LOOKUP_DB vmtps = Constant.LookupMTPROCStatus.Where(x => x.CODE == strStatus).FirstOrDefault();
                string strProcessStatus = vmtps.DESCRIPTION;
                string strProcessStatusRemark = vmtps.REMARK;

                yield return new vProcessDB
                {
                    NO = Convert.ToDecimal(row["NO"].ToString()),
                    MSGID = Convert.ToString(row["MSGID"]),
                    DATETIME = dtDATETIME,
                    STEP = Convert.ToDecimal(strStep),
                    STATUS = Convert.ToDecimal(strStatus),
                    ProcessStep = strProcessStep,
                    ProcessStepRemark = strProcessStepRemark,
                    ProcessStatus = strProcessStatus,
                    ProcessStatusRemark = strProcessStatusRemark
                };
            }
        }

        private int CheckIsNullOrEmpty(PROCESS_DB db)
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

        private string GetSqlQuery(PROCESS_DB db)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;
            select = "SELECT * FROM PROCESS_DB";
            if (CheckIsNullOrEmpty(db) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                sql.Append(" WHERE 1=0 AND ORDER BY DATETIME");
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
                            where = varprop.Name + " = '@'"; where = where.Replace("@", value.Trim());
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

                    if (varprop.PropertyType.Name == "Decimal")
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

                        if (propertyType.Name == "Decimal")
                        {
                            decimal? value = varprop.GetValue(db, null) as decimal?;
                            if (value.HasValue)
                            {
                                where = varprop.Name + " = @"; where = where.Replace("@", value.Value.ToString());
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
                where = "ROWNUM <= @"; where = where.Replace("@", 500.ToString());
                if (icond > 0)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                }
                sbwhere.Append(where);

                orderby = "ORDER BY DATETIME ASC";
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