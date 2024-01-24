using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Text;
using einvoice.Models;
using System.Collections;

namespace einvoice.Models
{
    public interface ISYSEVENTDBCollection
    {
        IEnumerable<TURNKEY_SYSEVENT_LOG> Get(TURNKEY_SYSEVENT_LOG mtdb);
        string GetSqlString();
    }

    public class SYSEVENTDBCollection
    {
        public IEnumerable<TURNKEY_SYSEVENT_LOG> SYSEVENTDBList;
        private string SqlString = string.Empty;
        private string config = string.Empty;
        private eInvoiceDBContext _db;
        public SYSEVENTDBCollection(string config)
        {
            this.config = config;
            this._db = new eInvoiceDBContext(config);
        }

        private int CheckIsNullOrEmpty(TURNKEY_SYSEVENT_LOG mtdb)
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

        private string GetSqlQuery(TURNKEY_SYSEVENT_LOG mtdb)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;

            DateTime? dtCDTFrom = mtdb.GetCreateDateFrom();
            DateTime? dtCDTEnd = mtdb.GetCreateDateEnd();
            select = @"SELECT * FROM TURNKEY_SYSEVENT_LOG";
            if (CheckIsNullOrEmpty(mtdb) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                //TO_DATE('2016/5/18 00:50:00','yyyy/MM/dd HH24:MI:SS')
                sql.Append("WHERE 1=0 ORDER BY EVENTDTS ASC");
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
                    where = "EVENTDTS >= REPLACE(CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),112)+CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),114),':','')";
                    where = where.Replace("@", dtCDTFrom.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    sbwhere.Append(where);
                }

                if (dtCDTEnd.HasValue)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                    where = "EVENTDTS <= REPLACE(CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),112)+CONVERT(NVARCHAR,CONVERT(DATETIME, '@'),114),':','')";
                    where = where.Replace("@", dtCDTEnd.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    sbwhere.Append(where);
                    sbwhere.Append(space);
                }

                orderby = "ORDER BY EVENTDTS DESC";
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

        public IEnumerable<TURNKEY_SYSEVENT_LOG> Get(TURNKEY_SYSEVENT_LOG syseventdb)
        {
            SqlString = GetSqlQuery(syseventdb);
            List<TURNKEY_SYSEVENT_LOG> tml = _db.TurnKeySyseventLog.SqlQuery(SqlString).ToList();
            this.SYSEVENTDBList = tml;
            //System.Data.DataTable dt = Constant.ToDataTable(tml);

            //MTDBList = ConvertToTankReadings(dt);
            //MTDBList = MTDBList.Where(x => x.DOCNUM == docnum);
            return tml;
        }

        private IEnumerable<TURNKEY_SYSEVENT_LOG> ConvertToTankReadings(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new TURNKEY_SYSEVENT_LOG
                {
                    EVENTDTS = Convert.ToString(row["EVENTDTS"]),
                    PARTY_ID = Convert.ToString(row["PARTY_ID"]),
                    SEQNO = Convert.ToString(row["SEQNO"]),
                    SUBSEQNO = Convert.ToString(row["SUBSEQNO"]),
                    ERRORCODE = Convert.ToString(row["ERRORCODE"]),
                    UUID = Convert.ToString(row["UUID"]),
                    INFORMATION1 = Convert.ToString(row["INFORMATION1"]),
                    INFORMATION2 = Convert.ToString(row["INFORMATION2"]),
                    INFORMATION3 = Convert.ToString(row["INFORMATION3"]),
                    MESSAGE1 = Convert.ToString(row["MESSAGE1"]),
                    MESSAGE2 = Convert.ToString(row["MESSAGE2"]),
                    MESSAGE3 = Convert.ToString(row["MESSAGE3"]),
                    MESSAGE4 = Convert.ToString(row["MESSAGE4"]),
                    MESSAGE5 = Convert.ToString(row["MESSAGE5"]),
                    MESSAGE6 = Convert.ToString(row["MESSAGE6"])
                };
            }
        }
    }

    public class TURNKEY_SYSEVENT_LOG
    {
        [Key]
        public string EVENTDTS { get; set; }
        public string PARTY_ID { get; set; }
        public string SEQNO { get; set; }
        public string SUBSEQNO { get; set; }
        public string ERRORCODE { get; set; }
        public string UUID { get; set; }
        public string INFORMATION1 { get; set; }
        public string INFORMATION2 { get; set; }
        public string INFORMATION3 { get; set; }
        public string MESSAGE1 { get; set; }
        public string MESSAGE2 { get; set; }
        public string MESSAGE3 { get; set; }
        public string MESSAGE4 { get; set; }
        public string MESSAGE5 { get; set; }
        public string MESSAGE6 { get; set; }
        private eInvoiceDBContext _db { get; set; }

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

        public TURNKEY_SYSEVENT_LOG(eInvoiceDBContext db)
        {
            this._db = db;
        }

        public TURNKEY_SYSEVENT_LOG()
        {
      
        }
        public TURNKEY_SYSEVENT_LOG(string eInvServer)
        {
            string connstring = string.Empty;
            if (this._db == null)
            {
                switch (eInvServer)
                {
                    case "DEVServer":
                        connstring = Constant.DEVDBContext;
                        break;
                    case "QASServer":
                        connstring = Constant.QASDBContext;
                        break;
                    case "PRDServer":
                        connstring = Constant.PRDDBContext;
                        break;
                }

                this._db = new eInvoiceDBContext(connstring);
            }
        }
    }
}