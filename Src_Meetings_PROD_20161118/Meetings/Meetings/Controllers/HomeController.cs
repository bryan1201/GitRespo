using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Meetings.Models;
using Meetings.Controllers.General;
using System.Web.Security;

namespace Meetings.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public void Authentication(string User)
        {
            List<string> roles = new List<string>() { User };

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                          User,//使用者ID
                          DateTime.Now,//核發日期
                          DateTime.Now.AddMinutes(30),//到期日期 30分鐘 
                          true,//永續性
                          string.Join(",", roles.ToArray()),//使用者定義的資料
                          FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
        }

        [AllowAnonymous]
        public ActionResult Login(string User)
        {
            if (!string.IsNullOrEmpty(User))
            {
                Authentication(User);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("User", User);
                return RedirectToAction("Index", "Meeting");
            }
            return View();
        }

        [AllowAnonymous]
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

        public ActionResult Report(Guid? id, string Sex, bool? Attend)
        {
            IMeetingDateMemberLog imdml = DataAccess.CreateMeetingDateMemberLog();
            IEnumerable<vMeetingDateAttendStatus> vmdas = new List<vMeetingDateAttendStatus>();
            if (string.IsNullOrEmpty(Sex))
                vmdas = imdml.GetReport(id.Value).OrderBy(x => x.UserCode);
            else
            {
                ViewBag.Sex = Sex;
                if (Constant.SexArray.Contains(Sex))
                    vmdas = imdml.GetReport(id.Value).Where(x => x.Sex == Sex).OrderBy(x => x.UserCode);
                else
                    vmdas = imdml.GetReport(id.Value).OrderBy(x => x.UserCode);
            }
            if (Attend.HasValue)
                vmdas = vmdas.Where(x => x.Attend == Attend.Value);

            return View(vmdas);
        }

        public ActionResult DetailList(Guid? id)
        {
            IMeetingDateMemberLog imdml = DataAccess.CreateMeetingDateMemberLog();
            IEnumerable<vMeetingDateMemberLog> vmdmls = imdml.Get(id.Value).OrderBy(x=>x.UserCode);
            
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