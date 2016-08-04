using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meetings.Models;
using Meetings.Controllers.General;

namespace Meetings.Controllers
{
    public class MeetingDateActivateController : Controller
    {
        IMeetingDateActivate imda = DataAccess.CreateMeetingDateActivate();
        // GET: MeetingDateActivate
        public ActionResult Index(Guid? id)
        {
            // input meetingDateId
            IEnumerable<vMeetingDateActivate> vmda = imda.Get(id.Value);
            return View(vmda);
        }

        // GET: MeetingDateActivate/Details/5
        public ActionResult Details(Guid? id)
        {
            vMeetingDateActivate vmda = imda.GetCurrent(id.Value);
            return View(vmda);
        }

        // GET: MeetingDateActivate/Create
        public ActionResult Create(Guid? id)
        {
            if(id==null)
                return RedirectToAction("Index");
            ViewBag.meetingDateId = id.Value;
            return View();
        }

        // POST: MeetingDateActivate/Create
        [HttpPost]
        public ActionResult Create(MeetingDateActivate mda)
        {
            try
            {
                mda.ActivateId = Guid.NewGuid();
                imda.Insert(mda);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MeetingDateActivate/Edit/5
        public ActionResult Edit(Guid id)
        {
            MeetingDateActivate mda = imda.GetCurrentEdit(id);
            return View(mda);
        }

        // POST: MeetingDateActivate/Edit/5
        [HttpPost]
        public ActionResult Edit(MeetingDateActivate mda)
        {
            try
            {
                imda.Update(mda);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MeetingDateActivate/Delete/5
        public ActionResult Delete(Guid id)
        {
            vMeetingDateActivate vmda = imda.GetCurrent(id);
            return View(vmda);
        }

        // POST: MeetingDateActivate/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                imda.Remove(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
