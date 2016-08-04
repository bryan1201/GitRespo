using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Barcelona.Models;
using Controllers.General;

namespace VS2012MVC4.Controllers
{
    public class ShareFileGroupsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: ShareFileGroups
        public ActionResult Index()
        {
            return View(db.sharefilegroup.ToList());
        }

        // GET: ShareFileGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareFileGroup shareFileGroup = db.sharefilegroup.Find(id);
            if (shareFileGroup == null)
            {
                return HttpNotFound();
            }
            return View(shareFileGroup);
        }

        // GET: ShareFileGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShareFileGroups/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,LimitSize,UsageSize,SDate,EDate,SummaryAdvice")] ShareFileGroup shareFileGroup)
        {
            DateTime today = DateTime.Now;
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            if (ModelState.IsValid)
            {
                shareFileGroup.editor = Editor;
                shareFileGroup.cdt = today;
                shareFileGroup.udt = today;
                shareFileGroup.GroupId = Guid.NewGuid();
                db.sharefilegroup.Add(shareFileGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shareFileGroup);
        }

        public IEnumerable<vSharedFile> GetUnSelectedFile(Guid? id)
        {
            DateTime today = DateTime.Now;
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            IEnumerable<vSharedFile> vsfall = new List<vSharedFile>();
            if (id == null)
            {
                return vsfall;
            }
            List<Int64> fileIds = db.vsharefile.Where(x => x.editor == Editor && x.GroupId == id).Select(x => x.fileId).ToList();
            IEnumerable<attachFile> att = db.attachFiles.Where(x => x.editor == Editor && !fileIds.Contains(x.fileId));
            vsfall = from t in att
                     select new vSharedFile
                     {
                         SLId = Guid.NewGuid(),
                         fileId=t.fileId,
                         GroupId = id.Value,
                         udt=today,
                         cdt=today,
                         displayname = t.displayname,
                         fileName=t.fileName,
                         filePath = t.filePath,
                         editor=Editor
                     };
            return vsfall;
        }

        public IEnumerable<vShareFileGroupUsers> GetUnShareFileGroupUsers(Guid? id)
        {
            DateTime today = DateTime.Now;
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());

            IEnumerable<vShareFileGroupUsers> retsfgu = new List<vShareFileGroupUsers>();
            IEnumerable<string> sfguseremail = db.vshareFileGroupUsers.Where(x=>x.GroupId==id).Select(x=>x.UserEmail);
            IEnumerable<vUser> vuser = db.vuser.Where(x=>!sfguseremail.Contains(x.Email));
            retsfgu = from t in vuser
                      select new vShareFileGroupUsers
                     {
                         Id = Guid.NewGuid(),
                         GroupId = id.Value,
                         UserId = t.UserId,
                         UserName = t.UserName,
                         UserEmail = t.Email,
                         AppDescription = t.AppDescription,
                         ApplicationName = t.ApplicationName,
                         ApplicationId = t.ApplicationId
                     };
            return retsfgu;
        }

        // GET: ShareFileGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareFileGroup shareFileGroup = db.sharefilegroup.Find(id);
            if (shareFileGroup == null)
            {
                return HttpNotFound();
            }
            IEnumerable<vSharedFile> vsf = db.vsharefile.Where(x => x.GroupId == id);
            ViewData["vShareFile"] = vsf;
            ViewData["vShareFileAll"] = GetUnSelectedFile(id);

            IEnumerable<vShareFileGroupUsers> vshgu = db.vshareFileGroupUsers.Where(x => x.GroupId == id);
            ViewData["UnShareFileGroupUsers"] = GetUnShareFileGroupUsers(id);
            ViewData["ShareFileGroupUsersSelected"] = vshgu;
            return View(shareFileGroup);
        }

        // POST: ShareFileGroups/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,LimitSize,UsageSize,SDate,EDate,SummaryAdvice, cdt")] ShareFileGroup shareFileGroup)
        {
            DateTime today = DateTime.Now;
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            if (ModelState.IsValid)
            {
                shareFileGroup.editor = Editor;
                shareFileGroup.udt = today;
                db.Entry(shareFileGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shareFileGroup);
        }
        //http://localhost:52710/ShareFileGroups/AddMediaIntoGroup?fileId=16&GroupId=00c01a09-db49-4072-bcb7-f8f3ac6ede0d&SLId=8969722f-2e1b-45d5-8cd1-f093f1bc4514&record=True
        public ActionResult AddMediaIntoGroup([Bind(Include = "GroupId, SLId, fileId")] ShareFileList sharefilelist)
        {
            if (ModelState.IsValid)
            {
                db.sharefilelist.Add(sharefilelist);
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        //http://localhost:52710/ShareFileGroups/UnSelectFile?fileId=19&GroupId=00c01a09-db49-4072-bcb7-f8f3ac6ede0d&SLId=a0128e4d-0d47-4367-a11e-404dedb2ceee&record=True
        public ActionResult RemoveMediaFromGroup(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareFileList sharefilelist = db.sharefilelist.Find(id);
            if (sharefilelist == null)
                return HttpNotFound();
            db.sharefilelist.Remove(sharefilelist);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult AddUserIntoGroup([Bind(Include = "Id, GroupId,UserId")] ShareFileGroupUsers shareFileGroupUsers)
        {
            if (ModelState.IsValid)
            {
                db.shareFileGroupUsers.Add(shareFileGroupUsers);
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveUserFromGroup(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareFileGroupUsers shareFileGroupUsers = db.shareFileGroupUsers.Find(id);
            if (shareFileGroupUsers == null)
                return HttpNotFound();
            db.shareFileGroupUsers.Remove(shareFileGroupUsers);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        // GET: ShareFileGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShareFileGroup shareFileGroup = db.sharefilegroup.Find(id);
            if (shareFileGroup == null)
            {
                return HttpNotFound();
            }
            return View(shareFileGroup);
        }

        // POST: ShareFileGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ShareFileGroup shareFileGroup = db.sharefilegroup.Find(id);
            db.sharefilegroup.Remove(shareFileGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
