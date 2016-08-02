using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;
namespace ChangeDB
{
    class DataAccess
    {
        private static readonly string AssemblyName = "ChangeDB"; // The string is the current namespace
        private static readonly string User = "User";
        private static readonly string Dept = "Dept";
        private static readonly string db = ConfigurationManager.AppSettings["DB"];

        public static IUser CreateUser()
        {
            string className = string.Format("{0}.{1}{2}",AssemblyName, db, User);
            return (IUser)Assembly.Load(AssemblyName).CreateInstance(className);
        }

        public static IDept CreateDept()
        {
            string className = string.Format("{0}.{1}{2}", AssemblyName, db, Dept);
            return (IDept)Assembly.Load(AssemblyName).CreateInstance(className);
        }
    }
}
