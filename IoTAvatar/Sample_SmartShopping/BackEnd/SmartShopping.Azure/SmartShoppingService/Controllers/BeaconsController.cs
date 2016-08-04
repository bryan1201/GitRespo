using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartShoppingDemoService.Models;

namespace SmartShoppingDemoService.Controllers
{
    public class BeaconsController : Controller
    {
        private SmartShoppingDemoServiceContext db = new SmartShoppingDemoServiceContext();

        // GET: Beacons
        public ActionResult Index()
        {
            return View(db.Beacons.ToList().OrderByDescending(data => data.Id));
        }

        // GET: Beacons/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beacon beacon = db.Beacons.Find(id);
            if (beacon == null)
            {
                return HttpNotFound();
            }
            return View(beacon);
        }

        // GET: Beacons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beacons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BeaconId,ProductId,StoreId,InFilter,OutFilter,Xaxis,Yaxis,Longitude,LatuLatitude")] Beacon beacon)
        {
            if (ModelState.IsValid)
            {
                db.Beacons.Add(beacon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beacon);
        }

        // GET: Beacons/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beacon beacon = db.Beacons.Find(id);
            if (beacon == null)
            {
                return HttpNotFound();
            }
            return View(beacon);
        }

        // POST: Beacons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BeaconId,ProductId,StoreId,InFilter,OutFilter,Xaxis,Yaxis,Longitude,LatuLatitude")] Beacon beacon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beacon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beacon);
        }

        // GET: Beacons/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beacon beacon = db.Beacons.Find(id);
            if (beacon == null)
            {
                return HttpNotFound();
            }
            return View(beacon);
        }

        // POST: Beacons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Beacon beacon = db.Beacons.Find(id);
            db.Beacons.Remove(beacon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
