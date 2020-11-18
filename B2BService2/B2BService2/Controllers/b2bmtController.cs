using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B2BService.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Types;
using System.Web.Routing;

namespace B2BService.Controllers
{
    public class b2bmtController : Controller
    {
        // GET: b2bmt
        IEnumerable<VMTREFDB> vmtrefdbList;
        public ActionResult Index()
        {
            InitDLL("");
            return View();
        }

        public void InitDLL(string selectedvalue)
        {
            Constant.Init();
            var olist = Constant.LookupMTStatus.Select(x => new { Id = x.CODE, Name = x.REMARK }).ToList();
            var initlist = Enumerable.Empty<object>().Select(r => new { Id = "", Name = "" }).ToList();
            foreach (var item in olist)
            {
                initlist.Add(new
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }
            List<SelectListItem> initList = new List<SelectListItem>();
            initList.Add(new SelectListItem()
            {
                Text = "",
                Value = ""
            });
            var orderlist = initlist.ToList();
            foreach (var item in orderlist)
            {
                initList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (!string.IsNullOrEmpty(selectedvalue)) ? (item.Id.ToString() == selectedvalue) : (item.Id.ToString() == "")
                });
            }

            SelectList cList = new SelectList(initList, "Value", "Text");
            ViewData["ddlSTATUS"] = cList;
            ViewBag.ddlSTATUS = cList;
        }

        [ValidateInput(false)]
        public ActionResult MT_DB(string piServer, string SegmentDelimiter,
            string partner, string division, string region,
            string docnum, string msgid, string parent, string controlnum, string idoc,
            string edimsgtype, string chlMsgId, decimal? direction, decimal? ddlStatus,
            string gssenderid, string gsreceiverid,
            string isasenderid, string isareceiverid, DateTime? cdtFrom, DateTime? cdtEnd, string keyWordSearch)
        {
            MT_DB mtdb = new Models.MT_DB();
            //mtdb.AS2PARTNER = (string.IsNullOrEmpty(partner)) ? string.Empty : partner.ToUpper();
            //等對應表修正了，再加入上列查詢條件。

            mtdb.MSGID = (string.IsNullOrEmpty(msgid)) ? string.Empty : msgid.ToLower();
            mtdb.PARENT = (string.IsNullOrEmpty(parent)) ? string.Empty : parent.ToLower();
            mtdb.DOCNUM = (string.IsNullOrEmpty(docnum)) ? string.Empty : docnum;
            mtdb.CONTROLNUM = (string.IsNullOrEmpty(controlnum)) ? string.Empty : controlnum;
            mtdb.IDOC = (string.IsNullOrEmpty(idoc)) ? string.Empty : idoc;
            mtdb.EDIMSGTYPE = (string.IsNullOrEmpty(edimsgtype)) ? string.Empty : edimsgtype;
            mtdb.CHLMSGID = (string.IsNullOrEmpty(chlMsgId)) ? string.Empty : chlMsgId;
            mtdb.DIRECTION = (!direction.HasValue) ? null : direction;
            mtdb.STATUS = (!ddlStatus.HasValue) ? null : ddlStatus;
            mtdb.GSSENDERID = (string.IsNullOrEmpty(gssenderid)) ? string.Empty : gssenderid.Trim();
            mtdb.GSRECEIVERID = (string.IsNullOrEmpty(gsreceiverid)) ? string.Empty : gsreceiverid.Trim();
            mtdb.ISASENDERID = (string.IsNullOrEmpty(isasenderid)) ? string.Empty : isasenderid.Trim();
            mtdb.ISARECEIVERID = (string.IsNullOrEmpty(isareceiverid)) ? string.Empty : isareceiverid.Trim();
            mtdb.KEYWORD_SEARCH = (string.IsNullOrEmpty(keyWordSearch)) ? string.Empty : keyWordSearch.Trim();
            mtdb.SetCreateDateFrom(cdtFrom);
            mtdb.SetCreateDateEnd(cdtEnd);
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            IMTDBCollection imtdbcollection = DataAccess.CreateMTDBCollection(piServer);
            IEnumerable<MT_DB> result = imtdbcollection.Get(mtdb);
            ViewBag.piServer = piServer;
            ViewBag.SqlString = imtdbcollection.GetSqlString();
            ViewBag.SegmentDelimiter = SegmentDelimiter;
            InitDLL((mtdb.STATUS.HasValue) ? mtdb.STATUS.Value.ToString() : "");

            MT_REFDB(piServer, partner, division, region,
                isasenderid, isareceiverid, gssenderid, edimsgtype);
            return View(result);
        }
        private void MT_REFDB(string optradio, string partner, string division, string region,
            string isasenderid, string isareceiverid,
            string gssenderid, string edimsgtype)
        {
            VMTREFDB vmtrefdb = new VMTREFDB();
            vmtrefdb.PARTNER = (string.IsNullOrEmpty(partner)) ? string.Empty : string.Empty;
            vmtrefdb.DIVISION = (string.IsNullOrEmpty(division)) ? string.Empty : string.Empty;
            vmtrefdb.REGION = (string.IsNullOrEmpty(region)) ? string.Empty : string.Empty;
            vmtrefdb.ISASENDERID = (string.IsNullOrEmpty(isasenderid)) ? string.Empty : string.Empty;
            vmtrefdb.ISARECEIVERID = (string.IsNullOrEmpty(isareceiverid)) ? string.Empty : string.Empty;
            vmtrefdb.GSSENDERID = (string.IsNullOrEmpty(gssenderid)) ? string.Empty : string.Empty;
            vmtrefdb.EDIMSGTYPE = (string.IsNullOrEmpty(edimsgtype)) ? string.Empty : string.Empty;

            optradio = Constant.PIQServer;
            optradio = string.IsNullOrEmpty(optradio) ? Constant.PIQServer : optradio;
            //IMTRef imtref = DataAccess.CreateMTREFDB(optradio);
            IMTREFDBCollection imtrefdb = DataAccess.CreateMTREFDBCollection(optradio);
            vmtrefdbList = imtrefdb.Get(vmtrefdb);
            IList<string> partners = vmtrefdbList.OrderBy(x => x.PARTNER).Select(x => x.PARTNER.ToUpper().Trim()).Distinct().ToList();
            IList<string> divisions = vmtrefdbList.OrderBy(x => x.DIVISION).Select(x => x.DIVISION.ToUpper().Trim()).Distinct().ToList();
            IList<string> regions = vmtrefdbList.OrderBy(x => x.REGION).Select(x => x.REGION.ToUpper().Trim()).Distinct().ToList();
            IList<string> isasenderids = vmtrefdbList.OrderBy(x => x.ISASENDERID).Select(x => x.ISASENDERID.ToUpper().Trim()).Distinct().ToList();
            IList<string> isareceiverids = vmtrefdbList.OrderBy(x => x.ISARECEIVERID).Select(x => x.ISARECEIVERID.ToUpper().Trim()).Distinct().ToList();
            IList<string> gssenderids = vmtrefdbList.OrderBy(x => x.GSSENDERID).Select(x => x.GSSENDERID.ToUpper().Trim()).Distinct().ToList();
            IList<string> edimsgtypes = vmtrefdbList.OrderBy(x => x.EDIMSGTYPE).Select(x => x.EDIMSGTYPE.ToUpper().Trim()).Distinct().ToList();

            ViewData["Partners"] = partners;
            ViewData["Divisions"] = divisions;
            ViewData["Regions"] = regions;
            ViewData["ISASenderIds"] = isasenderids;
            ViewData["ISAReceiverIds"] = isareceiverids;
            ViewData["GSSenderIds"] = gssenderids;
            ViewData["EDIMsgTypes"] = edimsgtypes;
        }

