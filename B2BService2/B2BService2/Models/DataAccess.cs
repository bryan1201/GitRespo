using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Configuration;

namespace B2BService.Models
{
    public class DataAccess
    {
        private static readonly string AssemblyName = "B2BService"; // The string is the current namespace
        private static readonly string MTDBCollection = "MTDBCollection";
        private static readonly string LOOKUPDBCollection = "LOOKUPDBCollection";
        private static readonly string PROCESSDBCollection = "PROCESSDBCollection";
        private static readonly string STATISTIC = "Statistic";
        private static string db = ConfigurationManager.AppSettings["PIQServer"];

        public static IRawData CreateRawData(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, Constant.RawData);
            return (IRawData)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMDN CreateMDN(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, Constant.MDN);
            return (IMDN)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IAuditLog CreateAuditLog(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, Constant.AuditLog);
            return (IAuditLog)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMTDBCollection CreateMTDBCollection(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, MTDBCollection);
            return (IMTDBCollection)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static ILOOKUPDBCollection CreateLOOKUPDBCollection(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, LOOKUPDBCollection);
            return (ILOOKUPDBCollection)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IPROCESSDBCollection CreatePROCESSDBCollection(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, PROCESSDBCollection);
            return (IPROCESSDBCollection)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IMTRef CreateMTREFDB (string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, Constant.MTREFDB);
            return (IMTRef)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IStatistic CreateStatistic(string server)
        {
            db = ConfigurationManager.AppSettings[server];
            string className = string.Format("{0}.{1}.{2}{3}", AssemblyName, "Models", db, STATISTIC);
            return (IStatistic)Assembly.Load(AssemblyName).CreateInstance(className);
        }
    }
}