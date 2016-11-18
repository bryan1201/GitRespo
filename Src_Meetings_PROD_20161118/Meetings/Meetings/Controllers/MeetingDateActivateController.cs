using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meetings.Models;
using Meetings.Controllers.General;
using System.Web.Routing;
namespace Meetings.Controllers
{
    public class MeetingDateActivateController : Controller
    {
        IMeetingDateActivate imda = DataAccess.CreateMeetingDateActivate();
        // GET: MeetingDateActivate
        public ActionResult Index(Guid? id)
        {
            ViewBag.id = id.Value;
            // input meetingDateId
            IEnumerable<vMeetingDateActivate> vmda = imda.Get(id.Value);
            if (vmda == null)
                vmda = new List<vMeetingDateActivate>();
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
            if(!id.HasValue)
                return RedirectToAction("Index","Meeting");
            ViewBag.meetingDateId = id.Value;
            MeetingDateActivate mda = new MeetingDateActivate();
            mda.meetingDateId = id.Value;
            mda.ActivateId = Guid.NewGuid();
            mda.ActivateDate = DateTime.Now;
            mda.ActivateDesc = "[訓練對象]";
            mda.Sex = "";
            string dtToday = DateTime.Now.ToString("yyyy/MM/dd");
            string timeFrom = "19:00";
            string timeTo = "21:30";
            DateTime from = DateTime.Parse(dtToday + " " + timeFrom);
            DateTime to = DateTime.Parse(dtToday + " " + timeTo);
            mda.SDT = from;
            mda.EDT = to;
            return View(mda);
        }

        // POST: MeetingDateActivate/Create
        [HttpPost]
        public ActionResult Create(MeetingDateActivate mda)
        {
            try
            {
                imda.Insert(mda);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", mda.meetingDateId);
                return RedirectToAction("Index", rv);
            }
            catch
            {
                return View(mda);
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
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", mda.meetingDateId);
                return RedirectToAction("Index", rv);
            }
            catch
            {
                return View(mda);
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
