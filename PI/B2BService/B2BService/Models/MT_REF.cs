using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
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

    public class MT_REF_DB : absMT_REF
    {
        private string config = string.Empty;
        public MT_REF_DB(string config)
        {
            this.config = config;
        }

        public IEnumerable<MT_REF_DB> QUERY()
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
                    REFID = Convert.ToString(row[9]),
                    PARTNER = Convert.ToString(row[0]),
                    DIVISION = Convert.ToString(row[1]),
                    REGION = Convert.ToString(row[2]),
                    ISASENDERID = Convert.ToString(row[3]),
                    ISARECEIVERID = Convert.ToString(row[4]),
                    GSSENDERID = Convert.ToString(row[5]),
                    EDIMSGTYPE = Convert.ToString(row[6])
                };
            }
        }
    }
}