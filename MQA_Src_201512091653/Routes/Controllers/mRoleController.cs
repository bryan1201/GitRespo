using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Routes.Models;

namespace Routes.Controllers
{
    public class mRoleController : Controller
    {
        private RouteDBContext db = new RouteDBContext();

        //
        // GET: /mRole/

        public ActionResult Index()
        {
            return View(db.mRoles.ToList());
        }

        //
        // GET: /mRole/Details/5

        public ActionResult Details(int id = 0)
        {
            mRole mrole = db.mRoles.Find(id);
            if (mrole == null)
            {
                return HttpNotFound();
            }
            return View(mrole);
        }

        //
        // GET: /mRole/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /mRole/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(mRole mrole)
        {
            if (ModelState.IsValid)
            {
                db.mRoles.Add(mrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mrole);
        }

        //
        // GET: /mRole/Edit/5

        public ActionResult Edit(int id = 0)
        {
            mRole mrole = db.mRoles.Find(id);
            if (mrole == null)
            {
                return HttpNotFound();
            }
            return View(mrole);
        }

        //
        // POST: /mRole/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(mRole mrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mrole);
        }

        //
        // GET: /mRole/Delete/5

        public ActionResult Delete(int id = 0)
        {
            mRole mrole = db.mRoles.Find(id);
            if (mrole == null)
            {
                return HttpNotFound();
            }
            return View(mrole);
        }

        //
        // POST: /mRole/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mRole mrole = db.mRoles.Find(id);
            db.mRoles.Remove(mrole);
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