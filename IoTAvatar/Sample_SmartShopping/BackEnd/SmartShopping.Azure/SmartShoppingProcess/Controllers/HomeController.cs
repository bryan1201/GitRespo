using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartShoppingDemoProcess.Models;
using System.Data;
using System.Data.Entity;

namespace SmartShoppingDemoProcess.Controllers
{
    public class HomeController : Controller
    {
        private SmartShoppingDemoProcessContext db = new SmartShoppingDemoProcessContext();

        private static int curSelectedIndex = 0;
        private static List<SelectListItem> beaconList = new List<SelectListItem>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCurrentFilter(int id)
        {
            ViewBag.FilterIn = ProcessAdvertisement.GetFilter(beaconList[id].Text, true);
            ViewBag.FilterOut = ProcessAdvertisement.GetFilter(beaconList[id].Text, false);
            beaconList[curSelectedIndex].Selected = false;

            curSelectedIndex = id;
            beaconList[curSelectedIndex].Selected = true;

            return RedirectToAction("SetFilter");
        }

        public ActionResult SetFilter( string BeaconId,
                                       string FilterIn,
                                       string FilterOut
                                     )
        {
            if (FilterIn == null)
            {
                // Add BeaconId list
                if (beaconList.Count == 0)
                {
                    foreach (var beacon in db.Beacons.OrderByDescending(data => data.Id))
                    {
                        beaconList.Add(new SelectListItem { Text = beacon.BeaconId, Value = beacon.BeaconId });
                    }
                }

                ViewBag.BeaconId = beaconList;

                ViewBag.FilterIn = ProcessAdvertisement.GetFilter(beaconList[curSelectedIndex].Text, true);
                ViewBag.FilterOut = ProcessAdvertisement.GetFilter(beaconList[curSelectedIndex].Text, false);

                return View();
            }

            int valueIn = 0, valueOut = 0;

            if (!int.TryParse(FilterIn, out valueIn))
                return View();

            if (!int.TryParse(FilterOut, out valueOut))
                return View();
            ProcessAdvertisement.SetFilter(BeaconId, valueIn, valueOut);


            Beacon result = db.Beacons.Where(data => data.BeaconId == BeaconId).FirstOrDefault();

            result.InFilter = valueIn;
            result.OutFilter = valueOut;

            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();

            //return View();
            return RedirectToAction("Index");
        }
    }
}