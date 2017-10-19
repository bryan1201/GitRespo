using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CERLLAB.Models;

namespace CERLLAB.Controllers
{
    public class PCBVendorController : Controller
    {
        private CERLDBContext db = new CERLDBContext();

        //
        // GET: /PCBVendor/

        public ActionResult Index()
        {
            return View(db.PCBVendor.ToList());
        }

        //
        // GET: /PCBVendor/Details/5

        public ActionResult Details(int id = 0)
        {
            PCBVendor pcbvendor = db.PCBVendor.Find(id);
            if (pcbvendor == null)
            {
                return HttpNotFound();
            }
            return View(pcbvendor);
        }

        //
        // GET: /PCBVendor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PCBVendor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PCBVendor pcbvendor)
        {
            if (ModelState.IsValid)
            {
                db.PCBVendor.Add(pcbvendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pcbvendor);
        }

        //
        // GET: /PCBVendor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PCBVendor pcbvendor = db.PCBVendor.Find(id);
            if (pcbvendor == null)
            {
                return HttpNotFound();
            }
            return View(pcbvendor);
        }

        //
        // POST: /PCBVendor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PCBVendor pcbvendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pcbvendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pcbvendor);
        }

        //
        // GET: /PCBVendor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PCBVendor pcbvendor = db.PCBVendor.Find(id);
            if (pcbvendor == null)
            {
                return HttpNotFound();
            }
            return View(pcbvendor);
        }

        //
        // POST: /PCBVendor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PCBVendor pcbvendor = db.PCBVendor.Find(id);
            db.PCBVendor.Remove(pcbvendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}