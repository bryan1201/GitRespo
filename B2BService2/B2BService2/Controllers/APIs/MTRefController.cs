using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using B2BService.Models;
using B2BService.Controllers;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace B2BService.Controllers.APIs
{
    public class MTRefController : ApiController
    {
        // GET api/<controller>
        private ServiceType _type = ServiceType.Json;
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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

        private HttpResponseMessage retRspMsg(IList<string> listItems)
        {
            string json = JsonConvert.SerializeObject(listItems, Formatting.Indented);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        public HttpResponseMessage GetPartners(string optradio)
        {
            //http://localhost:52010/B2BService/api/MTRef/GetPartners?optradio=PIQServer
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> partners = ((IEnumerable<string>)imtref.GetPARTNER(_type)).ToList();
            var result = retRspMsg(partners);
            return result;
        }

        public HttpResponseMessage GetDivisions(string partner, string optradio)
        {
            //http://localhost:52010/B2BService/api/MTRef/GetDivisions?partner=DELL&optradio=PIQServer
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> divisions = ((IEnumerable<string>)imtref.GetDIVISION(_type, partner)).ToList();
            var result = retRspMsg(divisions);
            return result;
        }

        public HttpResponseMessage GetRegions(string partner, string division, string optradio)
        {
            //http://localhost:52010/B2BService/api/MTRef/GetRegions?partner=DELL&division=Server&optradio=PIQServer
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> regions = ((IEnumerable<string>)imtref.GetREGION(_type, partner, division)).ToList();
            var result = retRspMsg(regions);
            return result;
        }

        public HttpResponseMessage GetISASenderIDs(string partner, string division, string region, string optradio)
        {
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> isasenderids = ((IEnumerable<string>)imtref.GetISASENDERID(_type, partner, division, region)).ToList();
            var result = retRspMsg(isasenderids);
            return result;
        }

        public HttpResponseMessage GetISAReceiverIDs(string partner, string division, string region, string optradio)
        {
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> isasreceiverids = ((IEnumerable<string>)imtref.GetISARECEIVERID(_type, partner, division, region)).ToList();
            var result = retRspMsg(isasreceiverids);
            return result;
        }

        public HttpResponseMessage GetGSSenderIDs(string partner, string division, string region, string optradio)
        {
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> gssenderids = ((IEnumerable<string>)imtref.GetGSSENDERID(_type, partner, division, region)).ToList();
            var result = retRspMsg(gssenderids);
            return result;
        }

        public HttpResponseMessage GetEDIMsgTypes(string partner, string division, string region, string optradio)
        {
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> edimsgtypes = ((IEnumerable<string>)imtref.GetEDIMSGTYPE(_type, partner, division, region)).ToList();
            var result = retRspMsg(edimsgtypes);
            return result;
        }
    }
}