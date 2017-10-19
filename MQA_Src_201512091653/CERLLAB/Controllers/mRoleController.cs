using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CERLLAB.Models;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class mRoleController : Controller
    {
        private CERLDBContext db = new CERLDBContext();
        private CERLEntities edb = new CERLEntities();

        protected void InitDDL(string ddlName, vRole vrole, string Type)
        {
            int RoleId = 0;
            int menuId = 0;
            RoleId = (vrole != null ? vrole.RoleId : -1);
            menuId = (vrole != null ? int.Parse(vrole.menuId.ToString()) : -1);
            string[] RoleArray = { "RoleList"};
            string[] CERLMenuArray = { "CERLMenuList" };
            string[] TestItemArray = { "TestItemList" };
            var initlist = Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();
            if(RoleArray.Contains(ddlName))
             initlist = db.mRoles.Select(x => new { Id = x.RoleId, Name = x.RoleName }).ToList();

            if (CERLMenuArray.Contains(ddlName))
            {
                var existslist = edb.FnRoleMenuFunction(RoleId).Select(x => x.menuId).ToList();
                initlist = edb.FnCERLMenuDropDownList("0").Where(x => !existslist.Contains(x.Id)).ToList().Select(x => new { Id = int.Parse(x.Id.ToString()), Name = x.Name }).ToList();
                //initlist = db.CERLMenu.Where(x=>!existslist.Contains(x.menuId)).Select(x => new { Id = x.menuId, Name = x.menuText }).ToList();
            }

            if (TestItemArray.Contains(ddlName))
            {
                initlist = edb.FnTestItemMenuDropDownList("0").Where(x => x.lvl == 1).ToList().Select(x => new { Id = int.Parse(x.Id.ToString()), Name = x.Name }).ToList();
                //initlist = db.CERLMenu.Where(x=>!existslist.Contains(x.menuId)).Select(x => new { Id = x.menuId, Name = x.menuText }).ToList();
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

            if (vrole != null)
            {
                switch (ddlName)
                {
                    case "RoleList":
                        selectedvalue = RoleId.ToString();
                        break;
                    case "TestItemList":
                        selectedvalue = vrole.menuId.ToString();
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

        protected void InitDDLShow(vRole vrole, string action)
        {
            InitDDL("RoleList", vrole, action);
            InitDDL("CERLMenuList",vrole, action);
            InitDDL("TestItemList", vrole, action);

            int RoleId = 0;
            int menuId = 0;
            RoleId = (vrole != null ? vrole.RoleId : -1);
            menuId = (vrole != null ? int.Parse(vrole.menuId.ToString()) : -1);
            IQueryable<vUserRole> vuserrole = db.vUserRole.Where(x => x.RoleId == RoleId).Take(100); //列出此角色下的人員
            if(Session["QryBadgeCode"]!=null)
            {
                string BadgeCode = (string)Session["QryBadgeCode"];
                vuserrole = db.vUserRole.Where(x => x.RoleId == RoleId && x.BadgeCode.Contains(BadgeCode)).Take(100); //列出此角色下的人員
                Session["QryBadgeCode"] = null;
            }
            IQueryable<vRoleMenu> vrolemenu = db.vRoleMenu.Where(x => x.RoleId == RoleId); //列出此角色下使用中的目錄
            ViewData["RoleId"] = RoleId;
            ViewData["menuId"] = menuId;
            ViewData["vuserrole"] = vuserrole;
            ViewData["vrolemenu"] = vrolemenu;
        }

        //
        // GET: /mRole/

        public ActionResult Index()
        {
            int root = 0;
            var sitelist = edb.FnTestItemMenuDropDownList(root.ToString()).Where(y => y.lvl == 0).Select(x => new { Id = x.Id, Name = x.Name }).ToList();

            List<SelectListItem> siteList = new List<SelectListItem>();

            foreach (var item in sitelist)
            {
                siteList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = (item.Id == root.ToString())
                });
            }

            SelectList mList = new SelectList(siteList, "Value", "Text", "");
            ViewData["siteList"] = siteList;
            return View(db.vRole.ToList());
        }

        //
        // GET: /mRole/Details/5

        public ActionResult Details(int id = 0)
        {
            vRole vrole = db.vRole.Find(id);
            if (vrole == null)
            {
                return HttpNotFound();
            }
            InitDDLShow(vrole, "");
            return View(vrole);
        }

        public ActionResult AddRoleMenu(int? RoleId, int? menuId)
        {
            if (RoleId.HasValue && menuId.HasValue)
            {
                int rId = int.Parse(RoleId.ToString());
                int mId = int.Parse(menuId.ToString());
                var currrolemenu = db.RoleMenu.Where(x => x.RoleId == rId && x.menuId == mId).Select(x => x.id);
                if (currrolemenu == null || currrolemenu.Count() == 0)
                {
                    db.RoleMenu.Add(new RoleMenu()
                    {
                        RoleId = int.Parse(RoleId.ToString()),
                        menuId = int.Parse(menuId.ToString())
                    });
                    db.SaveChanges();
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveRoleMenu(int? id)
        {
            if(id.HasValue)
            {
                RoleMenu rolemenu = db.RoleMenu.Find(id);
                db.RoleMenu.Remove(rolemenu);
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult QueryUserRole(int? RoleId, string BadgeCode)
        {
            Session["QryBadgeCode"] = BadgeCode;
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult AddRoleUser(int? RoleId, string BadgeCode, string Description)
        {
            if (RoleId.HasValue && BadgeCode != null)
            {
                if (BadgeCode.Trim().Length >= 9)
                {
                    BadgeCode = BadgeCode.Trim().ToUpper();
                    var currroleuser = db.UserRole.ToList().Where(x => x.BadgeCode == BadgeCode && x.RoleId == int.Parse(RoleId.ToString())).Select(x=>x.id);
                    var existuser = db.userprofile.Where(x => x.BadgeCode == BadgeCode).Select(x => x.BadgeCode);
                    if (existuser != null && existuser.Count() > 0)
                    {
                        if (currroleuser.Count() == 0)
                        {
                            db.UserRole.Add(new UserRole()
                            {
                                RoleId = int.Parse(RoleId.ToString()),
                                BadgeCode = BadgeCode,
                                Description = Description
                            });
                        }
                        else
                        {
                            UserRole userrole = (UserRole)db.UserRole.ToList().Where(x => x.BadgeCode == BadgeCode && int.Parse(x.RoleId.ToString())==RoleId).FirstOrDefault();
                            userrole.Description = Description;
                            db.Entry(userrole).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveRoleUser(int? id)
        {
            if (id.HasValue)
            {
                UserRole ur = db.UserRole.Find(id);
                db.UserRole.Remove(ur);
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        //
        // GET: /mRole/Create

        public ActionResult Create()
        {
            InitDDLShow(null, "");
            return View();
        }

        //
        // POST: /mRole/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(mRole mrole)
        {
            if (ModelState.IsValid)
            {
                db.mRoles.Add(mrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mrole);
        }

        //
        // GET: /mRole/Edit/5

        public ActionResult Edit(int id = 0)
        {
            vRole vrole = db.vRole.Find(id);
            
            if (vrole == null)
            {
                return HttpNotFound();
            }
            InitDDLShow(vrole, "Edit");
            return View(vrole);
        }

        //
        // POST: /mRole/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(mRole mrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            vRole vrole = db.vRole.Find(mrole.id);
            return View(vrole);
        }

        //
        // GET: /mRole/Delete/5

        public ActionResult Delete(int id = 0)
        {
            vRole vrole = db.vRole.Find(id);
            if (vrole == null)
            {
                return HttpNotFound();
            }
            return View(vrole);
        }

        //
        // POST: /mRole/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mRole mrole = db.mRoles.Find(id);
            db.mRoles.Remove(mrole);
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