using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using einvoice.Models.eInvoiceMessage;
using System.Configuration;
using System.Reflection;

namespace einvoice.Models
{
    public class DataAccess:IRawData
    {
        private static readonly string AssemblyName = "einvoice"; // The string is the current namespace
        private static readonly string MTDBCollection = "MTDBCollection";
        private static readonly string SYSEVENTDBCollection = "SYSEVENTDBCollection";
        private static readonly string RawData = "RawData";
        private static readonly string _db = ConfigurationManager.AppSettings["QASServer"];

        public DataAccess()
        {

        }

        public static IMTDBCollection CreateMTDBCollection(string server)
        {
            /*
            einvoice.Models.DEVServerMTDBCollection d = new Models.DEVServerMTDBCollection();
            einvoice.Models.QASServerMTDBCollection q = new Models.QASServerMTDBCollection();
            einvoice.Models.PRDServerMTDBCollection p = new Models.PRDServerMTDBCollection();
            */
            string models = "Models";
            string db = ConfigurationManager.AppSettings[server];
            db = (db == null) ? _db : db;
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, models, db, MTDBCollection);
            return (IMTDBCollection)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static ISYSEVENTDBCollection CreateSYSEVENTDBCollection(string server)
        {
            string models = "Models";
            string db = ConfigurationManager.AppSettings[server];
            db = (db == null) ? _db : db;
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, models, db, SYSEVENTDBCollection);
            return (ISYSEVENTDBCollection)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IRawData CreateRawData(string server)
        {
            string models = "Models";
            string db = ConfigurationManager.AppSettings[server];
            db = (db == null) ? _db : db;
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, models, db, RawData);
            return (IRawData)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public void SaveRawData(A0101 Invoice, string filename)
        {
            Message m = new Models.Message();
            m.SerializeObject<A0101>(Invoice, filename);
            m.FtpRawData(filename);
        }

        public string GetContent(string filepathname, string contenttype)
        {
            string url = Constant.S_eInvoiceFTPServer;
            RawData r = new RawData(filepathname, contenttype);
            
            return r.Content; ;
        }
    }
}