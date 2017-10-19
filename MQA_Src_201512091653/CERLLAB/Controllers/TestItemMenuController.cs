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
    public class TestItemMenuController : Controller
    {
        private CERLDBContext db = new CERLDBContext();
        CERLEntities edb = new CERLEntities();

        protected void InitDDL(string ddlName, vTestItemMenu vtestitemmenu, string Type)
        {
            int parentMenuId = 0;
            parentMenuId = (vtestitemmenu != null ? int.Parse(vtestitemmenu.parentMenuId.ToString()) : -1);

            string[] TestItemMenuArray = { "TestItemMenuList" };
            var initlist = Enumerable.Empty<object>().Select(r => new { Id = 0, Name = "" }).ToList();

            if (TestItemMenuArray.Contains(ddlName))
            {
                initlist = edb.FnTestItemMenuDropDownList("0").ToList().Select(x => new { Id = int.Parse(x.Id.ToString()), Name = x.Name }).ToList();
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

            if (vtestitemmenu != null)
            {
                switch (ddlName)
                {
                    case "TestItemMenuList":
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

        protected void InitDDLShow(vTestItemMenu vtestitemmenu, string action)
        {
            InitDDL("TestItemMenuList", vtestitemmenu, action);
        }

        public void InitTreeAndPath()
        {
            int RoleId = Constant.UserRoleId;
            string UserId = Method.GetLogonUserId(Session, User.Identity.Name);

            string webroot = Constant.WebRoot;
            string TreeViewRoot = webroot + "/TestItemMenu/Index";
            ItemTreeDbSet tr = new ItemTreeDbSet(TreeViewRoot);
            ItemTree tree = tr.GetData(TreeViewRoot, UserId, 0, mIsShowReport: 1, mIsShowUnsignForm: 0);
            ViewData["ItemTree"] = tree;

            IList<FnGetItemParentListById_Result> parentlist = tr.GetParentList(ViewBag.id);
            ViewData["parentlist"] = parentlist;
        }

        //
        // GET: /TestItemMenu/

        public ActionResult Index(int? id)
        {
            if (id != null)
                ViewBag.id = id;
            else
                ViewBag.id = id = 0;

            var itemlist = edb.FnTestItemMenuDropDownList(id.ToString()).Select(x => x.Id).ToList();
            IList<int> itemList = new List<int>();
            foreach (var item in itemlist)
            {
                int i = int.Parse(item);
                itemList.Add(i);
            }
            InitTreeAndPath();
            return View(db.vTestItemMenus.Where(x => itemList.Contains(x.menuId)).ToList());
        }

        //
        // GET: /TestItemMenu/Details/5

        public ActionResult Details(int id = 0)
        {
            vTestItemMenu vtestitemmenu = db.vTestItemMenus.Find(id);
            ViewBag.id = vtestitemmenu.parentMenuId;
            if (vtestitemmenu == null)
            {
                return HttpNotFound();
            }
            InitTreeAndPath();
            string kk = vtestitemmenu.ParentMenuName;
            return View(vtestitemmenu);
        }

        //
        // GET: /TestItemMenu/Create

        public ActionResult Create(int? id)
        {
            if (id != null)
                ViewBag.id = id;
            else
                ViewBag.id = id = 0;
            vTestItemMenu vtestitemmenu = new vTestItemMenu();
            vtestitemmenu.parentMenuId = int.Parse(id.ToString());
            vtestitemmenu.FlowCode = 1001001;
            InitTreeAndPath();
            InitDDLShow(vtestitemmenu, "Create");
            return View(vtestitemmenu);
        }

        //
        // POST: /TestItemMenu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestItemMenu testitemmenu)
        {
            if (ModelState.IsValid)
            {
                db.TestItemMenu.Add(testitemmenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testitemmenu);
        }

        //
        // GET: /TestItemMenu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            vTestItemMenu vtestitemmenu = db.vTestItemMenus.Find(id);
            ViewBag.id = vtestitemmenu.parentMenuId;
            if (vtestitemmenu == null)
            {
                return HttpNotFound();
            }
            InitTreeAndPath();
            InitDDLShow(vtestitemmenu, "Edit");
            return View(vtestitemmenu);
        }

        //
        // POST: /TestItemMenu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestItemMenu testitemmenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testitemmenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testitemmenu);
        }

        //
        // GET: /TestItemMenu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            vTestItemMenu vtestitemmenu = db.vTestItemMenus.Find(id);
            if (vtestitemmenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = vtestitemmenu.parentMenuId;
            InitTreeAndPath();
            return View(vtestitemmenu);
        }

        //
        // POST: /TestItemMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestItemMenu testitemmenu = db.TestItemMenu.Find(id);
            db.TestItemMenu.Remove(testitemmenu);
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