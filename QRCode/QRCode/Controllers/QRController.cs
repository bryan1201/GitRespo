using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QRCode.Controllers;
using QRCode.Models;

namespace QRCode.Controllers
{
    public class QRController : ApiController
    {
        // GET: api/QR
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/QR/5
        public string Get(int id)
        {
            QRTool qrtool = new Models.QRTool();
            string rslt = qrtool.QREncrypterString();
            return rslt;
        }

        // POST: api/QR
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/QR/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/QR/5
        public void Delete(int id)
        {
        }
    }
}
