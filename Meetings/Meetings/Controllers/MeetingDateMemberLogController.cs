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
    public class MeetingDateMemberLogController : Controller
    {
        // GET: MeetingDateMemberLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BulkInsertUser(Guid? id, string UserCodes)
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

            if (!string.IsNullOrEmpty(UserCodes))
            {
                imdml.BulkInsertLog(id.Value, UserCodes, out RetRslt);
                ViewBag.ReturnResult = RetRslt;
            }

            IEnumerable<vMeetingDateMemberLog> vmdmls = imdml.Get(id.Value);
            ViewData["vMeetingDateMemberLogs"] = vmdmls;
            ViewBag.ActivateId = id.Value;
            return View(vmdmls);
        }
    }
}