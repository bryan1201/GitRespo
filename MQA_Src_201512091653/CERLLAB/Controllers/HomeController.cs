using System.Web.Mvc;
using System.Text;

using CERLLAB.Controllers.General;


namespace CERLLAB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string UserId = "";
            string UserIdentity = User.Identity.Name;

            UserId = Method.GetLogonUserId(Session, this, UserIdentity);// Constant.LogonUserId;

            StringBuilder vchSet = new StringBuilder();
            
            vchSet.Append(Method.BuildXML(UserId, "UserId"));
            vchSet.Append(Method.BuildXML(UserIdentity, "UserIdentity"));
            ViewBag.Title = UserIdentity;
            ViewBag.Message = vchSet.ToString();

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
