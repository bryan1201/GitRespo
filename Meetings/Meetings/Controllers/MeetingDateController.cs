using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meetings.Models;
using Meetings.Controllers.General;

namespace Meetings.Controllers
{
    public class MeetingDateController : Controller
    {
        // GET: MeetingDate
        public ActionResult Index(Guid? id)
        {
            IMeetingDate imd = DataAccess.CreateMeetingDate();
            IEnumerable<vMeetingDate> vmd = imd.Get(id.Value);
            return View(vmd);
        }

        // GET: MeetingDate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MeetingDate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeetingDate/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MeetingDate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MeetingDate/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MeetingDate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MeetingDate/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
