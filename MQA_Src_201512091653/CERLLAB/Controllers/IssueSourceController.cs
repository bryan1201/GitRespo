using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class IssueSourceController : Controller
    {
        private CERLDBContext db = new CERLDBContext();

        //
        // GET: /IssueSource/

        public ActionResult Index()
        {
            return View(db.IssueSources.ToList());
        }

        //
        // GET: /IssueSource/Details/5

        public ActionResult Details(int id = 0)
        {
            IssueSource issuesource = db.IssueSources.Find(id);
            if (issuesource == null)
            {
                return HttpNotFound();
            }
            return View(issuesource);
        }

        //
        // GET: /IssueSource/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /IssueSource/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssueSource issuesource)
        {
            if (ModelState.IsValid)
            {
                db.IssueSources.Add(issuesource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(issuesource);
        }

        //
        // GET: /IssueSource/Edit/5

        public ActionResult Edit(int id = 0)
        {
            IssueSource issuesource = db.IssueSources.Find(id);
            if (issuesource == null)
            {
                return HttpNotFound();
            }
            return View(issuesource);
        }

        //
        // POST: /IssueSource/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IssueSource issuesource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issuesource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(issuesource);
        }

        //
        // GET: /IssueSource/Delete/5

        public ActionResult Delete(int id = 0)
        {
            IssueSource issuesource = db.IssueSources.Find(id);
            if (issuesource == null)
            {
                return HttpNotFound();
            }
            return View(issuesource);
        }

        //
        // POST: /IssueSource/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IssueSource issuesource = db.IssueSources.Find(id);
            db.IssueSources.Remove(issuesource);
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