        public ActionResult ProcessDB(string Id, string piServer)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIQServer : piServer;
            Constant.Init();
            if (string.IsNullOrEmpty(Id))
                return View();
            IPROCESSDBCollection iprocessdbcollection = DataAccess.CreatePROCESSDBCollection(piServer);
            IEnumerable<vProcessDB> vdb = new List<vProcessDB>();
            if (iprocessdbcollection != null)
                vdb = iprocessdbcollection.Get(Id);
            ViewBag.Id = Id;
            return View(vdb);
        }

        public ActionResult RawData(string id, string piServer, string SegmentDelimiter)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.Message = Constant.RawData;
            IRawData irawdata = DataAccess.CreateRawData(piServer);
            string Rslt = irawdata.Get(id, Constant.ContentTypeUTF8).Content;

            try
            {
                if (!string.IsNullOrEmpty(SegmentDelimiter))
                    Rslt = Rslt.Replace(SegmentDelimiter, "\r\n");

            }
            catch (System.Xml.XmlException)
            {
                if (!string.IsNullOrEmpty(SegmentDelimiter))
                    Rslt = Rslt.Replace(SegmentDelimiter, "\r\n");
            }

            ViewData["RsltRawdata"] = Rslt;

            return View();
        }

        public ActionResult MDN(string id, string piServer)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.Message = Constant.MDN;
            IMDN imdn = DataAccess.CreateMDN(piServer);
            string Rslt = imdn.Get(id, Constant.ContentTypeUTF8).Content;
            ViewData["RsltMDN"] = Rslt;

            return View();
        }

        public ActionResult AuditLog(string id, string piServer)
        {
            try
            {
                piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
                ViewBag.Message = Constant.AuditLog;
                IAuditLog iauditlog = DataAccess.CreateAuditLog(piServer);
                AuditLog r = iauditlog.Get(id, Constant.ContentTypeUTF8);
                if (r.auditLogStrList == null)
                {
                    RouteValueDictionary rv = new RouteValueDictionary();
                    rv.Add("id", id);
                    rv.Add("piServer", piServer);
                    return RedirectToAction("AuditLog2", rv);
                }
                ViewData["RsltAuditLog"] = r;//JsonConvert.SerializeObject(Rslt, Formatting.Indented);
                return View(r);
            }
            catch
            {
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", id);
                rv.Add("piServer", piServer);
                return RedirectToAction("AuditLog2", rv);
            }
        }

        public ActionResult AuditLog2(string id, string piServer)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.Message = Constant.AuditLog;
            IAuditLog iauditlog = DataAccess.CreateAuditLog(piServer);
            AuditLog2 r = iauditlog.Get2(id, Constant.ContentTypeUTF8);

            ViewData["RsltAuditLog2"] = r;//JsonConvert.SerializeObject(Rslt, Formatting.Indented);

            return View(r);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStatusItemMenu(string root, string iWhere)
        {
            var initlist = Constant.LookupMTStatus.Select(x => new { Id = x.CODE, Name = x.DESCRIPTION }).ToList();
            return Json(initlist, JsonRequestBehavior.AllowGet);
        }
    }
}
