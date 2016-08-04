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
    public class BuyInfoesController : Controller
    {
        private SmartShoppingDemoServiceContext db = new SmartShoppingDemoServiceContext();

        // GET: BuyInfoes
        public ActionResult Index()
        {
            return View(db.BuyInfoes.ToList().OrderByDescending(data => data.Id));
        }

        // GET: BuyInfoes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyInfo buyInfo = db.BuyInfoes.Find(id);
            if (buyInfo == null)
            {
                return HttpNotFound();
            }
            return View(buyInfo);
        }

        // GET: BuyInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BuyInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceId,ProductId,Quantity,Amount,BuyTime")] BuyInfo buyInfo)
        {
            if (ModelState.IsValid)
            {
                db.BuyInfoes.Add(buyInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(buyInfo);
        }

        // GET: BuyInfoes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyInfo buyInfo = db.BuyInfoes.Find(id);
            if (buyInfo == null)
            {
                return HttpNotFound();
            }
            return View(buyInfo);
        }

        // POST: BuyInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeviceId,ProductId,Quantity,Amount,BuyTime")] BuyInfo buyInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buyInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buyInfo);
        }

        // GET: BuyInfoes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyInfo buyInfo = db.BuyInfoes.Find(id);
            if (buyInfo == null)
            {
                return HttpNotFound();
            }
            return View(buyInfo);
        }

        // POST: BuyInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            BuyInfo buyInfo = db.BuyInfoes.Find(id);
            db.BuyInfoes.Remove(buyInfo);
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
