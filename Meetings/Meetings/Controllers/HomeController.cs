using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Meetings.Models;
using Meetings.Controllers.General;

namespace Meetings.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(Guid? id, string UserCode)
        {
            string RetRslt = string.Empty;
            if (id.HasValue == true)
            {
                ViewData["ActivateId"] = id.Value;
            }
            else
            {
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", id);
                return RedirectToAction("Index", "Meeting", rv);
            }

            IMeetingDateMemberLog imdml = DataAccess.CreateMeetingDateMemberLog();

            if (!string.IsNullOrEmpty(UserCode))
            {
                imdml.Login(id.Value, UserCode, out RetRslt);
                ViewBag.ReturnResult = RetRslt;
            }

            IEnumerable<vMeetingDateMemberLog> vmdmls = imdml.Get(id.Value).Where(x=>x.UserCode==UserCode);
            ViewData["vMeetingDateMemberLogs"] = vmdmls;
            ViewBag.ActivateId = id.Value;
            vMeetingDateMemberLog vmdml = vmdmls.FirstOrDefault();
            return View(vmdml);
        }

        public ActionResult Report(Guid? id)
        {
            IMeetingDateMemberLog imdml = DataAccess.CreateMeetingDateMemberLog();
            IEnumerable<vMeetingDateAttendStatus> vmdas = imdml.GetReport(id.Value).OrderBy(x => x.UserCode);
            return View(vmdas);
        }

        public ActionResult DetailList(Guid? id)
        {
            IMeetingDateMemberLog imdml = DataAccess.CreateMeetingDateMemberLog();
            IEnumerable<vMeetingDateMemberLog> vmdmls = imdml.Get(id.Value);
            
            return View(vmdmls);
        }

        public ActionResult SetMeeting(Guid? id)
        {
            IMeetingDateActivate imda = DataAccess.CreateMeetingDateActivate();
            IEnumerable<vMeetingDateActivate> b = imda.Get(id.Value);
            return View(b);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}