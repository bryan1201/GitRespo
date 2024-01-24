using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using einvoice.Models;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace einvoice.Controllers
{
    public class MessageTrackController : Controller
    {
        // GET: MessageTrack
        public ActionResult Index(string eInvServer,  string SEQNO, string MESSAGE_TYPE, string INVOICE_IDENTIFIER, 
            string FROM_PARTY_ID, string TO_PARTY_ID,
            DateTime? CdtFrom, DateTime? CdtEnd)
        {
            try
            {
                eInvServer = (eInvServer == null) ? Constant.DEVServer : eInvServer;
                TURNKEY_MESSAGE_LOG tml = new Models.TURNKEY_MESSAGE_LOG();
                tml.SEQNO = SEQNO;
                tml.MESSAGE_TYPE = MESSAGE_TYPE;
                tml.INVOICE_IDENTIFIER = INVOICE_IDENTIFIER;
                tml.FROM_PARTY_ID = FROM_PARTY_ID;
                tml.TO_PARTY_ID = TO_PARTY_ID;
                tml.SetCreateDateFrom(CdtFrom);
                tml.SetCreateDateEnd(CdtEnd);
                IMTDBCollection imtdbcollection = DataAccess.CreateMTDBCollection(eInvServer);
                IEnumerable<TURNKEY_MESSAGE_LOG> result = imtdbcollection.Get(tml);
                ViewBag.eInvServer = eInvServer;
                ViewBag.SqlString = imtdbcollection.GetSqlString();
                return View(result);
            }
            catch(Exception ex)
            {
                string rslt = ex.Message;
                return View();
            }
        }

        public ActionResult RawData(string eInvServer, string filename)
        {
            eInvServer = (eInvServer == null) ? Constant.DEVServer : eInvServer;
            
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            string xmlstring = string.Empty;
            string ftpUrl = string.Empty;
            try
            {
                IRawDataCollection ir = DataAccess.CreateRawDataCollection(eInvServer);
                xmlstring = ir.GetContent(filename, "text"); // text/xml
                ftpUrl = ir.GetFtpUrl();
                result.Content = new StringContent(xmlstring);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                result.Content = new StringContent(ex.Message);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            ViewData["einvoiceDoc"] = HttpUtility.HtmlEncode(xmlstring);
            ViewBag.eInvServer = eInvServer;
            ViewBag.filename = filename;
            ViewBag.ftpUrl = ftpUrl;

            return View();
        }

        // GET: MessageTrack/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MessageTrack/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageTrack/Create
        [System.Web.Http.HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MessageTrack/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MessageTrack/Edit/5
        [System.Web.Http.HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MessageTrack/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MessageTrack/Delete/5
        [System.Web.Http.HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
