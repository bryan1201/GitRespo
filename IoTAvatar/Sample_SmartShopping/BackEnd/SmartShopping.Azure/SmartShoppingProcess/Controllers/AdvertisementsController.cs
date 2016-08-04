using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartShoppingDemoProcess.Models;

namespace SmartShoppingDemoProcess.Controllers
{
    public class AdvertisementsController : Controller
    {
        private SmartShoppingDemoProcessContext db = new SmartShoppingDemoProcessContext();

        // GET: Advertisements
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
