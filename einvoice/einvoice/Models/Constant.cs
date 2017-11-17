using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace einvoice.Models
{
    public class Constant
    {
        public static readonly String S_SPACE = " ";
        public static readonly String S_AESTestCode = ConfigurationManager.AppSettings["AESTestCode"];
        public static readonly string S_BusinessIdentifier = ConfigurationManager.AppSettings["BusinessIdentifier"];
        public static readonly string S_A0101FilePath = ConfigurationManager.AppSettings["A0101FilePath"];
        public static readonly string S_eInvoiceFTPUser= ConfigurationManager.AppSettings["S_eInvoiceFTPUser"];
        public static readonly string S_eInvoiceFTPPWD = ConfigurationManager.AppSettings["eInvoiceFTPPWD"];
        public static readonly string S_eInvoiceFTPServer = ConfigurationManager.AppSettings["eInvoiceFTPServer"];
        public static readonly string S_eInoviceFTPA0101 = ConfigurationManager.AppSettings["eInoviceFTPA0101"];
        public static readonly string DEVDBContext = ConfigurationManager.ConnectionStrings["DEVDBContext"].ToString();
        public static readonly string QASDBContext = ConfigurationManager.ConnectionStrings["QASDBContext"].ToString();
        public static readonly string PRDDBContext = ConfigurationManager.ConnectionStrings["PRDDBContext"].ToString();

        public static readonly string DEVServer = "DEVServer";
        public static readonly string QASServer = "QASServer";
        public static readonly string PRDServer = "PRDServer";

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public Constant()
        {

        }
    }
}