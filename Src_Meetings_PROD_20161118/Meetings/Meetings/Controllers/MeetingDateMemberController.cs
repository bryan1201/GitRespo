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
    public class MeetingDateMemberController : Controller
    {
        IMeetingDateMember imdm = DataAccess.CreateMeetingDateMember();
        // GET: MeetingDateMember
        public ActionResult Index(Guid? id, string Sex, string code)
        {
            code = (string.IsNullOrEmpty(code)) ? code : code.Trim();
            IEnumerable<vMeetingDateMember> vmdm = imdm.GetMeetingDateMembers(id.Value);
            vmdm = (string.IsNullOrEmpty(Sex)) ? vmdm.OrderBy(x => x.UserCode) : vmdm.Where(x => x.Sex == Sex).OrderBy(x => x.UserCode);
            vmdm = (string.IsNullOrEmpty(code)) ? vmdm : vmdm.Where(x => x.UserCode.Contains(code) || x.UserName.Contains(code));
            ViewBag.SearchCode = code;
            return View(vmdm);
        }

        public ActionResult Meeting()
        {
            IMeeting im = DataAccess.CreateMeeting();
            IEnumerable<Meeting> m = im.Get();
            return View(m);
        }

        // GET: MeetingDateMember/Details/5
        public ActionResult Details(Guid? id)
        {
            vMeetingDateMember vmdm = imdm.Get(id.Value);
            return View(vmdm);
        }

        // GET: MeetingDateMember/Create
        public ActionResult Create(Guid? id)
        {
            MeetingDateMember mdm = new MeetingDateMember();
            mdm.uniqueId = Guid.NewGuid();
            mdm.meetingDateId = id.Value;
            return View(mdm);
        }

        // POST: MeetingDateMember/Create
        [HttpPost]
        public ActionResult Create(MeetingDateMember mdm)
        {
            try
            {
                imdm.Insert(mdm);
                mdm = imdm.GetCurrentEdit(mdm.uniqueId);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", mdm.meetingDateId);
                rv.Add("Sex", mdm.GetSex());
                return RedirectToAction("Index", "MeetingDateMember", rv);
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(mdm);
            }
        }

        // GET: MeetingDateMember/Edit/5
        public ActionResult Edit(Guid? id, Guid uniqueId)
        {
            MeetingDateMember mdm = imdm.GetCurrentEdit(uniqueId);
            return View(mdm);
        }

        // POST: MeetingDateMember/Edit/5
        [HttpPost]
        public ActionResult Edit(MeetingDateMember mdm)
        {
            try
            {
                imdm.Update(mdm);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", mdm.meetingDateId);
                rv.Add("Sex", mdm.GetSex());
                return RedirectToAction("Index", "MeetingDateMember", rv);
            }
            catch
            {
                return View();
            }
        }

        // GET: MeetingDateMember/Delete/5
        public ActionResult Delete(Guid? id, string code)
        {
            MeetingDateMember mdm = imdm.GetCurrentEdit(id.Value);
            imdm.Remove(id.Value);
            RouteValueDictionary rv = new RouteValueDictionary();
            rv.Add("id", mdm.meetingDateId);
            rv.Add("Sex", mdm.GetSex());
            rv.Add("code", code);
            return RedirectToAction("Index", "MeetingDateMember", rv);
        }
    }
}
