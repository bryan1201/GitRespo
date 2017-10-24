using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using einvoice.Models;
using System.Web.Script.Services;
using einvoice.QRCodeReference;
using System.Xml;

namespace einvoice.Services
{
    /// <summary>
    ///QREncryptService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class QREncryptService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public Models.QRCode InitQRCode()
        {
            Models.QRCode qrcode = new Models.QRCode();
            qrcode = qrcode.InitTestQRCode();
            return qrcode;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string QREncryptTest()
        {
            Models.QRCode qrcode = new Models.QRCode();
            qrcode = qrcode.InitTestQRCode();
            string result = qrcode.QREncrypterString(true);
            return result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string QREncrypt(string QRCodeXMLString)
        {
            string result = string.Empty;
            
            Serializer ser = new Serializer();
            Models.QRCode qrcode = ser.Deserialize<Models.QRCode>(QRCodeXMLString);

            try
            {  
                result = qrcode.QREncrypterString(false);
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
            
            return result;
        }
    }
}
