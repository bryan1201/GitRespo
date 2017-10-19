using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace QRCode.Models
{
    public class Constant
    {
        public static String S_SPACE = " ";
        public static String S_AESTestCode = ConfigurationManager.AppSettings["AESTestCode"];

        public Constant()
        {

        }
    }
}