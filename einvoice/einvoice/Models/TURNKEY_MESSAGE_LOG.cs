using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using einvoice.Models;
using System.Collections;

namespace einvoice.Models
{
    public interface IMTDBCollection
    {
        IEnumerable<TURNKEY_MESSAGE_LOG> Get(TURNKEY_MESSAGE_LOG mtdb);
        string GetSqlString();
    }

    public class MTDBCollection
    {
        public IEnumerable<TURNKEY_MESSAGE_LOG> MTDBList;
        private string SqlString = string.Empty;
        private string config = string.Empty;
        private eInvoiceDBContext _db;
        public MTDBCollection(string config)
        {
            this.config = config;
            this._db = new eInvoiceDBContext(config);
        }

        private int CheckIsNullOrEmpty(TURNKEY_MESSAGE_LOG mtdb)
        {
            int rslt = 0;
            DateTime? dtCDTFrom = mtdb.GetCreateDateFrom();
            DateTime? dtCDTEnd = mtdb.GetCreateDateEnd();

            foreach (PropertyInfo prop in mtdb.GetType().GetProperties())
            {
                var varprop = mtdb.GetType().GetProperty(prop.Name);

                if (dtCDTFrom.HasValue)
                    rslt++;
                if (dtCDTEnd.HasValue)
                    rslt++;

                if (varprop.PropertyType.Name == "String")
                {
                    rslt = string.IsNullOrEmpty(varprop.GetValue(mtdb, null) as string) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.Name == "Int32")
                {
                    rslt = ((varprop.GetValue(mtdb, null) as int?).HasValue) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.Name == "DateTime")
                {
                    rslt = ((varprop.GetValue(mtdb, null) as DateTime?).HasValue) ? rslt : ++rslt;
                }

                if (varprop.PropertyType.IsGenericType &&
                        varprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var propertyType = varprop.PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
                    if (propertyType.Name == "DateTime")
                    {
                        DateTime? value = varprop.GetValue(mtdb, null) as DateTime?;
                        rslt = (!value.HasValue) ? rslt : ++rslt;
                    }

                    if (propertyType.Name == "Int32")
                    {
                        int? value = varprop.GetValue(mtdb, null) as int?;
                        rslt = (!value.HasValue) ? rslt : ++rslt;
                    }

                    if (propertyType.Name == "Decimal")
                    {
                        decimal? value = varprop.GetValue(mtdb, null) as decimal?;
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

        private string GetSqlQuery(TURNKEY_MESSAGE_LOG mtdb)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;
            
            DateTime? dtCDTFrom = mtdb.GetCreateDateFrom();
            DateTime? dtCDTEnd = mtdb.GetCreateDateEnd();
            select = @"SELECT * FROM TURNKEY_MESSAGE_LOG";
            if (CheckIsNullOrEmpty(mtdb) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                //TO_DATE('2016/5/18 00:50:00','yyyy/MM/dd HH24:MI:SS')
                sql.Append("WHERE 1=0 ORDER BY MESSAGE_DTS ASC");
            }
            else
            {
                sbwhere.Append("WHERE"); sbwhere.Append(space);
                int icond = 0;

                foreach (PropertyInfo prop in mtdb.GetType().GetProperties())
                {
                    var varprop = mtdb.GetType().GetProperty(prop.Name);

                    if (varprop.PropertyType.Name == "String")
                    {
                        string value = varprop.GetValue(mtdb, null) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (varprop.Name == "INVOICE_IDENTIFIER")
                            {
                                string[] values = value.Split(' ');
                                StringBuilder sbSubwhere = new StringBuilder();
                                int isubcond = 0;
                                foreach (string item in values)
                                {
                                    where = varprop.Name + " LIKE '%@%'"; where = where.Replace("@", item.Trim());
                                    if (isubcond > 0)
                                    {
                                        sbSubwhere.Append(space);
                                        sbSubwhere.Append("OR");
                                        sbSubwhere.Append(space);
                                    }
                                    sbSubwhere.Append(where);
                                    isubcond++;
                                }

                                where = "(" + sbSubwhere.ToString() + ")";
                                if (icond > 0)
                                {
                                    sbwhere.Append(space);
                                    sbwhere.Append("AND");
                                    sbwhere.Append(space);
                                }
                                sbwhere.Append(where);
                                icond++;
                            }
                            else
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

                        }
                    }

                    if (varprop.PropertyType.Name == "Int32")
                    {
                        int? value = varprop.GetValue(mtdb, null) as int?;
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

                    if (varprop.PropertyType.Name == "DateTime")
                    {
                        DateTime? value = varprop.GetValue(mtdb, null) as DateTime?;
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
                    } // end if

                    if (varprop.PropertyType.IsGenericType &&
                        varprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var propertyType = varprop.PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
                        if (propertyType.Name == "DateTime")
                        {
                            DateTime? value = varprop.GetValue(mtdb, null) as DateTime?;
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

                        if (propertyType.Name == "Int32")
                        {
                            Int32? value = varprop.GetValue(mtdb, null) as Int32?;
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

                        if (propertyType.Name == "Decimal")
                        {
                            decimal? value = varprop.GetValue(mtdb, null) as decimal?;
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

                    }
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

                if (dtCDTFrom.HasValue)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                    where = "MESSAGE_DTS >= REPLACE(CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),112)+CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),114),':','')";
                    where = where.Replace("@", dtCDTFrom.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    sbwhere.Append(where);
                }

                if (dtCDTEnd.HasValue)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                    where = "MESSAGE_DTS <= REPLACE(CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),112)+CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),114),':','')";
                    where = where.Replace("@", dtCDTEnd.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    sbwhere.Append(where);
                    sbwhere.Append(space);
                }

                orderby = "ORDER BY MESSAGE_DTS";
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

        public IEnumerable<TURNKEY_MESSAGE_LOG> Get(TURNKEY_MESSAGE_LOG mtdb)
        {
            SqlString = GetSqlQuery(mtdb);
            System.Data.DataTable dt = DAO.sqlCmdDataTable(this.config, SqlString);

            this.MTDBList = ConvertToTankReadings(dt);
            return MTDBList;
        }

        private IEnumerable<TURNKEY_MESSAGE_LOG> ConvertToTankReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new TURNKEY_MESSAGE_LOG(this._db)
                {
                    SEQNO = Convert.ToString(row["SEQNO"]),
                    SUBSEQNO = Convert.ToString(row["SUBSEQNO"]),
                    UUID = Convert.ToString(row["UUID"]),
                    MESSAGE_TYPE = Convert.ToString(row["MESSAGE_TYPE"]),
                    CATEGORY_TYPE = Convert.ToString(row["CATEGORY_TYPE"]),
                    PROCESS_TYPE = Convert.ToString(row["PROCESS_TYPE"]),
                    FROM_PARTY_ID = Convert.ToString(row["FROM_PARTY_ID"]),
                    TO_PARTY_ID = Convert.ToString(row["TO_PARTY_ID"]),
                    MESSAGE_DTS = Convert.ToString(row["MESSAGE_DTS"]),
                    CHARACTER_COUNT = Convert.ToString(row["CHARACTER_COUNT"]),
                    STATUS = Convert.ToString(row["STATUS"]),
                    IN_OUT_BOUND = Convert.ToString(row["IN_OUT_BOUND"]),
                    FROM_ROUTING_ID = Convert.ToString(row["FROM_ROUTING_ID"]),
                    TO_ROUTING_ID = Convert.ToString(row["TO_ROUTING_ID"]),
                    INVOICE_IDENTIFIER = Convert.ToString(row["INVOICE_IDENTIFIER"])
                };
            }
        }
    }

    public class TURNKEY_MESSAGE_LOG
    {   
        [Key]
        public string SEQNO { get; set; }
        public string SUBSEQNO { get; set; }
        public string UUID { get; set; }
        public string MESSAGE_TYPE { get; set; }
        public string CATEGORY_TYPE { get; set; }
        public string PROCESS_TYPE { get; set; }
        public string FROM_PARTY_ID { get; set; }
        public string TO_PARTY_ID { get; set; }
        public string MESSAGE_DTS { get; set; }
        public string CHARACTER_COUNT { get; set; }
        public string STATUS { get; set; }
        public string IN_OUT_BOUND { get; set; }
        public string FROM_ROUTING_ID { get; set; }
        public string TO_ROUTING_ID { get; set; }
        public string INVOICE_IDENTIFIER { get; set; }

        private eInvoiceDBContext _db { get; set; }

        public IEnumerable<TURNKEY_MESSAGE_LOG_DETAIL> TurnkeyMessageLogDetail {
            get {
                return this.GetTurnkeyMessageLogDetail(this.SEQNO, _db);
            }
        }

        private DateTime? _CreateDateFrom { get; set; }
        private DateTime? _CreateDateEnd { get; set; }
        public void SetCreateDateFrom(DateTime? dt)
        {
            this._CreateDateFrom = dt;
        }

        public void SetCreateDateEnd(DateTime? dt)
        {
            this._CreateDateEnd = dt; //DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd 23:59:59"));
        }

        public DateTime? GetCreateDateFrom()
        {
            return (this._CreateDateFrom.HasValue) ? this._CreateDateFrom.Value : this._CreateDateFrom;
        }

        public DateTime? GetCreateDateEnd()
        {
            return (this._CreateDateEnd.HasValue) ? this._CreateDateEnd.Value : this._CreateDateEnd;
        }

        public TURNKEY_MESSAGE_LOG(eInvoiceDBContext db)
        {
            this._db = db;
        }
        public TURNKEY_MESSAGE_LOG()
        {

        }
        private IEnumerable<TURNKEY_MESSAGE_LOG_DETAIL> GetTurnkeyMessageLogDetail(string SEQNO, eInvoiceDBContext _db)
        {
            IEnumerable<TURNKEY_MESSAGE_LOG_DETAIL> tmld = _db.TurnKeyMessageLogDetail.Where(x => x.SEQNO == SEQNO).OrderBy(x => x.PROCESS_DTS);
            return tmld;
        }
    }
}