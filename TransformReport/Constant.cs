using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TransformReport
{
    public class Constant
    {
        public static string HALL = ConfigurationManager.AppSettings["HALL"];
        public static string ROWS = ConfigurationManager.AppSettings["ROWS"];
        public static string REPORT = ConfigurationManager.AppSettings["REPORT"];
    }
}
