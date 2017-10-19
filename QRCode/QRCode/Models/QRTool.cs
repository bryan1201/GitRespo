using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.tradevan.qrutil;
using QRCode.Models;

namespace QRCode.Models
{
    public class QRTool
    {
        public string QREncrypterString()
        {
            string result = string.Empty;
            string AESCode = Constant.S_AESTestCode;
            com.tradevan.qrutil.QREncrypter qrEncrypter = new com.tradevan.qrutil.QREncrypter();
            try
            {
                String[][] abc = new String[1][];
                result = qrEncrypter.QRCodeINV("AA12345678", "1001231", "150000", "1234", 100, 100, 100, "12345678", "87654321", "12344321", "43211234", AESCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
        
}