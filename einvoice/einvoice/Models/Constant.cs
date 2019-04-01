using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace einvoice.Models
{
    public class Constant
    {
        public static readonly String S_SPACE = " ";
        public static readonly String S_AESTestCode = ConfigurationManager.AppSettings["AESTestCode"];
        public static readonly string S_BusinessIdentifier = ConfigurationManager.AppSettings["BusinessIdentifier"];
        public static readonly string S_A0101FilePath = ConfigurationManager.AppSettings["A0101FilePath"];
        public static readonly string S_eInvoiceFTPUser= ConfigurationManager.AppSettings["eInvoiceFTPUser"];
        public static readonly string S_eInvoiceFTPPWD = ConfigurationManager.AppSettings["eInvoiceFTPPWD"];
        public static readonly string S_eInvoiceFTPServer = ConfigurationManager.AppSettings["eInvoiceFTPServer"];
        public static readonly string S_eInoviceFTPA0101 = ConfigurationManager.AppSettings["eInoviceFTPA0101"];
        public static readonly string DEVDBContext = ConfigurationManager.ConnectionStrings["DEVDBContext"].ToString();
        public static readonly string QASDBContext = ConfigurationManager.ConnectionStrings["QASDBContext"].ToString();
        public static readonly string PRDDBContext = ConfigurationManager.ConnectionStrings["PRDDBContext"].ToString();
        public static readonly string Turnkeyfileroot = ConfigurationManager.AppSettings["Turnkeyfileroot"];

        public static readonly string DEVServer = "DEVServer";
        public static readonly string QASServer = "QASServer";
        public static readonly string PRDServer = "PRDServer";
        public static readonly string RawData = "RawData";

        public static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

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

        public static void webRequestException(WebException ex, HttpContext con, string url, out string response)
        {
            string ret = "\r\n";
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            dynamic obj = JsonConvert.DeserializeObject(resp.ToString());
            string statuscode = obj["httpStatusCode"].ToString();
            string message = obj["message"].ToString();
            string stackTrackFromServer = obj["stackTrace"].ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("WebClient.DownadString from url:{0}{1}", url, ret));
            sb.Append(string.Format("HttpContext.Request.Url:{0}{1}", con.Request.Url.ToString(), ret));
            sb.Append(string.Format("HttpContext.Request:{0}{1}", con.Request.ToString(), ret));
            sb.Append(string.Format("##WebException.Response.GetResponseStream from Server:{0}", ret));
            sb.Append(string.Format("httpStatusCode: {0}{1}", statuscode, ret));
            sb.Append(string.Format("message: {0}{1}", message, ret));
            sb.Append(string.Format("stackTrack:{0}{1}", stackTrackFromServer, ret));
            response = sb.ToString();
        }
    }
}