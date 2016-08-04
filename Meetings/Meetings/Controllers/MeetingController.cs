using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meetings.Models;
using Meetings.Controllers.General;

namespace Meetings.Controllers
{
    public class MeetingController : Controller
    {
        IMeeting im = DataAccess.CreateMeeting();
        // GET: Meeting
        public ActionResult Index()
        {   
            return View(im.Get());
        }

        // GET: Meeting/Details/5
        public ActionResult Details(Guid id)
        {
            Meeting m = im.Get(id);
            return View(m);
        }

        // GET: Meeting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Meeting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Meeting meeting)
        {
            try
            {
               if(ModelState.IsValid)
                {
                    im.Insert(meeting);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
                
            }
        }

        // GET: Meeting/Edit/5
        public ActionResult Edit(Guid id)
        {
            Meeting m = im.Get(id);
            if (m == null)
            {
                return HttpNotFound();
            }
            return View(m);
        }

        // POST: Meeting/Edit/5
        [HttpPost]
        public ActionResult Edit(Meeting meeting)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    im.Update(meeting);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(meeting);
            }
        }

        // GET: Meeting/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                return View(im.Get(id));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Meeting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                im.Remove(id);
            }
            catch
            {
                //
            }

            return RedirectToAction("Index");
        }
    }
}
