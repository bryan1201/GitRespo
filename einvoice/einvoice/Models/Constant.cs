using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace einvoice.Models
{
    public class Constant
    {
        public static readonly String S_SPACE = " ";
        public static readonly String S_AESTestCode = ConfigurationManager.AppSettings["AESTestCode"];
        public static readonly string S_BusinessIdentifier = ConfigurationManager.AppSettings["BusinessIdentifier"];
        public Constant()
        {

        }
    }
}