using Oracle.DataAccess;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Reflection;

namespace B2BService.Models
{
    public interface IMTDBCollection
    {
        IEnumerable<VMTDB> Get(MT_DB mtdb);
        string GetSqlString();
    }

    public class MT_DB
    {
        public string MSGID { get; set; }
        public string PARENT { get; set; }
        public string AS2PARTNER { get; set; }
        public decimal? DIRECTION { get; set; }
        public string CHLMSGID { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public decimal? STATUS { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public string GSSENDERID { get; set; }
        public string GSRECEIVERID { get; set; }
        public string DOCNUM { get; set; }
        public string ISASENDERID { get; set; }
        public string ISARECEIVERID { get; set; }
        public string CONTROLNUM { get; set; }
        public DateTime? EDIDATE { get; set; }
        public string EDIMSGTYPE { get; set; }
        public string FILEURI { get; set; }
        public string KEYWORD_SEARCH { get; set; }
        public string IDOC { get; set; }
        public int? AUDIT_CHECK_STATUS { get; set; }
        public string PI_STATUS { get; set; }
        public string PI_ERROR_CODE { get; set; }
        public string PI_ERROR_CATEGORY { get; set; }
        public string IDOC_CALLBACK_MSG { get; set; }

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
            return (this._CreateDateFrom.HasValue)? this._CreateDateFrom.Value : this._CreateDateFrom;
        }

        public DateTime? GetCreateDateEnd()
        {
            return (this._CreateDateEnd.HasValue) ? this._CreateDateEnd.Value : this._CreateDateEnd;
        }
    }

    public class VMTDB : MT_DB
    {
        public string MTStatus { get; set; }
        public string MTStatusRemark { get; set; }
        public string MTDirection { get; set; }
        public string MTDirectionRemark { get; set; }
    }

    public class MTDBCollection
    {
        public IEnumerable<VMTDB> MTDBList;
        private string SqlString = string.Empty;
        private string config = string.Empty;
        public MTDBCollection(string config)
        {
            this.config = config;
        }

        private int CheckIsNullOrEmpty(MT_DB mtdb)
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
                    rslt = string.IsNullOrEmpty(varprop.GetValue(mtdb,null) as string) ? rslt : ++rslt;
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

