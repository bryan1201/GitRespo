using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meetings.Models;
using Meetings.Controllers.General;
using Meetings.Controllers;
using System.Web.Routing;

namespace Meetings.Controllers
{
    public class MeetingDateController : Controller
    {
        IMeetingDate imd = DataAccess.CreateMeetingDate();
        // GET: MeetingDate
        public ActionResult Index(Guid? id)
        {
            IEnumerable<vMeetingDate> vmd = imd.Get(id.Value);
            ViewBag.id = id.Value;
            return View(vmd);
        }

        // GET: MeetingDate/Details/5
        public ActionResult Details(Guid id)
        {
            vMeetingDate vmd = imd.GetCurrent(id);
            return View(vmd);
        }

        // GET: MeetingDate/Create
        public ActionResult Create(Guid id)
        {
            MeetingDate md = new MeetingDate();
            md.meetingId = id;
            md.meetingDateId = Guid.NewGuid();
            md.STDT = DateTime.Now;
            md.ENDDT = DateTime.Now;
            md.CDT = DateTime.Now;
            return View(md);
        }

        // POST: MeetingDate/Create
        [HttpPost]
        public ActionResult Create(MeetingDate md)
        {
            try
            {
                md.CDT = DateTime.Now;
                imd.Insert(md);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", md.meetingId);
                return RedirectToAction("Index", rv);
            }
            catch
            {
                return View(md);
            }
        }

        // GET: MeetingDate/Edit/5
        public ActionResult Edit(Guid id)
        {
            ChruchLifeDBContext _db = new ChruchLifeDBContext();
            MeetingDate md = _db.MeetingDates.Find(id);
            return View(md);
        }

        // POST: MeetingDate/Edit/5
        [HttpPost]
        public ActionResult Edit(MeetingDate md)
        {
            try
            {
                imd.Update(md);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", md.meetingId);
                return RedirectToAction("Index", rv);
            }
            catch
            {
                return View(md);
            }
        }

        // GET: MeetingDate/Delete/5
        public ActionResult Delete(Guid id)
        {
            ChruchLifeDBContext _db = new ChruchLifeDBContext();
            MeetingDate md = _db.MeetingDates.Find(id);
            return View(md);
        }

        // POST: MeetingDate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(MeetingDate md)
        {
            try
            {
                imd.Remove(md.meetingDateId);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", md.meetingId);
                return RedirectToAction("Index", rv);
            }
            catch
            {
                return View(md);
            }
        }
    }
}
