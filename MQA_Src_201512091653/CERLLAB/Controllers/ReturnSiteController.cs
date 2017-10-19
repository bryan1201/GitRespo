using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class ReturnSiteController : Controller
    {
        private CERLDBContext db = new CERLDBContext();


        protected void InitDDL(string ddlName, ReturnSite returnsite, string Type)
        {
            int ReturnTypeID = 0;
            ReturnTypeID = (returnsite != null ? int.Parse(returnsite.ReturnTypeID.ToString()) : -1);

            string[] ReturnTypeArray = { "ReturnTypeList" };
            var initlist = Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();

            if (ReturnTypeArray.Contains(ddlName))
            {
                initlist = db.ReturnTypes.ToList().Select(x => new { Id = x.TypeID, Name = x.TypeName }).ToList();
            }

            List<SelectListItem> initList = new List<SelectListItem>();

            if (!(Type == null || Type.Trim().Length == 0))
            {
                initList.Add(new SelectListItem()
                {
                    Text = "",
                    Value = ""
                });
            }

            string selectedvalue = "";

            if (returnsite != null)
            {
                switch (ddlName)
                {
                    case "ReturnTypeList":
                        selectedvalue = ReturnTypeID.ToString();
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in initlist)
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

        protected void InitDDLShow(ReturnSite returnsite, string action)
        {
            InitDDL("ReturnTypeList", returnsite, action);
        }

        //
        // GET: /ReturnSite/

        public ActionResult Index()
        {
            IEnumerable<vReturnSite> vreturnsite = from s in db.ReturnSite
                                                   join t in db.ReturnTypes
                                                   on s.ReturnTypeID equals t.TypeID into ts
                                                   from t in ts.DefaultIfEmpty()
                                                   select new vReturnSite {
                                                       SiteID = s.SiteID,
                                                       SiteNAME = s.SiteNAME,
                                                       ReturnTypeID= s.ReturnTypeID,
                                                       ReturnTypeName = t.TypeName
                                                   };
            return View(vreturnsite.ToList());
        }

        public vReturnSite ViewReturnSite(int id=0)
        {
            ReturnSite returnsite = db.ReturnSite.Find(id);
            ReturnType returntype = db.ReturnTypes.Where(x => x.TypeID == returnsite.ReturnTypeID).FirstOrDefault();
            vReturnSite vreturnsite = new vReturnSite
            {
                SiteID = returnsite.SiteID,
                SiteNAME = returnsite.SiteNAME,
                ReturnTypeID = returnsite.ReturnTypeID,
                ReturnTypeName = returntype.TypeName
            };
            return vreturnsite;
        }

        //
        // GET: /ReturnSite/Details/5
        public ActionResult Details(int id = 0)
        {
            ReturnSite returnsite = db.ReturnSite.Find(id);                                      
            if (returnsite == null)
            {
                return HttpNotFound();
            }
            vReturnSite vreturnsite = ViewReturnSite(id);
            return View(vreturnsite);
        }

        //
        // GET: /ReturnSite/Create

        public ActionResult Create()
        {
            InitDDLShow(null, "");
            return View();
        }

        //
        // POST: /ReturnSite/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReturnSite returnsite)
        {
            if (ModelState.IsValid)
            {
                db.ReturnSite.Add(returnsite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(returnsite);
        }

        //
        // GET: /ReturnSite/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ReturnSite returnsite = db.ReturnSite.Find(id);
            
            if (returnsite == null)
            {
                return HttpNotFound();
            }
            InitDDLShow(returnsite, "");
            vReturnSite vreturnsite = ViewReturnSite(id);
            return View(vreturnsite);
        }

        //
        // POST: /ReturnSite/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReturnSite returnsite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(returnsite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(returnsite);
        }

        //
        // GET: /ReturnSite/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ReturnSite returnsite = db.ReturnSite.Find(id);
            if (returnsite == null)
            {
                return HttpNotFound();
            }
            return View(returnsite);
        }

        //
        // POST: /ReturnSite/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReturnSite returnsite = db.ReturnSite.Find(id);
            db.ReturnSite.Remove(returnsite);
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