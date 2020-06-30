using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using einvoice.Models;

using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace einvoice.Controllers.APIs
{
    public class eInvoiceDocController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetEinvoiceFile(string filename)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                IRawData ir = new RawData();
                string xmlstring = ir.GetContent(filename, "application / octet - stream"); // text/xml
                result.Content = new StringContent(xmlstring);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                result.Content = new StringContent(ex.Message);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            return result;
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            string qrEncryptString = string.Empty;
            Models.QRCode qr = new QRCode();
            qr.InitTestQRCode();
            //QRCodeReference.QREncryptServiceSoapClient qrc = new QRCodeReference.QREncryptServiceSoapClient();
            qrEncryptString = qr.QREncrypterString(true);
            return qrEncryptString;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}