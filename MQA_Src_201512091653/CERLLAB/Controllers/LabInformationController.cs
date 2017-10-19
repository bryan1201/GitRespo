using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CERLLAB.Models;
using System.IO;
using CERLLAB.Controllers.General;
using System.Web.UI.DataVisualization;
using System.Data.Entity;

namespace CERLLAB.Controllers
{
    public class LabInformationController : Controller
    {
        
        private CERLEntities edb = new CERLEntities();

        //
        // GET: /LabInformation/

        public ActionResult Home(int? id)
        {
            CERLDBContext db = new CERLDBContext();
            int ItemId = -1;
            if (id != null)
            {
                int.TryParse(id.ToString(), out ItemId);
                ViewBag.id = id = ItemId;
            }
            else
                ViewBag.id = id = ItemId;

            IList<vLabInformation> PartialInfoContent = db.vLabInformation.Where(x => x.IsShowMessage == true).ToList();
            IList<vLabInfoAttachFile> PartialInfoImage = db.vLabInfoAttachFile.Where(x => x.IsShowPicture == true).ToList();
            ViewData["PartialInfoContent"] = PartialInfoContent;
            ViewData["PartialInfoImage"] = PartialInfoImage;
            return View();
        }

        public ActionResult Index()
        {
            CERLDBContext db = new CERLDBContext();
            return View(db.vLabInformation.ToList());
        }

        //
        // GET: /LabInformation/Details/5

        public ActionResult Details(int id = 0)
        {
            CERLDBContext db = new CERLDBContext();
            vLabInformation vlabinfo = db.vLabInformation.Find(id);
            if (vlabinfo == null)
            {
                return HttpNotFound();
            }
            InitAttachFiles(vlabinfo.fID);
            return View(vlabinfo);
        }

        public ActionResult GetFilePath(int fileId, bool record)
        {
            CERLDBContext db = new CERLDBContext();
            if (fileId == 0)
            {
                return View();
            }
            string result = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                             select f.filePath + f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
            {
                return File(System.IO.File.ReadAllBytes(result), "application/unknown", HttpUtility.UrlEncode(Path.GetFileName(result)));
            }
            else
                return View("File not exists: " + result);
        }

        public void InitAttachFiles(string fID)
        {
            AttachFileSet attSet = new AttachFileSet();
            ViewData["attFileLabInformation"] = attSet.GetFiles(fID, "LabInformation");
        }

        //
        // GET: /LabInformation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /LabInformation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection labInfo, LabInformation labinformation)
        {
            CERLDBContext db = new CERLDBContext();
            string FolderId = "LabInformation";
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            string FormId = System.Guid.NewGuid().ToString().ToUpper();
            labinformation.fID = FormId;
            labinformation.editor = Editor;
            labinformation.cdt = DateTime.Now;
            labinformation.udt = DateTime.Now;

            if (ModelState.IsValid)
            {
                var r = new List<attachFile>();
                int i = 0;

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;

                    string formId = FormId;
                    string filePath = Constant.UserFileDirectory + FolderId + @"\" + formId + @"\";
                    FileInfo newinfo = new FileInfo(hpf.FileName);
                    string savedFileName = Path.Combine(filePath, Path.GetFileName(newinfo.Name));

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    hpf.SaveAs(savedFileName);

                    r.Add(new attachFile()
                    {
                        fID = formId,
                        displayname = newinfo.Name,
                        fileName = newinfo.Name,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        folderId = FolderId,
                        editor = Editor,
                        filePath = filePath,

                        cdt = DateTime.Now,
                        udt = DateTime.Now
                    });

                    i++;
                }
                foreach (attachFile a in r)
                {
                    db.attachFiles.Add(a);
                }

                db.LabInformation.Add(labinformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(labinformation);
        }

        //
        // GET: /LabInformation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CERLDBContext db = new CERLDBContext();
            vLabInformation info = db.vLabInformation.Find(id);
            if (info == null)
            {
                return HttpNotFound();
            }

            InitAttachFiles(info.fID);
            return View(info);
        }

        //
        // POST: /LabInformation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection labInfo, LabInformation labinformation)
        {
            CERLDBContext db = new CERLDBContext();
            string UserId = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            string FolderId = "LabInformation";
            if (ModelState.IsValid)
            {
                labinformation.udt = DateTime.Now;
                labinformation.editor = UserId;
                labinformation.Width = (labinformation.Width == null || labinformation.Width.Trim() == "") ? "100%" : labinformation.Width;
                labinformation.Height = (labinformation.Height == null || labinformation.Height.Trim() == "") ? "100%" : labinformation.Height;

                var r = new List<attachFile>();
                int i = 0;

                foreach (string file in Request.Files)
                {

                    HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;

                    string formId = labinformation.fID;
                    string filePath = Constant.UserFileDirectory + FolderId + @"\" + formId + @"\";
                    FileInfo newinfo = new FileInfo(hpf.FileName);
                    string savedFileName = Path.Combine(filePath, Path.GetFileName(newinfo.Name));

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    hpf.SaveAs(savedFileName);
                    int count = db.attachFiles.Where(x => x.fID == formId && x.fileName == newinfo.Name).Count();

                    if (count == 0)
                    {
                        r.Add(new attachFile()
                        {
                            fID = formId,
                            displayname = newinfo.Name,
                            fileName = newinfo.Name,
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            folderId = FolderId,
                            editor = UserId,
                            filePath = filePath,

                            cdt = DateTime.Now,
                            udt = DateTime.Now
                        });
                    }
                    i++;
                }
                foreach (attachFile a in r)
                {
                    db.attachFiles.Add(a);
                }

                db.Entry(labinformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(labinformation);
        }

        //
        // GET: /LabInformation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CERLDBContext db = new CERLDBContext();
            LabInformation labinformation = db.LabInformation.Find(id);
            if (labinformation == null)
            {
                return HttpNotFound();
            }
            return View(labinformation);
        }

        //
        // POST: /LabInformation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CERLDBContext db = new CERLDBContext();
            LabInformation labinformation = db.LabInformation.Find(id);
            db.LabInformation.Remove(labinformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            CERLDBContext db = new CERLDBContext();
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}