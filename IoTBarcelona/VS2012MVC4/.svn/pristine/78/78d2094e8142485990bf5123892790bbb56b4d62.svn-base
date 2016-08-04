using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Controllers.General;
namespace VS2012MVC4.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Manage()
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            if (Editor.Equals(""))
                return RedirectToAction("Login", "Account");
            else
                return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
