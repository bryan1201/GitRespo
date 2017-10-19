using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class ProcessStepController : Controller
    {
        private CERLDBContext db = new CERLDBContext();

        //
        // GET: /ProcessStep/

        public ActionResult Index()
        {
            return View(db.ProcessSteps.OrderBy(x=>x.TextValue).ToList());
        }

        //
        // GET: /ProcessStep/Details/5

        public ActionResult Details(int id = 0)
        {
            ProcessStep processstep = db.ProcessSteps.Find(id);
            if (processstep == null)
            {
                return HttpNotFound();
            }
            return View(processstep);
        }

        //
        // GET: /ProcessStep/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProcessStep/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProcessStep processstep)
        {
            if (ModelState.IsValid)
            {
                db.ProcessSteps.Add(processstep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(processstep);
        }

        //
        // GET: /ProcessStep/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProcessStep processstep = db.ProcessSteps.Find(id);
            if (processstep == null)
            {
                return HttpNotFound();
            }
            return View(processstep);
        }

        //
        // POST: /ProcessStep/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProcessStep processstep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(processstep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(processstep);
        }

        //
        // GET: /ProcessStep/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProcessStep processstep = db.ProcessSteps.Find(id);
            if (processstep == null)
            {
                return HttpNotFound();
            }
            return View(processstep);
        }

        //
        // POST: /ProcessStep/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProcessStep processstep = db.ProcessSteps.Find(id);
            db.ProcessSteps.Remove(processstep);
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