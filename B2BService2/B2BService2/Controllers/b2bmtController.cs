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
                    Selected = (!string.IsNullOrEmpty(selectedvalue))?(item.Id.ToString() == selectedvalue): (item.Id.ToString() == "")
                });
            }

            SelectList cList = new SelectList(initList, "Value", "Text");
            ViewData["ddlSTATUS"] = cList;
            ViewBag.ddlSTATUS = cList;
        }

        public ActionResult MT_DB(string piServer, string docnum, string msgid, string parent, string controlnum, string idoc,
            string edimsgtype, string chlMsgId, decimal? direction, decimal? ddlStatus,
            string gssenderid, string gsreceiverid,
            string isasenderid, string isareceiverid, DateTime? cdtFrom, DateTime? cdtEnd, string keyWordSearch)
        {
            MT_DB mtdb = new Models.MT_DB();
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
            mtdb.GSRECEIVERID= (string.IsNullOrEmpty(gsreceiverid)) ? string.Empty : gsreceiverid.Trim();
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
            InitDLL((mtdb.STATUS.HasValue)?mtdb.STATUS.Value.ToString():"");
            return View(result);
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

        public ActionResult RawData(string id, string piServer)
        {
            piServer = string.IsNullOrEmpty(piServer) ? Constant.PIPServer : piServer;
            ViewBag.Message = Constant.RawData;
            IRawData irawdata = DataAccess.CreateRawData(piServer);
            string Rslt = irawdata.Get(id, Constant.ContentTypeUTF8).Content;
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
                if(r.auditLogStrList == null)
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