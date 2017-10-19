using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class RoleMenuController : Controller
    {
        private CERLDBContext db = new CERLDBContext();

        //
        // GET: /RoleMenu/

        public ActionResult Index()
        {
            return View(db.RoleMenu.ToList());
        }

        //
        // GET: /RoleMenu/Details/5

        public ActionResult Details(int id = 0)
        {
            RoleMenu rolemenu = db.RoleMenu.Find(id);
            if (rolemenu == null)
            {
                return HttpNotFound();
            }
            return View(rolemenu);
        }

        //
        // GET: /RoleMenu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RoleMenu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleMenu rolemenu)
        {
            if (ModelState.IsValid)
            {
                db.RoleMenu.Add(rolemenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolemenu);
        }

        //
        // GET: /RoleMenu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoleMenu rolemenu = db.RoleMenu.Find(id);
            if (rolemenu == null)
            {
                return HttpNotFound();
            }
            return View(rolemenu);
        }

        //
        // POST: /RoleMenu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleMenu rolemenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolemenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolemenu);
        }

        //
        // GET: /RoleMenu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RoleMenu rolemenu = db.RoleMenu.Find(id);
            if (rolemenu == null)
            {
                return HttpNotFound();
            }
            return View(rolemenu);
        }

        //
        // POST: /RoleMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMenu rolemenu = db.RoleMenu.Find(id);
            db.RoleMenu.Remove(rolemenu);
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