using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using B2BService.Models;

namespace B2BService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Help";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test(string id, string optradio)
        {
            optradio = (string.IsNullOrEmpty(optradio)) ? Constant.PIQServer : optradio;

            ViewBag.Message = "Your function test page.";
            //IFactory ifactory = DataAccess.CreateRawData(optradio);
            IRawData irawdata = DataAccess.CreateRawData(optradio);
            //string Rslt = irawdata.Get().Content;
            RawData  r = irawdata.Get(id, Constant.ContentTypeUTF8);
            string newurl = r.Url;
            string Rslt = r.Get();
            ViewBag.newUrl = newurl;
            ViewData["RsltRawdata"] = Rslt;
            ViewBag.piServer = optradio;

            return View();
        }

        private ServiceType _type = ServiceType.List;
        public ActionResult MtRefDB(string id, string optradio)
        {
            IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IList<string> partners = ((IEnumerable<string>)imtref.GetPARTNER(_type)).ToList();
            
            ViewData["Partners"] = partners;
            ViewBag.piServer = optradio;
            return View();
        }
    }
}