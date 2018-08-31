using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
}