        private string GetSqlQuery(MT_DB mtdb)
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            string space = " ";
            string select = string.Empty;
            string where = string.Empty;
            string orderby = string.Empty;
            string MSGID = mtdb.MSGID;
            DateTime? dtCDTFrom = mtdb.GetCreateDateFrom();
            DateTime? dtCDTEnd = mtdb.GetCreateDateEnd();
            select = "SELECT /*+FIRST_ROWS(50)*/ * FROM MT_DB";
            if (CheckIsNullOrEmpty(mtdb) == 0)
            {
                sql.Append(select);
                sql.Append(space);
                //TO_DATE('2016/5/18 00:50:00','yyyy/MM/dd HH24:MI:SS')
                sql.Append("WHERE ROWNUM=0 AND NVL(DOCNUM, ' ') != ' ' ORDER BY CREATEDATE DESC");
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
                            /*
                            if (varprop.Name == "CONTROLNUM" || varprop.Name == "IDOC")
                            {
                                int ivalue = 0;
                                value = int.TryParse(value, out ivalue) ? ivalue.ToString("000000000") : value.Trim();
                            }
                            */
                            if (varprop.Name == "KEYWORD_SEARCH")
                            {
                                string[] values = value.Split(' ');
                                StringBuilder sbSubwhere = new StringBuilder();
                                int isubcond = 0;
                                foreach (string item in values)
                                {
                                    where = "TRIM(" + varprop.Name + ") LIKE '%@%'"; where = where.Replace("@", item.Trim());
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
                            if(icond > 0)
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

                    if(varprop.PropertyType.IsGenericType &&
                        varprop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var propertyType = varprop.PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
                        if(propertyType.Name=="DateTime")
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
                    where = "CREATEDATE >= TO_DATE('@', 'yyyy/MM/dd HH24:MI:SS')";
                    where = where.Replace("@", dtCDTFrom.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    sbwhere.Append(where);
                }

                if (dtCDTEnd.HasValue)
                {
                    sbwhere.Append(space);
                    sbwhere.Append("AND");
                    sbwhere.Append(space);
                    where = "CREATEDATE <= TO_DATE('@', 'yyyy/MM/dd HH24:MI:SS')";
                    where = where.Replace("@", dtCDTEnd.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                    sbwhere.Append(where);
                    sbwhere.Append(space);
                }

                orderby = "ORDER BY CREATEDATE DESC";
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

        public IEnumerable<VMTDB> Get(MT_DB mtdb)
        {
            SqlString = GetSqlQuery(mtdb);
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, SqlString).Tables[0];

            MTDBList = ConvertToTankReadings(dt);
            //MTDBList = MTDBList.Where(x => x.DOCNUM == docnum);
            return MTDBList;
        }

        private IEnumerable<VMTDB> ConvertToTankReadings(DataTable dataTable)
        {
            Constant.Init();
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime? dtCREATEDATE = string.IsNullOrEmpty(row["CREATEDATE"].ToString()) ? (DateTime?)null : DateTime.Parse(row["CREATEDATE"].ToString());
                DateTime? dtUPDATETIME = string.IsNullOrEmpty(row["UPDATETIME"].ToString()) ? (DateTime?)null : DateTime.Parse(row["UPDATETIME"].ToString());
                DateTime? dtEDIDATE = string.IsNullOrEmpty(row["EDIDATE"].ToString()) ? (DateTime?)null : DateTime.Parse(row["EDIDATE"].ToString());

                string strStatus = row["STATUS"].ToString();
                string strDirection = row["DIRECTION"].ToString();
                LOOKUP_DB vmts = Constant.LookupMTStatus.Where(x => x.CODE == strStatus).FirstOrDefault();
                string strMTStatus = vmts.DESCRIPTION;
                string strMTStatusRemark = vmts.REMARK;
                LOOKUP_DB vmtd = Constant.LookupMTMsgDirection.Where(x => x.CODE == strDirection).FirstOrDefault();
                string strMTDirection = vmtd.DESCRIPTION;
                string strMTDirectionRemark = vmtd.REMARK;

                yield return new VMTDB
                {
                    MSGID = Convert.ToString(row["MSGID"]),
                    PARENT = Convert.ToString(row["PARENT"]),
                    AS2PARTNER = Convert.ToString(row["AS2PARTNER"]),
                    DIRECTION = Convert.ToDecimal(row["DIRECTION"]),
                    MTDirection = strMTDirection,
                    MTDirectionRemark = strMTDirectionRemark,
                    CHLMSGID= Convert.ToString(row["CHLMSGID"]),
                    CREATEDATE = dtCREATEDATE,
                    STATUS = Convert.ToDecimal(row["STATUS"]),
                    MTStatus = strMTStatus,
                    MTStatusRemark = strMTStatusRemark,
                    UPDATETIME = dtUPDATETIME,
                    GSSENDERID = Convert.ToString(row["GSSENDERID"]),
                    GSRECEIVERID = Convert.ToString(row["GSRECEIVERID"]),
                    DOCNUM = Convert.ToString(row["DOCNUM"]),
                    ISASENDERID = Convert.ToString(row["ISASENDERID"]),
                    ISARECEIVERID = Convert.ToString(row["ISARECEIVERID"]),
                    CONTROLNUM = Convert.ToString(row["CONTROLNUM"]),
                    EDIDATE = dtEDIDATE,
                    EDIMSGTYPE = Convert.ToString(row["EDIMSGTYPE"]),
                    FILEURI = Convert.ToString(row["FILEURI"]),
                    KEYWORD_SEARCH = Convert.ToString(row["KEYWORD_SEARCH"]),
                    IDOC = Convert.ToString(row["IDOC"]),
                    AUDIT_CHECK_STATUS = Convert.ToInt16(row["AUDIT_CHECK_STATUS"]),
                    PI_STATUS = Convert.ToString(row["PI_STATUS"]),
                    PI_ERROR_CODE = Convert.ToString(row["PI_ERROR_CODE"]),
                    PI_ERROR_CATEGORY = Convert.ToString(row["PI_ERROR_CATEGORY"]),
                    IDOC_CALLBACK_MSG = Convert.ToString(row["IDOC_CALLBACK_MSG"])
                };
            }

        }
    }
}