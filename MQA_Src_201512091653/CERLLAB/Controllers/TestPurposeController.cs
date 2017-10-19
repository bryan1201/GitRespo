using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class TestPurposeController : Controller
    {
        private CERLDBContext db = new CERLDBContext();

        //
        // GET: /TestPurpose/

        public ActionResult Index()
        {
            return View(db.TestPurposes.ToList());
        }

        //
        // GET: /TestPurpose/Details/5

        public ActionResult Details(int id = 0)
        {
            TestPurpose testpurpose = db.TestPurposes.Find(id);
            if (testpurpose == null)
            {
                return HttpNotFound();
            }
            return View(testpurpose);
        }

        //
        // GET: /TestPurpose/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TestPurpose/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestPurpose testpurpose)
        {
            if (ModelState.IsValid)
            {
                db.TestPurposes.Add(testpurpose);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testpurpose);
        }

        //
        // GET: /TestPurpose/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TestPurpose testpurpose = db.TestPurposes.Find(id);
            if (testpurpose == null)
            {
                return HttpNotFound();
            }
            return View(testpurpose);
        }

        //
        // POST: /TestPurpose/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestPurpose testpurpose)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testpurpose).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testpurpose);
        }

        //
        // GET: /TestPurpose/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TestPurpose testpurpose = db.TestPurposes.Find(id);
            if (testpurpose == null)
            {
                return HttpNotFound();
            }
            return View(testpurpose);
        }

        //
        // POST: /TestPurpose/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestPurpose testpurpose = db.TestPurposes.Find(id);
            db.TestPurposes.Remove(testpurpose);
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