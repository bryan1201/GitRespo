using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using System.Web;
using Oracle.DataAccess;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;

namespace B2BService.Models
{
    public abstract class absDBEDIMSGTYPE
    { //TYPEID, CUSTOMER, EDIMSGTYPE, UDTIME, "USER"
        [Key]
        public Guid TYPEID { get; set; }
        public string CUSTOMER { get; set; }
        public string EDIMSGTYPE { get; set; }
        public DateTime UDTIME { get; set; }
        public string USER { get; set; }
    }
    public class EDIMSGTYPE_DB:absDBEDIMSGTYPE
    {
        private string config = string.Empty;
        public EDIMSGTYPE_DB(string config)
        {
            this.config = config;
        }

        public IEnumerable<EDIMSGTYPE_DB> QUERY()
        {
            DateTime dtEnd = DateTime.Now;
            DateTime dtFrom = dtEnd.AddHours(-23.0);

            string sqlString = "SELECT * FROM EDIMSGTYPE_DB";
            System.Data.DataTable dt = DAO.oracleCmdDataSetSP(this.config, sqlString).Tables[0];
            IEnumerable<EDIMSGTYPE_DB> dresult = ConvertToDB_Readings(dt);
            
            return dresult;
        }

        private IEnumerable<EDIMSGTYPE_DB> ConvertToDB_Readings(DataTable dataTable)
        {
            //TYPEID, CUSTOMER, EDIMSGTYPE, UDTIME, "USER"
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new EDIMSGTYPE_DB(this.config)
                {
                    TYPEID = Guid.Parse(row[0].ToString()),
                    CUSTOMER = Convert.ToString(row[1]),
                    EDIMSGTYPE = Convert.ToString(row[2]),
                    UDTIME = Convert.ToDateTime(row[3].ToString()),
                    USER = Convert.ToString(row[4])
                };
            }
        }
    }
}