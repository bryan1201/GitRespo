using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using einvoice.Models;
using einvoice.Models.eInvoiceMessage;

namespace einvoice.Services
{
    /// <summary>
    ///InvoiceService 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class InvoiceService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description = "InvoiceDate用民國年，BusinessIdentifier預設為04322046")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string A0101(A0101 Invoice, string filename)
        {
            string result = string.Empty;

            try
            {
                DataAccess da = new Models.DataAccess();
                string filepath = string.Format("{0}{1}",Constant.S_A0101FilePath, filename);
                da.SaveRawData(Invoice, filepath);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
