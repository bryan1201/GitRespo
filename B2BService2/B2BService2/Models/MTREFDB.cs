using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;

namespace B2BService.Models
{
    public abstract class absMT_REF
    {
        [Key]
        public string REFID { get; set; }

        public string PARTNER { get; set; }
        public string DIVISION { get; set; }
        public string REGION { get; set; }
        public string ISASENDERID { get; set; }
        public string ISARECEIVERID { get; set; }
        public string GSSENDERID { get; set; }
        public string EDIMSGTYPE { get; set; }
    }

    public class MT_REF_DB : absMT_REF, IMTRef
    {
        private string config = string.Empty;
        private IEnumerable<MT_REF_DB> mt_refdb;

        public MT_REF_DB()
        {
            this.config = "PIQ";
            mt_refdb = QUERYREFDB();
        }

        public MT_REF_DB(string config)
        {
            this.config = config;
            mt_refdb = QUERYREFDB();
        }

        private HttpResponseMessage retJsonMessage(IEnumerable<string> listItems)
        {
            string json = JsonConvert.SerializeObject(listItems, Formatting.Indented);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        public object GetPARTNER(ServiceType Type)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb =
                (from mt in mt_refdb
                 orderby mt.PARTNER
                 select new { PARTNER = mt.PARTNER.Trim().ToUpper() })
                .Select(x => x.PARTNER).Distinct();

            if (Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public object GetDIVISION(ServiceType Type, string partner)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb =
                (from mt in mt_refdb
                 where mt.PARTNER == partner.ToUpper().Trim()
                 orderby mt.DIVISION
                 select new { DIVISION = mt.DIVISION.Trim().ToUpper() })
                 .Select(x => x.DIVISION).Distinct();

            if (Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public object GetREGION(ServiceType Type, string partner, string division)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb =
                (from mt in mt_refdb
                 where mt.PARTNER == partner.Trim().ToUpper() && mt.DIVISION == division.Trim().ToUpper()
                 orderby mt.REGION
                 select new { REGION = mt.REGION.Trim().ToUpper() })
                 .Select(x => x.REGION).Distinct();

            if (Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public object GetISASENDERID(ServiceType Type, string partner, string division, string region)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb = mt_refdb
                .Where(x => x.PARTNER.Trim().ToUpper() == partner.Trim().ToUpper() 
                    && x.DIVISION.Trim().ToUpper() == division.Trim().ToUpper() 
                    && x.REGION.Trim().ToUpper() == region.Trim().ToUpper())
                .Select(x => x.ISASENDERID.Trim()).Distinct();
            if(Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public object GetISARECEIVERID(ServiceType Type, string partner, string division, string region)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb = mt_refdb
                .Where(x => x.PARTNER.Trim().ToUpper() == partner.Trim().ToUpper()
                    && x.DIVISION.Trim().ToUpper() == division.Trim().ToUpper()
                    && x.REGION.Trim().ToUpper() == region.Trim().ToUpper())
                .Select(x => x.ISARECEIVERID.Trim()).Distinct();
            if(Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public object GetGSSENDERID(ServiceType Type, string partner, string division, string region)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb = mt_refdb
                .Where(x => x.PARTNER.Trim().ToUpper() == partner.Trim().ToUpper()
                    && x.DIVISION.Trim().ToUpper() == division.Trim().ToUpper()
                    && x.REGION.Trim().ToUpper() == region.Trim().ToUpper())
                .Select(x => x.GSSENDERID.Trim()).Distinct();
            if(Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public object GetEDIMSGTYPE(ServiceType Type, string partner, string division, string region)
        {
            HttpResponseMessage result;
            IEnumerable<string> resultdb = mt_refdb
                .Where(x => x.PARTNER.Trim().ToUpper() == partner.Trim().ToUpper()
                    && x.DIVISION.Trim().ToUpper() == division.Trim().ToUpper()
                    && x.REGION.Trim().ToUpper() == region.Trim().ToUpper())
                .Select(x => x.EDIMSGTYPE.Trim()).Distinct();
            if(Type == ServiceType.Json)
                result = retJsonMessage(resultdb);
            return resultdb;
        }

        public IEnumerable<MT_REF_DB> QUERYREFDB()
        {
            DateTime dtEnd = DateTime.Now;
            DateTime dtFrom = dtEnd.AddHours(-23.0);

            string sqlString = "SELECT * FROM MT_REF";
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];
            IEnumerable<MT_REF_DB> dresult = ConvertToDB_Readings(dt);

            return dresult;
        }

        private IEnumerable<MT_REF_DB> ConvertToDB_Readings(DataTable dataTable)
        {
            //TYPEID, CUSTOMER, EDIMSGTYPE, UDTIME, "USER"
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new MT_REF_DB(this.config)
                {
                    REFID = Convert.ToString(row["REFID"]),
                    PARTNER = Convert.ToString(row["PARTNER"]),
                    DIVISION = Convert.ToString(row["DIVISION"]),
                    REGION = Convert.ToString(row["REGION"]),
                    ISASENDERID = Convert.ToString(row["ISASENDERID"]),
                    ISARECEIVERID = Convert.ToString(row["ISARECEIVERID"]),
                    GSSENDERID = Convert.ToString(row["GSSENDERID"]),
                    EDIMSGTYPE = Convert.ToString(row["EDIMSGTYPE"])
                };
            }
        }
    }

    public enum ServiceType
    {
        List,
        Json
    }

    public interface IMTREFDBCollection
    {
        IEnumerable<MT_REF_DB> Get(MT_REF_DB mtrefdb);
        string GetSqlString();
    }

    public class MTREFDBCollection
    {
        public IEnumerable<MT_REF_DB> MTREFDBList;
        private string SqlString = string.Empty;
        private string config = string.Empty;
        public MTREFDBCollection(string config)
        {
            this.config = config;
        }

        private int CheckIsNullOrEmpty(MT_REF_DB mtrefdb)
        {
            int rslt = 0;

            foreach (PropertyInfo prop in mtrefdb.GetType().GetProperties())
            {
                var varprop = mtrefdb.GetType().GetProperty(prop.Name);

                if (varprop.PropertyType.Name == "String")
                {
                    rslt = string.IsNullOrEmpty(varprop.GetValue(mtrefdb, null) as string) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.Name == "Int32")
                {
                    rslt = ((varprop.GetValue(mtrefdb, null) as int?).HasValue) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.Name == "DateTime")
                {
                    rslt = ((varprop.GetValue(mtrefdb, null) as DateTime?).HasValue) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.IsGenericType &&
                        varprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var propertyType = varprop.PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
                    if (propertyType.Name == "DateTime")
                    {
                        DateTime? value = varprop.GetValue(mtrefdb, null) as DateTime?;
                        rslt = (!value.HasValue) ? rslt : ++rslt;
                    }

                    if (propertyType.Name == "Int32")
                    {
                        int? value = varprop.GetValue(mtrefdb, null) as int?;
                        rslt = (!value.HasValue) ? rslt : ++rslt;
                    }

                    if (propertyType.Name == "Decimal")
                    {
                        decimal? value = varprop.GetValue(mtrefdb, null) as decimal?;
                        rslt = (!value.HasValue) ? rslt : ++rslt;
                    }
                }
            }
            //rslt = (mtdb.CREATEDATE.HasValue) ? rslt : rslt++;
            return rslt;
        }

        private bool IsStringArray(string value)
        {
            bool rslt = false;
            value = value.Trim();
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = value.Split(stringSeparators, StringSplitOptions.None);
            if (lines.Count() > 0)
                rslt = true;
            return rslt;
        }

        private string ConvertToStringArray(string value, string Name)
        {
            string outrslt = value.Trim();
            StringBuilder rslt = new StringBuilder();
            value = value.Trim();
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = value.Split(stringSeparators, StringSplitOptions.None);
            if (lines.Count() > 0)
            {
                foreach (string item in lines)
                {
                    string itemvalue = item.Trim();
                    if (Name == "CONTROLNUM" || Name == "IDOC")
                    {
                        int ivalue = 0;
                        itemvalue = int.TryParse(itemvalue, out ivalue) ? ivalue.ToString("000000000") : itemvalue;
                    }

                    string inner = "'@',";
                    inner = inner.Replace("@", itemvalue);
                    rslt.Append(inner);
                }
                outrslt = rslt.ToString().Substring(0, rslt.ToString().Length - 1);
            }
            else
            {
                if (Name == "CONTROLNUM" || Name == "IDOC")
                {
                    int ivalue = 0;
                    outrslt = int.TryParse(outrslt, out ivalue) ? ivalue.ToString("000000000") : outrslt;
                }
            }

            return outrslt;
        }

        private string GetSqlQuery(MT_REF_DB mtrefdb)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;
            
            
            select = @"SELECT /*+" + Constant.TSQL_HINT + @"*/ * FROM MT_REF";
            if (CheckIsNullOrEmpty(mtrefdb) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                //TO_DATE('2016/5/18 00:50:00','yyyy/MM/dd HH24:MI:SS')
                sql.Append("WHERE ROWNUM=0 AND NVL(PARTNER, ' ') != ' '");
            }
            else
            {
                sbwhere.Append("WHERE"); sbwhere.Append(space);
                int icond = 0;

                foreach (PropertyInfo prop in mtrefdb.GetType().GetProperties())
                {
                    var varprop = mtrefdb.GetType().GetProperty(prop.Name);

                    if (varprop.PropertyType.Name == "String")
                    {
                        string value = varprop.GetValue(mtrefdb, null) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            string outstring = ConvertToStringArray(value, varprop.Name);
                            where = varprop.Name + " IN (@)"; where = where.Replace("@", outstring);
                            if (icond > 0)
                            {
                                sbwhere.Append(space);
                                sbwhere.Append("AND");
                                sbwhere.Append(space);
                            }
                            sbwhere.Append(where);
                            icond++;
                        }
                    } // end if

                } // end foreach

                //where = "ROWNUM <= @"; where = where.Replace("@", 100.ToString());
                where = "1=1";

                if (icond > 0)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                }
                sbwhere.Append(where);

                orderby = "ORDER BY 1 ASC";
                sql.Append(select);
                sql.Append(space);
                sql.Append(sbwhere.ToString());
                sql.Append(space);
                sql.Append(orderby);
            } // end if

            return sql.ToString();
        }

        public string GetSqlString()
        {
            return this.SqlString;
        }

        public IEnumerable<MT_REF_DB> Get(MT_REF_DB mtrefdb)
        {
            SqlString = GetSqlQuery(mtrefdb);
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, SqlString).Tables[0];

            MTREFDBList = ConvertToTankReadings(dt);
            //MTDBList = MTDBList.Where(x => x.DOCNUM == docnum);
            return MTREFDBList;
        }

        private IEnumerable<MT_REF_DB> ConvertToTankReadings(DataTable dataTable)
        {
            Constant.Init();
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new MT_REF_DB
                {
                    REFID = Convert.ToString(row["REFID"]),
                    PARTNER = Convert.ToString(row["PARTNER"]),
                    DIVISION = Convert.ToString(row["DIVISION"]),
                    REGION = Convert.ToString(row["REGION"]),
                    ISASENDERID = Convert.ToString(row["ISASENDERID"]),
                    ISARECEIVERID = Convert.ToString(row["ISARECEIVERID"]),
                    GSSENDERID = Convert.ToString(row["GSSENDERID"]),
                    EDIMSGTYPE = Convert.ToString(row["EDIMSGTYPE"])
                };
            }

        }
    }
}