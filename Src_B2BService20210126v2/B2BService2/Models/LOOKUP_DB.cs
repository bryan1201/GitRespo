using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Types;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Reflection;

namespace B2BService.Models
{
    public interface ILOOKUPDBCollection
    {
        IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db);
    }

    public class LOOKUP_DB
    {
        public string CODE { get; set; }
        public string TYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARK { get; set; }
    }

    public class LOOKUPDBCollection
    {
        public IEnumerable<LOOKUP_DB> LOOKUPDBList;
        private string config = string.Empty;
        public LOOKUPDBCollection(string config)
        {
            this.config = config;
        }

        public IEnumerable<LOOKUP_DB> Get(LOOKUP_DB db)
        {
            string sqlString = GetSqlQuery(db);
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];

            LOOKUPDBList = ConvertToTankReadings(dt);
            return LOOKUPDBList;
        }

        private IEnumerable<LOOKUP_DB> ConvertToTankReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new LOOKUP_DB
                {
                    CODE = Convert.ToString(row["CODE"]),
                    TYPE = Convert.ToString(row["TYPE"]),
                    DESCRIPTION = Convert.ToString(row["DESCRIPTION"]),
                    REMARK = Convert.ToString(row["REMARK"])
                };
            }
        }

        private int CheckIsNullOrEmpty(LOOKUP_DB db)
        {
            int rslt = 0;

            foreach (PropertyInfo prop in db.GetType().GetProperties())
            {
                var varprop = db.GetType().GetProperty(prop.Name);

                if (varprop.PropertyType.Name == "String")
                {
                    rslt = string.IsNullOrEmpty(varprop.GetValue(db, null) as string) ? rslt : ++rslt;
                }
            }
            //rslt = (mtdb.CREATEDATE.HasValue) ? rslt : rslt++;
            return rslt;
        }

        private string GetSqlQuery(LOOKUP_DB db)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;
            select = "SELECT * FROM LOOKUP_DB";
            if (CheckIsNullOrEmpty(db) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                sql.Append(" ORDER BY CODE");
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
                } // end foreach

                where = "ROWNUM <= @"; where = where.Replace("@", 100.ToString());
                if (icond > 0)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                }
                sbwhere.Append(where);

                orderby = "ORDER BY CODE";
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