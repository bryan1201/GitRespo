using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Barcelona.Models;

namespace VS2012MVC4.Controllers
{
    public class ApplicationController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Application/

        public ActionResult Index()
        {
            return View(db.application.ToList());
        }

        //
        // GET: /Application/Details/5

        public ActionResult Details(string id = null)
        {
            Applications applications = db.application.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // GET: /Application/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Application/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Applications applications)
        {
            if (ModelState.IsValid)
            {
                applications.ApplicationId = System.Guid.NewGuid();
                db.application.Add(applications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applications);
        }

        //
        // GET: /Application/Edit/5

        public ActionResult Edit(Guid id)
        {
            Applications applications = db.application.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // POST: /Application/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Applications applications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applications);
        }

        //
        // GET: /Application/Delete/5

        public ActionResult Delete(Guid id)
        {
            Applications applications = db.application.Find(id);
            if (applications == null)
            {
                return HttpNotFound();
            }
            return View(applications);
        }

        //
        // POST: /Application/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Applications applications = db.application.Find(id);
            db.application.Remove(applications);
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