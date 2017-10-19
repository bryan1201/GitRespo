using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class CustomerNameController : Controller
    {
        private CERLDBContext db = new CERLDBContext();

        //
        // GET: /CustomerName/

        public ActionResult Index()
        {
            return View(db.CustomerNames.ToList());
        }

        //
        // GET: /CustomerName/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomerName customername = db.CustomerNames.Find(id);
            if (customername == null)
            {
                return HttpNotFound();
            }
            return View(customername);
        }

        //
        // GET: /CustomerName/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CustomerName/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerName customername)
        {
            if (ModelState.IsValid)
            {
                db.CustomerNames.Add(customername);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customername);
        }

        //
        // GET: /CustomerName/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomerName customername = db.CustomerNames.Find(id);
            if (customername == null)
            {
                return HttpNotFound();
            }
            return View(customername);
        }

        //
        // POST: /CustomerName/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerName customername)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customername).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customername);
        }

        //
        // GET: /CustomerName/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomerName customername = db.CustomerNames.Find(id);
            if (customername == null)
            {
                return HttpNotFound();
            }
            return View(customername);
        }

        //
        // POST: /CustomerName/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerName customername = db.CustomerNames.Find(id);
            db.CustomerNames.Remove(customername);
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