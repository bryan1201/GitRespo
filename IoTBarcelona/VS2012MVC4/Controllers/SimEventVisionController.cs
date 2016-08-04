using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Barcelona.Models;
using Controllers.General;
using System.Text;

namespace VS2012MVC4.Controllers
{
    public class SimEventVisionController : Controller
    {
        private DBContext db = new DBContext();

        public void ClearData(string MyTable)
        {
            string editor = Method.GetLogonUserId(Session, this, User.Identity.Name);
            string sp = "sp_Barcelona";
            string vchCmd = "CLEAR_" + MyTable;
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(MyTable, "Table"));
            vchSet.Append(Method.BuildXML(editor, "editor"));
            //string ClearTable = "TRUNCATE TABLE temp/prd data";
            string sqlCmd = Method.GetSqlCmd(sp, vchCmd: vchCmd, vchObjectName: "", vchSet: vchSet.ToString());
            DAO.sqlCmd(Constant.ConnDBContext, sqlCmd);
        }

        public void MoveData(string MyAction, string MyTable)
        {
            string editor = Method.GetLogonUserId(Session, this, User.Identity.Name);
            string sp = "sp_Barcelona";
            string vchCmd = MyAction;
            StringBuilder vchSet = new StringBuilder();
            vchSet.Append(Method.BuildXML(MyTable, "Table"));
            vchSet.Append(Method.BuildXML(editor, "editor"));
            //string ClearTable = "TRUNCATE TABLE ToolingMatrix";
            string sqlCmd = Method.GetSqlCmd(sp, vchCmd: vchCmd, vchObjectName: "", vchSet: vchSet.ToString());
            DAO.sqlCmd(Constant.ConnDBContext, sqlCmd);
        }

        public ActionResult Import(string FileTable)
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            int i = 0;
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                DataTable workTable = ExcelManager.getExcelSheetData(hpf);
                attachFileTable attachfiletable = db.attachFileTables.Where(x => x.FileTable == FileTable).FirstOrDefault();
                string insert_Table = attachfiletable.TempTable;
                int ColumnsCount = attachfiletable.ColumnsCount;
                if (workTable.Columns.Count > ColumnsCount)
                {
                    for (int c = ColumnsCount; c < workTable.Columns.Count; c++)
                    {
                        DataColumn removedc = workTable.Columns[c];
                        if (workTable.Columns.Count > ColumnsCount)
                        {
                            workTable.Columns.Remove(removedc);
                            c--;
                        }
                    }
                }

                DAO.DatatableToSQL(Constant.ConnDBContext, workTable, insert_Table);
                workTable.Clear();
            }
            return View();
        }


        //
        // GET: /SimEventVision/

        public ActionResult Index()
        {
            return View(db.EventVision.ToList());
        }

        //
        // GET: /SimEventVision/Details/5

        public ActionResult Details(long id = 0)
        {
            EventVision eventvision = db.EventVision.Find(id);
            if (eventvision == null)
            {
                return HttpNotFound();
            }
            return View(eventvision);
        }

        //
        // GET: /SimEventVision/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SimEventVision/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventVision eventvision)
        {
            if (ModelState.IsValid)
            {
                db.EventVision.Add(eventvision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventvision);
        }

        //
        // GET: /SimEventVision/Edit/5

        public ActionResult Edit(long id = 0)
        {
            EventVision eventvision = db.EventVision.Find(id);
            if (eventvision == null)
            {
                return HttpNotFound();
            }
            return View(eventvision);
        }

        //
        // POST: /SimEventVision/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventVision eventvision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventvision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventvision);
        }

        //
        // GET: /SimEventVision/Delete/5

        public ActionResult Delete(long id = 0)
        {
            EventVision eventvision = db.EventVision.Find(id);
            if (eventvision == null)
            {
                return HttpNotFound();
            }
            return View(eventvision);
        }

        //
        // POST: /SimEventVision/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            EventVision eventvision = db.EventVision.Find(id);
            db.EventVision.Remove(eventvision);
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