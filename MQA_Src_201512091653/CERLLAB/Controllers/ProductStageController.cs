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
    public class ProductStageController : Controller
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();

        private void InitDDL(string ddlName, string root, int iWhere, string selectedvalue, string Type)
        {
            string[] stringArray = { "siteList", "testList", "requestList" };
            string[] MemberArray = { "SupervisorList", "LocalSupervisorList", "LabMemberList" };

            var initlist = edb.FnGeneralDropDownList(ddlName, root).Select(x => new { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).ToList();

            if (stringArray.Contains(ddlName))
            {
                initlist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl == iWhere).Select(x => new { Id = x.Id, Name = x.Name }).OrderBy(x => x.Id).ToList();
            }

            if (MemberArray.Contains(ddlName))
            {
                initlist = edb.FnMemberDropDownList(ddlName, root).Select(x => new { Id = x.BadgeCode, Name = x.Name }).OrderBy(x => x.Name).ToList();
            }


            List<SelectListItem> initList = new List<SelectListItem>();
            var orderlist = initlist.ToList();
            foreach (var item in orderlist)
            {
                initList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item.Id.ToString() == selectedvalue)
                });
            }
            SelectList cList = new SelectList(initList, "Value", "Text");
            ViewData[ddlName] = cList;
            Session[ddlName] = cList;
        }

        //
        // GET: /ProductStage/

        public ActionResult Index(int? CustomerName)
        {
            int CustomerID = (CustomerName==null)?1:CustomerName.Value;
            InitDDL("CustomerNameList", "0", 0, CustomerID.ToString(), null);
            return View(db.ProductStage.Where(x=>x.CustomerID==CustomerID).ToList());
        }

        //
        // GET: /ProductStage/Details/5

        public ActionResult Details(int id = 0)
        {
            ProductStage productstage = db.ProductStage.Find(id);
            if (productstage == null)
            {
                return HttpNotFound();
            }
            return View(productstage);
        }

        //
        // GET: /ProductStage/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductStage/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductStage productstage)
        {
            if (ModelState.IsValid)
            {
                db.ProductStage.Add(productstage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productstage);
        }

        //
        // GET: /ProductStage/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProductStage productstage = db.ProductStage.Find(id);
            if (productstage == null)
            {
                return HttpNotFound();
            }
            return View(productstage);
        }

        //
        // POST: /ProductStage/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductStage productstage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productstage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productstage);
        }

        //
        // GET: /ProductStage/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProductStage productstage = db.ProductStage.Find(id);
            if (productstage == null)
            {
                return HttpNotFound();
            }
            return View(productstage);
        }

        //
        // POST: /ProductStage/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductStage productstage = db.ProductStage.Find(id);
            db.ProductStage.Remove(productstage);
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