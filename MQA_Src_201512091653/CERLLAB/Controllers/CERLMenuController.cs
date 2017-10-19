using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using CERLLAB.Controllers.General;
using System.Data.Entity;
namespace CERLLAB.Controllers
{
    public class CERLMenuController : Controller
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();

        protected void InitDDL(string ddlName, vCERLMenu vcerlmenu, string Type)
        {
            int parentMenuId = 0;
            parentMenuId = (vcerlmenu != null ? int.Parse(vcerlmenu.parentMenuId.ToString()) : -1);

            string[] CERLMenuArray = { "CERLMenuList" };
            var initlist = Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();

            if (CERLMenuArray.Contains(ddlName))
            {
                initlist = edb.FnCERLMenuDropDownList("0").ToList().Select(x => new { Id = int.Parse(x.Id.ToString()), Name = x.Name }).ToList();
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

            if (vcerlmenu != null)
            {
                switch (ddlName)
                {
                    case "CERLMenuList":
                        selectedvalue = parentMenuId.ToString();
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

        protected void InitDDLShow(vCERLMenu vcerlmenu, string action)
        {
            InitDDL("CERLMenuList", vcerlmenu, action);
        }
        //
        // GET: /CERLMenu/

        public ActionResult Index()
        {
            return View(db.vcerlmenu.ToList());
        }

        //
        // GET: /CERLMenu/Details/5

        public ActionResult Details(int id = 0)
        {
            vCERLMenu vcerlmenu = db.vcerlmenu.Find(id);
            if (vcerlmenu == null)
            {
                return HttpNotFound();
            }
            return View(vcerlmenu);
        }

        //
        // GET: /CERLMenu/Create

        public ActionResult Create()
        {
            InitDDLShow(null, "Edit");
            return View();
        }

        //
        // POST: /CERLMenu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CERLMenu cerlmenu)
        {
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            cerlmenu.cdt = DateTime.Now;
            cerlmenu.udt = DateTime.Now;
            cerlmenu.editor = UserId;
            ModelState.Remove("cdt");
            ModelState.Remove("udt");
            ModelState.Remove("editor");
            if (TryUpdateModel(cerlmenu, null, null, new string[] {"udt","cdt", "editor" }))
            {
                db.CERLMenu.Add(cerlmenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            InitDDLShow(null, "");
            return View(cerlmenu);
        }

        //
        // GET: /CERLMenu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            vCERLMenu vcerlmenu = db.vcerlmenu.Find(id);
            if (vcerlmenu == null)
            {
                return HttpNotFound();
            }
            InitDDLShow(vcerlmenu, "Edit");
            return View(vcerlmenu);
        }

        //
        // POST: /CERLMenu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CERLMenu cerlmenu)
        {
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());

            if (ModelState.IsValid)
            {
                cerlmenu.editor = UserId;
                cerlmenu.udt = System.DateTime.Now;
                db.Entry(cerlmenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cerlmenu);
        }

        //
        // GET: /CERLMenu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CERLMenu cerlmenu = db.CERLMenu.Find(id);
            if (cerlmenu == null)
            {
                return HttpNotFound();
            }
            return View(cerlmenu);
        }

        //
        // POST: /CERLMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CERLMenu cerlmenu = db.CERLMenu.Find(id);
            db.CERLMenu.Remove(cerlmenu);
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