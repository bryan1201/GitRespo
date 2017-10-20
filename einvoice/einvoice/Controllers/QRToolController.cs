using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using einvoice.Models;

namespace einvoice.Controllers
{
    public class QRToolController : Controller
    {
        // GET: QRTool
        public ActionResult Index()
        {
            QRCode qr = new Models.QRCode();
            qr = qr.InitTestQRCode();
            return View(qr);
        }

        // GET: QRTool/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QRTool/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QRTool/Create
        [HttpPost]
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

        // GET: QRTool/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QRTool/Edit/5
        [HttpPost]
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

        // GET: QRTool/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QRTool/Delete/5
        [HttpPost]
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
