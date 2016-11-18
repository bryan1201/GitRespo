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
    public class UserProfileController : Controller
    {
        IUserProfile iup = DataAccess.CreateUserProfile();
        // GET: UserProfile
        public ActionResult Index(string condition)
        {
            IEnumerable<UserProfile> up = new List<UserProfile>();
            if (string.IsNullOrEmpty(condition))
            {
                up = new List<UserProfile>();  //iup.Get();
            }
            else
            {
                if (condition.Trim() == "All")
                    up = iup.Get();
                else
                    up = iup.Search(condition);
            }

            return View(up);
        }

        // GET: UserProfile/Details/5
        public ActionResult Details(string id)
        {
            UserProfile up = iup.GetProfile(id);
            return View(up);
        }

        // GET: UserProfile/Create
        public ActionResult Create()
        {
            UserProfile up = new UserProfile();
            up.uniqueId = Guid.NewGuid();
            return View(up);
        }

        // POST: UserProfile/Create
        [HttpPost]
        public ActionResult Create(UserProfile up)
        {
            try
            {
                up.cdt = DateTime.Now;
                up.udt = DateTime.Now;
                iup.Insert(up);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", up.uniqueId);
                return RedirectToAction("Details", rv);
            }
            catch
            {
                return View(up);
            }
        }

        // GET: UserProfile/Edit/5
        public ActionResult Edit(Guid id)
        {
            UserProfile up = iup.GetProfile(id.ToString());
            return View(up);
        }

        // POST: UserProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(UserProfile up)
        {
            try
            {
                iup.Update(up);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", up.uniqueId);
                return RedirectToAction("Details", rv);
            }
            catch
            {
                return View(up);
            }
        }

        // GET: UserProfile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProfile/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                // TODO: Add delete logic here
                iup.Remove(id);
                RouteValueDictionary rv = new RouteValueDictionary();
                rv.Add("id", id);
                return RedirectToAction("Index", "UserProfile", rv);
            }
            catch
            {
                UserProfile up = iup.GetProfile(id.ToString());
                return View(up);
            }
        }
    }
}
